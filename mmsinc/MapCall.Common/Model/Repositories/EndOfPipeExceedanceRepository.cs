using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;
using System.Linq;

namespace MapCall.Common.Model.Repositories
{
    public class EndOfPipeExceedanceRepository : MapCallSecuredRepositoryBase<EndOfPipeExceedance>, IEndOfPipeExceedanceRepository
    {
        public override ICriteria Criteria =>
            CurrentUserCanAccessAllTheRecords
                ? base.Criteria
                // Had to rewrite below because last implementation was causing Edit / View pages to not search by Id on the entity
                : base.Criteria.Add(Restrictions.In("OperatingCenter.Id", GetUserOperatingCenterIds().ToArray()));

        public override IQueryable<EndOfPipeExceedance> Linq =>
            CurrentUserCanAccessAllTheRecords
                ? base.Linq
                : base.Linq.Where(x => GetUserOperatingCenterIds().Contains(x.OperatingCenter.Id));

        public EndOfPipeExceedanceRepository(ISession session, IContainer container, IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container, authenticationService, roleRepo) { }

        public override RoleModules Role => RoleModules.EnvironmentalGeneral;
    }

    public interface IEndOfPipeExceedanceRepository : IRepository<EndOfPipeExceedance> { }
}
