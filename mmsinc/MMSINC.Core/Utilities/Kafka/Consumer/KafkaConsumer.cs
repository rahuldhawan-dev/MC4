using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Threading;
using MMSINC.Utilities.Kafka.Configuration;
using log4net;

namespace MMSINC.Utilities.Kafka.Consumer
{
    /// <inheritdoc />
    public class KafkaConsumer : IKafkaConsumer
    {
        #region Private Members

        private readonly ILog _logger;
        private readonly IKafkaConfiguration _configuration;
        private readonly IConsumer<Null, string> _consumer;
        private bool _disposedValue;

        #endregion

        #region Constructors

        public KafkaConsumer(ILog logger, IKafkaConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _consumer = new ConsumerBuilder<Null, string>(new ConsumerConfig {
                BootstrapServers = configuration.BootstrapServers,
                GroupId = configuration.ConsumerGroupId,
                AutoOffsetReset = configuration.AutoOffsetReset,
                EnableAutoCommit = configuration.EnableAutoCommit
            }).Build();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KafkaConsumer"/> class.
        /// </summary>
        /// <remarks>
        /// This ctor exists so we can mock the consumer for testing, while limiting its
        /// instantiating via DI because IProducer{Null, string}> is not registered.
        /// </remarks>
        internal KafkaConsumer(ILog logger, IKafkaConfiguration configuration, IConsumer<Null, string> consumer)
        {
            _logger = logger;
            _consumer = consumer;
            _configuration = configuration;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<string> ReadMessages(string topic, CancellationToken cancellationToken)
        {
            // The token could be canceled at this point, if so let's exit early.

            cancellationToken.ThrowIfCancellationRequested();

            _consumer.Subscribe(topic);
            _logger.Info($"Consumer GroupId {_configuration.ConsumerGroupId}: subscribed to topic: {topic}");

            while (true)
            {
                string messageValue;

                try
                {
                    cancellationToken.ThrowIfCancellationRequested();

                    var consumeResult = _consumer.Consume(cancellationToken);

                    if (consumeResult != null)
                    {
                        messageValue = consumeResult.Message.Value;
                        _logger.Debug($"Consumer GroupId {_configuration.ConsumerGroupId}: consumed a message: {messageValue}");
                    }
                    else
                    {
                        // Null messages mean end of partition result, so ignore

                        continue;
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.Info($"Consumer GroupId {_configuration.ConsumerGroupId}: cancellation requested.");
                    throw;
                }
                catch (ConsumeException ex)
                {
                    _logger.Error($"Consumer GroupId {_configuration.ConsumerGroupId}: consume exception encountered: {ex.Error.Reason}.", ex);
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.Error($"Consumer GroupId {_configuration.ConsumerGroupId}: unexpected exception encountered: {ex.Message}.", ex);
                    throw;
                }

                yield return messageValue;
            }
        }

        public void Commit()
        {
            if (_configuration.EnableAutoCommit)
            {
                return;
            }

            _consumer.Commit();
        }

        private void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                _consumer?.Close();
                _consumer?.Dispose();
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
