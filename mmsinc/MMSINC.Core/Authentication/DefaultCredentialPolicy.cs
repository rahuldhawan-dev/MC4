namespace MMSINC.Authentication
{
    public interface ICredentialPolicy
    {
        #region Properties

        int MaximumFailedLoginAttemptCount { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns true if the given password meets security requirements.
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        bool PasswordMeetsRequirement(string password);

        #endregion
    }

    /// <summary>
    /// Default implementation of IPasswordRequirement that does not contain any actual requirements. 
    /// This is meant to be used for sites(MapCall) where login credentials are handled externally(SSO).  
    /// </summary>
    public sealed class DefaultCredentialPolicy : ICredentialPolicy
    {
        #region Properties

        public int MaximumFailedLoginAttemptCount => int.MaxValue;

        #endregion

        #region Public Methods

        public bool PasswordMeetsRequirement(string password)
        {
            return true;
        }

        #endregion
    }
}
