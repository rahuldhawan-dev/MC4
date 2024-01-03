using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LINQTo271.Common;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class MaterialsUsed1 : WorkOrdersReport
    {
        #region Private Members

        private IRepository<MaterialsUsed> _repository;

        #endregion

        #region Properties

        protected IRepository<MaterialsUsed> Repository
        {
            get
            {
                if (_repository == null)
                    _repository =
                        DependencyResolver.Current.GetService<IRepository<MaterialsUsed>>();
                return _repository;
            }
        }

        #endregion

        #region Private Methods

        private IEnumerable<MaterialsUsed> GetMaterialsUsed(string sortExpression)
        {
            return Repository.GetFilteredSortedData(GetFilterExpression(), sortExpression);
        }

        private Expression<Func<MaterialsUsed, bool>> GetFilterExpression()
        {
            var builder =
                new ExpressionBuilder<MaterialsUsed>(m => m.MaterialID != null);

            if (ddlOperatingCenter.GetSelectedValue() != null)
            {
                builder.And(
                    m =>
                        m.WorkOrder.OperatingCenterID == ddlOperatingCenter.GetSelectedValue());
            }

            if (ddlStockLocation.GetSelectedValue() != null)
            {
                builder.And(
                    m =>
                    m.StockLocationID == ddlStockLocation.GetSelectedValue());
            }

            if (drDateCompleted.HasEndDate)
            {
                switch(drDateCompleted.SelectedOperator)
                {
                    case DateRange.Operators.BETWEEN:
                        builder.And(
                            mu =>
                            mu.WorkOrder.DateCompleted >= drDateCompleted.StartDate &&
                            mu.WorkOrder.DateCompleted <= drDateCompleted.EndDate);
                        break;
                    case DateRange.Operators.EQUALS:
                        builder.And(
                            mu => mu.WorkOrder.DateCompleted == drDateCompleted.Date);
                        break;
                    case DateRange.Operators.GREATER_THAN:
                        builder.And(
                            mu => mu.WorkOrder.DateCompleted > drDateCompleted.Date);
                        break;
                    case DateRange.Operators.GREATER_THAN_OR_EQUAL_TO:
                        builder.And(
                            mu => mu.WorkOrder.DateCompleted >= drDateCompleted.Date);
                        break;
                    case DateRange.Operators.LESS_THAN:
                        builder.And(
                            mu => mu.WorkOrder.DateCompleted < drDateCompleted.Date);
                        break;
                    case DateRange.Operators.LESS_THAN_OR_EQUAL_TO:
                        builder.And(
                            mu => mu.WorkOrder.DateCompleted <= drDateCompleted.Date);
                        break;
                }
            }
            
            return builder;
        }

        #endregion

        #region Event Handlers

        protected void gvSearchResults_DataBinding(object sender, EventArgs e)
        {
            var resultView = ((MvpGridView)sender);
            if (resultView.DataSource == null)
            {
                resultView.DataSource = GetMaterialsUsed(null);
            }
        }

        protected void gvSearchResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            var resultView = ((MvpGridView)sender);
            resultView.DataSource = GetMaterialsUsed(GetSortExpression(e.SortExpression));
            resultView.DataBind();
        }

        #endregion
    }
}