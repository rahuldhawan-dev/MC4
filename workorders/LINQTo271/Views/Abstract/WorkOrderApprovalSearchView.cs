using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LINQTo271.Common;
using MMSINC.Common;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.Abstract
{
    public abstract class WorkOrderApprovalSearchView : WorkOrderSearchView
    {
        #region Control Declarations

        protected IDateRange drDateCompleted;
        protected IDropDownList ddlCrew;

        #endregion

        #region Properties

        #region New Properties

        public virtual int? CrewID
        {
            get { return ddlCrew.GetSelectedValue(); }
        }

        public virtual DateTime? DateCompleted
        {
            get { return drDateCompleted.Date; }
        }

        public virtual DateTime? DateCompletedStart
        {
            get { return drDateCompleted.StartDate; }
        }

        public virtual DateTime? DateCompletedEnd
        {
            get { return drDateCompleted.EndDate; }
        }

        #endregion

        #region Overrides

        public override sealed Expression<Func<WorkOrder, bool>> BaseExpression
        {
            get
            {
                if (_baseExpression == null)
                    _baseExpression = GetBaseExpression();
                return _baseExpression;
            }
        }

        protected Expression<Func<WorkOrder, bool>> BaseBaseExpression
            => base.BaseExpression;

        public override WorkOrderPhase Phase
        {
            get { return WorkOrderPhase.Approval; }
        }

        public override sealed int? AssetTypeID
        {
            get { return null; }
        }

        public override sealed List<int> DescriptionOfWorkIDs
        {
            get { return null; }
        }

        public override sealed int? NearestCrossStreetID
        {
            get { return null; }
        }

        public override sealed int? StreetID
        {
            get { return null; }
        }

        public override sealed string StreetNumber
        {
            get { return null; }
        }

        public override sealed int? TownSectionID
        {
            get { return null; }
        }

        #endregion

        #endregion

        #region Event Handlers

        protected override sealed void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
        }

        #endregion

        #region Private Members

        protected sealed override void ApplyCommonSearchFilters(ExpressionBuilder<WorkOrder> builder)
        {
            if (OperatingCenterID != null)
                builder.And(wo => wo.OperatingCenterID == OperatingCenterID);
            if (TownID != null)
                builder.And(wo => wo.TownID == TownID);
            if (CrewID != null)
            {
                builder.And(
                    wo => (from ca in wo.CrewAssignments
                           where
                               ca.AssignedFor.Date ==
                               (from ca2 in wo.CrewAssignments
                                select ca2.AssignedFor.Date).Max()
                           select ca).Take(1).SingleOrDefault().CrewID == CrewID);
            }

            if (DateCompleted != null)
            {
                switch (drDateCompleted.SelectedOperator)
                {
                    case "=":
                        builder.And(wo => wo.DateCompleted.Value.Date == DateCompleted.Value.Date);
                        break;
                    case ">":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value.Date, DateCompleted.Value.Date) > 0);
                        break;
                    case ">=":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value.Date, DateCompleted.Value.Date) >= 0);
                        break;
                    case "<":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value.Date, DateCompleted.Value.Date) < 0);
                        break;
                    case "<=":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value.Date, DateCompleted.Value.Date) <= 0);
                        break;
                }
            }
            else if (DateCompletedStart != null && DateCompletedEnd != null)
                builder.And(
                        wo =>
                        (DateTime.Compare(wo.DateCompleted.Value.Date, DateCompletedStart.Value.Date) >= 0 &&
                         DateTime.Compare(wo.DateCompleted.Value.Date, DateCompletedEnd.Value.Date) <= 0));
        }

        #endregion

        #region Exposed Methods

        public override sealed void DisplaySearchError(string message)
        {
            base.DisplaySearchError(message);
        }

        #endregion

        #region Abstract Methods

        protected abstract Expression<Func<WorkOrder, bool>>
            GetBaseExpression();

        #endregion
    }
}
