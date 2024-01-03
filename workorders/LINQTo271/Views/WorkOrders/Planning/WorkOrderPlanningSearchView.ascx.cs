using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LINQTo271.Common;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using SecurityServiceClass = WorkOrders.Library.Permissions.SecurityService;

namespace LINQTo271.Views.WorkOrders.Planning
{
    public partial class WorkOrderPlanningSearchView : WorkOrderSearchView
    {
        #region Control Declarations

        protected IDropDownList ddlPriority,
                                ddlRequestedBy,
                                ddlAcousticMonitoringType,
                                ddlSOPRequirement,
                                ddlStreetOpeningPermitRequested,
                                ddlStreetOpeningPermitIssued,
                                ddlMarkoutRequirement,
                                ddlOfficeAssignment;

        protected IDateRange drDateReceived;
        protected IPanel pnlAssignedTo;
        protected ICheckBox chkMarkoutToBeCalled;
        protected IListBox lstDrivenBy;
        protected ITextBox txtWBSCharged, txtNotes;

        #endregion

        #region Private Members

        protected ISecurityService _securityService;

        #endregion

        #region Properties

        public override Expression<Func<WorkOrder, bool>> BaseExpression
        {
            get
            {
                if (_baseExpression == null)
                    _baseExpression = GetBaseExpression();
                return _baseExpression;
            }
        }

        protected int? PriorityID
        {
            get { return ddlPriority.GetSelectedValue(); }
        }

        public DateTime? DateReceived
        {
            get { return drDateReceived.Date; }
        }

        protected DateTime? DateReceivedStart
        {
            get { return drDateReceived.StartDate; }
        }

        protected DateTime? DateReceivedEnd
        {
            get { return drDateReceived.EndDate; }
        }

        protected int? MarkoutRequirementID
        {
            get { return ddlMarkoutRequirement.GetSelectedValue(); }
        }

        protected int? OfficeAssignedToID
        {
            get { return ddlOfficeAssignment.GetSelectedValue(); }
        }

        public List<int> PurposeIDs
        {
            get { return lstDrivenBy.GetSelectedValues(); }
        }

        protected int? RequestedByID
        {
            get { return ddlRequestedBy.GetSelectedValue(); }
        }
        protected int? AcoustingMonitoringTypeID
        {
            get { return ddlAcousticMonitoringType.GetSelectedValue(); }
        }

        protected bool? SOPRequired
        {
            get { return ddlSOPRequirement.GetBooleanValue(); }
        }

        public bool? StreetOpeningPermitRequested => ddlStreetOpeningPermitRequested.GetBooleanValue();

        public bool? StreetOpeningPermitIssued => ddlStreetOpeningPermitIssued.GetBooleanValue();

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Planning; }
        }

        protected ISecurityService SecurityService
        {
            get
            {
                if (_securityService == null)
                    _securityService = SecurityServiceClass.Instance;
                return _securityService;
            }
        }

        protected bool MarkoutToBeCalled
        {
            get { return chkMarkoutToBeCalled.Checked; }
        }

        protected string AccountCharged => txtWBSCharged?.Text;
        protected string Notes => txtNotes.Text;

        #endregion

        #region Private Methods

        private Expression<Func<WorkOrder, bool>> GetBaseExpression()
        {
            //Return only work orders in planning phase...
            var predicate = base.BaseExpression
                .And(SAPValid)
                .And(NotRetiredRemovedOrCancelled)
                .And(wo => wo.DateCompleted == null)
                .And(PredicateBuilder.False<WorkOrder>()
                    // has markout requirement and no markout
                    .Or(wo => 
                        wo.MarkoutRequirementID != MarkoutRequirementRepository.Indices.NONE 
                        &&
                        (from mo in wo.Markouts
                         where mo.ExpirationDate.Date >= DateTime.Now.Date
                         select mo).Count() == 0)
                    // has a sop requirement and no valid sop
                    .Or(wo =>
                        wo.StreetOpeningPermitRequired
                        &&
                        (from sop in wo.StreetOpeningPermits
                         where sop.DateIssued != null &&
                         sop.ExpirationDate.Value.Date >= DateTime.Now.Date
                         select sop).Count() == 0)
                    );

            if (!SecurityService.IsAdmin)
            {
                predicate = predicate.And(
                    wo =>
                    wo.OfficeAssignmentID ==
                    SecurityService.GetEmployeeID());
            }

            return predicate;
        }

        private void AddDateReceivedFilter(ExpressionBuilder<WorkOrder> builder)
        {
            if (DateReceived != null)
            {
                switch (drDateReceived.SelectedOperator)
                {
                    case "=":
                        builder.And(wo => wo.DateReceived == DateReceived);
                        break;
                    case ">":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateReceived.Value) > 0);
                        break;
                    case ">=":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateReceived.Value) >= 0);
                        break;
                    case "<":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateReceived.Value) < 0);
                        break;
                    case "<=":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateReceived.Value) <= 0);
                        break;
                }
            }
            else if (DateReceivedStart != null && DateReceivedEnd != null)
                builder.And(
                        wo =>
                        (DateTime.Compare(wo.DateReceived.Value, DateReceivedStart.Value) >= 0 &&
                         DateTime.Compare(wo.DateReceived.Value, DateReceivedEnd.Value) <= 0));
        }

        protected override void ApplySearchFilters(ExpressionBuilder<WorkOrder> builder)
        {
            AddDateReceivedFilter(builder);

            if (!string.IsNullOrWhiteSpace(Notes))
                builder.And(wo => wo.Notes.Contains(Notes));
            if (RequestedByID != null)
                builder.And(wo => wo.RequesterID == RequestedByID);
            if (AcoustingMonitoringTypeID != null)
                builder.And(wo => wo.AcousticMonitoringTypeId == AcoustingMonitoringTypeID);
            if (MarkoutRequirementID != null)
                builder.And(wo => wo.MarkoutRequirementID == MarkoutRequirementID);
            if(PriorityID != null)
                builder.And(wo => wo.PriorityID == PriorityID);
            if (SOPRequired != null)
                builder.And(wo => wo.StreetOpeningPermitRequired == SOPRequired);
            if (StreetOpeningPermitRequested != null)
                builder.And(wo => wo.StreetOpeningPermits.Any() == StreetOpeningPermitRequested);
            if (StreetOpeningPermitIssued != null)
                builder.And(wo => wo.StreetOpeningPermits.Any(x => x.DateIssued != null) == StreetOpeningPermitIssued);
            if (PurposeIDs != null && PurposeIDs.Any())
                builder.And(wo => PurposeIDs.Contains(wo.PurposeID));
            if (OfficeAssignedToID != null)
                builder.And(wo => wo.OfficeAssignmentID == OfficeAssignedToID);
            if (MarkoutToBeCalled)
                builder.And(
                    wo =>
                    wo.MarkoutToBeCalled.HasValue &&
                    wo.MarkoutToBeCalled.Value.Date == DateTime.Today);
            if (!string.IsNullOrWhiteSpace(AccountCharged))
            {
                builder.And(wo => wo.AccountCharged == AccountCharged);
            }
        }

        #endregion

        #region Exposed Methods

        protected override void Page_Prerender(object sender, EventArgs e)
        {
            base.Page_Prerender(sender, e);
            pnlAssignedTo.Visible = SecurityService.IsAdmin;
        }

        #endregion
    }
}
