using Confluent.Kafka;
using System.Configuration;

namespace MMSINC.Utilities.Kafka.Configuration
{
    /// <inheritdoc cref="IKafkaConfigurationSection"/>
    public class KafkaConfigurationSection : ConfigurationSection, IKafkaConfigurationSection
    {
        #region Constants

        public readonly struct Keys
        {
            public const string BOOTSTRAP_SERVERS = "bootstrapServers";
            public const string CONSUMER_GROUP_ID = "consumerGroupId";
            public const string AUTO_OFFSET_RESET = "autoOffsetReset";
            public const string ENABLE_AUTO_COMMIT = "enableAutoCommit";
        }

        #endregion

        #region Properties

        [ConfigurationProperty(Keys.BOOTSTRAP_SERVERS, IsRequired = true)]
        public string BootstrapServers => this[Keys.BOOTSTRAP_SERVERS].ToString();

        [ConfigurationProperty(Keys.CONSUMER_GROUP_ID)]
        public string ConsumerGroupId => this[Keys.CONSUMER_GROUP_ID].ToString();

        [ConfigurationProperty(Keys.AUTO_OFFSET_RESET)]
        public AutoOffsetReset AutoOffsetReset => (AutoOffsetReset)this[Keys.AUTO_OFFSET_RESET];

        [ConfigurationProperty(Keys.ENABLE_AUTO_COMMIT)]
        public bool EnableAutoCommit => bool.Parse(this[Keys.ENABLE_AUTO_COMMIT].ToString());

        #endregion  
    }
}
