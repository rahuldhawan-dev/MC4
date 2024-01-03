using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class MarkoutRepository : SecuredRepositoryBase<Markout, ContractorUser>
    {
        #region Properties

        public override IQueryable<Markout> Linq
        {
            get
            {
                return
                    (from m in base.Linq
                     where
                         m.WorkOrder.AssignedContractor.Id ==
                             CurrentUser.Contractor.Id
                     select m);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria
                    .CreateAlias("WorkOrder", "o")
                    .Add(Restrictions.Eq("o.AssignedContractor",
                        CurrentUser.Contractor));
            }
        }

        #endregion

        #region Constructor

        public MarkoutRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion
    }
}
