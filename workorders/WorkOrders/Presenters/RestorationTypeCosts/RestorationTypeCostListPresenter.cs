using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.RestorationTypeCosts
{
    public class RestorationTypeCostListPresenter : ListPresenter<RestorationTypeCost>
    {
        #region Constructors

        public RestorationTypeCostListPresenter(IListView<RestorationTypeCost> view)
            : base(view)
        {
        }

        #endregion
    }
}
