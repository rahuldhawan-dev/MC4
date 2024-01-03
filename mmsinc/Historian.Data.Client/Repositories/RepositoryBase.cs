using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Historian.Data.Client.Mappers;

namespace Historian.Data.Client.Repositories
{
    public abstract class RepositoryBase
    {
        #region Constants

        public const string CONNECTION_STRING_KEY = "Main";

        #endregion

        #region Private Methods

        protected TResult WithConnection<TResult>(Func<SqlConnection, TResult> fn)
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                return fn(connection);
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings[CONNECTION_STRING_KEY].ConnectionString);
        }

        #endregion
    }

    public abstract class RepositoryBase<TEntity> : RepositoryBase
    {
        #region Private Members

        private readonly IMapper<TEntity> _mapper;

        #endregion

        #region Constructors

        protected RepositoryBase(IMapper<TEntity> mapper)
        {
            _mapper = mapper;
        }

        #endregion

        #region Private Methods

        protected IEnumerable<TEntity> Find(string query, params object[] args)
        {
            return WithConnection(c => {
                var command = new SqlCommand(string.Format(query, args), c);

                return _mapper.Map(command.ExecuteReader());
            });
        }

        #endregion
    }

    public abstract class RepositoryBase<TEntity, TMapper> : RepositoryBase<TEntity>
        where TMapper : IMapper<TEntity>
    {
        protected RepositoryBase(TMapper mapper) : base(mapper) { }
    }
}
