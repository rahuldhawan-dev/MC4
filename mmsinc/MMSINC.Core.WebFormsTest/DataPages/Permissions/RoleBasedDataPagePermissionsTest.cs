using System;
using MMSINC.DataPages.Permissions;
using MMSINC.Interface;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MMSINC.Core.WebFormsTest.DataPages.Permissions
{
    /// <summary>
    /// Summary description for DataPagePermissionsTest
    /// </summary>
    [TestClass]
    public class RoleBasedDataPagePermissionsTest
    {
        #region Private Members

        private Mock<IUser> _user;
        private Mock<IModulePermissions> _modPerms;
        private Mock<IPermissionsObject> _allowPermObj; // Users with specific Allow role
        private Mock<IPermissionsObject> _unspecifiedPermObj; // Users without an Allow role
        private Mock<IPermissionsObject> _denyPermObj; // Users with a Deny role. 
        private TestRoleBasedDataPagePermissions _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void RoleBasedDataPagePermissionsTestInitialize()
        {
            ResetTarget();
        }

        private void ResetTarget()
        {
            _user = new Mock<IUser>();
            _modPerms = new Mock<IModulePermissions>();
            _modPerms.SetupGet(x => x.Application).Returns("Burger");
            _modPerms.SetupGet(x => x.Module).Returns("King");

            _allowPermObj = CreateMockPermissionsObject(true);
            _unspecifiedPermObj = CreateMockPermissionsObject(false);

            _target = new TestRoleBasedDataPagePermissions(_modPerms.Object, _user.Object);
        }

        private Mock<IPermissionsObject> CreateMockPermissionsObject(bool allow)
        {
            var p = new Mock<IPermissionsObject>();
            p.Setup(x => x.InAny()).Returns(allow);
            p.SetupGet(x => x.SpecificPermissions).Returns(_modPerms.Object);
            p.SetupGet(x => x.Action).Returns(ModuleAction.Read);
            return p;
        }

        #endregion

        #region Constructor tests

        [TestMethod]
        public void TestConstructorSetsRolePropertyNamesAndIUserMember()
        {
            ResetTarget();

            var expected = _modPerms.Object.Application + _modPerms.Object.Module;

            Assert.AreEqual(expected, _target.PageRole);
            Assert.AreSame(_user.Object, _target.IUser);
            Assert.AreSame(_modPerms.Object, _target.ModulePermissions);
        }

        [TestMethod]
        public void TestConstructorThrowsNullExceptionForNullModulePermissions()
        {
            MyAssert.Throws<NullReferenceException>(
                () => new TestRoleBasedDataPagePermissions(null, _user.Object));
        }

        [TestMethod]
        public void TestConstructorThrowsNullExceptionForNullIUser()
        {
            MyAssert.Throws<NullReferenceException>(
                () => new TestRoleBasedDataPagePermissions(_modPerms.Object, null));
        }

        #endregion

        #region Reading

        [TestMethod]
        public void TestReadAccessAllowedReturnsTrueIfUserCanRead()
        {
            ResetTarget();
            _user.Setup(x => x.CanRead(_modPerms.Object)).Returns(_allowPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_allowPermObj.Object);

            Assert.IsTrue(_target.ReadAccess.IsAllowed);
        }

        [TestMethod]
        public void TestReadAccessAllowedReturnsTrueIfUserCanAdministrate()
        {
            ResetTarget();
            _user.Setup(x => x.CanRead(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_allowPermObj.Object);

            Assert.IsTrue(_target.ReadAccess.IsAllowed);
        }

        [TestMethod]
        public void TestReadAccessAllowedTrueIfUserCanReadIsFalseAndCanAdministrateIsFalse()
        {
            ResetTarget();
            _user.Setup(x => x.CanRead(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);

            Assert.IsFalse(_target.ReadAccess.IsAllowed);
        }

        #endregion

        #region Adding

        [TestMethod]
        public void TestCreateAccessAllowedReturnsTrueIfUserCanAdd()
        {
            ResetTarget();
            _user.Setup(x => x.CanAdd(_modPerms.Object)).Returns(_allowPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_allowPermObj.Object);

            Assert.IsTrue(_target.CreateAccess.IsAllowed);
        }

        [TestMethod]
        public void TestCreateAccessAllowedReturnsTrueIfUserCanAdministrate()
        {
            ResetTarget();
            _user.Setup(x => x.CanAdd(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_allowPermObj.Object);

            Assert.IsTrue(_target.CreateAccess.IsAllowed);
        }

        [TestMethod]
        public void TestCreateAccessAllowedTrueIfUserCanAddIsFalseAndCanAdministrateIsFalse()
        {
            ResetTarget();
            _user.Setup(x => x.CanAdd(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);

            Assert.IsFalse(_target.CreateAccess.IsAllowed);
        }

        #endregion

        #region Editing

        [TestMethod]
        public void TestEditAccessAllowedReturnsTrueIfUserCanEdit()
        {
            ResetTarget();
            _user.Setup(x => x.CanEdit(_modPerms.Object)).Returns(_allowPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_allowPermObj.Object);

            Assert.IsTrue(_target.EditAccess.IsAllowed);
        }

        [TestMethod]
        public void TestEditAccessAllowedReturnsTrueIfUserCanAdministrate()
        {
            ResetTarget();
            _user.Setup(x => x.CanEdit(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_allowPermObj.Object);

            Assert.IsTrue(_target.EditAccess.IsAllowed);
        }

        [TestMethod]
        public void TestEditAccessAllowedTrueIfUserCanEditIsFalseAndCanAdministrateIsFalse()
        {
            ResetTarget();
            _user.Setup(x => x.CanEdit(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);

            Assert.IsFalse(_target.EditAccess.IsAllowed);
        }

        #endregion

        #region Deleting

        [TestMethod]
        public void TestDeleteAccessAllowedReturnsTrueIfUserCanDelete()
        {
            ResetTarget();
            _user.Setup(x => x.CanDelete(_modPerms.Object)).Returns(_allowPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_allowPermObj.Object);

            Assert.IsTrue(_target.DeleteAccess.IsAllowed);
        }

        [TestMethod]
        public void TestDeleteAccessAllowedReturnsTrueIfUserCanAdministrate()
        {
            ResetTarget();
            _user.Setup(x => x.CanDelete(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_allowPermObj.Object);

            Assert.IsTrue(_target.DeleteAccess.IsAllowed);
        }

        [TestMethod]
        public void TestDeleteAccessAllowedTrueIfUserCanDeleteIsFalseAndCanAdministrateIsFalse()
        {
            ResetTarget();
            _user.Setup(x => x.CanDelete(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);
            _user.Setup(x => x.CanAdministrate(_modPerms.Object)).Returns(_unspecifiedPermObj.Object);

            Assert.IsFalse(_target.DeleteAccess.IsAllowed);
        }

        #endregion
    }

    class TestRoleBasedDataPagePermissions : RoleBasedDataPagePermissions
    {
        #region Constructors

        public TestRoleBasedDataPagePermissions(IModulePermissions pageRoleName, IUser user)
            : base(pageRoleName, user) { }

        #endregion

        #region test methods

        public void SetPageRole(string roleName)
        {
            PageRole = roleName;
        }

        #endregion
    }
}
