using System;
using System.Linq;
using System.Linq.Expressions;
using LINQTo271.Common;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using WorkOrders.Model;
using WorkOrders.Views.WorkOrders;

namespace LINQTo271.Views.WorkOrders.MarkoutPlanning
{
    public partial class WorkOrderMarkoutPlanningSearchView : WorkOrderSearchView, IWorkOrderSearchView
    {
        #region Control Declarations

        protected ITextBox txtSAPNotificationNumber, txtSAPWorkOrderNumber, txtWBSCharged;

        protected IDropDownList ddlPriority,
            ddlRequestedBy,
            ddlSOPRequirement,
            ddlStreetOpeningPermitRequested,
            ddlStreetOpeningPermitIssued,
            ddlDrivenBy,
            ddlMarkoutRequirement,
            ddlCreatedBy;


        protected IDateRange drDateReceived;

        #endregion

        #region Properties

        protected Expression<Func<WorkOrder, bool>> BaseBaseExpression
            => base.BaseExpression.And(SAPValid);

        public override Expression<Func<WorkOrder, bool>> BaseExpression => _baseExpression ??
                                                                            (_baseExpression = GetBaseExpression());

        public long? SAPNotificationNumber
            => string.IsNullOrWhiteSpace(txtSAPNotificationNumber.Text)
                ? (long?)null : long.Parse(txtSAPNotificationNumber.Text);

        public long? SAPWorkOrderNumber
            => string.IsNullOrWhiteSpace(txtSAPWorkOrderNumber.Text)
                ? (long?)null : long.Parse(txtSAPWorkOrderNumber.Text);

        protected int? PriorityID => ddlPriority.GetSelectedValue();

        public DateTime? DateReceived => drDateReceived.Date;

        protected DateTime? DateReceivedStart => drDateReceived.StartDate;

        protected DateTime? DateReceivedEnd => drDateReceived.EndDate;

        protected virtual int? RequestedByID => ddlRequestedBy.GetSelectedValue();

        protected bool? SOPRequired => ddlSOPRequirement.GetBooleanValue();

        public bool? StreetOpeningPermitRequested => ddlStreetOpeningPermitRequested.GetBooleanValue();

        public bool? StreetOpeningPermitIssued => ddlStreetOpeningPermitIssued.GetBooleanValue();

        public int? PurposeID => ddlDrivenBy.GetSelectedValue();

        protected int? CreatedByID => ddlCreatedBy.GetSelectedValue();

        protected string AccountCharged => txtWBSCharged?.Text;

        // hack so that we only get orders that require a routine markout
        protected virtual int? MarkoutRequirementID => MarkoutRequirementRepository.Indices.ROUTINE;

        // hack to get around some nastyness in the SearchPresenter
        public override WorkOrderPhase Phase => WorkOrderPhase.General;

        #endregion

        #region Private Methods

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

            if (SAPNotificationNumber != null)
                builder.And(wo => wo.SAPNotificationNumber == SAPNotificationNumber);
            if (SAPWorkOrderNumber != null)
                builder.And(wo => wo.SAPWorkOrderNumber == SAPWorkOrderNumber);
            if (RequestedByID != null)
                builder.And(wo => wo.RequesterID == RequestedByID);
            if (MarkoutRequirementID != null)
                builder.And(wo => wo.MarkoutRequirementID == MarkoutRequirementID);
            if (PriorityID != null)
                builder.And(wo => wo.PriorityID == PriorityID);
            if (SOPRequired != null)
                builder.And(wo => wo.StreetOpeningPermitRequired == SOPRequired);
            if (StreetOpeningPermitRequested != null)
                builder.And(wo => wo.StreetOpeningPermits.Any() == StreetOpeningPermitRequested);
            if (StreetOpeningPermitIssued != null)
                builder.And(wo => wo.StreetOpeningPermits.Any(x => x.DateIssued != null) == StreetOpeningPermitIssued);
            if (CreatedByID != null)
                builder.And(wo => wo.CreatorID == CreatedByID);
            if (PurposeID != null)
                builder.And(wo => wo.PurposeID == PurposeID);
            if (!string.IsNullOrWhiteSpace(AccountCharged))
            {
                builder.And(wo => wo.AccountCharged == AccountCharged);
            }
        }

        protected virtual Expression<Func<WorkOrder, bool>> GetBaseExpression()
        {
            return BaseBaseExpression.And(SAPValid)
                .And(wo => wo.DateCompleted == null)
                .And(wo => wo.AssignedContractorID == null)
                .And(wo => wo.MarkoutRequirementID != MarkoutRequirementRepository.Indices.NONE
                           && !(from mo in wo.Markouts
                                where mo.ExpirationDate.Date >= DateTime.Now.Date
                                select mo).Any())
                .And(PredicateBuilder.False<WorkOrder>()
                        .Or(wo => !wo.StreetOpeningPermitRequired)
                        .Or(wo => wo.StreetOpeningPermitRequired 
                        && (from sop in wo.StreetOpeningPermits
                                where sop.DateIssued != null &&
                                sop.ExpirationDate.Value.Date >= DateTime.Now.Date
                                select sop).Any()));
        }

        #endregion
    }
}
