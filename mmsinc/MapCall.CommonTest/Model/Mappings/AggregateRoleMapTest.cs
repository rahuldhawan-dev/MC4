using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using MapCall.Common.Model.Entities.Users;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class AggregateRoleMapTest : MapCallMvcInMemoryDatabaseTestBase<AggregateRole>
    {
        #region Fields

        private RoleAction _readAction;
        private Module _module1;
        private OperatingCenter _opc1;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _readAction = GetEntityFactory<RoleAction>().Create(new { Id = (int)RoleActions.Read });
            _module1 = GetEntityFactory<Module>().Create(new { Id = (int)RoleModules.BAPPTeamSharingGeneral });
            _opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestViewReturnsCorrectValuesWithMultipleUsersAndRoleGroups()
        {
            // HUGE NOTE: All the sanity checks are basically regression tests
            // due to the amount of problems I came across with initially setting
            // up the view mapping. If this test starts to fail at all, take
            // a close look at how the CompositeId is being generated. It *must*
            // be a unique string value. This test is not only testing that the view
            // is correct, but that NHibernate doesn't choke on it and return
            // null values when the CompositeId somehow ends up null.

            // NOTE: For this test, the RoleGroups and their roles can be functionally
            // identical as long as they're different instances.
            var roleGroup1 = GetEntityFactory<RoleGroup>().Create();
            var roleGroup2 = GetEntityFactory<RoleGroup>().Create();
            var roleGroupRole1 = GetEntityFactory<RoleGroupRole>().Create(new {
                Module = _module1,
                Action = _readAction,
                RoleGroup = roleGroup1
            }); 
            var roleGroupRole2 = GetEntityFactory<RoleGroupRole>().Create(new {
                Module = _module1,
                Action = _readAction,
                RoleGroup = roleGroup2
            });

            Assert.IsTrue(roleGroup1.Roles.Contains(roleGroupRole1), "Sanity");
            Assert.IsTrue(roleGroup2.Roles.Contains(roleGroupRole2), "Sanity");

            var user1 = GetEntityFactory<User>().Create();
            user1.RoleGroups.Add(roleGroup1);
            var user2 = GetEntityFactory<User>().Create();
            user2.RoleGroups.Add(roleGroup2);
            Session.Flush();

            var existingRoleForUser1 = GetEntityFactory<Role>().Create(new {
                User = user1
            });
            var existingRoleForUser2 = GetEntityFactory<Role>().Create(new {
                User = user2
            });

            Assert.IsTrue(user1.Roles.Contains(existingRoleForUser1), "Sanity");
            Assert.IsTrue(user2.Roles.Contains(existingRoleForUser2), "Sanity");

            Session.Clear();

            void AssertAggregateRolesIsValid(User user, Role expectedRoleForUser, RoleGroupRole expectedRoleGroupRole)
            {
                var userAgain = Session.Query<User>().Single(x => x.Id == user.Id);
                Assert.AreNotSame(user, userAgain, "Sanity");
                Assert.AreNotSame(expectedRoleForUser, userAgain.Roles.Single(), "Sanity");
                Assert.AreNotSame(expectedRoleGroupRole, userAgain.RoleGroups.Single(), "Sanity");

                var aggregateRoles = userAgain.AggregateRoles.ToList();
                Assert.AreEqual(2, aggregateRoles.Count(), "There should only be two aggregate roles for each user in this test.");
                
                var hasNormalRole = aggregateRoles.Any(x => x.UserRole?.Id == expectedRoleForUser.Id);
                var hasGroupRole = aggregateRoles.Any(x => x.RoleGroupRole?.Id == expectedRoleGroupRole.Id);
                Assert.IsTrue(hasNormalRole && hasGroupRole, $"HasNormalRole: {hasNormalRole}, HasGroupRole: {hasGroupRole}");
            }

            AssertAggregateRolesIsValid(user1, existingRoleForUser1, roleGroupRole1);
            AssertAggregateRolesIsValid(user2, existingRoleForUser2, roleGroupRole2);
        }

        #endregion
    }
}
