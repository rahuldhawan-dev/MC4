using System;
using System.Web.Mvc;
using MMSINC.Data.Linq;
using MMSINC.Exceptions;
using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using WorkOrders.Views.CrewAssignments;

namespace WorkOrders.Presenters.CrewAssignments
{
    public class CrewAssignmentsListPresenter : ListPresenter<CrewAssignment>
    {
        #region Constants

        public struct RedirectUrls
        {
            public const string PRINT_VIEW =
                                    "~/Views/WorkOrders/ReadOnly/WorkOrderReadOnlyRPCPage.aspx?cmd=view&arg={0}",
                                FINALIZATION =
                                    "~/Views/WorkOrders/Finalization/WorkOrderFinalizationResourceRPCPage.aspx?cmd=update&arg={0}";
        }

        #endregion

        #region Private Members

        private ICrewAssignmentsListView _listView;
        private ICrewAssignmentsRPCListView _rpcListView;
        protected IRepository<Markout> _markoutRepository;
        protected global::WorkOrders.Library.Permissions.ISecurityService _securityService;

        #endregion

        #region Properties

        public ICrewAssignmentsListView CrewAssignmentsListView
        {
            get
            {
                if (_listView == null)
                    _listView = View as ICrewAssignmentsListView;
                return _listView;
            }
        }

        public ICrewAssignmentsRPCListView CrewAssignmentsRPCListView
        {
            get {
                if (_rpcListView == null)
                    _rpcListView = View as ICrewAssignmentsRPCListView;
                return _rpcListView;
            }
        }

        public IRepository<Markout> MarkoutRepository
        {
            get
            {
                if(_markoutRepository == null)
                {
                    _markoutRepository =
                        DependencyResolver.Current.GetService<IRepository<Markout>>();
                }
                return _markoutRepository;
            }    
        }

        public ISecurityService SecurityService => _securityService ??
                                                   (_securityService = Library.Permissions.SecurityService.Instance);

        #endregion

        #region Constructors

        public CrewAssignmentsListPresenter(IListView<CrewAssignment> view)
            : base(view)
        {
        }

        #endregion

        #region Private Methods

        private void SetDateForCommand(CrewAssignmentStartEndEventArgs e, CrewAssignment assignment)
        {
            switch (e.Command)
            {
                case CrewAssignmentStartEndEventArgs.Commands.Start:
                    assignment.DateStarted = e.Date;
                    assignment.StartedBy = SecurityService.Employee;
                    assignment.WorkOrder.SetCurrentMarkoutExpiration();
                    if(assignment.WorkOrder.MarkoutRequired && assignment.WorkOrder.CurrentMarkout != null)
                        MarkoutRepository.UpdateCurrentEntity(assignment.WorkOrder.CurrentMarkout);
                    break;
                case CrewAssignmentStartEndEventArgs.Commands.End:
                    if(e.Date < assignment.DateStarted) { throw new DomainLogicException($"Date Ended {e.Date} is before Date Started {assignment.DateStarted}, the current time is {DateTime.Now}"); }
                    assignment.DateEnded = e.Date;
                    assignment.EmployeesOnJob = e.EmployeesOnJob;
                    break;
            }
            Repository.UpdateCurrentEntity(assignment);
            CrewAssignmentRepository.UpdateSAPWorkOrderStatic(assignment.WorkOrder);
        }

        private void ProcessPostUpdate(CrewAssignmentStartEndEventArgs e, CrewAssignment assignment)
        {
            Redirect(RedirectUrls.FINALIZATION, assignment.WorkOrderID);
        }

        protected void Redirect(string format, int workOrderID)
        {
            CrewAssignmentsListView.Redirect(string.Format(format, workOrderID));
        }

        #endregion

        #region Exposed Methods

        public override void OnViewLoaded()
        {
            if (CrewAssignmentsListView != null)
            {
                CrewAssignmentsListView.AssignmentCommand +=
                    View_AssignmentCommand;
            }
            else if (CrewAssignmentsRPCListView != null)
            {
                CrewAssignmentsRPCListView.DeleteCommand += View_DeleteCommand;
                CrewAssignmentsRPCListView.PrioritizeCommand +=
                    View_PrioritizeCommand;
            }
        }

        #endregion

        #region Event Handlers

        //TODO: This belongs in the presenter.
        protected void View_AssignmentCommand(object sender, CrewAssignmentStartEndEventArgs e)
        {
            var assignment = Repository.Get(e.CrewAssignmentID);
            SetDateForCommand(e, assignment);
            ProcessPostUpdate(e, assignment);
        }

        protected void View_PrioritizeCommand(object sender, CrewAssignmentPrioritizeEventArgs e)
        {
            foreach(var x in e.CrewAssignmentPriorities)
            {
                var crewAssignment = Repository.Get(x.CrewAssignmentID);
                crewAssignment.Priority = x.Priority;
                Repository.UpdateCurrentEntity(crewAssignment);
                CrewAssignmentRepository.UpdateSAPWorkOrderStatic(crewAssignment.WorkOrder);
            }
        }

        protected void View_DeleteCommand(object sender, CrewAssignmentDeleteEventArgs e)
        {
            foreach (var x in e.CrewAssignmentIDs)
            {
                Repository.DeleteEntity(Repository.Get(x));
            }
        }

        #endregion
    }
}
