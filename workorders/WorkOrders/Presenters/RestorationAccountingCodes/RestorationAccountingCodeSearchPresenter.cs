using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.RestorationAccountingCodes
{
    public class RestorationAccountingCodeSearchPresenter : SearchPresenter<RestorationAccountingCode>
    {
        #region Constructors

        public RestorationAccountingCodeSearchPresenter(ISearchView<RestorationAccountingCode> view)
            : base(view)
        {
        }

        #endregion
    }
}
