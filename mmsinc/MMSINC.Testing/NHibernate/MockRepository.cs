using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;

namespace MMSINC.Testing.NHibernate
{
    public class MockRepository<TEntity> : IRepository<TEntity>
    {
        #region Private Members

        private IList<TEntity> _repositoryImplementation;

        #endregion

        #region Properties

        public IQueryable<TEntity> Linq => _repositoryImplementation.AsQueryable();
        public ICriteria Criteria => throw new NotImplementedException();

        #endregion

        #region Constructors

        public MockRepository(IList<TEntity> entities)
        {
            _repositoryImplementation = entities;
        }

        #endregion

        #region Exposed Methods

        public bool Exists(int id)
        {
            return _repositoryImplementation.Any(e => GetIdentifier(e) == id);
        }

        public void Delete(TEntity entity)
        {
            _repositoryImplementation.Remove(entity);
        }

        public TEntity Save(TEntity entity)
        {
            _repositoryImplementation.Add(entity);
            return entity;
        }

        public void Save(IEnumerable<TEntity> entities)
        {
            _repositoryImplementation.AddRange(entities);
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity Find(int id)
        {
            return _repositoryImplementation.SingleOrDefault(e => GetIdentifier(e) == id);
        }

        public int GetIdentifier(TEntity entity)
        {
            return (int)entity.GetPropertyValueByName("Id");
        }

        public int GetCountForCriterion(ICriterion criterion, IDictionary<string, string> aliases = null,
            ICriterion additionalCriterion = null)
        {
            throw new NotImplementedException();
        }

        public int GetCountForCriteria(ICriteria criteria)
        {
            throw new NotImplementedException();
        }

        public int GetCountForSearchSet<T>(ISearchSet<T> search) where T : class
        {
            throw new NotImplementedException();
        }

        public ICriteria Search(ICriterion criterion, IDictionary<string, string> aliases = null,
            ICriterion additionalCriterion = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args, ICriteria criteria,
            Action<ISearchMapper> searchMapperCallback = null,
            int? maxResults = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TModel> Search<TModel>(ISearchSet<TModel> args, IQueryOver query,
            Action<ISearchMapper> searchMapperCallback = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _repositoryImplementation.AsQueryable();
        }

        public IQueryable<TEntity> GetAllSorted()
        {
            return GetAll();
        }

        public IQueryable<TEntity> GetAllSorted(Expression<Func<TEntity, object>> sort)
        {
            return GetAll().OrderBy(sort);
        }

        public IEnumerable<TAs> GetAllAs<TAs>(Expression<Func<TEntity, TAs>> expression)
        {
            return GetAll().Select(expression);
        }

        public IEnumerable<TEntity> BuildPaginatedQuery(int pageIndex, int pageSize, ICriterion filter,
            string sort = null,
            bool sortAsc = true)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> BuildPaginatedQuery(int pageIndex, int pageSize, ICriteria criteria,
            string sort = null,
            bool sortAsc = true)
        {
            throw new NotImplementedException();
        }

        public TEntity Load(int id)
        {
            return Find(id);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> p)
        {
            return _repositoryImplementation.AsQueryable().Where(p);
        }

        public bool Any(Expression<Func<TEntity, bool>> p)
        {
            return _repositoryImplementation.AsQueryable().Any(p);
        }

        public Dictionary<int, TEntity> FindManyByIds(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public void ClearSession() { }

        #endregion
    }
}
