using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.CrewAssignments
{
    public interface ICrewAssignmentsRPCListView : IListView<CrewAssignment>
    {
        #region Events

        event CrewAssignmentPrioritizeEventHandler PrioritizeCommand;
        event CrewAssignmentDeleteEventHandler DeleteCommand;

        #endregion

        #region Methods

        void Redirect(string url);

        #endregion
    }
}