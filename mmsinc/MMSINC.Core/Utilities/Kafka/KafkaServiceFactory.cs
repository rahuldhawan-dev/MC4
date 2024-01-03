using log4net;
using MMSINC.Utilities.Kafka.Configuration;
using StructureMap;

namespace MMSINC.Utilities.Kafka
{
    public class KafkaServiceFactory<TKafkaService> : IKafkaServiceFactory<TKafkaService> where TKafkaService : IKafkaService
    {
        #region Private Members

        private readonly IContainer _container;
        private readonly ILog _logger;

        #endregion

        #region Constructors

        public KafkaServiceFactory(ILog logger, IContainer container)
        {
            _container = container;
            _logger = logger;
        }

        #endregion

        #region Exposed Methods

        public TKafkaService Build(IKafkaConfiguration config)
        {
            _logger.Info($"Building a {typeof(TKafkaService)} targeting: {config.BootstrapServers}");

            return _container.With(typeof(IKafkaConfiguration), config)
                             .GetInstance<TKafkaService>();
        }

        #endregion
    }
}
