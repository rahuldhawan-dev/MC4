using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class IncompleteWorkOrdersWithMaterials : WorkOrdersReport
    {
        #region Private Members

        private IRepository<WorkOrder> _repository;

        #endregion

        #region Properties

        protected IRepository<WorkOrder> Repository
        {
            get
            {
                if (_repository == null)
                    _repository =
                        DependencyResolver.Current.GetService<IRepository<WorkOrder>>();
                return _repository;
            }
        }

        #endregion

        #region Private Methods

        private IEnumerable<WorkOrder> GetWorkOrders(string sortExpression)
        {
            return Repository.GetFilteredSortedData(GetFilterExpression(), sortExpression);
        }

        private Expression<Func<WorkOrder, bool>> GetFilterExpression()
        {
            var builder =
                new ExpressionBuilder<WorkOrder>(
                    wo => wo.DateCompleted == null && wo.CancelledAt == null);

            if (ddlOperatingCenter.GetSelectedValue() != null)
            {
                builder.And(
                    wo => 
                        wo.OperatingCenterID == ddlOperatingCenter.GetSelectedValue() &&
                        wo.MaterialsUseds.Count > 0);
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
                resultView.DataSource = GetWorkOrders(null);
            }
        }

        protected void gvSearchResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            var resultView = ((MvpGridView)sender);
            resultView.DataSource = GetWorkOrders(GetSortExpression(e.SortExpression));
            resultView.DataBind();
        }

        #endregion
    }
}
