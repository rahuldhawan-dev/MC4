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
    public partial class RptBudgetReportByOpCode : DataElementPage
    {
        #region Large Sql String of DooM
        private const string SQL = @"Select 
  #2.OpCode,
  BudgetYear,
  #1.LookupValue as [Budget Category],
  #2.ExpenseLine,
  CAST(SUM([Jan]) as int) as [Jan],
  CAST(SUM([Feb]) as int) as [Feb],
  CAST(SUM([Mar]) as int) as [Mar],
  CAST(SUM([Apr]) as int) as [Apr],
  CAST(SUM([May]) as int) as [May],
  CAST(SUM([Jun]) as int) as [Jun],
  CAST(SUM([Jul]) as int) as [Jul],
  CAST(SUM([Aug]) as int) as [Aug],
  CAST(SUM([Sep]) as int) as [Sep],
  CAST(SUM([Oct]) as int) as [Oct],
  CAST(SUM([Nov]) as int) as [Nov],
  CAST(SUM([Dec]) as int) as [Dec],
  CAST((sum(jan)+sum(feb)+sum(mar)+sum(apr)+sum(may)+sum(jun)+sum(jul)+sum(aug)+sum(sep)+sum(oct)+sum(nov)+sum(dec)) as int) as Total,
  Budget_Category
from 
    tblBudget_ExpenseLines_and_Ref 
LEFT JOIN 
    Lookup #1 on #1.LookupID = Budget_Category
LEFT JOIN 
    tblAccounting_ExpenseLines #2 on #2.expenselinenumber_ID = [tblBudget_ExpenseLines_and_Ref].expenselinenumber_ID
GROUP BY 
    #2.OpCode
, BudgetYear
, #1.LookupValue 
, #2.ExpenseLine, Budget_Category
ORDER BY 1, 2
";
        #endregion

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

            //Call the search_click if the refresh button on the chart is clicked.
            cws.ChartRefreshClick += btnSearch_Click;
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

                changeGridView();
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

            cws.SeriesNames = string.IsNullOrEmpty(dfExpenseLine.CurrentValue.ToString()) ? new[] { "BudgetYear", "OpCode", "Budget Category" }
                : cws.SeriesNames = new[] { "ExpenseLine", "BudgetYear", "OpCode", "Budget Category" };
            cws.SeriesDefault = SeriesType.Line;
            cws.ShowSettingsDiv = true;
        }

        private bool columnExists(GridView gridView, string columnName)
        {
            bool bRet = false;
            foreach (DataControlField col in gridView.Columns)
            {
                if (col.ToString() == columnName)
                {
                    bRet = true;
                    break;
                }
            }

            return bRet;
        }

        private void changeGridView()
        {
            //If they chose an expense line, add the bound field to the grid and change the selector
            if (!string.IsNullOrEmpty(dfExpenseLine.CurrentValue.ToString()))
            {
                if (!columnExists(GridView1, "ExpenseLine"))
                {
                    BoundField bf = new BoundField();
                    bf.DataField = "ExpenseLine";
                    bf.HeaderText = "ExpenseLine";
                    GridView1.Columns.Insert(0, bf);
                }

                SqlDataSource1.SelectCommand = SQL;
            } 
        }

    }
}
