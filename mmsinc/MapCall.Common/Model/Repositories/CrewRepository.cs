using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class CrewRepository : OperatingCenterSecuredRepositoryBase<Crew>, ICrewRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        public CrewRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        public override RoleModules Role => ROLE;
    }

    public interface ICrewRepository : IRepository<Crew> { }
}
