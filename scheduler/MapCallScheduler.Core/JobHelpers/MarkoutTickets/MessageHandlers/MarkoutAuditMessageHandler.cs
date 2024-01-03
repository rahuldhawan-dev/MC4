using MapCall.Common.Model.Entities;
using MapCallScheduler.Metadata;
using MMSINC.Interface;

namespace MapCallScheduler.JobHelpers.MarkoutTickets.MessageHandlers
{
    [HandlesSubject(SUBJECT_REGEX)]
    public class MarkoutAuditMessageHandler : OneCallMessageHandlerBase<OneCallMarkoutAudit, IMarkoutAuditParser>
    {
        #region Constants

        public const string SUBJECT_REGEX = "^Center\\s+Messages$";

        #endregion

        #region Private Members

        private readonly IMarkoutTicketAuditor _auditor;
        private readonly IAuditFailureEmailer _emailer;
        private OneCallMarkoutAudit _audit;

        #endregion

        #region Constructors

        public MarkoutAuditMessageHandler(IncomingEmailMessageHandlerArgs<IMarkoutTicketServiceConfiguration, OneCallMarkoutAudit, IMarkoutAuditParser> args, IMarkoutTicketAuditor auditor, IAuditFailureEmailer emailer) : base(args)
        {
            _auditor = auditor;
            _emailer = emailer;
        }

        #endregion

        #region Private Methods

        protected override OneCallMarkoutAudit Parse(uint uid, string mailbox, IMailMessage message)
        {
            // need to keep a reference so we can send an email later once it has an id
            return (_audit = base.Parse(uid, mailbox, message));
        }

        #endregion

        #region Exposed Methods

        public override void Dispose()
        {
            base.Dispose();

            _emailer.Dispose();
        }

        protected override void BeforeSaveAndCleanup(IMailMessage message, OneCallMarkoutAudit entity)
        {
            base.BeforeSaveAndCleanup(message, entity);

            _auditor.Audit(_audit);

            if (!_audit.Success.Value)
            {
                _emailer.SendMessage(_audit);
            }
        }

        #endregion
    }
}