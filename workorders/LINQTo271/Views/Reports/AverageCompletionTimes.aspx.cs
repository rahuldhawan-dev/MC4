using System;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class AverageCompletionTimes : WorkOrdersReport
    {
        #region Event Handlers

        protected void gvSearchResults_DataBinding(object sender, EventArgs e)
        {
            ((MvpGridView)sender).DataSource =
                CrewAssignmentRepository.
                    GetWorkOrderTimeAverages(
                        DateTime.Parse(txtDateStart.Text),
                        DateTime.Parse(txtDateEnd.Text),
                        int.Parse(ddlOperatingCenter.SelectedValue)
                        );
        }

        protected void gvSearchResults_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void gvSearchResults_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Add another header cell with the selected date range.
            if (e.Row.RowType == DataControlRowType.Header)
            {
                var gvr = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
                var th = new TableHeaderCell
                {
                    ColumnSpan = e.Row.Cells.Count,
                    Text = String.Format("From {0} - {1}", txtDateStart.Text, txtDateEnd.Text)
                };
                gvr.Cells.Add(th);
                var gv = (GridView)sender;
                var tbl = (Table)gv.Controls[0];
                tbl.Rows.AddAt(0, gvr);
            }
        }

        #endregion
    }
}
