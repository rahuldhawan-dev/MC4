using Confluent.Kafka;

namespace MMSINC.Utilities.Kafka.Configuration
{
    /// <summary>
    /// Represents the various configuration settings for building and
    /// working with Kafka Producers and Consumers.
    /// </summary>
    public interface IKafkaConfiguration
    {
        /// <summary>
        /// A list of host/port pairs to use for establishing the initial connection to the
        /// Kafka cluster. The client will make use of all servers irrespective of which
        /// servers are specified here for bootstrapping—this list only impacts the initial
        /// hosts used to discover the full set of servers. This list should be in the form
        /// host1:port1,host2:port2,.... Since these servers are just used for the initial
        /// connection to discover the full cluster membership (which may change dynamically),
        /// this list need not contain the full set of servers
        /// </summary>
        string BootstrapServers { get; }

        /// <summary>
        /// A unique string that identifies the consumer group a consumer belongs to.
        /// </summary>
        string ConsumerGroupId { get; }

        /// <summary>
        /// If true the consumer's offset will be periodically committed in the background.
        ///
        /// - default: true
        /// </summary>
        bool EnableAutoCommit { get; }

        /// <summary>
        /// What to do when there is no initial offset in Kafka or if the current offset does not exist any more on the server (e.g. because that data has been deleted):        
        /// * Latest: automatically reset the offset to the latest offset
        /// * Earliest: automatically reset the offset to the earliest offset
        /// * Error - throw exception to the consumer if no previous offset is found for the consumer's group
        /// * anything else - throw exception to the consumer.
        ///
        /// - Default: 0 - latest
        /// </summary>
        AutoOffsetReset AutoOffsetReset { get; }
    }
}
