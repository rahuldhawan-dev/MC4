using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class UserTest : InMemoryDatabaseTest<User>
    {
        #region Fields

        private User _user;
        private Application _application;
        private Module _module;

        private RoleAction _readAction,
                           _editAction,
                           _userAdminAction;

        private OperatingCenter _opc1;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _opc1 = GetFactory<OperatingCenterFactory>().Create();
            _user = GetFactory<UserFactory>().Create();
            _application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            _module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.OperationsHealthAndSafety});
            _readAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            _editAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Edit});
            _userAdminAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.UserAdministrator});
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetCachedMatchingRolesReturnsRoleMatchForTheArgumentsPassedIn()
        {
            var expectedOpc = new OperatingCenter();
            var match = _user.GetCachedMatchingRoles(RoleModules.OperationsTrainingRecords, RoleActions.Edit,
                expectedOpc);

            Assert.AreEqual(RoleActions.Edit, match.Action);
            Assert.AreEqual(RoleModules.OperationsTrainingRecords, match.Module);
            Assert.AreSame(expectedOpc, match.OperatingCenter);
        }

        #endregion
    }
}
