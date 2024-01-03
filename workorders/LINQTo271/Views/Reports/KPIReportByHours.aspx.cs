using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class KPIReportByHours : WorkOrdersReport
    {
        #region Private Members

        protected IEnumerable<WorkCategory> _categories;
        protected int? _year;

        #endregion

        #region Properties

        protected int Year
        {
            get
            {
                if (_year == null)
                    _year = Convert.ToInt32(ddlYear.SelectedValue);
                return _year.Value;
            }
        }

        protected string OpCode
        {
            get { return ddlOpCode.SelectedItem.Text; }
        }

        protected int OperatingCenterID
        {
            get { return Convert.ToInt32(ddlOpCode.SelectedValue); }
        }

        protected IEnumerable<WorkCategory> Categories
        {
            get
            {
                if (_categories == null)
                    _categories = WorkCategoryRepository.SelectAllAsList();
                return _categories;
            }
        }

        #endregion

        #region Private Methods

        #region Incomplete Orders

        private void DataBindIncompleteOrdersTable()
        {
            foreach (var cat in Categories)
            {
                RenderTableRowOfIncompleteOrders(cat);
            }
        }

        private void RenderTableRowOfIncompleteOrders(WorkCategory cat)
        {
            var tr = new TableRow();
            AddTableCellWithValue(tr, cat.Description);
            AddTableCellWithValue(tr, FormatTimeSpan(
                WorkOrderRepository.GetIncompleteOrderManHoursByCategory(
                    OperatingCenterID,
                    cat.WorkCategoryID)));
            tblIncompleteWorkOrders.Rows.Add(tr);
        }

        #endregion

        #region Complete Orders

        private void DataBindCompleteOrdersTable()
        {
            foreach (var cat in Categories)
            {
                RenderTableRowOfCompleteOrders(cat);
            }
        }

        private void RenderTableRowOfCompleteOrders(WorkCategory cat)
        {
            var curMonth = new DateTime(Year, 1, 1);
            var tr = new TableRow();
            AddTableCellWithValue(tr, cat.Description);
            tblCompletedWorkOrders.Rows.Add(tr);
            while (curMonth.Year < Year + 1)
            {
                AddTableCellWithValue(tr, FormatTimeSpan(
                    WorkOrderRepository.
                        GetCompleteOrderManHoursByMonthAndCategory(
                        OperatingCenterID, cat.WorkCategoryID, curMonth)));
                curMonth = curMonth.AddMonths(1);
            }
            AddTableCellWithValue(tr, FormatTimeSpan(
                WorkOrderRepository.GetCompleteOrderManHoursByYearAndCategory(
                    OperatingCenterID, cat.WorkCategoryID, Year)));
        }

        #endregion

        #region General

        private void AddTableCellWithValue(TableRow tr, string value)
        {
            var td = new TableCell();
            td.Controls.Add(new Label {
                Text = value
            });
            tr.Cells.Add(td);
        }

        private string FormatTimeSpan(TimeSpan span)
        {
            return string.Format("{0}:{1:00}", (span.Days * 60) + span.Hours,
                span.Minutes);
        }

        #endregion

        #endregion

        #region Event Handlers

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = false;
            pnlResults.Visible = true;

            lblOpCode.Text = OpCode;
            lblYear.Text = Year.ToString();

            DataBindCompleteOrdersTable();
            DataBindIncompleteOrdersTable();
        }

        #endregion
    }
}
