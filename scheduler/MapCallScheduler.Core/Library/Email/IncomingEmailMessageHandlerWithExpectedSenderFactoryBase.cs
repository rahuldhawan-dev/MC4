using MMSINC.Interface;
using StructureMap;

namespace MapCallScheduler.Library.Email
{
    public abstract class IncomingEmailMessageHandlerWithExpectedSenderFactoryBase<THandler> : IncomingEmailMessageHandlerFactoryBase<THandler>
        where THandler : IIncomingEmailMessageHandler
    {
        #region Abstract Properties

        public abstract string ExpectedSender { get; }

        #endregion

        #region Constructors

        protected IncomingEmailMessageHandlerWithExpectedSenderFactoryBase(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override IIncomingEmailMessageHandler GetHandler(IWrappedImapClient imapClient, IMailMessage message)
        {
            if (message.From.Address != ExpectedSender)
            {
                return ConstructHandler(imapClient, JunkMessageHandlerType);
            }

            return base.GetHandler(imapClient, message);
        }

        #endregion
    }
}
