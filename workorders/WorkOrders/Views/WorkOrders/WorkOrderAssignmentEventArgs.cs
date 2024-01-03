using System;
using System.Collections.Generic;

namespace WorkOrders.Views.WorkOrders
{
    public class WorkOrderAssignmentEventArgs : EventArgs
    {
        #region Properties

        public int CrewID { get; protected set; }
        public DateTime Date { get; protected set; }
        public IEnumerable<int> WorkOrderIDs { get; protected set; }

        #endregion

        #region Constructors

        public WorkOrderAssignmentEventArgs(int crewID, DateTime date, IEnumerable<int> workOrderIDs)
        {
            CrewID = crewID;
            Date = date;
            WorkOrderIDs = workOrderIDs;
        }

        #endregion
    }

    public delegate void WorkOrderAssignmentEventHandler(
        object sender, WorkOrderAssignmentEventArgs e);
}