using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class RoleMatchTest : InMemoryDatabaseTest<User>
    {
        #region Fields

        private Application _application;
        private Module _module;

        private RoleAction _readAction,
                           _addAction,
                           _editAction,
                           _deleteAction,
                           _userAdminAction;

        private OperatingCenter _opc1;

        private List<AggregateRole> _roles;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _roles = new List<AggregateRole>();
            _opc1 = GetFactory<OperatingCenterFactory>().Create();
            _application = GetFactory<ApplicationFactory>().Create(new {Id = RoleApplications.Operations});
            _module = GetFactory<ModuleFactory>().Create(new {Id = RoleModules.OperationsHealthAndSafety});
            _readAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Read});
            _addAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Add});
            _editAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Edit});
            _deleteAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.Delete});
            _userAdminAction = GetFactory<ActionFactory>().Create(new {Id = RoleActions.UserAdministrator});
        }

        #endregion

        #region Private Methods

        private AggregateRole CreateRole(object args)
        {
            var role = GetFactory<RoleFactory>().Build(args);
            var aggregateRole = new AggregateRole(role);
            _roles.Add(aggregateRole);
            return aggregateRole;
        }

        #endregion

        #region Tests

        #region Constructor

        [TestMethod]
        public void TestConstructorSetsActionPropertyToValuePassed()
        {
            var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Edit,
                null);
            Assert.AreEqual(RoleActions.Edit, target.Action);
        }

        [TestMethod]
        public void TestConstructorSetsModulePropertyToValuePassed()
        {
            var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Edit,
                null);
            Assert.AreEqual(RoleModules.OperationsHealthAndSafety, target.Module);
        }

        [TestMethod]
        public void TestConstructorSetsOperatingCenterPropertyToValuePassed()
        {
            var expected = new OperatingCenter();
            var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Edit,
                expected);
            Assert.AreSame(expected, target.OperatingCenter);

            target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Edit, null);
            Assert.IsNull(target.OperatingCenter);
        }

        #endregion

        #region HasWildCardMatch

        [TestMethod]
        public void TestHasWildCardMatch_ReturnsTrueIfMatchesContainsAWildcardRole()
        {
            var goodRole = CreateRole(new {
                Application = _application,
                Module = _module,
                Action = _readAction,
            });
            goodRole.OperatingCenter = null;

            var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Read,
                null);
            Assert.IsTrue(target.HasWildCardMatch);
        }

        [TestMethod]
        public void TestHasWildCardMatch_ReturnsFalseIfMatchesDoesNotContainAWildcardRole()
        {
            var goodRole = CreateRole(new {
                Application = _application,
                Module = _module,
                Action = _readAction,
                OperatingCenter = _opc1
            });

            var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Read,
                null);
            Assert.IsFalse(target.HasWildCardMatch);
        }

        [TestMethod]
        public void TestHasWildCardMatch_ReturnsFalseIfThereArentAnyMatchesAtAll()
        {
            var target = new RoleMatch(new AggregateRole[] { }.AsQueryable(), RoleModules.OperationsHealthAndSafety,
                RoleActions.Read, null);
            Assert.IsFalse(target.HasWildCardMatch);
        }

        #endregion

        #region Matches

        [TestMethod]
        public void TestMatches_ReturnsExpectedMatches()
        {
            var goodRole = CreateRole(new {
                Application = _application,
                Module = _module,
                Action = _readAction,
                OperatingCenter = _opc1,
            });
            var badRole = CreateRole(new {
                Application = _application,
                Module = _module,
                Action = _editAction,
                OperatingCenter = _opc1,
            });

            var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Edit,
                null);
            Assert.AreSame(badRole, target.Matches.Single());
            Assert.IsTrue(target.CanAccessRole);
        }

        [TestMethod]
        public void TestMatches_ReturnsUserAdminMatchesIfRoleActionIsSomeOtherAction()
        {
            var goodRole = CreateRole(new {
                Application = _application,
                Module = _module,
                Action = _userAdminAction,
                OperatingCenter = _opc1,
            });

            var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Read,
                null);
            Assert.AreSame(goodRole, target.Matches.Single());
            Assert.IsTrue(target.CanAccessRole);
        }

        [TestMethod]
        public void TestMatches_ReturnsMatchesForReadActionIfUserHasRoleForAnyActionAtAll()
        {
            var actions = new[] {_readAction, _addAction, _editAction, _deleteAction, _userAdminAction};

            foreach (var a in actions)
            {
                _roles.Clear();
                var goodRole = CreateRole(new {
                    Application = _application,
                    Module = _module,
                    Action = a,
                });

                var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety,
                    RoleActions.Read, null);
                Assert.AreSame(goodRole, target.Matches.SingleOrDefault());
                Assert.IsTrue(target.CanAccessRole);
            }
        }

        [TestMethod]
        public void TestMatches_DoesNotReturnAnythingIfNoMatchesAreFound()
        {
            var anotherOpCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var goodRole = CreateRole(new {
                Application = _application,
                Module = _module,
                Action = _userAdminAction,
                OperatingCenter = _opc1,
            });

            var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Read,
                anotherOpCenter);
            Assert.IsFalse(target.Matches.Any());
            Assert.IsFalse(target.CanAccessRole);
        }

        [TestMethod]
        public void TestMatches_ReturnsRoleIfSpecificOperatingCenterIsSearchedForAndAWildcardRoleExists()
        {
            var goodRole = CreateRole(new {
                Application = _application,
                Module = _module,
                Action = _userAdminAction,
            });
            goodRole.OperatingCenter = null;

            Assert.IsTrue(goodRole.IsValidForAnyOperatingCenter, "Sanity check");
            var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Read,
                _opc1);
            Assert.AreSame(goodRole, target.Matches.Single());
            Assert.IsTrue(target.CanAccessRole);

            _roles.Clear();
            target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Read,
                _opc1);
            Assert.IsNull(target.Matches.SingleOrDefault());
            Assert.IsFalse(target.CanAccessRole);
        }

        #endregion

        #region OperatingCenters

        [TestMethod]
        public void TestOperatingCenters_ReturnsDistinctOperatingCentersThatMatchAGivenOperatingCenterModuleAndAction()
        {
            var goodRole = CreateRole(new {
                Application = _application,
                Module = _module,
                Action = _readAction,
                OperatingCenter = _opc1,
            });
            var duplicatedOperatingCenterRole = CreateRole(new {
                Application = _application,
                Module = _module,
                Action = _editAction,
                OperatingCenter = _opc1,
            });

            var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Read,
                null);
            Assert.AreEqual(_opc1.Id, target.OperatingCenters.SingleOrDefault());
        }

        [TestMethod]
        public void TestOperatingCenters_DoesNotIncludeNullsIfWildcardRolesExist()
        {
            var goodRole = CreateRole(new {
                Application = _application,
                Module = _module,
                Action = _readAction,
            });
            goodRole.OperatingCenter = null;

            var target = new RoleMatch(_roles.AsQueryable(), RoleModules.OperationsHealthAndSafety, RoleActions.Read,
                null);
            Assert.IsFalse(target.OperatingCenters.Any());
        }

        #endregion

        #endregion
    }
}
