using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;

namespace MapCall.Common.Metadata
{
    /// <summary>
    /// Version of SecuredAttribute that allows role access to also determine if a user is an admin.
    /// If you aren't using the role access part of this then just use the base class.
    /// </summary>
    public class RoleSecuredAttribute : MMSINC.Metadata.SecuredAttribute
    {
        #region Properties

        public RoleModules Role { get; }
        public RoleActions Action { get; }

        #endregion

        #region Constructor

        public RoleSecuredAttribute(RoleModules role, RoleActions action)
        {
            Role = role;
            Action = action;
        }

        #endregion

        #region Public Methods

        public override bool UserCanEdit(IAuthenticationService<IAdministratedUser> srv)
        {
            return base.UserCanEdit(srv) || ((IAuthenticationService<User>)srv).CurrentUser
               .GetCachedMatchingRoles(Role, Action, null)
               .CanAccessRole;
        }

        #endregion
    }
}
