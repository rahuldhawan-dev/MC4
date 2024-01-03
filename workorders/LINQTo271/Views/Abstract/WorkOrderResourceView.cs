using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.Abstract
{
    public abstract class WorkOrderResourceView : WorkOrdersResourceView<WorkOrder>, IWorkOrderView
    {
        #region Control Declarations

        protected IButton btnBackToList;

        #endregion

        #region Properties

        public override IButton BackToListButton
        {
            get { return btnBackToList; }
        }

        public abstract WorkOrderPhase Phase { get; }

        #endregion

        #region Exposed Methods

        public override void SetViewMode(ResourceViewMode newMode)
        {
            base.SetViewMode(newMode);
            ToggleList(false);
            ToggleDetail(false);
            ToggleSearch(false);
            ToggleBackToListButton(false);
            switch (newMode)
            {
                case ResourceViewMode.Search:
                    ToggleSearch(true);
                    break;
                case ResourceViewMode.Detail:
                    ToggleDetail(true);
                    ToggleBackToListButton(true);
                    break;
                default:
                    ToggleList(true);
                    break;
            }
        }

        #endregion
    }
}
