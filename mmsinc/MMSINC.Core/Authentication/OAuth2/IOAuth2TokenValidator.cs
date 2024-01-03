namespace MMSINC.Authentication.OAuth2
{
    /// <summary>
    /// Validator for <see cref="OAuth2IdToken"/>s supplied by an authentication provider.
    /// </summary>
    public interface IOAuth2TokenValidator
    {
        /// <summary>
        /// Throws an exception if the issuer does not match the configured authentication server url, if the issued at
        /// time is in the future, or if the expiry time is in the past. 
        /// </summary>
        void Validate(OAuth2IdToken token);
    }
}
