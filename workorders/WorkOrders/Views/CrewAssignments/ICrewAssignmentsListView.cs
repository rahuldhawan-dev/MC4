using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.CrewAssignments
{
    public interface ICrewAssignmentsListView : IListView<CrewAssignment>
    {
        #region Events

        event CrewAssignmentStartEndEventHandler AssignmentCommand;

        #endregion

        #region Methods

        void Redirect(string url);

        #endregion
    }
}
