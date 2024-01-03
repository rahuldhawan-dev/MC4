using System;
using System.Configuration;
using System.Text;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;

namespace MapCall.Modules.HR.Accounting
{
    public partial class SystemDelivery : DataElementRolePage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get { return HumanResources.SystemDelivery; }
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

        protected void SystemDelivery1_ItemInserted(object sender, EventArgs e)
        {
            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
            Notes1.DataLinkID = SystemDelivery1.SystemDeliveryID;
            Notes1.Visible = true;
            Documents1.DataLinkID = SystemDelivery1.SystemDeliveryID;
            Documents1.Visible = true;
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            SystemDelivery1.ChangeMode(DetailsViewMode.Insert);
            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
            Documents1.Visible = false;
            Notes1.Visible = false;
        }
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = true;
            pnlResults.Visible = false;
            pnlDetail.Visible = false;
        }
        protected void btnBackToResults_Click(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = Filter;
            GridView1.DataBind();
            pnlSearch.Visible = false;
            pnlResults.Visible = true;
            pnlDetail.Visible = false;
            Documents1.DataLinkID = 0;
            Notes1.DataLinkID = 0;
            Documents1.Visible = false;
            Notes1.Visible = false;
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = Filter;
            MMSINC.Utility.ExportToExcel(Page, SqlDataSource1);
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            if (ddlOpCntr.SelectedIndex > 0)
                sb.Append(String.Format(" AND OpCode = '{0}'", ddlOpCntr.SelectedValue.Replace("'", "''")));
            if (!String.IsNullOrEmpty(txtBudgetYear.End))
                sb.Append(txtBudgetYear.FilterExpression("Budget_Year"));
            if (!String.IsNullOrEmpty(txtPWSID.Text))
				sb.Append(String.Format(" and [PWSID] like '%{0}%'", txtPWSID.Text.Replace("'", "''")));
            if (ddlSystem_Delivery_Category.SelectedIndex > 0)
                sb.Append(String.Format(" AND System_Delivery_Category = {0}", ddlSystem_Delivery_Category.SelectedValue));

            if (sb.Length > 0)
            {
                SqlDataSource1.FilterExpression = sb.ToString().Substring(5);
                this.Filter = SqlDataSource1.FilterExpression;
            }
            else
            {
                SqlDataSource1.FilterExpression = String.Empty;
                this.Filter = String.Empty;
            }

            GridView1.PageIndex = 0;
            GridView1.DataBind();

            pnlSearch.Visible = false;
            pnlResults.Visible = true;
            pnlDetail.Visible = false;
            Documents1.DataLinkID = 0;
            Notes1.DataLinkID = 0;
        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.Filter))
                SqlDataSource1.FilterExpression = Filter;
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SystemDelivery1.SystemDeliveryID = Int32.Parse(GridView1.SelectedDataKey[0].ToString());
            SystemDelivery1.DataBind();
            Audit.Insert(
                (int)AuditCategory.DataView,
               Page.User.Identity.Name,
                String.Format("Viewed SystemDelivery:{0}", SystemDelivery1.SystemDeliveryID),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
            Notes1.DataLinkID = Int32.Parse(GridView1.SelectedValue.ToString());
            Notes1.Visible = true;
            Documents1.DataLinkID = Int32.Parse(GridView1.SelectedValue.ToString());
            Documents1.Visible = true;
            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
        }
        protected void GridView1_Sorting(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.Filter))
                SqlDataSource1.FilterExpression = Filter;
        }

    }
}
