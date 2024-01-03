using System.Configuration;

namespace MapCallScheduler.JobHelpers.GISMessageBroker
{
    /// <inheritdoc cref="IGISMessageBrokerJobConfigurationSection" />
    public class GISMessageBrokerJobConfigurationSection : ConfigurationSection, IGISMessageBrokerJobConfigurationSection
    {
        #region Constants

        public const int DEFAULT_MAX_RECORDS_TO_PUBLISH_PER_JOB_RUN = 1000;

        public struct ConfigurationKeys
        {
            public const string MAX_RECORDS_TO_PUBLISH_PER_JOB_RUN = "maxRecordsToPublishPerJobRun";
        }

        #endregion

        #region Properties

        [ConfigurationProperty(
            ConfigurationKeys.MAX_RECORDS_TO_PUBLISH_PER_JOB_RUN,
            DefaultValue = DEFAULT_MAX_RECORDS_TO_PUBLISH_PER_JOB_RUN)]
        public int MaxRecordsToPublishPerJobRun => (int)this[ConfigurationKeys.MAX_RECORDS_TO_PUBLISH_PER_JOB_RUN];

        #endregion
    }
}
