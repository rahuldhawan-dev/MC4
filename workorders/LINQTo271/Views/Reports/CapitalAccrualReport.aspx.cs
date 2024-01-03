using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class CapitalAccrualReport : WorkOrdersReport
    {
        #region Constants

        public struct RestorationParameterNames
        {
            public const string ACCOUNTING_TYPE_ID = "accountingTypeID";
        }

        public static Dictionary<string, Func<Restoration, object>> SORT_COLUMNS
            = new Dictionary<string, Func<Restoration, object>> {
                { "WorkOrderID", r => r.WorkOrderID },
                { "OldWorkOrderNumber", r => r.WorkOrder.OldWorkOrderNumber },
                { "DateCompleted", r => r.WorkOrder.DateCompleted },
                { "AccountCharged", r => r.WorkOrder.AccountCharged },
                { "AssetType", r => r.WorkOrder.AssetType.Description },
                { "WorkDescription",
                    r => r.WorkOrder.WorkDescription.Description },
                { "RestorationType", r => r.RestorationType.Description },
                { "RestorationSize", r => r.RestorationSize },
                { "MeasurementType", r => r.RestorationType.MeasurementTypeString },
                { "TotalAccruedCost", r => r.TotalAccruedCost },
                { "PartialRestorationDate", r => r.PartialRestorationDate },
                { "TotalInitalCost", r => r.PartialRestorationActualCost },
                { "AccrualVariance", r => Restoration.GetAccrualVariance(r) }
            };

        #endregion

        #region Properties

        protected DateTime DateStart
        {
            get { return DateTime.Parse(txtDateStart.Text); }
        }

        protected DateTime DateEnd
        {
            get { return DateTime.Parse(txtDateEnd.Text); }
        }

        protected int AccountingTypeID
        {
            get { return AccountingTypeRepository.Capital.AccountingTypeID; }
        }

        protected int OperatingCenterID
        {
            get { return ddlOpCode.GetSelectedValue().Value; }
        }

        #endregion

        protected override void DataBindResults()
        {
            gvSearchResults.DataSource =
                RestorationRepository.
                    GetRestorationsByWorkOrderDateCompletedAndAccountingType(
                    DateStart, DateEnd, AccountingTypeID, OperatingCenterID);
            
            base.DataBindResults();
        }

        #region Event Handlers

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
                RestorationRepository.
                    GetApprovedRestorationsByWorkOrderDateCompletedAndAccountingType(
                    DateStart, DateEnd, AccountingTypeID, OperatingCenterID,
                    SORT_COLUMNS[e.SortExpression], SortDirection);
            gvSearchResults.DataBind();
            SortExpression = e.SortExpression;
        }

        private decimal _accrualValueSum;
        protected void gvSearchResults_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var rowLabel = (Label)e.Row.FindControl("lblAccrualValue");
                var accrualValue = decimal.Parse(rowLabel.Text);
                _accrualValueSum += accrualValue;
                rowLabel.Text = string.Format("{0:C}", accrualValue);
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                var footerLabel = (Label)e.Row.FindControl("lblAccrualValueSum");
                footerLabel.Text = string.Format("{0:C}", _accrualValueSum);
            }
        }

        #endregion

    }
}
