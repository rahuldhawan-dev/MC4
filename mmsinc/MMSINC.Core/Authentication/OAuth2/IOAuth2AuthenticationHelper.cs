using System;
using System.Collections.Specialized;

namespace MMSINC.Authentication.OAuth2
{
    /// <summary>
    /// Implementation of OAuth2 authentication tasks, specifically performing an initial redirect to ensure the user is
    /// authenticated a provider, and then using an authentication code provided in the resulting redirect back to
    /// identify the current user.
    /// </summary>
    public interface IOAuth2AuthenticationHelper : IDisposable
    {
        /// <summary>
        /// Redirect to the auth provider using <paramref name="redirectFn"/> to perform the actual redirection, to
        /// which a string containing a url to the provider and any necessary details such as client id will be
        /// provided. 
        /// </summary>
        /// <param name="redirectFn">
        /// <see cref="Action{T}"/> which can perform the actual browser redirect.  Most likely this should be or call
        /// <see cref="System.Web.HttpResponse.Redirect(string)"/>.
        /// </param>
        /// <param name="returnUrl">
        /// Optional url referencing the page the user had initially requested, so they can be redirected back after
        /// successful authentication.
        /// </param>
        void DoAuthRedirect(Action<string> redirectFn, string returnUrl);
        
        /// <summary>
        /// Handle the result provided by the redirect back from the authentication provider, and if successful gather
        /// some profile information about them (as well as the optional return url so they can be redirected back to
        /// the page they initially requested prior to authentication happening).
        /// </summary>
        /// <param name="queryString">
        /// Values passed via the query string from the authentication provider redirect.  Most likely this should be
        /// <see cref="System.Web.HttpRequest.QueryString"/>.
        /// </param>
        OAuth2AuthenticationResult HandleAuthenticationResult(NameValueCollection queryString);
    }
}
