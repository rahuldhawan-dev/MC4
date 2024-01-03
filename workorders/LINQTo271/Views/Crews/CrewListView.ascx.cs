using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.Crews
{
    public partial class CrewListView : WorkOrdersListView<Crew>
    {
        #region Properties

        public override IListControl ListControl
        {
            get { return gvCrews; }
        }

        #endregion

        #region Methods

        public override void SetViewControlsVisible(bool visible)
        {
            btnCreate.Visible = visible;
        }

        #endregion
    }
}
