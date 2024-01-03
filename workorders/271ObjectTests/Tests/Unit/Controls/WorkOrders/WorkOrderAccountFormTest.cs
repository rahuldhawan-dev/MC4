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

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    [TestClass]
    public class WorkOrderAccountFormTest : EventFiringTestClass
    {
        #region Private Members

        private ISecurityService _securityService;
        private TestWorkOrderAccountForm _target;
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

            _target = new TestWorkOrderAccountFormBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource)
                .WithPage(_iPage)
                .WithViewState(_iViewState)
                .WithSecurityService(_securityService)
                .WithRepository(_repository);

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

        //[TestMethod]
        //public void TestUpdateCallsUpdateItemOnDetailControl()
        //{
        //    using (_mocks.Record())
        //    {
        //        _detailControl.UpdateItem(true);
        //    }

        //    using (_mocks.Playback())
        //    {
        //        _target.Update();
        //    }
        //}

        [TestMethod]
        public void TestObjectDataSourceUpdatedCommandFiresUpdatingEvent()
        {
            _mocks.ReplayAll();

            _target =
                new TestWorkOrderAccountFormBuilder()
                    .WithUpdateHandler((sender, e) => _called = true);

            InvokeEventByName(_target, "ods_Updated");

            Assert.IsTrue(_called);
        }

        [TestMethod]
        public void TestFormViewItemUpdatingSetsRequiredParamsWhenCommandArgIsEmpty()
        {
            var expectedCreatorID = 12345678;
            var expectedCompletedByID = 42;
            var expectedDateCompleted = DateTime.Now;
            var expectedBusinessUnit = "123456";
            var hidBusinessUnit = _mocks.DynamicMock<IHiddenField>();

            var workOrderID = 1;
            var workOrder = new WorkOrder {
                CompletedByID = expectedCompletedByID,
                DateCompleted = expectedDateCompleted,
                BusinessUnit = expectedBusinessUnit,
                WorkDescription = new WorkDescription(),
                StreetOpeningPermitRequired = true,
                TrafficControlRequired = true
            };
            var args = new FormViewUpdateEventArgs(new object());

            using (_mocks.Record())
            {
                SetupResult.For(_securityService.GetEmployeeID()).Return(
                    expectedCreatorID);
                SetupResult.For(
                    _iViewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID)).Return(workOrderID);
                SetupResult.For(_repository.Get(workOrderID)).Return(workOrder);
                SetupResult.For(
                    _detailControl.FindIControl<IHiddenField>(
                        WorkOrderAccountForm.ControlIDs.BusinessUnit)).Return(hidBusinessUnit);
                SetupResult.For(hidBusinessUnit.Value).Return(expectedBusinessUnit);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "fvWorkOrder_ItemUpdating",
                    new object[] {
                        null, args
                    });

                MyAssert.AreClose(DateTime.Parse(
                args.NewValues[
                    WorkOrderAccountForm.WorkOrderParameterNames.APPROVED_ON].
                    ToString()), DateTime.Now);
                Assert.AreEqual(
                    args.NewValues[
                        WorkOrderAccountForm.WorkOrderParameterNames.
                            APPROVED_BY_ID], expectedCreatorID);
                Assert.AreEqual(
                    args.NewValues[
                        WorkOrderAccountForm.WorkOrderParameterNames.
                            COMPLETED_BY_ID], expectedCompletedByID);
                Assert.AreEqual(
                    args.NewValues[
                        WorkOrderAccountForm.WorkOrderParameterNames.
                            DATE_COMPLETED], expectedDateCompleted);
                Assert.AreEqual(
                    args.NewValues[
                        WorkOrderAccountForm.WorkOrderParameterNames.
                            BUSINESS_UNIT], expectedBusinessUnit);
                Assert.AreEqual(
                    args.NewValues[
                        WorkOrderAccountForm.WorkOrderParameterNames.
                            STREET_OPENING_PERMIT_REQUIRED], true);
                Assert.AreEqual(
                    args.NewValues[
                        WorkOrderAccountForm.WorkOrderParameterNames.
                            TRAFFIC_CONTROL_REQUIRED], true);

            }
        }

        [TestMethod]
        public void TestFormViewItemUpdatingSetsRequiredParamsWhenCommandArgIsReject()
        {
            int workOrderID = 1;
            string name = "User Name";
            string notes = "Rejection notes go here.";
            var workOrder = new WorkOrder { CompletedByID = 42, DateCompleted = DateTime.Now };
            var args = new FormViewUpdateEventArgs("Reject");
            var user = _mocks.DynamicMock<IUser>();
            var txtNotes = _mocks.DynamicMock<ITextBox>();
            var datetimeNow = DateTime.Now;

            int? expectedCompletedByID = null;
            int? expectedDateCompleted = null;
            string expectedNotes = string.Format("{0} {1} {2}: {3}", Environment.NewLine, datetimeNow, name, notes);

            using (_mocks.Record())
            {
                SetupResult.For(
                    _iViewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID)).Return(workOrderID);
                SetupResult.For(_repository.Get(workOrderID)).Return(workOrder);
                SetupResult.For(_securityService.CurrentUser).Return(user);
                SetupResult.For(user.Name).Return(name);
                SetupResult.For(
                    _detailControl.FindIControl<ITextBox>(
                        WorkOrderAccountForm.ControlIDs.Notes)).Return(txtNotes);
                SetupResult.For(txtNotes.Text).Return(notes);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "fvWorkOrder_ItemUpdating",
                    new object[] {
                        null, args
                    });

                Assert.AreEqual(
                    args.NewValues[
                        WorkOrderAccountForm.WorkOrderParameterNames.
                            COMPLETED_BY_ID], expectedCompletedByID);
                Assert.AreEqual(
                    args.NewValues[
                        WorkOrderAccountForm.WorkOrderParameterNames.
                            DATE_COMPLETED], expectedDateCompleted);
                Assert.AreEqual(
                    args.NewValues[WorkOrderAccountForm.WorkOrderParameterNames.NOTES], expectedNotes);
            }
        }

        [TestMethod]
        public void TestBtnSaveClickCallsAccountFormUpdate()
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

    internal class TestWorkOrderAccountFormBuilder : TestDataBuilder<TestWorkOrderAccountForm>
    {
        #region Private Members

        private ISecurityService _securityService;
        private IDetailControl _fvWorkOrder;
        private IObjectContainerDataSource _odsWorkOrder;
        private IPage _iPage;
        private IViewState _iViewState;
        private EventHandler<ObjectContainerDataSourceStatusEventArgs> _onUpdating;
        private IRepository<WorkOrder> _repository;

        #endregion

        #region Private Methods

        private void View_OnDispose(TestWorkOrderAccountForm ctrl)
        {
            if (_onUpdating != null)
                ctrl.Updating -= _onUpdating;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrderAccountForm Build()
        {
            var obj = new TestWorkOrderAccountForm();
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
            if (_repository != null)
                obj.SetRepository(_repository);

            obj._onDispose += View_OnDispose;

            return obj;
        }

        public TestWorkOrderAccountFormBuilder WithDetailControl(IDetailControl fvWorkOrder)
        {
            _fvWorkOrder = fvWorkOrder;
            return this;
        }

        public TestWorkOrderAccountFormBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _odsWorkOrder = dataSource;
            return this;
        }

        public TestWorkOrderAccountFormBuilder WithPage(IPage page)
        {
            _iPage = page;
            return this;
        }

        public TestWorkOrderAccountFormBuilder WithViewState(IViewState viewState)
        {
            _iViewState = viewState;
            return this;
        }

        public TestWorkOrderAccountFormBuilder WithUpdateHandler(EventHandler<ObjectContainerDataSourceStatusEventArgs> handler)
        {
            _onUpdating = handler;
            return this;
        }

        public TestWorkOrderAccountFormBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        public TestWorkOrderAccountFormBuilder WithRepository(IRepository<WorkOrder> repository)
        {
            _repository = repository;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderAccountForm : WorkOrderAccountForm
    {
        #region Delegates

        internal delegate void OnDisposeHandler(TestWorkOrderAccountForm view);

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

        public void SetRepository(IRepository<WorkOrder> repository)
        {
            _repository = repository;
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
