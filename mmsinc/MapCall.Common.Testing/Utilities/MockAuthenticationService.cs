using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using Moq;

namespace MapCall.Common.Testing.Utilities
{
    public class MockAuthenticationService<TUser> : Mock<IAuthenticationService<TUser>>, IAuthenticationService<TUser>
        where TUser : IAdministratedUser
    {
        #region Properties

        public bool CurrentUserIsAdmin => Object.CurrentUserIsAdmin;

        public bool CurrentUserIsAuthenticated => Object.CurrentUserIsAuthenticated;

        public string CurrentUserIdentifier => Object.CurrentUserIdentifier;

        public int CurrentUserId => Object.CurrentUserId;

        public TUser CurrentUser => Object.CurrentUser;

        #endregion

        #region Constructors

        public MockAuthenticationService(TUser user)
        {
            SetUser(user);
        }

        #endregion

        #region Exposed Methods

        public void SetUser(TUser user)
        {
            SetupGet(x => x.CurrentUser).Returns(user);
        }

        public UserLoginAttemptStatus ValidateUser(string email, string password)
        {
            return Object.ValidateUser(email, password);
        }

        public void SignIn(int userId, bool isTokenAuthenticated)
        {
            Object.SignIn(userId, isTokenAuthenticated);
        }

        public void SignOut()
        {
            Object.SignOut();
        }

        public int GetUserId(string email)
        {
            return Object.GetUserId(email);
        }

        #endregion
    }

    public class MockAuthenticationService : MockAuthenticationService<User>
    {
        #region Constructors

        public MockAuthenticationService(User user) : base(user) { }

        #endregion
    }
}
