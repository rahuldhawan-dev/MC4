using MMSINC.Configuration;
using MMSINC.Utilities.Kafka.Configuration;

namespace MapCallKafkaConsumer.Consumers.Lims
{
    public class LimsConfiguration : IGroupedServiceConfiguration, ILimsConfiguration
    {
        #region Constants

        public const string GROUP_NAME = "lims";

        #endregion

        #region Properties

        public string GroupName => GROUP_NAME;

        public IKafkaConfiguration KafkaConfig => this.GetConfigSection<IKafkaConfiguration>("kafka");

        #endregion
    }
}
