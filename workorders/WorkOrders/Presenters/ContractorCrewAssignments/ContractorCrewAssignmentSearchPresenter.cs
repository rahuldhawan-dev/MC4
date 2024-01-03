using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.ContractorCrewAssignments
{
    public class ContractorCrewAssignmentSearchPresenter : SearchPresenter<CrewAssignment>
    {
        #region Constructors

        public ContractorCrewAssignmentSearchPresenter(
            ISearchView<CrewAssignment> view) : base(view)
        {
        }

        #endregion

    }
}
