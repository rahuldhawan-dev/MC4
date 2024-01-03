using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.JobHelpers.GIS
{
    public class GISFileImportServiceConfiguration : IGISFileImportServiceConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "gisFileImportService";

        #endregion

        #region Properties

        public string GroupName => GROUP_NAME;
        public IFileConfigSection FileConfig => this.GetFileConfig();

        #endregion
    }

    public interface IGISFileImportServiceConfiguration : IFileServiceConfiguration {}
}
