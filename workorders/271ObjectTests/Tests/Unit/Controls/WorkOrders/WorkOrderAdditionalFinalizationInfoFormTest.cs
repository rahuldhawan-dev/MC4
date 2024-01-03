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
using MMSINC.Utilities;
using MMSINC.Utilities.StructureMap;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;
using StructureMap;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using Employee = WorkOrders.Model.Employee;
using Hydrant = WorkOrders.Model.Hydrant;
using WorkDescription = WorkOrders.Model.WorkDescription;
using WorkOrder = WorkOrders.Model.WorkOrder;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderAdditionalFinalizationInfoFormTest.
    /// </summary>
    [TestClass]
    public class WorkOrderAdditionalFinalizationInfoFormTest : EventFiringTestClass
    {
        #region Private Members

        private TestWorkOrderAdditionalFinalizationInfoForm _target;
        private IRepository<WorkOrder> _repository;
        private IRepository<WorkOrderDescriptionChange> _descriptionChangeRepository;
        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;
        private IDateTimeProvider _dateTimeProvider;
        private ILabel _lblCurrentNotes;
        private ITextBox _txtAppendNotes;
        private IDropDownList _ddlFinalWorkDescription;
        private IPage _iPage;
        private IViewState _iViewState;
        private ISecurityService _securityService;
        private IUser _user;
        private IContainer _container;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _container = new Container();
            _mocks
                .DynamicMock(out _repository)
                .DynamicMock(out _descriptionChangeRepository)
                .DynamicMock(out _detailControl)
                .DynamicMock(out _dataSource)
                .DynamicMock(out _lblCurrentNotes)
                .DynamicMock(out _txtAppendNotes)
                .DynamicMock(out _ddlFinalWorkDescription)
                .DynamicMock(out _iPage)
                .DynamicMock(out _iViewState)
                .DynamicMock(out _securityService)
                .DynamicMock(out _dateTimeProvider)
                .DynamicMock(out _user);

            _target = new TestWorkOrderAdditionalFinalizationInfoFormBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource)
                .WithPage(_iPage)
                .WithViewState(_iViewState)
                .WithSecurityService(_securityService);

            _container.Inject(_dateTimeProvider);

            SetupResult.For(_securityService.CurrentUser).Return(_user);
            
            SetupFormView();

            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();

            _target.Dispose();
        }

        private void SetupFormView()
        {
            SetupResult.For(
                _detailControl.FindIControl<ILabel>(
                    WorkOrderAdditionalFinalizationInfoForm.ControlIDs.
                        CURRENT_NOTES_LABEL)).Return(_lblCurrentNotes);

            SetupResult
                .For(
                _detailControl.FindIControl<ITextBox>(
                    WorkOrderAdditionalFinalizationInfoForm.ControlIDs.
                        APPEND_NOTES_BOX))
                .Return(_txtAppendNotes);
            SetupResult
                .For(
                _detailControl.FindIControl<IDropDownList>(
                    WorkOrderAdditionalFinalizationInfoForm.ControlIDs.
                        WORK_DESCRIPTION_SELECT))
                .Return(_ddlFinalWorkDescription);
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

        [TestMethod]
        public void TestAppendNotesPropertyReturnsTextValueFromNotesTextBox()
        {
            var expected = "These are some new notes.";

            using (_mocks.Record())
            {
                SetupResult.For(_txtAppendNotes.Text).Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.IsTrue(_target.AppendNotes.Contains(expected));
            }
        }

        [TestMethod]
        public void TestFinalWorkDescriptionIDPropertyReturnsIntegerValueFromFinalWorkDescriptionDropDown()
        {
            var expected = 666;

            using (_mocks.Record())
            {
                SetupResult
                    .For(_ddlFinalWorkDescription.GetSelectedValue())
                    .Return(expected);
            }

            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.FinalWorkDescriptionID);
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
        public void TestFVWorkOrderItemUpdatingAppendsNotesIfUserEnteredNotes()
        {
            string userName = "Encargado del queso",
                   currentNotes = "These are the current notes.",
                   appendNotes = "These are the notes to append.",
                   newNotes;
            int workDescriptionID = 1;
            var args = new FormViewUpdateEventArgs(null);
            args.OldValues.Add("WorkDescriptionID", workDescriptionID);

            using (_mocks.Record())
            {
                SetupResult.For(_txtAppendNotes.Text).Return(appendNotes);
                SetupResult.For(_lblCurrentNotes.Text).Return(currentNotes);
                SetupResult.For(_ddlFinalWorkDescription.GetSelectedValue())
                    .Return(workDescriptionID);
                SetupResult.For(_user.Name).Return(userName);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "fvWorkOrder_ItemUpdating", null, args);

                newNotes =
                    args.NewValues[
                        WorkOrderAdditionalFinalizationInfoForm.WorkOrderParameterNames.NOTES].ToString();

                Assert.IsTrue(newNotes.Contains(userName), "new notes does not contain user name");
                Assert.IsTrue(newNotes.Contains(appendNotes), "new notes does not contain the notes to append");
            }
        }

        [TestMethod]
        public void TestFVWorkOrderItemUpdatingCreatesAndSavesNewWorkOrderWorkDescriptionChangeRecordIfNewDescriptionChosen()
        {
            int workOrderID = 666,
                fromWorkDescriptionID = 1,
                toWorkDescriptionID = 2,
                responsibleEmployeeID = 3;
            var args = new FormViewUpdateEventArgs(null);
            args.OldValues.Add("WorkDescriptionID", fromWorkDescriptionID);
            _container.Inject(_descriptionChangeRepository);

            using (_mocks.Record())
            {
                SetupResult.For(_txtAppendNotes.Text).Return(String.Empty);
                SetupResult.For(_ddlFinalWorkDescription.GetSelectedValue())
                    .Return(toWorkDescriptionID);
                SetupResult.For(_securityService.Employee)
                    .Return(new Employee {
                        EmployeeID = responsibleEmployeeID
                    });
                SetupResult.For(_iViewState.GetValue("WorkOrderID"))
                    .Return(workOrderID);
                Expect
                    .Call(
                    () => _descriptionChangeRepository.InsertNewEntity(null))
                    .IgnoreArguments()
                    .Constraints(Property.Value("WorkOrderID", workOrderID) &&
                                 Property.Value("ToWorkDescriptionID",toWorkDescriptionID) &&
                                 Property.Value("FromWorkDescriptionID",fromWorkDescriptionID) &&
                                 Property.Value("ResponsibleEmployeeID",responsibleEmployeeID));
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "fvWorkOrder_ItemUpdating",
                    new object[] {
                        null, args
                    });
            }
        }

        [TestMethod]
        public void
            TestFVWorkOrderItemUpdatingSetsInitialFlushTimeEnteredByAndAtWhenInitialFlushTimeEntered()
        {
            int workDescriptionID = 1;
            var args = new FormViewUpdateEventArgs(null) {
                OldValues = {
                    [nameof(WorkOrder.WorkDescriptionID)] = workDescriptionID
                },
                NewValues = {
                    [nameof(WorkOrder.InitialServiceLineFlushTime)] = 42
                }
            };
            var expectedEnteredById = 1234;
            var expectedEnteredAt = DateTime.Now;

            using (_mocks.Record())
            {
                SetupResult.For(_ddlFinalWorkDescription.GetSelectedValue()).Return(workDescriptionID);
                SetupResult.For(_securityService.GetEmployeeID()).Return(expectedEnteredById);
                SetupResult.For(_dateTimeProvider.GetCurrentDate()).Return(expectedEnteredAt);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "fvWorkOrder_ItemUpdating", null, args);

                Assert.AreEqual(
                    args.NewValues[nameof(WorkOrder.InitialFlushTimeEnteredById)],
                    expectedEnteredById);
                Assert.AreEqual(
                    args.NewValues[nameof(WorkOrder.InitialFlushTimeEnteredAt)],
                    expectedEnteredAt);
            }
        }

        [TestMethod]
        public void TestLBUpdateCallsUpdateItemOnDetailControl()
        {
            using (_mocks.Record())
            {
                _detailControl.UpdateItem(true);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "lbUpdate_Click");
            }
        }

        [TestMethod]
        public void TestObjectDataSourceUpdatedCommandFiresUpdatingEvent()
        {
            _mocks.ReplayAll();

            _target =
                new TestWorkOrderAdditionalFinalizationInfoFormBuilder()
                    .WithUpdateHandler((sender, e) => _called = true);

            InvokeEventByName(_target, "ods_Updated");

            Assert.IsTrue(_called);
        }

        [TestMethod]
        public void TestUpdateDetailControlFiresFvWorkOrderUpdateItem()
        {
            using (_mocks.Record())
            {
                _detailControl.UpdateItem(true);
            }
            using (_mocks.Playback())
            {
                _target.UpdateDetailControl();
            }
        }

        [TestMethod]
        public void TestFinalWorkDescriptionOnDataBindingAddsItemIfMissing()
        {
            var items = new ListItemCollection();
            var wd = new WorkDescription { WorkDescriptionID = 1, Description = "Foo" };
            var wo = new WorkOrder { WorkDescription = wd };
            using (_mocks.Record())
            {
                SetupResult.For(_ddlFinalWorkDescription.SelectedValue).Return(wd.WorkDescriptionID.ToString());
                SetupResult.For(_ddlFinalWorkDescription.DataItem).Return(wo);
                SetupResult.For(_ddlFinalWorkDescription.Items).Return(items);
                _ddlFinalWorkDescription.Stub(x => x.DataBind())
                    .Throw(new ArgumentOutOfRangeException());
            }
            using (_mocks.Playback())
            {
                var args = new EventArgs();
                InvokeEventByName(_target, "ddlFinalWorkDescription_OnDataBinding",
                    new object[] {
                        _ddlFinalWorkDescription, args
                    });
                Assert.IsNotNull(items.FindByValue(wo.WorkDescriptionID.ToString()));
            }
        }

        [TestMethod]
        public void TestDataBoundSetsFinalizationLinkVisibleWhenAssetIsNotRetiredCancelledOrRemoved()
        {
            var hl = new HyperLink();
            var args = new EventArgs();
            var hydrant = new Hydrant { AssetStatusID = 1 };
            var workOrder = new WorkOrder { Hydrant = hydrant, AssetTypeID = MapCall.Common.Model.Entities.AssetType.Indices.HYDRANT, WorkOrderID = 1 };
            
            using (_mocks.Record())
            {
                SetupResult.For(_iPage.IRequest.RelativeUrl).Return("Foo");
                SetupResult.For(_detailControl.DataItem).Return(workOrder);
                SetupResult.For(_detailControl.FindControl<HyperLink>("hlFinalization")).Return(hl);
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "fvWorkOrder_DataBound", new object[] { _detailControl, args });
                Assert.IsTrue(hl.Visible);
            }
        }

        [TestMethod]
        public void TestDataBoundSetsFinalizationLinkToNotVisibleWhenAssetIsRetiredCancelledOrRemoved()
        {
            var hl = new HyperLink();
            var args = new EventArgs();
            var hydrant = new Hydrant { AssetStatusID = MapCall.Common.Model.Entities.AssetStatus.Indices.CANCELLED};
            var workOrder = new WorkOrder { Hydrant = hydrant, AssetTypeID = MapCall.Common.Model.Entities.AssetType.Indices.HYDRANT, WorkOrderID = 1 };
            
            using (_mocks.Record())
            {
                SetupResult.For(_iPage.IRequest.RelativeUrl).Return("Foo");
                SetupResult.For(_detailControl.DataItem).Return(workOrder);
                SetupResult.For(_detailControl.FindControl<HyperLink>("hlFinalization")).Return(hl);
            }
            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "fvWorkOrder_DataBound", new object[] { _detailControl, args });
                Assert.IsFalse(hl.Visible);
            }
        }

        [TestMethod]
        public void TestDataBoundSetsFinalizationLinkToNotVisibleWhenCurrentUrlIsFinalization()
        {
            var hl = new HyperLink();
            var args = new EventArgs();
            var hydrant = new Hydrant { AssetStatusID = MapCall.Common.Model.Entities.AssetStatus.Indices.CANCELLED };
            var workOrder = new WorkOrder { Hydrant = hydrant, AssetTypeID = MapCall.Common.Model.Entities.AssetType.Indices.HYDRANT, WorkOrderID = 1 };

            using (_mocks.Record())
            {
                SetupResult.For(_iPage.IRequest.RelativeUrl).Return("WorkOrderFinalizationResourceRPCPage");
                SetupResult.For(_detailControl.DataItem).Return(workOrder);
                SetupResult.For(_detailControl.FindControl<HyperLink>("hlFinalization")).Return(hl);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "fvWorkOrder_DataBound", new object[] { _detailControl, args });
                Assert.IsFalse(hl.Visible);
            }
        }
        #endregion
    }

    internal class TestWorkOrderAdditionalFinalizationInfoFormBuilder : TestDataBuilder<TestWorkOrderAdditionalFinalizationInfoForm>
    {
        #region Private Members

        private IDetailControl _fvWorkOrder;
        private IObjectContainerDataSource _odsWorkOrder;
        private IPage _iPage;
        private IViewState _iViewState;
        private EventHandler<ObjectContainerDataSourceStatusEventArgs> _onUpdating;
        private ISecurityService _securityService;

        #endregion

        #region Private Methods

        private void View_OnDispose(TestWorkOrderAdditionalFinalizationInfoForm ctrl)
        {
            if (_onUpdating != null)
                ctrl.Updating -= _onUpdating;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrderAdditionalFinalizationInfoForm Build()
        {
            var obj = new TestWorkOrderAdditionalFinalizationInfoForm();
            if (_fvWorkOrder != null)
                obj.SetFVWorkOrder(_fvWorkOrder);
            if (_odsWorkOrder != null)
                obj.SetODSWorkOrder(_odsWorkOrder);
            if (_iPage != null)
                obj.SetPage(_iPage);
            if (_iViewState != null)
                obj.SetViewState(_iViewState);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            if (_onUpdating != null)
                obj.Updating += _onUpdating;
            obj._onDispose += View_OnDispose;
            return obj;
        }

        public TestWorkOrderAdditionalFinalizationInfoFormBuilder WithDetailControl(IDetailControl fvWorkOrder)
        {
            _fvWorkOrder = fvWorkOrder;
            return this;
        }

        public TestWorkOrderAdditionalFinalizationInfoFormBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _odsWorkOrder = dataSource;
            return this;
        }

        public TestWorkOrderAdditionalFinalizationInfoFormBuilder WithPage(IPage page)
        {
            _iPage = page;
            return this;
        }

        public TestWorkOrderAdditionalFinalizationInfoFormBuilder WithViewState(IViewState viewState)
        {
            _iViewState = viewState;
            return this;
        }

        public TestWorkOrderAdditionalFinalizationInfoFormBuilder WithUpdateHandler(EventHandler<ObjectContainerDataSourceStatusEventArgs> handler)
        {
            _onUpdating = handler;
            return this;
        }

        public TestWorkOrderAdditionalFinalizationInfoFormBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderAdditionalFinalizationInfoForm : WorkOrderAdditionalFinalizationInfoForm
    {
        #region Delegates

        internal delegate void OnDisposeHandler(TestWorkOrderAdditionalFinalizationInfoForm view);

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

        protected void fvWorkOrder_DataBound(object sender, EventArgs e)
        {
            base.fvWorkOrder_OnDataBound(sender, e);
        }
        
        #endregion
    }

    internal class MockFormViewUpdateEventArgs : FormViewUpdateEventArgs
    {
        #region Constructors

        public MockFormViewUpdateEventArgs(object commandArgument) : base(commandArgument)
        {
        }

        #endregion
    }
}
