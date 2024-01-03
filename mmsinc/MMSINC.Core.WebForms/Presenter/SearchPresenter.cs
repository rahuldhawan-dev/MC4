using System;
using System.Linq.Expressions;
using MMSINC.Data.Linq;
using MMSINC.Interface;

namespace MMSINC.Presenter
{
    public abstract class SearchPresenter<TEntity> : ISearchPresenter<TEntity>
        where TEntity : class
    {
        #region Private Members

        private readonly ISearchView<TEntity> _view;

        #endregion

        #region Properties

        public virtual IRepository<TEntity> Repository { get; set; }

        public virtual ISearchView<TEntity> View
        {
            get { return _view; }
        }

        #endregion

        #region Constructors

        public SearchPresenter(ISearchView<TEntity> view)
        {
            _view = view;
        }

        #endregion

        #region Event Handlers

        protected virtual void View_SearchClicked(object sender, EventArgs e) { }

        protected virtual void View_CancelClicked(object sender, EventArgs e) { }

        #endregion

        #region Exposed Methods

        public virtual void OnViewInitialized() { }

        public virtual void OnViewLoaded()
        {
            View.SearchClicked += View_SearchClicked;
            View.CancelClicked += View_CancelClicked;
        }

        public virtual Expression<Func<TEntity, bool>> GenerateExpression()
        {
            return View.GenerateExpression();
        }

        #endregion
    }
}
