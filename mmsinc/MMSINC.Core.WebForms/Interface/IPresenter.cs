using MMSINC.Data.Linq;

namespace MMSINC.Interface
{
    public interface IPresenter<TEntity>
        where TEntity : class
    {
        #region Properties

        IRepository<TEntity> Repository { get; set; }

        #endregion

        #region Methods

        void OnViewInitialized();
        void OnViewLoaded();

        #endregion
    }
}
