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
    /// Summary description for WorkOrdersAdminResourcePresenterTest.
    /// </summary>
    [TestClass]
    public class WorkOrdersAdminResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private ISecurityService _securityService;
        private IResourceView _view;
        private IRepository<WorkOrder> _repository;
        private TestWorkOrdersAdminResourcePresenter _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();

            _mocks
                .DynamicMock(out _securityService)
                .DynamicMock(out _view)
                .DynamicMock(out _repository);

            _target = new TestWorkOrdersAdminResourcePresenterBuilder(_view,
                _repository)
                .WithSecurityService(_securityService);
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #endregion

        [TestMethod]
        public void TestCheckUserSecurityThrowsExceptionIfCurrentUserIsNotAdmin()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_securityService.IsAdmin).Return(false);
            }

            using (_mocks.Playback())
            {
                MyAssert.Throws<UnauthorizedAccessException>(
                    () => _target.ExposedCheckUserSecurity());
            }
        }

        [TestMethod]
        public void TestCheckUserSecurityDoesNotThrowExceptionIfCurrentUserIsAdmin()
        {
            using (_mocks.Record())
            {
                SetupResult.For(_securityService.IsAdmin).Return(true);
            }

            using (_mocks.Playback())
            {
                MyAssert.DoesNotThrow(() => _target.ExposedCheckUserSecurity());
            }
        }
    }

    internal class TestWorkOrdersAdminResourcePresenterBuilder : TestDataBuilder<TestWorkOrdersAdminResourcePresenter>
    {
        #region Private Members

        private ISecurityService _securityService;
        private readonly IResourceView _view;
        private readonly IRepository<WorkOrder> _repository;

        #endregion

        #region Constructors

        internal TestWorkOrdersAdminResourcePresenterBuilder(IResourceView view, IRepository<WorkOrder> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestWorkOrdersAdminResourcePresenter Build()
        {
            var obj = new TestWorkOrdersAdminResourcePresenter(_view, _repository);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestWorkOrdersAdminResourcePresenter WithSecurityService(ISecurityService service)
        {
            _securityService = service;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrdersAdminResourcePresenter : WorkOrdersAdminResourcePresenter<WorkOrder>
    {
        #region Constructors

        public TestWorkOrdersAdminResourcePresenter(IResourceView view, IRepository<WorkOrder> repository) : base(view, repository)
        {
        }

        #endregion

        #region Exposed Methods

        public void ExposedCheckUserSecurity()
        {
            CheckUserSecurity();
        }

        public void SetSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        #endregion
    }
}
