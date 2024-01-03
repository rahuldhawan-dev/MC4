using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories {
    public interface IMeterChangeOutStatusRepository : IRepository<MeterChangeOutStatus>
    {
        MeterChangeOutStatus GetScheduledStatus();
        int[] GetNotScheduledLVMOrVaultStatuses();
    }
}