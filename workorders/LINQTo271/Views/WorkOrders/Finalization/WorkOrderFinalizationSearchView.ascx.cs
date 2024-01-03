using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.Finalization
{
    public partial class WorkOrderFinalizationSearchView : WorkOrderSearchView
    {
        #region Control Declarations

        protected ITextBox txtWBSCharged;

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

        public List<int> PurposeIDs
        {
            get { return lstDrivenBy.GetSelectedValues(); }
        }

        protected string AccountCharged => txtWBSCharged?.Text;

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets WorkOrders that AssignedFor is <= today or is an emergency and isn't approved by a supervisor yet.
        /// </summary>
        /// <returns></returns>
        private Expression<Func<WorkOrder, bool>> GetBaseExpression()
        {
            var expr = base.BaseExpression
                .And(SAPValid)
                .And(NotRetiredRemovedOrCancelled)
                .And(wo => wo.AssignedContractorID == null)
                    .And(

                    wo =>
                    (((
                        from a in wo.CrewAssignments
                        where a.AssignedFor.Date <= DateTime.Today.Date || a.DateStarted != null select a).Any()) 
                              || (wo.PriorityID == WorkOrderPriorityRepository.Indices.EMERGENCY)
                              || (wo.AssetTypeID == AssetTypeRepository.Indices.EQUIPMENT)
                    )
                    && wo.ApprovedOn == null && wo.ApprovedByID == null
                    );
            return expr;
        }

        protected override void ApplySearchFilters(ExpressionBuilder<WorkOrder> builder)
        {
            if (PurposeIDs != null && PurposeIDs.Any())
                builder.And(wo => PurposeIDs.Contains(wo.PurposeID));
            if (!string.IsNullOrWhiteSpace(AccountCharged))
            {
                builder.And(wo => wo.AccountCharged == AccountCharged);
            }
        }

        #endregion
    }
}
