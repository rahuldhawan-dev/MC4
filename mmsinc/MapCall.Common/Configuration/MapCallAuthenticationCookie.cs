using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using StructureMap;

namespace MapCall.Common.Configuration
{
    /// <summary>
    /// IAuthenticationCookie implementation that deals with the WebForms Forms Authentication
    /// junk that WebForms MapCall uses.
    /// </summary>
    public class MapCallAuthenticationCookie<TUser> : AuthenticationCookie
        where TUser : IAdministratedUser
    {
        #region Properties

        public override bool IsValidlyFormatted
        {
            get
            {
                // Override needed because base class also does an IsValidEmail check on UserName.
                return (Id.GetValueOrDefault() > 0 && !string.IsNullOrWhiteSpace(UserName));
            }
        }

        #endregion

        #region Constructor

        public MapCallAuthenticationCookie() { }
        public MapCallAuthenticationCookie(int id, string userName) : base(id, userName) { }

        public MapCallAuthenticationCookie(IContainer container,
            string rawCookieValue) : base(container, rawCookieValue) { }

        #endregion

        #region Private Methods

        protected override void ParseAndSetValues(IContainer container, string rawCookieValue)
        {
            // Don't call base, it's not gonna help anything.

            if (!string.IsNullOrWhiteSpace(rawCookieValue))
            {
                UserName = rawCookieValue;
                var existingUser = container.GetInstance<IAuthenticationRepository<TUser>>().GetUser(rawCookieValue);
                if (existingUser != null)
                {
                    Id = existingUser.Id;
                }
            }
        }

        #endregion

        #region Public Methods

        public override string ToCookieString()
        {
            // MapCall's cookies only have the UserName.
            return UserName;
        }

        #endregion
    }

    public class MapCallAuthenticationCookieFactory<TUser> : IAuthenticationCookieFactory
        where TUser : IAdministratedUser
    {
        private readonly IContainer _container;

        public MapCallAuthenticationCookieFactory(IContainer container)
        {
            _container = container;
        }

        public IAuthenticationCookie CreateCookie(string rawCookieValue)
        {
            return new MapCallAuthenticationCookie<TUser>(_container, rawCookieValue);
        }

        public IAuthenticationCookie CreateCookie(int id, string userName)
        {
            return new MapCallAuthenticationCookie<TUser>(id, userName);
        }

        public IAuthenticationCookie CreateEmptyCookie()
        {
            return new MapCallAuthenticationCookie<TUser>();
        }
    }
}
