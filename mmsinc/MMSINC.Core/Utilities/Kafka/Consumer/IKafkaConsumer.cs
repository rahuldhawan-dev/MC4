using System;
using System.Collections.Generic;
using System.Threading;

namespace MMSINC.Utilities.Kafka.Consumer
{
    /// <summary>
    /// Represents the things with which a Kafka Consumer should be doing.
    /// </summary>
    public interface IKafkaConsumer : IKafkaService
    {
        /// <summary>
        /// Subscribes to the given topic and begins reading messages. It will block until messages are available.
        /// </summary>
        /// <param name="topic">The topic to be consume.</param>
        /// <param name="cancellationToken">The token that allows callers to cancel the consumer's subscription.</param>
        /// <returns>An enumerable of messages yielded by the consumer.</returns>
        IEnumerable<string> ReadMessages(string topic, CancellationToken cancellationToken);

        /// <summary>
        /// Commits all offsets for the current assignment (message).
        /// </summary>
        /// <remarks>
        /// This method will only commit when the consumer configuration value for
        /// EnableAutoCommit is false.
        /// </remarks>
        void Commit();
    }
}
