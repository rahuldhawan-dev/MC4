using System;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using MMSINC.Controls.GridViewHelper;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class WorkOrdersCreatedBy : WorkOrdersReport
    {
        #region Event Handlers

        protected void gvSearchResults_DataBinding(object sender, EventArgs e)
        {
            ((MvpGridView)sender).DataSource =
                EmployeeRepository.
                    GetEmployeeCounts(
                        int.Parse(ddlEmployee.SelectedValue == ""
                                      ? "0" : ddlEmployee.SelectedValue),
                        int.Parse(ddlOperatingCenter.SelectedValue),
                        DateTime.Parse(txtDateStart.Text),
                        DateTime.Parse(txtDateEnd.Text)
                        );
            
            var helper = new GridViewHelper((GridView)gvSearchResults);

            helper.RegisterGroup("OperatingCenter", true, true);
            helper.RegisterSummary("Created", SummaryOperation.Sum, "OperatingCenter");
            helper.RegisterSummary("Completed", SummaryOperation.Sum, "OperatingCenter");
            helper.ApplyGroupSort();
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
                var th = new TableHeaderCell {
                    ColumnSpan = e.Row.Cells.Count-1, 
                    Text= String.Format("{0} - {1}", txtDateStart.Text, txtDateEnd.Text)
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
