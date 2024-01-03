using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.RestorationTypeCosts
{
    public partial class RestorationTypeCostListView : WorkOrdersListView<RestorationTypeCost>
    {
        #region Control Declarations

        protected IListControl gvRestorationTypeCosts;

        #endregion

        #region Properties

        public override IListControl ListControl
        {
            get { return gvRestorationTypeCosts; }
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
            // noop (for now)
        }

        #endregion
    }
}
