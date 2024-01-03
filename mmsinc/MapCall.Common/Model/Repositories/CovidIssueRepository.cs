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
    public class CovidIssueRepository : MapCallEmployeeSecuredRepositoryBase<CovidIssue>, ICovidIssueRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.HumanResourcesCovid;

        #endregion

        #region Properties

        public override RoleModules Role => ROLE;

        #endregion

        #region Constructors

        public CovidIssueRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        #endregion
    }

    public interface ICovidIssueRepository : IRepository<CovidIssue> { }
}
