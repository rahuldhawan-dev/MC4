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
    public class EstimatingProjectRepository : OperatingCenterSecuredRepositoryBase<EstimatingProject>,
        IEstimatingProjectRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesEstimatingProjects;

        #endregion

        #region Properties

        public override ICriteria Criteria => CurrentUserCanAccessAllTheRecords
            ? base.Criteria
            : base.Criteria.Add(Restrictions.In("OperatingCenter.Id", UserOperatingCenterIds.ToArray()));

        public override IQueryable<EstimatingProject> Linq => CurrentUserCanAccessAllTheRecords
            ? base.Linq
            : (from p in base.Linq
               where UserOperatingCenterIds.Contains(p.OperatingCenter.Id)
               select p);

        public override RoleModules Role => ROLE;

        #endregion

        #region Constructors

        public EstimatingProjectRepository(IRepository<AggregateRole> roleRepo, ISession session, IContainer container,
            IAuthenticationService<User> authenticationService) : base(session, container, authenticationService,
            roleRepo) { }

        #endregion

        #region Exposed Methods

        public override IQueryable<EstimatingProject> GetAllSorted()
        {
            return GetAll().OrderBy(y => y.WBSNumber).ThenBy(y => y.Street).ThenBy(y => y.Town.ShortName)
                           .ThenBy(y => y.OperatingCenter.OperatingCenterCode);
        }

        #endregion
    }

    public interface IEstimatingProjectRepository : IRepository<EstimatingProject> { }
}
