using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class KPIReport : WorkOrdersReport
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
            AddTableCellWithValue(tr,
                WorkOrderRepository.GetIncompleteWorkOrderCountByCategory(
                    OperatingCenterID,
                    cat.WorkCategoryID).ToString());
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
                AddTableCellWithValue(tr,
                    WorkOrderRepository.GetCompleteOrderCountByMonthAndCategory(
                        OperatingCenterID, cat.WorkCategoryID, curMonth).
                        ToString());
                curMonth = curMonth.AddMonths(1);
            }
            AddTableCellWithValue(tr,
                WorkOrderRepository.GetCompleteOrderCountByYearAndCategory(
                    OperatingCenterID, cat.WorkCategoryID, Year).ToString());
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
