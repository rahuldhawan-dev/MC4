using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Presenters.WorkOrders;
using WorkOrders.Views.WorkOrders;

namespace _271ObjectTests.Tests.Unit.Presenters.WorkOrders
{
    /// <summary>
    /// Summary description for WorkOrderResourceRPCPresenterTest.
    /// </summary>
    [TestClass]
    public class WorkOrderResourceRPCPresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IDetailView<WorkOrder> _detailView;
        private IWorkOrderResourceRPCView _view;
        private IRepository<WorkOrder> _repository;
        private TestWorkOrderResourceRPCPresenter _target;
        private ISecurityService _securityService;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _repository)
                .DynamicMock(out _detailView)
                .DynamicMock(out _securityService);

            _target = new TestWorkOrderResourceRPCPresenterBuilder(_view, _repository)
                .WithDetailView(_detailView)
                .WithSecurityService(_securityService);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestResourceViewPropertyReturnsViewCastAsIWorkOrderResourceRPCView()
        {
            Assert.AreSame(_view, _target.ResourceView);
            Assert.IsInstanceOfType(_target.ResourceView,
                typeof(IWorkOrderResourceRPCView));

            _mocks.ReplayAll();
        }

        #endregion

        #region Method Tests

        [TestMethod]
        public void TestSetRepositoryDataKeyFromListViewDataKeyDoesNothing()
        {
            _mocks
                .CreateMock(out _view)
                .CreateMock(out _repository);

            _target = new TestWorkOrderResourceRPCPresenterBuilder(_view, _repository);

            using (_mocks.Record())
            {
                // EXPECT NOTHING!!!!
            }

            using (_mocks.Playback())
            {
                _target.SetRepositoryDataKeyFromListViewDataKey();
            }
        }

        #endregion

        [TestMethod]
        public void TestInheritsFromBaseWorkOrdersResourceRPCPresenter()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(_target,
                typeof(WorkOrdersResourceRPCPresenter<WorkOrder>),
                "ResourcePresenters in this project should inherit from WorkOrdersResourceRPCPresenter, lest bad tings happen.");
        }

        [TestMethod]
        public void TestDetailViewEditCommandChangesViewCommandToUpdate()
        {
            // using full mocks to ensure that only what's expected happens
            _mocks
                .CreateMock(out _view)
                .CreateMock(out _repository);

            _target = new TestWorkOrderResourceRPCPresenterBuilder(_view,
                _repository);

            string currentUrl = RPCQueryStringValues.COMMAND + "=" +
                                RPCCommandNames.VIEW,
                   expectedUrl = RPCQueryStringValues.COMMAND + "=" +
                                 RPCCommandNames.UPDATE;

            using (_mocks.Record())
            {
                SetupResult.For(_view.Command).Return(RPCCommandNames.VIEW);
                SetupResult.For(_view.RelativeUrl).Return(currentUrl);
                _view.Redirect(expectedUrl);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "DetailView_EditClicked");
            }
        }

        [TestMethod]
        public void TestNoCommandRedirectsToWorkOrderGeneralResourceView()
        {
            _mocks
                .CreateMock(out _view)
                .CreateMock(out _repository);

            _target = new TestWorkOrderResourceRPCPresenterBuilder(_view,
                _repository);

            using (_mocks.Record())
            {
                SetupResult.For(_view.Command).Return(null);
                _view.Redirect("WorkOrderGeneralResourceView.aspx");
            }

            using (_mocks.Playback())
            {
                _target.ExposedProcessCommandAndArgument();
            }

        }

        [TestMethod]
        public void TestCheckUserSecurityThrowsExceptionForNonAdminWhenPhaseIsGeneralAndCommandIsUpdateCreateOrDelete()
        {
            var user = _mocks.DynamicMock<IUser>();

            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                foreach (RPCCommands command in Enum.GetValues(typeof(RPCCommands)))
                {
                    using (_mocks.Record())
                    {
                        SetupResult.For(_securityService.CurrentUser).Return(
                            user);
                        SetupResult.For(_securityService.UserHasAccess).Return(
                            true);
                        SetupResult.For(_securityService.IsAdmin).Return(false);
                        SetupResult.For(_view.Phase).Return(phase);
                        SetupResult.For(_view.Command).Return(command.ToString());
                        SetupResult.For(_view.RPCCommand).Return(command);
                        SetupResult.For(_view.Argument).Return("foo");
                    }
                    using (_mocks.Playback())
                    {
                        if (phase == WorkOrderPhase.General && 
                                (command == RPCCommands.Create || 
                                 command == RPCCommands.Delete || 
                                 command == RPCCommands.Update))
                            MyAssert.Throws(
                                () => _target.ExposedCheckUserSecurity(),
                                typeof(UnauthorizedAccessException), string.Format("{0}:{1}", command, phase));
                        else
                            MyAssert.DoesNotThrow(
                                () => _target.ExposedCheckUserSecurity(),
                                typeof(UnauthorizedAccessException), string.Format("{0}:{1}", command, phase));
                    }
                    _mocks.VerifyAll();
                    _mocks.BackToRecordAll();
                }
            }
            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestCheckUserSecurityDoesNotThrowExceptionForAdminWhenPhaseIsGeneralAndCommandIsUpdateCreateOrDelete()
        {
            var user = _mocks.DynamicMock<IUser>();

            foreach (WorkOrderPhase phase in Enum.GetValues(typeof(WorkOrderPhase)))
            {
                foreach (RPCCommands command in Enum.GetValues(typeof(RPCCommands)))
                {
                    using (_mocks.Record())
                    {
                        SetupResult.For(_securityService.CurrentUser).Return(
                            user);
                        SetupResult.For(_securityService.UserHasAccess).Return(
                            true);
                        SetupResult.For(_securityService.IsAdmin).Return(true);
                        SetupResult.For(_view.Phase).Return(phase);
                        SetupResult.For(_view.Command).Return(command.ToString());
                    }
                    using (_mocks.Playback())
                    {
                        MyAssert.DoesNotThrow(
                            () => _target.ExposedCheckUserSecurity(),
                            typeof(UnauthorizedAccessException));
                    }
                    _mocks.VerifyAll();
                    _mocks.BackToRecordAll();
                }
            }
            _mocks.ReplayAll();
        }
    }

    internal class TestWorkOrderResourceRPCPresenterBuilder : TestDataBuilder<TestWorkOrderResourceRPCPresenter>
    {
        #region Private Members

        private IDetailView<WorkOrder> _detailView;
        private readonly IResourceRPCView<WorkOrder> _view;
        private readonly IRepository<WorkOrder> _repository;
        private ISecurityService _securityService;

        #endregion

        #region Constructors

        public TestWorkOrderResourceRPCPresenterBuilder(IResourceRPCView<WorkOrder> view, IRepository<WorkOrder> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrderResourceRPCPresenter Build()
        {
            var obj = new TestWorkOrderResourceRPCPresenter(_view, _repository);
            if (_detailView != null)
                obj.DetailView = _detailView;
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestWorkOrderResourceRPCPresenterBuilder WithDetailView(IDetailView<WorkOrder> detailView)
        {
            _detailView = detailView;
            return this;
        }

        public TestWorkOrderResourceRPCPresenterBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderResourceRPCPresenter : WorkOrderResourceRPCPresenter
    {
        #region Constructors

        public TestWorkOrderResourceRPCPresenter(IResourceRPCView<WorkOrder> view, IRepository<WorkOrder> repository) : base(view, repository)
        {
        }

        #endregion

        #region Exposed Methods

        public void ExposedCheckUserSecurity()
        {
            CheckUserSecurity();
        }

        public void ExposedProcessCommandAndArgument()
        {
            ProcessCommandAndArgument();
        }

        public void SetSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        #endregion
    }
}
