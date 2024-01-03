using System;
using System.Diagnostics.CodeAnalysis;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;

namespace MapCallImporter.Library
{
    [ExcludeFromCodeCoverage]
    public class DangerousAuthenticationService : IAuthenticationService<User>
    {
        #region Properties

        public bool CurrentUserIsAdmin => true;
        public bool CurrentUserIsAuthenticated => true;
        public string CurrentUserIdentifier => throw new NotImplementedException();
        public int CurrentUserId => throw new NotImplementedException();
        public User CurrentUser => new User {Id = 402, IsAdmin = true};

        #endregion

        #region Exposed Methods

        public UserLoginAttemptStatus ValidateUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public void SignIn(int userId, bool isTokenAuthenticated)
        {
            throw new NotImplementedException();
        }

        public void SignOut()
        {
            throw new NotImplementedException();
        }

        public int GetUserId(string email)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
