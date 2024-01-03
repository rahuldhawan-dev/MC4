using System;

namespace WorkOrders.Views.WorkOrders
{
    public class OfficeAssignmentEventArgs : EventArgs
    {
        #region Properties

        public int WorkOrderID { get; protected set; }
        public int EmployeeID { get; protected set; }
        public DateTime Date { get; protected set; }

        #endregion

        #region Constructors

        public OfficeAssignmentEventArgs(int workOrderID, int employeeID, DateTime date)
        {
            WorkOrderID = workOrderID;
            EmployeeID = employeeID;
            Date = date;
        }

        #endregion
    }

    public delegate void OfficeAssignmentEventHandler(
        object sender, OfficeAssignmentEventArgs e);
}
