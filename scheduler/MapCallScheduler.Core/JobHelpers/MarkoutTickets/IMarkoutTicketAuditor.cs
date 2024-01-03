using MapCall.Common.Model.Entities;

namespace MapCallScheduler.JobHelpers.MarkoutTickets
{
    public interface IMarkoutTicketAuditor
    {
        #region Abstract Methods

        void Audit(OneCallMarkoutAudit audit);

        #endregion
    }
}