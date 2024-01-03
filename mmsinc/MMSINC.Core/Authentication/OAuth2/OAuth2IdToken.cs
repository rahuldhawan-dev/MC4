using Newtonsoft.Json;

namespace MMSINC.Authentication.OAuth2
{
    /// <summary>
    /// Represents data passed back from the authentication provider in the form of an identity token.  The presence of
    /// any specific values will depend on the scope(s) initially requested.
    /// </summary>
    public class OAuth2IdToken
    {
        /// <summary>
        /// Name of the authenticated user.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// Identifier of the authentication provider (should match the url of the server authentication requests are
        /// made to).
        /// </summary>
        [JsonProperty("iss")]
        public string Issuer { get; set; }
        
        /// <summary>
        /// Unix time when token was issued.
        /// </summary>
        [JsonProperty("iat")]
        public int IssuedAtTime { get; set; }
        
        /// <summary>
        /// Unix time at which the token will expire.
        /// </summary>
        [JsonProperty("exp")]
        public int ExpiryTime { get; set; }
    }
}
