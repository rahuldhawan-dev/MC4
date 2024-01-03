using System;
using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using WorkOrders.Model;

namespace LINQTo271.Views.Reports
{
    public partial class AccrualReport : WorkOrdersReport
    {
        #region Constants

        public struct RestorationParameterNames
        {
            public const string ACCOUNTING_TYPE_ID = "accountingTypeID";
        }

        #endregion

        #region Event Handlers

        protected override void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            odsRestorations.SelectParameters[RestorationParameterNames.ACCOUNTING_TYPE_ID].DefaultValue =
                AccountingTypeRepository.OAndM.AccountingTypeID.ToString();
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
