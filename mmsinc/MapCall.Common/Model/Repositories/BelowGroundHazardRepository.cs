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
    public interface IBelowGroundHazardRepository : IRepository<BelowGroundHazard> { }

    public class BelowGroundHazardRepository : MapCallSecuredRepositoryBase<BelowGroundHazard>, IBelowGroundHazardRepository
    {
        #region Constructor

        public BelowGroundHazardRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        #endregion

        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                var crit = base.Criteria
                               .CreateAlias("OperatingCenter", "criteriaOperatingCenter",
                                    JoinType.LeftOuterJoin);
                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    crit = crit.Add(Restrictions.In("criteriaOperatingCenter.Id", opCenterIds));
                }

                return crit;
            }
        }

        public override IQueryable<BelowGroundHazard> Linq
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

        #region Constants

        public override RoleModules Role => RoleModules.FieldServicesAssets;

        #endregion

        #endregion
    }
}