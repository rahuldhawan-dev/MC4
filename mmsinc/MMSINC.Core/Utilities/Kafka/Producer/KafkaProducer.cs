using Confluent.Kafka;
using log4net;
using MMSINC.Utilities.Kafka.Configuration;
using System;

namespace MMSINC.Utilities.Kafka.Producer
{
    /// <inheritdoc />
    public class KafkaProducer : IKafkaProducer
    {
        #region Private Members

        private readonly ILog _logger;
        private readonly IProducer<Null, string> _producer;

        #endregion

        #region Constructors

        public KafkaProducer(ILog logger, IKafkaConfiguration configuration)
        {
            _logger = logger;
            _producer = new ProducerBuilder<Null, string>(new ProducerConfig {
                BootstrapServers = configuration.BootstrapServers
            }).Build();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaProducer"/> class.
        /// </summary>
        /// <remarks>
        /// This ctor exists so we can mock the producer for testing, while limiting its
        /// instantiating via DI because IProducer{TKey, TValue}> is not registered.
        /// </remarks>
        /// <param name="logger">The logger to be used for ... logging purposes.</param>
        /// <param name="producer">The producer to be used for testing purposes.</param>
        internal KafkaProducer(
            ILog logger,
            IProducer<Null, string> producer)
        {
            _logger = logger;
            _producer = producer;
        }

        #endregion

        #region Exposed Methods

        public void SendMessage(string topic, string message)
        {
            try
            {
                _logger.Info($"Sending message to topic: {topic}: {message}");

                _producer.Produce(topic, new Message<Null, string> { Value = message });
            }
            catch (ProduceException<Null, string> ex)
            {
                _logger.Warn($"ProduceException encountered. Is fatal: {ex.Error.IsFatal}. Message: {message}", ex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected exception encountered. Message: {message}", ex);
                throw;
            }
        }

        public void Dispose()
        {
            _producer?.Flush();
            _producer?.Dispose();
        }

        #endregion
    }
}
