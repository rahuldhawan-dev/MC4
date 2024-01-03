using System;
using System.Linq;
using System.Linq.Expressions;

namespace MMSINC.Common
{
    public static class PredicateBuilder
    {
        #region Exposed Static Methods

        public static Expression<Func<TEntity, bool>> True<TEntity>()
        {
            return o => true;
        }

        public static Expression<Func<TEntity, bool>> False<TEntity>()
        {
            return o => false;
        }

        public static Expression<Func<TEntity, bool>> Or<TEntity>(this Expression<Func<TEntity, bool>> expr1,
            Expression<Func<TEntity, bool>> expr2)
        {
            var prms = expr1.Parameters;
            var invokedExpr = Expression.Invoke(expr2, prms.Cast<Expression>());
            return Expression.Lambda<Func<TEntity, bool>>
                (Expression.Or(expr1.Body, invokedExpr), prms);
        }

        public static Expression<Func<TEntity, bool>> And<TEntity>(this Expression<Func<TEntity, bool>> expr1,
            Expression<Func<TEntity, bool>> expr2)
        {
            var prms = expr1.Parameters;
            var invokedExpr = Expression.Invoke(expr2, prms.Cast<Expression>());
            return Expression.Lambda<Func<TEntity, bool>>
                (Expression.And(expr1.Body, invokedExpr), prms);
        }

        #endregion
    }
}
