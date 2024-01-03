using System.Configuration;

namespace MMSINC.Authentication.OAuth2
{
    /// <summary>
    /// Configuration information for connecting to an OAuth2 provider.
    /// </summary>
    public class OAuth2Config
    {
        #region Constants

        /// <summary>
        /// Key values used to store OAuth2 provider configuration data as AppSettings.
        /// </summary>
        public struct Keys
        {
            // ReSharper disable MissingXmlDoc
            public const string CLIENT_ID = "okta:ClientId",
                                CLIENT_SECRET = "okta:ClientSecret",
                                OKTA_DOMAIN = "okta:OktaDomain",
                                REDIRECT_URI = "okta:RedirectUri",
                                POST_LOGOUT_REDIRECT_URI = "okta:PostLogoutRedirectUri";
            // ReSharper restore MissingXmlDoc
        }

        #endregion
        
        #region Properties

        /// <summary>
        /// String representing the current application to the authentication provider.
        /// </summary>
        public string ClientId { get; }

        /// <summary>
        /// Secret key for making requests from the current application to the authentication provider.
        /// </summary>
        public string ClientSecret { get; }

        /// <summary>
        /// Url of the authentication provider server.
        /// </summary>
        public string OktaDomain { get; }

        /// <summary>
        /// Url which the authentication provider will redirect back to upon initial authentication.  This must match
        /// what the authentication provider has configured for the client application.
        /// </summary>
        public string RedirectUri { get; }

        /// <summary>
        /// Url which the authentication provider will redirect back to upon logout.
        /// </summary>
        public string PostLogoutRedirectUri { get; }
        
        #endregion
        
        #region Constructors

        /// <summary>
        /// Constructor for the <see cref="OAuth2Config"/> class.
        /// </summary>
        public OAuth2Config(
            string clientId,
            string clientSecret,
            string oktaDomain,
            string redirectUri,
            string postLogoutRedirectUri)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            OktaDomain = oktaDomain;
            RedirectUri = redirectUri;
            PostLogoutRedirectUri = postLogoutRedirectUri;
        }
        
        #endregion
        
        #region Exposed Methods

        /// <summary>
        /// Load the <see cref="OAuth2Config"/> from AppSettings.
        /// </summary>
        public static OAuth2Config Load()
        {
            return new OAuth2Config(
                ConfigurationManager.AppSettings[Keys.CLIENT_ID],
                ConfigurationManager.AppSettings[Keys.CLIENT_SECRET],
                ConfigurationManager.AppSettings[Keys.OKTA_DOMAIN],
                ConfigurationManager.AppSettings[Keys.REDIRECT_URI],
                ConfigurationManager.AppSettings[Keys.POST_LOGOUT_REDIRECT_URI]
            );
        }
        
        #endregion
    }
}
