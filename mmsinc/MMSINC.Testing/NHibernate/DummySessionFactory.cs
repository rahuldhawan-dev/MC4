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

namespace MMSINC.Testing.NHibernate
{
    public class DummySessionFactory : ISessionFactory
    {
        #region Private Members

        private readonly ISession _session;

        #endregion

        #region Properties

        public IStatistics Statistics { get; }
        public bool IsClosed { get; }
        public ICollection<string> DefinedFilterNames { get; }

        #endregion

        #region Constructors

        public DummySessionFactory(ISession session)
        {
            _session = session;
        }

        #endregion

        #region Exposed Methods

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ISession OpenSession(DbConnection conn)
        {
            return _session;
        }

        public ISession OpenSession(IInterceptor sessionLocalInterceptor)
        {
            return _session;
        }

        public ISession OpenSession(DbConnection conn, IInterceptor sessionLocalInterceptor)
        {
            return _session;
        }

        public ISession OpenSession()
        {
            return _session;
        }

        public IClassMetadata GetClassMetadata(Type persistentClass)
        {
            return _session.SessionFactory.GetClassMetadata(persistentClass);
        }

        public IClassMetadata GetClassMetadata(string entityName)
        {
            return _session.SessionFactory.GetClassMetadata(entityName);
        }

        public ICollectionMetadata GetCollectionMetadata(string roleName)
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, IClassMetadata> GetAllClassMetadata()
        {
            throw new NotImplementedException();
        }

        public IDictionary<string, ICollectionMetadata> GetAllCollectionMetadata()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Evict(Type persistentClass)
        {
            throw new NotImplementedException();
        }

        public void Evict(Type persistentClass, object id)
        {
            throw new NotImplementedException();
        }

        public void EvictEntity(string entityName)
        {
            throw new NotImplementedException();
        }

        public void EvictEntity(string entityName, object id)
        {
            throw new NotImplementedException();
        }

        public void EvictCollection(string roleName)
        {
            throw new NotImplementedException();
        }

        public void EvictCollection(string roleName, object id)
        {
            throw new NotImplementedException();
        }

        public void EvictQueries()
        {
            throw new NotImplementedException();
        }

        public void EvictQueries(string cacheRegion)
        {
            throw new NotImplementedException();
        }

        public IStatelessSession OpenStatelessSession()
        {
            throw new NotImplementedException();
        }

        public IStatelessSession OpenStatelessSession(DbConnection connection)
        {
            throw new NotImplementedException();
        }

        public FilterDefinition GetFilterDefinition(string filterName)
        {
            throw new NotImplementedException();
        }

        public ISession GetCurrentSession()
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictAsync(Type persistentClass, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictAsync(Type persistentClass, object id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictEntityAsync(string entityName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictEntityAsync(string entityName, object id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictCollectionAsync(string roleName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictCollectionAsync(string roleName, object id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictQueriesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task EvictQueriesAsync(string cacheRegion, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ISessionBuilder WithOptions()
        {
            throw new NotImplementedException();
        }

        public IStatelessSessionBuilder WithStatelessOptions()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
