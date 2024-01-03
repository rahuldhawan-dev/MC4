using Confluent.Kafka;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.Kafka.Consumer;
using Moq;
using System;
using System.Linq;
using System.Threading;
using log4net;
using MMSINC.Utilities.Kafka.Configuration;

namespace MapCall.CommonTest.Utility.Kafka
{
    [TestClass]
    public class KafkaConsumerTest
    {
        #region Constants

        private const int NUMBER_OF_TEST_MESSAGES = 3;

        #endregion

        #region Private Members

        private readonly IConsumer<Null, string> _consumerMock = Mock.Of<IConsumer<Null, string>>();
        private readonly ILog _loggerMock = Mock.Of<ILog>();
        private KafkaConsumer _target;

        #endregion

        [TestInitialize]
        public void InitializeTest()
        {
            Mock.Get(_consumerMock)
                .SetupSequence(x => x.Consume(It.IsAny<CancellationToken>()))
                .Returns(new ConsumeResult<Null, string> {
                     Message = new Message<Null, string> {
                         Value = "x"
                     }
                 })
                .Returns(new ConsumeResult<Null, string> {
                     Message = new Message<Null, string> {
                         Value = "y"
                     }
                 })
                .Returns(new ConsumeResult<Null, string> {
                     Message = new Message<Null, string> {
                         Value = "z"
                     }
                 });

            var configurationMock = Mock.Of<IKafkaConfiguration>();
            Mock.Get(configurationMock)
                .SetupGet(x => x.EnableAutoCommit)
                .Returns(true);

            _target = new KafkaConsumer(_loggerMock, configurationMock, _consumerMock);
        }

        [TestMethod]
        public void ReadMessages_ReadsMessages_UntilItIsRequestedToStop()
        {
            var receivedMessages = 0;

            using (var cancellation = new CancellationTokenSource())
            {
                try
                {
                    foreach (var unused in _target.ReadMessages("test-topic", cancellation.Token))
                    {
                        if (++receivedMessages >= NUMBER_OF_TEST_MESSAGES)
                        {
                            cancellation.Cancel();
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // expected, nothing to see here.
                }
            }

            Mock.Get(_consumerMock)
                .Verify(x => x.Consume(It.IsAny<CancellationToken>()), Times.Exactly(NUMBER_OF_TEST_MESSAGES));

            Assert.AreEqual(NUMBER_OF_TEST_MESSAGES, receivedMessages);
        }

        [TestMethod]
        public void ReadMessages_MovesNext_UntilItIsRequestedToStop()
        {
            var receivedMessages = 0;

            using (var cancellation = new CancellationTokenSource())
            {
                try
                {
                    using (var enumerator = _target.ReadMessages("test-topic", cancellation.Token)
                                                   .GetEnumerator())
                    {
                        for (var a = 0; a < NUMBER_OF_TEST_MESSAGES; a++)
                        {
                            enumerator.MoveNext();
                            if (++receivedMessages >= NUMBER_OF_TEST_MESSAGES)
                            {
                                cancellation.Cancel();
                            }
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // expected, nothing to see here.
                }
            }

            Mock.Get(_consumerMock)
                .Verify(x => x.Consume(It.IsAny<CancellationToken>()), Times.Exactly(NUMBER_OF_TEST_MESSAGES));

            Assert.AreEqual(NUMBER_OF_TEST_MESSAGES, receivedMessages);
        }

        [TestMethod]
        public void ReadMessages_Throws_IfCancelledBeforeRead()
        {
            Assert.ThrowsException<OperationCanceledException>(() => {
                using (var cancellation = new CancellationTokenSource())
                {
                    cancellation.Cancel();
                    _ = _target.ReadMessages("test-topic", cancellation.Token)
                               .Count();
                }
            }, "The iterator should have indicated the token was cancelled.");

            Mock.Get(_consumerMock).Verify(x => x.Consume(It.IsAny<CancellationToken>()), Times.Never);
        }

        [TestMethod]
        public void Dispose_CleansUp_Dependencies()
        {
            _target.Dispose();

            Mock.Get(_consumerMock).Verify(x => x.Close(), Times.Once);
            Mock.Get(_consumerMock).Verify(x => x.Dispose(), Times.Once);
        }

        [TestMethod]
        public void Commit_CommitsWhenItShould()
        {
            var configurationMock = Mock.Of<IKafkaConfiguration>();
            Mock.Get(configurationMock)
                .SetupGet(x => x.EnableAutoCommit)
                .Returns(false);

            _target = new KafkaConsumer(_loggerMock, configurationMock, _consumerMock);

            _target.Commit();

            Mock.Get(_consumerMock).Verify(x => x.Commit(), Times.Once);
        }

        [TestMethod]
        public void Commit_DoesNotCommitWhenItShouldNot()
        {
            _target.Commit();

            Mock.Get(_consumerMock).Verify(x => x.Commit(), Times.Never);
        }
    }
}
