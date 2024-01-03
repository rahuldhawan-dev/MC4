using System.Web.UI.WebControls;
using LINQTo271.Views.WorkOrders.StockToIssue;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.StockToIssue
{
    /// <summary>
    /// Summary description for WorkOrderStockToIssueDetailViewTest.
    /// </summary>
    [TestClass]
    public class WorkOrderStockToIssueDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;
        private Button btnEdit, btnSave, btnCancel;
        private TestWorkOrderStockToIssueDetailView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks
                .DynamicMock(out _detailControl)
                .DynamicMock(out _dataSource);

            btnEdit = new Button();
            btnSave = new Button();
            btnCancel = new Button();

            _target = new TestWorkOrderStockToIssueDetailViewBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource)
                .WithEditButton(btnEdit)
                .WithSaveButton(btnSave)
                .WithCancelButton(btnCancel);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

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
        public void TestPhasePropertyDenotesStockApproval()
        {
            Assert.AreEqual(WorkOrderPhase.StockApproval, _target.Phase);

            _mocks.ReplayAll();
        }

        #endregion

        #region Method Tests

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

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestItemUpdatingCommandSetsApprovalDateToCurrentTimeAndApproverToCurrentUser()
        {
            _mocks.ReplayAll();

            Assert.Inconclusive("Test not yet written.");
        }

        //[TestMethod]
        //public void TestFormViewItemUpdatingSetsApprovalDateApprovedBy()
        //{
        //    var expectedCreatorID = _target.GetEmployeeIDForLoggedInUser();

        //    var args = new FormViewUpdateEventArgs(null);
        //    _mocks.ReplayAll();
        //    InvokeEventByName(_target, "fvWorkOrder_ItemUpdating",
        //        new object[] {
        //            null, args
        //        });

        //    MyAssert.AreClose(DateTime.Parse(
        //        args.NewValues[
        //            WorkOrderStockToIssueDetailView.WorkOrderParameterNames.MATERIALS_APPROVED_ON].
        //            ToString()), DateTime.Now);
        //    Assert.AreEqual(
        //        args.NewValues[
        //            WorkOrderStockToIssueDetailView.WorkOrderParameterNames.
        //                MATERIALS_APPROVED_BY_ID], expectedCreatorID);
        //}

        #endregion
    }

    internal class TestWorkOrderStockToIssueDetailViewBuilder : TestDataBuilder<TestWorkOrderStockToIssueDetailView>
    {
        #region Private Members

        private IDetailControl _detailControl = new MvpDetailsView();
        private IObjectContainerDataSource _dataSource = new MvpObjectContainerDataSource();
        private Button _btnEdit = new Button(),
                       _btnSave = new Button(),
                       _btnCancel = new Button();

        #endregion

        #region Exposed Methods

        public override TestWorkOrderStockToIssueDetailView Build()
        {
            var obj = new TestWorkOrderStockToIssueDetailView();
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

        public TestWorkOrderStockToIssueDetailViewBuilder WithDetailControl(IDetailControl control)
        {
            _detailControl = control;
            return this;
        }

        public TestWorkOrderStockToIssueDetailViewBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _dataSource = dataSource;
            return this;
        }

        public TestWorkOrderStockToIssueDetailViewBuilder WithEditButton(Button btnEdit)
        {
            _btnEdit = btnEdit;
            return this;
        }

        public TestWorkOrderStockToIssueDetailViewBuilder WithSaveButton(Button btnSave)
        {
            _btnSave = btnSave;
            return this;
        }

        public TestWorkOrderStockToIssueDetailViewBuilder WithCancelButton(Button btnCancel)
        {
            _btnCancel = btnCancel;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderStockToIssueDetailView : WorkOrderStockToIssueDetailView
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
