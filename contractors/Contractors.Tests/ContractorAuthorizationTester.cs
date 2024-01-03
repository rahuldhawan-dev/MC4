
using MapCall.Common.Model.Entities;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using NHibernate;
using StructureMap;
using UserType = MMSINC.Testing.UserType;

namespace Contractors.Tests
{
   
    public class ContractorAuthorizationTester : ControllerAuthorizationTester<MvcApplication, ContractorUser, ContractorAuthorizationAsserter>
    {
        #region Fields

        protected readonly IContainer _container;

        #endregion

        #region Constructors

        public ContractorAuthorizationTester(MvcApplicationTester<MvcApplication> appTester, IContainer container, ITestDataFactoryService testFactoryService) : base(appTester, testFactoryService)
        {
            // Don't do any _container.injecting here. Because this is kind of a sub-test
            // for an actual test, we don't want to care about the order this is constructed
            // with other things in a [TestInitialize] scenario. ie: RoleService/AuthServ/something
            // getting injected here and then immediately overwritten somewhere else. Also don't
            // create a RoleService instance because it immediately gets an AuthServ instance.
            _container = container;
        }

        #endregion

        #region Private Methods

        protected override ContractorAuthorizationAsserter CreateAsserter()
        {
            return _container.GetInstance<ContractorAuthorizationAsserter>();
        }

        #endregion
    }

    public class ContractorAuthorizationAsserter :
        ControllerAuthorizationAsserter<MvcApplication, ContractorUser>
    {
        #region Constructors

        public ContractorAuthorizationAsserter(MvcApplicationTester<MvcApplication> appTester, ITestDataFactoryService testFactoryService, IContainer container) : base(appTester, testFactoryService, container) { }

        #endregion

        #region Private Methods

        protected override ContractorUser CreateUser(UserType userType)
        {
            return GetFactory<ContractorUserFactory>().Create(new { IsAdmin = userType == UserType.SiteAdmin });
        }

        #endregion
    }
}
