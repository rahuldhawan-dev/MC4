using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class TrafficControlRequirement : WorkOrdersReport
    {
        #region Constants

        public const int MAX_RECORDS = 1000;
        
        #endregion

        #region Properties

        protected int? OperatingCenterID
        {
            get { return ddlOpCode.GetSelectedValue(); }
        }

        protected int? TownID
        {
            get { return ddlTown.GetSelectedValue(); }
        }

        protected bool? TrafficControlRequired
        {
            get
            {
                return
                    ddlTrafficControl.GetSelectedValue(
                        li =>
                        li.Value == String.Empty ? (bool?)null : bool.Parse(li.Value));
            }
        }

        protected DateTime? DateCompleted
        {
            get { return drCompletedDate.Date; }
        }

        #endregion

        #region Event Handlers

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var data = GatherData();

            if (data.Count() > MAX_RECORDS)
            {
                lblError.Text =
                    string.Format(
                        "The criteria you have chosen has resulted in more than {0} records.  Please refine your search.",
                        MAX_RECORDS);
            }
            else
            {
                lblError.Text = "";
                gvSearchResults.DataSource = data.OrderBy(wo => wo.WorkOrderID);
                base.btnSearch_Click(sender, e);
            }
        }

        protected void gvSearchResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            var resultView = ((MvpGridView)sender);
            resultView.DataSource = GetWorkOrders(GetSortExpression(e.SortExpression));
            resultView.DataBind();
        }

        #endregion

        #region Primate Methods

        // OOOH OOOH OOOH AAAAAH AAAAAH AAAAAH!!!!
        private IQueryable<WorkOrder> GatherData()
        {
            var data =
                WorkOrderRepository.
                    GetCompletedOrdersByOperatingCenterAndTrafficControlRequirement(
                        OperatingCenterID, TownID, TrafficControlRequired);

            if (DateCompleted.HasValue)
            {
                switch (drCompletedDate.SelectedOperator)
                {
                    case "=":
                        data = data.Where(wo => wo.DateCompleted == drCompletedDate.Date);
                        break;
                    case ">":
                        data = data.Where(
                            wo =>
                            DateTime.Compare(wo.DateCompleted.Value,
                                drCompletedDate.Date.Value) > 0);
                        break;
                    case ">=":
                        data = data.Where(
                            wo =>
                            DateTime.Compare(wo.DateCompleted.Value,
                                drCompletedDate.Date.Value) >= 0);
                        break;
                    case "<":
                        data = data.Where(
                            wo =>
                            DateTime.Compare(wo.DateCompleted.Value,
                                drCompletedDate.Date.Value) < 0);
                        break;
                    case "<=":
                        data = data.Where(
                            wo =>
                            DateTime.Compare(wo.DateCompleted.Value,
                                drCompletedDate.Date.Value) <= 0);
                        break;
                    case "BETWEEN":
                        data = data.Where(
                            wo =>
                            (DateTime.Compare(wo.DateCompleted.Value,
                                drCompletedDate.StartDate.Value) >= 0 &&
                             DateTime.Compare(wo.DateCompleted.Value,
                                 drCompletedDate.EndDate.Value) <= 0));
                        break;
                }
            }

            return data;
        }

        private IEnumerable<WorkOrder> GetWorkOrders(string sortExpression)
        {
            return GatherData().Sorting().Sort<WorkOrder>(sortExpression);
        }

        #endregion
    }
}
