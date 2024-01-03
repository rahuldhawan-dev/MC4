using System;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LINQTo271.Controls.WorkOrders;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.Practices.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using Rhino.Mocks;
using StructureMap;
using WorkOrders;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using LINQTo271;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    [TestClass]
    public class WorkOrderStockApprovalFormTest : EventFiringTestClass
    {
        #region Private Members

        private ISecurityService _securityService;
        private TestWorkOrderStockApprovalForm _target;
        private IRepository<WorkOrder> _repository;
        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;
        private IPage _iPage;
        private IViewState _iViewState;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _container = new Container();

            _mocks.DynamicMock(out _repository)
                 .DynamicMock(out _detailControl)
                 .DynamicMock(out _dataSource)
                 .DynamicMock(out _iPage)
                 .DynamicMock(out _iViewState)
                 .DynamicMock(out _securityService);

            _target = new TestWorkOrderStockApprovalFormBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource)
                .WithPage(_iPage)
                .WithViewState(_iViewState)
                .WithSecurityService(_securityService);
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
            _target.Dispose();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestSettingWorkOrderIDSetsDataSourceDataSourceFromRepositoryInstance()
        {
            var index = 1;
            var expected = new WorkOrder();
            SetupResult.For(_repository.Get(index)).Return(expected);
            _container.Inject(_repository);

            using (_mocks.Record())
            {
                _dataSource.DataSource = expected;
            }

            using (_mocks.Playback())
            {
                _target.WorkOrderID = index;
            }
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPageLoadChangesModeOfDetailControlToCurrentMvpMode()
        {
            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                using (_mocks.Record())
                {
                    SetupResult.For(
                        _iViewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.CURRENT_MVP_MODE)).Return(mode);

                    _detailControl.ChangeMvpMode(mode);
                }

                using (_mocks.Playback())
                {
                    InvokeEventByName(_target, "Page_Load");
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestObjectDataSourceUpdatedCommandFiresUpdatingEvent()
        {
            _mocks.ReplayAll();

            _target =
                new TestWorkOrderStockApprovalFormBuilder()
                    .WithUpdateHandler((sender, e) => _called = true);

            InvokeEventByName(_target, "ods_Updated");

            Assert.IsTrue(_called);
        }

        [TestMethod]
        public void TestFormViewItemUpdatingSetsApprovalDateApprovedByAndBooleanValues()
        {
            var expectedCreatorID = 12345678;
            int workOrderID = 666;
            SetupResult.For(_securityService.GetEmployeeID()).Return(
                expectedCreatorID);
            SetupResult.For(_iViewState.GetValue("WorkOrderID"))
                       .Return(workOrderID);
            SetupResult.For(_repository.Get(workOrderID)).Return(new WorkOrder {
                WorkOrderID = workOrderID,
                StreetOpeningPermitRequired = true,
                TrafficControlRequired = true,
                DigitalAsBuiltRequired = true
            });
            _container.Inject(_repository);
            var args = new FormViewUpdateEventArgs(null);
            _mocks.ReplayAll();
            
            InvokeEventByName(_target, "fvWorkOrder_ItemUpdating",
                new object[] {
                    null, args
                });

            MyAssert.AreClose(DateTime.Parse(
                args.NewValues[
                    WorkOrderStockApprovalForm.WorkOrderParameterNames.MATERIALS_APPROVED_ON].
                    ToString()), DateTime.Now);
            Assert.AreEqual(
                args.NewValues[
                    WorkOrderStockApprovalForm.WorkOrderParameterNames.MATERIALS_APPROVED_BY_ID], expectedCreatorID);
            Assert.IsTrue((bool)args.NewValues[WorkOrderStockApprovalForm.WorkOrderParameterNames.STREET_OPENING_PERMIT_REQUIRED]);
            Assert.IsTrue((bool)args.NewValues[WorkOrderStockApprovalForm.WorkOrderParameterNames.TRAFFIC_CONTROL_REQUIRED]);
            Assert.IsTrue((bool)args.NewValues[WorkOrderStockApprovalForm.WorkOrderParameterNames.DIGITAL_ASBUILT_REQUIRED]);
        }

        [TestMethod]
        public void TestBtnSaveClickCallsStockApprovalFormUpdate()
        {
            using (_mocks.Record())
            {
                _detailControl.UpdateItem(true);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnSave_Click");
            }
        }

        #endregion

    }

    internal class TestWorkOrderStockApprovalFormBuilder : TestDataBuilder<TestWorkOrderStockApprovalForm>
    {
        #region Private Members

        private ISecurityService _securityService;
        private IDetailControl _fvWorkOrder;
        private IObjectContainerDataSource _odsWorkOrder;
        private IPage _iPage;
        private IViewState _iViewState;
        private EventHandler<ObjectContainerDataSourceStatusEventArgs> _onUpdating;

        #endregion

        #region Private Methods

        private void View_OnDispose(TestWorkOrderStockApprovalForm ctrl)
        {
            if (_onUpdating != null)
                ctrl.Updating -= _onUpdating;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrderStockApprovalForm Build()
        {
            var obj = new TestWorkOrderStockApprovalForm();
            if (_fvWorkOrder != null)
                obj.SetFVWorkOrder(_fvWorkOrder);
            if (_odsWorkOrder != null)
                obj.SetODSWorkOrder(_odsWorkOrder);
            if (_iPage != null)
                obj.SetPage(_iPage);
            if (_iViewState != null)
                obj.SetViewState(_iViewState);
            if (_onUpdating != null)
                obj.Updating += _onUpdating;
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            obj._onDispose += View_OnDispose;

            return obj;
        }

        public TestWorkOrderStockApprovalFormBuilder WithDetailControl(IDetailControl fvWorkOrder)
        {
            _fvWorkOrder = fvWorkOrder;
            return this;
        }

        public TestWorkOrderStockApprovalFormBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _odsWorkOrder = dataSource;
            return this;
        }

        public TestWorkOrderStockApprovalFormBuilder WithPage(IPage page)
        {
            _iPage = page;
            return this;
        }

        public TestWorkOrderStockApprovalFormBuilder WithViewState(IViewState viewState)
        {
            _iViewState = viewState;
            return this;
        }

        public TestWorkOrderStockApprovalFormBuilder WithUpdateHandler(EventHandler<ObjectContainerDataSourceStatusEventArgs> handler)
        {
            _onUpdating = handler;
            return this;
        }

        public TestWorkOrderStockApprovalFormBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        #endregion

    }

    internal class TestWorkOrderStockApprovalForm : WorkOrderStockApprovalForm
    {
        #region Delegates

        internal delegate void OnDisposeHandler(TestWorkOrderStockApprovalForm view);

        #endregion

        #region Events

        internal OnDisposeHandler _onDispose;

        #endregion

        #region Exposed Methods

        public void SetFVWorkOrder(IDetailControl ctrl)
        {
            fvWorkOrder = ctrl;
        }

        public void SetODSWorkOrder(IObjectContainerDataSource ods)
        {
            odsWorkOrder = ods;
        }

        public void SetPage(IPage iPage)
        {
            _iPage = iPage;
        }

        public void SetViewState(IViewState viewState)
        {
            _iViewState = viewState;
        }

        public void SetSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public override void Dispose()
        {
            base.Dispose();

            if (_onDispose != null)
                _onDispose(this);
        }

        #endregion
    }
}
