using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class OrcomOrders : WorkOrdersReport
    {
        #region Private Members

        protected IEnumerable<WorkDescription> _descriptions;
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
            get
            {
                return
                    ddlOpCode.GetSelectedValue(
                        li => li.Value == "" ? "n/a" /* yo */ : li.Text);
            }
        }

        protected int? OperatingCenterID
        {
            get { return ddlOpCode.GetSelectedValue(); }
        }

        protected IEnumerable<WorkDescription> WorkDescriptions
        {
            get
            {
                if (_descriptions == null)
                    _descriptions = WorkDescriptionRepository.SelectAllSorted();
                return _descriptions;
            }
        }

        #endregion

        #region Private Members

        protected void DataBindTable()
        {
            foreach (var desc in WorkDescriptions)
            {
                RenderTableRowOfOrders(desc);
            }
        }

        protected void RenderTableRowOfOrders(WorkDescription desc)
        {
            var curMonth = new DateTime(Year, 1, 1);
            var tr = new TableRow();
            AddTableCellWithValue(tr, desc.Description);
            tblWorkOrders.Rows.Add(tr);
            while (curMonth.Year < Year + 1)
            {
                AddTableCellWithValue(tr,
                    WorkOrderRepository.
                        GetORCOMOrderCountByMonthAndWorkDescription(
                        OperatingCenterID, desc.WorkDescriptionID, curMonth).
                        ToString());
                curMonth = curMonth.AddMonths(1);
            }
            AddTableCellWithValue(tr, WorkOrderRepository.
                GetORCOMOrderCountByYearAndWorkDescription(OperatingCenterID,
                    desc.WorkDescriptionID, Year).ToString());
        }

        private void AddTableCellWithValue(TableRow tr, string value)
        {
            var td = new TableCell();
            td.Controls.Add(new Label {
                Text = value
            });
            tr.Cells.Add(td);
        }

        #endregion

        #region Event Handlers

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = false;
            pnlResults.Visible = true;

            lblOpCode.Text = OpCode;
            lblYear.Text = Year.ToString();

            DataBindTable();
        }

        #endregion
    }
}
