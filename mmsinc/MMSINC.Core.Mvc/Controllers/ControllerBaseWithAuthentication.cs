using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using StructureMap;

namespace MMSINC.Controllers
{
    // TODO: Merge this functionality down into ControllerBase. 
    // This class exists solely to add AuthenticationService, but there's no reason it can't
    // exist on the base class. This class only adds unnecessary inheritance confusion.

    public class ControllerBaseWithAuthentication<TRepository, TEntity, TUser> : ControllerBase<TRepository, TEntity>
        where TRepository : class, IRepository<TEntity> where TUser : IAdministratedUser
    {
        #region Private Members

        private readonly IAuthenticationService<TUser> _authenticationService;

        #endregion

        #region Properties

        public IAuthenticationService<TUser> AuthenticationService
        {
            get { return _authenticationService; }
        }

        #endregion

        #region Constructors

        public ControllerBaseWithAuthentication(
            ControllerBaseWithAuthenticationArguments<TRepository, TEntity, TUser> args) : base(args)
        {
            _authenticationService = args.AuthenticationService;
        }

        #endregion
    }

    public class
        ControllerBaseWithAuthentication<TEntity, TUser> : ControllerBaseWithAuthentication<IRepository<TEntity>,
            TEntity, TUser>
        where TUser : IAdministratedUser
    {
        #region Constructors

        public ControllerBaseWithAuthentication(
            ControllerBaseWithAuthenticationArguments<IRepository<TEntity>, TEntity, TUser> args) : base(args) { }

        #endregion
    }

    public class
        ControllerBaseWithAuthenticationArguments<TRepository, TEntity, TUser> : ControllerBaseArguments<TRepository,
            TEntity>
        where TRepository : class, IRepository<TEntity>
        where TUser : IAdministratedUser
    {
        #region Private Members

        private readonly IAuthenticationService<TUser> _authenticationService;

        #endregion

        #region Properties

        public IAuthenticationService<TUser> AuthenticationService
        {
            get { return _authenticationService; }
        }

        #endregion

        #region Constructors

        public ControllerBaseWithAuthenticationArguments(TRepository repository, IContainer container,
            IViewModelFactory viewModelFactory, IAuthenticationService<TUser> authenticationService) : base(repository,
            container, viewModelFactory)
        {
            _authenticationService = authenticationService;
        }

        #endregion
    }
}
