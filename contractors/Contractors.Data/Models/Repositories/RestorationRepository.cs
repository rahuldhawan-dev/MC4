using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class RestorationRepository : SecuredRepositoryBase<Restoration, ContractorUser>
    {
        #region Properties

        #region Base Linq/Criteria

        public override IQueryable<Restoration> Linq
        {
            get { return base.Linq.Where(wo => (wo.AssignedContractor == CurrentUser.Contractor) || (wo.AssignedContractor == null && wo.CreatedByContractor == CurrentUser.Contractor)); }
        }

        public override ICriteria Criteria
        {
            get
            {
                var assignedContractorHasValue = Restrictions.Eq("AssignedContractor", CurrentUser.Contractor);
                var createdByHasValueInstead = Restrictions.Conjunction().Add(Restrictions.IsNull("AssignedContractor")).Add(Restrictions.Eq("CreatedByContractor", CurrentUser.Contractor));
                var or = Restrictions.Or(assignedContractorHasValue, createdByHasValueInstead);
                return base.Criteria.Add(or);
            }
        }

        #endregion

        #endregion

        #region Constructors

        public RestorationRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion
    }
}
