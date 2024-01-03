using log4net;
using MapCallScheduler.JobHelpers.MarkoutTickets.MessageHandlers;
using MapCallScheduler.Library;
using MapCallScheduler.Library.Email;

namespace MapCallScheduler.JobHelpers.MarkoutTickets
{
    /// <summary>
    /// Uses a OneCallMessageHandler to process email messages and deletes them from
    /// the mailbox with an ImapClient.
    /// </summary>
    public class OneCallMessageProcessorService : IncomingEmailMessageProcessorServiceBase<IOneCallMessageHandlerFactory>, IOneCallMessageProcessorService
    {
        #region Constructors

        public OneCallMessageProcessorService(ILog log, IWrappedImapClient imapClient, IOneCallMessageHandlerFactory handlerFactory) : base(log, imapClient, handlerFactory) {}

        #endregion
    }

    public interface IOneCallMessageProcessorService : IIncomingEmailMessageProcessorService {}
}