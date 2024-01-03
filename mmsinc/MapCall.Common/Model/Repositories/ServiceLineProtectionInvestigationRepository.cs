using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ServiceLineProtectionInvestigationRepository :
        MapCallSecuredRepositoryBase<ServiceLineProtectionInvestigation>, IServiceLineProtectionInvestigationRepository
    {
        #region Exposed Methods

        public override IQueryable<ServiceLineProtectionInvestigation> Linq
        {
            get
            {
                var linq = base.Linq;
                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    linq = linq.Where(x => opCenterIds.Contains(x.OperatingCenter.Id));
                }

                return linq;
            }
        }

        public override ICriteria Criteria
        {
            get
            {
                var crit = base.Criteria
                               .CreateAlias("Coordinate", "c", JoinType.LeftOuterJoin);
                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    crit = crit.Add(Restrictions.In("OperatingCenter.Id", opCenterIds));
                }

                //// Performance critical for searches/indexes, make sure associations being displayed are eager loaded.
                //crit = crit.SetFetchMode("Street", FetchMode.Eager)
                //           .SetFetchMode("CrossStreet", FetchMode.Eager);
                return crit;
            }
        }

        #endregion

        #region Properties

        public override RoleModules Role
        {
            get { return RoleModules.ServiceLineProtection; }
        }

        #endregion

        public ServiceLineProtectionInvestigationRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }

    public interface IServiceLineProtectionInvestigationRepository : IRepository<ServiceLineProtectionInvestigation> { }
}
