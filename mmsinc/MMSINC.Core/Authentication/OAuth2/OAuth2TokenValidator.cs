using System.Security.Authentication;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Utilities;

namespace MMSINC.Authentication.OAuth2
{
    /// <inheritdoc />
    public class OAuth2TokenValidator : IOAuth2TokenValidator
    {
        #region Private Members
        
        private readonly OAuth2Config _config;
        private readonly IDateTimeProvider _dateTimeProvider;
        
        #endregion
        
        #region Constructors

        /// <summary>
        /// Constructor for the <see cref="OAuth2TokenValidator"/> class.
        /// </summary>
        public OAuth2TokenValidator(OAuth2Config config, IDateTimeProvider dateTimeProvider)
        {
            _config = config;
            _dateTimeProvider = dateTimeProvider;
        }
        
        #endregion
        
        #region Exposed Methods

        /// <inheritdoc />
        public void Validate(OAuth2IdToken token)
        {
            if (!token.Issuer.StartsWith(_config.OktaDomain))
            {
                throw new AuthenticationException("Supplied token issuer did not match expected value");
            }

            var now = _dateTimeProvider.GetCurrentDate().SecondsSinceEpoch();

            if (token.ExpiryTime < now)
            {
                throw new AuthenticationException("Supplied token is expired");
            }
        }
        
        #endregion
    }
}
