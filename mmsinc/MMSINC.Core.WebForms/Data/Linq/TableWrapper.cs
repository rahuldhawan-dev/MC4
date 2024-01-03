using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace MMSINC.Data.Linq
{
    /// <summary>
    /// This is not tested because we can't create instances of System.Data.Linq.Table<T> -jd
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class TableWrapper<TEntity> : ITable<TEntity>
        where TEntity : class
    {
        #region Private Members

        private readonly Table<TEntity> _table;

        #endregion

        #region Constructors

        public TableWrapper(Table<TEntity> table)
        {
            _table = table;
        }

        #endregion

        #region Exposed Methods

        public void InsertOnSubmit(TEntity entity)
        {
            _table.InsertOnSubmit(entity);
        }

        public void DeleteOnSubmit(TEntity entity)
        {
            _table.DeleteOnSubmit(entity);
        }

        public List<TEntity> ToList()
        {
            return _table.ToList();
        }

        public int Count()
        {
            return _table.Count();
        }

        public IQueryResult<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return new QueryResultWrapper<TEntity>(_table.Where(expression));
        }

        public void Attach(TEntity entity)
        {
            _table.Attach(entity);
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _table.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _table.GetEnumerator();
        }

        public Expression Expression
        {
            get { return ((IQueryable)_table).Expression; }
        }

        public Type ElementType
        {
            get { return ((IQueryable)_table).ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return ((IQueryable)_table).Provider; }
        }

        #endregion
    }

    public interface ITable<TEntity> : IQueryResult<TEntity>
        where TEntity : class
    {
        #region Methods

        void Attach(TEntity entity);
        void InsertOnSubmit(TEntity entity);
        void DeleteOnSubmit(TEntity entity);
        List<TEntity> ToList();
        int Count();
        IQueryResult<TEntity> Where(Expression<Func<TEntity, bool>> expression);

        #endregion
    }
}
