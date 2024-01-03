using System;
using System.Threading;
using log4net;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Configuration;
using MMSINC.Utilities.Kafka.Consumer;

namespace MapCallKafkaConsumer.Library
{
    public abstract class ConsumerBase<TMessageProcessor, TMessage, TEntity> 
        : IConsumer where TMessageProcessor : IMessageToEntityProcessor<TMessage, TEntity>
    {
        #region Private Members

        protected readonly ILog _logger;
        protected readonly TMessageProcessor _processor;
        protected readonly IKafkaConsumer _consumer;
        protected readonly CancellationTokenSource _cancellationTokenSource;

        #endregion

        #region Properties

        public abstract string Topic { get; }

        public abstract string Identifier { get; }

        #endregion

        #region Constructors

        protected ConsumerBase(ILog logger,
            IKafkaServiceConfiguration configuration, 
            IKafkaServiceFactory<IKafkaConsumer> serviceFactory, 
            TMessageProcessor processor)
        {
            _logger = logger;
            _processor = processor;
            _consumer = serviceFactory.Build(configuration.KafkaConfig);
            _cancellationTokenSource = new CancellationTokenSource();
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            // The token may have been canceled prior to having start called

            _cancellationTokenSource.Token.ThrowIfCancellationRequested();

            _logger.Info($"{Identifier} starting.");

            foreach (var message in _consumer.ReadMessages(Topic, _cancellationTokenSource.Token))
            {
                try
                {
                    _cancellationTokenSource.Token.ThrowIfCancellationRequested();

                    _logger.Debug($"{Identifier}: consumed message from topic: {Topic} - {message}");

                    _processor.Process(message);

                    _consumer.Commit();
                }
                catch (OperationCanceledException)
                {
                    _logger.Info("Cancellation requested.");
                    throw;
                }
                catch (Exception ex)
                {
                    _logger.Error($"{Identifier}: could not consume and process message: {message}", ex);
                    throw;
                }
            }
        }

        public void Stop()
        {
            _cancellationTokenSource.Cancel(true);

            _consumer.Dispose();

            _logger.Info($"{Identifier}: stopped.");
        }

        #endregion
    }
}
