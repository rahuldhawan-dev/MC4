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
    public partial class SpoilsPerWorkOrder : WorkOrdersReport
    {
        #region Constants

        public const int MAX_RECORDS = 1000;

        #endregion

        #region Properties

        protected int? OperatingCenterID
        {
            get { return ddlOperatingCenter.GetSelectedValue(); }
        }

        protected int? TownID
        {
            get { return ddlTown.GetSelectedValue(); }
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
            if (data.Count() > 1000)
            {
                lblError.Text =
                    String.Format(
                        "The criteria you have chosen will bring back over {0} results.  Please refine your search.",
                        MAX_RECORDS);
            }
            else
            {
                lblError.Text = "";
                base.btnSearch_Click(sender, e);
            }
        }

        protected void gvSearchResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            var resultView = ((MvpGridView)sender);
            resultView.DataSource = GetSpoils(GetSortExpression(e.SortExpression));
            resultView.DataBind();
        }

        #endregion

        #region Private Methods

        protected override void DataBindResults()
        {
            gvSearchResults.DataSource = GatherData().OrderBy(x => x.WorkOrderID);
            base.DataBindResults();
        }

        private IEnumerable<Spoil> GatherData()
        {
            var data = SpoilRepository.
                GetSpoilsByTownAndOpCenterForCompleteWorkOrders(
                    OperatingCenterID, TownID);

            if (DateCompleted.HasValue)
            {
                switch (drCompletedDate.SelectedOperator)
                {
                    case "=":
                        data = data.Where(s => s.WorkOrder.DateCompleted.Value.Date == DateCompleted);
                        break;
                    case ">":
                        data = data.Where(s => DateTime.Compare(s.WorkOrder.DateCompleted.Value.Date, DateCompleted.Value) > 0);
                        break;
                    case ">=":
                        data = data.Where(s => DateTime.Compare(s.WorkOrder.DateCompleted.Value.Date, DateCompleted.Value) >= 0);
                        break;
                    case "<":
                        data = data.Where(s => DateTime.Compare(s.WorkOrder.DateCompleted.Value.Date, DateCompleted.Value) < 0);
                        break;
                    case "<=":
                        data = data.Where(s => DateTime.Compare(s.WorkOrder.DateCompleted.Value.Date, DateCompleted.Value) <= 0);
                        break;
                }
            } else if (drCompletedDate.StartDate.HasValue && drCompletedDate.EndDate.HasValue)
            {
                data = data.Where(s =>
                        (DateTime.Compare(s.WorkOrder.DateCompleted.Value.Date, drCompletedDate.StartDate.Value.Date) >= 0 &&
                         DateTime.Compare(s.WorkOrder.DateCompleted.Value.Date, drCompletedDate.EndDate.Value.Date) <= 0));

            }

            return data;
        }

        private IEnumerable<Spoil> GetSpoils(string sortExpression)
        {
            return GatherData().Sorting().Sort<Spoil>(sortExpression);
        }

        #endregion
    }
}