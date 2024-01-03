using System;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories.Users;
using MMSINC.Authentication;

namespace MapCall
{
    public partial class Impersonate : System.Web.UI.Page
    {
#if DEBUG

        // NOTE: This exists for regression tests only as a means of bypassing SecureAuth/SSO.
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // The regression tests hit this page to test that the server is running. We don't want to throw
            // errors here if the username isn't set. 

            var userName = Request.QueryString["username"];

            if (!string.IsNullOrWhiteSpace(userName))
            {
                var user = DependencyResolver.Current.GetService<IUserRepository>().TryGetUserByUserName(userName);

                if (user == null)
                {
                    throw new InvalidOperationException($"Unable to find user '{userName}'");
                }

                DependencyResolver.Current.GetService<IAuthenticationService>().SignIn(user.Id, isTokenAuthenticated: false);
            }

            Response.Redirect(OAuth2Consume.DEFAULT_REDIRECT_URL);
        }

#endif
    }
}