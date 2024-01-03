using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.GISMessageBroker;
using MapCallScheduler.JobHelpers.GISMessageBroker.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Producer;
using Moq;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.GISMessageBroker.Tasks
{
    public abstract class GISMessageBrokerTaskTestBase<TEntity, TTask>
    {
        #region Private Members

        protected Container _container;

        protected Mock<IRepository<TEntity>> _repository;
        protected Mock<IGISMessageBrokerSerializer> _serializer;
        protected Mock<IKafkaServiceFactory<IKafkaProducer>> _kafkaProducerFactory;
        protected Mock<IGISMessageBrokerConfiguration> _kafkaConfiguration;
        protected Mock<IKafkaProducer> _kafkaProducer;
        protected Mock<IDateTimeProvider> _dateTimeProvider;
        protected Mock<IMapCallSchedulerConfiguration> _schedulerConfiguration;

        protected DateTime _now;

        protected TTask _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);
            _kafkaProducerFactory
               .Setup(x => x.Build(_kafkaConfiguration.Object.KafkaConfig))
               .Returns((_kafkaProducer = new Mock<IKafkaProducer>()).Object);
            _dateTimeProvider.Setup(x => x.GetCurrentDate()).Returns(_now = DateTime.Now);

            _target = _container.GetInstance<TTask>();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _repository.VerifyAll();
            _serializer.VerifyAll();
            _kafkaProducer.VerifyAll();
        }

        #endregion

        #region Private Methods

        private void InitializeContainer(ConfigurationExpression e)
        {
            _repository = e.For<IRepository<TEntity>>().Mock();
            _serializer = e.For<IGISMessageBrokerSerializer>().Mock();
            _kafkaConfiguration = e.For<IGISMessageBrokerConfiguration>().Mock();
            _kafkaProducerFactory = e.For<IKafkaServiceFactory<IKafkaProducer>>().Mock();
            _dateTimeProvider = e.For<IDateTimeProvider>().Mock();
            _schedulerConfiguration = e.For<IMapCallSchedulerConfiguration>().Mock();
            
            e.For<ILog>().Mock();
        }

        #endregion
    }
}
