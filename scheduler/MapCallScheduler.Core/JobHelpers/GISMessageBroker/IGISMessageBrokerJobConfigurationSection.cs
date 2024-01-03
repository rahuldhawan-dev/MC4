using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.JobHelpers.GISMessageBroker
{
    public interface IGISMessageBrokerJobConfigurationSection
    {
        /// <summary>
        /// The number of records the broker should attempt to publish per job run
        /// </summary>
        int MaxRecordsToPublishPerJobRun { get; }
    }
}
