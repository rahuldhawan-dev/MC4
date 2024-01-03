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
    public class RecurringProjectRepository : MapCallSecuredRepositoryBase<RecurringProject>,
        IRecurringProjectRepository
    {
        #region Properties

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

                // Performance critical for searches/indexes, make sure associations being displayed are eager loaded.
                crit = crit.SetFetchMode("Town", FetchMode.Eager)
                           .SetFetchMode("RecurringProjectType", FetchMode.Eager);
                return crit;
            }
        }

        public override IQueryable<RecurringProject> Linq
        {
            get
            {
                var linq = base.Linq;

                // BIG NOTE: If the Linq includes the op center check, then anyone
                //           trying to add a .Where() afterwards will get an error 
                //           because NHibernate is awful. Fetching now has to be done
                //           with the AddFetchToLinqQuery method.
                if (!CurrentUserCanAccessAllTheRecords)
                {
                    var opCenterIds = GetUserOperatingCenterIds();
                    linq = linq.Where(x => opCenterIds.Contains(x.OperatingCenter.Id));
                }

                return linq;
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesProjects; }
        }

        #endregion

        public RecurringProjectRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }

    public interface IRecurringProjectRepository : IRepository<RecurringProject> { }
}
