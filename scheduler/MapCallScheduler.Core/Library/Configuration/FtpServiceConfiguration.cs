using MMSINC.Configuration;

namespace MapCallScheduler.Library.Configuration
{
    public static class FtpServiceConfigurationExtensions
    {
        #region Constants

        public const string SECTION_KEY = "ftp";

        #endregion

        #region Extension Methods

        public static IFtpConfigSection GetFtpConfig(this IFtpServiceConfiguration that)
        {
            return that.GetConfigSection<IFtpConfigSection>(SECTION_KEY);
        }

        #endregion
    }

    public interface IFtpServiceConfiguration : IGroupedServiceConfiguration
    {
        #region Abstract Properties

        IFtpConfigSection FtpConfig { get; }

        #endregion
    }
}