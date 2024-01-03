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
    public interface IJobSiteCheckListRepository : IRepository<JobSiteCheckList> { }

    public class JobSiteCheckListRepository : MapCallSecuredRepositoryBase<JobSiteCheckList>,
        IJobSiteCheckListRepository
    {
        #region Private Members

        private RoleMatch _roleMatch;

        #endregion

        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                {
                    return base.Criteria;
                }

                var opCenterIds = GetUserOperatingCenterIds();
                return base.Criteria.Add(Restrictions.In("OperatingCenter.Id", opCenterIds));
            }
        }

        public override IQueryable<JobSiteCheckList> Linq
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                {
                    return base.Linq;
                }

                var opCenterIds = GetUserOperatingCenterIds();
                return base.Linq.Where(x => opCenterIds.Contains(x.OperatingCenter.Id));
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.FieldServicesWorkManagement; }
        }

        #endregion

        #region Constructors

        public JobSiteCheckListRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService) : base(session, container, authenticationService,
            roleRepo) { }

        #endregion
    }
}
