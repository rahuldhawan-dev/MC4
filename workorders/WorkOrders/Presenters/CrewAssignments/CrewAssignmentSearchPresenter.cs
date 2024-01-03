using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.CrewAssignments
{
    public class CrewAssignmentSearchPresenter : SearchPresenter<CrewAssignment>
    {
        #region Constructors

        public CrewAssignmentSearchPresenter(ISearchView<CrewAssignment> view)
            : base(view)
        {
        }

        #endregion
    }
}
