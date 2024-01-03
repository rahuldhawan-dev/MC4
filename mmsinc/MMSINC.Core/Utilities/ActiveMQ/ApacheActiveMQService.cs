using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Apache.NMS;
using Apache.NMS.ActiveMQ;

namespace MMSINC.Utilities.ActiveMQ
{
    public class ApacheActiveMQService : ActiveMQServiceBase
    {
        #region Constructors

        public ApacheActiveMQService(IActiveMQConfiguration config) : base(config) { }

        #endregion

        #region Private Methods

        protected virtual IConnection WithConnection(Action<IConnection> fn, bool dispose = true)
        {
            var connection = CreateConnection();

            fn(connection);

            if (dispose)
            {
                CleanUpResources(connection);
            }

            return connection;
        }

        private IConnection CreateConnection()
        {
            IConnection connection;
            var factory = new ConnectionFactory(_config.Url);

            if (string.IsNullOrWhiteSpace(_config.User) && string.IsNullOrWhiteSpace(_config.Password))
            {
                connection = factory.CreateConnection();
            }
            else if (string.IsNullOrWhiteSpace(_config.User) || string.IsNullOrWhiteSpace(_config.Password))
            {
                throw new ConfigurationErrorsException("Either user and password must be set, or neither.");
            }
            else
            {
                connection = factory.CreateConnection(_config.User, _config.Password);
            }

            return connection;
        }

        protected virtual (ISession Session, ITopic Topic) WithSessionAndTopic(IConnection connection, string topicStr,
            Action<ISession, ITopic> fn, bool cleanUp = true)
        {
            var session = connection.CreateSession();
            var topic = session.GetTopic(topicStr);

            fn(session, topic);

            if (cleanUp)
            {
                CleanUpResources(topic, session);
            }

            return (session, topic);
        }

        protected virtual void CleanUpResources(IMessageConsumer consumer)
        {
            consumer.Close();
            consumer.Dispose();
        }

        protected virtual void CleanUpResources(IConnection connection)
        {
            if (connection.IsStarted)
            {
                connection.Stop();
            }

            connection.Dispose();
        }

        protected virtual void CleanUpResources(IDestination destination, ISession session)
        {
            session.Close();
            destination.Dispose();
            session.Dispose();
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Send the given message to the configured topic.  All connections
        /// and related objects are cleaned up and disposed automatically.
        /// </summary>
        /// <param name="message"></param>
        public override void SendMessage(string topic, string message)
        {
            SendMessage(topic, message, null);
        }

        public override void SendMessage(string topic, string message, Dictionary<string, object> properties = null)
        {
            WithConnection(connection => {
                connection.Start();
                WithSessionAndTopic(connection, topic, (session, t) => {
                    using (var producer = session.CreateProducer(t))
                    {
                        producer.Send(CreateMessage(session, message, properties));
                        producer.Close();
                    }
                });
            });
        }

        private ITextMessage CreateMessage(ISession session, string message, Dictionary<string, object> properties)
        {
            var msg = session.CreateTextMessage(message);

            if (properties == null)
            {
                return msg;
            }

            foreach (var obj in properties)
            {
                switch (obj.Value)
                {
                    case int intValue:
                        msg.Properties.SetInt(obj.Key, intValue);
                        break;
                    default:
                        throw new NotSupportedException($"Type is not supported for {obj.Key} : {obj.Value}");
                }
            }

            return msg;
        }

        //var msg = session.CreateTextMessage(message);
        //msg.Properties.SetInt("FSRID", 112312312);
        //producer.Send(msg);
        //producer.Close();
        /// <summary>
        /// Attach a listener to perform the given action when a message is
        /// received.  Nothing is cleaned up or disposed, all disposable
        /// resources are returned to be cleaned up by the caller, and an
        /// action is returned to do so.
        /// </summary>
        public override Action ReceiveMessages(string topic, Action<IMessage> onMessageReceived)
        {
            IMessageConsumer consumer = null;
            ISession session = null;
            ITopic topicObj = null;
            var connection = WithConnection(c => {
                c.ClientId = "mc." + topic.Replace(".", "_");
                c.Start();
                (session, topicObj) = WithSessionAndTopic(c, topic, (s, t) => {
                    consumer = s.CreateDurableConsumer(t, "dev", null, false);

                    consumer.Listener += m => { onMessageReceived(new ApacheMessageWrapper(m)); };
                }, false);
            }, false);

            return () => {
                CleanUpResources(consumer);
                CleanUpResources(topicObj, session);
                CleanUpResources(connection);
            };
        }

        #endregion
    }
}
