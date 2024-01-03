using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.CrewAssignments
{
    public interface ICrewAssignmentsReadOnly : IResourceView
    {
        #region Properties

        int? CrewID { get; }

        #endregion

        #region Methods

        void DataBind();
        void DataBindCrew(Crew crew);
        void Redirect(string url);

        #endregion

        #region Events

        event CrewAssignmentStartEndEventHandler AssignmentCommand;
        
        #endregion
    }
}