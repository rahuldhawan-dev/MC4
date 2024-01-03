using log4net;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Producer;
using System.Collections.Generic;
using System.Linq;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Tasks
{
    public class SewerMainCleaningTask : GISMessageBrokerTaskBase<SewerMainCleaning>
    {
        #region Constants

        public const string KAFKA_TOPIC = "com.amwater.mapcall.sewermaincleaning.topic";

        #endregion

        #region Constructors

        public SewerMainCleaningTask(
            IGISMessageBrokerSerializer serializer, 
            ILog log, 
            IRepository<SewerMainCleaning> repository, 
            IKafkaServiceFactory<IKafkaProducer> kafkaProducerFactory,
            IGISMessageBrokerConfiguration messageBrokerConfiguration, 
            IDateTimeProvider dateTimeProvider) 
            : base(
                serializer, 
                log, 
                repository, 
                kafkaProducerFactory, 
                messageBrokerConfiguration, 
                dateTimeProvider) { }

        #endregion

        public override string KafkaTopic => KAFKA_TOPIC;

        protected override string Serialize(SewerMainCleaning entity)
        {
            return _serializer.Serialize(entity);
        }

        protected override IEnumerable<SewerMainCleaning> GetRecordsToPublish()
        {
            return _repository.Linq
                              .Where(x => x.NeedsToSync 
                                          && x.InspectionType != null 
                                          && x.InspectionType.Id != SewerMainInspectionType.Indices.SMOKE_TEST)
                              .Take(_gisMessageBrokerConfiguration.JobConfig?.MaxRecordsToPublishPerJobRun ?? GISMessageBrokerJobConfigurationSection.DEFAULT_MAX_RECORDS_TO_PUBLISH_PER_JOB_RUN);
        }
    }
}
