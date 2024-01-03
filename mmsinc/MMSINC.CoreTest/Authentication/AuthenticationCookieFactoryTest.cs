using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;

namespace MMSINC.CoreTest.Authentication
{
    [TestClass]
    public class AuthenticationCookieFactoryTest
    {
        #region Fields

        private AuthenticationCookieFactory _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new AuthenticationCookieFactory(null);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestCreateEmptyCookieCreatesACookieWithNoUserNameOrIdSet()
        {
            var result = _target.CreateEmptyCookie();
            Assert.IsNull(result.Id);
            Assert.IsNull(result.UserName);
        }

        [TestMethod]
        public void TestCreateCookieWithIdAndUsernameParametersReturnsCookieWithThosePropertiesSet()
        {
            var result = _target.CreateCookie(42, "cool");
            Assert.AreEqual(42, result.Id);
            Assert.AreEqual("cool", result.UserName);
        }

        [TestMethod]
        public void TestCreateCookieWithRawCookieParameterPassesThatToNewInstance()
        {
            var result = _target.CreateCookie("42; cool");
            Assert.AreEqual(42, result.Id);
            Assert.AreEqual("cool", result.UserName);
        }

        #endregion
    }
}
