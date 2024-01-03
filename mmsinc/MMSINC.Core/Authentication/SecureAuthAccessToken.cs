namespace MMSINC.Authentication
{
    public sealed class SecureAuthAccessToken
    {
        #region Properties

        /// <summary>
        /// Gets the access token value that should be sent via the Authorization header. 
        /// </summary>
        public string AccessToken { get; set; }

        public string TokenType { get; set; }

        /// <summary>
        /// Gets the number of seconds this token is valid for. 
        /// </summary>
        public int ExpiresIn { get; set; }

        #endregion
    }
}
