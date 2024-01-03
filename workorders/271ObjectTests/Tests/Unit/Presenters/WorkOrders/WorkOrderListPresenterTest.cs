using System;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using Moq;
using Rhino.Mocks;
using StructureMap;
using WorkOrders.Model;
using WorkOrders.Presenters.WorkOrders;
using WorkOrders.Views.WorkOrders;

namespace _271ObjectTests.Tests.Unit.Presenters.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderListPresenterrTest.
    /// </summary>
    [TestClass]
    public class WorkOrderListPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IWorkOrderListView _view;
        private IWorkOrderSchedulingListView _schedulingView;
        private IWorkOrderPrePlanningListView _prePlanningView;
        private IRepository<WorkOrder> _repository;
        private IRepository<CrewAssignment> _crewAssignmentRepository;
        private IRepository<Crew> _crewRepository;
        private TestWorkOrderListPresenter _target;
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
                .DynamicMock(out _schedulingView)
                .DynamicMock(out _prePlanningView)
                .DynamicMock(out _repository)
                .DynamicMock(out _crewAssignmentRepository)
                .DynamicMock(out _crewRepository);

            _container.Inject(_crewAssignmentRepository);
            _container.Inject(_crewRepository);

            _iGeneralWorkOrderRepositoryMock = new Mock<IGeneralWorkOrderRepository>();
            _container.Inject(_iGeneralWorkOrderRepositoryMock.Object);

            _target = new TestWorkOrderListPresenterBuilder(_view)
                .WithRepository(_repository);
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
            var target = new WorkOrderListPresenter(_view);

            _mocks.ReplayAll();

            Assert.AreSame(_view, target.View);
        }

        [TestMethod]
        public void TestListViewReturnsViewCastToIWorkOrderView()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_view, _target.ListView);
            Assert.IsInstanceOfType(_target.ListView, typeof(IWorkOrderView));
        }

        [TestMethod]
        public void TestOnViewLoadedWiresUpEventHandlers()
        {
            using (_mocks.Record())
            {
                _view.SelectedIndexChanged += null;
                LastCall.IgnoreArguments();

                _view.DataSourceCreating += null;
                LastCall.IgnoreArguments();

                _view.CreateClicked += null;
                LastCall.IgnoreArguments();

                _repository.EntityInserted += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewLoaded();
            }
        }

        [TestMethod]
        public void TestCrewAssignmentRepositoryReturnsInstanceFromIocContainer()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_crewAssignmentRepository,
                _target.CrewAssignmentRepository);
        }

        [TestMethod]
        public void TestCrewRepositoryReturnsInstanceFromIocContainer()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_crewRepository, _target.CrewRepository);
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestOnViewInitWiresUpEventHandlersIfViewIsScheduling()
        {
            var phase = WorkOrderPhase.Scheduling;

            using (_mocks.Record())
            {
                _view = (phase == WorkOrderPhase.Scheduling)
                    ? (IWorkOrderListView)_schedulingView : _prePlanningView;
                _target = new TestWorkOrderListPresenterBuilder(_view)
                   .WithRepository(_repository);

                SetupResult.For(_view.Phase).Return(phase);
                _schedulingView.AssignClicked += null;
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.OnViewInit();
            }

            _mocks.VerifyAll();
            _mocks.BackToRecordAll();

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnViewInitDoesNotWireUpAssignClickedEventHandlerIfViewIsNotSchedulingOrPrePlanning()
        {
            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                if (phase == WorkOrderPhase.Scheduling || phase == WorkOrderPhase.PrePlanning)
                    continue;

                var views = new IWorkOrderListView[] {
                    _schedulingView, _prePlanningView
                };

                foreach (var view in views)
                {
                    _view = view;
                    _target = new TestWorkOrderListPresenterBuilder(_view)
                        .WithRepository(_repository);

                    using (_mocks.Record())
                    {
                        SetupResult.For(_view.Phase).Return(phase);
                        if (_view == _schedulingView)
                        {
                            DoNotExpect.Call(
                                () =>
                                ((IWorkOrderSchedulingListView)_view).
                                    AssignClicked += null);
                        }
                        else
                        {
                            DoNotExpect.Call(
                                () =>
                                ((IWorkOrderPrePlanningListView)_view).
                                    AssignClicked += null);
                            DoNotExpect.Call(
                                () => 
                                ((IWorkOrderPrePlanningListView)_view).
                                    ContractorAssignClicked += null);
                        }
                        LastCall.IgnoreArguments();
                    }

                    using (_mocks.Playback())
                    {
                        _target.OnViewInit();
                    }

                    _mocks.VerifyAll();
                    _mocks.BackToRecordAll();
                }
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestOnSelectedIndexChangedDisplaysCurrentEntity()
        {
            var keyVal = "key";

            using (_mocks.Record())
            {
                SetupResult.For(_view.SelectedDataKey).Return(keyVal);
                _repository.SetSelectedDataKey(keyVal);
                _view.SetViewControlsVisible(true);
            }

            using (_mocks.Playback())
            {
                _target.OnSelectedIndexChanged();
            }
        }

        [TestMethod]
        public void TestListViewAssignClickedEventCreatesCrewAssignmentsFromEventArgs()
        {
            var crewID = 1;
            var crew1 = new Crew {
                CrewID = crewID
            };
            var date = DateTime.Today;
            var workOrderIDs = new[] {
                1, 2, 3
            };
            var operatingCenter = new OperatingCenter {
                SAPEnabled = false
            };
            WorkOrder workOrder1 = new WorkOrder {
                    WorkOrderID = workOrderIDs[0],
                    OperatingCenter = operatingCenter
                },
                workOrder2 = new WorkOrder {
                    WorkOrderID = workOrderIDs[1],
                    OperatingCenter = operatingCenter
                },
                workOrder3 = new WorkOrder {
                    WorkOrderID = workOrderIDs[2],
                    OperatingCenter = operatingCenter
                };
            var args = new WorkOrderAssignmentEventArgs(crewID, date,
                workOrderIDs);
            var mcWorkOrder1= new MapCall.Common.Model.Entities.WorkOrder { Id = workOrder1.WorkOrderID, OperatingCenter = new MapCall.Common.Model.Entities.OperatingCenter() { SAPEnabled = false } };
            var mcWorkOrder2 = new MapCall.Common.Model.Entities.WorkOrder { Id = workOrder2.WorkOrderID, OperatingCenter = new MapCall.Common.Model.Entities.OperatingCenter() { SAPEnabled = false } };
            var mcWorkOrder3 = new MapCall.Common.Model.Entities.WorkOrder { Id = workOrder3.WorkOrderID, OperatingCenter = new MapCall.Common.Model.Entities.OperatingCenter() { SAPEnabled = false } };

            using (_mocks.Record())
            {
                SetupResult.For(_crewRepository.Get(crewID)).Return(crew1);
                SetupResult.For(_repository.Get(workOrderIDs[0])).Return(workOrder1);
                SetupResult.For(_repository.Get(workOrderIDs[1])).Return(workOrder2);
                SetupResult.For(_repository.Get(workOrderIDs[2])).Return(workOrder3);
                _iGeneralWorkOrderRepositoryMock.Setup(x => x.Find(workOrderIDs[0])).Returns(mcWorkOrder1);
                _iGeneralWorkOrderRepositoryMock.Setup(x => x.Find(workOrderIDs[1])).Returns(mcWorkOrder2);
                _iGeneralWorkOrderRepositoryMock.Setup(x => x.Find(workOrderIDs[2])).Returns(mcWorkOrder3);
                        
                _crewRepository.UpdateCurrentEntity(crew1);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ListView_AssignClicked", new object[] {
                    null, args
                });
            }

            Assert.AreSame(crew1.CrewAssignments[0].WorkOrder, workOrder1);
            Assert.AreSame(crew1.CrewAssignments[1].WorkOrder, workOrder2);
            Assert.AreSame(crew1.CrewAssignments[2].WorkOrder, workOrder3);
            foreach (var assignment in crew1.CrewAssignments)
            {
                Assert.AreEqual(date, assignment.AssignedFor);
            }
        }

        #endregion
    }

    internal class TestWorkOrderListPresenterBuilder : TestDataBuilder<TestWorkOrderListPresenter>
    {
        #region Private Members

        private readonly IListView<WorkOrder> _view;
        private IRepository<WorkOrder> _repository;

        #endregion

        #region Constructors

        public TestWorkOrderListPresenterBuilder(IListView<WorkOrder> view)
        {
            _view = view;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrderListPresenter Build()
        {
            var obj = new TestWorkOrderListPresenter(_view);
            if (_repository != null)
                obj.Repository = _repository;
            return obj;
        }

        public TestWorkOrderListPresenterBuilder WithRepository(IRepository<WorkOrder> repository)
        {
            _repository = repository;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderListPresenter : WorkOrderListPresenter
    {
        #region Constructors

        public TestWorkOrderListPresenter(IListView<WorkOrder> view) : base(view)
        {
        }

        #endregion
    }
}
