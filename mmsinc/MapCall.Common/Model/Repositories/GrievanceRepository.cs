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
    public class GrievanceRepository : MapCallSecuredRepositoryBase<Grievance>, IGrievanceRepository
    {
        #region Private Members

        private readonly IRepository<GrievanceEmployee> _employeeRepo;

        #endregion

        #region Properties

        public override ICriteria Criteria => CurrentUserCanAccessAllTheRecords
            ? base.Criteria
            : base.Criteria.Add(Restrictions.In("OperatingCenter.Id", UserOperatingCenterIds.ToArray()));

        public override IQueryable<Grievance> Linq => CurrentUserCanAccessAllTheRecords
            ? base.Linq
            // trust me, I hate the .ToArray call just as much as you do
            : (from c in base.Linq where UserOperatingCenterIds.ToArray().Contains(c.OperatingCenter.Id) select c);

        public IEnumerable<int> UserOperatingCenterIds => MatchingRolesForCurrentUser.OperatingCenters;

        public override RoleModules Role => RoleModules.HumanResourcesUnion;

        public IEnumerable<Grievance> GetByEmployeeId(int employeeId)
        {
            return _employeeRepo.Linq.Where(x => x.Employee.Id == employeeId).Select(x => x.Grievance).OrderByDescending(x => x.Id);
        }

        #endregion

        public GrievanceRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo,
            IRepository<GrievanceEmployee> employeeRepo) : base(session, container,
            authenticationService, roleRepo)
        {
            _employeeRepo = employeeRepo;
        }
    }

    public interface IGrievanceRepository : IRepository<Grievance>
    {
        IEnumerable<Grievance> GetByEmployeeId(int employeeId);
    }
}
