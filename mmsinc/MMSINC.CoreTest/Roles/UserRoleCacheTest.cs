using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.DesignPatterns.Mvp.Interface;
using MMSINC.Utilities.Permissions;
using Moq;

namespace MMSINC.CoreTest.Roles
{
    [TestClass]
    public class UserRoleCacheTest
    {
        #region Fields

        private Mock<IUser> _user;
        private IEnumerable<IRole> _roleCollection;

        #endregion

        #region Initialization

        [TestInitialize]
        public void UserRoleCacheTestInitialize()
        {
            _user = new Mock<IUser>();
            _roleCollection = new List<IRole>();
        }

        #endregion

        #region Test Methods

        [TestMethod]
        public void TestConstructorSetsUserProperty()
        {
            var target = new RoleManager.UserRoleCache(_user.Object, _roleCollection);
            Assert.AreSame(_user.Object, target.User);
        }

        [TestMethod]
        public void TestConstructorSetsRolesProperty()
        {
            var target = new RoleManager.UserRoleCache(_user.Object, _roleCollection);
            Assert.AreSame(_roleCollection, target.Roles);
        }
        

        #endregion
    }
}
