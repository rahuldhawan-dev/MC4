using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Engine;
using NHibernate.Stat;
using NHibernate.Type;

namespace MMSINC.Data.NHibernate
{
    public class MemoizedSessionWrapper : ISession
    {
        #region Private Members

        private readonly ISession _session;
        private readonly Dictionary<Type, Func<object, object>> _memoizedLookupDictionary;

        #endregion

        #region Properties

        [ExcludeFromCodeCoverage]
        public FlushMode FlushMode
        {
            get => _session.FlushMode;
            set => _session.FlushMode = value;
        }

        [ExcludeFromCodeCoverage]
        public CacheMode CacheMode
        {
            get => _session.CacheMode;
            set => _session.CacheMode = value;
        }

        [ExcludeFromCodeCoverage]
        public ISessionFactory SessionFactory => _session.SessionFactory;

        [ExcludeFromCodeCoverage]
        DbConnection ISession.Connection => _session.Connection;

        [ExcludeFromCodeCoverage]
        public bool IsOpen => _session.IsOpen;

        [ExcludeFromCodeCoverage]
        public bool IsConnected => _session.IsConnected;

        [ExcludeFromCodeCoverage]
        public bool DefaultReadOnly
        {
            get => _session.DefaultReadOnly;
            set => _session.DefaultReadOnly = value;
        }

        [ExcludeFromCodeCoverage]
        public ITransaction Transaction => _session.Transaction;

        [ExcludeFromCodeCoverage]
        public ISessionStatistics Statistics => _session.Statistics;

        #endregion

        #region Constructors

        public MemoizedSessionWrapper(ISession session)
        {
            _session = session;
            _memoizedLookupDictionary = new Dictionary<Type, Func<object, object>>();
        }

        #endregion

        #region Private Methods

        protected T MemoizedGet<T>(object id)
        {
            Func<object, object> lookupFn;
            var refType = typeof(T);
            var refTypeName = refType.Name;

            if (_memoizedLookupDictionary.ContainsKey(refType))
            {
                lookupFn = _memoizedLookupDictionary[refType];
            }
            else
            {
                _memoizedLookupDictionary.Add(refType,
                    lookupFn = FuncExtensions.Memoize<object, object>(i => _session.Get<T>(i)));
            }

            return (T)lookupFn(id);
        }

        #endregion

        #region Exposed Methods

        public IQueryable<T> Query<T>(string entityName)
        {
            return _session.Query<T>(entityName);
        }

        [ExcludeFromCodeCoverage]
        public void Dispose()
        {
            _session.Dispose();
        }

        public Task FlushAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.FlushAsync(cancellationToken);
        }

        public Task<bool> IsDirtyAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.IsDirtyAsync(cancellationToken);
        }

        public Task EvictAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.EvictAsync(cancellationToken);
        }

        public Task<object> LoadAsync(Type theType, object id, LockMode lockMode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.LoadAsync(theType, id, lockMode, cancellationToken);
        }

        public Task<object> LoadAsync(string entityName, object id, LockMode lockMode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.LoadAsync(entityName, id, lockMode, cancellationToken);
        }

        public Task<object> LoadAsync(Type theType, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.LoadAsync(theType, id, cancellationToken);
        }

        public Task<T> LoadAsync<T>(object id, LockMode lockMode, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.LoadAsync<T>(id, lockMode, cancellationToken);
        }

        public Task<T> LoadAsync<T>(object id, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.LoadAsync<T>(id, cancellationToken);
        }

        public Task<object> LoadAsync(string entityName, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.LoadAsync(entityName, id, cancellationToken);
        }

        public Task LoadAsync(object obj, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.LoadAsync(obj, id, cancellationToken);
        }

        public Task ReplicateAsync(object obj, ReplicationMode replicationMode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.ReplicateAsync(obj, replicationMode, cancellationToken);
        }

        public Task ReplicateAsync(string entityName, object obj, ReplicationMode replicationMode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.ReplicateAsync(entityName, obj, replicationMode, cancellationToken);
        }

        public Task<object> SaveAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.SaveAsync(obj, cancellationToken);
        }

        public Task SaveAsync(object obj, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.SaveAsync(obj, id, cancellationToken);
        }

        public Task<object> SaveAsync(string entityName, object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.SaveAsync(entityName, obj, cancellationToken);
        }

        public Task SaveAsync(string entityName, object obj, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.SaveAsync(entityName, obj, id, cancellationToken);
        }

        public Task SaveOrUpdateAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.SaveOrUpdateAsync(obj, cancellationToken);
        }

        public Task SaveOrUpdateAsync(string entityName, object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.SaveOrUpdateAsync(entityName, obj, cancellationToken);
        }

        public Task SaveOrUpdateAsync(string entityName, object obj, object id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.SaveOrUpdateAsync(entityName, obj, id, cancellationToken);
        }

        public Task UpdateAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.UpdateAsync(obj, cancellationToken);
        }

        public Task UpdateAsync(object obj, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.UpdateAsync(obj, id, cancellationToken);
        }

        public Task UpdateAsync(string entityName, object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.UpdateAsync(entityName, obj, cancellationToken);
        }

        public Task UpdateAsync(string entityName, object obj, object id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.UpdateAsync(entityName, obj, id, cancellationToken);
        }

        public Task<object> MergeAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.MergeAsync(obj, cancellationToken);
        }

        public Task<object> MergeAsync(string entityName, object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            return _session.MergeAsync(entityName, obj, cancellationToken);
        }

        public Task<T> MergeAsync<T>(T entity, CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            throw new NotImplementedException();
        }

        public Task<T> MergeAsync<T>(string entityName, T entity, CancellationToken cancellationToken = new CancellationToken()) where T : class
        {
            throw new NotImplementedException();
        }

        public Task PersistAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task PersistAsync(string entityName, object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string entityName, object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string query, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string query, object value, IType type, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(string query, object[] values, IType[] types,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task LockAsync(object obj, LockMode lockMode, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task LockAsync(string entityName, object obj, LockMode lockMode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task RefreshAsync(object obj, LockMode lockMode, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<IQuery> CreateFilterAsync(object collection, string queryString,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAsync(Type clazz, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAsync(Type clazz, object id, LockMode lockMode, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAsync(string entityName, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(object id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync<T>(object id, LockMode lockMode, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEntityNameAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public ISharedSessionBuilder SessionWithOptions()
        {
            return _session.SessionWithOptions();
        }

        [ExcludeFromCodeCoverage]
        public void Flush()
        {
            _session.Flush();
        }

        [ExcludeFromCodeCoverage]
        DbConnection ISession.Disconnect()
        {
            return _session.Disconnect();
        }

        [ExcludeFromCodeCoverage]
        public void Reconnect()
        {
            _session.Reconnect();
        }

        [ExcludeFromCodeCoverage]
        public void Reconnect(DbConnection connection)
        {
            _session.Reconnect(connection);
        }

        [ExcludeFromCodeCoverage]
        DbConnection ISession.Close()
        {
            return _session.Close();
        }

        [ExcludeFromCodeCoverage]
        public void CancelQuery()
        {
            _session.CancelQuery();
        }

        [ExcludeFromCodeCoverage]
        public bool IsDirty()
        {
            return _session.IsDirty();
        }

        [ExcludeFromCodeCoverage]
        public bool IsReadOnly(object entityOrProxy)
        {
            return _session.IsReadOnly(entityOrProxy);
        }

        [ExcludeFromCodeCoverage]
        public void SetReadOnly(object entityOrProxy, bool readOnly)
        {
            _session.SetReadOnly(entityOrProxy, readOnly);
        }

        [ExcludeFromCodeCoverage]
        public object GetIdentifier(object obj)
        {
            return _session.GetIdentifier(obj);
        }

        [ExcludeFromCodeCoverage]
        public bool Contains(object obj)
        {
            return _session.Contains(obj);
        }

        [ExcludeFromCodeCoverage]
        public void Evict(object obj)
        {
            _session.Evict(obj);
        }

        [ExcludeFromCodeCoverage]
        public object Load(Type theType, object id, LockMode lockMode)
        {
            return _session.Load(theType, id, lockMode);
        }

        [ExcludeFromCodeCoverage]
        public object Load(string entityName, object id, LockMode lockMode)
        {
            return _session.Load(entityName, id, lockMode);
        }

        [ExcludeFromCodeCoverage]
        public object Load(Type theType, object id)
        {
            return _session.Load(theType, id);
        }

        [ExcludeFromCodeCoverage]
        public T Load<T>(object id, LockMode lockMode)
        {
            return _session.Load<T>(id, lockMode);
        }

        [ExcludeFromCodeCoverage]
        public T Load<T>(object id)
        {
            return _session.Load<T>(id);
        }

        [ExcludeFromCodeCoverage]
        public object Load(string entityName, object id)
        {
            return _session.Load(entityName, id);
        }

        [ExcludeFromCodeCoverage]
        public void Load(object obj, object id)
        {
            _session.Load(obj, id);
        }

        [ExcludeFromCodeCoverage]
        public void Replicate(object obj, ReplicationMode replicationMode)
        {
            _session.Replicate(obj, replicationMode);
        }

        [ExcludeFromCodeCoverage]
        public void Replicate(string entityName, object obj, ReplicationMode replicationMode)
        {
            _session.Replicate(entityName, obj, replicationMode);
        }

        [ExcludeFromCodeCoverage]
        public object Save(object obj)
        {
            return _session.Save(obj);
        }

        [ExcludeFromCodeCoverage]
        public void Save(object obj, object id)
        {
            _session.Save(obj, id);
        }

        [ExcludeFromCodeCoverage]
        public object Save(string entityName, object obj)
        {
            return _session.Save(entityName, obj);
        }

        [ExcludeFromCodeCoverage]
        public void Save(string entityName, object obj, object id)
        {
            _session.Save(entityName, obj, id);
        }

        [ExcludeFromCodeCoverage]
        public void SaveOrUpdate(object obj)
        {
            _session.SaveOrUpdate(obj);
        }

        [ExcludeFromCodeCoverage]
        public void SaveOrUpdate(string entityName, object obj)
        {
            _session.SaveOrUpdate(entityName, obj);
        }

        [ExcludeFromCodeCoverage]
        public void SaveOrUpdate(string entityName, object obj, object id)
        {
            _session.SaveOrUpdate(entityName, obj, id);
        }

        [ExcludeFromCodeCoverage]
        public void Update(object obj)
        {
            _session.Update(obj);
        }

        [ExcludeFromCodeCoverage]
        public void Update(object obj, object id)
        {
            _session.Update(obj, id);
        }

        [ExcludeFromCodeCoverage]
        public void Update(string entityName, object obj)
        {
            _session.Update(entityName, obj);
        }

        [ExcludeFromCodeCoverage]
        public void Update(string entityName, object obj, object id)
        {
            _session.Update(entityName, obj, id);
        }

        [ExcludeFromCodeCoverage]
        public object Merge(object obj)
        {
            return _session.Merge(obj);
        }

        [ExcludeFromCodeCoverage]
        public object Merge(string entityName, object obj)
        {
            return _session.Merge(entityName, obj);
        }

        [ExcludeFromCodeCoverage]
        public T Merge<T>(T entity) where T : class
        {
            return _session.Merge(entity);
        }

        [ExcludeFromCodeCoverage]
        public T Merge<T>(string entityName, T entity) where T : class
        {
            return _session.Merge(entityName, entity);
        }

        [ExcludeFromCodeCoverage]
        public void Persist(object obj)
        {
            _session.Persist(obj);
        }

        [ExcludeFromCodeCoverage]
        public void Persist(string entityName, object obj)
        {
            _session.Persist(entityName, obj);
        }

        [ExcludeFromCodeCoverage]
        public void Delete(object obj)
        {
            _session.Delete(obj);
        }

        [ExcludeFromCodeCoverage]
        public void Delete(string entityName, object obj)
        {
            _session.Delete(entityName, obj);
        }

        [ExcludeFromCodeCoverage]
        public int Delete(string query)
        {
            return _session.Delete(query);
        }

        [ExcludeFromCodeCoverage]
        public int Delete(string query, object value, IType type)
        {
            return _session.Delete(query, value, type);
        }

        [ExcludeFromCodeCoverage]
        public int Delete(string query, object[] values, IType[] types)
        {
            return _session.Delete(query, values, types);
        }

        [ExcludeFromCodeCoverage]
        public void Lock(object obj, LockMode lockMode)
        {
            _session.Lock(obj, lockMode);
        }

        [ExcludeFromCodeCoverage]
        public void Lock(string entityName, object obj, LockMode lockMode)
        {
            _session.Lock(entityName, obj, lockMode);
        }

        [ExcludeFromCodeCoverage]
        public void Refresh(object obj)
        {
            _session.Refresh(obj);
        }

        [ExcludeFromCodeCoverage]
        public void Refresh(object obj, LockMode lockMode)
        {
            _session.Refresh(obj, lockMode);
        }

        [ExcludeFromCodeCoverage]
        public LockMode GetCurrentLockMode(object obj)
        {
            return _session.GetCurrentLockMode(obj);
        }

        [ExcludeFromCodeCoverage]
        public ITransaction BeginTransaction()
        {
            return _session.BeginTransaction();
        }

        [ExcludeFromCodeCoverage]
        public ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return _session.BeginTransaction(isolationLevel);
        }

        public void JoinTransaction()
        {
            _session.JoinTransaction();
        }

        [ExcludeFromCodeCoverage]
        public ICriteria CreateCriteria<T>() where T : class
        {
            return _session.CreateCriteria<T>();
        }

        [ExcludeFromCodeCoverage]
        public ICriteria CreateCriteria<T>(string alias) where T : class
        {
            return _session.CreateCriteria<T>(alias);
        }

        [ExcludeFromCodeCoverage]
        public ICriteria CreateCriteria(Type persistentClass)
        {
            return _session.CreateCriteria(persistentClass);
        }

        [ExcludeFromCodeCoverage]
        public ICriteria CreateCriteria(Type persistentClass, string alias)
        {
            return _session.CreateCriteria(persistentClass, alias);
        }

        [ExcludeFromCodeCoverage]
        public ICriteria CreateCriteria(string entityName)
        {
            return _session.CreateCriteria(entityName);
        }

        [ExcludeFromCodeCoverage]
        public ICriteria CreateCriteria(string entityName, string alias)
        {
            return _session.CreateCriteria(entityName, alias);
        }

        [ExcludeFromCodeCoverage]
        public IQueryOver<T, T> QueryOver<T>() where T : class
        {
            return _session.QueryOver<T>();
        }

        [ExcludeFromCodeCoverage]
        public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias) where T : class
        {
            return _session.QueryOver(alias);
        }

        [ExcludeFromCodeCoverage]
        public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
        {
            return _session.QueryOver<T>(entityName);
        }

        [ExcludeFromCodeCoverage]
        public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
        {
            return _session.QueryOver(entityName, alias);
        }

        [ExcludeFromCodeCoverage]
        public IQuery CreateQuery(string queryString)
        {
            return _session.CreateQuery(queryString);
        }

        [ExcludeFromCodeCoverage]
        public IQuery CreateFilter(object collection, string queryString)
        {
            return _session.CreateFilter(collection, queryString);
        }

        [ExcludeFromCodeCoverage]
        public IQuery GetNamedQuery(string queryName)
        {
            return _session.GetNamedQuery(queryName);
        }

        [ExcludeFromCodeCoverage]
        public ISQLQuery CreateSQLQuery(string queryString)
        {
            return _session.CreateSQLQuery(queryString);
        }

        [ExcludeFromCodeCoverage]
        public void Clear()
        {
            _session.Clear();
        }

        [ExcludeFromCodeCoverage]
        public object Get(Type clazz, object id)
        {
            return _session.Get(clazz, id);
        }

        [ExcludeFromCodeCoverage]
        public object Get(Type clazz, object id, LockMode lockMode)
        {
            return _session.Get(clazz, id, lockMode);
        }

        [ExcludeFromCodeCoverage]
        public object Get(string entityName, object id)
        {
            return _session.Get(entityName, id);
        }

        public T Get<T>(object id)
        {
            return MemoizedGet<T>(id);
        }

        [ExcludeFromCodeCoverage]
        public T Get<T>(object id, LockMode lockMode)
        {
            return _session.Get<T>(id, lockMode);
        }

        [ExcludeFromCodeCoverage]
        public string GetEntityName(object obj)
        {
            return _session.GetEntityName(obj);
        }

        [ExcludeFromCodeCoverage]
        public IFilter EnableFilter(string filterName)
        {
            return _session.EnableFilter(filterName);
        }

        [ExcludeFromCodeCoverage]
        public IFilter GetEnabledFilter(string filterName)
        {
            return _session.GetEnabledFilter(filterName);
        }

        [ExcludeFromCodeCoverage]
        public void DisableFilter(string filterName)
        {
            _session.DisableFilter(filterName);
        }

        [ExcludeFromCodeCoverage]
        public IMultiQuery CreateMultiQuery()
        {
            return _session.CreateMultiQuery();
        }

        [ExcludeFromCodeCoverage]
        public ISession SetBatchSize(int batchSize)
        {
            return _session.SetBatchSize(batchSize);
        }

        [ExcludeFromCodeCoverage]
        public ISessionImplementor GetSessionImplementation()
        {
            return _session.GetSessionImplementation();
        }

        [ExcludeFromCodeCoverage]
        public IMultiCriteria CreateMultiCriteria()
        {
            return _session.CreateMultiCriteria();
        }

        [ExcludeFromCodeCoverage]
        public ISession GetSession(EntityMode entityMode)
        {
            return _session.GetSession(entityMode);
        }

        public IQueryable<T> Query<T>()
        {
            return _session.Query<T>();
        }

        #endregion
    }
}
