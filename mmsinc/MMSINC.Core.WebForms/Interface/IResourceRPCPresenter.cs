using MMSINC.Presenter;

namespace MMSINC.Interface
{
    public interface IResourceRPCPresenter<TEntity> : IPresenter<TEntity>
        where TEntity : class
    {
        #region Properties

        IResourceRPCView<TEntity> RPCView { get; }
        IListView<TEntity> ListView { get; set; }
        IDetailView<TEntity> DetailView { get; set; }
        ISearchView<TEntity> SearchView { get; set; }

        #endregion

        #region Methods

        void OnViewInit(IUser iUser);
        void ChangeViewCommand(RPCCommands command);

        #endregion
    }
}
