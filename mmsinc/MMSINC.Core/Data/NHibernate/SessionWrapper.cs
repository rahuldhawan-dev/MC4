using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Cache;
using NHibernate.Collection;
using NHibernate.Engine;
using NHibernate.Engine.Query.Sql;
using NHibernate.Event;
using NHibernate.Hql;
using NHibernate.Impl;
using NHibernate.Loader.Custom;
using NHibernate.Persister.Entity;
using NHibernate.Stat;
using NHibernate.Transaction;
using NHibernate.Type;

namespace MMSINC.Data.NHibernate
{
    public class SessionWrapper : ISession, ISessionImplementor
    {
        #region Private Members

        private readonly ISession _session;
        private ISessionImplementor _sessionImplementor;

        #endregion

        #region Properties

        public static string DisposalMessage { get; protected set; }

        private ISessionImplementor SessionImplementor => _sessionImplementor ??
                                                          (_sessionImplementor = (ISessionImplementor)_session);

        public string MyDisposalMessage { get; private set; }

        public FlushMode FlushMode
        {
            get => _session.FlushMode;
            set => _session.FlushMode = value;
        }

        DbConnection ISession.Connection => _session.Connection;

        public bool IsOpen => _session.IsOpen;

        public bool IsConnected => _session.IsConnected;

        public ITransaction Transaction => _session.Transaction;

        public bool DefaultReadOnly
        {
            get => _session.DefaultReadOnly;
            set => _session.DefaultReadOnly = value;
        }

        public ISessionStatistics Statistics => _session.Statistics;

        public CacheMode CacheMode
        {
            get => _session.CacheMode;
            set => _session.CacheMode = value;
        }

        public ISessionFactory SessionFactory => _session.SessionFactory;

        #region ISessionImplementor-Specific

        public string FetchProfile
        {
            get => SessionImplementor.FetchProfile;
            set => SessionImplementor.FetchProfile = value;
        }

        public IPersistenceContext PersistenceContext => SessionImplementor.PersistenceContext;

        DbConnection ISessionImplementor.Connection => _session.Connection;

        public bool IsClosed => SessionImplementor.IsClosed;

        public bool TransactionInProgress => SessionImplementor.TransactionInProgress;

        public FutureCriteriaBatch FutureCriteriaBatch => SessionImplementor.FutureCriteriaBatch;

        public FutureQueryBatch FutureQueryBatch => SessionImplementor.FutureQueryBatch;

        public Guid SessionId => SessionImplementor.SessionId;

        public ITransactionContext TransactionContext
        {
            get => SessionImplementor.TransactionContext;
            set => SessionImplementor.TransactionContext = value;
        }

        public CacheKey GenerateCacheKey(object id, IType type, string entityOrRoleName)
        {
            //            return SessionImplementor.GenerateCacheKey(id, type, entityOrRoleName);
            throw new NotImplementedException();
        }

        public long Timestamp => SessionImplementor.Timestamp;

        public ISessionFactoryImplementor Factory => SessionImplementor.Factory;

        public IBatcher Batcher => SessionImplementor.Batcher;

        public IDictionary<string, IFilter> EnabledFilters => SessionImplementor.EnabledFilters;

        public IInterceptor Interceptor => SessionImplementor.Interceptor;

        public EventListeners Listeners => SessionImplementor.Listeners;

        public ConnectionManager ConnectionManager => SessionImplementor.ConnectionManager;

        public bool IsEventSource => SessionImplementor.IsEventSource;

        #endregion

        #endregion

        #region Constructors

        public SessionWrapper(ISession sess)
        {
            if (sess == null)
            {
                throw new InvalidOperationException("Can't wrap a null object");
            }

            _session = sess;
        }

        #endregion

        #region Exposed Methods

        public IQueryable<T> Query<T>(string entityName)
        {
            return _session.Query<T>(entityName);
        }

        public static void ResetMessage()
        {
            DisposalMessage = null;
        }

        public void Dispose()
        {
            MyDisposalMessage =
                DisposalMessage =
                    $"### NHIBERNATE SESSION DISPOSED AT {DateTime.Now}\n{new System.Diagnostics.StackTrace(true)}";
            _session.Dispose();
        }

        public Task InitializeCollectionAsync(IPersistentCollection collection, bool writing, CancellationToken cancellationToken)
        {
            return SessionImplementor.InitializeCollectionAsync(collection, writing, cancellationToken);
        }

        public Task<object> InternalLoadAsync(string entityName, object id, bool eager, bool isNullable, CancellationToken cancellationToken)
        {
            return SessionImplementor.InternalLoadAsync(entityName, id, eager, isNullable, cancellationToken);
        }

        public Task<object> ImmediateLoadAsync(string entityName, object id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList> ListAsync(IQueryExpression queryExpression, QueryParameters parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ListAsync(IQueryExpression queryExpression, QueryParameters queryParameters, IList results,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> ListAsync<T>(IQueryExpression queryExpression, QueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> ListAsync<T>(CriteriaImpl criteria, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ListAsync(CriteriaImpl criteria, IList results, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList> ListAsync(CriteriaImpl criteria, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable> EnumerableAsync(IQueryExpression query, QueryParameters parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> EnumerableAsync<T>(IQueryExpression query, QueryParameters queryParameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList> ListFilterAsync(object collection, string filter, QueryParameters parameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList> ListFilterAsync(object collection, IQueryExpression queryExpression, QueryParameters parameters,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> ListFilterAsync<T>(object collection, string filter, QueryParameters parameters,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable> EnumerableFilterAsync(object collection, string filter, QueryParameters parameters,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> EnumerableFilterAsync<T>(object collection, string filter, QueryParameters parameters,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task BeforeTransactionCompletionAsync(ITransaction tx, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task FlushBeforeTransactionCompletionAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AfterTransactionCompletionAsync(bool successful, ITransaction tx, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList> ListAsync(NativeSQLQuerySpecification spec, QueryParameters queryParameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ListAsync(NativeSQLQuerySpecification spec, QueryParameters queryParameters, IList results,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> ListAsync<T>(NativeSQLQuerySpecification spec, QueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ListCustomQueryAsync(ICustomQuery customQuery, QueryParameters queryParameters, IList results,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> ListCustomQueryAsync<T>(ICustomQuery customQuery, QueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryTranslator[]> GetQueriesAsync(IQueryExpression query, bool scalar, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetEntityUsingInterceptorAsync(EntityKey key, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ISessionImplementor.FlushAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteNativeUpdateAsync(NativeSQLQuerySpecification specification, QueryParameters queryParameters,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> ExecuteUpdateAsync(IQueryExpression query, QueryParameters queryParameters, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IQuery> CreateFilterAsync(object collection, IQueryExpression queryExpression, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        Task ISession.FlushAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsDirtyAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task EvictAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> LoadAsync(Type theType, object id, LockMode lockMode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> LoadAsync(string entityName, object id, LockMode lockMode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> LoadAsync(Type theType, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> LoadAsync<T>(object id, LockMode lockMode, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<T> LoadAsync<T>(object id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> LoadAsync(string entityName, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(object obj, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task ReplicateAsync(object obj, ReplicationMode replicationMode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task ReplicateAsync(string entityName, object obj, ReplicationMode replicationMode,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> SaveAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(object obj, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> SaveAsync(string entityName, object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(string entityName, object obj, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task SaveOrUpdateAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task SaveOrUpdateAsync(string entityName, object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task SaveOrUpdateAsync(string entityName, object obj, object id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(object obj, object id, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string entityName, object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(string entityName, object obj, object id,
            CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> MergeAsync(object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public Task<object> MergeAsync(string entityName, object obj, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public void Flush()
        {
            _session.Flush();
        }

        DbConnection ISession.Disconnect()
        {
            return _session.Disconnect();
        }

        public void Reconnect(DbConnection connection)
        {
            _session.Reconnect(connection);
        }

        DbConnection ISession.Close()
        {
            return _session.Close();
        }

        public void Reconnect()
        {
            _session.Reconnect();
        }

        public void CancelQuery()
        {
            _session.CancelQuery();
        }

        public bool IsDirty()
        {
            return _session.IsDirty();
        }

        public bool IsReadOnly(object entityOrProxy)
        {
            return _session.IsReadOnly(entityOrProxy);
        }

        public bool Contains(object obj)
        {
            return _session.Contains(obj);
        }

        public object GetIdentifier(object obj)
        {
            return _session.GetIdentifier(obj);
        }

        public void SetReadOnly(object entityOrProxy, bool readOnly)
        {
            _session.SetReadOnly(entityOrProxy, readOnly);
        }

        public void Evict(object obj)
        {
            _session.Evict(obj);
        }

        public object Load(Type theType, object id, LockMode lockMode)
        {
            return _session.Load(theType, id, lockMode);
        }

        public object Load(string entityName, object id, LockMode lockMode)
        {
            return _session.Load(entityName, id, lockMode);
        }

        public object Load(Type theType, object id)
        {
            return _session.Load(theType, id);
        }

        public T Load<T>(object id, LockMode lockMode)
        {
            return _session.Load<T>(id, lockMode);
        }

        public T Load<T>(object id)
        {
            return _session.Load<T>(id);
        }

        public object Load(string entityName, object id)
        {
            return _session.Load(entityName, id);
        }

        public void Load(object obj, object id)
        {
            _session.Load(obj, id);
        }

        public void Replicate(object obj, ReplicationMode replicationMode)
        {
            _session.Replicate(obj, replicationMode);
        }

        public object Save(object obj)
        {
            return _session.Save(obj);
        }

        public void Save(object obj, object id)
        {
            _session.Save(obj, id);
        }

        public void Replicate(string entityName, object obj, ReplicationMode replicationMode)
        {
            _session.Replicate(entityName, obj, replicationMode);
        }

        public object Save(string entityName, object obj)
        {
            return _session.Save(entityName, obj);
        }

        public void Save(string entityName, object obj, object id)
        {
            //            _session.Save(entityName, obj, id);
            throw new NotImplementedException();
        }

        public void SaveOrUpdate(string entityName, object obj, object id)
        {
            //            _session.SaveOrUpdate(entityName, obj, id);
            throw new NotImplementedException();
        }

        public void Update(object obj)
        {
            _session.Update(obj);
        }

        public void SaveOrUpdate(string entityName, object obj)
        {
            _session.SaveOrUpdate(entityName, obj);
        }

        public void SaveOrUpdate(object obj)
        {
            _session.SaveOrUpdate(obj);
        }

        public void Update(object obj, object id)
        {
            _session.Update(obj, id);
        }

        public void Update(string entityName, object obj)
        {
            _session.Update(entityName, obj);
        }

        public void Update(string entityName, object obj, object id)
        {
            //            _session.Update(entityName, obj, id);
            throw new NotImplementedException();
        }

        public object Merge(object obj)
        {
            return _session.Merge(obj);
        }

        public object Merge(string entityName, object obj)
        {
            return _session.Merge(entityName, obj);
        }

        public T Merge<T>(T entity) where T : class
        {
            return _session.Merge(entity);
        }

        public T Merge<T>(string entityName, T entity) where T : class
        {
            return _session.Merge(entityName, entity);
        }

        public void Persist(object obj)
        {
            _session.Persist(obj);
        }

        public void Persist(string entityName, object obj)
        {
            _session.Persist(entityName, obj);
        }

        public object SaveOrUpdateCopy(object obj)
        {
            throw new NotImplementedException(
                "Current version of NHibernate does not implement ISession#SaveOrUpdateCopy it seems.");
        }

        public object SaveOrUpdateCopy(object obj, object id)
        {
            throw new NotImplementedException(
                "Current version of NHibernate does not implement ISession#SaveOrUpdateCopy it seems.");
        }

        public void Delete(object obj)
        {
            _session.Delete(obj);
        }

        public void Delete(string entityName, object obj)
        {
            _session.Delete(entityName, obj);
        }

        public int Delete(string query, object value, IType type)
        {
            return _session.Delete(query, value, type);
        }

        public int Delete(string query)
        {
            return _session.Delete(query);
        }

        public int Delete(string query, object[] values, IType[] types)
        {
            return _session.Delete(query, values, types);
        }

        public void Lock(object obj, LockMode lockMode)
        {
            _session.Lock(obj, lockMode);
        }

        public void Lock(string entityName, object obj, LockMode lockMode)
        {
            _session.Lock(entityName, obj, lockMode);
        }

        public void Refresh(object obj)
        {
            _session.Refresh(obj);
        }

        public void Refresh(object obj, LockMode lockMode)
        {
            _session.Refresh(obj, lockMode);
        }

        public LockMode GetCurrentLockMode(object obj)
        {
            return _session.GetCurrentLockMode(obj);
        }

        public ITransaction BeginTransaction()
        {
            return _session.BeginTransaction();
        }

        public ITransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return _session.BeginTransaction(isolationLevel);
        }

        void ISession.JoinTransaction()
        {
            _session.JoinTransaction();
        }

        public void CloseSessionFromSystemTransaction()
        {
            SessionImplementor.CloseSessionFromSystemTransaction();
        }

        public IQuery CreateFilter(object collection, IQueryExpression queryExpression)
        {
            return SessionImplementor.CreateFilter(collection, queryExpression);
        }

        public ICriteria CreateCriteria<T>() where T : class
        {
            return _session.CreateCriteria<T>();
        }

        public ICriteria CreateCriteria<T>(string alias) where T : class
        {
            return _session.CreateCriteria<T>(alias);
        }

        public ICriteria CreateCriteria(Type persistentClass)
        {
            return _session.CreateCriteria(persistentClass);
        }

        public ICriteria CreateCriteria(Type persistentClass, string alias)
        {
            return _session.CreateCriteria(persistentClass, alias);
        }

        public ICriteria CreateCriteria(string entityName)
        {
            return _session.CreateCriteria(entityName);
        }

        public ICriteria CreateCriteria(string entityName, string alias)
        {
            return _session.CreateCriteria(entityName, alias);
        }

        public IQueryOver<T, T> QueryOver<T>() where T : class
        {
            return _session.QueryOver<T>();
        }

        public IQueryOver<T, T> QueryOver<T>(Expression<Func<T>> alias)
            where T : class
        {
            return _session.QueryOver(alias);
        }

        public IQueryOver<T, T> QueryOver<T>(string entityName) where T : class
        {
            return _session.QueryOver<T>(entityName);
        }

        public IQueryOver<T, T> QueryOver<T>(string entityName, Expression<Func<T>> alias) where T : class
        {
            return _session.QueryOver(entityName, alias);
        }

        public IQuery CreateQuery(string queryString)
        {
            return _session.CreateQuery(queryString);
        }

        public IQuery CreateFilter(object collection, string queryString)
        {
            return _session.CreateFilter(collection, queryString);
        }

        public IQuery GetNamedQuery(string queryName)
        {
            return _session.GetNamedQuery(queryName);
        }

        public ISQLQuery CreateSQLQuery(string queryString)
        {
            return _session.CreateSQLQuery(queryString);
        }

        public void Clear()
        {
            _session.Clear();
        }

        public object Get(Type clazz, object id)
        {
            return _session.Get(clazz, id);
        }

        public object Get(Type clazz, object id, LockMode lockMode)
        {
            return _session.Get(clazz, id, lockMode);
        }

        public object Get(string entityName, object id)
        {
            return _session.Get(entityName, id);
        }

        public T Get<T>(object id)
        {
            return _session.Get<T>(id);
        }

        public T Get<T>(object id, LockMode lockMode)
        {
            return _session.Get<T>(id, lockMode);
        }

        public string GetEntityName(object obj)
        {
            return _session.GetEntityName(obj);
        }

        public IFilter EnableFilter(string filterName)
        {
            return _session.EnableFilter(filterName);
        }

        public IFilter GetEnabledFilter(string filterName)
        {
            return _session.GetEnabledFilter(filterName);
        }

        public void DisableFilter(string filterName)
        {
            _session.DisableFilter(filterName);
        }

        public IMultiQuery CreateMultiQuery()
        {
            return _session.CreateMultiQuery();
        }

        public ISession SetBatchSize(int batchSize)
        {
            return _session.SetBatchSize(batchSize);
        }

        public ISessionImplementor GetSessionImplementation()
        {
            return _session.GetSessionImplementation();
        }

        public IMultiCriteria CreateMultiCriteria()
        {
            return _session.CreateMultiCriteria();
        }

        public ISession GetSession(EntityMode entityMode)
        {
            return _session.GetSession(entityMode);
        }

        public IQueryable<T> Query<T>()
        {
            return _session.Query<T>();
        }

        #region ISessionImplementor-Specific

        public void Initialize()
        {
            SessionImplementor.Initialize();
        }

        public void InitializeCollection(IPersistentCollection collection, bool writing)
        {
            SessionImplementor.InitializeCollection(collection, writing);
        }

        public object InternalLoad(string entityName, object id, bool eager, bool isNullable)
        {
            return SessionImplementor.InternalLoad(entityName, id, eager, isNullable);
        }

        public object ImmediateLoad(string entityName, object id)
        {
            return SessionImplementor.ImmediateLoad(entityName, id);
        }

        public IList List(IQueryExpression queryExpression, QueryParameters parameters)
        {
            return SessionImplementor.List(queryExpression, parameters);
        }

        public IQuery CreateQuery(IQueryExpression queryExpression)
        {
            return SessionImplementor.CreateQuery(queryExpression);
        }

        public void List(IQueryExpression queryExpression, QueryParameters queryParameters, IList results)
        {
            //            SessionImplementor.List(queryExpression, queryParameters, results);
            throw new NotImplementedException();
        }

        public IList<T> List<T>(IQueryExpression queryExpression, QueryParameters queryParameters)
        {
            //            return SessionImplementor.List<T>(queryExpression, queryParameters);
            throw new NotImplementedException();
        }

        public IList<T> List<T>(CriteriaImpl criteria)
        {
            return SessionImplementor.List<T>(criteria);
        }

        public void List(CriteriaImpl criteria, IList results)
        {
            SessionImplementor.List(criteria, results);
        }

        public IList List(CriteriaImpl criteria)
        {
            return SessionImplementor.List(criteria);
        }

        public IEnumerable Enumerable(IQueryExpression query, QueryParameters parameters)
        {
            //            return SessionImplementor.Enumerable(query, parameters);
            throw new NotImplementedException();
        }

        public IEnumerable<T> Enumerable<T>(IQueryExpression query, QueryParameters queryParameters)
        {
            //            return SessionImplementor.Enumerable<T>(query, queryParameters);
            throw new NotImplementedException();
        }

        public IList ListFilter(object collection, string filter, QueryParameters parameters)
        {
            return SessionImplementor.ListFilter(collection, filter, parameters);
        }

        public IList ListFilter(object collection, IQueryExpression queryExpression, QueryParameters parameters)
        {
            return SessionImplementor.ListFilter(collection, queryExpression, parameters);
        }

        public IList<T> ListFilter<T>(object collection, string filter, QueryParameters parameters)
        {
            return SessionImplementor.ListFilter<T>(collection, filter, parameters);
        }

        public IEnumerable EnumerableFilter(object collection, string filter, QueryParameters parameters)
        {
            return SessionImplementor.EnumerableFilter(collection, filter,
                parameters);
        }

        public IEnumerable<T> EnumerableFilter<T>(object collection, string filter, QueryParameters parameters)
        {
            return SessionImplementor.EnumerableFilter<T>(collection, filter,
                parameters);
        }

        public IEntityPersister GetEntityPersister(string entityName, object obj)
        {
            return SessionImplementor.GetEntityPersister(entityName, obj);
        }

        public void AfterTransactionBegin(ITransaction tx)
        {
            SessionImplementor.AfterTransactionBegin(tx);
        }

        public void BeforeTransactionCompletion(ITransaction tx)
        {
            SessionImplementor.BeforeTransactionCompletion(tx);
        }

        public void FlushBeforeTransactionCompletion()
        {
            SessionImplementor.FlushBeforeTransactionCompletion();
        }

        public void AfterTransactionCompletion(bool successful, ITransaction tx)
        {
            SessionImplementor.AfterTransactionCompletion(successful, tx);
        }

        public object GetContextEntityIdentifier(object obj)
        {
            return SessionImplementor.GetContextEntityIdentifier(obj);
        }

        public object Instantiate(string entityName, object id)
        {
            return SessionImplementor.Instantiate(entityName, id);
        }

        public IList List(NativeSQLQuerySpecification spec, QueryParameters queryParameters)
        {
            return SessionImplementor.List(spec, queryParameters);
        }

        public void List(NativeSQLQuerySpecification spec, QueryParameters queryParameters, IList results)
        {
            SessionImplementor.List(spec, queryParameters, results);
        }

        public IList<T> List<T>(NativeSQLQuerySpecification spec, QueryParameters queryParameters)
        {
            return SessionImplementor.List<T>(spec, queryParameters);
        }

        public void ListCustomQuery(ICustomQuery customQuery, QueryParameters queryParameters, IList results)
        {
            SessionImplementor.ListCustomQuery(customQuery, queryParameters, results);
        }

        public IList<T> ListCustomQuery<T>(ICustomQuery customQuery, QueryParameters queryParameters)
        {
            return SessionImplementor.ListCustomQuery<T>(customQuery, queryParameters);
        }

        public object GetFilterParameterValue(string filterParameterName)
        {
            return SessionImplementor.GetFilterParameterValue(filterParameterName);
        }

        public IType GetFilterParameterType(string filterParameterName)
        {
            return SessionImplementor.GetFilterParameterType(filterParameterName);
        }

        public IQuery GetNamedSQLQuery(string name)
        {
            return SessionImplementor.GetNamedSQLQuery(name);
        }

        public IQueryTranslator[] GetQueries(IQueryExpression query, bool scalar)
        {
            //            return SessionImplementor.GetQueries(query, scalar);
            throw new NotImplementedException();
        }

        public object GetEntityUsingInterceptor(EntityKey key)
        {
            return SessionImplementor.GetEntityUsingInterceptor(key);
        }

        public string BestGuessEntityName(object entity)
        {
            return SessionImplementor.BestGuessEntityName(entity);
        }

        public string GuessEntityName(object entity)
        {
            return SessionImplementor.GuessEntityName(entity);
        }

        public int ExecuteNativeUpdate(NativeSQLQuerySpecification specification, QueryParameters queryParameters)
        {
            return SessionImplementor.ExecuteNativeUpdate(specification, queryParameters);
        }

        public int ExecuteUpdate(IQueryExpression query, QueryParameters queryParameters)
        {
            //            return SessionImplementor.ExecuteUpdate(query, queryParameters);
            throw new NotImplementedException();
        }

        void ISessionImplementor.JoinTransaction()
        {
            SessionImplementor.JoinTransaction();
        }

        public EntityKey GenerateEntityKey(object id, IEntityPersister persister)
        {
            //            return SessionImplementor.GenerateEntityKey(id, persister);
            throw new NotImplementedException();
        }

        #endregion

        #region Object Overrides

        public override bool Equals(object obj)
        {
            return _session.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _session.GetHashCode();
        }

        public override string ToString()
        {
            return _session.ToString();
        }

        #endregion

        #endregion
    }
}
