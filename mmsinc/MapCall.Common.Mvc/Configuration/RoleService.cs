using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using JetBrains.Annotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MMSINC.Authentication;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.Common.Configuration
{
    public interface IRoleService : IBasicRoleService
    {
        #region Abstract Properties

        /// <summary>
        /// Returns the current user's roles. If the user's not logged in, returns an empty collection.
        /// </summary>
        IEnumerable<AggregateRole> CurrentUserRoles { get; }

        #endregion

        #region Abstract Methods

        IEnumerable<RequiresRoleAttribute> GetRequiredRolesForRoute(RequestContext context,
            [AspMvcAction] string action,
            [AspMvcController] string controller, [AspMvcArea] string area = null);

        IEnumerable<RequiresRoleAttribute> GetRequiredRolesForCurrentRoute(bool throwIfMissing = true);
        IEnumerable<User> GetUsersWithAccessToCurrentRoute(OperatingCenter opCenter = null);

        #endregion
    }

    public class RoleService : BasicRoleService, IRoleService
    {
        #region Fields

        private readonly IUserRepository _userRepository;

        #endregion

        #region Properties

        public IEnumerable<AggregateRole> CurrentUserRoles
        {
            get
            {
                return _authServ.CurrentUserIsAuthenticated ? _authServ.CurrentUser.AggregateRoles : Enumerable.Empty<AggregateRole>();
            }
        }

        #endregion

        #region Constructors

        public RoleService(IAuthenticationService<User> authServ, IUserRepository userRepository) : base(authServ)
        {
            _userRepository = userRepository;
        }

        #endregion

        #region Private Methods

        private static IEnumerable<RequiresRoleAttribute> GetRequiredRolesForRouteContext(RouteContext routeContext)
        {
            var allRequiredRoles = new List<RequiresRoleAttribute>();
            // Need to do a bunch of if-checkin' if the RouteContext is set not to throw when
            // a controller or action is missing.
            allRequiredRoles.AddRange(
                routeContext.ControllerDescriptor.GetFilterAttributes(true).OfType<RequiresRoleAttribute>());

            allRequiredRoles.AddRange(
                routeContext.ActionDescriptor.GetFilterAttributes(true).OfType<RequiresRoleAttribute>());

            return allRequiredRoles;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<RequiresRoleAttribute> GetRequiredRolesForRoute(RequestContext context,
            [AspMvcAction] string action, [AspMvcController] string controller, [AspMvcArea] string area = null)
        {
            RouteContext routeContext = null;

            try
            {
                routeContext = new RouteContext(context, controller, action, area);
            }
            catch (RouteContextException)
            {
                // Means the controller or action doesn't exist. This is valid for actions as a controller may
                // not have said action and this is being called from a shared view.
                return Enumerable.Empty<RequiresRoleAttribute>();
            }

            return GetRequiredRolesForRouteContext(routeContext);
        }

        public IEnumerable<RequiresRoleAttribute> GetRequiredRolesForCurrentRoute(
            bool throwIfMissingControllerOrAction = true)
        {
            RouteContext routeContext = null;
            try
            {
                routeContext = new RouteContext(HttpContext.Current.Request.RequestContext);
            }
            catch (Exception)
            {
                if (!throwIfMissingControllerOrAction)
                {
                    return Enumerable.Empty<RequiresRoleAttribute>();
                }

                throw;
            }

            // TODO(Maybe):
            // Split this into another method that accepts a controller descriptor and action descriptor, that way the
            // RoleAuthorizationFilter can use it. Will need to return a custom class that has controller and action roles
            // as separate properties.
            return GetRequiredRolesForRouteContext(routeContext);
        }

        public IEnumerable<User> GetUsersWithAccessToCurrentRoute(OperatingCenter opCenter = null)
        {
            var opCenterId = opCenter == null ? (int?)null : opCenter.Id;
            var roles = GetRequiredRolesForCurrentRoute().ToArray();

            if (!roles.Any())
            {
                return null;
            }

            var firstRole = roles.First();
            var users = _userRepository
               .GetUsersWithRole((int)firstRole.Module, opCenterId, (int)firstRole.Action, userHasAccess: null);

            for (var i = 1; i < roles.Count(); ++i)
            {
                var role = roles[i];
                users = (from u in users
                         where u.GetCachedMatchingRoles(role.Module, role.Action, opCenter).CanAccessRole
                         select u);
            }

            return users;
        }

        #endregion
    }
}
