using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class MainBreakRepository : SecuredRepositoryBase<MainBreak, ContractorUser>
    {
        #region Properties

        public override IQueryable<MainBreak> Linq
        {
            get
            {
                return (from m in base.Linq
                        where m.WorkOrder.AssignedContractor.Id == CurrentUser.Contractor.Id
                        select m);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria
                    .CreateAlias("WorkOrder", "o")
                    .CreateAlias("o.AssignedContractor", "c")
                    .Add(Restrictions.Eq("c.Id", CurrentUser.Contractor.Id));
            }
        }

        #endregion

        #region Constructor

        public MainBreakRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion
    }
}
