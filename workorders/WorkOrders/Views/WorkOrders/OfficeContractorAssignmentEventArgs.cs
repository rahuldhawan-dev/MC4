using System;

namespace WorkOrders.Views.WorkOrders
{
    public class OfficeContractorAssignmentEventArgs : EventArgs
    {
        #region Properties

        public int WorkOrderID { get; protected set; }
        public int ContractorID { get; protected set; }
        public DateTime Date { get; protected set; }

        #endregion

        #region Constructors

        public OfficeContractorAssignmentEventArgs(int workOrderID, int contractorID, DateTime date)
        {
            WorkOrderID = workOrderID;
            ContractorID = contractorID;
            Date = date;
        }

        #endregion
    }

    public delegate void OfficeContractorAssignmentEventHandler(
    object sender, OfficeContractorAssignmentEventArgs e);

}