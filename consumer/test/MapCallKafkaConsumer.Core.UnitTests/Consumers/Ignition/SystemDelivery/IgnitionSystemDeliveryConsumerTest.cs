using System;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using Moq;
using System.Threading;
using MapCall.Common.Model.Entities;
using MapCallKafkaConsumer.Core.UnitTests.Testing;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;
using StructureMap;
using MapCall.Common.Model.Repositories;
using MMSINC.Utilities;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Configuration;
using MMSINC.Utilities.Kafka.Consumer;
using System.Collections.Generic;
using MapCallKafkaConsumer.Consumers.Ignition;
using MapCallKafkaConsumer.Consumers.Ignition.SystemDelivery;

namespace MapCallKafkaConsumer.Core.UnitTests.Consumers.Ignition.SystemDelivery
{
    [TestClass]
    public class IgnitionSystemDeliveryConsumerTest : MapCallKafkaConsumerInMemoryDatabaseTest<SystemDeliveryEntry, IRepository<SystemDeliveryEntry>>
    {
        #region Private Members

        private IIgnitionConfiguration _configurationMock;
        private IKafkaServiceFactory<IKafkaConsumer> _serviceFactoryMock;
        private IKafkaConsumer _consumerMock;
        private IIgnitionSystemDeliveryProcessor _processorMock;
        private ILog _loggerMock;

        private static readonly List<string> MESSAGES_TO_READ = new List<string> {
            "yellow"
        };

        #endregion
        
        #region Init / Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            
            e.For<IUnitOfWorkFactory>().Use<TestUnitOfWorkFactory>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IDateTimeProvider>().Use<DateTimeProvider>();

            _loggerMock = Mock.Of<ILog>();
            _configurationMock = Mock.Of<IIgnitionConfiguration>();

            _consumerMock = Mock.Of<IKafkaConsumer>();
            Mock.Get(_consumerMock)
                .Setup(x => x.ReadMessages(
                     It.IsAny<string>(),
                     It.IsAny<CancellationToken>()))
                .Returns(MESSAGES_TO_READ);

            _serviceFactoryMock = Mock.Of<IKafkaServiceFactory<IKafkaConsumer>>();
            Mock.Get(_serviceFactoryMock)
                .Setup(x => x.Build(It.IsAny<IKafkaConfiguration>()))
                .Returns(_consumerMock);

            _processorMock = Mock.Of<IIgnitionSystemDeliveryProcessor>();

            e.For<ILog>().Use(_loggerMock);
            e.For<IIgnitionConfiguration>().Use(_configurationMock);
            e.For<IKafkaServiceFactory<IKafkaConsumer>>().Use(_serviceFactoryMock);
            e.For<IIgnitionSystemDeliveryProcessor>().Use(_processorMock);
        }

        #endregion
        
        #region Tests

        [TestMethod]
        public void Start_ReadsMessages_AsExpected()
        {
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var target = new IgnitionSystemDeliveryConsumer(
                    _loggerMock,
                    _configurationMock,
                    _serviceFactoryMock,
                    _processorMock);

                target.Start();
                target.Stop();

                Mock.Get(_consumerMock)
                    .Verify(x => x.ReadMessages(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

                Mock.Get(_consumerMock)
                    .Verify(x => x.Dispose(), Times.Once);

                Mock.Get(_processorMock)
                    .Verify(x => x.Process(It.IsAny<string>()), Times.Exactly(MESSAGES_TO_READ.Count));
            }
        }

        [TestMethod]
        public void Start_Exits_WhenSomethingGoesWrong()
        {
            Mock.Get(_processorMock)
                .Setup(x => x.Process(It.IsAny<string>()))
                .Throws(new Exception("ruh roh raggy!"));

            Mock.Get(_loggerMock)
                .Setup(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>()));

            var target = new IgnitionSystemDeliveryConsumer(
                _loggerMock,
                _configurationMock,
                _serviceFactoryMock,
                _processorMock);

            Assert.ThrowsException<Exception>(() => {
                target.Start();
            }, "The consumer should have thrown an exception while attempting to read messages.");

            Mock.Get(_consumerMock)
                .Verify(x => x.ReadMessages(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

            Mock.Get(_processorMock)
                .Verify(x => x.Process(It.IsAny<string>()), Times.Once);

            Mock.Get(_loggerMock)
                .Verify(x => x.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }
    }
    
    #endregion
}
