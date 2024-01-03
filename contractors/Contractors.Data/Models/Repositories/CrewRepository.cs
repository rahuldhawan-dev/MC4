using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class CrewRepository : SecuredRepositoryBase<Crew, ContractorUser>
    {
        #region Properties

        public override IQueryable<Crew> Linq
        {
            get
            {
                return
                    (from c in base.Linq where c.Contractor.Id == CurrentUser.Contractor.Id select c);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria
                    .Add(Restrictions.Eq("Contractor.Id", CurrentUser.Contractor.Id));
            }
        }

        #endregion

        #region Constructors

        public CrewRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion
    }
}
