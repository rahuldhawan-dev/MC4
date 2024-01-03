using System.Collections.Generic;
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
    public class UnionContractRepository : MapCallSecuredRepositoryBase<UnionContract>, IUnionContractRepository
    {
        #region Properties

        public override ICriteria Criteria => CurrentUserCanAccessAllTheRecords
            ? base.Criteria
            : base.Criteria.Add(Restrictions.In("OperatingCenter.Id", UserOperatingCenterIds.ToArray()));

        public override IQueryable<UnionContract> Linq => CurrentUserCanAccessAllTheRecords
            ? base.Linq
            // trust me, I hate the .ToArray call just as much as you do
            : (from c in base.Linq where UserOperatingCenterIds.ToArray().Contains(c.OperatingCenter.Id) select c);

        public IEnumerable<int> UserOperatingCenterIds => MatchingRolesForCurrentUser.OperatingCenters;

        public override RoleModules Role => RoleModules.HumanResourcesUnion;

        #endregion

        public UnionContractRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }

    public interface IUnionContractRepository : IRepository<UnionContract> { }
}
