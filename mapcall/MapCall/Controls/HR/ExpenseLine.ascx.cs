using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;
using MapCall.Controls.HR.dropdownlists;

namespace MapCall.Controls.HR
{
    public partial class ExpenseLine : UserControl
    {
        #region Public Members

        public int expenseLineID;
        public int BudgetExpenseLineID;
        public event EventHandler ItemInserted;

        #endregion

        #region Properties
        
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }

        #endregion
        
        #region Event Handlers

        protected void OnItemInserted(EventArgs e)
        {
            if (ItemInserted != null)
            {
                ItemInserted(this, e);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            divCharts.Visible = true;
            lblResults.Text = "";
        }
        protected void DetailsView1_DataBound(object sender, EventArgs e)
        {
            if (BudgetExpenseLineID != 0)
            {
                SqlDataSource1.SelectParameters[0].DefaultValue = BudgetExpenseLineID.ToString();
                LoadChart();
            }            
        }
        protected void DetailsView1_PreRender(object sender, EventArgs e)
        {
            if (DetailsView1.CurrentMode == DetailsViewMode.Insert)
            {
                divCharts.Visible = false;
                imgPieChart.Visible = false;
                imgChart.Visible = false;
            }

            var lbtnDelete = Utility.GetFirstControlInstance(DetailsView1, "btnDelete");
            if (lbtnDelete != null)
                lbtnDelete.Visible = AllowDelete;
            var lbtnEdit = Utility.GetFirstControlInstance(DetailsView1, "btnEdit");
            if (lbtnEdit != null)
                lbtnEdit.Visible = AllowEdit;
        }
        protected void lbShowCharts_Click(object sender, EventArgs e)
        {
            imgPieChart.Visible = true;
            imgChart.Visible = true;
            divCharts.Focus();
        }
        protected void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            string strValue = ((IDbDataParameter)e.Command.Parameters["@BudgetExpenseLineID"]).Value.ToString();
            SqlDataSource1.SelectParameters[0].DefaultValue = strValue;
            Audit.Insert(
                (int)AuditCategory.DataInsert,
                Page.User.Identity.Name,
                String.Format("Added BudgetExpenseLineID:{0}", strValue),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
            this.BudgetExpenseLineID = Int32.Parse(strValue);
            OnItemInserted(e);
            DetailsView1.ChangeMode(DetailsViewMode.ReadOnly);
            DetailsView1.DataBind();
        }
        protected void SqlDataSource1_Deleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            lblResults.Text = "Record Deleted";
            Audit.Insert(
                (int)AuditCategory.DataDelete,
                Page.User.Identity.Name,
                String.Format("Deleted BudgetExpenseLineID:{0}", ((IDbDataParameter)e.Command.Parameters["@BudgetExpenseLineID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }
        protected void SqlDataSource1_Updated(object sender, SqlDataSourceStatusEventArgs e)
        {
            lblResults.Text = "Record Updated";
            Audit.Insert(
                (int)AuditCategory.DataUpdate,
                Page.User.Identity.Name,
                String.Format("Updated BudgetExpenseLineID:{0}", ((IDbDataParameter)e.Command.Parameters["@BudgetExpenseLineID"]).Value.ToString()),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }

        protected void btnLookup_Click(object sender, EventArgs e)
        {
            PopulateReforecast();
        }
    #endregion

    #region Private Methods

        private void LoadChart()
        {
            double[] x = { 1.1, 1.2, 1.3 };
            int[] y = { 1, 3, 4, 5, 6 };
            
            DataControlFieldCell lblJan = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblJan");
            DataControlFieldCell lblFeb = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblFeb");
            DataControlFieldCell lblMar = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblMar");
            DataControlFieldCell lblApr = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblApr");
            DataControlFieldCell lblMay = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblMay");
            DataControlFieldCell lblJun = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblJun");
            DataControlFieldCell lblJul = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblJul");
            DataControlFieldCell lblAug = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblAug");
            DataControlFieldCell lblSep = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblSep");
            DataControlFieldCell lblOct = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblOct");
            DataControlFieldCell lblNov = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblNov");
            DataControlFieldCell lblDec = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblDec");
            DataControlFieldCell lblTotal = (DataControlFieldCell)Utility.GetFirstControlInstance(DetailsView1, "dvLblTotal");

            try
            {
                if (lblJan != null && !String.IsNullOrEmpty(lblJan.Text) && !lblJan.Text.Contains("&nbsp;"))
                {
                    imgChart.ImageUrl = String.Format("http://chart.apis.google.com/chart?cht=lc&chs=600x300&chd=t:{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}&chtt=&chco=ff0000,0000ff&chxt=x,y&chxl=0:|Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|1:|0|1000000&chg=9.1,25&chco=0000ff",
                        Double.Parse(lblJan.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblFeb.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblMar.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblApr.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblMay.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblJun.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblJul.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblAug.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblSep.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblOct.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblNov.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblDec.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100
                        );
                    imgPieChart.ImageUrl = String.Format("http://chart.apis.google.com/chart?cht=p&chs=600x300&chd=t:{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11}&chtt=&chco=ff0000,0000ff&chxt=x,y&chxl=0:|Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec|1:|0|1000000&chg=9.1,25&chco=0000ff&chtt={12}",
                        Double.Parse(lblJan.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblFeb.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblMar.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblApr.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblMay.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblJun.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblJul.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblAug.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblSep.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblOct.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblNov.Text.Replace("$", "").Replace("(", "-").Replace(")", "").Replace(",", "")) / 1000000 * 100,
                        Double.Parse(lblDec.Text.Replace("$", "")) / 1000000 * 100,
                        String.Format("Total : {0}", lblTotal.Text)
                        );
                }
            }
            catch
            {
                imgChart.Visible = false;
                imgPieChart.Visible = false;
            }
        }

        /// <summary>
        /// The user is going to recreate new item from the existing data
        /// lookup the values to help them out.
        /// This code is very weak.
        /// </summary>
        private void PopulateReforecast()
        {
            ddlBudgetCategory ddlBudget_Category = (ddlBudgetCategory)Utility.GetFirstControlInstance(DetailsView1, "ddlBudget_Category");
            ddlExpenseLine ddlExpenseLine = (ddlExpenseLine)Utility.GetFirstControlInstance(DetailsView1, "ddlExpenseLine");
            TextBox txtBudgetYear = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtBudgetYear_-1");
            Label lblExpenseLine = (Label)Utility.GetFirstControlInstance(DetailsView1, "lblExpenseLine");
            lblExpenseLine.Text = "";
            TextBox txtJan = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtJan_-1");
            TextBox txtFeb = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtFeb_-1");
            TextBox txtMar = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtMar_-1");
            TextBox txtApr = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtApr_-1");
            TextBox txtMay = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtMay_-1");
            TextBox txtJun = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtJun_-1");
            TextBox txtJul = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtJul_-1");
            TextBox txtAug = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtAug_-1");
            TextBox txtSep = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtSep_-1");
            TextBox txtOct = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtOct_-1");
            TextBox txtNov = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtNov_-1");
            TextBox txtDec = (TextBox)Utility.GetFirstControlInstance(DetailsView1, "dvTxtDec_-1");

            txtJan.Text = "0";
            txtFeb.Text = "0";
            txtMar.Text = "0";
            txtApr.Text = "0";
            txtMay.Text = "0";
            txtJun.Text = "0";
            txtJul.Text = "0";
            txtAug.Text = "0";
            txtSep.Text = "0";
            txtOct.Text = "0";
            txtNov.Text = "0";
            txtDec.Text = "0";

            dsExpenseLine.SelectParameters["BudgetYear"].DefaultValue = txtBudgetYear.Text;
            dsExpenseLine.SelectParameters["BudgetCategory"].DefaultValue = "53"; //53-Actual 54-Budget
            dsExpenseLine.SelectParameters["ExpenseLineNumber"].DefaultValue = ddlExpenseLine.SelectedValue.ToString();
            DataView dvActual = (DataView)dsExpenseLine.Select(DataSourceSelectArguments.Empty);

            dsExpenseLine.SelectParameters["BudgetCategory"].DefaultValue = "54"; //53-Actual 54-Budget
            DataView dvBudget = (DataView)dsExpenseLine.Select(DataSourceSelectArguments.Empty);

            if (dvBudget==null || dvActual==null || dvBudget.Count == 0 || dvActual.Count == 0)
            {
                lblExpenseLine.Text = "<br>No data was found for the data provided. Please double check the Budget Year, Category, and Expense Line.";
            }
            else
            {
                switch (ddlBudget_Category.SelectedValue)
                {
                    case "55": // Q1 Reforcast (As long as no one changes any lookups)
                        // Fill in Jan - Mar from Actual
                        if (dvActual.Count > 0)
                        {
                            txtJan.Text = dvActual[0][0].ToString();
                            txtFeb.Text = dvActual[0][1].ToString();
                        }
                        // Fill in Apr - Dec from Budget
                        if (dvBudget.Count > 0)
                        {
                            txtMar.Text = dvActual[0][2].ToString();
                            txtApr.Text = dvBudget[0][3].ToString();
                            txtMay.Text = dvBudget[0][4].ToString();
                            txtJun.Text = dvBudget[0][5].ToString();
                            txtJul.Text = dvBudget[0][6].ToString();
                            txtAug.Text = dvBudget[0][7].ToString();
                            txtSep.Text = dvBudget[0][8].ToString();
                            txtOct.Text = dvBudget[0][9].ToString();
                            txtNov.Text = dvBudget[0][10].ToString();
                            txtDec.Text = dvBudget[0][11].ToString();
                        }
                        break;
                    case "56": // Q2
                        // Fill in Jan - Jun from Actual
                        if (dvActual.Count > 0)
                        {
                            txtJan.Text = dvActual[0][0].ToString();
                            txtFeb.Text = dvActual[0][1].ToString();
                            txtMar.Text = dvActual[0][2].ToString();
                            txtApr.Text = dvActual[0][3].ToString();
                            txtMay.Text = dvActual[0][4].ToString();
                        }
                        // Fill in Jul - Dec from Budget
                        if (dvBudget.Count > 0)
                        {
                            txtJun.Text = dvActual[0][5].ToString();
                            txtJul.Text = dvBudget[0][6].ToString();
                            txtAug.Text = dvBudget[0][7].ToString();
                            txtSep.Text = dvBudget[0][8].ToString();
                            txtOct.Text = dvBudget[0][9].ToString();
                            txtNov.Text = dvBudget[0][10].ToString();
                            txtDec.Text = dvBudget[0][11].ToString();
                        }
                        break;
                    case "57": // Q3 
                        // Fill in Jan - Sep from Actual
                        if (dvActual.Count > 0)
                        {
                            txtJan.Text = dvActual[0][0].ToString();
                            txtFeb.Text = dvActual[0][1].ToString();
                            txtMar.Text = dvActual[0][2].ToString();
                            txtApr.Text = dvActual[0][3].ToString();
                            txtMay.Text = dvActual[0][4].ToString();
                            txtJun.Text = dvActual[0][5].ToString();
                            txtJul.Text = dvActual[0][6].ToString();
                            txtAug.Text = dvActual[0][7].ToString();
                        }
                        // Fill in Oct - Dec from Budget
                        if (dvBudget.Count > 0)
                        {
                            txtSep.Text = dvActual[0][8].ToString();
                            txtOct.Text = dvBudget[0][9].ToString();
                            txtNov.Text = dvBudget[0][10].ToString();
                            txtDec.Text = dvBudget[0][11].ToString();
                        }
                        break;
                    default:
                        break;
                }
            }

            if (dvActual != null)
                dvActual.Dispose();
            if (dvBudget != null)
                dvBudget.Dispose();
        }
    #endregion

    #region Exposed Methods

        public void ChangeMode(DetailsViewMode dvm)
        {
            DetailsView1.ChangeMode(dvm);
        }

    #endregion
    }
}