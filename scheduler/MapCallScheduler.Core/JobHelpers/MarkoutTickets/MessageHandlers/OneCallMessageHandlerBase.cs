using MapCallScheduler.Library.Email;
using MMSINC.Interface;

namespace MapCallScheduler.JobHelpers.MarkoutTickets.MessageHandlers
{
    public abstract class OneCallMessageHandlerBase<TEntity, TParser> : IncomingEmailMessageHandlerBase<IMarkoutTicketServiceConfiguration, TEntity, TParser>
        where TParser : IParser<TEntity>
    {
        public OneCallMessageHandlerBase(IncomingEmailMessageHandlerArgs<IMarkoutTicketServiceConfiguration, TEntity, TParser> args) : base(args) {}
    }
}