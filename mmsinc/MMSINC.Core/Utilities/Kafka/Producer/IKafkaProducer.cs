using System;

namespace MMSINC.Utilities.Kafka.Producer
{
    /// <summary>
    /// Represents the things with which a Kafka Producer should be doing.
    /// </summary>
    public interface IKafkaProducer : IKafkaService
    {
        /// <summary>
        /// Sends a message to a topic.
        /// </summary>
        /// <param name="topic">The topic to send the given message to.</param>
        /// <param name="message">The message to be sent to the given topic.</param>
        void SendMessage(string topic, string message);
    }
}
