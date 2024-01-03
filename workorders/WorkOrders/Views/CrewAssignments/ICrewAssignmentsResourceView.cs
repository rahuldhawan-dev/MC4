using MMSINC.Interface;
using WorkOrders.Model;

namespace WorkOrders.Views.CrewAssignments
{
    public interface ICrewAssignmentsResourceView : IResourceView
    {
        #region Methods

        void DataBind();
        void DataBindCrew(Crew crew);

        #endregion
    }
}