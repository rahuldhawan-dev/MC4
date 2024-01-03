using Microsoft.Practices.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.View;
using MMSINCTestImplementation.Model;
using Rhino.Mocks;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using System.Web.UI;
using MMSINC.Utilities.StructureMap;

namespace MMSINC.Core.WebFormsTest.View
{
    /// <summary>
    /// Summary description for DetailViewTest
    /// </summary>
    [TestClass]
    public class DetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private MockDetailView _target;
        private IDetailPresenter<Employee> _presenter;

        #endregion

        #region Private Static Methods

        private static void InvokePageLoad(object obj)
        {
            InvokePageLoad(obj, GetEventArgArray());
        }

        private static void InvokePageLoad(object obj, object[] eventArgsArray)
        {
            InvokeEventByName(obj, "Page_Load", eventArgsArray);
        }

        #endregion

        #region Additional test attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks = new MockRepository();
            _presenter = _mocks.DynamicMock<IDetailPresenter<Employee>>();
            _container.Inject(_presenter);
            _target = new TestDetailViewBuilder();
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
            _mocks.VerifyAll();
            _presenter = null;
        }

        #endregion

        [TestMethod]
        public void TestThatYouRunningYourTestsInDebugMode()
        {
#if DEBUG
            _mocks.ReplayAll();
#else
            Assert.Fail("DOH! Switch to DEBUG mode and run all your tests again.");
#endif
        }

        [TestMethod]
        public void TestPageLoadCallsPresenterOnViewInitializedWhenIsNotPostBack()
        {
            _target = new TestDetailViewBuilder().WithPostBack(false);

            using (_mocks.Record())
            {
                _presenter.OnViewInitialized();
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadDoesNotCallPresenterOnViewInitializedWhenIsPostBack()
        {
            _target = new TestDetailViewBuilder().WithPostBack(true);

            using (_mocks.Record())
            {
                DoNotExpect.Call(_presenter.OnViewInitialized);
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestPageLoadFiresUserControlLoadedEvent()
        {
            using (_target = new TestDetailViewBuilder().WithOnUserControlLoadedHandler(
                _testableHandler))
            {
                _mocks.ReplayAll();

                InvokePageLoad(_target);

                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void TestPageLoadCallsOnViewLoadedOnPresenter()
        {
            _target = new TestDetailViewBuilder();

            using (_mocks.Record())
            {
                _presenter.OnViewLoaded();
            }

            using (_mocks.Playback())
            {
                InvokePageLoad(_target);
            }
        }

        [TestMethod]
        public void TestbtnCancel_ClickFiresDiscardChangesClickedEvent()
        {
            using (_target = new TestDetailViewBuilder().WithOnDiscardChangesClickedHandler(_testableHandler))
            {
                _mocks.ReplayAll();

                InvokeEventByName(_target, "btnCancel_Click");

                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void TestbtnEdit_ClickFiresEditClickedEvent()
        {
            using (_target = new TestDetailViewBuilder().WithOnEditClickedHandler(_testableHandler))
            {
                _mocks.ReplayAll();

                InvokeEventByName(_target, "btnEdit_Click");

                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void TestbtnDelete_ClickCallsDeleteItemOnDetailControl()
        {
            var detailControl = _mocks.DynamicMock<IDetailControl>();
            _target =
                new TestDetailViewBuilder().WithDetailControl(detailControl);

            using (_mocks.Record())
            {
                detailControl.DeleteItem();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnDelete_Click");
            }
        }

        [TestMethod]
        public void TestbtnSave_ClickCallsUpdateItemOnDetailControlWhenModeIsEdit()
        {
            var detailControl = _mocks.DynamicMock<IDetailControl>();
            SetupResult.For(detailControl.CurrentMvpMode).Return(
                DetailViewMode.Edit);
            _target =
                new TestDetailViewBuilder().WithDetailControl(detailControl);

            using (_mocks.Record())
            {
                detailControl.UpdateItem(true);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnSave_Click");
            }
        }

        [TestMethod]
        public void TestbtnSave_ClickCallsInsertItemOnDetailControlWhenModeIsInsert()
        {
            var detailControl = _mocks.DynamicMock<IDetailControl>();
            SetupResult.For(detailControl.CurrentMvpMode).Return(
                DetailViewMode.Insert);
            _target =
                new TestDetailViewBuilder().WithDetailControl(detailControl);

            using (_mocks.Record())
            {
                detailControl.InsertItem(true);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnSave_Click");
            }
        }

        // TODO: Should the above also throw an exception when mode is ReadOnly?
        [TestMethod]
        public void TestbtnSave_ClickThrowsExceptionWhenModeIsInsert()
        {
            var detailControl = _mocks.DynamicMock<IDetailControl>();
            SetupResult.For(detailControl.CurrentMvpMode).Return(
                DetailViewMode.ReadOnly);
            _target =
                new TestDetailViewBuilder().WithDetailControl(detailControl);
            _mocks.ReplayAll();
            MyAssert.Throws<InvalidOperationException>(
                () => InvokeEventByName(_target, "btnSave_Click"));
        }

        [TestMethod]
        public void Testods_DeletedFiresDeleteClickedEvent()
        {
            using (_target = new TestDetailViewBuilder().WithOnDeleteClickedHandler((sender, e) => _called = true))
            {
                var arg =
                    new ObjectContainerDataSourceStatusEventArgs(
                        new Employee(), 1);

                _mocks.ReplayAll();

                InvokeEventByName(_target, "ods_Deleted",
                    new object[] {null, arg});

                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void Testods_InsertedFiresInsertingEvent()
        {
            using (_target = new TestDetailViewBuilder().WithOnInsertingHandler((sender, e) => _called = true))
            {
                var arg =
                    new ObjectContainerDataSourceStatusEventArgs(
                        new Employee(), 1);

                _mocks.ReplayAll();

                InvokeEventByName(_target, "ods_Inserted",
                    new object[] {null, arg});

                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void Testods_UpdatedFiresUpdatingEvent()
        {
            using (_target = new TestDetailViewBuilder().WithOnUpdatingHandler((sender, e) => _called = true))
            {
                var arg =
                    new ObjectContainerDataSourceStatusEventArgs(
                        new Employee(), 1);

                _mocks.ReplayAll();

                InvokeEventByName(_target, "ods_Updated",
                    new object[] {null, arg});

                Assert.IsTrue(_called);
            }
        }

        [TestMethod]
        public void TestCurrentModeReflectsDetailControlCurrentMode()
        {
            var mode = DetailViewMode.Insert;
            var detailControl = _mocks.DynamicMock<IDetailControl>();
            SetupResult.For(detailControl.CurrentMvpMode).Return(mode);
            _target =
                new TestDetailViewBuilder().WithDetailControl(detailControl);

            _mocks.ReplayAll();

            Assert.AreEqual(mode, _target.CurrentMode);
        }

        [TestMethod]
        public void TestGetChildResourceViewReturnsOnlyChildResourceViews()
        {
            _target =
                new TestDetailViewBuilder();
            _mocks.ReplayAll();
            Assert.IsNotNull(_target.ChildResourceViews);
            Assert.IsInstanceOfType(_target.ChildResourceViews, typeof(IEnumerable<IChildResourceView>));
        }
    }

    internal class TestDetailViewBuilder : TestDataBuilder<MockDetailView>
    {
        #region Private Members

        private bool? _postBack = true;

        private EventHandler _onUserControlLoaded,
                             _onDiscardChangesClicked,
                             _onEditClicked;

        private EventHandler<EntityEventArgs<Employee>>
            _onDeleteClicked, _onInserting, _onUpdating;

        private IDetailControl _detailControl;

        #endregion

        #region Private Methods

        private void SetPostBack(MockDetailView dv)
        {
            var isPostBack = dv.GetType().GetField("_isMvpPostBack",
                BindingFlags.Instance |
                BindingFlags.NonPublic);
            isPostBack.SetValue(dv, _postBack.Value);
        }

        private void DetailView_Dispose(MockDetailView dv)
        {
            if (_onUserControlLoaded != null)
                dv.UserControlLoaded -= _onUserControlLoaded;
            if (_onDiscardChangesClicked != null)
                dv.DiscardChangesClicked -= _onDiscardChangesClicked;
            if (_onEditClicked != null)
                dv.EditClicked -= _onEditClicked;
            if (_onDeleteClicked != null)
                dv.DeleteClicked -= _onDeleteClicked;
            if (_onInserting != null)
                dv.Inserting -= _onInserting;
            if (_onUpdating != null)
                dv.Updating -= _onUpdating;
            // TODO: This is no longer necessary
            //if (_onSaveClicked != null)
            //    dv.SaveClicked -= _onSaveClicked;
        }

        #endregion

        #region Exposed Methods

        public override MockDetailView Build()
        {
            var dv = new MockDetailView();
            if (_postBack != null)
                SetPostBack(dv);
            if (_onUserControlLoaded != null)
                dv.UserControlLoaded += _onUserControlLoaded;
            if (_onDiscardChangesClicked != null)
                dv.DiscardChangesClicked += _onDiscardChangesClicked;
            if (_onEditClicked != null)
                dv.EditClicked += _onEditClicked;
            if (_onDeleteClicked != null)
                dv.DeleteClicked += _onDeleteClicked;
            if (_onInserting != null)
                dv.Inserting += _onInserting;
            if (_onUpdating != null)
                dv.Updating += _onUpdating;
            // TODO: This is no longer necessary
            //if (_onSaveClicked != null)
            //    dv.SaveClicked += _onSaveClicked;
            if (_detailControl != null)
                dv.SetDetailControl(_detailControl);
            dv.OnDispose += DetailView_Dispose;
            return dv;
        }

        public TestDetailViewBuilder WithPostBack(bool postBack)
        {
            _postBack = postBack;
            return this;
        }

        public TestDetailViewBuilder WithOnUserControlLoadedHandler(EventHandler onUserControlLoaded)
        {
            _onUserControlLoaded = onUserControlLoaded;
            return this;
        }

        public TestDetailViewBuilder WithOnDiscardChangesClickedHandler(EventHandler onDiscardChangesClicked)
        {
            _onDiscardChangesClicked = onDiscardChangesClicked;
            return this;
        }

        public TestDetailViewBuilder WithOnEditClickedHandler(EventHandler onEditClicked)
        {
            _onEditClicked = onEditClicked;
            return this;
        }

        public TestDetailViewBuilder WithOnDeleteClickedHandler(EventHandler<EntityEventArgs<Employee>> onDeleteClicked)
        {
            _onDeleteClicked = onDeleteClicked;
            return this;
        }

        public TestDetailViewBuilder WithOnInsertingHandler(EventHandler<EntityEventArgs<Employee>> onInserting)
        {
            _onInserting = onInserting;
            return this;
        }

        public TestDetailViewBuilder WithOnUpdatingHandler(EventHandler<EntityEventArgs<Employee>> onUpdating)
        {
            _onUpdating = onUpdating;
            return this;
        }

        // TODO: This is no longer necessary
        //public TestDetailViewBuilder WithOnSaveClickedHandler(EventHandler<EntityEventArgs<Employee>> onSaveClicked)
        //{
        //    _onSaveClicked = onSaveClicked;
        //    return this;
        //}

        public TestDetailViewBuilder WithDetailControl(IDetailControl detailControl)
        {
            _detailControl = detailControl;
            return this;
        }

        #endregion
    }

    internal class MockDetailView : DetailView<Employee>
    {
        #region Private Members

        private IDetailControl _detailControl;

        #endregion

        #region Delegates

        public delegate void OnDisposeHandler(MockDetailView dv);

        #endregion

        #region Events

        public OnDisposeHandler OnDispose;

        #endregion

        #region Properties

        public override IDetailControl DetailControl
        {
            get { return _detailControl; }
        }

        #endregion

        #region Private Methods

        private void AddMockedControl(IControl control)
        {
            Controls.Add(new MockControl(control));
        }

        #endregion

        #region Exposed Methods

        public void SetDetailControl(IDetailControl detailControl)
        {
            _detailControl = detailControl;
            AddMockedControl(detailControl);
        }

        public override void Dispose()
        {
            base.Dispose();
            if (OnDispose != null)
                OnDispose(this);
        }

        public override void SetViewControlsVisible(bool visible)
        {
            throw new NotImplementedException();
        }

        public override void SetViewMode(DetailViewMode newMode)
        {
            throw new NotImplementedException();
        }

        public override void ShowEntity(Employee instance)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    internal class MockControl : Control, IControl
    {
        #region Private Members

        private IControl _innerControl;

        #endregion

        #region Properties

        public override bool Visible
        {
            get { return _innerControl.Visible; }
            set { _innerControl.Visible = value; }
        }

        #endregion

        #region Constructors

        private MockControl() { }

        public MockControl(IControl control)
        {
            _innerControl = control;
        }

        #endregion

        #region Exposed Methods

        public TControl FindControl<TControl>(string id) where TControl : Control
        {
            return
                ControlExtensions.FindControl
                    <TControl>(this, id);
        }

        public TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
