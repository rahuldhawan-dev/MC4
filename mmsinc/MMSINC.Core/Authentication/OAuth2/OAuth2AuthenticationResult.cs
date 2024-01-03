namespace MMSINC.Authentication.OAuth2
{
    /// <summary>
    /// Result from successfully authenticating the current user with an OAuth2 provider.
    /// </summary>
    public class OAuth2AuthenticationResult
    {
        #region Properties
        
        /// <summary>
        /// Name of the recently-authenticated user.
        /// </summary>
        public string Username { get; }
        
        /// <summary>
        /// Url of the page the as-yet unauthenticated user had initially requested, or null if there wasn't one. 
        /// </summary>
        public string ReturnUrl { get; }
        
        /// <summary>
        /// Boolean indicating whether or not the authentication attempt was successful. 
        /// </summary>
        public bool Success { get; }
        
        /// <summary>
        /// If the authentication attempt was unsuccessful (see <see cref="Success"/>), this will contain the error
        /// message that was supplied.
        /// </summary>
        public string Error { get; }

        #endregion
        
        #region Constructors
        
        public OAuth2AuthenticationResult(string username, string returnUrl)
        {
            Success = true;
            Username = username;
            ReturnUrl = returnUrl;
        }

        public OAuth2AuthenticationResult(string error)
        {
            Success = false;
            Error = error;
        }
        
        #endregion
    }
}
