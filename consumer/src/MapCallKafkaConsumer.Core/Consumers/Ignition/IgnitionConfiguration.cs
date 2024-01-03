using MMSINC.Configuration;
using MMSINC.Utilities.Kafka.Configuration;

namespace MapCallKafkaConsumer.Consumers.Ignition
{
    public class IgnitionConfiguration : IGroupedServiceConfiguration, IIgnitionConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "ignition";

        #endregion

        #region Properties

        public string GroupName => GROUP_NAME;

        public IKafkaConfiguration KafkaConfig => this.GetConfigSection<IKafkaConfiguration>("kafka");

        #endregion
    }
}
