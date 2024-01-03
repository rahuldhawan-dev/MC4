using System;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories.Users;
using MMSINC.Authentication;
using MMSINC.Authentication.OAuth2;

namespace MapCall
{
    public partial class OAuth2Consume : System.Web.UI.Page
    {
        #region Constants

        public const string DEFAULT_REDIRECT_URL = "~/Modules/HR/Home.aspx";

        #endregion

        #region Private Members

        private IOAuth2AuthenticationHelper _authenticationHelper;
        private IUserRepository _userRepository;
        private IAuthenticationService _authenticationService;

        #endregion
        
        #region Properties

        private IOAuth2AuthenticationHelper AuthenticationHelper =>
            _authenticationHelper ??
            (_authenticationHelper = DependencyResolver.Current.GetService<IOAuth2AuthenticationHelper>());

        private IUserRepository UserRepository =>
            _userRepository ??
            (_userRepository = DependencyResolver.Current.GetService<IUserRepository>());

        private IAuthenticationService AuthenticationService =>
            _authenticationService ??
            (_authenticationService = DependencyResolver.Current.GetService<IAuthenticationService>());

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var result = AuthenticationHelper.HandleAuthenticationResult(Request.QueryString);
            
            User user;
            var redirectUrl = result.ReturnUrl;

            if (!result.Success)
            {
                // redirect back to login.
                redirectUrl = $"~/login.aspx?authError={HttpUtility.UrlEncode(result.Error)}";
            }
            else if ((user = UserRepository.TryGetUserByUserName(result.Username)) == null)
            {
                // redirect back to login.
                redirectUrl = "~/login.aspx?invalidUser=true";
            }
            else if (!user.HasAccess)
            {
                redirectUrl = "~/login.aspx?noAccess=true";
            }
            else
            {
                AuthenticationService.SignIn(user.Id, false);
            }

            Response.Redirect(redirectUrl ?? DEFAULT_REDIRECT_URL);
        }
        
        #endregion
    }
}