using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Utility.Permissions.Roles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Permissions;
using Moq;

namespace MapCall.CommonTest.Utility.Permissions.Roles
{
    [TestClass]
    public class RoleCacheManagerTest
    {
        #region Fields

        private Mock<IUser> _user;

        #endregion

        #region Initialating

        private TestRoleCacheManager InitializeBuilder()
        {
            _user = new Mock<IUser>();
            _user.Setup(x => x.Name).Returns("Bilbo Baggins");

            var rcm = new TestRoleCacheManager();

            return rcm;
        }

        #endregion

        #region Tests

        #region CreateCache method

        [TestMethod]
        public void TestCreateCacheThrowsIfRoleCreatorHandlerIsNull()
        {
            var target = InitializeBuilder();
            Assert.IsFalse(target.TestHasCache(_user.Object));

            // GetAllRolesForUser calls CreateCache internally.
            MyAssert.Throws<NullReferenceException>(
                () => target.GetAllRolesForUser(_user.Object));
        }

        #endregion

        #region HasCache method

        [TestMethod]
        public void TestHasCashReturnsFalseIfItDoesNotHaveCache()
        {
            var target = InitializeBuilder();
            Assert.IsFalse(target.TestHasCache(_user.Object));
        }

        [TestMethod]
        public void TestHasCashIsCaseInsensitiveWithUsername()
        {
            var target = InitializeBuilder();

            var expectedRole = new Mock<IRole>();
            _user.Setup(x => x.Name).Returns("Ross Dickinson");

            var iWasCalled = false;
            target.RoleCreatorHandler = (user) => {
                iWasCalled = true;
                var roles = new List<IRole>();
                roles.Add(expectedRole.Object);
                return roles;
            };

            // Creates the initial cache.
            var result = target.GetAllRolesForUser(_user.Object);
            Assert.IsTrue(iWasCalled);
            Assert.IsTrue(result.Any());

            _user.Setup(x => x.Name).Returns("ross dickinson");
            result = target.GetAllRolesForUser(_user.Object);
            Assert.IsTrue(result.Any());
        }

        #endregion

        #region GetAllRolesForUser method

        [TestMethod]
        public void TestGetAllRolesForUserThrowsForNullParameter()
        {
            var target = InitializeBuilder();
            MyAssert.Throws<ArgumentNullException>(
                () => target.GetAllRolesForUser(null));
        }

        [TestMethod]
        public void TestGetAllRolesForUserReturnsRolesGeneratedFromRoleCreatorHandler()
        {
            var target = InitializeBuilder();

            var expectedRole = new Mock<IRole>();

            target.RoleCreatorHandler = (user) => {
                var roles = new List<IRole>();
                roles.Add(expectedRole.Object);
                return roles;
            };

            var result = target.GetAllRolesForUser(_user.Object);
            Assert.IsTrue(result.Contains(expectedRole.Object));
        }

        #endregion

        #region RoleCreatorHandler property

        [TestMethod]
        public void TestRoleCreatorHandlerPropertyGetsSet()
        {
            var target = InitializeBuilder();
            Assert.IsNull(target.RoleCreatorHandler);

            Func<IUser, IEnumerable<IRole>> expected = (user) => null;
            target.RoleCreatorHandler = expected;

            Assert.AreSame(expected, target.RoleCreatorHandler);
        }

        #endregion

        #endregion
    }

    internal class TestRoleCacheManager : RoleCacheManager
    {
        public bool TestHasCache(IUser user)
        {
            return HasCache(user);
        }
    }
}
