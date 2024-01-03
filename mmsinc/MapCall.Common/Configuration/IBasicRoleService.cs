using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using StructureMap;

namespace MapCall.Common.Configuration
{
    public interface IBasicRoleService
    {
        bool CanAccessRole(RoleModules module, RoleActions action = RoleActions.Read, OperatingCenter opCenter = null);
    }

    public class BasicRoleService : IBasicRoleService
    {
        #region Fields

        protected IAuthenticationService<User> _authServ;

        #endregion

        #region Constructors

        public BasicRoleService(IAuthenticationService<User> authServ)
        {
            _authServ = authServ;
        }

        #endregion

        #region Exposed Methods

        public bool CanAccessRole(RoleModules module, RoleActions action = RoleActions.Read,
            OperatingCenter opCenter = null)
        {
            if (!_authServ.CurrentUserIsAuthenticated)
            {
                return false;
            }

            if (_authServ.CurrentUserIsAdmin)
            {
                return true;
            }

            return _authServ.CurrentUser.GetCachedMatchingRoles(module, action, opCenter).CanAccessRole;
        }

        #endregion
    }
}
