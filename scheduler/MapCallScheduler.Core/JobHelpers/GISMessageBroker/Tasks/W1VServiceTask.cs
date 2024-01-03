using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCallScheduler.JobHelpers.GISMessageBroker.Models;
using MapCallScheduler.Library.Configuration;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Producer;
using MapCallService = MapCall.Common.Model.Entities.Service;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Tasks
{
    public class W1VServiceTask : GISMessageBrokerTaskBase<MapCallService>
    {
        #region Constants

        public const string TOPIC_NAME = "mc.premise.outbound";

        #endregion

        #region Private Members

        private readonly IMapCallSchedulerConfiguration _config;

        #endregion

        #region Constructors

        public W1VServiceTask(
            IGISMessageBrokerSerializer serializer,
            ILog log,
            IRepository<MapCallService> repository,
            IKafkaServiceFactory<IKafkaProducer> kafkaProducerFactory,
            IGISMessageBrokerConfiguration gisMessageBrokerConfiguration,
            IDateTimeProvider dateTimeProvider,
            IMapCallSchedulerConfiguration config)
            : base(
                serializer,
                log,
                repository,
                kafkaProducerFactory,
                gisMessageBrokerConfiguration,
                dateTimeProvider)
        {
            _config = config;
        }
        
        #endregion

        //While testing locally, they may ask to drop message to
        //local.mc.premise.outbound
        public override string KafkaTopic => _config.IsProduction ? $"prod.{TOPIC_NAME}" :
            _config.IsStaging ? $"qa.{TOPIC_NAME}" : $"dev.{TOPIC_NAME}";

        protected override string Serialize(MapCallService entity)
        {
            return _serializer.Serialize(entity);
        }
        
        protected override IEnumerable<MapCallService> GetRecordsToPublish()
        {
            return _repository.Linq
                              .Where(x => x.NeedsToSync)
                              .Take(_gisMessageBrokerConfiguration.JobConfig?.MaxRecordsToPublishPerJobRun 
                                    ?? GISMessageBrokerJobConfigurationSection.DEFAULT_MAX_RECORDS_TO_PUBLISH_PER_JOB_RUN);
        }
    }
}
