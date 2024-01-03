using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IMeterChangeOutStatusRepository : IRepository<MeterChangeOutStatus>
    {
        MeterChangeOutStatus GetScheduledStatus();
        int[] GetNotScheduledLVMOrVaultStatuses();
    }

    public class MeterChangeOutStatusRepository : RepositoryBase<MeterChangeOutStatus>, IMeterChangeOutStatusRepository
    {
        #region Constructor

        public MeterChangeOutStatusRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public MeterChangeOutStatus GetScheduledStatus()
        {
            return Linq.Single(x => x.Description == "Scheduled");
        }

        public int[] GetNotScheduledLVMOrVaultStatuses()
        {
            var excludedStatuses = new[] {"Scheduled", "LVM", "Vault"};
            return Linq.Where(x => !excludedStatuses.Contains(x.Description)).Select(x => x.Id).ToArray();
        }

        #endregion
    }
}
