using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;

namespace WorkOrders.Presenters.CrewAssignments
{
    public class CrewAssignmentResourceRPCPresenter : WorkOrdersResourceRPCPresenter<CrewAssignment>
    {
        #region Constructors

        public CrewAssignmentResourceRPCPresenter(IResourceRPCView<CrewAssignment> view, IRepository<CrewAssignment> repository)
            : base(view, repository)
        {
        }

        #endregion

        #region Exposed Methods

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();

            View.SetViewMode(ResourceViewMode.List);
        }

        #endregion
    }
}
