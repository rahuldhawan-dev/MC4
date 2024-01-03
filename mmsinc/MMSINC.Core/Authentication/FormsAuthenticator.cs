using System.Drawing;
using System.Web;
using System.Web.Security;
using StructureMap;

namespace MMSINC.Authentication
{
    /// <summary>
    /// Interface wrapper around FormsAuthentication so it can be tested.
    /// </summary>
    public interface IFormsAuthenticator
    {
        #region Properties

        string FormsCookieName { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates and sets the forms auth cookie in the response. Returns the cookie value.
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        string SetAuthCookie(IAuthenticationCookie cookie);

        void SignOut();

        string GetCurrentRequestAuthenticationCookie();

        #endregion
    }

    public class FormsAuthenticator : IFormsAuthenticator
    {
        #region Fields

        private readonly IContainer _container;

        #endregion

        #region Properties

        public string FormsCookieName
        {
            get { return FormsAuthentication.FormsCookieName; }
        }

        #endregion

        #region Constructor

        public FormsAuthenticator(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Exposed Methods

        public string SetAuthCookie(IAuthenticationCookie cookie)
        {
            // Set this to true to use a persistent cookie rather than one that expires after the browser
            // closes. iPad Safari/mobile browsers have a tendency to lose their session cookies when the
            // app gets dumped by the operating system and users complain about having to login repeatedly
            // through out the day because of it.
            const bool USE_PERSISTENT_COOKIE = true;
            var actualCookie = FormsAuthentication.GetAuthCookie(cookie.ToCookieString(), USE_PERSISTENT_COOKIE);
            HttpContext.Current.Response.Cookies.Set(actualCookie);
            return actualCookie.Value;
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public string GetCurrentRequestAuthenticationCookie()
        {
            var httpContext = _container.GetInstance<HttpContextBase>();
            // If this is not returning a value when you expect it to, make sure that it's
            // being set in HttpApplicationBase.OnBeginRequest.
            return (string)httpContext.Items[FormsAuthentication.FormsCookieName];
        }

        #endregion
    }
}
