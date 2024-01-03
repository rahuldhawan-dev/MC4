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
    public interface IMarkoutDamageRepository : IRepository<MarkoutDamage> { }

    public class MarkoutDamageRepository : MapCallSecuredRepositoryBase<MarkoutDamage>, IMarkoutDamageRepository
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

        public override IQueryable<MarkoutDamage> Linq
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

        public MarkoutDamageRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }
}
