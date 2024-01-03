using System;
using System.Collections.Generic;
using System.Web.Mvc;
using LINQTo271.Common;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;
using PredicateBuilder = MMSINC.Common.PredicateBuilder;

namespace LINQTo271.Views.Reports
{
    public partial class DSICMainBreaks : WorkOrdersReport
    {
        #region Control Declarations

        protected IDropDownList ddlOpCode, ddlTown, ddlMainBreakMaterial;
        protected IDateRange drDateReceived, drDateCompleted;

        #endregion

        #region Private Members

        private IRepository<MainBreak> _repository;

        #endregion

        #region Properties

        protected IRepository<MainBreak> Repository
        {
            get
            {
                if (_repository == null)
                    _repository =
                        DependencyResolver.Current.GetService<IRepository<MainBreak>>();
                return _repository;
            }
        }

        #endregion

        #region Private Methods

        protected IEnumerable<MainBreak> BuildReportDataSource()
        {
            var builder =
                new ExpressionBuilder<MainBreak>(mb => mb.WorkOrder.CancelledAt == null);

            ApplyOpCodeParamaters(builder);
            ApplyDateReceivedParameters(builder);
            ApplyDateCompletedParameters(builder);
            ApplyTownParameters(builder);
            ApplyMaterialParameters(builder);

            return Repository.GetFilteredSortedData(builder, "");
        }

        private void ApplyMaterialParameters(ExpressionBuilder<MainBreak> builder)
        {
            var value = ddlMainBreakMaterial.GetSelectedValue();
            if (value != null)
            {
                builder.And(mb => mb.MainBreakMaterialID == value);
            }
        }

        private void ApplyTownParameters(ExpressionBuilder<MainBreak> builder)
        {
            var value = ddlTown.GetSelectedValue();
            if (value != null)
            {
                builder.And(mb => mb.WorkOrder.TownID == value);
            }
        }

        private void ApplyDateCompletedParameters(ExpressionBuilder<MainBreak> builder)
        {
            if (!drDateCompleted.HasEndDate)
            {
                return;
            }

            switch (drDateCompleted.SelectedOperator)
            {
                case DateRange.Operators.BETWEEN:
                    builder.And(
                        mb =>
                        mb.WorkOrder.DateCompleted.Value.Date >= drDateCompleted.StartDate &&
                        mb.WorkOrder.DateCompleted.Value.Date <= drDateCompleted.EndDate);
                    break;
                case DateRange.Operators.EQUALS:
                    builder.And(
                        mb => mb.WorkOrder.DateCompleted.Value.Date == drDateCompleted.Date);
                    break;
                case DateRange.Operators.GREATER_THAN:
                    builder.And(
                        mb => mb.WorkOrder.DateCompleted.Value.Date > drDateCompleted.Date);
                    break;
                case DateRange.Operators.GREATER_THAN_OR_EQUAL_TO:
                    builder.And(
                        mb => mb.WorkOrder.DateCompleted.Value.Date >= drDateCompleted.Date);
                    break;
                case DateRange.Operators.LESS_THAN:
                    builder.And(
                        mb => mb.WorkOrder.DateCompleted.Value.Date < drDateCompleted.Date);
                    break;
                case DateRange.Operators.LESS_THAN_OR_EQUAL_TO:
                    builder.And(
                        mb => mb.WorkOrder.DateCompleted.Value.Date <= drDateCompleted.Date);
                    break;
            }
        }

        private void ApplyDateReceivedParameters(ExpressionBuilder<MainBreak> builder)
        {
            if (!drDateReceived.HasEndDate)
            {
                return;
            }

            switch (drDateReceived.SelectedOperator)
            {
                case DateRange.Operators.BETWEEN:
                    builder.And(
                        mb =>
                        mb.WorkOrder.DateReceived.Value.Date >= drDateReceived.StartDate &&
                        mb.WorkOrder.DateReceived.Value.Date <= drDateReceived.EndDate);
                    break;
                case DateRange.Operators.EQUALS:
                    builder.And(
                        mb => mb.WorkOrder.DateReceived.Value.Date == drDateReceived.Date);
                    break;
                case DateRange.Operators.GREATER_THAN:
                    builder.And(
                        mb => mb.WorkOrder.DateReceived.Value.Date > drDateReceived.Date);
                    break;
                case DateRange.Operators.GREATER_THAN_OR_EQUAL_TO:
                    builder.And(
                        mb => mb.WorkOrder.DateReceived.Value.Date >= drDateReceived.Date);
                    break;
                case DateRange.Operators.LESS_THAN:
                    builder.And(
                        mb => mb.WorkOrder.DateReceived.Value.Date < drDateReceived.Date);
                    break;
                case DateRange.Operators.LESS_THAN_OR_EQUAL_TO:
                    builder.And(
                        mb => mb.WorkOrder.DateReceived.Value.Date <= drDateReceived.Date);
                    break;
            }
        }

        private void ApplyOpCodeParamaters(ExpressionBuilder<MainBreak> builder)
        {
            var value = ddlOpCode.GetSelectedValue();
            if (value != null)
            {
                builder.And(mb => mb.WorkOrder.OperatingCenterID == value);
            }
        }

        #endregion

        #region Event Handlers

        protected void gvSearchResults_DataBinding(object sender, EventArgs e)
        {
            ((IGridView)sender).DataSource = BuildReportDataSource();
        }

        #endregion
    }
}
