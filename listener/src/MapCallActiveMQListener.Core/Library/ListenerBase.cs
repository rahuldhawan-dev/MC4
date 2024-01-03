using System;
using log4net;
using MMSINC.Utilities.ActiveMQ;

namespace MapCallActiveMQListener.Library
{
    public abstract class ListenerBase<TMessageProcessor> : ActiveMQThingerBase, IListener
        where TMessageProcessor : IMessageProcessor
    {
        #region Private Members

        private Action _cleanupFn;
        protected readonly ILog _log;
        private IMessageProcessor _processor;

        #endregion

        #region Properties

        public abstract string ListenTopic { get; }

        #endregion

        #region Constructors

        public ListenerBase(IActiveMQConfiguration configuration, IActiveMQServiceFactory factory, TMessageProcessor processor, ILog log) : base(configuration, factory)
        {
            _log = log;
            _processor = processor;
        }

        #endregion

        #region Private Methods
        
        private static bool LogAndNegate(ILog log, string message, Exception e)
        {
            log.Error(message, e);
            return false;
        }

        protected virtual void OnMessageReceived(IActiveMQService service, IMessage message)
        {
            try
            {
                _processor.Process(service, message, ListenTopic);
            }
            catch (Exception e) when (LogAndNegate(_log, $"Error processing message:{Environment.NewLine}{message.Text}", e))
            {
                //this will never get hit.
                throw;
            }
        }

        #endregion

        #region Exposed Methods

        public void Start()
        {
            WithService(service =>
                _cleanupFn = service.ReceiveMessages(ListenTopic, (message) => {
                    _log.Info("Message Received:");
                    _log.Info(message.Text);
                    OnMessageReceived(service, message);
                }));
        }

        public void Stop()
        {
            _cleanupFn();
        }

        #endregion
    }
}
