using System.Collections.Generic;
using MMSINC.Interface;

namespace MMSINC.Utilities.Permissions
{
    public interface IRoleManager
    {
        #region Properties

        ILookupCache Lookup { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns all roles for a user.
        /// </summary>
        IEnumerable<IRole> GetAllRolesForUser(IUser user);

        bool UserCanAdministrateRole(IUser user, IRole role);
        bool UserIsInRole(IPermissionsObject obj);

        bool UserIsInRoleWithOperatingCenter(IPermissionsObject obj,
            string opCenterCode);

        #endregion
    }
}
