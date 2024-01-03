using System;
using System.Linq.Expressions;
using MMSINC.Common;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using WorkOrders.Model;
using WorkOrders.Presenters.Abstract;
using WorkOrders.Views.WorkOrders;

namespace WorkOrders.Presenters.WorkOrders
{
    /// <summary>
    /// Presenter class intended to manage all access to the WorkOrder resource
    /// by any of its views.
    /// </summary>
    public class WorkOrderResourcePresenter : WorkOrdersResourcePresenter<WorkOrder>
    {
        #region Constants

        // McAdmin (or dev or something, idk)
        public const int CREATOR_ID = 291;
        public const string CREW_VIEW_URL_FORMAT = "/Modules/mvc/FieldOperations/CrewAssignment/ShowCalendar{0}";
        public const string CREW_VIEW_DATA_FORMAT = "?crew={0}&date={1}";
        public const string GENERAL_VIEW_EDIT_URL_FORMAT =
            "~/Views/WorkOrders/General/WorkOrderGeneralResourceRPCPage.aspx?cmd=update&arg={0}";
        public const string NO_RECORDS_FOUND_ERROR =
            "No records found.  Please refine your search.";
        public const string OVER_SEARCH_RESULT_LIMIT_ERROR =
            "The query you have entered will bring back more than {0} results.  Please refine your search.";
        public const int SEARCH_RESULT_LIMIT = 1000;

        #endregion

        #region Private Members

        private IWorkOrderView _resourceView;
        private IWorkOrderSearchView _workOrderSearchView;

        #endregion

        #region Properties

        protected IWorkOrderView ResourceView
        {
            get
            {
                if (_resourceView == null)
                    _resourceView = (IWorkOrderView)View;
                return _resourceView;
            }
        }

        protected IWorkOrderSearchView WorkOrderSearchView
        {
            get
            {
                if (_workOrderSearchView == null)
                    _workOrderSearchView = (IWorkOrderSearchView)SearchView;
                return _workOrderSearchView;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the WorkOrderResourcePresenter class,
        /// using the supplied view and repository.
        /// </summary>
        /// <param name="view">IResourceView object used to manage access to
        /// child views.</param>
        /// <param name="repository">Typed IRepository object for the WorkOrder
        /// class.</param>
        public WorkOrderResourcePresenter(IResourceView view, IRepository<WorkOrder> repository)
            : base(view, repository)
        {
        }


        #endregion

        #region Private Methods

        protected override void CheckUserSecurity()
        {
            base.CheckUserSecurity();

            switch (ResourceView.Phase)
            {
                case WorkOrderPhase.Scheduling:
                case WorkOrderPhase.Approval:
                case WorkOrderPhase.StockApproval:
                    if (!SecurityService.IsAdmin)
                        throw new UnauthorizedAccessException("You must be an administrator to access the Approval Phase of the work order.");
                    break;
                default:
                    break;
            }
        }

        private void InitializeForPlanning()
        {
            View.SetViewMode(ResourceViewMode.Search);
        }

        private void InitializeForPrePlanning()
        {
            View.SetViewMode(ResourceViewMode.Search);
        }

        private void InitializeForScheduling()
        {
            View.SetViewMode(ResourceViewMode.Search);
        }

        private void InitializeForInput()
        {
            View.SetViewMode(ResourceViewMode.Detail);
        }

        private void InitializeForFinalization()
        {
            View.SetViewMode(ResourceViewMode.Search);
        }

        private void InitializeForApproval()
        {
            View.SetViewMode(ResourceViewMode.Search);
        }

        private void InitializeForGeneralSearchAndEdit()
        {
            View.SetViewMode(ResourceViewMode.Search);
        }

        private void SetRepositoryDataKeyFromDetailViewDataKey()
        {
            if (DetailView.CurrentDataKey != null)
                Repository.SetSelectedDataKey(DetailView.CurrentDataKey);
        }

        private string GenerateCrewAssignmentLink(WorkOrderAssignmentEventArgs e)
        {
            return String.Format(CREW_VIEW_URL_FORMAT,
                    String.Format(CREW_VIEW_DATA_FORMAT, e.CrewID,
                        e.Date.ToString("yyyy-MM-dd")));
        }

        private string GenerateEditInGeneralLink(int workOrderID)
        {
            return String.Format(GENERAL_VIEW_EDIT_URL_FORMAT, workOrderID);
        }

        protected override void SetListViewData(Expression<Func<WorkOrder, bool>> filterExpression, string sortExpression)
        {
            if (ResourceView.Phase != WorkOrderPhase.Input)
            {
                base.SetListViewData(filterExpression, sortExpression);
            }
        }

        #endregion

        #region Event Passthroughs

        public override void OnViewLoaded()
        {
            base.OnViewLoaded();

            switch (ResourceView.Phase)
            {
                case WorkOrderPhase.PrePlanning:
                    if (ListView is IWorkOrderPrePlanningListView)
                    {
                        ((IWorkOrderPrePlanningListView)ListView).AssignClicked += ListView_OfficeAssignClicked;
                        ((IWorkOrderPrePlanningListView)ListView).ContractorAssignClicked += ListView_ContractorAssignClicked;
                    }
                    else if (ListView is IWorkOrderMarkoutPlanningListView)
                    {
                        ((IWorkOrderMarkoutPlanningListView)ListView).
                            SaveClicked += ListView_SaveClicked;
                    }
                    break;
                case WorkOrderPhase.Scheduling:
                    ((IWorkOrderSchedulingListView)ListView).AssignClicked +=
                        ListView_AssignClicked;
                    break;
            }
        }

        public override void OnViewInitialized()
        {
            switch (ResourceView.Phase)
            {
                case WorkOrderPhase.Input:
                    InitializeForInput();
                    break;
                case WorkOrderPhase.PrePlanning:
                    InitializeForPrePlanning();
                    break;
                case WorkOrderPhase.Planning:
                    InitializeForPlanning();
                    break;
                case WorkOrderPhase.Scheduling:
                    InitializeForScheduling();
                    break;
                case WorkOrderPhase.Finalization:
                    InitializeForFinalization();
                    break;
                case WorkOrderPhase.StockApproval:
                case WorkOrderPhase.Approval:
                case WorkOrderPhase.OrcomOrderApproval:
                    InitializeForApproval();
                    break;
                case WorkOrderPhase.General:
                    InitializeForGeneralSearchAndEdit();
                    break;
            }
        }

        #endregion

        #region Event Handlers

        #region List View

        private void ListView_AssignClicked(object sender, WorkOrderAssignmentEventArgs e)
        {
            View.Redirect(GenerateCrewAssignmentLink(e));
        }

        private void ListView_OfficeAssignClicked(object sender, OfficeAssignmentEventArgs e)
        {
            SetListViewData();
        }

        private void ListView_ContractorAssignClicked(object sender, OfficeContractorAssignmentEventArgs e)
        {
            SetListViewData();
        }

        private void ListView_SaveClicked(object sender, MarkoutPlanningEventArgs e)
        {
            SetListViewData();
        }

        #endregion

        #region Detail View

        protected override void DetailView_DiscardChangesClicked(object sender, EventArgs e)
        {
            if (ResourceView.Phase == WorkOrderPhase.Input && DetailView.CurrentMode == DetailViewMode.Insert)
                View.Redirect(Configuration.MENU_URL);
            else
                base.DetailView_DiscardChangesClicked(sender, e);
        }

        protected override void DetailView_EditClicked(object sender, EventArgs e)
        {
            if (ResourceView.Phase == WorkOrderPhase.Input)
                SetRepositoryDataKeyFromDetailViewDataKey();
            else
                base.DetailView_EditClicked(sender, e);
        }

        protected override void DetailView_Updating(object sender, EventArgs e)
        {
            switch (ResourceView.Phase)
            {
                case WorkOrderPhase.Input:
                    SetRepositoryDataKeyFromDetailViewDataKey();
                    break;
                case WorkOrderPhase.StockApproval:
                case WorkOrderPhase.OrcomOrderApproval:
                    if (((EntityEventArgs<WorkOrder>)e).Entity.MaterialsApprovedBy == null)
                        goto default;
                    View.SetViewMode(ResourceViewMode.List);
                    View.DataBind();
                    // sorry, this really is how you fall through case
                    // labels in c#.  i didn't want to, i swear
                    goto default;
                default:
                    SetListViewData();
                    SetRepositoryDataKeyFromListViewDataKey();
                    if (Repository.CurrentIndex == -1)
                    {
                        View.SetViewMode(ResourceViewMode.List);
                        View.DataBind();
                    }
                    break;
            }
        }

        #endregion

        #region Search View

        protected override void SearchView_SearchClicked(object sender, EventArgs e)
        {
            switch (WorkOrderSearchView.Phase)
            {
                case WorkOrderPhase.Scheduling:
                case WorkOrderPhase.PrePlanning:
                    if (((IWorkOrderSearchView)SearchView).OperatingCenterID == null)
                    {
                        throw new ArgumentException(
                            "Operating Center was null. You must specify an Operating Center.");
                    }
                    
                    //Set the OperatingCenterID on the list view.
                    ((IWorkOrderListView)ListView).OperatingCenterID = ((IWorkOrderSearchView)SearchView).OperatingCenterID;
                    break;
            }
            
            var count = Repository.GetCountForExpression(SearchView.GenerateExpression());

            if (count == 0)
            {
                WorkOrderSearchView.DisplaySearchError(
                    NO_RECORDS_FOUND_ERROR);
                return;
            }
            if (count > SEARCH_RESULT_LIMIT)
            {
                WorkOrderSearchView.DisplaySearchError(string.Format(OVER_SEARCH_RESULT_LIMIT_ERROR, SEARCH_RESULT_LIMIT));
                return;
            }
            base.SearchView_SearchClicked(sender, e);
        }

        #endregion

        #endregion
    }
}
