using System;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LINQTo271.Common;
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
using MMSINC.Utilities;
using MMSINC.Utilities.StructureMap;
using Rhino.Mocks;
using StructureMap;
using WorkOrders;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderInputFormViewTestTest
    /// </summary>
    [TestClass]
    public class WorkOrderInputFormViewTest : EventFiringTestClass
    {
        #region Private Members

        private ISecurityService _securityService;
        private IViewState _iViewState;
        private IButton _btnSave;
        private IRepository<WorkOrder> _repository;
        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;
        private IObjectDataSource _odsTowns, _odsEmployees;
        private ParameterCollection _odsTownsSelectParameters,
                                    _odsEmployeesSelectParameters;
        private ILatLonPicker _picker;
        private TestWorkOrderInputFormView _target;
        private IRequest _iRequest;
        private IQueryString _iQueryString;

        private IDateTimeProvider _iDateTimeProvider;
        private IContainer _container;


        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _container = new Container();

            _mocks
                .DynamicMock(out _detailControl)
                .DynamicMock(out _dataSource)
                .DynamicMock(out _odsTowns)
                .DynamicMock(out _odsEmployees)
                .DynamicMock(out _repository)
                .DynamicMock(out _picker)
                .DynamicMock(out _btnSave)
                .DynamicMock(out _iViewState)
                .DynamicMock(out _securityService)
                .DynamicMock(out _iRequest)
                .DynamicMock(out _iQueryString)
                .DynamicMock(out _iDateTimeProvider);

            _odsTownsSelectParameters = new ParameterCollection();
            SetupResult.For(_odsTowns.SelectParameters).Return(
                _odsTownsSelectParameters);
            _odsEmployeesSelectParameters = new ParameterCollection();
            SetupResult.For(_odsEmployees.SelectParameters).Return(
                _odsEmployeesSelectParameters);

            SetupResult.For(
                _detailControl.FindIControl<ILatLonPicker>(
                    WorkOrderInputFormView.ASSET_CONTROL_ID)).Return(_picker);

            _target = new TestWorkOrderInputFormViewBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource)
                .WithODSTowns(_odsTowns)
                .WithODSEmployees(_odsEmployees)
                .WithBTNSave(_btnSave)
                .WithViewState(_iViewState)
                .WithSecurityService(_securityService)
                .WithIRequest(_iRequest);

            _container.Inject(_iDateTimeProvider);
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
        public void TestDetaultViewModeIsInsert()
        {
            using (_mocks.Record())
            {
                SetupResult.For(
                    _iViewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.CURRENT_MVP_MODE)).
                    Return(null);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(DetailViewMode.Insert, _target.CurrentMvpMode);
            }
        }

        [TestMethod]
        public void TestInnerDataSourceReturnsDataSource()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_dataSource, _target.InnerDataSource);
        }

        [TestMethod]
        public void TestInnerDetailControlReturnsDetailControl()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_detailControl, _target.InnerDetailControl);
        }

        [TestMethod]
        public void TestLatitudeReturnsValueOfHiddenLatitudeControl()
        {
            var expectedLatitude = 1.1;

            using (_mocks.Record())
            {
                SetupResult.For(_picker.Latitude).Return(expectedLatitude);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expectedLatitude.ToString(), _target.Latitude);
            }
        }

        [TestMethod]
        public void TestLongitudeReturnsValueOfHiddenLongitudeControl()
        {
            var expectedLongitude = 1.1;

            using (_mocks.Record())
            {
                SetupResult.For(_picker.Longitude).Return(expectedLongitude);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expectedLongitude.ToString(), _target.Longitude);
            }
        }

        [TestMethod]
        public void TextDataSourcePropertySetsDataSourceDataSource()
        {
            // namrock namrock. THIS NO MY PEE!!!!
            var expected = new object();

            using (_mocks.Record())
            {
                _dataSource.DataSource = expected;
            }

            using (_mocks.Playback())
            {
                _target.DataSource = expected;
            }
        } 

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

        [TestMethod]
        public void TestWorkOrderIDReturnsInnerDetailControlDataKeyWhenCurrentMvpModeIsEdit()
        {
            var workOrderID = 42;
            var dictionary = new OrderedDictionary {{ "Value", workOrderID }};
            var key = new DataKey(dictionary);
            var mode = DetailViewMode.Edit;
            using (_mocks.Record())
            {
                _target.SetMvpMode(mode);
                SetupResult.For(_detailControl.DataKey).Return(key);
                SetupResult.For(_iRequest.IQueryString).Return(_iQueryString);
                SetupResult.For(_iQueryString.GetValue("arg")).Return(null);

            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(_target.WorkOrderID, workOrderID);
            }
        }

        #endregion

        #region Exposed Method Tests

        [TestMethod]
        public void TestChangeMVPModeChangesMVPModeOfInnerDetailControl()
        {
            using (_mocks.Record())
            {
                _detailControl.ChangeMvpMode(DetailViewMode.Edit);
            }

            using (_mocks.Playback())
            {
                _target.ChangeMvpMode(DetailViewMode.Edit);
            }
        }

        [TestMethod]
        public void TestInsertItemCallsInsertItemOnInnerDetailControl()
        {
            var bools = new[] {
                true, false
            };

            foreach (var bl in bools)
            {
                using (_mocks.Record())
                {
                    _detailControl.InsertItem(bl);
                }

                using (_mocks.Playback())
                {
                    _target.InsertItem(bl);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestUpdateItemCallsUpdateItemOnInnerDetailControl()
        {
            var bools = new[] {
                true, false
            };

            foreach (var bl in bools)
            {
                using (_mocks.Record())
                {
                    _detailControl.UpdateItem(bl);
                }

                using (_mocks.Playback())
                {
                    _target.UpdateItem(bl);
                }

                _mocks.VerifyAll();
                _mocks.BackToRecordAll();
            }

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestDeleteItemThrowsNotImplementedException()
        {
            using (_mocks.Record())
            {
                DoNotExpect.Call(
                    () =>
                    _detailControl.DeleteItem());
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws<NotImplementedException>(
                    () =>
                    _target.DeleteItem());
            }
        }

        #endregion

        #region Event Handler Tests

        [TestMethod]
        public void TestPageLoadSetsMvpModeOfDetailControlAndTogglesButton()
        {
            _target = new TestWorkOrderInputFormViewBuilder()
                .WithViewState(_iViewState)
                .WithDetailControl(_detailControl)
                .WithBTNSave(_btnSave);

            foreach (DetailViewMode mode in Enum.GetValues(typeof(DetailViewMode)))
            {
                using (_mocks.Record())
                {
                    SetupResult.For(
                        _iViewState.GetValue(
                            WorkOrderDetailControlBase.ViewStateKeys.
                                CURRENT_MVP_MODE)).Return(mode);
                    _detailControl.ChangeMvpMode(mode);
                    _btnSave.Visible = (mode != DetailViewMode.ReadOnly);
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
        //public void TestPageLoadSetsSelectParametersOnDataSources()
        //{
        //    var expectedID = Utilities.GetCurrentOperatingCenterID().ToString();
        //    var townParam =
        //        new Parameter(
        //            WorkOrderInputFormView.EntityKeys.OPERATING_CENTER_ID);
        //    _odsTownsSelectParameters.Add(townParam);
        //    var employeeParam =
        //        new Parameter(
        //            WorkOrderInputFormView.EntityKeys.OPERATING_CENTER_ID);
        //    _odsEmployeesSelectParameters.Add(employeeParam);

        //    _mocks.ReplayAll();

        //    InvokeEventByName(_target, "Page_Load");

        //    Assert.AreEqual(expectedID, townParam.DefaultValue);
        //    Assert.AreEqual(expectedID, employeeParam.DefaultValue);
        //}

        [TestMethod]
        public void TestFVWorkOrderItemInsertingSetsValuesOnEventArgs()
        {
            var expectedCreatorID = 12345678;
            var expectedLatitude = 1.1;
            var expectedLongitude = 1.2;
            var args = new FormViewInsertEventArgs(null);

            SetupResult.For(_securityService.GetEmployeeID()).Return(
                expectedCreatorID);
            SetupResult.For(_picker.Latitude).Return(expectedLatitude);
            SetupResult.For(_picker.Longitude).Return(expectedLongitude);
            
            _mocks.ReplayAll();

            InvokeEventByName(_target, "fvWorkOrder_ItemInserting",
                new object[] {
                    null, args
                });

            Assert.AreEqual(expectedCreatorID,
                args.Values[WorkOrderInputFormView.EntityKeys.CREATOR_ID]);
            Assert.AreEqual(expectedLatitude.ToString(),
                args.Values[WorkOrderInputFormView.EntityKeys.LATITUDE]);
            Assert.AreEqual(expectedLongitude.ToString(),
                args.Values[WorkOrderInputFormView.EntityKeys.LONGITUDE]);
        }

        [TestMethod]
        public void TestFVWorkOrderItemUpdatingSetsValuesOnEventArgs()
        {
            var expectedLatitude = 1.1;
            var expectedLongitude = 1.2;
            var args = new FormViewUpdateEventArgs(null);

            SetupResult.For(_picker.Latitude).Return(expectedLatitude);
            SetupResult.For(_picker.Longitude).Return(expectedLongitude);

            _mocks.ReplayAll();

            InvokeEventByName(_target, "fvWorkOrder_ItemUpdating",
                new object[] {
                    null, args
                });

            Assert.AreEqual(expectedLatitude.ToString(),
                args.NewValues[WorkOrderInputFormView.EntityKeys.LATITUDE]);
            Assert.AreEqual(expectedLongitude.ToString(),
                args.NewValues[WorkOrderInputFormView.EntityKeys.LONGITUDE]);
        }

        [TestMethod]
        public void TestFVWorkOrderItemUpdatingSetsAlertStartedIfAlertIssuedAndWasNullAndAlertStartedNull()
        {
            var date = new DateTime(1976,8,30);
            SetupResult.For(_iDateTimeProvider.GetCurrentDate()).Return(date);
            var args = new FormViewUpdateEventArgs(null);
            args.NewValues[WorkOrderInputFormView.EntityKeys.ALERT_ISSUED] = true;
            args.OldValues[WorkOrderInputFormView.EntityKeys.ALERT_ISSUED] = "";
            args.OldValues[WorkOrderInputFormView.EntityKeys.ALERT_STARTED] = "";

            _mocks.ReplayAll();

            InvokeEventByName(_target, "fvWorkOrder_ItemUpdating",
                new object[] {
                    null, args
                });

            Assert.AreEqual(date, args.NewValues[WorkOrderInputFormView.EntityKeys.ALERT_STARTED]);
        }

        [TestMethod]
        public void TestFVWorkOrderItemUpdatingSetsAlertStartedIfAlertIssuedAndWasFalseAndAlertStartedNull()
        {
            var date = new DateTime(1976, 8, 30);
            SetupResult.For(_iDateTimeProvider.GetCurrentDate()).Return(date);
            var args = new FormViewUpdateEventArgs(null);
            args.OldValues[WorkOrderInputFormView.EntityKeys.ALERT_STARTED] = "";
            args.OldValues[WorkOrderInputFormView.EntityKeys.ALERT_ISSUED] = false;
            args.NewValues[WorkOrderInputFormView.EntityKeys.ALERT_ISSUED] = true;

            _mocks.ReplayAll();

            // prev issued is null, prev started is null
            InvokeEventByName(_target, "fvWorkOrder_ItemUpdating",
                new object[] {
                    null, args
                });

            Assert.AreEqual(date, args.NewValues[WorkOrderInputFormView.EntityKeys.ALERT_STARTED]);
        }

        [TestMethod]
        public void TestFVWorkOrderItemUpdatingDoesNotChangeAlertStartedIfAlertIssuedSetToTrueAndAlertStartedWasPreviouslySet()
        {
            var date = new DateTime(1976,8,30);
            var actualDate = new DateTime(2014, 6, 13).ToString();

            SetupResult.For(_iDateTimeProvider.GetCurrentDate()).Return(date);
            var args = new FormViewUpdateEventArgs(null);
            args.NewValues[WorkOrderInputFormView.EntityKeys.ALERT_ISSUED] = true;
            args.OldValues[WorkOrderInputFormView.EntityKeys.ALERT_ISSUED] = "";  //because forms have these as empty strings.
            args.OldValues[WorkOrderInputFormView.EntityKeys.ALERT_STARTED] = actualDate;
            args.NewValues[WorkOrderInputFormView.EntityKeys.ALERT_STARTED] = actualDate;

            _mocks.ReplayAll();

            // prev issued is null, prev started is null
            InvokeEventByName(_target, "fvWorkOrder_ItemUpdating",
                new object[] {
                    null, args
                });

            Assert.AreEqual(actualDate, args.NewValues[WorkOrderInputFormView.EntityKeys.ALERT_STARTED]);
        }

        [TestMethod]
        public void TestObjectDataSourceDeletedCommandFiresDeletingEvent()
        {
            _mocks.ReplayAll();

            var called = false;
            _target = new TestWorkOrderInputFormViewBuilder()
                .WithDeleteHandler((sender, e) => called = true);
            
            InvokeEventByName(_target, "ods_Deleted");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void TestObjectDataSourceUpdatedCommandFiresUpdatingEvent()
        {
            _mocks.ReplayAll();

            _target = new TestWorkOrderInputFormViewBuilder()
                .WithUpdateHandler((sender, e) => _called = true);

            InvokeEventByName(_target, "ods_Updated");

            Assert.IsTrue(_called);
        }

        [TestMethod]
        public void TestObjectDataSourceInsertedCommandFiresInsertingEvent()
        {
            _mocks.ReplayAll();

            var called = false;
            _target = new TestWorkOrderInputFormViewBuilder()
                .WithInsertHandler((sender, e) => called = true);

            InvokeEventByName(_target, "ods_Inserted");

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void TestBTNSaveClickCallsUpdateItemOnDetailControlWhenCurrentModeIsInsert()
        {
            // if the setup of WorkOrderDetailControlBase#CurrentMvpMode ever changes,
            // this will break.
            _target.InitialMode = DetailViewMode.Insert;

            using (_mocks.Record())
            {
                _detailControl.InsertItem(true);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnSave_Click");
            }
        }

        [TestMethod]
        public void TestBTNSaveClickCallsInsertItemOnDetailControlWhenCurrentModeIsEdit()
        {
            // if the setup of WorkOrderDetailControlBase#CurrentMvpMode ever changes,
            // this will break.
            _target.InitialMode = DetailViewMode.Edit;

            using (_mocks.Record())
            {
                _detailControl.UpdateItem(true);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnSave_Click");
            }
        }

        [TestMethod]
        public void TestDdlOperatingCenterDataBoundSetsSelectedIndexToOneIfOnlyOneOperatingCenter()
        {
            var ddlOperatingCenters = _mocks.CreateMock<IDropDownList>();
            
            using (_mocks.Record())
            {
                SetupResult.For(
                    _detailControl.FindIControl<IDropDownList>(
                        "ddlOperatingCenter")).Return(ddlOperatingCenters);
                SetupResult.For(_securityService.UserOperatingCentersCount).
                    Return(1);
                ddlOperatingCenters.SelectedIndex = 1;
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlOperatingCenter_DataBound");
            }
        }

        [TestMethod]
        public void TestBtnRemoveContractorAssignmentSetsAssignedContractorIDNull()
        {
            const int workOrderID = 5;
            var wo = new WorkOrder() {
                WorkOrderID = workOrderID, 
                AssignedContractorID = 5
            };
            SetupResult.For(_repository.Get(wo.WorkOrderID)).Return(wo);
            _container.Inject(_repository);
            var ph = _mocks.DynamicMock<PlaceHolder>();

            using (_mocks.Record())
            {
                SetupResult.For(
                    _detailControl.FindControl<PlaceHolder>(
                        "phContractorAssigned")).Return(ph);
                SetupResult.For(
                    _iViewState.GetValue(
                        WorkOrderDetailControlBase.ViewStateKeys.WORK_ORDER_ID))
                    .Return(workOrderID);
                wo.AssignedContractorID = null;
                _repository.UpdateCurrentEntityLiterally(wo);
                ph.Visible = false;
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "btnRemoveContractorAssignment_Click");
            }
        }

        #endregion
    }

    internal class TestWorkOrderInputFormViewBuilder : TestDataBuilder<TestWorkOrderInputFormView>
    {
        #region Private Members

        private ISecurityService _securityService;
        private IViewState _iViewState;
        private IButton _btnSave;
        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;
        private IObjectDataSource _odsTowns, _odsEmployees;
        private EventHandler<ObjectContainerDataSourceStatusEventArgs>
            _onDeleting, _onInserting, _onUpdating;
        private IRequest _iRequest;

        #endregion

        #region Private Methods

        private void View_OnDispose(TestWorkOrderInputFormView view)
        {
            if (_onDeleting != null)
                view.Deleting -= _onDeleting;
            if (_onUpdating != null)
                view.Updating -= _onUpdating;
            if (_onInserting != null)
                view.Inserting -= _onInserting;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrderInputFormView Build()
        {
            var obj = new TestWorkOrderInputFormView();
            if (_detailControl != null)
                obj.SetDetailControl(_detailControl);
            if (_dataSource != null)
                obj.SetDataSource(_dataSource);
            if (_odsTowns != null)
                obj.SetODSTowns(_odsTowns);
            if (_odsEmployees != null)
                obj.SetODSEmployees(_odsEmployees);
            if (_btnSave != null)
                obj.SetBTNSave(_btnSave);
            if (_iViewState != null)
                obj.SetIViewState(_iViewState);
            if (_onDeleting != null)
                obj.Deleting += _onDeleting;
            if (_onUpdating != null)
                obj.Updating += _onUpdating;
            if (_onInserting != null)
                obj.Inserting += _onInserting;
            if (_securityService !=null)
                obj.SetSecurityService(_securityService);
            if (_iRequest != null)
                SetIRequest(obj);
            obj._onDispose += View_OnDispose;
            return obj;
        }

        private void SetIRequest(TestWorkOrderInputFormView view)
        {
            SetFieldValue(view, "_iRequest", _iRequest);
        }

        public TestWorkOrderInputFormViewBuilder WithDetailControl(IDetailControl detailControl)
        {
            _detailControl = detailControl;
            return this;
        }

        public TestWorkOrderInputFormViewBuilder WithDataSource(IObjectContainerDataSource source)
        {
            _dataSource = source;
            return this;
        }

        public TestWorkOrderInputFormViewBuilder WithDeleteHandler(EventHandler<ObjectContainerDataSourceStatusEventArgs> onDeleting)
        {
            _onDeleting = onDeleting;
            return this;
        }

        public TestWorkOrderInputFormViewBuilder WithInsertHandler(EventHandler<ObjectContainerDataSourceStatusEventArgs> onInserting)
        {
            _onInserting = onInserting;
            return this;
        }

        public TestWorkOrderInputFormViewBuilder WithUpdateHandler(EventHandler<ObjectContainerDataSourceStatusEventArgs> onUpdating)
        {
            _onUpdating = onUpdating;
            return this;
        }

        public TestWorkOrderInputFormViewBuilder WithODSTowns(IObjectDataSource source)
        {
            _odsTowns = source;
            return this;
        }

        public TestWorkOrderInputFormViewBuilder WithODSEmployees(IObjectDataSource source)
        {
            _odsEmployees = source;
            return this;
        }

        public TestWorkOrderInputFormViewBuilder WithBTNSave(IButton button)
        {
            _btnSave = button;
            return this;
        }

        public TestWorkOrderInputFormViewBuilder WithViewState(IViewState state)
        {
            _iViewState = state;
            return this;
        }

        public TestWorkOrderInputFormViewBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        public TestWorkOrderInputFormViewBuilder WithIRequest(IRequest request)
        {
            _iRequest = request;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderInputFormView : WorkOrderInputFormView
    {
        #region Private Members

        private DetailViewMode? _currentMvpMode;

        #endregion

        #region Properties

        public override DetailViewMode CurrentMvpMode
        {
            get
            {
                return _currentMvpMode ?? base.CurrentMvpMode;
            }
            protected set
            {
                base.CurrentMvpMode = value;
            }
        }

        #endregion

        #region Delegates

        internal delegate void OnDisposeHandler(TestWorkOrderInputFormView view);

        #endregion

        #region Events

        internal OnDisposeHandler _onDispose;

        #endregion
        
        #region Exposed Methods

        public void SetDetailControl(IDetailControl detailControl)
        {
            fvWorkOrder = detailControl;
        }

        public void SetDataSource(IObjectContainerDataSource source)
        {
            odsWorkOrder = source;
        }

        public override void Dispose()
        {
            base.Dispose();
            if (_onDispose != null)
                _onDispose(this);
        }

        public void SetODSTowns(IObjectDataSource source)
        {
            odsTowns = source;
        }

        public void SetODSEmployees(IObjectDataSource source)
        {
            odsEmployees = source;
        }

        public void SetBTNSave(IButton button)
        {
            btnSave = button;
        }

        public void SetIViewState(IViewState state)
        {
            _iViewState = state;
        }

        public void SetSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;    
        }

        public void SetMvpMode(DetailViewMode mode)
        {
            _currentMvpMode = mode;
        }

        #endregion
    }
}
