using MapCall.Common.Model.Entities;
using MapCallScheduler.Metadata;

namespace MapCallScheduler.JobHelpers.MarkoutTickets.MessageHandlers
{
    [HandlesSubject(SUBJECT_REGEX)]
    public class MarkoutTicketMessageHandler : OneCallMessageHandlerBase<OneCallMarkoutTicket, IMarkoutTicketParser>
    {
        #region Constants

        public const string SUBJECT_REGEX = "^(ROUTINE|EMERGENCY|BROADCAST|UPDATE|CANCELLED)\\s+(\\d+)$",
            DUPLICATE_BOX = "INBOX.Duplicates";

        #endregion

        #region Constructors

        public MarkoutTicketMessageHandler(IncomingEmailMessageHandlerArgs<IMarkoutTicketServiceConfiguration, OneCallMarkoutTicket, IMarkoutTicketParser> args) : base(args) {}

        #endregion

        #region Private Methods

        protected override void SaveAndCleanup(uint uid, string mailbox, OneCallMarkoutTicket entity)
        {
            if (!_repository.Any(t => t.RequestNumber == entity.RequestNumber && t.CDCCode == entity.CDCCode))
            {
                base.SaveAndCleanup(uid, mailbox, entity);
            }
            else if (_config.IncomingEmailConfig.MakeChanges)
            {
                _imapClient.MoveMessage(uid, DUPLICATE_BOX, mailbox);
            }
        }

        #endregion
    }
}
