using System;
using System.Text;
using System.Threading;
using Apache.NMS;
using log4net;
using MapCallActiveMQListener.Ioc;
using MapCallActiveMQListener.Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.ActiveMQ;
using Stomp.Net;
using StructureMap;
using ConnectionFactory = Apache.NMS.ActiveMQ.ConnectionFactory;
using IMessage = MMSINC.Utilities.ActiveMQ.IMessage;

namespace MapCallActiveMQListener.Tests
{
    [TestClass]
    public class ListenerAndSenderTest
    {
        #region Nested Type: TestListener

        public abstract class TestListener<TServiceFactory> : ListenerBase<TestMessageProcessor>
            where TServiceFactory : IActiveMQServiceFactory
        {
            #region Constructors

            public TestListener(IActiveMQConfiguration configuration, TServiceFactory factory, TestMessageProcessor processor, ILog log) : base(configuration, factory, processor, log) { }

            #endregion

            #region Private Methods

            protected override void OnMessageReceived(IActiveMQService service, IMessage message)
            {
                MessageReceived?.Invoke(this, message);
            }

            #endregion

            #region Events/Delegates

            public event EventHandler<IMessage> MessageReceived;

            #endregion
        }

        public class ApacheTestListener : TestListener<ApacheActiveMQServiceFactory>
        {
            public override string ListenTopic => "test";

            public ApacheTestListener(IActiveMQConfiguration config, ApacheActiveMQServiceFactory factory, TestMessageProcessor processor, ILog log) : base(config, factory, processor, log) { }
        }

        public class TestMessageProcessor : IMessageProcessor
        {
            public void Process(IActiveMQService service, IMessage message, string topic)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(new DependencyRegistry());
            _container.Configure(e => {
                e.For<IActiveMQService>().Use<ApacheActiveMQService>();
            });
        }

        //[TestMethod]
        public void ManualStompDotNetTest()
        {
            var config = _container.GetInstance<IActiveMQConfiguration>();
            var connectionFactory = new Stomp.Net.ConnectionFactory($"{config.Scheme}://{config.Host}:{config.Port}", new StompConnectionSettings {
                UserName = config.User, Password = config.Password, ClientId = "test"
            });

            var connection = connectionFactory.CreateConnection();
            connection.Start();
            var session = connection.CreateSession();
            var topic = session.GetTopic("test");
            var producer = session.CreateProducer(topic);
            var consumer = session.CreateConsumer(topic);

            var expected = "foo";
            var received = false;
            var receiveTimeout = TimeSpan.FromSeconds(60);
            var semaphore = new AutoResetEvent(false);

            consumer.Listener += message => {
                received = true;
                var messageText = Encoding.UTF8.GetString(message.Content);
                Assert.AreEqual(expected, messageText);
                semaphore.Set();
            };

            producer.Send(session.CreateBytesMessage(Encoding.UTF8.GetBytes("expected")));
            semaphore.WaitOne((int)receiveTimeout.TotalMilliseconds, true);

            connection.Stop();
            connection.Dispose();

            Assert.IsTrue(received);
        }

        //[TestMethod]
        public void ManualStompDotNetWithDurabilityTest()
        {
            var config = _container.GetInstance<IActiveMQConfiguration>();
            var connectionFactory = new Stomp.Net.ConnectionFactory($"{config.Scheme}://{config.Host}:{config.Port}", new StompConnectionSettings {
                UserName = config.User, Password = config.Password, ClientId = "test"
            });
            var expected = "foo";

            var connection = connectionFactory.CreateConnection();
            connection.Start();
            var session = connection.CreateSession();
            var topic = session.GetTopic("test");
            var producer = session.CreateProducer(topic);

            producer.Send(session.CreateBytesMessage(Encoding.UTF8.GetBytes(expected)));

            producer.Dispose();
            session.Close();
            session.Dispose();
            connection.Stop();
            connection.Dispose();

            connection = connectionFactory.CreateConnection();
            connection.Start();
            session = connection.CreateSession();
            topic = session.GetTopic("test");
            var consumer = session.CreateDurableConsumer(topic, null, null, false);

            var received = false;
            var receiveTimeout = TimeSpan.FromSeconds(60);
            var semaphore = new AutoResetEvent(false);

            consumer.Listener += message => {
                received = true;
                var messageText = Encoding.UTF8.GetString(message.Content);
                Assert.AreEqual(expected, messageText);
                message.Acknowledge();
                semaphore.Set();
            };

            semaphore.WaitOne((int)receiveTimeout.TotalMilliseconds, true);

            connection.Stop();
            connection.Dispose();

            Assert.IsTrue(received);
        }

        [TestMethod]
        public void ManualApacheTest()
        {
            var config = _container.GetInstance<IActiveMQConfiguration>();
            var connectionFactory = new ConnectionFactory($"{config.Scheme}://{config.Host}:{config.Port}");
            var connection = connectionFactory.CreateConnection();
            connection.Start();
            var session = connection.CreateSession();
            var topic = session.GetTopic("mc-integration-test");
            var producer = session.CreateProducer(topic);
            var consumer = session.CreateConsumer(topic);

            var expected = "foo";
            var received = false;
            var receiveTimeout = TimeSpan.FromSeconds(60);
            var semaphore = new AutoResetEvent(false);

            consumer.Listener += message => {
                received = true;
                var txtMessage = message as ITextMessage;
                Assert.AreEqual(expected, txtMessage.Text);
                semaphore.Set();
            };

            producer.Send(session.CreateTextMessage(expected));
            semaphore.WaitOne((int)receiveTimeout.TotalMilliseconds, true);

            connection.Stop();
            connection.Dispose();

            Assert.IsTrue(received);
        }

        [TestMethod]
        public void TestConnectAndReceiveMessagesApache()
        {
            var listener = _container.GetInstance<ApacheTestListener>();
            var sender = _container
                        .With((IActiveMQServiceFactory)_container.GetInstance<ApacheActiveMQServiceFactory>())
                        .GetInstance<ISender>();
            var expected = "foo";
            var received = false;
            var receiveTimeout = TimeSpan.FromSeconds(60);
            var semaphore = new AutoResetEvent(false);

            listener.MessageReceived += (_, message) => {
                received = true;
                Assert.AreEqual(expected, message.Text);
                semaphore.Set();
            };

            listener.Start();

            sender.Send("test", expected);
            semaphore.WaitOne((int)receiveTimeout.TotalMilliseconds, true);

            listener.Stop();

            Assert.IsTrue(received);
        }
    }
}
