using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;
using WorkOrders.Views.RestorationAccountingCodes;

namespace WorkOrders.Presenters.RestorationAccountingCodes
{
    public class RestorationAccountingCodeResourcePresenter : ResourcePresenter<RestorationAccountingCode>
    {
        #region Properties

        protected IRestorationAccountingCodeSearchView RestorationAccountingCodeSearchView
        {
            get { return (IRestorationAccountingCodeSearchView)SearchView; }
        }

        protected IRestorationAccountingCodeListView RestorationAccountingCodeListView
        {
            get { return (IRestorationAccountingCodeListView)ListView; }
        }

        #endregion
        
        #region Constructors

        public RestorationAccountingCodeResourcePresenter(IResourceView view, IRepository<RestorationAccountingCode> repository)
            : base(view, repository)
        {
        }

        #endregion
    }
}
