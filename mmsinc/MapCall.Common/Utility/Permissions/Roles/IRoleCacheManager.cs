using System;
using System.Collections.Generic;
using MMSINC.Interface;
using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions.Roles
{
    public interface IRoleCacheManager
    {
        #region Properties

        /// <summary>
        /// Method responsible for creating IRole objects. 
        /// </summary>
        Func<IUser, IEnumerable<IRole>> RoleCreatorHandler { get; }

        #endregion

        #region Methods

        IEnumerable<IRole> GetAllRolesForUser(IUser user);

        #endregion
    }
}
