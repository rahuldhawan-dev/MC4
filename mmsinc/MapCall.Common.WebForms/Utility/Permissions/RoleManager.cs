using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Interface;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Utility.Permissions.Roles;
using MMSINC.Data.NHibernate;
using StructureMap;

namespace MapCall.Common.Utility.Permissions
{
    // TODO: At this point, I'm pretty sure RoleManager is mostly useless and can be replaced by
    // RoleService wherever it's currently being used. However, that's a problem for another day. -Ross 4/14/2023
    public class RoleManager : IRoleManager
    {
        #region Constants

        public const string OPERATING_CENTER_CODE_ALL = "ALL";

        /// <summary>
        /// Set to a negative number so it's impossible to set in the database. This needs
        /// to be set to null when used. 
        /// </summary>
        public const int OPERATING_CENTER_ID_ALL = -1;

        internal const string ROLE_MANAGE_CONTEXT_ITEMS_KEY = "RoleManager";

        #endregion

        #region Structs

        public struct CreateRoleCacheParams
        {
            public const string USERNAME = "UserName";
        }

        #endregion

        #region Fields

        private static string _connectionString;
        private static ILookupCache _sharedLookupCache;

        // Cache by string instead of IUser, because different IUser instances may refer to the same user. 
        private ILookupCache _instanceLookupCache;
        private IRoleCacheManager _cacheManager;
        private readonly IContainer _container;

        #endregion

        #region Properties

        #region Static

        /// <summary>
        /// Returns the RoleManager instance responsible for the current HttpContext.
        /// Setter is there for ultimate testability.
        /// 
        /// Though maybe this should create an instance-per-request and store that
        /// in the HttpContext.Items. That probably makes more sense AMIRITE. 
        /// 
        /// Also I still think we can use enums. Have this deal with grabbing the necessary
        /// data from the db, caching it, then matching it up and returning a Role instance.
        /// 
        /// Unless the User object ever needs to save crap.
        /// </summary>
        public static IRoleManager Current
        {
            get
            {
                var cur = (IRoleManager)Context.Items[ROLE_MANAGE_CONTEXT_ITEMS_KEY];
                if (cur == null)
                {
                    cur = DependencyResolver.Current.GetService<IContainer>().GetInstance<RoleManager>();
                    Context.Items[ROLE_MANAGE_CONTEXT_ITEMS_KEY] = cur;
                }

                return cur;
            }
        }

        private static IHttpContext Context
        {
            get { return DependencyResolver.Current.GetService<IHttpContext>(); }
        }

        public static string ConnectionString
        {
            get
            {
                // TODO: Test
                if (_connectionString == null)
                {
                    var config = RoleManagerConfiguration.GetRoleManagerConfiguration();
                    var csName = WebConfigurationManager.ConnectionStrings[config.ConnectionStringName];
                    if (csName == null)
                    {
                        throw new NullReferenceException("Unable to locate ConnectionString in web.config");
                    }

                    _connectionString = csName.ConnectionString;
                }

                return _connectionString;
            }
            set { _connectionString = value; }
        }

        internal static ILookupCache SharedLookup
        {
            get
            {
                if (_sharedLookupCache == null)
                {
                    var cache = new LookupCache(ConnectionString);
                    AddAllOperatingCenterToLookupCache(cache);
                    _sharedLookupCache = cache;
                }

                return _sharedLookupCache;
            }
        }

        /// <summary>
        /// Internal for testing purposes only.
        /// </summary>
        /// <param name="cache"></param>
        internal static void AddAllOperatingCenterToLookupCache(ILookupCache cache)
        {
            var allOp = new OperatingCenter {
                OperatingCenterId = OPERATING_CENTER_ID_ALL,
                OperatingCenterCode = OPERATING_CENTER_CODE_ALL,
            };
            cache.OperatingCenters.Add(OPERATING_CENTER_ID_ALL, allOp);
            cache.OperatingCentersByName.Add(OPERATING_CENTER_CODE_ALL, allOp);
        }

        #endregion

        public ILookupCache Lookup
        {
            get
            {
                if (_instanceLookupCache != null)
                {
                    return _instanceLookupCache;
                }

                return SharedLookup;
            }
            internal set { _instanceLookupCache = value; }
        }

        internal IRoleCacheManager CacheManager
        {
            get
            {
                if (_cacheManager == null)
                {
                    _cacheManager = CreateRoleCacheManager();
                }

                return _cacheManager;
            }
            set { _cacheManager = value; }
        }

        #endregion

        #region Constructors

        public RoleManager(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Private Methods

        protected virtual IDbConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// This method's sole existance is because these two parameters get null checked frequently.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="role"></param>
        private static void VerifyUserAndRoleParameters(IUser user, IRole role)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
        }

        internal RoleCacheManager CreateRoleCacheManager()
        {
            var rcm = new RoleCacheManager();
            rcm.RoleCreatorHandler = CreateRoleCache;
            return rcm;
        }

        /// <summary>
        /// Creates a UserRoleCache for a user. This only internal virtual for testing.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected IEnumerable<IRole> CreateRoleCache(IUser user)
        {
            // Not uber-thrilled with having the sql here. 
            // OperatingCenters needs to be a left join because we're using
            // null to represent the "ALL" operating centers value.
            const string sql = @"select 
                                   r.UserRoleID,
                                   r.UserID, 
                                   r.OperatingCenterID,
                                   app.ApplicationID, 
                                   r.ModuleID,
                                   r.ActionID
                               from [AggregateRoles] r 
                               inner join [Modules] module on module.ModuleID = r.ModuleID 
                               inner join [Applications] app on app.ApplicationID = module.ApplicationID
                               inner join [tblPermissions] p on p.RecId = r.UserId
                               where p.UserName = @UserName";

            using (var conn = GetConnection())
            using (var com = conn.CreateCommand())
            {
                com.CommandText = sql;
                com.AddParameter("UserName", user.Name);
                conn.Open();

                using (var reader = com.ExecuteReader())
                {
                    return ReadRoles(reader);
                }
            }
        }

        protected IEnumerable<IRole> ReadRoles(IDataReader reader)
        {
            var roles = new List<IRole>();

            // Grabbing these once so we aren't repeatedly looking
            // up the ordinal on every loop.
            var userIdOrd = reader.GetOrdinal("UserID");
            var opCenterIdOrd = reader.GetOrdinal("OperatingCenterID");
            var appIdOrd = reader.GetOrdinal("ApplicationID");
            var moduleIdOrd = reader.GetOrdinal("ModuleID");
            var actionIdOrd = reader.GetOrdinal("ActionID");

            while (reader.Read())
            {
                var r = new Role();
                r.UserId = reader.GetInt32(userIdOrd);
                r.OperatingCenterId = !reader.IsDBNull(opCenterIdOrd)
                    ? reader.GetInt32(opCenterIdOrd)
                    : OPERATING_CENTER_ID_ALL;
                r.ApplicationId = reader.GetInt32(appIdOrd);
                r.ModuleId = reader.GetInt32(moduleIdOrd);
                r.ActionId = reader.GetInt32(actionIdOrd);

                HydrateRoleStringProperties(r);

                roles.Add(r);
            }

            return roles;
        }

        /// <summary>
        /// Sets the various name properties on a role instance.
        /// </summary>
        /// <param name="r"></param>
        protected void HydrateRoleStringProperties(Role r)
        {
            if (!Lookup.Applications.ContainsKey(r.ApplicationId) ||
                !Lookup.Modules.ContainsKey(r.ModuleId) ||
                !Lookup.OperatingCenters.ContainsKey(r.OperatingCenterId) ||
                !Lookup.Actions.ContainsKey(r.ActionId))
            {
                Lookup.Reinitialize();
            }

            r.Application = Lookup.Applications[r.ApplicationId].Name;
            r.Module = Lookup.Modules[r.ModuleId].Name;
            r.OperatingCenter = Lookup.OperatingCenters[r.OperatingCenterId].OperatingCenterCode;
            r.Action = Lookup.Actions[r.ActionId].Name;
        }

        protected IEnumerable<IRole> GetMatchingPermissionRoles(IPermissionsObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            var roles = GetAllRolesForUser(obj.User);
            var appId =
                Lookup.ApplicationsByName[obj.SpecificPermissions.Application].ApplicationId;

            return (from r in roles
                    where r.ApplicationId == appId &&
                          r.Module.Equals(obj.SpecificPermissions.Module,
                              StringComparison.InvariantCultureIgnoreCase) &&
                          r.ActionId == (int)obj.Action
                    select r);
        }

        #endregion

        #region Public Methods

        public IEnumerable<IRole> GetAllRolesForUser(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return CacheManager.GetAllRolesForUser(user);
        }

        public bool UserCanAdministrateRole(IUser user, IRole role)
        {
            VerifyUserAndRoleParameters(user, role);

            // NOTE: This should be using AuthenticationService.CurrentUserIsAdmin, but the RoleControl 
            //       needs to check roles for both the current user and the user they are editing, making
            //       it impossible to use here.
            if (_container.GetInstance<IRepository<User>>().GetUserByUserName(user.Name).IsAdmin)
            {
                return true;
            }

            return (from r in GetAllRolesForUser(user)
                    where r.ApplicationId == role.ApplicationId
                          && r.ModuleId == role.ModuleId
                          && r.ActionId == (int)ModuleAction.Administrate
                          && (r.OperatingCenterId == role.OperatingCenterId
                              || r.OperatingCenterId == OPERATING_CENTER_ID_ALL)
                    select r).Any();
        }

        public bool UserIsInRole(IPermissionsObject obj)
        {
            return GetMatchingPermissionRoles(obj).Any();
        }

        // I find this sloppy for some reason. 
        public bool UserIsInRoleWithOperatingCenter(IPermissionsObject obj, string opCntrCode)
        {
            if (!Lookup.OperatingCentersByName.ContainsKey(opCntrCode))
            {
                Lookup.Reinitialize();
            }

            return (from r in GetMatchingPermissionRoles(obj)
                    where r.OperatingCenterId == Lookup.OperatingCentersByName[opCntrCode].OperatingCenterId
                          || r.OperatingCenterId == OPERATING_CENTER_ID_ALL
                    select r).Any();
        }

        #endregion
    }
}
