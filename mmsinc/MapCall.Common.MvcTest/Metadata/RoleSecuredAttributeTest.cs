using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using Moq;
using System.Linq;
using RoleAction = MapCall.Common.Model.Entities.RoleAction;

namespace MapCall.Common.MvcTest.Metadata
{
    [TestClass]
    public class RoleSecuredAttributeTest 
    {
        #region Fields

        private Mock<IAuthenticationService<User>> _authServ;
        private RoleSecuredAttribute _target;
        private RoleAction _readAction;
        private Module _bappModule;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _authServ = new Mock<IAuthenticationService<User>>();
            _readAction = new RoleAction { Id = (int)RoleActions.Read }; 
            _bappModule = new Module { Id = (int)RoleModules.BAPPTeamSharingGeneral };
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestUserCanEditReturnsTrueIfAppliesToAdminsIsFalseAndUserIsSiteAdminRegardlessOfRoleMatch()
        {
            _target = new RoleSecuredAttribute(RoleModules.BAPPTeamSharingGeneral, RoleActions.Read) { AppliesToAdmins = false };
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);
            // No need to setup anything with the User, including roles, as that shouldn't be accessed at all.
            Assert.IsTrue(_target.UserCanEdit(_authServ.Object));
        }

        [TestMethod]
        public void TestUserCanEditReturnsFalseIfAppliesToAdminsIsTrueAndUserIsSiteAdminAndDoesNotHaveRoleMatch()
        {
            _target = new RoleSecuredAttribute(RoleModules.BAPPTeamSharingGeneral, RoleActions.Read) { AppliesToAdmins = true };
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            var matchingRoles = new[] { new AggregateRole { Action = _readAction, Module = _bappModule } };
            var roleMatch = new RoleMatch(matchingRoles, RoleModules.BAPPTeamSharingGeneral, RoleActions.Read, null);
            _authServ.Setup(x => x.CurrentUser.GetCachedMatchingRoles(RoleModules.BAPPTeamSharingGeneral, RoleActions.Read, null)).Returns(roleMatch);
            Assert.IsTrue(roleMatch.CanAccessRole, "Sanity");
            Assert.IsTrue(_target.UserCanEdit(_authServ.Object), "User should be able to edit since they have a matching role even though they are a site admin.");
            
            roleMatch = new RoleMatch(Enumerable.Empty<AggregateRole>(), RoleModules.BAPPTeamSharingGeneral, RoleActions.Read, null);
            _authServ.Setup(x => x.CurrentUser.GetCachedMatchingRoles(RoleModules.BAPPTeamSharingGeneral, RoleActions.Read, null)).Returns(roleMatch);
            Assert.IsFalse(roleMatch.CanAccessRole, "Sanity");
            Assert.IsFalse(_target.UserCanEdit(_authServ.Object), "User should not be able to edit since they are missing a matching role even though they are a site admin.");
        }

        [TestMethod]
        public void TestUserCanEditReturnsTrueIfUserIsNotSiteAdminButHasMatchingRoles()
        { 
            _target = new RoleSecuredAttribute(RoleModules.BAPPTeamSharingGeneral, RoleActions.Read) { AppliesToAdmins = true };
            _authServ.Setup(x => x.CurrentUserIsAdmin).Returns(false);

            var matchingRoles = new[] { new AggregateRole { Action = _readAction, Module = _bappModule } };
            var roleMatch = new RoleMatch(matchingRoles, RoleModules.BAPPTeamSharingGeneral, RoleActions.Read, null);
            _authServ.Setup(x => x.CurrentUser.GetCachedMatchingRoles(RoleModules.BAPPTeamSharingGeneral, RoleActions.Read, null)).Returns(roleMatch);
            Assert.IsTrue(roleMatch.CanAccessRole, "Sanity");
            Assert.IsTrue(_target.UserCanEdit(_authServ.Object), "User should be able to edit since they have a matching role even though they are a site admin.");
            
            roleMatch = new RoleMatch(Enumerable.Empty<AggregateRole>(), RoleModules.BAPPTeamSharingGeneral, RoleActions.Read, null);
            _authServ.Setup(x => x.CurrentUser.GetCachedMatchingRoles(RoleModules.BAPPTeamSharingGeneral, RoleActions.Read, null)).Returns(roleMatch);
            Assert.IsFalse(roleMatch.CanAccessRole, "Sanity");
            Assert.IsFalse(_target.UserCanEdit(_authServ.Object), "User should not be able to edit since they are missing a matching role even though they are a site admin.");
        }

        #endregion
    }
}
