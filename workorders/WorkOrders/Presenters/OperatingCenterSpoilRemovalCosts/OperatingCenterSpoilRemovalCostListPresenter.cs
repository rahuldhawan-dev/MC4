using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.OperatingCenterSpoilRemovalCosts
{
    public class OperatingCenterSpoilRemovalCostListPresenter : ListPresenter<OperatingCenterSpoilRemovalCost>
    {
        #region Constructors

        public OperatingCenterSpoilRemovalCostListPresenter(IListView<OperatingCenterSpoilRemovalCost> view)
            : base(view)
        {
        }

        #endregion
    }
}
