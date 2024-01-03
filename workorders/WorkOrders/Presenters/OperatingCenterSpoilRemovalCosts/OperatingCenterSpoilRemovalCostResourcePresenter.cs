using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;

namespace WorkOrders.Presenters.OperatingCenterSpoilRemovalCosts
{
    public class OperatingCenterSpoilRemovalCostResourcePresenter : WorkOrdersAdminResourcePresenter<OperatingCenterSpoilRemovalCost>
    {
        #region Constructors

        public OperatingCenterSpoilRemovalCostResourcePresenter(IResourceView view, IRepository<OperatingCenterSpoilRemovalCost> repository)
            : base(view, repository)
        {
        }

        #endregion
    }
}