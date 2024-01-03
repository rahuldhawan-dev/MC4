using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using LINQTo271.Common;
using LINQTo271.Controls.WorkOrders;
using LINQTo271.Views.WorkOrders.Input;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.Practices.Web.UI.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Views.WorkOrders.Input
{
    /// <summary>
    /// Summary description for WorkOrderInputDetailViewTest
    /// </summary>
    [TestClass]
    public class WorkOrderInputDetailViewTest : EventFiringTestClass
    {
        #region Private Members

        private TestWorkOrderInputDetailView _target;
        private IListView<WorkOrder> _listView;
        private IWorkOrderInputFormView _detailControl;
        private IDetailControl _innerDetailControl;
        private IObjectContainerDataSource _dataSource;
        private Button _btnCancel, _btnEdit, _btnSave;
        private IHiddenField _hidLatitude;
        private IHiddenField _hidLongitude;
        private IDropDownList _ddlOperatingCenter;
        private ILatLonPicker _latLonPicker;
        private IWorkOrderDetailControl _woDocumentForm;
        private IPanel _pnlDocumentTab;
        private IPlaceHolder _phDocuments;
        private IRequest _iRequest;
        private IQueryString _iQueryString;


        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _mocks.DynamicMock(out _listView)
                .DynamicMock(out _detailControl)
                .DynamicMock(out _innerDetailControl)
                .DynamicMock(out _hidLatitude)
                .DynamicMock(out _hidLongitude)
                .DynamicMock(out _dataSource)
                .DynamicMock(out _ddlOperatingCenter)
                .DynamicMock(out _latLonPicker)
                .DynamicMock(out _woDocumentForm)
                .DynamicMock(out _phDocuments)
                .DynamicMock(out _pnlDocumentTab)
                .DynamicMock(out _iRequest)
                .DynamicMock(out _iQueryString);


            SetupResult.For(_detailControl.InnerDataSource).Return(_dataSource);
            SetupResult.For(_detailControl.InnerDetailControl)
                .Return(_innerDetailControl);

            _btnCancel = new Button();
            _btnEdit = new Button();
            _btnSave = new Button();

            _target =
                new TestWorkOrderInputDetailViewBuilder()
                    .WithListView(_listView)
                    .WithDetailControl(_detailControl)
                    .WithCancelButton(_btnCancel)
                    .WithEditButton(_btnEdit)
                    .WithSaveButton(_btnSave)
                    .WithDocumentForm(_woDocumentForm)
                    .WithPhDocuments(_phDocuments)
                    .WithPnlDocumentTab(_pnlDocumentTab)
                    .WithIRequest(_iRequest);
                    
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
            _mocks.ReplayAll();

            Assert.AreSame(_detailControl, _target.DetailControl);
        }

        [TestMethod]
        public void TestDataSourcePropertyReturnsDataSource()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_dataSource, _target.DataSource);
        }

        [TestMethod]
        public void TesthidLatitudePropertyReturnsHidLatitudeFromLatLonPicker()
        {
            using(_mocks.Record())
            {
                SetupResult
                    .For(_detailControl.FindIControl<ILatLonPicker>("llpAsset"))
                    .Return(_latLonPicker);
                SetupResult.For(_latLonPicker.hidLatitude).Return(_hidLatitude);
            }

            using(_mocks.Playback())
            {
                Assert.AreSame(_hidLatitude,
                    _target.GetPropertyValueByName("hidLatitude"));
            }
        }

        [TestMethod]
        public void TesthidLongitudePropertyReturnsHidLongitudeFromLatLonPicker()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_detailControl.FindIControl<ILatLonPicker>("llpAsset")).Return(_latLonPicker);
                SetupResult.For(_latLonPicker.hidLongitude).Return(_hidLongitude);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(_hidLongitude, _target.GetPropertyValueByName("hidLongitude"));
            }
        }

        [TestMethod]
        public void TestLatLonPickerPropertyReturnsLatLonPicker()
        {
            using (_mocks.Record())
            {
                SetupResult
                    .For(_detailControl.FindIControl<ILatLonPicker>("llpAsset"))
                    .Return(_latLonPicker);
            }

            using (_mocks.Playback())
            {
                Assert.AreSame(_latLonPicker,
                    _target.GetPropertyValueByName("latLonPicker"));
            }
        }

        [TestMethod]
        public void TestddlOperatingCenterPropertyGet()
        {
            SetupResult.For(_detailControl.FindIControl<IDropDownList>("ddlOperatingCenter")).Return(_ddlOperatingCenter);

            _mocks.ReplayAll();

            Assert.AreSame(_ddlOperatingCenter, _target.GetPropertyValueByName("ddlOperatingCenter"));
        }

        [TestMethod]
        public void TestPhasePropertyDenotesInput()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(WorkOrderPhase.Input, _target.Phase);
        }

        [TestMethod]
        public void TestCurrentDataKeyPropertyReturnsInnerDetailControlDataKey()
        {
            const int value = 1;
            var dictionary = new OrderedDictionary {
                {
                    value, value
                }
            };
            var key = new DataKey(dictionary);

            using (_mocks.Record())
            {
                SetupResult.For(_innerDetailControl.DataKey).Return(key);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(value, _target.CurrentDataKey);
            }
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestOdsUpdatingHandlerFiresOnUpdatingEvent()
        {
            var order = _mocks.DynamicMock<WorkOrder>();
            var args = new ObjectContainerDataSourceStatusEventArgs(order, 0);
            _target = new TestWorkOrderInputDetailViewBuilder()
                .WithUpdateHandler((sender, e) => {
                    _called = true;
                    Assert.AreSame(order, e.Entity);
                });

            _mocks.ReplayAll();

            InvokeEventByName(_target, "ods_Updating", new object[] {
                null, args
            });

            Assert.IsTrue(_called);
        }

        [TestMethod]
        public void TestOdsInsertingHandlerFiresOnInsertingEvent()
        {
            var order = _mocks.DynamicMock<WorkOrder>();
            var args = new ObjectContainerDataSourceStatusEventArgs(order, 0);
            _target = new TestWorkOrderInputDetailViewBuilder()
                .WithInsertHandler((sender, e) => {
                    _called = true;
                    Assert.AreSame(order, e.Entity);
                });

            _mocks.ReplayAll();

            InvokeEventByName(_target, "ods_Inserting", new object[] {
                null, args
            });

            Assert.IsTrue(_called);
        }

        #endregion

        [TestMethod]
        public void TestSetViewModeShowsDocumentsWhenViewModeIsEdit()
        {
            var workOrderID = 42;

            using(_mocks.Record())
            {
                SetupResult.For(_detailControl.WorkOrderID).Return(workOrderID);
                _pnlDocumentTab.Visible = false;
                _phDocuments.Visible = false;
                SetupResult.For(_iRequest.IQueryString).Return(_iQueryString);
                SetupResult.For(_iQueryString.GetValue("arg")).Return(null);
            }

            using (_mocks.Playback())
            {
                _target.SetViewMode(DetailViewMode.Edit);
            }
        }

        [TestMethod]
        public void TestSetViewModeSetsDocumentFormWorkOrderIDFromInputFormWhenNotRPCPage()
        {
            var workOrderID = 42;
            
            using(_mocks.Record())
            {
                SetupResult.For(_iRequest.IQueryString).Return(_iQueryString);
                SetupResult.For(_iQueryString.GetValue("arg")).Return(null);

                SetupResult.For(_detailControl.WorkOrderID).Return(workOrderID);
                _woDocumentForm.WorkOrderID = workOrderID;
            }

            using (_mocks.Playback())
            {
                _target.SetViewMode(DetailViewMode.Edit);
            }
        }

        [TestMethod]
        public void TestSetViewModeSetsDocumentFormWorkOrderIDFromInputFormWhenRPCPage()
        {
            var arg = "42";
            var woID = 42;

            using (_mocks.Record())
            {
                SetupResult.For(_iRequest.IQueryString).Return(_iQueryString);
                SetupResult.For(_iQueryString.GetValue("arg")).Return(arg);
                SetupResult.For(_detailControl.WorkOrderID).Return(woID);
                _woDocumentForm.WorkOrderID = int.Parse(arg);
            }

            using (_mocks.Playback())
            {
                _target.SetViewMode(DetailViewMode.Edit);
            }
        }

        [TestMethod]
        public void TestSetViewControlsVisibleShowsBtnEditWhenReadOnlyMode()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_detailControl.CurrentMvpMode).Return(
                    DetailViewMode.ReadOnly);
            }
            using(_mocks.Playback())
            {
                _target.SetViewControlsVisible(false);
            }

            Assert.IsTrue(_btnEdit.Visible);
        }

        [TestMethod]
        public void TestSetViewControlsVisibleHidesBtnEditWhenEditMode()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_detailControl.CurrentMvpMode).Return(
                    DetailViewMode.Edit);
            }
            using (_mocks.Playback())
            {
                _target.SetViewControlsVisible(true);
            }

            Assert.IsFalse(_btnEdit.Visible);
        }

    }

    internal class TestWorkOrderInputDetailViewBuilder : TestDataBuilder<TestWorkOrderInputDetailView>
    {
        #region Private Members

        private IListView<WorkOrder> _listView = new WorkOrderInputListView();
        private IWorkOrderInputFormView _detailControl = new WorkOrderInputFormView();
        private Button _btnCancel = new Button(),
                       _btnEdit = new Button(),
                       _btnSave = new Button();

        private IHiddenField _hidLatitude;
        private IHiddenField _hidLongitude;
        private IDropDownList _ddlOperatingCenter;
        private ILatLonPicker _latLonPicker;
        private EventHandler<EntityEventArgs<WorkOrder>> _onUpdating,
                                                         _onInserting;
        private IWorkOrderDetailControl _woDocumentForm;
        private IPlaceHolder _phDocuments;
        private IPanel _pnlDocumentTab;
        private IRequest _iRequest;
        private IQueryString _iQueryString;

        #endregion

        #region Private Methods

        private void View_OnDispose(TestWorkOrderInputDetailView ctrl)
        {
            if (_onUpdating != null)
                ctrl.Updating -= _onUpdating;
            if (_onInserting != null)
                ctrl.Inserting -= _onInserting;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrderInputDetailView Build()
        {
            var view = new TestWorkOrderInputDetailView();
            if (_listView != null)
                view.SetListView(_listView);
            if (_detailControl != null)
                view.SetDetailControl(_detailControl);
            if (_btnCancel != null)
                view.SetCancelButton(_btnCancel);
            if (_btnEdit != null)
                view.SetEditButton(_btnEdit);
            if (_btnSave != null)
                view.SetSaveButton(_btnSave);
            if (_hidLatitude != null)
                view.SethidLatitude(_hidLatitude);
            if (_hidLongitude != null)
                view.SethidLongitude(_hidLongitude);
            if (_ddlOperatingCenter != null)
                view.SetddlOperatingCenter(_ddlOperatingCenter);
            if (_latLonPicker != null)
                view.SetLatLonPicker(_latLonPicker);
            if (_onUpdating != null)
                view.Updating += _onUpdating;
            if (_onInserting != null)
                view.Inserting += _onInserting;
            if (_woDocumentForm != null)
                view.SetWoDocumentForm(_woDocumentForm);
            if (_phDocuments != null)
                view.SetPhDocuments(_phDocuments);
            if (_pnlDocumentTab != null)
                view.SetPnlDocumentTab(_pnlDocumentTab);
            view._onDispose += View_OnDispose;
            if (_iRequest != null)
                SetIRequest(view);
            //if (_iQueryString != null)
            //    SetIQueryString(view);
            
            return view;
        }

        private void SetIRequest(TestWorkOrderInputDetailView view)
        {
            SetFieldValue(view, "_iRequest", _iRequest);
        }
        
        //private void SetIQueryString(TestWorkOrderInputDetailView view)
        //{
        //    SetFieldValue(view, "_iQueryString", _iQueryString);
        //}

        public TestWorkOrderInputDetailViewBuilder WithListView(IListView<WorkOrder> listView)
        {
            _listView = listView;
            return this;
        }

        public TestWorkOrderInputDetailViewBuilder WithDetailControl(IWorkOrderInputFormView detailControl)
        {
            _detailControl = detailControl;
            return this;
        }

        public TestWorkOrderInputDetailViewBuilder WithCancelButton(Button btn)
        {
            _btnCancel = btn;
            return this;
        }

        public TestWorkOrderInputDetailViewBuilder WithEditButton(Button btn)
        {
            _btnEdit = btn;
            return this;
        }

        public TestWorkOrderInputDetailViewBuilder WithSaveButton(Button btn)
        {
            _btnSave = btn;
            return this;
        }

        public TestWorkOrderInputDetailViewBuilder WithUpdateHandler(EventHandler<EntityEventArgs<WorkOrder>> handler)
        {
            _onUpdating = handler;
            return this;
        }

        public TestWorkOrderInputDetailViewBuilder WithInsertHandler(EventHandler<EntityEventArgs<WorkOrder>> handler)
        {
            _onInserting = handler;
            return this;
        }

        public TestWorkOrderInputDetailViewBuilder WithDocumentForm(IWorkOrderDetailControl control)
        {
            _woDocumentForm = control;
            return this;
        }

        public TestWorkOrderInputDetailViewBuilder WithPhDocuments(IPlaceHolder phDocuments)
        {
            _phDocuments = phDocuments;
            return this;
        }

        public TestWorkOrderInputDetailViewBuilder WithPnlDocumentTab(IPanel pnlDocumentTab)
        {
            _pnlDocumentTab = pnlDocumentTab;
            return this;
        }

        public TestWorkOrderInputDetailViewBuilder WithIRequest(IRequest request)
        {
            _iRequest = request;
            return this;
        }

        //public TestWorkOrderInputDetailViewBuilder WithIQueryString(IQueryString iQueryString)
        //{
        //    _iQueryString = iQueryString;
        //    return this;
        //}
        
        #endregion
    }

    internal class TestWorkOrderInputDetailView : WorkOrderInputDetailView
    {
        #region Delegates

        internal delegate void OnDisposeHandler(TestWorkOrderInputDetailView view);

        #endregion

        #region Events

        internal OnDisposeHandler _onDispose;

        #endregion

        #region Exposed Methods

        public void SetListView(IListView<WorkOrder> listView)
        {
            wolvWorkOrderHistory = listView;
        }

        public void SetDetailControl(IWorkOrderInputFormView detailControl)
        {
            fvWorkOrder = detailControl;
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

        public void SethidLatitude(IHiddenField latitude)
        {
            _hidLatitude = latitude;
        }

        public void SethidLongitude(IHiddenField longitude)
        {
            _hidLongitude = longitude;
        }
        
        public void SetddlOperatingCenter(IDropDownList center)
        {
            _ddlOperatingCenter = center;
        }

        public void SetLatLonPicker(ILatLonPicker picker)
        {
            _latLonPicker = picker;
        }
        
        public void SetWoDocumentForm(IWorkOrderDetailControl control)
        {
            woDocumentForm = control;
        }

        public void SetPhDocuments(IPlaceHolder documents)
        {
            phDocumentTab = documents;
        }

        public void SetPnlDocumentTab(IPanel documentTab)
        {
            pnlDocumentTab = documentTab;
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
