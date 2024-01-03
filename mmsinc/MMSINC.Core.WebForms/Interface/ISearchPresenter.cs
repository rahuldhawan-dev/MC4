using System;
using System.Linq.Expressions;

namespace MMSINC.Interface
{
    public interface ISearchPresenter<TEntity> : IPresenter<TEntity>
        where TEntity : class
    {
        #region Methods

        Expression<Func<TEntity, bool>> GenerateExpression();

        #endregion
    }
}
