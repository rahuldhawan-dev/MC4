using log4net;
using MMSINC.Interface;

namespace MapCallScheduler.Library.Email
{
    public abstract class IncomingEmailMessageProcessorServiceBase<THandlerFactory> : IIncomingEmailMessageProcessorService
        where THandlerFactory : IIncomingEmailMessageHandlerFactory
    {
        #region Private Members

        protected readonly ILog _log;
        protected readonly IWrappedImapClient _imapClient;
        protected readonly THandlerFactory _handlerFactory;

        #endregion

        #region Constructors

        protected IncomingEmailMessageProcessorServiceBase(ILog log, IWrappedImapClient imapClient, THandlerFactory handlerFactory)
        {
            _log = log;
            _imapClient = imapClient;
            _handlerFactory = handlerFactory;
        }

        #endregion

        #region Abstract Methods

        protected virtual IIncomingEmailMessageHandler GetHandler(IMailMessage message)
        {
            return _handlerFactory.GetHandler(_imapClient, message);
        }

        #endregion

        #region Exposed Methods

        public void Process(string mailbox, uint uid)
        {
            var message = _imapClient.GetWrappedMessage(uid, true, mailbox);
            using (var handler = GetHandler(message))
            {
                _log.InfoFormat("uid: {0}, mailbox: '{1}', subject: '{2}', handler: {3}", uid, mailbox, message.Subject,
                    handler.GetType().Name);

                handler.Handle(uid, mailbox, message);
            }
        }

        #endregion
    }
}