using Newtonsoft.Json;

namespace MMSINC.Authentication.OAuth2
{
    /// <summary>
    /// Result of successfully requesting an access token from an authentication provider.
    /// </summary>
    public class OAuth2AccessTokenResponse
    {
        #region Properties
        
        /// <summary>
        /// Type of token provided, often just "Bearer".
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        
        /// <summary>
        /// String value representing the access token as issued by the authentication provider.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        
        /// <summary>
        /// Length of time until the provided access token will no longer be valid.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Scope(s) that the user has been granted (if different from the requested scope(s)).
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// ODIC token of the authenticated user.
        /// </summary>
        [JsonProperty("id_token")]
        public string IdToken { get; set; }
        
        #endregion
    }
}
