using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.JobHelpers.GIS
{
    public class GISFileDumpServiceConfiguration : IGISFileDumpServiceConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "gisFileDumpService";

        #endregion

        #region Properties

        public string GroupName => GROUP_NAME;
        public IFileConfigSection FileConfig => this.GetFileConfig();

        #endregion
    }

    public interface IGISFileDumpServiceConfiguration : IFileServiceConfiguration {}
}
