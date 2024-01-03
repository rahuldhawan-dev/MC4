using log4net;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Producer;
using MapCallSampleSite = MapCall.Common.Model.Entities.SampleSite;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Tasks
{
    public class SampleSiteTask : GISMessageBrokerTaskBase<MapCallSampleSite>
    {
        #region Constants

        public const string KAFKA_TOPIC = "com.amwater.mapcall.samplesite.topic";

        #endregion

        #region Constructors

        public SampleSiteTask(
            IGISMessageBrokerSerializer serializer, 
            ILog log, IRepository<SampleSite> repository, 
            IKafkaServiceFactory<IKafkaProducer> kafkaProducerFactory,
            IGISMessageBrokerConfiguration messageBrokerConfiguration, 
            IDateTimeProvider dateTimeProvider) 
            : base(serializer, 
                log, 
                repository, 
                kafkaProducerFactory, 
                messageBrokerConfiguration, 
                dateTimeProvider) { }

        #endregion

        public override string KafkaTopic => KAFKA_TOPIC;

        protected override string Serialize(MapCallSampleSite entity)
        {
            return _serializer.Serialize(entity);
        }
    }
}
