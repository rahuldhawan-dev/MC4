using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class OperatingCenterRepository : SecuredRepositoryBase<OperatingCenter, ContractorUser>, IOperatingCenterRepository
    {
        #region Properties

        public override IQueryable<OperatingCenter> Linq => (from o in base.Linq where CurrentUser.Contractor.OperatingCenters.Contains(o) select o);

        public override ICriteria Criteria => base.Criteria
                                                     .Add(Restrictions.In("Id", CurrentUser.OperatingCenterIds));

        #endregion

        #region Constructors

        public OperatingCenterRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }
        
        #endregion
    }
}
