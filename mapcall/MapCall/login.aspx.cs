using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using log4net.Util;
using MMSINC.Authentication.OAuth2;

namespace MapCall
{
    public partial class login : Page
    {
        #region Private Members
        
        private IOAuth2AuthenticationHelper _authenticationHelper;
        
        #endregion
        
        #region Properties

        private IOAuth2AuthenticationHelper AuthenticationHelper =>
            _authenticationHelper ??
            (_authenticationHelper = DependencyResolver.Current.GetService<IOAuth2AuthenticationHelper>());
        
        #endregion

        #region Private Methods

        private void DoOpenAuthLogin()
        {
            AuthenticationHelper.DoAuthRedirect(Response.Redirect, Request.QueryString["ReturnUrl"]);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // To ensure the AuthorizationLogs are being marked as LoggedOut correctly, any
            // user accessing this page while currently logged in must be logged out.
            //
            // Of course, this blows up when the RedirectUrl is stupid
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                ((Global)Context.ApplicationInstance).LogOutUser(false);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Only display the legacy login form if the user is unable to sign in with SecureAuth.
            var awwUserIsNotMapCallUser = Request.QueryString.AllKeys.Contains("invalidUser");
            var userHasNoAccess = Request.QueryString.AllKeys.Contains("noAccess");
            var authenticationError = Request.QueryString.AllKeys.Contains("authError");

            var doTheAutomaticRedirectBecauseNoOneWantsToClickAButton = 
                !(awwUserIsNotMapCallUser || userHasNoAccess || authenticationError);

            if (doTheAutomaticRedirectBecauseNoOneWantsToClickAButton)
            {
                DoOpenAuthLogin();
            }
            else
            {
                divLogin.Visible = awwUserIsNotMapCallUser;
                divNoAccess.Visible = userHasNoAccess;
                divAuthError.Visible = authenticationError;

                if (authenticationError)
                {
                    AuthErrorDetails.Text = HttpUtility.UrlDecode(Request.QueryString["authError"]);
                }
            }
        }

        #endregion
    }
}
