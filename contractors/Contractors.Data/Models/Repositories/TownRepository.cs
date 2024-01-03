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
    public class TownRepository : SecuredRepositoryBase<Town, ContractorUser>, ITownRepository
    {
        #region Properties

        public override IQueryable<Town> Linq
        {
            get
            {
                return (from t in base.Linq
                        where
                            (from oct in t.OperatingCentersTowns
                             where CurrentUser.Contractor.OperatingCenters.Contains(oct.OperatingCenter)
                             select oct).Any()
                        orderby t.ShortName
                        select t);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                var crit = DetachedCriteria.For<OperatingCenterTown>()
                    .SetProjection(Projections.Id())
                    .CreateAlias("Town", "oct")
                    .Add(Restrictions.EqProperty("oct.Id", "t.Id"))
                    .Add(Restrictions.In("OperatingCenter.Id", CurrentUser.OperatingCenterIds));

                return Session.CreateCriteria<Town>("t")
                    .Add(Subqueries.Exists(crit));
            }
        }

        #endregion

        #region Constructors

        public TownRepository(ISession session,
            IAuthenticationService<ContractorUser> authenticationService,
            IContainer container) : base(session, authenticationService,
            container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<Town> GetByOperatingCenterId(int operatingCenterId)
        {
            return (from t in Linq
                    where t.OperatingCentersTowns.Any(x => x.OperatingCenter.Id == operatingCenterId)
                    select t);
        }

        public IEnumerable<Town> GetByCountyId(int countyId)
        {
            return (from t in Linq where t.County.Id == countyId select t);
        }

        #endregion
    }
}
