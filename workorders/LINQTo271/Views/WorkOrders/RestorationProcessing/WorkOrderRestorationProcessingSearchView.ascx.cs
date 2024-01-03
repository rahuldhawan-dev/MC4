using System;
using System.Linq;
using System.Linq.Expressions;
using LINQTo271.Common;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.WorkOrders.RestorationProcessing
{
    public partial class WorkOrderRestorationProcessingSearchView : WorkOrderSearchView
    {
        #region Properties

        public override Expression<Func<WorkOrder, bool>> BaseExpression => _baseExpression ??
                                                                            (_baseExpression = GetBaseExpression());

        public override WorkOrderPhase Phase => WorkOrderPhase.Finalization;

        public int? CrewID => ddlCrew.GetSelectedValue();

        public DateTime? DateToSearch => drDateToSearch.Date;

        public DateTime? DateToSearchStart => drDateToSearch.StartDate;

        public DateTime? DateToSearchEnd => drDateToSearch.EndDate;

        #endregion

        #region Private Methods

        // Not exactly the DRYest code here :/

        private void AddDateReceivedFilter(ExpressionBuilder<WorkOrder> builder)
        {
            if (DateToSearch != null)
            {
                switch (drDateToSearch.SelectedOperator)
                {
                    case "=":
                        builder.And(wo => wo.DateReceived == DateToSearch);
                        break;
                    case ">":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateToSearch.Value) > 0);
                        break;
                    case ">=":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateToSearch.Value) >= 0);
                        break;
                    case "<":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateToSearch.Value) < 0);
                        break;
                    case "<=":
                        builder.And(wo => DateTime.Compare(wo.DateReceived.Value, DateToSearch.Value) <= 0);
                        break;
                }
            }
            else if (DateToSearchStart != null && DateToSearchEnd != null)
                builder.And(
                        wo =>
                        (DateTime.Compare(wo.DateReceived.Value, DateToSearchStart.Value) >= 0 &&
                         DateTime.Compare(wo.DateReceived.Value, DateToSearchEnd.Value) <= 0));
        }

        private void AddDateCompletedFilter(ExpressionBuilder<WorkOrder> builder)
        {
            if (DateToSearch != null)
            {
                switch (drDateToSearch.SelectedOperator)
                {
                    case "=":
                        builder.And(wo => wo.DateCompleted == DateToSearch);
                        break;
                    case ">":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value, DateToSearch.Value) > 0);
                        break;
                    case ">=":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value, DateToSearch.Value) >= 0);
                        break;
                    case "<":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value, DateToSearch.Value) < 0);
                        break;
                    case "<=":
                        builder.And(wo => DateTime.Compare(wo.DateCompleted.Value, DateToSearch.Value) <= 0);
                        break;
                }
            }
            else if (DateToSearchStart != null && DateToSearchEnd != null)
                builder.And(
                        wo =>
                        (DateTime.Compare(wo.DateCompleted.Value, DateToSearchStart.Value) >= 0 &&
                         DateTime.Compare(wo.DateCompleted.Value, DateToSearchEnd.Value) <= 0));
        }

        private void AddDateDocumentAttachedFilter(ExpressionBuilder<WorkOrder> builder)
        {
            if (DateToSearch != null)
            {
                switch (drDateToSearch.SelectedOperator)
                {
                    case "=":
                        builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                           where
                                               DateToSearch.Value.Date ==
                                               dwo.Document.CreatedOn.Date
                                           select dwo).Any());
                        break;
                    case ">":
                        builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                           where
                                               DateTime.Compare(
                                                   dwo.Document.CreatedOn.Date,
                                                   DateToSearch.Value.Date) > 0
                                           select dwo).Any());
                        break;
                    case ">=":
                        builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                           where
                                               DateTime.Compare(
                                                   dwo.Document.CreatedOn.Date,
                                                   DateToSearch.Value.Date) >= 0
                                           select dwo).Any());
                        break;
                    case "<":
                        builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                           where
                                               DateTime.Compare(
                                                   dwo.Document.CreatedOn.Date,
                                                   DateToSearch.Value.Date) < 0
                                           select dwo).Any());
                        break;
                    case "<=":
                        builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                           where
                                               DateTime.Compare(
                                                   dwo.Document.CreatedOn.Date,
                                                   DateToSearch.Value.Date) <= 0
                                           select dwo).Any());
                        break;
                }
            }
            else if (DateToSearchStart != null && DateToSearchEnd != null)
                builder.And(wo => (from dwo in wo.DocumentsWorkOrders
                                   where
                                       (DateTime.Compare(
                                            dwo.Document.CreatedOn.Date,
                                            DateToSearchStart.Value.Date) >= 0 &&
                                        DateTime.Compare(
                                            dwo.Document.CreatedOn.Date,
                                            DateToSearchEnd.Value.Date) <= 0)
                                   select dwo).Any());
        }

        private void AddInitialDateFilter(ExpressionBuilder<WorkOrder> builder)
        {
            if (DateToSearch != null)
            {
                switch (drDateToSearch.SelectedOperator)
                {
                    case "=":
                        builder.And(wo => (from res in wo.Restorations
                                           where
                                               DateToSearch.Value.Date ==
                                               res.PartialRestorationDate.Value.Date
                                           select res).Any());
                        break;
                    case ">":
                        builder.And(wo => (from res in wo.Restorations
                                           where
                                               DateTime.Compare(
                                               res.PartialRestorationDate.Value.Date,
                                                   DateToSearch.Value.Date) > 0
                                           select res).Any());
                        break;
                    case ">=":
                        builder.And(wo => (from res in wo.Restorations
                                           where
                                               DateTime.Compare(
                                               res.PartialRestorationDate.Value.Date,
                                                   DateToSearch.Value.Date) >= 0
                                           select res).Any());
                        break;
                    case "<":
                        builder.And(wo => (from res in wo.Restorations
                                           where
                                               DateTime.Compare(
                                               res.PartialRestorationDate.Value.Date,
                                                   DateToSearch.Value.Date) < 0
                                           select res).Any());
                        break;
                    case "<=":
                        builder.And(wo => (from res in wo.Restorations
                                           where
                                               DateTime.Compare(
                                               res.PartialRestorationDate.Value.Date,
                                                   DateToSearch.Value.Date) <= 0
                                           select res).Any());
                        break;
                }
            }
            else if (DateToSearchStart != null && DateToSearchEnd != null)
                builder.And(wo => (from res in wo.Restorations
                                   where
                                       (DateTime.Compare(
                                            res.PartialRestorationDate.Value.
                                            Date,
                                            DateToSearchStart.Value.Date) >= 0 &&
                                        DateTime.Compare(
                                            res.PartialRestorationDate.Value.
                                            Date,
                                            DateToSearchEnd.Value.Date) <= 0)
                                   select res).Any());
        }

        private void AddFinalDateFilter(ExpressionBuilder<WorkOrder> builder)
        {
            if (DateToSearch != null)
            {
                switch (drDateToSearch.SelectedOperator)
                {
                    case "=":
                        builder.And(wo => (from res in wo.Restorations
                                           where
                                               DateToSearch.Value.Date ==
                                               res.FinalRestorationDate.Value.Date
                                           select res).Any());
                        break;
                    case ">":
                        builder.And(wo => (from res in wo.Restorations
                                           where
                                               DateTime.Compare(
                                               res.FinalRestorationDate.Value.Date,
                                                   DateToSearch.Value.Date) > 0
                                           select res).Any());
                        break;
                    case ">=":
                        builder.And(wo => (from res in wo.Restorations
                                           where
                                               DateTime.Compare(
                                               res.FinalRestorationDate.Value.Date,
                                                   DateToSearch.Value.Date) >= 0
                                           select res).Any());
                        break;
                    case "<":
                        builder.And(wo => (from res in wo.Restorations
                                           where
                                               DateTime.Compare(
                                               res.FinalRestorationDate.Value.Date,
                                                   DateToSearch.Value.Date) < 0
                                           select res).Any());
                        break;
                    case "<=":
                        builder.And(wo => (from res in wo.Restorations
                                           where
                                               DateTime.Compare(
                                               res.FinalRestorationDate.Value.Date,
                                                   DateToSearch.Value.Date) <= 0
                                           select res).Any());
                        break;
                }
            }
            else if (DateToSearchStart != null && DateToSearchEnd != null)
                builder.And(wo => (from res in wo.Restorations
                                   where
                                       (DateTime.Compare(
                                            res.FinalRestorationDate.Value.
                                            Date,
                                            DateToSearchStart.Value.Date) >= 0 &&
                                        DateTime.Compare(
                                            res.FinalRestorationDate.Value.
                                            Date,
                                            DateToSearchEnd.Value.Date) <= 0)
                                   select res).Any());
        }

        private Expression<Func<WorkOrder, bool>> GetBaseExpression()
        {
            return base.BaseExpression.And(SAPValid)
                .And(
                    wo =>
                    (from a in wo.Restorations
                     select a).Any());
        }

        protected override void ApplySearchFilters(ExpressionBuilder<WorkOrder> builder)
        {
            switch (ddlDateType.SelectedValue)
            {
                case "DateCompleted":
                    AddDateCompletedFilter(builder);
                    break;
                case "DateReceived":
                    AddDateReceivedFilter(builder);
                    break;
                case "DateDocumentAttached":
                    AddDateDocumentAttachedFilter(builder);
                    break;
                case "InitialDate":
                    AddInitialDateFilter(builder);
                    break;
                case "FinalDate":
                    AddFinalDateFilter(builder);
                    break;
            }

            if (CrewID != null)
            {
                builder.And(
                    wo => (from ca in wo.CrewAssignments
                           where
                               ca.AssignedFor.Date ==
                               (from ca2 in wo.CrewAssignments
                                select ca2.AssignedFor.Date).Max()
                           select ca).SingleOrDefault().CrewID == CrewID
                    );
            }

        }

        #endregion

        #region Control Declarations

        protected IDateRange drDateToSearch;
        protected IDropDownList ddlDateType;

        #endregion
    }
}
