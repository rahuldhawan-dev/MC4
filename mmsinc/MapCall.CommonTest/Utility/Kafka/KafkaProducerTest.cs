using Confluent.Kafka;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.Kafka.Producer;
using Moq;
using System;
using System.Linq;
using System.Threading;
using log4net;

namespace MapCall.CommonTest.Utility.Kafka
{
    [TestClass]
    public class KafkaProducerTest
    {
        #region Private Members

        private readonly IProducer<Null, string> _producerMock = Mock.Of<IProducer<Null, string>>();

        private KafkaProducer _target;

        #endregion

        #region Init / Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new KafkaProducer(
                Mock.Of<ILog>(),
                _producerMock);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void Producer_SendsMessages_WhenAllIsRightInTheWorld()
        {
            _target.SendMessage("test-topic", "a");

            Mock.Get(_producerMock)
                .Verify(x => x.Produce(
                     It.IsAny<string>(),
                     It.IsAny<Message<Null, string>>(),
                     It.IsAny<Action<DeliveryReport<Null, string>>>()), Times.Once());
        }

        [TestMethod]
        public void Producer_Throws_WhenInternalProducerEncountersError()
        {
            Mock.Get(_producerMock)
                .Setup(x => x.Produce(
                     It.IsAny<string>(),
                     It.IsAny<Message<Null, string>>(),
                     It.IsAny<Action<DeliveryReport<Null, string>>>()))
                .Throws(new ProduceException<Null, string>(
                     new Error(ErrorCode.BrokerNotAvailable, "ruh roh, broker down!", true),
                     Mock.Of<DeliveryResult<Null, string>>(),
                     Mock.Of<Exception>()));

            Assert.ThrowsException<ProduceException<Null, string>>(() => {
                _target.SendMessage("test-topic", "a");
            }, "The producer should have thrown a produce exception and considered it fatal.");

            Mock.Get(_producerMock)
                .Verify(x => x.Produce(
                     It.IsAny<string>(),
                     It.IsAny<Message<Null, string>>(),
                     It.IsAny<Action<DeliveryReport<Null, string>>>()), Times.Once);
        }

        [TestMethod]
        public void Producer_Throws_WhenInternalProducerEncountersUnexpectedException()
        {
            Mock.Get(_producerMock)
                .Setup(x => x.Produce(
                     It.IsAny<string>(),
                     It.IsAny<Message<Null, string>>(),
                     It.IsAny<Action<DeliveryReport<Null, string>>>()))
                .Throws(new Exception("Things went horribly wrong."));

            Assert.ThrowsException<Exception>(() => {
                _target.SendMessage("test-topic", "a");
            }, "The producer should have thrown an exception.");

            Mock.Get(_producerMock)
                .Verify(x => x.Produce(
                     It.IsAny<string>(),
                     It.IsAny<Message<Null, string>>(),
                     It.IsAny<Action<DeliveryReport<Null, string>>>()), Times.Once);
        }

        #endregion
    }
}
