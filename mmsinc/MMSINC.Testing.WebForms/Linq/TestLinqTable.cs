using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MMSINC.Data.Linq;

namespace MMSINC.Testing.Linq
{
    public abstract class TestLinqTable<TEntity> : System.Data.Linq.ITable<TEntity>
        where TEntity : class
    {
        #region Properties

        public Expression Expression
        {
            get { return Entities.AsQueryable().Expression; }
        }

        public Type ElementType
        {
            get { throw new NotImplementedException("ElementType"); }
        }

        public IQueryProvider Provider
        {
            get { return Entities.AsQueryable().Provider; }
        }

        protected abstract IEnumerable<TEntity> Entities { get; }

        #endregion

        #region Constructors

        public TestLinqTable()
        {
            Initialize();
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            BuildTestData();
        }

        #endregion

        #region Exposed Methods

        public abstract void BuildTestData();

        public IEnumerator<TEntity> GetEnumerator()
        {
            throw new NotImplementedException("GetEnumerator");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Attach(TEntity entity) { }

        public void InsertOnSubmit(TEntity entity)
        {
            throw new NotImplementedException("InsertOnSubmit");
        }

        public void DeleteOnSubmit(TEntity entity)
        {
            throw new NotImplementedException("DeleteOnSubmit");
        }

        public List<TEntity> ToList()
        {
            throw new NotImplementedException("ToList");
        }

        public int Count()
        {
            throw new NotImplementedException("Count");
        }

        public IQueryResult<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return new QueryResultWrapper<TEntity>(Entities.AsQueryable().Where(expression));
        }

        #endregion
    }
}
