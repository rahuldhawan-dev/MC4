using MMSINC.Configuration;

namespace MMSINC.Utilities.Kafka.Configuration
{
    public interface IKafkaServiceConfiguration
    {
        #region Abstract Properties

        IKafkaConfiguration KafkaConfig { get; }

        #endregion
    }
}
