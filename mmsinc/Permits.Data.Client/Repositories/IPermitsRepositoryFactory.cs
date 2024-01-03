namespace Permits.Data.Client.Repositories
{
    /// <summary>
    /// Represents a factory that returns a repository that's correctly configured for
    /// user with the Permits API.
    /// </summary>
    public interface IPermitsRepositoryFactory
    {
        /// <summary>
        /// Returns a permits API repository configured with the given username.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="userName"></param>
        /// <returns></returns>
        IPermitsDataClientRepository<TEntity> GetRepository<TEntity>(string userName);
    }
}
