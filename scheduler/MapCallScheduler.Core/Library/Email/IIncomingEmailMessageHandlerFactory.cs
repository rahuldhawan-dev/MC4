using MMSINC.Interface;

namespace MapCallScheduler.Library.Email
{
    public interface IIncomingEmailMessageHandlerFactory
    {
        #region Abstract Methods

        IIncomingEmailMessageHandler GetHandler(IWrappedImapClient imapClient, IMailMessage message);

        #endregion
    }
}