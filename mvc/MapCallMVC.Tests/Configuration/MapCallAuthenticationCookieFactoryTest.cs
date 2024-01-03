using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Configuration
{
    [TestClass]
    public class MapCallAuthenticationCookieFactoryTest
    {
        #region Fields

        private MapCallAuthenticationCookieFactory<User> _target;
        private Mock<IAuthenticationRepository<User>> _authRepo;
        private IContainer _container;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _target = new MapCallAuthenticationCookieFactory<User>(_container);
            _authRepo = new Mock<IAuthenticationRepository<User>>();
            _container.Inject(_authRepo.Object);
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
            var user = new User {Id = 42, UserName = "some guy"};
            _authRepo.Setup(x => x.GetUser(user.UserName)).Returns(user).Verifiable();

            var result = _target.CreateCookie(user.UserName);
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.UserName, result.UserName);
            _authRepo.VerifyAll();
        }

        #endregion
    }
}
