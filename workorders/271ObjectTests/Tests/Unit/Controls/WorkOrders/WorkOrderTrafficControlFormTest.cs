using System;
using System.Security.Principal;
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
    public class WorkOrderTrafficControlFormTest : EventFiringTestClass
    {
        #region Private Members

        private TestWorkOrderTrafficControlForm _target;
        private IRepository<WorkOrder> _repository;
        private IDetailControl _detailControl;
        private IObjectContainerDataSource _dataSource;
        private IPage _iPage;
        private IViewState _iViewState;
        private ITextBox _txtAppendNotes;
        private ILabel _lblCurrentNotes;
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

            _mocks.DynamicMock(out _repository)
                  .DynamicMock(out _detailControl)
                  .DynamicMock(out _dataSource)
                  .DynamicMock(out _iPage)
                  .DynamicMock(out _iViewState)
                  .DynamicMock(out _txtAppendNotes)
                  .DynamicMock(out _lblCurrentNotes)
                  .DynamicMock(out _securityService)
                  .DynamicMock(out _user);

            _target = new TestWorkOrderTrafficControlFormBuilder()
                .WithDetailControl(_detailControl)
                .WithDataSource(_dataSource)
                .WithPage(_iPage)
                .WithViewState(_iViewState)
                .WithTXTAppendNotes(_txtAppendNotes)
                .WithLBLCurrentNotes(_lblCurrentNotes)
                .WithSecurityService(_securityService);

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

            SetupResult.For(
                _detailControl.FindIControl<ITextBox>(
                    WorkOrderAdditionalFinalizationInfoForm.ControlIDs.
                        APPEND_NOTES_BOX)).Return(_txtAppendNotes);
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
        public void TestObjectDataSourceUpdatedCommandFiresUpdatingEvent()
        {
            _mocks.ReplayAll();

            _target =
                new TestWorkOrderTrafficControlFormBuilder()
                    .WithUpdateHandler((sender, e) => _called = true);

            InvokeEventByName(_target, "ods_Updated");

            Assert.IsTrue(_called);
        }

        [TestMethod]
        public void TestBtnSaveClickCallsUpdateItemOnDetailControl()
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

        [TestMethod]
        public void TestFormViewItemUpdatingSetsApprovalDateApprovedBy()
        {
            string userName = "User",
                   currentNotes = "These are the current notes.",
                   appendNotes = "These are the notes to append.",
                   newNotes;
            var args = new FormViewUpdateEventArgs(null);

            using (_mocks.Record())
            {
                SetupResult.For(_txtAppendNotes.Text).Return(appendNotes);
                SetupResult.For(_lblCurrentNotes.Text).Return(currentNotes);
                SetupResult.For(_user.Name).Return(userName);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "fvWorkOrder_ItemUpdating",
                    new object[] {
                        null, args
                    });

                newNotes =
                    args.NewValues[
                        WorkOrderTrafficControlForm.WorkOrderParameterNames.NOTES].ToString();

                Assert.IsTrue(newNotes.StartsWith(currentNotes), "Existing notes have been deleted");
                Assert.IsTrue(newNotes.Contains(userName), "User name was not applied");
                Assert.IsTrue(newNotes.Contains(appendNotes), "New notes were not applied");
            }
        }

        #endregion
    }

    internal class TestWorkOrderTrafficControlFormBuilder : TestDataBuilder<TestWorkOrderTrafficControlForm>
    {
        #region Private Members

        private IDetailControl _fvWorkOrder;
        private IObjectContainerDataSource _odsWorkOrder;
        private IPage _iPage;
        private IViewState _iViewState;
        private EventHandler<ObjectContainerDataSourceStatusEventArgs> _onUpdating;
        private ILabel _lblCurrentNotes;
        private ITextBox _txtAppendNotes;
        private ISecurityService _securityService;

        #endregion

        #region Private Methods

        private void View_OnDispose(TestWorkOrderTrafficControlForm ctrl)
        {
            if (_onUpdating != null)
                ctrl.Updating -= _onUpdating;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrderTrafficControlForm Build()
        {
            var obj = new TestWorkOrderTrafficControlForm();
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
            if (_txtAppendNotes != null)
                obj.SetTXTAppendNotes(_txtAppendNotes);
            if (_lblCurrentNotes != null)
                obj.SetLBLCurrentNotes(_lblCurrentNotes);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            obj._onDispose += View_OnDispose;

            return obj;
        }

        public TestWorkOrderTrafficControlFormBuilder WithDetailControl(IDetailControl fvWorkOrder)
        {
            _fvWorkOrder = fvWorkOrder;
            return this;
        }

        public TestWorkOrderTrafficControlFormBuilder WithDataSource(IObjectContainerDataSource dataSource)
        {
            _odsWorkOrder = dataSource;
            return this;
        }

        public TestWorkOrderTrafficControlFormBuilder WithPage(IPage page)
        {
            _iPage = page;
            return this;
        }

        public TestWorkOrderTrafficControlFormBuilder WithViewState(IViewState viewState)
        {
            _iViewState = viewState;
            return this;
        }

        public TestWorkOrderTrafficControlFormBuilder WithUpdateHandler(EventHandler<ObjectContainerDataSourceStatusEventArgs> handler)
        {
            _onUpdating = handler;
            return this;
        }

        public TestWorkOrderTrafficControlFormBuilder WithTXTAppendNotes(ITextBox notes)
        {
            _txtAppendNotes = notes;
            return this;
        }

        public TestWorkOrderTrafficControlFormBuilder WithLBLCurrentNotes(ILabel notes)
        {
            _lblCurrentNotes = notes;
            return this;
        }

        public TestWorkOrderTrafficControlForm WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderTrafficControlForm : WorkOrderTrafficControlForm
    {
        #region Delegates

        internal delegate void OnDisposeHandler(TestWorkOrderTrafficControlForm view);

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

        public override void Dispose()
        {
            base.Dispose();

            if (_onDispose != null)
                _onDispose(this);
        }
        
        public void SetTXTAppendNotes(ITextBox notes)
        {
            _txtAppendNotes = notes;
        }

        public void SetLBLCurrentNotes(ILabel notes)
        {
            _lblCurrentNotes = notes;
        }

        public void SetSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        #endregion
    }
}
