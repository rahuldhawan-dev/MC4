using System.Collections.Generic;
using MMSINC.Data.Linq;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Interface;
using MMSINC.Testing;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Rhino.Mocks;
using WorkOrders.Model;
using WorkOrders.Presenters.ContractorCrewAssignments;
using WorkOrders.Views.CrewAssignments;

namespace _271ObjectTests.Tests.Unit.Presenters.ContractorAssignments
{
    [TestClass]
    public class ContractorCrewAssignmentsListPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private ICrewAssignmentsListView _view;
        private ICrewAssignmentsRPCListView _rpcView;
        private IRepository<Markout> _markoutRepository;
        private IRepository<CrewAssignment> _repository;
        private TestContractorCrewAssignmentsListPresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _rpcView)
                .DynamicMock(out _repository)
                .DynamicMock(out _markoutRepository);

            _target =
                new TestContractorCrewAssignmentListPresenterBuilder(_view)
                    .WithRepository(_repository)
                    .WithMarkoutRepository(_markoutRepository);
        }

        #endregion

        #region Property Tests

        // ContractorCrewAssignmentsListView
        [TestMethod]
        public void TestContractorCrewAssignmentsListViewReturnsViewCastToICrewAssignmentsListView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_view, _target.CrewAssignmentsListView);
            Assert.IsInstanceOfType(_target.CrewAssignmentsListView, typeof(ICrewAssignmentsListView));
        }

        // ContractorCrewAssignmentsRPCListView
        [TestMethod]
        public void TestContractorCrewAssignmentsRPCListViewCastToICrewAssigmnetsRPCListView()
        {
            _mocks.ReplayAll();

            _target = new TestContractorCrewAssignmentListPresenterBuilder(_rpcView);

            Assert.AreSame(_rpcView, _target.CrewAssignmentsRPCListView);
            Assert.IsInstanceOfType(_target.CrewAssignmentsRPCListView, typeof(ICrewAssignmentsRPCListView));
        }

        // MarkoutRepository
        [TestMethod]
        public void TestContractorCrewAssignmentRepositoryReturnsInstanceFromIocContainer()
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
            _target = new TestContractorCrewAssignmentListPresenterBuilder(_rpcView);

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
                new TestContractorCrewAssignmentListPresenterBuilder(_rpcView).
                    WithRepository(_repository);
            var crewAssignmentIDs = new[] { 1, 2 };
            var eventArgs = new CrewAssignmentDeleteEventArgs(crewAssignmentIDs);

            using (_mocks.Record())
            {
                foreach (var x in crewAssignmentIDs)
                {
                    var assignment = new CrewAssignment
                    {
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
        public void TestViewPrioritizeCommandPrioritizesEntities()
        {
            _target =
                new TestContractorCrewAssignmentListPresenterBuilder(_rpcView).
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

            var eventArgs = new CrewAssignmentPrioritizeEventArgs(crewAssignments);

            using (_mocks.Record())
            {
                foreach (var x in crewAssignments)
                {
                    var assignment = new CrewAssignment
                    {
                        CrewAssignmentID = x.CrewAssignmentID,
                        Priority = x.Priority
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

    internal class TestContractorCrewAssignmentListPresenterBuilder : TestDataBuilder<TestContractorCrewAssignmentsListPresenter>
    {
        #region Private Members

        private readonly IListView<CrewAssignment> _view;
        private IRepository<Markout> _markoutRepository;
        private IRepository<CrewAssignment> _repository;

        #endregion

        #region Constructors

        internal TestContractorCrewAssignmentListPresenterBuilder(IListView<CrewAssignment> view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestContractorCrewAssignmentsListPresenter Build()
        {
            var obj = new TestContractorCrewAssignmentsListPresenter(_view);
            if (_repository != null)
                obj.Repository = _repository;
            if (_markoutRepository != null)
                obj.SetMarkoutRepository(_markoutRepository);
            return obj;
        }

        public TestContractorCrewAssignmentListPresenterBuilder WithRepository(IRepository<CrewAssignment> repository)
        {
            _repository = repository;
            return this;
        }

        public TestContractorCrewAssignmentListPresenterBuilder WithMarkoutRepository(IRepository<Markout> repository)
        {
            _markoutRepository = repository;
            return this;
        }

        #endregion
    }

    internal class TestContractorCrewAssignmentsListPresenter : ContractorCrewAssignmentsListPresenter
    {
        #region Constructors

        public TestContractorCrewAssignmentsListPresenter(IListView<CrewAssignment> view)
            : base(view)
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
