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
    public class EmployeeAssignmentRepository : MapCallSecuredRepositoryBase<EmployeeAssignment>,
        IEmployeeAssignmentRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        #endregion

        #region Properties

        public override RoleModules Role => ROLE;

        public override RoleMatch MatchingRolesForCurrentUser => _roleMatch ??
                                                                 (_roleMatch = CurrentUser.GetCachedMatchingRoles(ROLE,
                                                                     RoleActions.UserAdministrator));

        public override ICriteria Criteria
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                {
                    return base.Criteria;
                }

                if (MatchingRolesForCurrentUser.CanAccessRole)
                {
                    var critter = base.Criteria
                                      .CreateAlias("AssignedTo", "at")
                                      .CreateAlias("at.OperatingCenter", "oc");

                    return critter.Add(Restrictions.Or(Restrictions.Eq("AssignedTo.Id", CurrentUser.Employee?.Id),
                        Restrictions.In("oc.Id", GetUserOperatingCenterIds())));
                }

                return base.Criteria.Add(Restrictions.Eq("AssignedTo.Id", CurrentUser.Employee?.Id));
            }
        }

        public override IQueryable<EmployeeAssignment> Linq
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                {
                    return base.Linq;
                }

                if (MatchingRolesForCurrentUser.CanAccessRole)
                {
                    return base.Linq.Where(x => GetUserOperatingCenterIds().Contains(x.AssignedTo.OperatingCenter.Id));
                }

                return base.Linq.Where(x => CurrentUser.Employee != null && x.AssignedTo.Id == CurrentUser.Employee.Id);
            }
        }

        #endregion

        #region Constructors

        public EmployeeAssignmentRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService) : base(session, container, authenticationService,
            roleRepo) { }

        #endregion
    }

    public interface IEmployeeAssignmentRepository : IRepository<EmployeeAssignment> { }
}
