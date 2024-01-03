using System;

namespace WorkOrders.Views.WorkOrders
{
    public class MarkoutPlanningEventArgs : EventArgs
    {
        #region Properties

        public int MarkoutTypeID { get; protected set; }
        public DateTime DateNeeded { get; protected set; }
        public int WorkOrderID { get; protected set; }
        public string MarkoutNote { get; protected set; }

        #endregion

        #region Constructors

        public MarkoutPlanningEventArgs(int workOrderID, DateTime dateNeeded, int markoutTypeID, string markoutNote)
        {
            WorkOrderID = workOrderID;
            DateNeeded = dateNeeded;
            MarkoutTypeID = markoutTypeID;
            MarkoutNote = markoutNote;
        }

        #endregion
    }
}
