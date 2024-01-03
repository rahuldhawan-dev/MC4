using StructureMap;

namespace Permits.Data.Client.Repositories
{
    /// <inheritdoc cref="IPermitsRepositoryFactory" />
    public class PermitsRepositoryFactory : IPermitsRepositoryFactory
    {
        #region Fields

        private readonly IContainer _container;

        #endregion

        #region Constructor

        public PermitsRepositoryFactory(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public IPermitsDataClientRepository<TEntity> GetRepository<TEntity>(string userName)
        {
            return _container.With("userName").EqualTo(userName).GetInstance<IPermitsDataClientRepository<TEntity>>();
        }

        #endregion
    }
}
