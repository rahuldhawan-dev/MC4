using MapCallScheduler.Library.Configuration;
using MMSINC.Configuration;
using MMSINC.Utilities.Kafka.Configuration;

namespace MapCallScheduler.JobHelpers.GISMessageBroker
{
    /// <inheritdoc cref="IGISMessageBrokerConfiguration" />
    public class GISMessageBrokerConfiguration : IGISMessageBrokerConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "gisMessageBroker";

        #endregion

        #region Properties

        public string GroupName => GROUP_NAME;

        public IKafkaConfiguration KafkaConfig => this.GetConfigSection<IKafkaConfiguration>("kafka");

        public IGISMessageBrokerJobConfigurationSection JobConfig => this.GetConfigSection<IGISMessageBrokerJobConfigurationSection>("jobSettings");

        #endregion
    }

    public interface IGISMessageBrokerConfiguration : IGroupedServiceConfiguration, IKafkaServiceConfiguration
    {
        IGISMessageBrokerJobConfigurationSection JobConfig { get; }
    }
}
