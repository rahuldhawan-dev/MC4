using System;

namespace WorkOrders.Views.CrewAssignments
{
    public class CrewAssignmentDeleteEventArgs : EventArgs
    {
        #region Private Members

        private int[] _crewAssignmentIDs;

        #endregion

        #region Properties

        public int[] CrewAssignmentIDs
        {
            get
            {
                return _crewAssignmentIDs;
            }
        }

        #endregion

        #region Constructors

        public CrewAssignmentDeleteEventArgs(int[] crewAssignmentIDs)
        {
            _crewAssignmentIDs = crewAssignmentIDs;
        }

        #endregion
    }
}
