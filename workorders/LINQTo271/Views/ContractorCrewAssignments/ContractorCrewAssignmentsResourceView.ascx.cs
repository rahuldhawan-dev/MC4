using System;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Views.CrewAssignments;

namespace LINQTo271.Views.ContractorCrewAssignments
{
    public partial class ContractorCrewAssignmentsResourceView : WorkOrdersResourceView<CrewAssignment>, ICrewAssignmentsResourceView
    {
        #region Control Declarations

        protected IListView<CrewAssignment> lvCrewAssignmentsListView;
        protected ISearchView<CrewAssignment> lvCrewAssignmentsSearchView;

        #endregion

        #region Properties

        public override IButton BackToListButton
        {
            get { return null; }
        }

        public override IListView<CrewAssignment> ListView
        {
            get { return lvContractorCrewAssignmentsListView; }
        }

        //No Detail View
        public override IDetailView<CrewAssignment> DetailView
        {
            get { return null; }
        }

        public override ISearchView<CrewAssignment> SearchView
        {
            get { return lvContractorCrewAssignmentsSearchView; }
        }

        #endregion

        #region ICrewAssignmentsResourceView Members

        public void DataBindCrew(Crew crew)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}