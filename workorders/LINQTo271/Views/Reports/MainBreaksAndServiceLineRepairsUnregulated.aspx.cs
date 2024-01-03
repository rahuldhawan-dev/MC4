using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using WorkOrders.Model;
using dotnetCHARTING;
using Label = System.Web.UI.WebControls.Label;

namespace LINQTo271.Views.Reports
{
    public partial class MainBreaksAndServiceLineRepairsUnregulated : WorkOrdersReport
    {
        #region Control Declarations

        protected IDropDownList ddlOpCode;

        #endregion

        #region Private Members

        protected int? _year;
        private IEnumerable<WorkDescription> _workDescriptions;

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

        protected int? StateId
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ddlState.SelectedValue))
                {
                    return null;
                }
                return Convert.ToInt32(ddlState.SelectedValue);
            }
        }

        private string State
        {
            get { return ddlState.SelectedItem.Text; }
        }

        protected string OpCode
        {
            get { return ddlOpCode.SelectedItem.Text; }
        }

        protected int? OperatingCenterID
        {
            get { return ddlOpCode.GetSelectedValue(); }
        }

        protected IEnumerable<WorkDescription> WorkDescriptions
        {
            get
            {
                if (_workDescriptions == null)
                {
                    _workDescriptions =
                        WorkDescriptionRepository.
                            SelectMainBreakAndServiceLineDescriptions();
                }
                return _workDescriptions;
            }
        }

        #endregion

        #region Private Methods

        #region Incomplete Orders

        private void DataBindIncompleteOrdersTable()
        {
            foreach (var desc in WorkDescriptions)
            {
                RenderTableRowOfIncompleteOrders(desc);
            }
        }

        private void RenderTableRowOfIncompleteOrders(WorkDescription desc)
        {
            var tr = new TableRow();
            AddTableCellWithValue(tr, desc.Description);
            AddTableCellWithValue(tr,
                WorkOrderRepository.GetIncompleteWorkOrderCountByDescription(StateId,
                    OperatingCenterID,
                    desc.WorkDescriptionID, false).ToString());
            tblIncompleteWorkOrders.Rows.Add(tr);
        }

        #endregion

        #region Complete Orders

        private void DataBindCompleteOrdersTableAndChart()
        {
            var sc = new SeriesCollection();

            foreach (var desc in WorkDescriptions)
            {
                sc.Add(RenderTableRowAndChartLineOfCompleteOrders(desc));
            }

            DataBindAndSetupChart(sc);
        }

        private Series RenderTableRowAndChartLineOfCompleteOrders(WorkDescription desc)
        {
            var curMonth = new DateTime(Year, 1, 1);
            var tr = new TableRow();
            AddTableCellWithValue(tr, desc.Description);
            tblCompletedWorkOrders.Rows.Add(tr);

            var series = new Series(desc.Description);

            while (curMonth.Year < Year + 1)
            {
                var count = WorkOrderRepository.
                    GetCompleteOrderCountByMonthAndDescription(StateId,
                    OperatingCenterID, desc.WorkDescriptionID, curMonth, false);

                series.Elements.Add(new Element(curMonth.ToString("MMMM"), count));
                AddTableCellWithValue(tr, count.ToString());

                curMonth = curMonth.AddMonths(1);
            }
            AddTableCellWithValue(tr,
                WorkOrderRepository.GetCompleteOrderCountByYearAndDescription(StateId,
                    OperatingCenterID, desc.WorkDescriptionID, Year, false).ToString());

            return series;
        }

        private void DataBindAndSetupChart(SeriesCollection sc)
        {
            #if DEBUG  
                Chart.TempDirectory = "~/temp";
            #else
                Chart.TempDirectory = "../../temp";
            #endif
            
            Chart.DefaultSeries.Type = SeriesType.Line;
            Chart.DefaultSeries.DefaultElement.Transparency = 20;

            Chart.YAxis.Label.Text = "completed order count";

            Chart.SeriesCollection.Add(sc);
        }

        #endregion

        #region General

        private void AddTableCellWithValue(TableRow tr, string value)
        {
            var td = new TableCell();
            td.Controls.Add(new Label
            {
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

            lblOpCode.Text = OperatingCenterID == null ? "All" : OpCode;
            lblYear.Text = Year.ToString();
            lblState.Text = State;

            DataBindCompleteOrdersTableAndChart();
            DataBindIncompleteOrdersTable();
        }

        #endregion
    }
}
