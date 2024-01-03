using System;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;

namespace MMSINC.Presenter
{
    public abstract class DetailPresenter<TEntity> : IDetailPresenter<TEntity>
        where TEntity : class
    {
        #region Private Members

        private readonly IDetailView<TEntity> _view;

        #endregion

        #region Properties

        /// <summary>
        /// IDetailView which this presenter will supervise.
        /// </summary>
        public virtual IDetailView<TEntity> View
        {
            get { return _view; }
        }

        /// <summary>
        /// IRepository for the entity type that this
        /// presenter has been setup for.
        /// </summary>
        public virtual IRepository<TEntity> Repository { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the EntityDetailPresenter,
        /// initialized to use view as its IDetailView.
        /// </summary>
        /// <param name="view">
        /// IDetailView intance to initialize with.
        /// </param>
        public DetailPresenter(IDetailView<TEntity> view)
        {
            _view = view;
        }

        #endregion

        #region Event Passthroughs

        /// <summary>
        /// This method is intended to be called from the codebehind of
        /// the view control, during the Page_Load event.  It's an
        /// initialization step, which means it should only be called
        /// once, and not during a PostBack.
        /// </summary>
        public virtual void OnViewInitialized()
        {
            if (View != null)
                View.SetViewControlsVisible(false);
        }

        /// <summary>
        /// This method is intended to be called from the codebehind of
        /// the view control, once it's Page_Load event has been fired
        /// and processed.
        /// </summary>
        public virtual void OnViewLoaded()
        {
            if (Repository != null)
                Repository.CurrentEntityChanged += Repository_CurrentEntityChanged;
            if (View != null)
            {
                View.EditClicked += View_EditClicked;
                View.DiscardChangesClicked += View_DiscardChangesClicked;
                View.Inserting += View_Inserting;
                View.Updating += View_Updating;
                View.DeleteClicked += View_DeleteClicked;
            }
        }

        #endregion

        #region Event Handlers

        protected virtual void Repository_CurrentEntityChanged(object sender, EventArgs e)
        {
            LoadCurrentEntityOnView();
            View.SetViewControlsVisible(true);
            View.SetViewMode(DetailViewMode.ReadOnly);
        }

        protected virtual void View_EditClicked(object sender, EventArgs e)
        {
            LoadCurrentEntityOnView();
            View.SetViewMode(DetailViewMode.Edit);
        }

        protected virtual void View_DiscardChangesClicked(object sender, EventArgs e)
        {
            if (Repository.CurrentEntity != null)
            {
                LoadCurrentEntityOnView();
                View.SetViewMode(DetailViewMode.ReadOnly);
            }
        }

        protected virtual void View_Inserting(object sender, EntityEventArgs<TEntity> e)
        {
            if (View != null)
                Repository.InsertNewEntity(e.Entity);
            LoadCurrentEntityOnView();
            if (View != null)
                View.SetViewMode(DetailViewMode.ReadOnly);
        }

        protected virtual void View_Updating(object sender, EntityEventArgs<TEntity> e)
        {
            if (View != null)
                Repository.UpdateCurrentEntity(e.Entity);
            LoadCurrentEntityOnView();
            if (View != null)
                View.SetViewMode(DetailViewMode.ReadOnly);
        }

        protected virtual void View_DeleteClicked(object sender, EntityEventArgs<TEntity> e)
        {
            Repository.DeleteEntity(e.Entity);
        }

        #endregion

        #region Private Methods

        protected virtual void LoadCurrentEntityOnView()
        {
            View.ShowEntity(Repository.CurrentEntity);
            if (View.ChildResourceViews != null)
                foreach (var view in View.ChildResourceViews)
                {
                    view.Presenter.ParentEntity = Repository.CurrentEntity;
                    view.Presenter.FilterListViews();
                }
        }

        #endregion
    }
}
