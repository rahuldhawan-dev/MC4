using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using MMSINC.Common;
using MMSINC.Interface;
using StructureMap;
using StructureMap.Pipeline;
using PredicateBuilder = MMSINC.Common.PredicateBuilder;

namespace MMSINC.View
{
    public abstract class SearchView : MvpUserControl, ISearchView
    {
        #region Event Handlers

        protected virtual void btnSearch_Click(object sender, EventArgs e)
        {
            OnSearchClicked(e);
        }

        protected virtual void btnCancel_Click(object sender, EventArgs e)
        {
            OnCancelClicked(e);
        }

        #endregion

        #region Events

        public event EventHandler SearchClicked,
                                  UserControlLoaded,
                                  CancelClicked;

        #endregion

        #region Event Passthroughs

        protected virtual void OnUserControlLoaded(EventArgs e)
        {
            if (UserControlLoaded != null)
                UserControlLoaded(this, e);
        }

        protected virtual void OnSearchClicked(EventArgs e)
        {
            if (SearchClicked != null)
                SearchClicked(this, e);
        }

        protected virtual void OnCancelClicked(EventArgs e)
        {
            if (CancelClicked != null)
                CancelClicked(this, e);
        }

        #endregion
    }

    public abstract class SearchView<TEntity> : SearchView, ISearchView<TEntity>
        where TEntity : class
    {
        #region Private Members

        private ISearchPresenter<TEntity> _presenter;
        protected Expression<Func<TEntity, bool>> _baseExpression;

        #endregion

        #region Properties

        public ISearchPresenter<TEntity> Presenter => _presenter;

        public virtual Expression<Func<TEntity, bool>> BaseExpression => _baseExpression ??
                                                                         (_baseExpression =
                                                                             PredicateBuilder.True<TEntity>());

        #endregion

        #region Event Handlers

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            _presenter =
                DependencyResolver.Current.GetService<IContainer>().GetInstance<ISearchPresenter<TEntity>>(
                    new ExplicitArguments(new Dictionary<string, object> {{"view", this}}));

            if (!IsMvpPostBack)
                Presenter.OnViewInitialized();

            OnUserControlLoaded(e);

            Presenter.OnViewLoaded();
        }

        #endregion

        #region Abstract Methods

        public abstract Expression<Func<TEntity, bool>> GenerateExpression();

        #endregion
    }
}
