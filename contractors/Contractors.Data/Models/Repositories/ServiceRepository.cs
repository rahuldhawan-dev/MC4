using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class ServiceRepository : SecuredRepositoryBase<Service, ContractorUser>, IServiceRepository
    {
        #region Properties

        public override IQueryable<Service> Linq =>
            (from s in base.Linq
             where CurrentUser.Contractor.OperatingCenters.Contains(
                 s.OperatingCenter)
             select s);

        public override ICriteria Criteria =>
            base.Criteria.Add(Restrictions.In("OperatingCenter.Id",
                CurrentUser.OperatingCenterIds));

        #endregion

        #region Constructors

        public ServiceRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Private Methods

        private IQueryable<Service> ByInstallationNumberAndOperatingCenterAndSampleSites(
            string installation, 
            int operatingCenterId)
        {
            return Linq.Where(x => x.Installation != null && 
                                   x.Installation != string.Empty &&
                                   x.Installation == installation && 
                                   x.OperatingCenter.Id == operatingCenterId && 
                                   x.Premise.SampleSites.Any());
        }

        #endregion

        #region Exposed Methods

        public bool AnyWithInstallationNumberAndOperatingCenterAndSampleSites(string installation, int operatingCenterId)
        {
            return ByInstallationNumberAndOperatingCenterAndSampleSites(installation, operatingCenterId).Any();
        }

        public IEnumerable<Service> FindByStreetId(int streetId)
        {
            // base because this is a report and we aren't conerned with roles
            return base.Linq.Where(x => x.Street != null && x.Street.Id == streetId);
        }

        #endregion
    }
}