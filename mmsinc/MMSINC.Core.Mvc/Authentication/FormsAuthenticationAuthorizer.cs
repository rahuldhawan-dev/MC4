using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using MMSINC.ClassExtensions;
using MMSINC.Metadata;
using StructureMap;

namespace MMSINC.Authentication
{
    /// <summary>
    /// Authorizer that checks that a user is currently logged in.
    /// </summary>
    public class FormsAuthenticationAuthorizer : MvcAuthorizer
    {
        #region Consts

        public struct AppSettingsKeys
        {
            public const string LOGOUT_URL = "logoutUrl";
        }

        public const string LOGOUT_URL_NOT_FOUND =
            "No app setting with the key '" + AppSettingsKeys.LOGOUT_URL + "' could be found.";

        #endregion

        #region Private Methods

        protected virtual TokenValidationAttribute GetTokenValidationAttribute(AuthorizationArgs authArgs)
        {
            return GetAttributes<TokenValidationAttribute>(authArgs.Context).SingleOrDefault();
        }

        #endregion

        #region Public Methods

        public override void Authorize(AuthorizationArgs authArgs)
        {
            var context = authArgs.Context;

            // NOTE: Do the token validation first as AuthenticationService doesn't know
            //       how to handle tokens, so AuthServ will always fail in that case.

            // There should only ever be one TokenValidationAttribute
            var token = GetTokenValidationAttribute(authArgs);

            if (token != null)
            {
                // TODO: Do stuff with this attribute that calls its methods.
                token.OnAuthorization(authArgs.Context);
                // Token should set the context response.
            }
            else if (!AuthenticationService.CurrentUserIsAuthenticated)
            {
                DoFormsAuth(context);
            }
        }

        private static void DoFormsAuth(AuthorizationContext context)
        {
            var logoutUrl =
                ConfigurationManager.AppSettings[AppSettingsKeys.LOGOUT_URL];

            if (null == logoutUrl)
            {
                throw new ConfigurationErrorsException(LOGOUT_URL_NOT_FOUND);
            }

            // This is used by permits and contractors. Not sure why it stores the url in TempData
            // rather than including it in the url as a querystring param.
            context.Controller.SetRedirectUrl(context.RequestContext.HttpContext.Request.RawUrl);

            // So if the current user is considered authenticated, but their username doesn't exist in the database,
            // we want to log them off so they're no longer authenticated. Otherwise they'd have random access
            // to things until their authentication cookie expired.
            context.Result = new RedirectResult(logoutUrl);

            // For ajax requests, we don't want to return the login page redirect because that messes up
            // things when javascript expects a partial. 
            if (context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new JsonResult {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new {
                        success = false,
                        reason = "User is not logged in. Please login and try again."
                    }
                };
                context.HttpContext.Response.StatusCode = 401;
                context.HttpContext.Response.SuppressFormsAuthenticationRedirect = true;
            }
        }

        #endregion

        public FormsAuthenticationAuthorizer(IContainer container) : base(container) { }
    }
}
