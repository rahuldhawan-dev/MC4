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
    public class LocalRepository : MapCallSecuredRepositoryBase<Local>, ILocalRepository
    {
        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                return CurrentUserCanAccessAllTheRecords
                    ? base.Criteria
                    : base.Criteria.Add(Restrictions.In("OperatingCenter.Id", GetUserOperatingCenterIds().ToArray()));
            }
        }

        public override IQueryable<Local> Linq
        {
            get
            {
                return CurrentUserCanAccessAllTheRecords
                    ? base.Linq
                    : (from l in base.Linq
                       where GetUserOperatingCenterIds().ToArray().Contains(l.OperatingCenter.Id)
                       select l);
            }
        }

        public override RoleModules Role
        {
            get { return RoleModules.HumanResourcesUnion; }
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<Local> GetByUnionId(int unionId)
        {
            return (from l in Linq where l.Union.Id == unionId select l);
        }

        public IEnumerable<Local> GetByOperatingCenterId(int operatingCenterId)
        {
            return (from l in Linq where l.OperatingCenter.Id == operatingCenterId select l);
        }

        #endregion

        public LocalRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }

    public interface ILocalRepository : IRepository<Local>
    {
        #region Abstract Methods

        IEnumerable<Local> GetByUnionId(int unionId);
        IEnumerable<Local> GetByOperatingCenterId(int id);

        #endregion
    }
}
