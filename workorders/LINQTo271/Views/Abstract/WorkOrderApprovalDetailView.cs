using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.Abstract
{
    public abstract class WorkOrderApprovalDetailView : WorkOrderDetailView, IWorkOrderApprovalDetailView
    {
        #region Control Declarations

        protected IDetailControl fvWorkOrder;
        protected IObjectContainerDataSource odsWorkOrder;

        #endregion

        #region Properties

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Approval; }
        }

        public override sealed IDetailControl DetailControl
        {
            get { return fvWorkOrder; }
        }

        public sealed override IObjectContainerDataSource DataSource
        {
            get { return odsWorkOrder; }
        }

        #endregion
    }
}
