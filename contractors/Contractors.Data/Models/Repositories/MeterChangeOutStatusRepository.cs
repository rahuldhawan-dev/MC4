using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class MeterChangeOutStatusRepository : SecuredRepositoryBase<MeterChangeOutStatus, ContractorUser>, IMeterChangeOutStatusRepository
    {
        #region Constructor

        public MeterChangeOutStatusRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Public Methods

        public MeterChangeOutStatus GetScheduledStatus()
        {
            return Linq.Single(x => x.Description == "Scheduled");
        }

        public int[] GetNotScheduledLVMOrVaultStatuses()
        {
            var excludedStatuses = new[] { "Scheduled", "LVM", "Vault" };
            return Linq.Where(x => !excludedStatuses.Contains(x.Description)).Select(x => x.Id).ToArray();
        }

        #endregion
    }
}
