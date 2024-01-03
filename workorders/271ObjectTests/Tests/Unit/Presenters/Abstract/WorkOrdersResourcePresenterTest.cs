using System;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;

namespace _271ObjectTests.Tests.Unit.Presenters.Abstract
{
    /// <summary>
    /// Summary description for WorkOrdersResourcePresenterTest.
    /// </summary>
    [TestClass]
    public class WorkOrdersResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private ISecurityService _securityService;
        private IResourceView _view;
        private IRepository<WorkOrder> _repository;
        private TestWorkOrdersResourcePresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _view)
                .DynamicMock(out _repository)
                .DynamicMock(out _securityService);

            _target = new TestWorkOrdersResourcePresenterBuilder(_view,
                _repository)
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
        public void TestSecurityServicePropertyReturnsMockedValueIfPresent()
        {
            _mocks.ReplayAll();

            Assert.AreSame(_securityService,
                _target.GetPropertyValueByName("SecurityService"));
        }

        [TestMethod]
        public void TestSecurityServicePropertyGetsSecurityServiceSingletonInstance()
        {
            _mocks.ReplayAll();

            _target.SetSecurityService(null);

            Assert.AreSame(SecurityService.Instance,
                _target.GetPropertyValueByName("SecurityService"));
        }

        #endregion

        #region Method Tests

        //[TestMethod]
        //public void TestGetOperatingCenterIDGetsValueFromUtilitiesClass()
        //{
        //    _mocks.ReplayAll();

        //    Assert.AreEqual(Utilities.GetCurrentOperatingCenterID(),
        //        _target.GetOperatingCenterID());
        //}

        [TestMethod]
        public void TestCheckUserSecurityThrowsExceptionWhenCurrentUserIsNull()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_securityService.CurrentUser).Return(null);
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws<UnauthorizedAccessException>(
                    () => _target.ExposedCheckUserSecurity());
            }
        }

        [TestMethod]
        public void TestCheckUserSecurityThrowsExceptionWhenCurrentUserIsNotGrantedAccess()
        {
            var user = _mocks.DynamicMock<IUser>();

            using (_mocks.Record())
            {
                SetupResult.For(_securityService.CurrentUser).Return(user);
                SetupResult.For(_securityService.UserHasAccess).Return(false);
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws<UnauthorizedAccessException>(
                    () => _target.ExposedCheckUserSecurity());
            }
        }

        [TestMethod]
        public void TestCheckUserSecurityDoesNotThrowExceptionWhenCurrentUserIsGrantedAccess()
        {
            var user = _mocks.DynamicMock<IUser>();

            using (_mocks.Record())
            {
                SetupResult.For(_securityService.CurrentUser).Return(user);
                SetupResult.For(_securityService.UserHasAccess).Return(true);
            }

            using (_mocks.Playback())
            {
                MyAssert.DoesNotThrow(() => _target.ExposedCheckUserSecurity());
            }
        }

        #endregion
    }

    internal class TestWorkOrdersResourcePresenterBuilder : TestDataBuilder<TestWorkOrdersResourcePresenter>
    {
        #region Private Members

        private ISecurityService _securityService;
        private IResourceView _view;
        private IRepository<WorkOrder> _repository;

        #endregion

        #region Constructors

        public TestWorkOrdersResourcePresenterBuilder(IResourceView view, IRepository<WorkOrder> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrdersResourcePresenter Build()
        {
            var obj = new TestWorkOrdersResourcePresenter(_view, _repository);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestWorkOrdersResourcePresenterBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrdersResourcePresenter : WorkOrdersResourcePresenter<WorkOrder>
    {
        #region Constructors

        public TestWorkOrdersResourcePresenter(IResourceView view, IRepository<WorkOrder> repository) : base(view, repository)
        {
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Implemented in order to expose the protected CheckUserSecurity method.
        /// </summary>
        public void ExposedCheckUserSecurity()
        {
            CheckUserSecurity();
        }

        public void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }

        #endregion
    }
}
