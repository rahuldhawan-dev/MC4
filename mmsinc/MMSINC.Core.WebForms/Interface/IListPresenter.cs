using System.Web.UI.WebControls;

namespace MMSINC.Interface
{
    public interface IListPresenter<TEntity> : IPresenter<TEntity>
        where TEntity : class
    {
        #region Methods

        void OnSelectedIndexChanged();
        void OnCreatingDataSource(ObjectDataSourceEventArgs e);
        void OnViewInit();

        #endregion
    }
}
