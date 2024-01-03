using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using NHibernate;
using StructureMap;

namespace MMSINC.CoreTest.Authentication
{
    [TestClass]
    public class AuthenticationLogRepositoryBaseTest : InMemoryDatabaseTest<TestAuthenticationLog>
    {
        #region Fields

        private TestUser _user;
        private TestLogRepository _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _user = GetFactory<TestUserFactory>().Create();
            _target = _container.GetInstance<TestLogRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestFindActiveLogByCookieReturnsLogWhereTheCookieMatchesAndTheLoggedOutDateIsNotSet()
        {
            var expectedCookie = "cooookiiiiie";
            var log = new TestAuthenticationLog {
                IpAddress = "what",
                LoggedInAt = DateTime.Now,
                User = _user
            };
            _target.Save(log, expectedCookie);
            var someOtherLog = new TestAuthenticationLog {
                IpAddress = "nah",
                LoggedInAt = DateTime.Today,
                User = _user
            };
            _target.Save(someOtherLog, "not expected");

            var result = _target.FindActiveLogByCookie(expectedCookie);
            Assert.AreSame(log, result);
        }

        [TestMethod]
        public void TestFindActiveLogByCookieReturnsNullIfNoMatchingCookieExists()
        {
            var result = _target.FindActiveLogByCookie("cookie");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestFindActiveLogByCookieReturnsNullIfMatchingCookieExistsButLoggedOutAtValueIsSet()
        {
            var expectedCookie = "cooookiiiiie";
            var log = new TestAuthenticationLog {
                IpAddress = "what",
                LoggedInAt = DateTime.Now,
                User = _user
            };
            _target.Save(log, expectedCookie);

            Assert.AreSame(log, _target.FindActiveLogByCookie(expectedCookie));

            log.LoggedOutAt = DateTime.Now;
            _target.Save(log);

            Assert.IsNull(_target.FindActiveLogByCookie(expectedCookie));
        }

        [TestMethod]
        public void TestSaveOverloadAlwaysSetsTheSameHashForACookieToTheAuthCookieHashProperty()
        {
            var expectedCookie = "cooookiiiiie";
            var log = new TestAuthenticationLog {
                IpAddress = "what",
                LoggedInAt = DateTime.Now,
                User = _user
            };
            _target.Save(log, expectedCookie);

            var expectedGuid = log.AuthCookieHash;
            Assert.AreNotEqual(Guid.Empty, expectedGuid, "Guid.Empty would mean that this value was not set.");

            log.AuthCookieHash = Guid.Empty;

            _target.Save(log, expectedCookie);
            Assert.AreEqual(expectedGuid, log.AuthCookieHash);
        }

        [TestMethod]
        public void TestSaveOverrideThrowsExceptionIfAuthCookieHashIsEmpty()
        {
            var expectedCookie = "cooookiiiiie";
            var log = new TestAuthenticationLog {
                IpAddress = "what",
                LoggedInAt = DateTime.Now,
                User = _user
            };
            _target.Save(log, expectedCookie);

            log.AuthCookieHash = Guid.Empty;

            MyAssert.Throws(() => _target.Save(log));
        }

        [TestMethod]
        public void TestDeleteThrowsNotSupportedException()
        {
            MyAssert.Throws<NotSupportedException>(() => _target.Delete(null));
        }

        #endregion

        #region Test classes

        private class TestLogRepository : AuthenticationLogRepositoryBase<TestAuthenticationLog, TestUser>
        {
            public TestLogRepository(ISession session, IContainer container) : base(session, container) { }
        }

        #endregion
    }
}
