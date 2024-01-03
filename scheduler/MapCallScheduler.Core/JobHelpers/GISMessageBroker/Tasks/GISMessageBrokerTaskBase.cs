using System;
using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using System.Linq;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Producer;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Tasks
{
    public abstract class GISMessageBrokerTaskBase<TEntity> : IGISMessageBrokerTask
        where TEntity : IThingWithSyncing
    {
        #region Private Members

        protected readonly IGISMessageBrokerSerializer _serializer;
        protected readonly IRepository<TEntity> _repository;
        protected readonly ILog _log;
        protected readonly IKafkaServiceFactory<IKafkaProducer> _kafkaProducerFactory;
        protected readonly IGISMessageBrokerConfiguration _gisMessageBrokerConfiguration;
        protected readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public GISMessageBrokerTaskBase(IGISMessageBrokerSerializer serializer, 
            ILog log, 
            IRepository<TEntity> repository,
            IKafkaServiceFactory<IKafkaProducer> kafkaProducerFactory,
            IGISMessageBrokerConfiguration gisMessageBrokerConfiguration,
            IDateTimeProvider dateTimeProvider)
        {
            _serializer = serializer;
            _repository = repository;
            _log = log;
            _gisMessageBrokerConfiguration = gisMessageBrokerConfiguration;
            _kafkaProducerFactory = kafkaProducerFactory;
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Abstract Properties

        public abstract string KafkaTopic { get; }

        #endregion

        #region Abstract Methods

        protected abstract string Serialize(TEntity entity);

        #endregion

        #region Private Methods

        protected virtual IEnumerable<TEntity> GetRecordsToPublish()
        {
            return _repository.Linq
                              .Where(x => x.NeedsToSync)
                              .Take(_gisMessageBrokerConfiguration.JobConfig?.MaxRecordsToPublishPerJobRun ?? GISMessageBrokerJobConfigurationSection.DEFAULT_MAX_RECORDS_TO_PUBLISH_PER_JOB_RUN);
        }

        private IKafkaProducer GetKafkaProducer()
        {
            return _kafkaProducerFactory.Build(_gisMessageBrokerConfiguration.KafkaConfig);
        }

        #endregion

        #region Exposed Methods

        public void Run()
        {
            using (var kafkaProducer = GetKafkaProducer())
            {
                var entitiesToPublish = GetRecordsToPublish().ToList();
                var i = 1;
                var count = entitiesToPublish.Count;

                _log.Info($"Found {count} {typeof(TEntity).Name} records to sync.");

                foreach (var entity in entitiesToPublish)
                {
                    _log.Info($"Syncing {typeof(TEntity).Name} {entity.Id} ({i++} of {count}).");

                    try
                    {
                        kafkaProducer.SendMessage(KafkaTopic, Serialize(entity));
                    }
                    catch (Exception e)
                    {
                        _log.Error($"Not able to sync {typeof(TEntity).Name} {entity.Id} for reasons: {e.Message}.", e);
                        continue;
                    }

                    entity.NeedsToSync = false;
                    entity.LastSyncedAt = _dateTimeProvider.GetCurrentDate();
                }

                _repository.Save(entitiesToPublish);
            }
        }

        #endregion
    }
}
