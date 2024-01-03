using System;
using System.Web.Mvc;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using StructureMap;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Views.CrewAssignments;
using PredicateBuilder = MMSINC.Common.PredicateBuilder;

namespace WorkOrders.Presenters.ContractorCrewAssignments
{
    public class ContractorCrewAssignmentResourcePresenter : WorkOrdersResourcePresenter<CrewAssignment>
    {
        private const string SQLSORT = "Priority";
        
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

        public ICrewAssignmentsResourceView CrewAssignmentsView
        {
            get { return (ICrewAssignmentsResourceView)View; }
        }

        #endregion

        #region Constructors

        public ContractorCrewAssignmentResourcePresenter(IResourceView view, IRepository<CrewAssignment> repository)
            : base(view, repository)
        {
        }

        #endregion

        #region Event Handlers

        protected override void View_LoadComplete(object sender, EventArgs e)
        {
            if (ListView != null && ListView.Visible)
            {
                SetListViewData();
                ListView.DataBind();
            }
        }

        //This override always sorts by SQLSORT
        protected override void SetListViewData()
        {
            SetListViewData((SearchView != null) ? SearchView.GenerateExpression() : PredicateBuilder.True<CrewAssignment>(),
                (ListView != null) ? SQLSORT : null);
        }

        #endregion

        #region Exposed Methods

        protected override void CheckUserSecurity()
        {
            if (!SecurityService.IsAdmin)
                throw new UnauthorizedAccessException("The Current User has not been granted access to the Work Management system.");
        }

        #endregion
    }

    public interface ICrewAssignmentResourceViewPresenter : IPresenter<CrewAssignment>
    {
        void OnViewInit(IUser iUser);
    }
}
