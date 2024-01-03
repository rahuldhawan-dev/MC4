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
    public class StreetRepository : SecuredRepositoryBase<Street, ContractorUser>, IStreetRepository
    {
        #region Properties

        public override IQueryable<Street> Linq
        {
            get
            {
                return (from s in base.Linq
                        where
                            (from oct in s.Town.OperatingCentersTowns
                             where CurrentUser.Contractor.OperatingCenters.Contains(oct.OperatingCenter)
                             select oct).Any()
                        select s);
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                var crit = DetachedCriteria.For<Town>()
                    .SetProjection(Projections.Id())
                    .CreateAlias("OperatingCentersTowns", "oct")
                    .Add(Restrictions.EqProperty("Id", "st.Town.Id"))
                    .Add(Restrictions.In("oct.OperatingCenter.Id", CurrentUser.OperatingCenterIds));

                return Session.CreateCriteria<Street>("st")
                    .Add(Subqueries.Exists(crit));
            }
        }

        #endregion

        #region Constructors
        
        public StreetRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Exposed Methods
        
        public IEnumerable<Street> GetByTownId(int townId)
        {
            return (from s in Linq where s.Town.Id == townId select s);
        }

        #endregion
    }
}
