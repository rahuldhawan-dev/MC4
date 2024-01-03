using System;
using WorkOrders.Model;

namespace WorkOrders.Views.CrewAssignments
{
    public interface ICrewAssignmentsByMonth
    {
        #region Properties

        DateTime SelectedDate { get; set; }
        DateTime VisibleDate { get; set; }

        #endregion

        #region Methods

        void DataBind(Crew crew, DateTime date);

        #endregion
    }
}
