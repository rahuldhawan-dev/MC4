using System;
using System.Web.UI.WebControls;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.WorkOrders.General;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.General
{
    /// <summary>
    /// Summary description for WorkOrderGeneralDetailViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderGeneralDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private IWorkOrderAccountForm _accountForm;
        private IWorkOrderInputFormView _inputForm;
        private IWorkOrderRestorationForm _restorationForm;
        private IWorkOrderStreetOpeningPermitForm _streetOpeningPermitForm;
        private IWorkOrderMainBreakForm _mainBreakForm;
        private IObjectContainerDataSource _dataSource;
        private IDetailControl _detailControl;
        private TestWorkOrderGeneralDetailView _target;
        private ISecurityService _securityService;
        private IDropDownList _ddlWorkOrderCancellationReasons;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _detailControl)
                .DynamicMock(out _dataSource)
                .DynamicMock(out _accountForm)
                .DynamicMock(out _inputForm)
                .DynamicMock(out _restorationForm)
                .DynamicMock(out _streetOpeningPermitForm)
                .DynamicMock(out _mainBreakForm)
                .DynamicMock(out _securityService)
                .DynamicMock(out _ddlWorkOrderCancellationReasons);
            
            _target = new TestWorkOrderGeneralDetailViewBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource)
                .WithCancelOrderButton(new MvpButton())
                .WithMaterialPlanningCompletedOnButton(new MvpButton())
                .WithSecurityService(_securityService)
                .WithBtnEdit(new MvpButton())
                .WithBtnDelete(new MvpButton())
                .WithBtnRefresh(new MvpButton())
                .WithLnkCreateService(new HyperLink())
                .WithCancellationReasons(_ddlWorkOrderCancellationReasons)
                .WithEntity(new WorkOrder() {
                    AssetType = new AssetType()
                });

            SetupFormView();
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        private void SetupFormView()
        {
            SetupResult.For(
                _detailControl.FindIControl<IWorkOrderInputFormView>(
                    WorkOrderGeneralDetailView.ControlIDs.INPUT_FORM))
                .Return(_inputForm);
            SetupResult.For(
                _detailControl.FindIControl<IWorkOrderRestorationForm>(
                    WorkOrderGeneralDetailView.ControlIDs.RESTORATION_FORM))
                .Return(_restorationForm);
            SetupResult.For(
                _detailControl.FindIControl<IWorkOrderStreetOpeningPermitForm>(
                    WorkOrderGeneralDetailView.ControlIDs.
                        STREETOPENINGPERMIT_FORM))
                .Return(_streetOpeningPermitForm);
            SetupResult.For(
                _detailControl.FindIControl<IWorkOrderMainBreakForm>(
                    WorkOrderGeneralDetailView.ControlIDs.MAINBREAK_FORM))
                .Return(_mainBreakForm);
            SetupResult.For(
                _detailControl.FindIControl<IWorkOrderAccountForm>(
                    WorkOrderGeneralDetailView.ControlIDs.ACCOUNT_FORM))
                .Return(_accountForm);
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestDataSourcePropertyReturnsDataSource()
        {
            Assert.AreSame(_dataSource, _target.DataSource);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDetailControlPropertyReturnsDetailControl()
        {
            Assert.AreSame(_detailControl, _target.DetailControl);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestPhasePropertyDenotesGeneral()
        {
            Assert.AreEqual(WorkOrderPhase.General, _target.Phase);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestCancelOrderButtonIsVisibleWhenNoCrewAssignmentExist()
        {
            //arrange
            _mocks.ReplayAll();
            var args = new EventArgs();

            //act
            InvokeEventByName(_target, "Page_Prerender", new object[] {null, args});

            //assert
            Assert.IsTrue(_target.CancelOrderButton.Visible);
        }
        
        [TestMethod]
        public void TestCancelOrderButtonIsVisibleWhenCrewAssignmentExistButNoneHaveBeenStarted()
        {
            //arrange
            _target.Entity.CrewAssignments.Add(new CrewAssignment { AssignedFor = DateTime.Now.AddDays(-1) });
            _mocks.ReplayAll();
            var args = new EventArgs();

            //act
            InvokeEventByName(_target, "Page_Prerender", new object[] { null, args });

            //assert
            Assert.IsTrue(_target.CancelOrderButton.Visible);
        }
        
        [TestMethod]
        public void TestCancelOrderButtonIsNOTVisibleWhenCrewAssignmentsExistButAreAllCompleted()
        {
            //arrange
            _target.Entity.CrewAssignments.Add(new CrewAssignment { AssignedFor = DateTime.Now.AddDays(-1), DateStarted = DateTime.Now.AddDays(-1).AddMinutes(-10), DateEnded = DateTime.Now.AddDays(-1) });
            _mocks.ReplayAll();
            var args = new EventArgs();

            //act
            InvokeEventByName(_target, "Page_Prerender", new object[] { null, args });

            //assert
            Assert.IsFalse(_target.CancelOrderButton.Visible);
        }

        [TestMethod]
        public void TestCancelOrderButtonIsNOTVisibleWhenCrewAssignmentsExistButAreSomeAreCompletedSomeAreNotStarted()
        {
            //arrange
            _target.Entity.CrewAssignments.Add(new CrewAssignment { AssignedFor = DateTime.Now.AddDays(-2), DateStarted = DateTime.Now.AddMinutes(-10), DateEnded = DateTime.Now });
            _target.Entity.CrewAssignments.Add(new CrewAssignment { AssignedFor = DateTime.Now.AddDays(-1) });
            _mocks.ReplayAll();
            var args = new EventArgs();

            //act
            InvokeEventByName(_target, "Page_Prerender", new object[] { null, args });

            //assert
            Assert.IsFalse(_target.CancelOrderButton.Visible);
        }

        [TestMethod]
        public void TestCancelOrderButtonIsNOTVisibleWhenCrewAssignmentsExistForToday()
        {
            //arrange
            _target.Entity.CrewAssignments.Add(new CrewAssignment { AssignedFor = DateTime.Today });
            _mocks.ReplayAll();
            var args = new EventArgs();

            //act
            InvokeEventByName(_target, "Page_Prerender", new object[] { null, args });

            //assert
            Assert.IsFalse(_target.CancelOrderButton.Visible);
        }
        
        [TestMethod]
        public void TestCancelOrderButtonIsNOTVisibleWhenCrewAssignmentsAreOpen()
        {
            //arrange
            _target.Entity.CrewAssignments.Add(new CrewAssignment { AssignedFor = DateTime.Now.AddDays(-1), DateStarted = DateTime.Now});
            _mocks.ReplayAll();
            var args = new EventArgs();

            //act
            InvokeEventByName(_target, "Page_Prerender", new object[] { null, args });

            //assert
            Assert.IsFalse(_target.CancelOrderButton.Visible);
        }

        [TestMethod]
        public void TestCancelOrderButtonIsNOTVisibleWhenMaterialsUsedExists()
        {
            //arrange
            _target.Entity.MaterialsUseds.Add(new MaterialsUsed { MaterialID = 1, Material = new Material { Description = "Test"}});
            _mocks.ReplayAll();
            var args = new EventArgs();

            //act
            InvokeEventByName(_target, "Page_Prerender", new object[] { null, args });

            //assert
            Assert.IsFalse(_target.CancelOrderButton.Visible);
        }

        [TestMethod]
        public void TestDeleteClickedSetsContractorDetailsNull()
        {
            _target.Entity.WorkOrderCancellationReasonID = 1;
            _target.Entity.AssignedToContractorOn = DateTime.Now;
            _target.Entity.AssignedContractorID = 123;
            _target.Entity.AssignedContractor = new Contractor { ContractorID = 123 };

            SetupResult.For(_ddlWorkOrderCancellationReasons.SelectedValue).Return("2");
            _mocks.ReplayAll();
            var args = new EventArgs();
            

            //act
            InvokeEventByName(_target, "btnDelete_Click", new object[] { null, args });

            //assert
            Assert.IsNull(_target.Entity.AssignedToContractorOn);
            Assert.IsNull(_target.Entity.AssignedContractor);
        }

        [TestMethod]
        public void TestEditButtonIsNotVisibleWhenInEditMode()
        {
            //arrange
            using (_mocks.Record())
            {
                SetupResult.For(_securityService.IsAdmin).Return(true);
                SetupResult.For(_detailControl.CurrentMvpMode).Return(DetailViewMode.Edit);
            }

            using (_mocks.Playback())
            {
                //act
                InvokeEventByName(_target, "Page_Prerender", new object[] {
                    null, new EventArgs()
                });

                //assert
                Assert.IsFalse(_target.EditButton.Visible);
            }
        }

        [TestMethod]
        public void TestEditButtonIsVisibleWhenNotInEditMode()
        {
            //arrange
            using (_mocks.Record())
            {
                SetupResult.For(_securityService.IsAdmin).Return(true);
                SetupResult.For(_detailControl.CurrentMvpMode).Return(DetailViewMode.Insert);
            }

            using (_mocks.Playback())
            {
                //act
                InvokeEventByName(_target, "Page_Prerender", new object[] {
                    null, new EventArgs()
                });

                //assert
                Assert.IsTrue(_target.EditButton.Visible);
            }
        }

        [TestMethod]
        public void TestMaterialsPlanningCompButtonIsNOTVisibleWhenCompleted()
        {
            //arrange
            _target.Entity.DateCompleted = DateTime.Now;
            _mocks.ReplayAll();
            var args = new EventArgs();

            //act
            InvokeEventByName(_target, "Page_Prerender", new object[] { null, args });

            //assert
            Assert.IsFalse(_target.MaterialPlanningCompletedOnButton.Visible);
        }

        [TestMethod]
        public void TestMaterialsPlanningCompButtonIsNOTVisibleWhenMaterialsPlanningCompleted()
        {
            //arrange
            _target.Entity.MaterialPlanningCompletedOn = DateTime.Now;
            _mocks.ReplayAll();
            var args = new EventArgs();

            //act
            InvokeEventByName(_target, "Page_Prerender", new object[] { null, args });

            //assert
            Assert.IsFalse(_target.MaterialPlanningCompletedOnButton.Visible);
        }

        [TestMethod]
        public void TestMaterialsPlanningCompButtonIsVisibleWhenMaterialsPlanningIsNotCompletedAndNotCompleted()
        {
            //arrange
            _mocks.ReplayAll();
            var args = new EventArgs();

            //act
            InvokeEventByName(_target, "Page_Prerender", new object[] { null, args });

            //assert
            Assert.IsTrue(_target.MaterialPlanningCompletedOnButton.Visible);
        }

        #endregion
    }

    internal class TestWorkOrderGeneralDetailViewBuilder : TestDataBuilder<TestWorkOrderGeneralDetailView>
    {
        #region Private Members

        private IObjectContainerDataSource _dataSource;
        private IDetailControl _detailControl;
        private IButton _cancelOrderButton, _materialsPlanningCompletedOnButton;
        private Button _btnEdit, _btnDelete, _btnRefresh;
        private HyperLink _lnkCreateService;
        private ISecurityService _securityService;
        private WorkOrder _entity;
        private IDropDownList _ddlWorkOrderCancellationReasons;

        #endregion

        #region Exposed Methods

        public override TestWorkOrderGeneralDetailView Build()
        {
            var obj = new TestWorkOrderGeneralDetailView();
            if (_dataSource != null)
                obj.SetDataSource(_dataSource);
            if (_detailControl != null)
                obj.SetDetailControl(_detailControl);
            if (_cancelOrderButton != null)
                obj.SetCancelOrderButton(_cancelOrderButton);
            if (_materialsPlanningCompletedOnButton != null)
                obj.SetMaterialPlanningCompletedOnButton(_materialsPlanningCompletedOnButton);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            if (_btnEdit != null)
                obj.SetButtonEdit(_btnEdit);
            if (_btnDelete!= null)
                obj.SetButtonDelete(_btnDelete);
            if (_btnRefresh != null)
                obj.SetButtonRefresh(_btnRefresh);
            if (_lnkCreateService != null)
                obj.SetLnkCreateService(_lnkCreateService);
            if (_entity != null)
                obj.SetEntity(_entity);
            if (_ddlWorkOrderCancellationReasons != null)
                obj.SetCancellationReason(_ddlWorkOrderCancellationReasons);
            return obj;
        }

        public TestWorkOrderGeneralDetailViewBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _dataSource = dataSource;
            return this;
        }

        public TestWorkOrderGeneralDetailViewBuilder WithDetailControl(IDetailControl detailControl)
        {
            _detailControl = detailControl;
            return this;
        }

        public TestWorkOrderGeneralDetailViewBuilder WithMaterialPlanningCompletedOnButton(IButton button)
        {
            _materialsPlanningCompletedOnButton = button;
            return this;
        }

        public TestWorkOrderGeneralDetailViewBuilder WithCancelOrderButton(IButton cancelOrderButton)
        {
            _cancelOrderButton = cancelOrderButton;
            return this;
        }

        public TestWorkOrderGeneralDetailViewBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        public TestWorkOrderGeneralDetailViewBuilder WithBtnEdit(Button btnEdit)
        {
            _btnEdit = btnEdit;
            return this;
        }

        public TestWorkOrderGeneralDetailViewBuilder WithBtnRefresh(
            Button btnRefresh)
        {
            _btnRefresh = btnRefresh;
            return this;
        }

        public TestWorkOrderGeneralDetailViewBuilder WithBtnDelete(Button btnDelete)
        {
            _btnDelete = btnDelete;
            return this;
        }

        public TestWorkOrderGeneralDetailViewBuilder WithLnkCreateService(HyperLink lnkCreateService)
        {
            _lnkCreateService = lnkCreateService;
            return this;
        }

        public TestWorkOrderGeneralDetailViewBuilder WithEntity(WorkOrder entity)
        {
            _entity = entity;
            return this;
        }

        public TestWorkOrderGeneralDetailViewBuilder WithCancellationReasons(IDropDownList reasons)
        {
            _ddlWorkOrderCancellationReasons = reasons;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderGeneralDetailView : WorkOrderGeneralDetailView
    {
        #region Exposed Methods

        internal void SetDataSource(IObjectContainerDataSource dataSource)
        {
            odsWorkOrder = dataSource;
        }

        internal void SetDetailControl(IDetailControl detailControl)
        {
            fvWorkOrder = detailControl;
        }

        internal void SetCancelOrderButton(IButton cancelOrderButton)
        {
            btnCancelOrder = (MvpButton)cancelOrderButton;
        }

        internal void SetMaterialPlanningCompletedOnButton(IButton button)
        {
            btnMaterialPlanningComplete = (MvpButton)button;
        }

        public void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }

        #endregion

        public void SetButtonEdit(Button button)
        {
            btnEdit = button;
        }

        public void SetButtonDelete(Button button)
        {
            btnDelete = button;
        }

        public void SetButtonRefresh(Button button)
        {
            btnRefresh = button;
        }

        public void SetEntity(WorkOrder entity)
        {
            Entity = entity;
        }

        public void SetLnkCreateService(HyperLink link)
        {
            lnkCreateService = link;
        }

        public void SetCancellationReason(IDropDownList list)
        {
            ddlWorkOrderCancellationReasons = list;
        }
    }
}
