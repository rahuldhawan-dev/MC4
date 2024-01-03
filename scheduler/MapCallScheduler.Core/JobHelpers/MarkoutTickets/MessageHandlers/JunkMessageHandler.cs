using MapCallScheduler.Library.Email;

namespace MapCallScheduler.JobHelpers.MarkoutTickets.MessageHandlers
{
    public class JunkMessageHandler : JunkMessageHandlerBase<IMarkoutTicketServiceConfiguration>
    {
        #region Constructors

        public JunkMessageHandler(IncomingEmailMessageHandlerArgs<IMarkoutTicketServiceConfiguration> args) : base(args) {}

        #endregion
    }
}