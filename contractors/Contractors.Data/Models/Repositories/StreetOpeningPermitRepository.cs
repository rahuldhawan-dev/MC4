using System.Linq;

using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class StreetOpeningPermitRepository : SecuredRepositoryBase<StreetOpeningPermit, ContractorUser>
    {
        #region Properties

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

        public override IQueryable<StreetOpeningPermit> Linq
        {
            get
            {
                return (from sop in base.Linq
                        where sop.WorkOrder.AssignedContractor == CurrentUser.Contractor 
                        select sop);
            }
        }

        #endregion

        #region Constructor

        public StreetOpeningPermitRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion
    }
}
