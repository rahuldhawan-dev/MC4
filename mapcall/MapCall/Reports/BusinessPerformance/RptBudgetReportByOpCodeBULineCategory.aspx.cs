using System;
using System.Text;
using System.Web.UI.WebControls;
using MMSINC.DataPages;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Controls.Data;
using dotnetCHARTING;

namespace MapCall.Modules.BusinessPerformance
{
    public partial class RptBudgetReportByOpCodeBULineCategory : DataElementPage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.BusinessPerformance.General;
            }
        }

        #endregion

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!CanView)
            {
                pnlSearch.Visible = false;
                lblPermissionErrors.Text = String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module);
            }

        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (Object ctrl in pnlSearch.Controls)
                if (ctrl is DataField)
                    sb.Append(((IDataField)ctrl).FilterExpression());

            if (lbBudgetCategories.SelectedValue.Length > 0)
            {
                sb.Append(" AND Budget_Category in (");
                foreach (ListItem li in lbBudgetCategories.Items)
                {
                    if (li.Selected)
                        sb.Append(String.Format("{0},", li.Value));
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append(")");
            }

            if (sb.Length > 0)
            {
                SqlDataSource1.FilterExpression = sb.ToString().Substring(5);
                Filter = SqlDataSource1.FilterExpression;
                hidFilter.Value = Filter;
            }
            else
            {
                SqlDataSource1.FilterExpression = String.Empty;
                Filter = String.Empty;
                hidFilter.Value = Filter;
            }

            base.btnSearch_Click(sender, e);
            lblRecordCount.Text = String.Format("Total Records: {0}", GridView1.Rows.Count);
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Chart Setup
            cws.ChartDataSource = SqlDataSource1;
            cws.ChartDataSource.FilterExpression = hidFilter.Value;
            cws.ColumnNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            cws.SeriesNames = new[] {   "OpCode", "BudgetYear", "BU", "Expenselinenumber", "ExpenseLine", "Budget Category" };
            cws.SeriesDefault = SeriesType.Line;
            cws.ShowSettingsDiv = true;
        }
    }
}
