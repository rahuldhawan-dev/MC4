using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;
using WorkOrders.Views.RestorationProductCodes;

namespace WorkOrders.Presenters.RestorationProductCodes
{
    public class RestorationProductCodeResourcePresenter : ResourcePresenter<RestorationProductCode>
    {
        #region Properties

        protected IRestorationProductCodeSearchView RestorationProductCodeSearchView
        {
            get { return (IRestorationProductCodeSearchView)SearchView; }
        }

        protected IRestorationProductCodeListView RestorationProductCodeListView
        {
            get { return (IRestorationProductCodeListView)ListView; }
        }

        #endregion

        #region Constructors

        public RestorationProductCodeResourcePresenter(IResourceView view, IRepository<RestorationProductCode> repository)
            : base(view, repository)
        {
        }

        #endregion
    }
}
