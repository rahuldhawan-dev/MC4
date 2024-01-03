using System;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.Abstract
{
    /// <summary>
    /// Summary description for WorkOrdersDetailViewTest
    /// </summary>
    [TestClass]
    public class WorkOrdersDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestWorkOrdersDetailView _target;
        private IObjectContainerDataSource _dataSource;
        private IDetailControl _detailControl;
        private Button _btnCancel, _btnEdit, _btnSave;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _dataSource = _mocks.DynamicMock<IObjectContainerDataSource>();
            _detailControl = _mocks.DynamicMock<IDetailControl>();
            _btnCancel = new Button();
            _btnEdit = new Button();
            _btnSave = new Button();
            _target =
                new TestWorkOrdersDetailViewBuilder()
                    .WithDataSource(_dataSource)
                    .WithDetailControl(_detailControl)
                    .WithCancelButton(_btnCancel).WithEditButton(_btnEdit)
                    .WithSaveButton(_btnSave);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestSetViewModeCallsDetailControlChangeMVPMode()
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
        public void TestSetViewModeShowsEditButtonAndHidesSaveAndCancelButtonsWhenModeIsReadOnly()
        {
            _mocks.ReplayAll();

            _btnEdit.Visible = false;
            _btnSave.Visible = _btnCancel.Visible = true;

            _target.SetViewMode(DetailViewMode.ReadOnly);

            Assert.IsTrue(_btnEdit.Visible);
            Assert.IsFalse(_btnSave.Visible);
            Assert.IsFalse(_btnCancel.Visible);
        }

        [TestMethod]
        public void TestSetViewModeShowsSaveAndCancelButtonsAndHidesEditButtonWhenModeIsNotReadOnly()
        {
            _mocks.ReplayAll();

            _btnEdit.Visible = true;
            _btnSave.Visible = _btnCancel.Visible = false;

            _target.SetViewMode(DetailViewMode.Edit);

            Assert.IsFalse(_btnEdit.Visible);
            Assert.IsTrue(_btnSave.Visible);
            Assert.IsTrue(_btnCancel.Visible);

            _btnEdit.Visible = true;
            _btnSave.Visible = _btnCancel.Visible = false;

            _target.SetViewMode(DetailViewMode.Insert);

            Assert.IsFalse(_btnEdit.Visible);
            Assert.IsTrue(_btnSave.Visible);
            Assert.IsTrue(_btnCancel.Visible);
        }

        [TestMethod]
        public void TestSetViewControlsVisibleTogglesVisibilityOfButtons()
        {
            _mocks.ReplayAll();

            _btnEdit.Visible = _btnSave.Visible = _btnCancel.Visible = false;

            _target.SetViewControlsVisible(true);

            Assert.IsTrue(_btnEdit.Visible);
            Assert.IsTrue(_btnSave.Visible);
            Assert.IsTrue(_btnCancel.Visible);

            _target.SetViewControlsVisible(false);

            Assert.IsFalse(_btnEdit.Visible);
            Assert.IsFalse(_btnSave.Visible);
            Assert.IsFalse(_btnCancel.Visible);
        }

        [TestMethod]
        public void TestShowEntitySetsDataSourceOfDataSource()
        {
            var expected = new WorkOrder();

            using (_mocks.Record())
            {
                _dataSource.DataSource = expected;
            }

            using (_mocks.Playback())
            {
                _target.ShowEntity(expected);
            }
        }

        [TestMethod]
        public void TestEditButtonPropertyReturnsEditButton()
        {
            Assert.AreSame(_btnEdit, _target.EditButton);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSaveButtonPropertyReturnsSaveButton()
        {
            Assert.AreSame(_btnSave, _target.SaveButton);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestCancelButtonPropertyReturnsCancelButton()
        {
            Assert.AreSame(_btnCancel, _target.CancelButton);

            _mocks.ReplayAll();
        }
    }

    internal class TestWorkOrdersDetailViewBuilder : TestDataBuilder<TestWorkOrdersDetailView>
    {
        #region Private Members

        private EventHandler _onDiscardChangesClicked;
        private IObjectContainerDataSource _dataSource = new MvpObjectContainerDataSource();
        private IDetailControl _detailControl = new MvpFormView();
        private Button _btnCancel = new Button(),
                       _btnEdit = new Button(),
                       _btnSave = new Button();

        #endregion

        #region Private Methods

        private void View_OnDispose(TestWorkOrdersDetailView view)
        {
            if (_onDiscardChangesClicked != null)
                view.DiscardChangesClicked -= _onDiscardChangesClicked;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrdersDetailView Build()
        {
            var view = new TestWorkOrdersDetailView();
            if (_detailControl != null)
                view.SetDetailControl(_detailControl);
            if (_dataSource != null)
                view.SetDataSource(_dataSource);
            if (_onDiscardChangesClicked != null)
                view.DiscardChangesClicked += _onDiscardChangesClicked;
            if (_btnCancel != null)
                view.SetCancelButton(_btnCancel);
            if (_btnEdit != null)
                view.SetEditButton(_btnEdit);
            if (_btnSave != null)
                view.SetSaveButton(_btnSave);
            view._onDispose = View_OnDispose;
            return view;
        }

        public TestWorkOrdersDetailViewBuilder WithDetailControl(IDetailControl detailControl)
        {
            _detailControl = detailControl;
            return this;
        }

        public TestWorkOrdersDetailViewBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _dataSource = dataSource;
            return this;
        }

        public TestWorkOrdersDetailViewBuilder WithDiscardChangesClickEventHandler(EventHandler fn)
        {
            _onDiscardChangesClicked = fn;
            return this;
        }

        public TestWorkOrdersDetailViewBuilder WithCancelButton(Button btn)
        {
            _btnCancel = btn;
            return this;
        }

        public TestWorkOrdersDetailViewBuilder WithEditButton(Button btn)
        {
            _btnEdit = btn;
            return this;
        }

        public TestWorkOrdersDetailViewBuilder WithSaveButton(Button btn)
        {
            _btnSave = btn;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrdersDetailView : WorkOrdersDetailView<WorkOrder>
    {
        #region Private Members

        private IObjectContainerDataSource _dataSource;
        private IDetailControl _detailControl;

        #endregion

        #region Properties

        public override IObjectContainerDataSource DataSource
        {
            get { return _dataSource; }
        }

        public override IDetailControl DetailControl
        {
            get { return _detailControl; }
        }

        public override Button EditButton
        {
            get { return btnEdit; }
        }

        public override Button SaveButton
        {
            get { return btnSave; }
        }

        public override Button CancelButton
        {
            get { return btnCancel; }
        }

        #endregion

        #region Delegates

        internal delegate void OnDisposeHandler(TestWorkOrdersDetailView view);

        #endregion

        #region Events

        internal OnDisposeHandler _onDispose;

        #endregion

        #region Exposed Methods

        public override void Dispose()
        {
            base.Dispose();
            if (_onDispose != null)
                _onDispose(this);
        }

        public void SetDetailControl(IDetailControl detailControl)
        {
            _detailControl = detailControl;
        }

        public void SetDataSource(IObjectContainerDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public void SetCancelButton(Button btn)
        {
            btnCancel = btn;
        }

        public void SetEditButton(Button btn)
        {
            btnEdit = btn;
        }

        public void SetSaveButton(Button btn)
        {
            btnSave = btn;
        }

        #endregion
    }
}
