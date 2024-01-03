using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Exceptions;
using MMSINC.Utilities.StructureMap;
using Moq;
using Rhino.Mocks;
using StructureMap;
using WorkOrders.Model;
using WorkOrders.Presenters.CrewAssignments;
using WorkOrders.Views.CrewAssignments;

namespace _271ObjectTests.Tests.Unit.Presenters.CrewAssignments
{
    /// <summary>
    /// Summary description for CrewAssignmentListPresenterTest.
    /// </summary>
    [TestClass]
    public class CrewAssignmentsListPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private ICrewAssignmentsListView _view;
        private ICrewAssignmentsRPCListView _rpcView;
        private IRepository<Markout> _markoutRepository;
        private IRepository<CrewAssignment> _repository;
        private TestCrewAssignmentsListPresenter _target;
        private Mock<IGeneralWorkOrderRepository> _iGeneralWorkOrderRepositoryMock;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _container = new Container();
            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _rpcView)
                .DynamicMock(out _repository)
                .DynamicMock(out _markoutRepository);

            _target =
                new TestCrewAssignmentListPresenterBuilder(_view)
                    .WithRepository(_repository)
                    .WithMarkoutRepository(_markoutRepository);

            _iGeneralWorkOrderRepositoryMock = new Mock<IGeneralWorkOrderRepository>();
            _container.Inject(_iGeneralWorkOrderRepositoryMock.Object);
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        #endregion

        #region Property Tests

        // CrewAssignmentsListView
        [TestMethod]
        public void TestCrewAssignmentsListViewReturnsViewCastToICrewAssignmentsListView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_view, _target.CrewAssignmentsListView);
            Assert.IsInstanceOfType(_target.CrewAssignmentsListView, typeof(ICrewAssignmentsListView));
        }

        // CrewAssignmentsRPCListView
        [TestMethod]
        public void TestCrewAssignmentsRPCListViewCastToICrewAssigmnetsRPCListView()
        {
            _mocks.ReplayAll();

            _target = new TestCrewAssignmentListPresenterBuilder(_rpcView);

            Assert.AreSame(_rpcView, _target.CrewAssignmentsRPCListView);
            Assert.IsInstanceOfType(_target.CrewAssignmentsRPCListView, typeof(ICrewAssignmentsRPCListView));
        }

        // MarkoutRepository
        [TestMethod]
        public void TestCrewAssignmentRepositoryReturnsInstanceFromIocContainer()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_markoutRepository,
                _target.MarkoutRepository);
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestOnViewLoadedWiresUpAssignmentCommandIfNotRPCView()
        {
            using (_mocks.Record())
            {
                _view.AssignmentCommand += null;
                LastCall.IgnoreArguments();
                DoNotExpect.Call(() => _rpcView.DeleteCommand += null);
                LastCall.IgnoreArguments();
                DoNotExpect.Call(() => _rpcView.PrioritizeCommand += null);
                LastCall.IgnoreArguments();
            }
            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestOnViewLoadedWiresUpDeleteAndPrioritizeRPCCommandsIfRPCView()
        {
            _target = new TestCrewAssignmentListPresenterBuilder(_rpcView);

            using (_mocks.Record())
            {
                DoNotExpect.Call(() => _view.AssignmentCommand += null);
                LastCall.IgnoreArguments();
                _rpcView.DeleteCommand += null;
                LastCall.IgnoreArguments();
                _rpcView.PrioritizeCommand += null;
                LastCall.IgnoreArguments();
            }
            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestDeleteCommandDeletesEventArgsEntities()
        {
            _target =
                new TestCrewAssignmentListPresenterBuilder(_rpcView).
                    WithRepository(_repository);
            var crewAssignmentIDs = new[] {1,2};
            var eventArgs = new CrewAssignmentDeleteEventArgs(crewAssignmentIDs);

            using(_mocks.Record())
            {
                foreach(var x in crewAssignmentIDs)
                {
                    var assignment = new CrewAssignment {
                        CrewAssignmentID = x
                    };
                    SetupResult.For(_repository.Get(x)).Return(assignment);
                    _repository.DeleteEntity(assignment);
                }
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "View_DeleteCommand", new[] {
                    new object(), eventArgs
                });
            }
        }

        [TestMethod]
        public void TestAssignmentCommandThrowsExceptionWhenEndDateIsBeforeStartDate()
        {
            var start = new DateTime(2019, 4, 18, 9, 56, 0);
            var end = start.AddHours(-1);
            var assignment = new CrewAssignment { CrewAssignmentID = 1, DateStarted = start };
            var args =
                new CrewAssignmentStartEndEventArgs(assignment.CrewAssignmentID,
                    CrewAssignmentStartEndEventArgs.Commands.End, end);
            using (_mocks.Record())
            {
                SetupResult.For(_repository.Get(assignment.CrewAssignmentID)).Return(assignment);
            }
            using (_mocks.Playback())
            {
                MyAssert.ThrowsWithMessage<DomainLogicException>(() =>
                    InvokeEventByName(_target, "View_AssignmentCommand",
                        new object(), args),
                    $"Date Ended 4/18/2019 8:56:00 AM is before Date Started 4/18/2019 9:56:00 AM, the current time is {DateTime.Now}"
                    );
            }
        }

        [TestMethod]
        public void TestViewPrioritizeCommandPrioritizesEntities()
        {
            //woRepo.Setup(x => x.Find())
            _target =
                new TestCrewAssignmentListPresenterBuilder(_rpcView).
                    WithRepository(_repository);
            var crewAssignments = new List<CrewAssignmentPriorities> {
                new CrewAssignmentPriorities {
                    CrewAssignmentID = 1,
                    Priority = 2
                },
                new CrewAssignmentPriorities {
                    CrewAssignmentID = 3,
                    Priority = 1
                }
            };
            var workOrder = new WorkOrder {  OperatingCenter = new OperatingCenter {  SAPEnabled = false} };
            _iGeneralWorkOrderRepositoryMock.Setup(x => x.Find(It.IsAny<int>())).Returns(
                new MapCall.Common.Model.Entities.WorkOrder {
                    OperatingCenter = new MapCall.Common.Model.Entities.OperatingCenter { SAPEnabled = false } });

            var eventArgs = new CrewAssignmentPrioritizeEventArgs(crewAssignments);

            using (_mocks.Record())
            {
                foreach (var x in crewAssignments)
                {
                    var assignment = new CrewAssignment {
                        CrewAssignmentID = x.CrewAssignmentID,
                        Priority = x.Priority,
                        WorkOrder = workOrder
                    };
                    SetupResult.For(_repository.Get(x.CrewAssignmentID)).Return(assignment);
                    assignment.Priority = x.Priority;
                    _repository.UpdateCurrentEntity(assignment);
                    
                }
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "View_PrioritizeCommand", new[] {
                    new object(), eventArgs
                });
            }
        }

        #endregion
    }

    internal class TestCrewAssignmentListPresenterBuilder : TestDataBuilder<TestCrewAssignmentsListPresenter>
    {
        #region Private Members

        private readonly IListView<CrewAssignment> _view;
        private IRepository<Markout> _markoutRepository;
        private IRepository<CrewAssignment> _repository;

        #endregion

        #region Constructors

        internal TestCrewAssignmentListPresenterBuilder(IListView<CrewAssignment> view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestCrewAssignmentsListPresenter Build()
        {
            var obj = new TestCrewAssignmentsListPresenter(_view);
            if (_repository != null)
                obj.Repository = _repository;
            if (_markoutRepository != null)
                obj.SetMarkoutRepository(_markoutRepository);
            return obj;
        }

        public TestCrewAssignmentListPresenterBuilder WithRepository(IRepository<CrewAssignment> repository)
        {
            _repository = repository;
            return this;
        }

        public TestCrewAssignmentListPresenterBuilder WithMarkoutRepository(IRepository<Markout> repository)
        {
            _markoutRepository = repository;
            return this;
        }

        #endregion
    }

    internal class TestCrewAssignmentsListPresenter : CrewAssignmentsListPresenter
    {
        #region Constructors

        public TestCrewAssignmentsListPresenter(IListView<CrewAssignment> view) : base(view)
        {
        }

        #endregion

        #region Methods
        
        public void SetMarkoutRepository(IRepository<Markout> repository)
        {
            _markoutRepository = repository;
        }

        #endregion
    }
}
