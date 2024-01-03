namespace MMSINC.Interface
{
    public interface IChildResourcePresenter<TEntity> : IResourcePresenter<TEntity>
        where TEntity : class { }

    public interface IChildResourcePresenter
    {
        #region Properties

        object ParentEntity { get; set; }

        #endregion

        #region Methods

        void FilterListViews();

        #endregion
    }
}
