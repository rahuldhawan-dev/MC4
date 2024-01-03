using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.MarkoutTickets
{
    /// <summary>
    /// Parses mail messages into OneCallMarkoutAudit objects, performs an audit
    /// on existing tickets with a Repository, and emails on failure with a
    /// DeveloperEmailer.
    /// </summary>
    public class MarkoutTicketAuditor : IMarkoutTicketAuditor
    {
        #region Constants

        public const string AUDIT_FAILED = "Markout Ticket Audit Failed";

        #endregion

        #region Private Members

        private readonly IRepository<OneCallMarkoutTicket> _repository;

        #endregion

        #region Constructors

        public MarkoutTicketAuditor(IRepository<OneCallMarkoutTicket> repository)
        {
            _repository = repository;
        }

        #endregion

        #region Exposed Methods

        public void Audit(OneCallMarkoutAudit audit)
        {
            foreach (var num in audit.TicketNumbers)
            {
                if (!_repository.Where(
                    t =>
                        t.RequestNumber == num.RequestNumber &&
                        t.CDCCode == num.CDCCode &&
                        t.MessageType.Description == num.MessageType.Description).Any())
                {
                    audit.Success = false;
                    return;
                }
            }

            audit.Success = true;
        }

        #endregion
    }
}