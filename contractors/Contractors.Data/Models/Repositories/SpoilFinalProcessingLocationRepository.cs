using System.Linq;

using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class SpoilFinalProcessingLocationRepository : SecuredRepositoryBase<SpoilFinalProcessingLocation, ContractorUser>, ISpoilFinalProcessingLocationRepository
    {
        #region Properties

        public override IQueryable<SpoilFinalProcessingLocation> Linq
        {
            get
            {
                return (from s in base.Linq
                        where CurrentUser.Contractor.OperatingCenters.Contains(s.OperatingCenter)
                        select s);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria.Add(Restrictions.In("OperatingCenter", CurrentUser.Contractor.OperatingCenters.ToArray()));
            }
        }

        #endregion

        #region Constructor

        public SpoilFinalProcessingLocationRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion
    }
}
