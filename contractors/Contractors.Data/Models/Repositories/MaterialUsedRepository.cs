using System.Linq;

using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class MaterialUsedRepository : SecuredRepositoryBase<MaterialUsed, ContractorUser>
    {
        #region Properties

        public override IQueryable<MaterialUsed> Linq
        {
            get
            {
                return (from mu in base.Linq
                        where
                            mu.WorkOrder.AssignedContractor ==
                                CurrentUser.Contractor
                        select mu);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria
                    .CreateAlias("WorkOrder", "wo")
                    .Add(Restrictions.Eq("wo.AssignedContractor",
                        CurrentUser.Contractor));
            }
        }

        #endregion

        #region Constructors

        public MaterialUsedRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion
    }
}
