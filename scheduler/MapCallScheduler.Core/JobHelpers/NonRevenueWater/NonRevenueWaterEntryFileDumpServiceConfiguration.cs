using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.JobHelpers.NonRevenueWater
{
    public class NonRevenueWaterEntryFileDumpServiceConfiguration : INonRevenueWaterEntryFileDumpServiceConfiguration
    {
        #region Constants

        private const string GROUP_NAME = "nonRevenueWaterEntryFileDumpService";

        #endregion

        #region Properties

        public string GroupName => GROUP_NAME;
        
        public IFileConfigSection FileConfig => this.GetFileConfig();

        #endregion
    }
}
