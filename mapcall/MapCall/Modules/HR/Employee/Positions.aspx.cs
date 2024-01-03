using System;
using System.Configuration;
using System.Text;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.DataPages;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls.Data;

namespace MapCall.Modules.HR.Employee
{
    public partial class Positions : DataElementRolePage
    {
       #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return HumanResources.Positions;
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
            btnAdd.Visible = Notes1.AllowAdd = Documents1.AllowAdd = CanAdd;
        }
        protected void Position1_DataBinding(object sender, EventArgs e)
        {
            Notes1.AllowEdit = Documents1.AllowEdit = Position1.AllowEdit = CanEdit;
            Notes1.AllowDelete = Documents1.AllowDelete = Position1.AllowDelete = CanDelete;
        }
        protected void Position1_ItemInserted(object sender, EventArgs e)
        {
            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
            Notes1.DataLinkID = Position1.positionID;
            Notes1.Visible = true;
            Documents1.DataLinkID = Position1.positionID;
            Documents1.Visible = true; 
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
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Position1.ChangeMode(DetailsViewMode.Insert);
            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
            Documents1.DataLinkID = 0;
            Documents1.Visible = false;
            Notes1.Visible = false;
            Notes1.DataLinkID = 0;
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

            foreach (Object ctrl in pnlSearch.Controls)
                if (ctrl is DataField)
                    sb.Append(((IDataField)ctrl).FilterExpression());

            // TODO: Move all of these to mmsinc:datafields
            if (ddlOpCode.SelectedIndex > 0)
                sb.Append(String.Format(" AND OpCode = '{0}'", ddlOpCode.SelectedValue.Replace("'", "''")));
            if (!String.IsNullOrEmpty(txtPosition.Text))
                sb.Append(String.Format(" AND Position like '%{0}%'", txtPosition.Text.Replace("'", "''")));
            if (ddlCategory.SelectedIndex > 0)
                sb.Append(String.Format(" AND Category = '{0}'", ddlCategory.SelectedValue.Replace("'", "''")));
            if (ddlEEO_Job_Code.SelectedIndex > 0)
                sb.Append(String.Format(" AND EEO_Job_Code = '{0}'", ddlEEO_Job_Code.SelectedValue.Replace("'", "''")));
            if (ddlEEO_Job_Description.SelectedIndex > 0)
                sb.Append(String.Format(" AND EEO_Job_Description = '{0}'", ddlEEO_Job_Description.SelectedValue.Replace("'", "''")));
            if (ddlDepartment.SelectedIndex > 0)
                sb.Append(String.Format(" AND Department = '{0}'", ddlDepartment.SelectedValue.Replace("'", "''")));
            if (ddlCommon_Name.SelectedIndex > 0)
                sb.Append(String.Format(" AND Common_Name = '{0}'", ddlCommon_Name.SelectedValue.Replace("'", "''")));
            if (ddlLocal.SelectedIndex > 0)
                sb.Append(String.Format(" And LocalID = {0}", ddlLocal.SelectedValue.ToString()));
            if (ddlFLSAStatus.SelectedIndex > 0)
                sb.Append(String.Format(" And FLSAStatus = {0}", ddlFLSAStatus.SelectedValue.ToString()));
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

            if (ddlOrderBy.SelectedIndex > 0)
                GridView1.Sort(ddlOrderBy.SelectedValue, SortDirection.Ascending);

            Notes1.DataLinkID = 0;
            Documents1.DataLinkID = 0;
            GridView1.PageIndex = 0;
            GridView1.DataBind();
            pnlSearch.Visible = false;
            pnlResults.Visible = true;
            pnlDetail.Visible = false;
        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.Filter))
                SqlDataSource1.FilterExpression = Filter;
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Position1.positionID = Int32.Parse(GridView1.SelectedDataKey[0].ToString());
            Position1.DataBind();
            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed PositionID:{0}", Position1.positionID),
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
