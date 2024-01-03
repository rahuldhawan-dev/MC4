using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.ClassExtensions;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class PowerPlantAsbuiltReport : WorkOrdersReport
    {
        public static Dictionary<string, Func<WorkOrder, object>>  SORT_COLUMNS =
            new Dictionary<string, Func<WorkOrder, object>> {
                {"AssetType", w => w.AssetType.Description},
                {"WorkOrderId", w => w.WorkOrderID},
                {"SAPWorkOrderNumber", w => w.SAPWorkOrderNumber },
                {"AccountCharged", w => w.AccountCharged},
                {"StreetAddress", w => w.StreetAddress},
                {"Town", w => w.Town.ToString()},
                {"AssetId", w => w.AssetID},
                {"DateCompleted", w => w.DateCompleted},
                {"WorkDescription", w => w.WorkDescription.Description},
            };

        protected DateTime DateStart
        {
            get { return DateTime.Parse(txtDateStart.Text); }
        }

        protected DateTime DateEnd
        {
            get { return DateTime.Parse(txtDateEnd.Text); }
        }

        protected int OperatingCenterID
        {
            get { return ddlOpCode.GetSelectedValue().Value; }
        }

        protected override void DataBindResults()
        {
            gvSearchResults.DataSource =
                WorkOrderRepository.GetCapitalWorkOrdersByOpCenterAndDateRange(
                    DateStart, DateEnd, OperatingCenterID);
            base.DataBindResults();
        }

        protected void gvSearchResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SortExpression == e.SortExpression && SortDirection == System.Web.UI.WebControls.SortDirection.Ascending.ToString())
            {
                SortDirection =
                    System.Web.UI.WebControls.SortDirection.Descending.ToString();
            }
            else
            {
                SortDirection =
                    System.Web.UI.WebControls.SortDirection.Ascending.ToString();
            }

            gvSearchResults.DataSource =
                WorkOrderRepository
                    .GetCapitalWorkOrdersByOpCenterAndDateRange(
                        DateStart, DateEnd, OperatingCenterID,
                        SORT_COLUMNS[e.SortExpression], SortDirection);
            gvSearchResults.DataBind();
            SortExpression = e.SortExpression;
        }
    }
}
