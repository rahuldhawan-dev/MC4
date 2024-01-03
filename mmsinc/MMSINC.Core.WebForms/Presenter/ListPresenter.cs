using System;
using System.Web.UI.WebControls;
using MMSINC.Data.Linq;
using MMSINC.Interface;

namespace MMSINC.Presenter
{
    public abstract class ListPresenter<TEntity> : IListPresenter<TEntity>
        where TEntity : class
    {
        #region Private Members

        private readonly IListView<TEntity> _view;

        #endregion

        #region Properties

        /// <summary>
        /// IRepository for the entity type that this
        /// presenter has been setup for.
        /// </summary>
        public virtual IRepository<TEntity> Repository { get; set; }

        /// <summary>
        /// IListView which this presenter will supervise.
        /// </summary>
        public virtual IListView<TEntity> View
        {
            get { return _view; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the EntityListPresenter
        /// class, initialized to use view as its IListView.
        /// </summary>
        /// <param name="view">
        /// IListView instance to initialize with.
        /// </param>
        public ListPresenter(IListView<TEntity> view)
        {
            _view = view;
        }

        #endregion

        #region Event Passthroughs

        public virtual void OnViewInit()
        {
            //View.IPage.LoadComplete = View_LoadComplete;
        }

        public virtual void OnViewInitialized() { }

        public virtual void OnViewLoaded()
        {
            if (View != null)
            {
                View.SelectedIndexChanged += View_SelectedIndexChanged;
                View.DataSourceCreating += View_CreatingDataSource;
                View.CreateClicked += View_CreateClicked;
                View.SetViewControlsVisible(true);
            }

            if (Repository != null)
            {
                Repository.EntityInserted += Repository_EntityInserted;
            }
        }

        public virtual void OnSelectedIndexChanged()
        {
            if (Repository != null)
            {
                Repository.SetSelectedDataKey(View.SelectedDataKey);
            }

            if (View != null)
                View.SetViewControlsVisible(true);
        }

        public virtual void OnCreatingDataSource(ObjectDataSourceEventArgs e)
        {
            if (e == null)
                throw new ArgumentNullException("e");

            e.ObjectInstance = Repository;
        }

        #endregion

        #region Event Handlers

        protected void View_SelectedIndexChanged(object sender, EventArgs e)
        {
            OnSelectedIndexChanged();
        }

        protected void View_CreatingDataSource(object sender, ObjectDataSourceEventArgs e)
        {
            OnCreatingDataSource(e);
        }

        protected void View_CreateClicked(object sender, EventArgs e)
        {
            if (View != null)
                View.SetViewControlsVisible(false);
        }

        protected void View_LoadComplete(object sender, EventArgs e)
        {
            //if (View.SelectedDataKey != null)
            //    Repository.SetSelectedDataKey(View.SelectedDataKey);
        }

        protected void Repository_EntityInserted(object sender, EventArgs e)
        {
            if (View != null)
                View.SetViewControlsVisible(true);
        }

        #endregion

        #region Exposed Methods

        #endregion
    }
}
