using MMSINC.Configuration;

namespace MapCallScheduler.Library.Configuration
{
    public static class IncomingEmailServiceConfigurationExtensions
    {
        #region Constants

        public const string SECTION_KEY = "incomingEmail";

        #endregion

        #region Extension Methods

        public static IIncomingEmailConfigSection GetIncomingEmailConfig(this IIncomingEmailServiceConfiguration that)
        {
            return that.GetConfigSection<IIncomingEmailConfigSection>(SECTION_KEY);
        }

        #endregion
    }

    /// <summary>
    /// Represents a set of configuration data which has a section pertaining to
    /// connecting to an Imap server for incoming email.
    /// </summary>
    public interface IIncomingEmailServiceConfiguration : IGroupedServiceConfiguration
    {
        #region Abstract Properties

        /// <summary>
        /// This should be defined using the extension method so that it can be overridden in tests.
        /// </summary>
        IIncomingEmailConfigSection IncomingEmailConfig { get; }

        #endregion
    }
}