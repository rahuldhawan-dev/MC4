using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Data
{
    public abstract class MapCallSecuredRepositoryBase<TEntity> : SecuredRepositoryBase<TEntity, User>
        where TEntity : class
    {
        #region Private Members

        protected RoleMatch _roleMatch;
        protected readonly IRepository<AggregateRole> _roleRepo;

        #endregion

        #region Properties

        public virtual RoleMatch MatchingRolesForCurrentUser =>
            _roleMatch ?? (_roleMatch = CurrentUser.GetQueryableMatchingRoles(_roleRepo, Role));

        public bool CurrentUserCanAccessAllTheRecords => _authenticationSerice.CurrentUser.IsAdmin ||
                                                         MatchingRolesForCurrentUser.HasWildCardMatch;

        #endregion

        #region Abstract Properties

        public abstract RoleModules Role { get; }

        #endregion

        #region Constructors

        public MapCallSecuredRepositoryBase(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService,
            IRepository<AggregateRole> roleRepo) : base(session, authenticationService, container)
        {
            _roleRepo = roleRepo;
        }

        #endregion

        #region Private Methods

        protected int[] GetUserOperatingCenterIds()
        {
            return MatchingRolesForCurrentUser.OperatingCenters;
        }

        #endregion
    }
}
