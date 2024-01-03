
namespace WorkOrders.Views.WorkOrders
{
    public interface IWorkOrderPrePlanningListView : IWorkOrderListView
    {
        #region Events

        event OfficeAssignmentEventHandler AssignClicked;
        event OfficeContractorAssignmentEventHandler ContractorAssignClicked;

        #endregion
    }
}
