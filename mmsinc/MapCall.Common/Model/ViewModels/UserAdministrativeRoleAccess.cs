using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Entities;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Type;

namespace MapCall.Common.Model.ViewModels
{
    /// <summary>
    /// Helper class for handling role administration access.
    /// </summary>
    public class UserAdministrativeRoleAccess
    {
        #region Properties

        public User User { get; }

        /// <summary>
        /// Gets a dictionary of modules and the operating centers that a user has UserAdministrator
        /// role access to. If a user is a site admin, this will be empty. The only use for this is
        /// to easily populate dropdowns without having to duplicate the logic elsewhere.
        /// </summary>
        public IDictionary<Module, IEnumerable<OperatingCenter>> OperatingCentersByModule { get; }

        #endregion

        #region Constructor

        public UserAdministrativeRoleAccess(User user)
        {
            User = user;

            if (user.IsAdmin || !user.IsUserAdmin)
            {
                // There's no point in populating this for either of these since site admins will have
                // access to everything and not-admins-at-all will have access to nothing. 
                OperatingCentersByModule = new Dictionary<Module, IEnumerable<OperatingCenter>>();
            }
            else
            {
                OperatingCentersByModule = user.AggregateRoles
                                               .Where(x => x.Action.Id == (int)RoleActions.UserAdministrator)
                                               .GroupBy(x => x.Module)
                                               .ToDictionary(x => x.Key, x => x.Select(y => y.OperatingCenter));
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns true if this user is a UserAdministrator for a given module and operating center.
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="operatingCenterId"></param>
        /// <returns></returns>
        public bool CanAdministrate(int moduleId, int? operatingCenterId)
        {
            // NOTE: This IsAdmin/IsUserAdmin logic comes originally from a mishmash of how the old
            // Users.aspx and RoleControl.ascx things worked in mapcall proper. The entire Users page
            // requires IsUserAdmin to be true which would then give access to RoleControl. In MVC,
            // we allow users to view their own User page and we hide the Roles tab behind the IsUserAdmin
            // flag. 
            
            if (User.IsAdmin)
            {
                return true;
            }

            if (!User.IsUserAdmin)
            {
                return false;
            }

            var matchingModule = OperatingCentersByModule.Keys.SingleOrDefault(x => x.Id == moduleId);
            if (matchingModule != null)
            {
                var operatingCenters = OperatingCentersByModule[matchingModule];
                // OperatingCenter can be null for wildcard roles.
                return operatingCenters.Any(x => x == null || x.Id == operatingCenterId);
            }

            return false;
        }

        /// <summary>
        /// Returns true if this user is a UserAdministrator for a given module and operating center.
        /// </summary>
        /// <param name="module"></param>
        /// <param name="operatingCenter"></param>
        /// <returns></returns>
        public bool CanAdministrate(Module module, OperatingCenter operatingCenter)
        {
            return CanAdministrate(module.Id, operatingCenter?.Id);
        }

        /// <summary>
        /// Returns true if this user has UserAdministrator access for every role
        /// in the given RoleGroup.
        /// </summary>
        /// <param name="roleGroup"></param>
        /// <returns></returns>
        public bool CanAdministrate(RoleGroup roleGroup)
        {
            // Selecting the distinct modules/operating centers first reduces a bunch of redundant calls to
            // CanAdministrate when a module/operating center has multiple actions.
            var distinctModulesAndOperatingCenters = roleGroup.Roles.Select(x => new { x.Module, x.OperatingCenter }).Distinct();
            return distinctModulesAndOperatingCenters.All(x => CanAdministrate(x.Module, x.OperatingCenter));
        }

        #endregion
    }
}
