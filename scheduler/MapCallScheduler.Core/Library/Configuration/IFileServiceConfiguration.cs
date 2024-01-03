using MMSINC.Configuration;

namespace MapCallScheduler.Library.Configuration
{
    public static class FileServiceConfigurationExtensions
    {
        #region Constants

        public const string SECTION_KEY = "file";

        #endregion

        #region Exposed Methods

        public static IFileConfigSection GetFileConfig(this IFileServiceConfiguration that)
        {
            return that.GetConfigSection<IFileConfigSection>(SECTION_KEY);
        }

        #endregion
    }

    public interface IFileServiceConfiguration : IGroupedServiceConfiguration
    {
        #region Abstract Properties

        IFileConfigSection FileConfig { get; }

        #endregion
    }
}