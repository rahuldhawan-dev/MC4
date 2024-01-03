using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.JobHelpers.W1V
{
    public class W1VFileImportServiceConfiguration : IW1VFileImportServiceConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "w1vFileImportService";

        #endregion

        #region Properties

        public string GroupName => GROUP_NAME;
        public IFileConfigSection FileConfig => this.GetFileConfig();

        #endregion
    }

    public interface IW1VFileImportServiceConfiguration : IFileServiceConfiguration {}
}
