using System;
using System.Collections.Generic;

namespace WorkOrders.Views.CrewAssignments
{
    public class CrewAssignmentPrioritizeEventArgs : EventArgs
    {
        #region Private Members

        private List<CrewAssignmentPriorities> _crewAssignmentPriorities;

        #endregion

        #region Properties

        public List<CrewAssignmentPriorities> CrewAssignmentPriorities
        {
            get
            {
                return _crewAssignmentPriorities;
            }
        }

        #endregion

        #region Constructors

        public CrewAssignmentPrioritizeEventArgs(List<CrewAssignmentPriorities> priorities)
        {
            _crewAssignmentPriorities = priorities;
        }

        #endregion
    }

    public struct CrewAssignmentPriorities
    {
        public int CrewAssignmentID;
        public int Priority;
    }
}
