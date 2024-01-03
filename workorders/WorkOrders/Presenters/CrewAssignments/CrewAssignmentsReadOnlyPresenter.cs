using System;
using System.Web.Mvc;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using StructureMap;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Views.CrewAssignments;

namespace WorkOrders.Presenters.CrewAssignments
{
    public class CrewAssignmentsReadOnlyPresenter : WorkOrdersResourcePresenter<CrewAssignment>, ICrewAssignmentsReadOnlyPresenter
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

        private IRepository<CrewAssignment> _repository;
        private IRepository<Crew> _crewRepository;

        #endregion

        #region Properties

        public override IRepository<CrewAssignment> Repository
        {
            get
            {
                if (_repository == null)
                    _repository =
                        DependencyResolver.Current.GetService<IRepository<CrewAssignment>>();
                return _repository;
            }
            set { _repository = value; }
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
            set { _crewRepository = value; }
        }

        public ICrewAssignmentsReadOnly CrewAssignmentsView
        {
            get { return (ICrewAssignmentsReadOnly)View; }
        }

        #endregion

        #region Constructors

        public CrewAssignmentsReadOnlyPresenter(ICrewAssignmentsReadOnly view, IRepository<CrewAssignment> repository) : base(view, repository)
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
                    break;
                case CrewAssignmentStartEndEventArgs.Commands.End:
                    assignment.DateEnded = e.Date;
                    break;
            }
            Repository.UpdateCurrentEntity(assignment);
        }

        private void ProcessPostUpdate(CrewAssignmentStartEndEventArgs e, CrewAssignment assignment)
        {
            switch (e.Command)
            {
                case CrewAssignmentStartEndEventArgs.Commands.Start:
                    CrewAssignmentsView.DataBind();
                    break;
                case CrewAssignmentStartEndEventArgs.Commands.End:
                    Redirect(RedirectUrls.FINALIZATION, assignment.WorkOrderID);
                    break;
            }
        }

        private void DataBindView()
        {
            if (CrewAssignmentsView.CrewID != null)
            {
                var crew = CrewRepository.Get(CrewAssignmentsView.CrewID);

                CrewAssignmentsView.DataBindCrew(crew);
            }
        }

        protected void Redirect(string format, int workOrderID)
        {
            CrewAssignmentsView.Redirect(string.Format(format, workOrderID));
        }

        // TODO: This is not tested because the page needs to be completely redone in mvp : arr, gun to head fix
        protected override void CheckUserSecurity()
        {
            if (!SecurityService.UserHasAccess)
                throw new UnauthorizedAccessException("The Current User has not been granted access to the Work Management system.");
        }

        #endregion

        #region Event Passthroughs

        // TODO: This is not tested because the page needs to be completely redone in mvp : arr, gun to head fix
        public override void OnViewInit(IUser iUser)
        {
            if (SecurityService != null)
                SecurityService.Init(iUser);

            CheckUserSecurity();
        }

        #endregion

        #region Event Handlers

        protected void View_AssignmentCommand(object sender, CrewAssignmentStartEndEventArgs e)
        {
            var assignment = Repository.Get(e.CrewAssignmentID);
            SetDateForCommand(e, assignment);
            ProcessPostUpdate(e, assignment);
        }

        #endregion

        #region Exposed Methods

        public void OnViewInitialized()
        {
            // noop
        }

        public void OnViewLoaded()
        {
            CrewAssignmentsView.AssignmentCommand += View_AssignmentCommand;

            DataBindView();
        }

        #endregion
    }

    public interface ICrewAssignmentsReadOnlyPresenter : IPresenter<CrewAssignment>
    {
        void OnViewInit(IUser iUser);
    }
}
