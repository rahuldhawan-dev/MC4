using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.JobHelpers.NSIPremiseFileLink
{
    public class NSIPremiseFileLinkServiceConfiguration : INSIPremiseFileLinkServiceConfiguration
    {

        #region Constants

        public const string GROUP_NAME = "nsiPremiseFileLinkService";

        #endregion

        #region Properties

        public string GroupName => GROUP_NAME;
        public IFileConfigSection FileConfig => this.GetFileConfig();

        #endregion
    }

    public interface INSIPremiseFileLinkServiceConfiguration : IFileServiceConfiguration { }
}
