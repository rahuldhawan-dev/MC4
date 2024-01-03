namespace MMSINC.Interface
{
    public interface IResourcePresenter<TEntity> : IPresenter<TEntity>
        where TEntity : class
    {
        #region Properties

        IListView<TEntity> ListView { get; set; }
        IDetailView<TEntity> DetailView { get; set; }
        ISearchView<TEntity> SearchView { get; set; }

        #endregion

        #region Methods

        void SetRepositoryDataKeyFromListViewDataKey();
        void OnViewInit(IUser iUser);
        void OnViewPrerender();

        #endregion
    }
}
