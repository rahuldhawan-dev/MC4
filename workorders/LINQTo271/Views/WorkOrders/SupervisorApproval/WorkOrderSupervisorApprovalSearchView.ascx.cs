using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.SupervisorApproval
{
    public partial class WorkOrderSupervisorApprovalSearchView : WorkOrderApprovalSearchView
    {
        #region Control Declarations

        protected IDropDownList ddlApproved, ddlRequestedBy;
        protected IListBox lstDrivenBy;

        #endregion

        #region Properties

        public bool? Approved
        {
            get { return ddlApproved.GetBooleanValue(); }
        }

        public List<int> PurposeIDs
        {
            get { return lstDrivenBy.GetSelectedValues(); }
        }

        public int? RequestedByID
        {
            get { return ddlRequestedBy.GetSelectedValue(); }
        }

        #endregion

        #region Private Methods

        protected override Expression<Func<WorkOrder, bool>> GetBaseExpression()
        {
            return BaseBaseExpression.And(SAPValid)
                .And(wo => wo.DateCompleted != null);
        }

        protected override void ApplySearchFilters(ExpressionBuilder<WorkOrder> builder)
        {
            if (Approved != null)
            {
                if (Approved.Value)
                    builder.And(wo => wo.ApprovedBy != null);
                else
                    builder.And(wo => wo.ApprovedBy == null);
            }
            if (RequestedByID != null)
                builder.And(wo => wo.RequesterID == RequestedByID);
            if (PurposeIDs != null && PurposeIDs.Any())
                builder.And(wo => PurposeIDs.Contains(wo.PurposeID));

        }

        #endregion
    }
}
