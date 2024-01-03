using System;
using MMSINC.Data.WebApi;

namespace Permits.Data.Client.Repositories
{
    public class PermitsDataClientRepository<TEntity> : RepositoryBase<TEntity>, IPermitsDataClientRepository<TEntity>
    {
        #region Constants

        public const string RSA_TOKEN_CONFIG_KEY = "PermitsRSAKey";

        #endregion

        #region Fields

        protected readonly string _userName;

        #endregion

        #region Properties

        public override Uri BaseAddress
        {
            get { return Utilities.BaseAddress; }
        }

        public override string RsaTokenConfigKey
        {
            get { return RSA_TOKEN_CONFIG_KEY; }
        }

        public override string UserName
        {
            get { return _userName; }
        }

        #endregion

        #region Constructors

        public PermitsDataClientRepository(string userName)
        {
            _userName = userName;
        }

        #endregion
    }

    public interface IPermitsDataClientRepository<TEntity> : IRepository<TEntity> { }
}
