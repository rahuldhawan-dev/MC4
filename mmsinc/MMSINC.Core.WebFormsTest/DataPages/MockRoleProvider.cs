using System;
using System.Collections.Generic;
using System.Linq;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    /// <summary>
    /// This is for mocking RoleProvider. It doesn't actually work like a real role provider. 
    /// </summary>
    public class MockRoleProvider : System.Web.Security.RoleProvider
    {
        private IEnumerable<string> _roles = new List<string>();
        private Dictionary<string, IEnumerable<string>> _userRoles = new Dictionary<string, IEnumerable<string>>();

        #region Implementation

        public override bool IsUserInRole(string username, string roleName)
        {
            return _userRoles[username].Contains(roleName);
        }

        public override string[] GetRolesForUser(string username)
        {
            if (_userRoles.ContainsKey(username))
            {
                return _userRoles[username].ToArray();
            }
            else
            {
                return new string[0];
            }
        }

        public void SetRolesForUser(string username, IEnumerable<string> roles)
        {
            _userRoles[username] = roles;
        }

        public void SetAllRoles(IEnumerable<string> roles)
        {
            _roles = roles;
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            return _roles.ToArray();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        #endregion
    }
}
