using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.PermitProcessing
{
    public partial class PermitProcessingDetailView : WorkOrderDetailView
    {
        public override IDetailControl DetailControl
        {
            get { return fvWorkOrder; }
        }

        public override IObjectContainerDataSource DataSource
        {
            get { return odsWorkOrder; }
        }

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Planning; }
        }
    }
}