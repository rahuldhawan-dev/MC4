using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using Moq;
using Moq.Language.Flow;
using NHibernate;
using NHibernate.Criterion;

namespace MMSINC.Testing.NHibernate
{
    public class TestRepository<TEntity> : TestRepository<TEntity, IRepository<TEntity>>
        where TEntity : class
    {
        public TestRepository(MockBehavior behavior = MockBehavior.Default) : base(behavior) { }
    }

    public class TestRepository<TEntity, TRepository> : IRepository<TEntity>
        where TRepository : class, IRepository<TEntity>
    {
        #region Private Members

        protected IQueryable<TEntity> _data;
        protected readonly Mock<TRepository> _innerRepoMock;

        #endregion

        #region Properties

        public IQueryable<TEntity> Linq => _data;
        public ICriteria Criteria => _innerRepoMock.Object.Criteria;

        #endregion

        #region Constructors

        public TestRepository(MockBehavior mockBehavior = MockBehavior.Default)
        {
            _innerRepoMock = new Mock<TRepository>(mockBehavior);
            SetData(Enumerable.Empty<TEntity>());
        }

        #endregion

        #region Exposed Methods

        public void SetData(params TEntity[] data)
        {
            SetData((IEnumerable<TEntity>)data);
        }

        public void SetData(IEnumerable<TEntity> data)
        {
            _data = data.AsQueryable();
        }

        public bool Exists(int id)
        {
            return _innerRepoMock.Object.Exists(id);
        }

        public void Delete(TEntity entity)
        {
            _innerRepoMock.Object.Delete(entity);
        }

        public TEntity Save(TEntity entity)
        {
            return _innerRepoMock.Object.Save(entity);
        }

        public void Save(IEnumerable<TEntity> entities)
        {
            _innerRepoMock.Object.Save(entities);
        }

        public virtual TEntity Find(int id)
        {
            return _innerRepoMock.Object.Find(id);
        }

        public TEntity Load(int id)
        {
            return _innerRepoMock.Object.Load(id);
        }

        public int GetIdentifier(TEntity entity)
        {
            return _innerRepoMock.Object.GetIdentifier(entity);
        }

        public TEntity Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Find(object id)
        {
            throw new NotImplementedException();
        }

        public TEntity Update(TEntity valve)
        {
            throw new NotImplementedException();
        }

        public int GetCountForCriterion(ICriterion criterion, IDictionary<string, string> aliases = null,
            ICriterion additionalCriterion = null)
        {
            return _innerRepoMock.Object.GetCountForCriterion(criterion, aliases, additionalCriterion);
        }

        public int GetCountForCriteria(ICriteria criteria)
        {
            return _innerRepoMock.Object.GetCountForCriteria(criteria);
        }

        public int GetCountForSearchSet<T>(ISearchSet<T> search) where T : class
        {
            throw new NotImplementedException();
        }

        public ICriteria Search(ICriterion criterion, IDictionary<string, string> aliases = null,
            ICriterion additionalCriterion = null)
        {
            return _innerRepoMock.Object.Search(criterion, aliases, additionalCriterion);
        }

        public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args)
        {
            return _innerRepoMock.Object.Search(args);
        }

        public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args, ICriteria criteria,
            Action<ISearchMapper> searchMapperCallback = null,
            int? maxResults = null)
        {
            return _innerRepoMock.Object.Search(args, criteria, searchMapperCallback, maxResults);
        }

        public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args, IQueryOver query,
            Action<ISearchMapper> searchMapperCallback = null)
        {
            return _innerRepoMock.Object.Search(args, query, searchMapperCallback);
        }

        public IQueryable<TEntity> GetAll()
        {
            return _data.AsQueryable();
        }

        public IQueryable<TEntity> GetAllSorted()
        {
            return GetAll();
        }

        public IQueryable<TEntity> GetAllSorted(Expression<Func<TEntity, object>> sort)
        {
            return _data.OrderBy(sort).AsQueryable();
        }

        public IEnumerable<TAs> GetAllAs<TAs>(Expression<Func<TEntity, TAs>> expression)
        {
            return _data.Select(expression);
        }

        public IEnumerable<TEntity> BuildPaginatedQuery(int pageIndex, int pageSize, ICriterion filter,
            string sort = null, bool sortAsc = true)
        {
            return _innerRepoMock.Object.BuildPaginatedQuery(pageIndex, pageSize, filter, sort, sortAsc);
        }

        public IEnumerable<TEntity> BuildPaginatedQuery(int pageIndex, int pageSize, ICriteria criteria,
            string sort = null, bool sortAsc = true)
        {
            return _innerRepoMock.Object.BuildPaginatedQuery(pageIndex, pageSize, criteria, sort, sortAsc);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> p)
        {
            return _data.Where(p);
        }

        public bool Any(Expression<Func<TEntity, bool>> p)
        {
            return _data.Any(p);
        }

        public Dictionary<int, TEntity> FindManyByIds(IEnumerable<int> ids)
        {
            return _innerRepoMock.Object.FindManyByIds(ids);
        }

        public void Verify(Expression<Func<TRepository, TEntity>> expression, Func<Times> times = null)
        {
            _innerRepoMock.Verify(expression, times ?? Times.AtLeastOnce);
        }

        public ISetup<TRepository, TEntity> Setup(Expression<Func<TRepository, TEntity>> expression)
        {
            return _innerRepoMock.Setup(expression);
        }

        public ISetup<TRepository> Setup(Expression<Action<TRepository>> expression)
        {
            return _innerRepoMock.Setup(expression);
        }

        public void ClearSession() { }

        void IBaseRepository<TEntity>.Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
