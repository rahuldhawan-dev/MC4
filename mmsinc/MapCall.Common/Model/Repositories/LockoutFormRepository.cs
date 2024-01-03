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
    public interface ILockoutFormRepository : IRepository<LockoutForm> { }

    public class LockoutFormRepository : MapCallSecuredRepositoryBase<LockoutForm>, ILockoutFormRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsLockoutForms;

        #endregion

        #region Properties

        public override RoleModules Role
        {
            get { return ROLE; }
        }

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

        public override IQueryable<LockoutForm> Linq
        {
            get
            {
                if (CurrentUserCanAccessAllTheRecords)
                    return base.Linq;

                var opCenterIds = GetUserOperatingCenterIds();
                return base.Linq.Where(x => opCenterIds.Contains(x.OperatingCenter.Id));
            }
        }

        #endregion

        public LockoutFormRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }
}
