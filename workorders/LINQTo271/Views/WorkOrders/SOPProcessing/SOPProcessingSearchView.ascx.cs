using System;
using System.Linq.Expressions;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.SOPProcessing
{
    public partial class SOPProcessingSearchView : WorkOrderSearchView
    {
        #region Control Declarations

        protected IDropDownList ddlPriority;

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

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Finalization; }
        }

        public int? PriorityID => ddlPriority.GetSelectedValue();

        #endregion

        #region Private Methods

        /// <summary>
        /// Bring back Workorders that have a priority of Emergency and an SOP Requirement but no SOP data entered.
        /// </summary>
        /// <returns></returns>
        private Expression<Func<WorkOrder, bool>> GetBaseExpression()  
        {  
            return base.BaseExpression.And(SAPValid)
                .And(wo => wo.StreetOpeningPermitRequired)
                .And(wo => wo.StreetOpeningPermits.Count == 0);
        }  

        protected override void ApplySearchFilters(ExpressionBuilder<WorkOrder> builder)
        {
            if (PriorityID != null)
                builder.And(wo => wo.PriorityID == PriorityID);
        }

        #endregion
    }
}
