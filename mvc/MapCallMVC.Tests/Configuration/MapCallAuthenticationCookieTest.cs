using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Configuration;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Configuration
{
    [TestClass]
    public class MapCallAuthenticationCookieTest
    {
        #region Fields

        private MapCallAuthenticationCookie<User> _target;
        private Mock<IAuthenticationRepository<User>> _authRepo;
        private IContainer _container;

        #endregion

        #region Init/Cleanup
        
        [TestInitialize]
        public void InitializeTest()
        {
            _container = new Container();
            _authRepo = new Mock<IAuthenticationRepository<User>>();
            _container.Inject(_authRepo.Object);
        }

        #endregion

        #region Tests

        #region Empty Constructor
        
        [TestMethod]
        public void TestEmptyConstructorSetsUserNameToNull()
        {
            Assert.IsNull(new MapCallAuthenticationCookie<User>().UserName);
        }

        [TestMethod]
        public void TestEmptyConstructorSetsIdToNull()
        {
            Assert.IsNull(new MapCallAuthenticationCookie<User>().Id);
        }

        #endregion

        #region UserName and Id Constructor

        [TestMethod]
        public void TestConstructorSetsUserName()
        {
            Assert.AreEqual("Cool", new MapCallAuthenticationCookie<User>(1, "Cool").UserName);
        }

        [TestMethod]
        public void TestConstructorSetsId()
        {
            Assert.AreEqual(42, new MapCallAuthenticationCookie<User>(42, "afaefe").Id);
        }

        #endregion

        #region Raw Cookie Constructor

        [TestMethod]
        public void TestRawConstructorSetsUserNameToRawCookieValue()
        {
            Assert.AreEqual("Kool and the Gang", new MapCallAuthenticationCookie<User>(_container, "Kool and the Gang").UserName);
        }

        [TestMethod]
        public void TestRawConstructorSetsIdIfUserNameMatchesAnExistingUser()
        {
            var user = new User {Id = 4, UserName = "someuser"};
            _authRepo.Setup(x => x.GetUser(user.UserName)).Returns(user).Verifiable();

            Assert.AreEqual(user.Id, new MapCallAuthenticationCookie<User>(_container, user.UserName).Id);
            _authRepo.VerifyAll();
        }

        [TestMethod]
        public void TestRawConstructorSetsIdToNullIfUserNameDoesNotMatchAnExistingUser()
        {
            _authRepo.Setup(x => x.GetUser("I don't exist")).Returns((User)null).Verifiable();
            Assert.IsNull(new MapCallAuthenticationCookie<User>(_container, "I don't exist").Id);

            _authRepo.VerifyAll();
        }

        [TestMethod]
        public void TestRawConstructorSetsUserNameToNullForNullOrEmptyOrWhiteSpaceRawCookieValues()
        {
            Assert.IsNull(new MapCallAuthenticationCookie<User>(_container, null).UserName);
            Assert.IsNull(new MapCallAuthenticationCookie<User>(_container, string.Empty).UserName);
            Assert.IsNull(new MapCallAuthenticationCookie<User>(_container, "     ").UserName);
        }

        #endregion

        [TestMethod]
        public void TestToCookieStringReturnsUserName()
        {
            Assert.AreEqual("Kool and the Gang", new MapCallAuthenticationCookie<User>(1, "Kool and the Gang").ToCookieString());
        }

        #endregion
    }
}
