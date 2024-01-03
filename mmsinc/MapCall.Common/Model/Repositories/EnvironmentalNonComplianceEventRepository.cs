using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class EnvironmentalNonComplianceEventRepository :
        MapCallSecuredRepositoryBase<EnvironmentalNonComplianceEvent>, IEnvironmentalNonComplianceEventRepository
    {
        public override ICriteria Criteria =>
            CurrentUserCanAccessAllTheRecords
                ? base.Criteria
                : base.Criteria.CreateAlias("OperatingCenter", "oc")
                      .Add(Restrictions.In("oc.Id", GetUserOperatingCenterIds()));

        public override IQueryable<EnvironmentalNonComplianceEvent> Linq =>
            CurrentUserCanAccessAllTheRecords
                ? base.Linq
                : base.Linq.Where(x => GetUserOperatingCenterIds().Contains(x.OperatingCenter.Id));

        public EnvironmentalNonComplianceEventRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        public override RoleModules Role => RoleModules.EnvironmentalGeneral;
    }

    public interface IEnvironmentalNonComplianceEventRepository : IRepository<EnvironmentalNonComplianceEvent> { }
}
