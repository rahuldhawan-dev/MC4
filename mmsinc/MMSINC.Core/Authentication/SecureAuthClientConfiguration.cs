namespace MMSINC.Authentication
{
    // NOTE: There's only one implementation for using SecureAuth at the moment.
    // This config could probably be split up into different objects depending
    // on whether particular values become dynamic.
    public class SecureAuthClientConfiguration
    {
        #region Properties

        public string BaseEndpointUrl { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        #endregion
    }
}
