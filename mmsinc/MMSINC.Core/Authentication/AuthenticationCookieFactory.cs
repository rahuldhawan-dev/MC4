using StructureMap;

namespace MMSINC.Authentication
{
    public interface IAuthenticationCookieFactory
    {
        IAuthenticationCookie CreateCookie(string rawCookieValue);
        IAuthenticationCookie CreateCookie(int id, string userName);

        /// <summary>
        /// Create an uninitialized cookie instance.
        /// </summary>
        /// <returns></returns>
        IAuthenticationCookie CreateEmptyCookie();
    }

    public class AuthenticationCookieFactory : IAuthenticationCookieFactory
    {
        protected readonly IContainer _container;

        public AuthenticationCookieFactory(IContainer container)
        {
            _container = container;
        }

        public IAuthenticationCookie CreateCookie(string rawCookieValue)
        {
            return new AuthenticationCookie(_container, rawCookieValue);
        }

        public IAuthenticationCookie CreateCookie(int id, string userName)
        {
            return new AuthenticationCookie(id, userName);
        }

        public IAuthenticationCookie CreateEmptyCookie()
        {
            return new AuthenticationCookie();
        }
    }
}
