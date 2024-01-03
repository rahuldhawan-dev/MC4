using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MMSINC.Authentication;
using StructureMap;

namespace MMSINC.Filters
{
    public class UserStorageFilter : IActionFilter
    {
        #region Consts

        public struct CookieKeys
        {
            public const string NAME = "UserStorage",
                                KEY = "key",
                                ENABLED = "enabled";
        }

        #endregion

        #region Fields

        private readonly IContainer _container;

        #endregion

        #region Properties

        public bool Enabled
        {
            get
            {
                // ReSharper disable once RedundantNameQualifier
                return !MMSINC.MvcApplication.IsInTestMode;
            }
        }

        #endregion

        public UserStorageFilter(IContainer container)
        {
            _container = container;
        }

        #region Private Methods

        private HttpCookie GetDisabledCookie()
        {
            var cookie = new HttpCookie(CookieKeys.NAME);
            cookie.Values.Add(CookieKeys.ENABLED, "false");
            return cookie;
        }

        private HttpCookie GetEnabledCookie(string identifier)
        {
            var cookie = new HttpCookie(CookieKeys.NAME);
            cookie.Values.Add(CookieKeys.KEY, identifier);
            cookie.Values.Add(CookieKeys.ENABLED, "true");
            return cookie;
        }

        #endregion

        #region Public Methods

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var http = filterContext.HttpContext;

            // For reasons I do not understand, allowing this to execute during
            // regressions causes all sorts of sql errors. We don't want this
            // enabled during regressions anyway.
            if (!Enabled)
            {
                http.Response.Cookies.Add(GetDisabledCookie());
                return;
            }

            // If this is a ResourcesController request, we want to ignore it. This will reduce
            // duplicate calls to the db just to check the authentication cookie. 
            var controllerName = filterContext.RouteData.Values["controller"] as string ?? string.Empty;
            if (controllerName.Equals("Resources", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            var authServ = _container.GetInstance<IAuthenticationService>();
            var identifier = (authServ.CurrentUserIsAuthenticated ? authServ.CurrentUserIdentifier : string.Empty);
            var requestCookie = http.Request.Cookies[CookieKeys.NAME];

            // The clientside script has functionality setup so that it can pretend that it works 
            // when there isn't localStorage available(IE7), or for during regression testing when
            // we do not want stored form-state values messing up tests by pre-filling in values.
            // localStorage is only made enabled if the user is currently logged in. 

            var isEnabled = (!string.IsNullOrWhiteSpace(identifier) && Enabled).ToString().ToLower();

            // We're only returning a new cookie as values change(to save on all 30 bytes of bandwidth
            // that it'd take to send this on every request). So this is what this is saying:
            //		1. If there's no request cookie, then the user doesn't have a cookie at all and we need to set one.
            //      2. If there is a cookie, but the key is different from what we're expecting, then we need to 
            //         send a new one(because a different user has logged in, or the current user logged out).
            //      3. If the enabled property has changed for any reason, that also needs to reset the cookie.
            if (requestCookie == null || requestCookie[CookieKeys.KEY] != identifier ||
                requestCookie[CookieKeys.ENABLED] != isEnabled)
            {
                if (!string.IsNullOrWhiteSpace(identifier))
                {
                    http.Response.Cookies.Add(GetEnabledCookie(identifier));
                }
                else
                {
                    http.Response.Cookies.Add(GetDisabledCookie());
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // noop
        }

        #endregion
    }
}
