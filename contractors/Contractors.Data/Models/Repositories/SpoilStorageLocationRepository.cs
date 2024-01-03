using System.Linq;

using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class SpoilStorageLocationRepository : SecuredRepositoryBase<SpoilStorageLocation, ContractorUser>, ISpoilStorageLocationRepository
    {
        #region Properties

        public override IQueryable<SpoilStorageLocation> Linq
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
                return
                    base.Criteria.Add(Restrictions.InG("OperatingCenter",
                        CurrentUser.Contractor.OperatingCenters.ToArray()));
            }
        }

        #endregion

        #region Constructor

        public SpoilStorageLocationRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        public IQueryable<SpoilStorageLocation> GetAllInOperatingCenter(int operatingCenterId)
        {
            if (!CurrentUser.Contractor.OperatingCenters.Any(x => x.Id == operatingCenterId))
            {
                // We won't be able to query any further than this, so let's just
                // return an empty without hitting the database.
                 return Enumerable.Empty<SpoilStorageLocation>().AsQueryable();
            }

            return Linq.Where(x => x.OperatingCenter.Id == operatingCenterId);
        }
    }
}
