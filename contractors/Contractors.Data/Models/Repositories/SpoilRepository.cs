using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class SpoilRepository : SecuredRepositoryBase<Spoil, ContractorUser>, ISpoilRepository
    {
        #region Properties

        public override IQueryable<Spoil> Linq
        {
            get
            {
                return (from s in base.Linq
                        where s.WorkOrder.AssignedContractor == CurrentUser.Contractor 
                        select s);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria
                    .CreateAlias("WorkOrder", "wo")
                    .Add(Restrictions.Eq("wo.AssignedContractor", CurrentUser.Contractor));
            }
        }

        #endregion

        #region Constructor

        public SpoilRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion
    }
}
