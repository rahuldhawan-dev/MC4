
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Utilities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate;
using StructureMap;

namespace Contractors.Tests.Library
{
    [TestClass]
    public class SecuredRepositoryBaseTest : InMemoryDatabaseTest<Town>
    {
        #region Private Members

        private TestSecuredTownRepository _target;
        private ContractorUser _currentUser;
        private Contractor _contractor;
        private OperatingCenter _opCenter;
        private MockAuthenticationService<ContractorUser> _authenticationService;

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IAuthenticationService<ContractorUser>>()
             .Singleton()
             .Use<MockAuthenticationService<ContractorUser>>();
        }

        [TestInitialize]
        public void SecuredRepositoryBaseTestInitialize()
        {
            _opCenter = GetFactory<OperatingCenterFactory>().Create();
            _contractor = GetFactory<ContractorFactory>().Create(new { OperatingCenters = new[] { _opCenter } });
            _currentUser = GetFactory<ContractorUserFactory>().Create(new { Contractor = _contractor});
            _authenticationService =
                (MockAuthenticationService<ContractorUser>)_container
                   .GetInstance<IAuthenticationService<ContractorUser>>();
            _authenticationService.SetUser(_currentUser);
            _target = _container.GetInstance<TestSecuredTownRepository>();
        }

        #endregion
        
        #region Save

        [TestMethod]
        public void TestSaveDoesNotThrowIfEntityIDIsGreaterThanZeroAndEntityDoesExistForUser()
        {
            var t = GetFactory<TownFactory>().Create(new { OperatingCenters = new [] { _opCenter }});
            MyAssert.DoesNotThrow(() => _target.Save(t));
        }
       
        #endregion
    }

    public class TestSecuredTownRepository : SecuredRepositoryBase<Town, ContractorUser>
    {
        public TestSecuredTownRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }
    }
}
