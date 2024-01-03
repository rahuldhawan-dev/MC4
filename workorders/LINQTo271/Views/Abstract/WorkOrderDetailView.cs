using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.Abstract
{
    public abstract class WorkOrderDetailView : WorkOrdersDetailView<WorkOrder>, IWorkOrderDetailView
    {
        #region Private Members

        private bool _modeSet;

        #endregion

        #region Properties

        public abstract WorkOrderPhase Phase { get; }
        public bool ModeSet { get { return _modeSet; } }

        #endregion

        #region Exposed Methods

        public override void SetViewMode(DetailViewMode newMode)
        {
            base.SetViewMode(newMode);
            _modeSet = true;
        }

        #endregion
    }
}
