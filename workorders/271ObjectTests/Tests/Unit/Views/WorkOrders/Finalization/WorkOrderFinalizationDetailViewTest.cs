using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.WorkOrders.Finalization;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Finalization
{
    /// <summary>
    /// Summary description for WorkOrderFinalizationDetailViewTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderFinalizationDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;
        private Button btnEdit, btnSave, btnCancel;
        private MvpLinkButton linkEdit;
        private TestWorkOrderFinalizationDetailView _target;

        private IWorkOrderAdditionalFinalizationInfoForm
            _woAdditionalFinalizationInfoForm;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks
                .DynamicMock(out _detailControl)
                .DynamicMock(out _dataSource)
                .DynamicMock(out _woAdditionalFinalizationInfoForm);

            btnEdit = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            linkEdit = new MvpLinkButton();

            _target = new TestWorkOrderFinalizationDetailViewBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource)
                .WithEditButton(btnEdit)
                .WithSaveButton(btnSave)
                .WithCancelButton(btnCancel)
                .WithEditLinkButton(linkEdit)
                .WithWOAddtionalFinalizationInfoForm(_woAdditionalFinalizationInfoForm);
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
        public void TestPhasePropertyDenotesFinalization()
        {
            Assert.AreEqual(WorkOrderPhase.Finalization, _target.Phase);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestWOAdditionalFinalizationInfoFormPropertyFindsControlsIfNull()
        {
            _target = new TestWorkOrderFinalizationDetailViewBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource)
                .WithEditButton(btnEdit)
                .WithSaveButton(btnSave)
                .WithCancelButton(btnCancel);
            SetupResult.For(
                _detailControl.FindIControl
                    <IWorkOrderAdditionalFinalizationInfoForm>(
                    "woafiAdditionalInfo")).Return(
                _woAdditionalFinalizationInfoForm);
            _mocks.ReplayAll();
            Assert.AreSame(_woAdditionalFinalizationInfoForm, _target.woAdditionalFinalizationInfoForm);
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestSetViewModeSetsModeOfDetailControl()
        {
            var workOrderID = 42;
            var dictionary = new OrderedDictionary { { "Value", workOrderID } };
            var key = new DataKey(dictionary);

            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                SetupResult.For(_detailControl.DataKey).Return(key);

                using (_mocks.Record())
                {
                    _detailControl.ChangeMvpMode(mode);
                }

                using (_mocks.Playback())
                {
                    _target.SetViewMode(mode);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSetViewModeHidesSaveButtonAndShowsEditButtonWhenModeIsReadOnly()
        {
            var workOrderID = 42;
            var dictionary = new OrderedDictionary { { "Value", workOrderID } };
            var key = new DataKey(dictionary);
            SetupResult.For(_detailControl.DataKey).Return(key);

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
            var workOrderID = 42;
            var dictionary = new OrderedDictionary { { "Value", workOrderID } };
            var key = new DataKey(dictionary);
            SetupResult.For(_detailControl.DataKey).Return(key);

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

        [TestMethod]
        public void TestBtnSaveClickFiresWoAdditionalInformationFormUpdateDetailControl()
        {
            using (_mocks.Record())
            {
                _woAdditionalFinalizationInfoForm.UpdateDetailControl();
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnSave_Click");
            }
        }

        #endregion
    }
    
    internal class TestWorkOrderFinalizationDetailViewBuilder : TestDataBuilder<TestWorkOrderFinalizationDetailView>
    {
        #region Private Members

        private IDetailControl _detailControl = new MvpDetailsView();
        private IObjectContainerDataSource _dataSource = new MvpObjectContainerDataSource();
        private Button _btnEdit = new Button(),
                       _btnSave = new Button(),
                       _btnCancel = new Button();

        private MvpLinkButton _lnkEdit = new MvpLinkButton();
        private IWorkOrderAdditionalFinalizationInfoForm
            _woAdditionalFinalizationInfoForm;
        private IWorkOrderMaterialsUsedForm _woMaterialsUsedForm;

        #endregion

        #region Private Methods

        private void View_OnDispose(WorkOrderFinalizationDetailView view)
        {
        }
    
        #endregion

        #region Exposed Methods

        public override TestWorkOrderFinalizationDetailView Build()
        {
            var obj = new TestWorkOrderFinalizationDetailView();
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
            if (_woAdditionalFinalizationInfoForm !=null)
                obj.SetWOAdditionalFinalizationInfoForm(
                    _woAdditionalFinalizationInfoForm);
            if (_lnkEdit != null)
                obj.SetEditLinkButton(_lnkEdit);
            if (_woMaterialsUsedForm != null)
                obj.SetWOMaterialsUSedForm(_woMaterialsUsedForm);
            obj._onDispose += View_OnDispose;
            return obj;
        }

        public TestWorkOrderFinalizationDetailViewBuilder WithDetailControl(IDetailControl detailControl)
        {
            _detailControl = detailControl;
            return this;
        }

        public TestWorkOrderFinalizationDetailViewBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _dataSource = dataSource;
            return this;
        }

        public TestWorkOrderFinalizationDetailViewBuilder WithEditButton(Button btnEdit)
        {
            _btnEdit = btnEdit;
            return this;
        }

        public TestWorkOrderFinalizationDetailViewBuilder WithSaveButton(Button btnSave)
        {
            _btnSave = btnSave;
            return this;
        }

        public TestWorkOrderFinalizationDetailViewBuilder WithCancelButton(Button btnCancel)
        {
            _btnCancel = btnCancel;
            return this;
        }

        public TestWorkOrderFinalizationDetailViewBuilder WithWOAddtionalFinalizationInfoForm(IWorkOrderAdditionalFinalizationInfoForm form)
        {
            _woAdditionalFinalizationInfoForm = form;
            return this;
        }

        public TestWorkOrderFinalizationDetailViewBuilder WithEditLinkButton(MvpLinkButton btn)
        {
            _lnkEdit = btn;
            return this;
        }

        public TestWorkOrderFinalizationDetailViewBuilder WithMaterialsUsedForm(IWorkOrderMaterialsUsedForm form)
        {
            _woMaterialsUsedForm = form;
            return this;
        }
     
        #endregion
     }

    internal class TestWorkOrderFinalizationDetailView : WorkOrderFinalizationDetailView
    {
        #region Delegates

        internal delegate void OnDisposeHandler(
            WorkOrderFinalizationDetailView view);

        #endregion

        #region Events

        internal OnDisposeHandler _onDispose;

        #endregion

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

        public void SetEditLinkButton(MvpLinkButton btn)
        {
            lnkEdit = btn;
        }

        public void SetSaveButton(Button btn)
        {
            btnSave = btn;
        }

        public void SetCancelButton(Button btn)
        {
            btnCancel = btn;
        }

        public void SetWOAdditionalFinalizationInfoForm(IWorkOrderAdditionalFinalizationInfoForm form)
        {
            _woAdditionalFinalizationInfoForm = form;
        }

        public void SetWOMaterialsUSedForm(IWorkOrderMaterialsUsedForm woMaterialsUsedForm)
        {
            _workOrderMaterialsUsedForm = woMaterialsUsedForm;
        }

        #endregion

    }
}