namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderMapForm : WorkOrderDetailControlBase, IWorkOrderMapForm
    {
        #region Private Methods

        protected override void SetDataSource(int workOrderID) 
        { 
            //noop 
        }

        #endregion
    }

    public interface IWorkOrderMapForm : IWorkOrderDetailControl
    {
    }
}