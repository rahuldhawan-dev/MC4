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
    public class TownSectionRepository : SecuredRepositoryBase<TownSection, ContractorUser>, ITownSectionRepository
    {
        #region Properties

        public override IQueryable<TownSection> Linq
        {
            get
            {
                return (from ts in base.Linq
                        where
                            (from oct in ts.Town.OperatingCentersTowns
                             where CurrentUser.Contractor.OperatingCenters.Contains(oct.OperatingCenter)
                             select oct).Any()
                        select ts);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                var crit = DetachedCriteria.For<Town>()
                    .SetProjection(Projections.Id())
                    .CreateAlias("OperatingCentersTowns", "oct")
                    .Add(Restrictions.EqProperty("TownID", "ts.TownID"))
                    .Add(Restrictions.In("oct.OperatingCenter.Id", CurrentUser.OperatingCenterIds));

                return Session.CreateCriteria<TownSection>("ts")
                    .Add(Subqueries.Exists(crit));
            }
        }

        #endregion

        #region Constructors

        public TownSectionRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<TownSection> GetByTownId(int townId)
        {
            return (from ts in Linq where ts.Town.Id == townId select ts);
        }

        #endregion
    }
}