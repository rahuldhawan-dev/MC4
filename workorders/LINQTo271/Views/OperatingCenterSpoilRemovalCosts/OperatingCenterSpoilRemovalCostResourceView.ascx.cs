using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.OperatingCenterSpoilRemovalCosts
{
    public partial class OperatingCenterSpoilRemovalCostResourceView : WorkOrdersResourceView<OperatingCenterSpoilRemovalCost>
    {
        #region Control Declarations

        protected IListView<OperatingCenterSpoilRemovalCost> ocstListView;

        #endregion

        #region Properties

        public override IListView<OperatingCenterSpoilRemovalCost> ListView
        {
            get { return ocstListView; }
        }

        public override IDetailView<OperatingCenterSpoilRemovalCost> DetailView
        {
            get { return null; }
        }

        public override ISearchView<OperatingCenterSpoilRemovalCost> SearchView
        {
            get { return null; }
        }

        public override IButton BackToListButton
        {
            get { return null; }
        }

        #endregion
    }
}