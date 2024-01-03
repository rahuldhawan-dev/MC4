using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Metadata;
using NHibernate.Stat;

namespace MMSINC.Data.NHibernate
{
    public class SessionFactoryWrapper : ISessionFactory
    {
        #region Private Members

        private readonly ISessionFactory _factory;

        #endregion

        #region Properties

        public IStatistics Statistics
        {
            get { return _factory.Statistics; }
        }

        public bool IsClosed
        {
            get { return _factory.IsClosed; }
        }

        public ICollection<string> DefinedFilterNames
        {
            get { return _factory.DefinedFilterNames; }
        }

        public static string DisposalMessage { get; protected set; }

        #endregion

        #region Constructors

        public SessionFactoryWrapper(ISessionFactory fact)
        {
            _factory = fact;
        }

        #endregion

        #region Exposed Methods

        public static void ResetMessage()
        {
            DisposalMessage = null;
        }

        public void Dispose()
        {
            DisposalMessage =
                String.Format("### NHIBERNATE SESSION FACTORY DISPOSED AT {0}\n{1}",
                    DateTime.Now, new System.Diagnostics.StackTrace(true));
            _factory.Dispose();
        }

        public ISession OpenSession(DbConnection conn)
        {
            return _factory.OpenSession(conn);
        }

        public ISession OpenSession(IInterceptor sessionLocalInterceptor)
        {
            return _factory.OpenSession(sessionLocalInterceptor);
        }

        public ISession OpenSession(DbConnection conn, IInterceptor sessionLocalInterceptor)
        {
            return _factory.OpenSession(conn, sessionLocalInterceptor);
        }

        public ISession OpenSession()
        {
            return _factory.OpenSession();
        }

        public IClassMetadata GetClassMetadata(Type persistentClass)
        {
            return _factory.GetClassMetadata(persistentClass);
        }

        public IClassMetadata GetClassMetadata(string entityName)
        {
            return _factory.GetClassMetadata(entityName);
        }

        public ICollectionMetadata GetCollectionMetadata(string roleName)
        {
            return _factory.GetCollectionMetadata(roleName);
        }

        public IDictionary<string, IClassMetadata> GetAllClassMetadata()
        {
            return _factory.GetAllClassMetadata();
        }

        public IDictionary<string, ICollectionMetadata> GetAllCollectionMetadata()
        {
            return _factory.GetAllCollectionMetadata();
        }

        public void Close()
        {
            _factory.Close();
        }

        public void Evict(Type persistentClass)
        {
            _factory.Evict(persistentClass);
        }

        public void Evict(Type persistentClass, object id)
        {
            _factory.Evict(persistentClass, id);
        }

        public void EvictEntity(string entityName)
        {
            _factory.EvictEntity(entityName);
        }

        public void EvictEntity(string entityName, object id)
        {
            _factory.EvictEntity(entityName, id);
        }

        public void EvictCollection(string roleName)
        {
            _factory.EvictCollection(roleName);
        }

        public void EvictCollection(string roleName, object id)
        {
            _factory.EvictCollection(roleName, id);
        }

        public void EvictQueries()
        {
            _factory.EvictQueries();
        }

        public void EvictQueries(string cacheRegion)
        {
            _factory.EvictQueries(cacheRegion);
        }

        public IStatelessSession OpenStatelessSession()
        {
            return _factory.OpenStatelessSession();
        }

        public IStatelessSession OpenStatelessSession(DbConnection connection)
        {
            return _factory.OpenStatelessSession(connection);
        }

        public FilterDefinition GetFilterDefinition(string filterName)
        {
            return _factory.GetFilterDefinition(filterName);
        }

        public ISession GetCurrentSession()
        {
            return _factory.GetCurrentSession();
        }

        #region Object Overrides

        public override bool Equals(object obj)
        {
            return _factory.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _factory.GetHashCode();
        }

        public override string ToString()
        {
            return _factory.ToString();
        }

        public Task CloseAsync(CancellationToken cancellationToken = default)
        {
            return _factory.CloseAsync(cancellationToken);
        }

        public Task EvictAsync(Type persistentClass, CancellationToken cancellationToken = default)
        {
            return _factory.EvictAsync(persistentClass, cancellationToken);
        }

        public Task EvictAsync(Type persistentClass, object id, CancellationToken cancellationToken = default)
        {
            return _factory.EvictAsync(persistentClass, id, cancellationToken);
        }

        public Task EvictEntityAsync(string entityName, CancellationToken cancellationToken = default)
        {
            return _factory.EvictEntityAsync(entityName, cancellationToken);
        }

        public Task EvictEntityAsync(string entityName, object id, CancellationToken cancellationToken = default)
        {
            return _factory.EvictEntityAsync(entityName, id, cancellationToken);
        }

        public Task EvictCollectionAsync(string roleName, CancellationToken cancellationToken = default)
        {
            return _factory.EvictCollectionAsync(roleName, cancellationToken);
        }

        public Task EvictCollectionAsync(string roleName, object id, CancellationToken cancellationToken = default)
        {
            return _factory.EvictCollectionAsync(roleName, id, cancellationToken);
        }

        public Task EvictQueriesAsync(CancellationToken cancellationToken = default)
        {
            return _factory.EvictQueriesAsync(cancellationToken);
        }

        public Task EvictQueriesAsync(string cacheRegion, CancellationToken cancellationToken = default)
        {
            return _factory.EvictQueriesAsync(cacheRegion, cancellationToken);
        }

        public ISessionBuilder WithOptions()
        {
            return _factory.WithOptions();
        }

        public IStatelessSessionBuilder WithStatelessOptions()
        {
            return _factory.WithStatelessOptions();
        }

        #endregion

        #endregion
    }
}
