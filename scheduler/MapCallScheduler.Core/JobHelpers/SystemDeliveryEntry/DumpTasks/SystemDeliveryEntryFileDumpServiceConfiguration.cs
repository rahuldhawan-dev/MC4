using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks
{
    public class SystemDeliveryEntryFileDumpServiceConfiguration : ISystemDeliveryEntryFileDumpServiceConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "systemDeliveryEntryFileDumpService";

        #endregion

        #region Properties

        public string GroupName => GROUP_NAME;
        public IFileConfigSection FileConfig => this.GetFileConfig();

        #endregion
    }

    public interface ISystemDeliveryEntryFileDumpServiceConfiguration : IFileServiceConfiguration {}
}
