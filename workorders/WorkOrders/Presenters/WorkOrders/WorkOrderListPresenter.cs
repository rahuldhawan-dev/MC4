using System.Web.Mvc;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MapCall.Common.Utility;
using StructureMap;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace WorkOrders.Presenters.WorkOrders
{
    public class WorkOrderListPresenter : ListPresenter<WorkOrder>
    {
        #region Private Members

        private IRepository<CrewAssignment> _crewAssignmentRepository;
        private IRepository<Crew> _crewRepository;

	    #endregion

        #region Properties

        public IWorkOrderView ListView
        {
            get { return (IWorkOrderView)View; }
        }

        // TODO: these probably shouldn't be here:
        public IRepository<CrewAssignment> CrewAssignmentRepository
        {
            get
            {
                if (_crewAssignmentRepository == null)
                    _crewAssignmentRepository =
                        DependencyResolver.Current.GetService<IRepository<CrewAssignment>>();
                return _crewAssignmentRepository;
            }
        }

        public IRepository<Crew> CrewRepository
        {
            get
            {
                if (_crewRepository == null)
                    _crewRepository =
                        DependencyResolver.Current.GetService<IRepository<Crew>>();
                return _crewRepository;
            }
        }

        #endregion

        #region Constructors

        public WorkOrderListPresenter(IListView<WorkOrder> view)
            : base(view)
        {
        }

        #endregion

        #region Event Handlers

        protected void ListView_AssignClicked(object sender, WorkOrderAssignmentEventArgs e)
        {
            var crew = CrewRepository.Get(e.CrewID);

            foreach (var id in e.WorkOrderIDs)
            {
                var order = Repository.Get(id);

                var assignment = new CrewAssignment {
                    Crew = crew,
                    WorkOrder = order,
                    AssignedFor = e.Date
                };
                CrewRepository.UpdateCurrentEntity(crew);
                Model.WorkOrderRepository.UpdateSAPWorkOrderStatic(order);
            }
        }

        protected void ListView_PlanMarkout(object sender, MarkoutPlanningEventArgs e)
        {
            var workOrder = Repository.Get(e.WorkOrderID);
            workOrder.MarkoutToBeCalled =
                WorkOrdersWorkDayEngine.GetCallDate(e.DateNeeded,
                    MarkoutRequirementEnum.Routine);
            workOrder.MarkoutTypeNeededID = e.MarkoutTypeID;
            workOrder.RequiredMarkoutNote = e.MarkoutNote;
            Repository.UpdateCurrentEntity(workOrder);
        }

        #endregion

        #region Exposed Methods

        public override void OnViewInit()
        {
            base.OnViewInit();

            switch (ListView.Phase)
            {
                case WorkOrderPhase.Scheduling:
                    ((IWorkOrderSchedulingListView)ListView).AssignClicked +=
                        ListView_AssignClicked;
                    break;
                case WorkOrderPhase.PrePlanning:
                    if (ListView is IWorkOrderMarkoutPlanningListView l)
                    {
                        l.SaveClicked += ListView_PlanMarkout;
                    }
                    break;
            }
        }

        #endregion
    }
}
