using log4net;
using MapCallScheduler.Library;
using MapCallScheduler.Library.Email;
using StructureMap;

namespace MapCallScheduler.JobHelpers.MarkoutTickets
{
    /// <summary>
    /// Uses an ImapClient to download messages and feed them to a
    /// OneCallMessageProcessor service.
    /// </summary>
    public class OneCallMessageService : ImapMessageProcessorServiceBase<IMarkoutTicketServiceConfiguration, IOneCallMessageProcessorService>, IOneCallMessageService
    {
        #region Constructors

        public OneCallMessageService(ILog log, IImapClientFactory factory, IMarkoutTicketServiceConfiguration config, IContainer container) : base(log, factory, config, container) {}

        #endregion
    }

    public interface IOneCallMessageService : IImapMessageProcessorService {}
}
