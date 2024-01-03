using System;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.StockToIssue
{
    public partial class WorkOrderStockToIssueSearchView : WorkOrderApprovalSearchView
    {
        #region Constants

        public static readonly string[] VALID_SAP_ERROR_CODES = new[] {
            "SUCCESS", "POSTING PERIOD", "DEFICIT", "POSTING ONLY POSSIBLE"
        };

        #endregion

        #region Control Declarations

        protected IDropDownList ddlMaterialsApproved;

        #endregion

        #region Properties

        public bool? MaterialsApproved
        {
            get { return ddlMaterialsApproved.GetBooleanValue(); }
        }

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.StockApproval; }
        }

        #endregion

        #region Private Methods

        protected override Expression<Func<WorkOrder, bool>> GetBaseExpression()
        {
            return BaseBaseExpression.And(SAPValid)
                .And(wo => wo.ApprovedBy != null)
                .And(wo => wo.MaterialsUseds.Any());
        }

        protected override void ApplySearchFilters(ExpressionBuilder<WorkOrder> builder)
        {
            if (MaterialsApproved != null)
            {
                if (MaterialsApproved.Value)
                    builder.And(wo => wo.MaterialsApprovedBy != null);
                else
                    builder.And(wo => wo.MaterialsApprovedBy == null);
            }
        }

        #endregion
    }
}
