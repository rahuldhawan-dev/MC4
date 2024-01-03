using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MMSINC.Data.Linq
{
    public class QueryResultWrapper<TEntity> : IQueryResult<TEntity>
        where TEntity : class
    {
        #region Constants

        private static readonly Func<IQueryable<TEntity>, Expression<Func<TEntity, bool>>, IQueryable<TEntity>>
            DEFAULT_QUERY_FACTORY = (qry, expr) => qry.Where(expr);

        #endregion

        #region Private Members

        private IQueryable<TEntity> _iQueryable;

        #endregion

        #region Properties

        public Expression Expression
        {
            get { return _iQueryable.Expression; }
        }

        public Type ElementType
        {
            get { return _iQueryable.ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return _iQueryable.Provider; }
        }

        #endregion

        #region Private Static Members

        private static Func<IQueryable<TEntity>, Expression<Func<TEntity, bool>>, IQueryable<TEntity>>
            _queryFactory = DEFAULT_QUERY_FACTORY;

        #endregion

        #region Constructors

        public QueryResultWrapper(IQueryable<TEntity> iQueryable)
        {
            _iQueryable = iQueryable;
        }

        #endregion

        #region Exposed Methods

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _iQueryable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IQueryResult<TEntity> Where(Expression<Func<TEntity, bool>> expression)
        {
            return new QueryResultWrapper<TEntity>(_queryFactory(_iQueryable, (expression)));
        }

        #endregion

        #region Exposed Static Methods

        public static void SetQueryFactory(
            Func<IQueryable<TEntity>, Expression<Func<TEntity, bool>>, IQueryable<TEntity>> factory)
        {
            _queryFactory = factory;
        }

        public static void ResetQueryFactory()
        {
            SetQueryFactory(DEFAULT_QUERY_FACTORY);
        }

        #endregion
    }

    public interface IQueryResult<TEntity> : IQueryable<TEntity>
        where TEntity : class
    {
        IQueryResult<TEntity> Where(Expression<Func<TEntity, bool>> expression);
    }
}
