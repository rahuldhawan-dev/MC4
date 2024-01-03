using System;
using MMSINC.Data.Linq;

namespace MMSINC.Interface
{
    public interface IResourceView<TEntity> : IResourceView
        where TEntity : class
    {
        #region Properties

        IRepository<TEntity> Repository { get; }
        IResourcePresenter<TEntity> Presenter { get; }
        IListView<TEntity> ListView { get; }
        IDetailView<TEntity> DetailView { get; }
        ISearchView<TEntity> SearchView { get; }

        #endregion
    }

    public interface IResourceView : IView
    {
        #region Properties

        ResourceViewMode CurrentMode { get; }
        IPage IPage { get; }

        /// <summary>
        /// Alex doesn't like this name.
        /// </summary>
        string RelativeUrl { get; }

        /// <summary>
        /// URL to redirect to when the presenter initializes the view, if
        /// CurrentMode is 'Redirect'
        /// </summary>
        string RedirectURL { get; set; }

        #endregion

        #region Events

        event EventHandler BackToListClicked;

        #endregion

        #region Methods

        void SetDetailMode(DetailViewMode mode);
        void SetViewMode(ResourceViewMode mode);
        void Redirect(string url);

        #endregion
    }

    public enum ResourceViewMode
    {
        /// <summary>
        /// Loads the views in 'List' mode
        /// </summary>
        List,

        /// <summary>
        /// Loads the views in 'Detail' mode
        /// </summary>
        Detail,

        /// <summary>
        /// Loads the views in 'Search' mode
        /// </summary>
        Search,

        /// <summary>
        /// Tells the view to redirect to a URL on load
        /// </summary>
        Redirect
    }
}
