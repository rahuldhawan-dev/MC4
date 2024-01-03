using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.OperatingCenterSpoilRemovalCosts
{
    public partial class OperatingCenterSpoilRemovalCostListView : WorkOrdersListView<OperatingCenterSpoilRemovalCost>
    {
        #region Control Declarations

        protected IListControl gvOperatingCenterSpoilRemovalCosts;

        #endregion

        #region Properties

        public override IListControl ListControl
        {
            get { return gvOperatingCenterSpoilRemovalCosts; }
        }

        #endregion

        #region Event Handlers

        protected void ListControl_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            e.OldValues.Clear();
        }

        #endregion

        #region Exposed Methods

        public override void SetViewControlsVisible(bool visible)
        {
            // noop (derp).
        }

        #endregion
    }
}