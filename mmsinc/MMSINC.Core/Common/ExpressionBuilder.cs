using System;
using System.Linq.Expressions;
using MMSINC.DesignPatterns;

namespace MMSINC.Common
{
    public class ExpressionBuilder<TTarget> : Builder<Expression<Func<TTarget, bool>>>
    {
        #region Private Members

        private Expression<Func<TTarget, bool>> _baseExpression;

        #endregion

        #region Constructors

        public ExpressionBuilder(Expression<Func<TTarget, bool>> baseExpression)
        {
            _baseExpression = baseExpression;
        }

        #endregion

        #region Exposed Methods

        public override Expression<Func<TTarget, bool>> Build()
        {
            return _baseExpression;
        }

        public ExpressionBuilder<TTarget> And(Expression<Func<TTarget, bool>> predicate)
        {
            _baseExpression = _baseExpression.And(predicate);
            return this;
        }

        public ExpressionBuilder<TTarget> Or(Expression<Func<TTarget, bool>> predicate)
        {
            _baseExpression = _baseExpression.Or(predicate);
            return this;
        }

        #endregion
    }
}
