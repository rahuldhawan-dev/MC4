using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Interface;
using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions.Roles
{
    internal class RoleCacheManager : IRoleCacheManager
    {
        #region Fields

        private readonly IDictionary<string, UserRoleCache> _cache =
            new Dictionary<string, UserRoleCache>(StringComparer.InvariantCultureIgnoreCase);

        #endregion

        #region Properties

        /// <summary>
        /// Set this to a method that handles getting the IRole objects from the database.
        /// Basically, RoleManager should provide this.
        /// </summary>
        public Func<IUser, IEnumerable<IRole>> RoleCreatorHandler { get; set; }

        #endregion

        #region Private Methods

        private UserRoleCache CreateCache(IUser user)
        {
            if (RoleCreatorHandler == null)
            {
                throw new NullReferenceException("RoleCreatorHandler must be set.");
            }

            var roles = RoleCreatorHandler(user);
            return new UserRoleCache(roles);
        }

        protected bool HasCache(IUser user)
        {
            return _cache.ContainsKey(user.Name);
        }

        private UserRoleCache GetCache(IUser user)
        {
            if (!HasCache(user))
            {
                _cache.Add(user.Name, CreateCache(user));
            }

            return _cache[user.Name];
        }

        #endregion

        #region Public Methods

        public IEnumerable<IRole> GetAllRolesForUser(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return GetCache(user).Roles;
        }

        //public void AddRole(IUser user, IRole role)
        //{
        //    // If we didn't cache the user yet then we haven't
        //    // read from the database, so reading from the db
        //    // just to remove a role from the cache would be
        //    // dumb.
        //    if (!HasCache(user))
        //    {
        //        return;
        //    }

        //    GetCache(user).Roles.Add(role);
        //}

        //public void RemoveRole(IUser user, IRole role)
        //{
        //    // If we didn't cache the user yet then we haven't
        //    // read from the database, so reading from the db
        //    // just to remove a role from the cache would be
        //    // dumb.
        //    if (!HasCache(user))
        //    {
        //        return;
        //    }

        //    GetCache(user).Roles.Remove(role);
        //}

        #endregion

        /// <summary>
        /// Simple class for dealing with role caching for a single user. 
        /// It's not meant to be used outside this class, but it's internal
        /// for testing purposes.
        /// </summary>
        private sealed class UserRoleCache
        {
            #region Properties

            public ICollection<IRole> Roles { get; private set; }

            #endregion

            #region Constructors

            public UserRoleCache(IEnumerable<IRole> roles)
            {
                // Do not make ReadOnly kthx.
                this.Roles = roles.ToList();
            }

            #endregion
        }
    }
}
