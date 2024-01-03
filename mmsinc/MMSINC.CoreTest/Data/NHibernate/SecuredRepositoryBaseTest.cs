using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using NHibernate;
using StructureMap;

namespace MMSINC.CoreTest.Data.NHibernate
{
    // [DeploymentItem(@"x86\SQLite.Interop.dll", "x86")]
    [DeploymentItem(@"x64\SQLite.Interop.dll", "x64")]
    [TestClass]
    public class SecuredRepositoryBaseTest : InMemoryDatabaseTest<TestUser>
    {
        #region Private Members

        private TestSecuredTestUserRepository _target;
        private TestUser _testUser;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void SecuredRepositoryBaseTestInitialize()
        {
            _target = _container.With(new MockAuthenticationService<TestUser>(_testUser).Object)
                                .GetInstance<TestSecuredTestUserRepository>();
        }

        #endregion

        #region Constructor

        [TestMethod]
        public void TestConstructorSetsSessionAndCurrentUser()
        {
            Assert.AreSame(Session, _target.GetHiddenPropertyValueByName("Session"));
            Assert.AreSame(_testUser, _target.GetHiddenPropertyValueByName("CurrentUser"));
        }

        #endregion

        #region Delete

        [TestMethod]
        public void TestDeleteThrowsExceptionIfEntityDoesNotExist()
        {
            var t = new TestUser();
            t.SetPropertyValueByName("Id", int.MaxValue);
            MyAssert.Throws<DomainLogicException>(() => _target.Delete(t));
        }

        [TestMethod]
        public void TestTestDeleteDeletesEntityIfItExists()
        {
            var t = GetFactory<TestUserFactory>().Create();

            MyAssert.DoesNotThrow(() => _target.Delete(t));
        }

        #endregion

        #region Save

        [TestMethod]
        public void TestSaveThrowsIfEntityIDIsGreaterThanZeroButEntityDoesNotExistForUser()
        {
            var t = new TestUser();
            t.SetPropertyValueByName("Id", int.MaxValue);
            MyAssert.Throws<DomainLogicException>(() => _target.Save(t));
        }

        [TestMethod]
        public void TestSaveDoesNotThrowIfEntityIDEqualsZero()
        {
            var t = new TestUser();
            MyAssert.DoesNotThrow(() => _target.Save(t));
        }

        #endregion
    }

    public class TestSecuredTestUserRepository : SecuredRepositoryBase<TestUser, TestUser>
    {
        public TestSecuredTestUserRepository(ISession session, IContainer container,
            IAuthenticationService<TestUser> authenticationService) :
            base(session, authenticationService, container) { }
    }
}
