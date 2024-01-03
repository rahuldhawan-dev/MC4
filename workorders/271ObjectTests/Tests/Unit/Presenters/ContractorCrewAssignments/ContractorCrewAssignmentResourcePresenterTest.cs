using System;
using MMSINC.Data.Linq;
using MMSINC.Testing.MSTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Interface;
using MMSINC.Testing;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Rhino.Mocks;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using WorkOrders.Presenters.ContractorCrewAssignments;

namespace _271ObjectTests.Tests.Unit.Presenters.ContractorAssignments
{
    [TestClass]
    public class ContractorCrewAssignmentsResourcePresenterTest : EventFiringTestClass
    {
        #region Private Members

        private IResourceView _view;
        private IRepository<CrewAssignment> _repository;
        private TestContractorCrewAssignmentResourcePresenter _target;
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
                .DynamicMock(out _securityService);

            _target = new TestContractorCrewAssignmentResourcePresenterBuilder(_view, _repository)
                .WithSecurityService(_securityService);
        }

        #endregion

        [TestMethod]
        public void TestCheckUserSecurityThrowsIfUserIsNotAdmin()
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
        public void TestCheckUserSecurityDoesNotThrowIfUserIsAdmin()
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

    internal class TestContractorCrewAssignmentResourcePresenterBuilder : TestDataBuilder<TestContractorCrewAssignmentResourcePresenter>
    {
        #region Private Members

        private readonly IResourceView _view;
        private readonly IRepository<CrewAssignment> _repository;
        private ISecurityService _securityService;

        #endregion

        #region Constructors

        internal TestContractorCrewAssignmentResourcePresenterBuilder(IResourceView view, IRepository<CrewAssignment> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public override TestContractorCrewAssignmentResourcePresenter Build()
        {
            var obj = new TestContractorCrewAssignmentResourcePresenter(_view, _repository);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestContractorCrewAssignmentResourcePresenterBuilder WithSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
            return this;
        }

        #endregion
    }

    internal class TestContractorCrewAssignmentResourcePresenter : ContractorCrewAssignmentResourcePresenter
    {
        #region Constructors

        public TestContractorCrewAssignmentResourcePresenter(IResourceView view, IRepository<CrewAssignment> repository)
            : base(view, repository)
        {
        }

        #endregion

        public void SetSecurityService(ISecurityService securityService)
        {
            _securityService = securityService;
        }

        public void ExposedCheckUserSecurity()
        {
            base.CheckUserSecurity();
        }

    }
}
