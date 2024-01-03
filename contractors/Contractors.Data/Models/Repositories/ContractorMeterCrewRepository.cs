using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class ContractorMeterCrewRepository : SecuredRepositoryBase<ContractorMeterCrew, ContractorUser>, IContractorMeterCrewRepository
    {
        #region Properties

        #region Base Linq/Criteria

        public override IQueryable<ContractorMeterCrew> Linq
        {
            get { return base.Linq.Where(wo => wo.Contractor == CurrentUser.Contractor); }
        }

        public override ICriteria Criteria
        {
            get { return base.Criteria.Add(Restrictions.Eq("Contractor", CurrentUser.Contractor)); }
        }

        #endregion

        #endregion

        #region Constructors

        public ContractorMeterCrewRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion
    }
}
