using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.Linq;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.StructureMap;
using Rhino.Mocks;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;
using WorkOrders.Presenters.CrewAssignments;
using WorkOrders.Views.CrewAssignments;

namespace _271ObjectTests.Tests.Unit.Presenters.CrewAssignments
{
    /// <summary>
    /// Summary description for CrewAssignmentsReadOnlyPresenterTest.
    /// </summary>
    [TestClass]
    public class CrewAssignmentsReadOnlyPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private ICrewAssignmentsReadOnly _view;
        private IRepository<CrewAssignment> _repository;
        private IRepository<Crew> _crewRepository;
        private CrewAssignmentsReadOnlyPresenter _target;
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
                .DynamicMock(out _repository)
                .DynamicMock(out _crewRepository);

            _target = new TestCrewAssignmentsReadOnlyPresenterBuilder(_view)
                .WithRepository(_repository)
                .WithCrewRepository(_crewRepository);

            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestConstructorSetsView()
        {
            _target = new CrewAssignmentsReadOnlyPresenter(_view, null);

            Assert.AreSame(_view, _target.View);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryPropertyGetsRepositoryFromIocContainer()
        {
            _target = new TestCrewAssignmentsReadOnlyPresenterBuilder(_view);
            _container.Inject(_repository);

            Assert.AreSame(_repository, _target.Repository);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestRepositoryPropertyReturnsMockedRepositoryInstance()
        {
            Assert.AreSame(_repository, _target.Repository);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestCrewRepositoryPropertyGetsCrewRepositoryFromIocContainer()
        {
            _target = new TestCrewAssignmentsReadOnlyPresenterBuilder(_view);
            _container.Inject(_crewRepository);

            Assert.AreSame(_crewRepository, _target.CrewRepository);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestCrewRepositoryPropertyReturnsMockedCrewRepositoryInstance()
        {
            Assert.AreSame(_crewRepository, _target.CrewRepository);

            _mocks.ReplayAll();
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestOnViewLoadedWiresUpRowCommandHandler()
        {
            using (_mocks.Record())
            {
                _view.AssignmentCommand += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestOnViewLoadedDataBindsViewWithCrewIfCrewIDValueIsNotNull()
        {
            var crewID = 1;
            var crew = new Crew {
                CrewID = crewID
            };

            using (_mocks.Record())
            {
                SetupResult.For(_view.CrewID).Return(crewID);
                SetupResult.For(_crewRepository.Get(crewID)).Return(crew);
                _view.DataBindCrew(crew);
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestOnViewLoadedDoesNotDataBindViewIfCrewIDValueIsNull()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_view.CrewID).Return(null);

                DoNotExpect.Call(() => _crewRepository.Get(null));
                LastCall.IgnoreArguments();

                DoNotExpect.Call(() => _view.DataBindCrew(null));
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestOnViewInitializedDoesNothing()
        {
            // setup full mocks for this
            _mocks
                .CreateMock(out _view)
                .CreateMock(out _repository)
                .CreateMock(out _crewRepository);

            _target = new TestCrewAssignmentsReadOnlyPresenterBuilder(_view)
                .WithRepository(_repository)
                .WithCrewRepository(_crewRepository);

            using (_mocks.Record())
            {
                // EXPECT NOTHING!!!!
            }

            using (_mocks.Playback())
            {
                _target.OnViewInitialized();
            }
        }

        [TestMethod]
        public void TestRedirectMethodFormatsStringAndRedirectsView()
        {
            var format = "some url format {0}";
            var orderID = 1;
            var expected = String.Format(format, orderID);

            using (_mocks.Record())
            {
                _view.Redirect(expected);
            }

            using (_mocks.Playback())
            {
                _target.InvokeInstanceMethod("Redirect", format, orderID);
            }
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestViewAssignmentCommandStartsAssignmentWithSpecifiedIDAndRedirectsWhenCommandIsStart()
        {
            var crewAssignmentID = 12;
            var workOrderID = 1;
            var command = CrewAssignmentStartEndEventArgs.Commands.Start;
            var date = DateTime.Now;
            var assignment = new CrewAssignment {
                CrewAssignmentID = crewAssignmentID,
                WorkOrderID = workOrderID
            };
            var args = new CrewAssignmentStartEndEventArgs(crewAssignmentID,
                command, date);

            using (_mocks.Record())
            {
                SetupResult.For(_repository.Get(crewAssignmentID)).Return(
                    assignment);
                _repository.UpdateCurrentEntity(assignment);
                _view.DataBind();

                DoNotExpect.Call(() => _view.Redirect(null));
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "View_AssignmentCommand",
                    new object[] {
                        null, args
                    });

                Assert.AreEqual(date, assignment.DateStarted);
            }
        }

        [TestMethod]
        public void TestViewAssignmentCommandEndsAssignmentWithSpecifiedIDWhenCommandIsEnd()
        {
            var crewAssignmentID = 12;
            var workOrderID = 1;
            var command = CrewAssignmentStartEndEventArgs.Commands.End;
            var dateStarted = DateTime.Now.AddDays(-1);
            var dateEnded = dateStarted.AddDays(1);
            var employeesOnJob = 20;
            var assignment = new CrewAssignment {
                DateStarted = dateStarted,
                CrewAssignmentID = crewAssignmentID,
                WorkOrderID = workOrderID
            };
            var args = new CrewAssignmentStartEndEventArgs(crewAssignmentID,
                command, dateEnded, employeesOnJob);

            using (_mocks.Record())
            {
                SetupResult.For(_repository.Get(crewAssignmentID)).Return(
                    assignment);
                _repository.UpdateCurrentEntity(assignment);
                _view.Redirect(
                    String.Format(
                        CrewAssignmentsReadOnlyPresenter.RedirectUrls.
                            FINALIZATION, workOrderID));
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "View_AssignmentCommand",
                    new object[] {
                        null, args
                    });

                Assert.AreEqual(dateEnded, assignment.DateEnded);
            }
        }

        #endregion
    }

    internal class TestCrewAssignmentsReadOnlyPresenterBuilder : TestDataBuilder<TestCrewAssignmentsReadOnlyPresenter>
    {
        #region Properties

        private IRepository<CrewAssignment> _repository;
        private ICrewAssignmentsReadOnly _view;
        private IRepository<Crew> _crewRepository;

        #endregion

        #region Constructors

        internal TestCrewAssignmentsReadOnlyPresenterBuilder(ICrewAssignmentsReadOnly view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestCrewAssignmentsReadOnlyPresenter Build()
        {
            var obj = new TestCrewAssignmentsReadOnlyPresenter(_view, null);
            if (_repository != null)
                obj.Repository = _repository;
            if (_crewRepository != null)
                obj.CrewRepository = _crewRepository;
            return obj;
        }

        public TestCrewAssignmentsReadOnlyPresenterBuilder WithRepository(IRepository<CrewAssignment> repository)
        {
            _repository = repository;
            return this;
        }

        public TestCrewAssignmentsReadOnlyPresenterBuilder WithCrewRepository(IRepository<Crew> repository)
        {
            _crewRepository = repository;
            return this;
        }

        #endregion
    }

    internal class TestCrewAssignmentsReadOnlyPresenter : CrewAssignmentsReadOnlyPresenter
    {
        #region Constructors

        public TestCrewAssignmentsReadOnlyPresenter(ICrewAssignmentsReadOnly view, IRepository<CrewAssignment> repository) : base(view, repository)
        {
        }

        #endregion
    }
}
