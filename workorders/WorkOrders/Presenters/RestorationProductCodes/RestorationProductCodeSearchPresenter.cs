using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.RestorationProductCodes
{
    public class RestorationProductCodeSearchPresenter : SearchPresenter<RestorationProductCode>
    {
        #region Constructors

        public RestorationProductCodeSearchPresenter(ISearchView<RestorationProductCode> view)
            : base(view)
        {
        }

        #endregion
    }
}
