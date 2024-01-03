using System;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Utilities.Permissions;
using PredicateBuilder = MMSINC.Common.PredicateBuilder;

namespace MMSINC.Presenter
{
    public abstract class ResourcePresenter<TEntity> : IResourcePresenter<TEntity>
        where TEntity : class, new()
    {
        #region Private Members

        protected ISecurityService _securityService;
        private readonly IResourceView _view;
        private IRepository<TEntity> _repository;

        #endregion

        #region Properties

        /// <summary>
        /// IRepository for the entity type that this
        /// presenter has been setup for.
        /// </summary>
        public virtual IRepository<TEntity> Repository
        {
            get { return _repository; }
            set { _repository = value; }
        }

        /// <summary>
        /// IResourceView which this presenter will supervise.
        /// </summary>
        public virtual IResourceView View
        {
            get { return _view; }
        }

        public virtual IDetailView<TEntity> DetailView { get; set; }

        public virtual IListView<TEntity> ListView { get; set; }

        public virtual ISearchView<TEntity> SearchView { get; set; }

        /// <summary>
        /// Security Service, intended to be set/overridden in the
        /// inheriting class.
        /// </summary>
        protected virtual ISecurityService SecurityService
        {
            get { return _securityService; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the EntityResourcePresenter class,
        /// initialized to use view as its IResourceView and repository
        /// as its IRepository.
        /// </summary>
        /// <param name="view">
        /// IResourceView instance to initialize with.
        /// </param>
        /// <param name="repository">
        /// IRepository instance to initialize with.
        /// </param>
        public ResourcePresenter(IResourceView view, IRepository<TEntity> repository)
        {
            _view = view;
            _repository = repository;
        }

        #endregion

        #region Private Methods

        public virtual void SetRepositoryDataKeyFromListViewDataKey()
        {
            if (ListView != null && ListView.SelectedDataKey != null)
            {
                var listViewSelectedDataKey = ListView.SelectedDataKey;
                if (listViewSelectedDataKey != null &&
                    (Repository.SelectedDataKey == null ||
                     listViewSelectedDataKey.ToString() != Repository.SelectedDataKey.ToString()))
                {
                    Repository.SetSelectedDataKey(listViewSelectedDataKey);
                }
            }
        }

        protected virtual void SetListViewData(Expression<Func<TEntity, bool>> filterExpression, string sortExpression)
        {
            if (ListView != null)
            {
                sortExpression = string.IsNullOrEmpty(sortExpression)
                    ? ListView.SqlSortExpression
                    : sortExpression;

                ListView.SetListData(
                    Repository.GetFilteredSortedData(filterExpression,
                        sortExpression));
            }
        }

        protected virtual void SetListViewData(Expression<Func<TEntity, bool>> filterExpression)
        {
            SetListViewData(filterExpression, null);
        }

        protected virtual void SetListViewData()
        {
            SetListViewData(
                (SearchView != null)
                    ? SearchView.GenerateExpression()
                    : PredicateBuilder.True<TEntity>(),
                (ListView != null)
                    ? ListView.SqlSortExpression
                    : null);
        }

        /// <summary>
        /// This method is intended to be overidden by inheriting classes in order to check
        /// the security clearance of the currently logged in user.  The implemented property
        /// 'SecurityService' should aide in this.
        /// </summary>
        protected virtual void CheckUserSecurity()
        {
            // noop goes the dynamite
        }

        #endregion

        #region Event Passthroughs

        public virtual void OnViewInit(IUser iUser)
        {
            if (SecurityService != null)
                SecurityService.Init(iUser);

            CheckUserSecurity();

            View.IPage.LoadComplete = View_LoadComplete;
        }

        public virtual void OnViewInitialized()
        {
            if (SearchView != null)
            {
                View.SetViewMode(ResourceViewMode.Search);
            }
            else if (ListView != null)
            {
                View.SetViewMode(ResourceViewMode.List);
            }
        }

        public virtual void OnViewLoaded()
        {
            View.BackToListClicked += View_BackToListClicked;

            if (ListView != null)
            {
                ListView.UserControlLoaded += ListView_UserControlLoaded;
                ListView.CreateClicked += ListView_CreateClicked;
                ListView.SelectedIndexChanged += ListView_SelectedIndexChanged;
                ListView.Sorting += ListView_Sorting;
                ListView.VisibilityChanged += ListView_VisibilityChanged;
            }

            if (DetailView != null)
            {
                DetailView.UserControlLoaded += DetailView_UserControlLoaded;
                DetailView.DiscardChangesClicked +=
                    DetailView_DiscardChangesClicked;
                DetailView.EditClicked += DetailView_EditClicked;
                DetailView.Updating += DetailView_Updating;
                DetailView.DeleteClicked += DetailView_DeleteClicked;
            }

            if (SearchView != null)
            {
                SearchView.UserControlLoaded += SearchView_UserControlLoaded;
                SearchView.SearchClicked += SearchView_SearchClicked;
            }

            Repository.CurrentEntityChanged += Repository_CurrentEntityChanged;
        }

        public virtual void OnViewPrerender()
        {
            if (View.CurrentMode == ResourceViewMode.Redirect)
            {
                View.Redirect(View.RedirectURL);
            }
        }

        #endregion

        #region Event Handlers

        #region Resource View

        protected virtual void View_LoadComplete(object sender, EventArgs e)
        {
            if (ListView != null && ListView.Visible)
                ListView.DataBind();
        }

        protected virtual void View_BackToListClicked(object sender, EventArgs e)
        {
            View.SetViewMode(ResourceViewMode.List);
        }

        #endregion

        #region List View

        protected virtual void ListView_UserControlLoaded(object sender, EventArgs e)
        {
            ListView.Presenter.Repository = Repository;
        }

        /// <summary>
        /// There's some issue here unresolved with the DetailView. You can't set
        /// the DataItem for one when its being set to insertmode. It only allows
        /// for a brand new item. You also still need the call to ShowEntity here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void ListView_CreateClicked(object sender, EventArgs e)
        {
            if (DetailView != null)
            {
                DetailView.ShowEntity(new TEntity());
                DetailView.SetViewMode(DetailViewMode.Insert);
            }

            if (View != null)
                View.SetViewMode(ResourceViewMode.Detail);
        }

        protected virtual void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetListViewData();
            // don't think this is needed either
            if (View != null)
                View.SetViewMode(ResourceViewMode.Detail);
        }

        protected virtual void ListView_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetListViewData();
        }

        protected virtual void ListView_VisibilityChanged(object sender, VisibilityChangeEventArgs e)
        {
            if (e.NewVisibility)
                SetListViewData();
        }

        #endregion

        #region Detail View

        protected virtual void DetailView_UserControlLoaded(object sender, EventArgs e)
        {
            DetailView.Presenter.Repository = Repository;
        }

        protected virtual void DetailView_DeleteClicked(object sender, EventArgs e)
        {
            SetListViewData();
            View.SetViewMode(ResourceViewMode.List);
        }

        protected virtual void DetailView_DiscardChangesClicked(object sender, EventArgs e)
        {
            if (DetailView.CurrentMode == DetailViewMode.Insert)
            {
                View.SetViewMode(ResourceViewMode.List);
            }
            else if (ListView != null)
            {
                if (DetailView.CurrentMode == DetailViewMode.Edit)
                {
                    SetListViewData();
                }

                SetRepositoryDataKeyFromListViewDataKey();
            }

            if (Repository.CurrentEntity != null)
            {
                DetailView.SetViewMode(DetailViewMode.ReadOnly);
                DetailView.ShowEntity(Repository.CurrentEntity);
            }
        }

        protected virtual void DetailView_EditClicked(object sender, EventArgs e)
        {
            SetListViewData();
            SetRepositoryDataKeyFromListViewDataKey();
        }

        protected virtual void DetailView_Updating(object sender, EventArgs e)
        {
            SetListViewData();
            SetRepositoryDataKeyFromListViewDataKey();
        }

        #endregion

        #region Search View

        protected virtual void SearchView_UserControlLoaded(object sender, EventArgs e)
        {
            SearchView.Presenter.Repository = Repository;
        }

        protected virtual void SearchView_SearchClicked(object sender, EventArgs e)
        {
            View.SetViewMode(ResourceViewMode.List);
        }

        #endregion

        #region Repository

        protected virtual void Repository_CurrentEntityChanged(object sender, EventArgs e)
        {
            if (ListView != null)
                ListView.SelectedIndex = Repository.CurrentIndex;
        }

        #endregion

        #endregion
    }
}
