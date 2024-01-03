using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using LINQTo271.Views.Abstract;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class IncompleteLeaks : WorkOrdersReport
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

        #region Event Handlers

        protected void gvSearchResults_DataBinding(object sender, EventArgs e)
        {
            var resultView = ((MvpGridView)sender);
            resultView.DataSource = GetWorkOrders(resultView.SortExpression);
        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            lblError.Text = String.Empty;
            base.btnSearch_Click(sender, e);
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
                    wo => wo.DateCompleted == null && wo.CancelledAt == null &&
                          (wo.WorkDescriptionID == 40 ||
                           wo.WorkDescriptionID == 41 ||
                           wo.WorkDescriptionID == 42 ||
                           wo.WorkDescriptionID == 43 ||
                           wo.WorkDescriptionID == 68 ||
                           wo.WorkDescriptionID == 27 ||
                           wo.WorkDescriptionID == 57 ||
                           wo.WorkDescriptionID == 58 ||
                           wo.WorkDescriptionID == 74 ||
                           wo.WorkDescriptionID == 80)
                    );
            /*
                SELECT 40, N'LEAK AT CURB', 4, 1.00, 9, 2 UNION ALL
                SELECT 41, N'LEAK IN STREET', 3, 1.25, 9, 2 UNION ALL
                SELECT 42, N'LEAK IN TILE, INLET', 4, 1.50, 1, 2 UNION ALL
                SELECT 43, N'LEAK IN TILE, OUTLET', 4, 1.75, 1, 2 UNION ALL
                SELECT 68, N'VALVE LEAKING', 1, 2.25, 17, 2 UNION ALL
                SELECT 27, N'HYDRANT LEAKING', 2, 0.75, 4, 2 UNION ALL
                SELECT 57, N'SERVICE LINE LEAK, COMPANY SIDE', 4, 2.00, 12, 2 UNION ALL
                SELECT 58, N'SERVICE LINE LEAK, CUST. SIDE', 4, 1.00, 9, 2 UNION ALL
             */

            if (ddlOperatingCenter.GetSelectedValue() != null)
            {
                builder.And(
                    wo =>
                    wo.OperatingCenterID ==
                    ddlOperatingCenter.GetSelectedValue());
            }

            return builder;
        }

        #endregion
    }
}
