using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Data
{
    public abstract class OperatingCenterSecuredRepositoryBase<TEntity> : MapCallSecuredRepositoryBase<TEntity>
        where TEntity : class, IThingWithOperatingCenter
    {
        public virtual IEnumerable<int> UserOperatingCenterIds => MatchingRolesForCurrentUser.OperatingCenters;

        public override ICriteria Criteria => CurrentUserCanAccessAllTheRecords
            ? base.Criteria
            : base.Criteria.Add(Restrictions.In("OperatingCenter.Id",
                UserOperatingCenterIds.ToArray()));

        public override IQueryable<TEntity> Linq => CurrentUserCanAccessAllTheRecords
            ? base.Linq
            : from x in base.Linq
              where UserOperatingCenterIds.Contains(x.OperatingCenter.Id)
              select x;

        protected OperatingCenterSecuredRepositoryBase(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }
    }
}
