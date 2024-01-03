using System;
using System.Linq.Expressions;

namespace MMSINC.Interface
{
    public interface ISearchView<TEntity> : ISearchView
        where TEntity : class
    {
        #region Properties

        ISearchPresenter<TEntity> Presenter { get; }
        Expression<Func<TEntity, bool>> BaseExpression { get; }

        #endregion

        #region Methods

        Expression<Func<TEntity, bool>> GenerateExpression();

        #endregion
    }

    public interface ISearchView : IView
    {
        #region Properties

        bool Visible { get; set; }

        #endregion

        #region Events

        event EventHandler SearchClicked, CancelClicked, UserControlLoaded;

        #endregion
    }
}
