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
    public abstract class MapCallEmployeeSecuredRepositoryBase<TEntity> : MapCallSecuredRepositoryBase<TEntity>
        where TEntity : class, IThingWithEmployee
    {
        #region Properties

        public override ICriteria Criteria
        {
            get
            {
                var critter = base.Criteria
                                  .CreateAlias("Employee", "e")
                                  .CreateAlias("e.OperatingCenter", "oc");
                return CurrentUserCanAccessAllTheRecords
                    ? critter
                    : critter.Add(Restrictions.In("oc.Id", GetUserOperatingCenterIds()));
            }
        }

        public override IQueryable<TEntity> Linq => CurrentUserCanAccessAllTheRecords
            ? base.Linq
            : base.Linq.Where(x => GetUserOperatingCenterIds().Contains(x.Employee.OperatingCenter.Id));

        #endregion

        #region Constructors

        protected MapCallEmployeeSecuredRepositoryBase(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        #endregion
    }
}
