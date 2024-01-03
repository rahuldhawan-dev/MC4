using System.Web.UI.WebControls;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.WorkOrders.SupervisorApproval;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.SupervisorApproval
{
    /// <summary>
    /// Summary description for WorkOrderSupervisorApprovalDetailViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderSupervisorApprovalDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;
        private IWorkOrderAccountForm _accountForm;
        private Button btnEdit, btnSave, btnCancel;
        private TestWorkOrderSupervisorApprovalDetailView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks
                .DynamicMock(out _detailControl)
                .DynamicMock(out _dataSource)
                .DynamicMock(out _accountForm);

            btnEdit = new Button();
            btnSave = new Button();
            btnCancel = new Button();

            _target = new TestWorkOrderSupervisorApprovalDetailViewBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource)
                .WithEditButton(btnEdit)
                .WithSaveButton(btnSave)
                .WithCancelButton(btnCancel);

            SetupForm();
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        public void SetupForm()
        {
            //SetupResult.For(_detailControl.FindIControl
            //    <IWorkOrderAccountForm>(
            //    WorkOrderSupervisorApprovalDetailView.ControlIDs.
            //        ACCOUNT_FORM)).Return(_accountForm);
        }

        #endregion

        [TestMethod]
        public void TestDetailControlPropertyReturnsDetailControl()
        {
            Assert.AreSame(_detailControl, _target.DetailControl);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDataSourcePropertyReturnsDataSource()
        {
            Assert.AreSame(_dataSource, _target.DataSource);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestPhasePropertyDenotesApproval()
        {
            Assert.AreEqual(WorkOrderPhase.Approval, _target.Phase);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSetViewModeSetsModeOfDetailControl()
        {
            using (_mocks.Record())
            {
                _detailControl.ChangeMvpMode(DetailViewMode.Edit);
                _detailControl.ChangeMvpMode(DetailViewMode.Insert);
                _detailControl.ChangeMvpMode(DetailViewMode.ReadOnly);
            }

            using (_mocks.Playback())
            {
                _target.SetViewMode(DetailViewMode.Edit);
                _target.SetViewMode(DetailViewMode.Insert);
                _target.SetViewMode(DetailViewMode.ReadOnly);
            }
        }

        [TestMethod]
        public void TestSetViewModeHidesSaveButtonAndShowsEditButtonWhenModeIsReadOnly()
        {
            _mocks.ReplayAll();

            btnEdit.Visible = false;
            btnSave.Visible = true;

            _target.SetViewMode(DetailViewMode.ReadOnly);

            Assert.IsTrue(btnEdit.Visible);
            Assert.IsFalse(btnSave.Visible);
        }

        [TestMethod]
        public void TestSetViewModeShowsSaveButtonAndHidesEditButtonWhenModeIsNotReadOnly()
        {
            _mocks.ReplayAll();

            btnEdit.Visible = true;
            btnSave.Visible = false;

            _target.SetViewMode(DetailViewMode.Edit);

            Assert.IsFalse(btnEdit.Visible);
            Assert.IsTrue(btnSave.Visible);

            btnEdit.Visible = true;
            btnSave.Visible = false;

            _target.SetViewMode(DetailViewMode.Insert);

            Assert.IsFalse(btnEdit.Visible);
            Assert.IsTrue(btnSave.Visible);
        }
    }

    internal class TestWorkOrderSupervisorApprovalDetailViewBuilder : TestDataBuilder<TestWorkOrderSupervisorApprovalDetailView>
    {
        #region Private Members

        private IDetailControl _detailControl = new MvpDetailsView();
        private IObjectContainerDataSource _dataSource = new MvpObjectContainerDataSource();
        private Button _btnEdit = new Button(),
                       _btnSave = new Button(),
                       _btnCancel = new Button();

        private IWorkOrderAccountForm _accountForm;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderSupervisorApprovalDetailView Build()
        {
            var obj = new TestWorkOrderSupervisorApprovalDetailView();
            if (_detailControl != null)
                obj.SetDetailControl(_detailControl);
            if (_dataSource != null)
                obj.SetDataSource(_dataSource);
            if (_btnEdit != null)
                obj.SetEditButton(_btnEdit);
            if (_btnSave != null)
                obj.SetSaveButton(_btnSave);
            if (_btnCancel != null)
                obj.SetCancelButton(_btnCancel);
            return obj;
        }

        public TestWorkOrderSupervisorApprovalDetailViewBuilder WithDetailControl(IDetailControl control)
        {
            _detailControl = control;
            return this;
        }

        public TestWorkOrderSupervisorApprovalDetailViewBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _dataSource = dataSource;
            return this;
        }

        public TestWorkOrderSupervisorApprovalDetailViewBuilder WithEditButton(Button btnEdit)
        {
            _btnEdit = btnEdit;
            return this;
        }

        public TestWorkOrderSupervisorApprovalDetailViewBuilder WithSaveButton(Button btnSave)
        {
            _btnSave = btnSave;
            return this;
        }

        public TestWorkOrderSupervisorApprovalDetailViewBuilder WithCancelButton(Button btnCancel)
        {
            _btnCancel = btnCancel;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderSupervisorApprovalDetailView : WorkOrderSupervisorApprovalDetailView
    {
        #region Exposed Methods

        public void SetDetailControl(IDetailControl detailControl)
        {
            fvWorkOrder = detailControl;
        }

        public void SetDataSource(IObjectContainerDataSource dataSource)
        {
            odsWorkOrder = dataSource;
        }

        public void SetEditButton(Button btn)
        {
            btnEdit = btn;
        }

        public void SetSaveButton(Button btn)
        {
            btnSave = btn;
        }

        public void SetCancelButton(Button btn)
        {
            btnCancel = btn;
        }

        #endregion
    }
}
