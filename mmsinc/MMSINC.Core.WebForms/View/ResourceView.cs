using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using StructureMap;
using StructureMap.Pipeline;

namespace MMSINC.View
{
    public abstract class ResourceView<TEntity> : MvpUserControl, IResourceView<TEntity>
        where TEntity : class
    {
        #region Private Members

        protected IResourcePresenter<TEntity> _presenter;
        protected IRepository<TEntity> _repository;

        #endregion

        #region Properties

        public virtual ResourceViewMode CurrentMode { get; protected set; }
        public virtual string RedirectURL { get; set; }

        public virtual IRepository<TEntity> Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = DependencyResolver.Current.GetService<IRepository<TEntity>>();
                }

                return _repository;
            }
        }

        public virtual IResourcePresenter<TEntity> Presenter
        {
            get
            {
                if (_presenter == null)
                {
                    _presenter =
                        DependencyResolver.Current.GetService<IContainer>().GetInstance<IResourcePresenter<TEntity>>(
                            new ExplicitArguments(new Dictionary<string, object> {
                                {"view", this},
                                {"repository", Repository}
                            }));
                }

                return _presenter;
            }
        }

        public abstract IListView<TEntity> ListView { get; }
        public abstract IDetailView<TEntity> DetailView { get; }
        public abstract ISearchView<TEntity> SearchView { get; }

        #endregion

        #region Events

        public event EventHandler BackToListClicked;

        #endregion

        #region Event Handlers

        protected virtual void Page_Init(object sender, EventArgs e)
        {
            Presenter.OnViewInit(IUser);
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            // Normally we'd get the presenter and repository from
            // Ioc right here, but in this class we need the presenter
            // in Page_Init, which happens before this does.  So both
            // Presenter and Repository have been moved to lazy-evaluating
            // properties that do that.
            if (ListView != null)
                Presenter.ListView = ListView;
            if (DetailView != null)
                Presenter.DetailView = DetailView;
            if (SearchView != null)
                Presenter.SearchView = SearchView;

            if (!IsMvpPostBack)
                Presenter.OnViewInitialized();

            Presenter.OnViewLoaded();
        }

        protected override void Page_Prerender(object sender, EventArgs e)
        {
            if (Presenter != null)
            {
                Presenter.OnViewPrerender();
            }

            base.Page_Prerender(sender, e);
        }

        protected virtual void btnBackToList_Click(object sender, EventArgs e)
        {
            OnBackToListClicked(e);
        }

        #endregion

        #region Event Passthroughs

        private void OnBackToListClicked(EventArgs e)
        {
            if (BackToListClicked != null)
                BackToListClicked(this, e);
        }

        #endregion

        #region Exposed Methods

        public virtual void SetDetailMode(DetailViewMode newMode)
        {
            DetailView.SetViewMode(newMode);
        }

        public virtual void SetViewMode(ResourceViewMode newMode)
        {
            CurrentMode = newMode;
        }

        public virtual void ToggleList(bool visible)
        {
            ListView.Visible = visible;
        }

        public virtual void ToggleDetail(bool visible)
        {
            DetailView.Visible = visible;
        }

        public virtual void ToggleSearch(bool visible)
        {
            SearchView.Visible = visible;
        }

        public virtual void Redirect(string url)
        {
            IResponse.Redirect(url);
        }

        #endregion
    }
}
