using System;
using System.Configuration;
using System.Text;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;

namespace MapCall.Modules.HR.Employee
{
    public partial class PositionPostings : DataElementPage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return HumanResources.PositionPosting;
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
        protected void PositionPostings1_ItemInserted(object sender, EventArgs e)
        {
            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
            Employee1.DataLinkID = PositionPosting1.positionPostingID;
            Employee1.Visible = true;
            Notes1.DataLinkID = PositionPosting1.positionPostingID;
            Notes1.Visible = true;
            Documents1.DataLinkID = PositionPosting1.positionPostingID;
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
            PositionPosting1.ChangeMode(DetailsViewMode.Insert);
            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
            Documents1.Visible = false;
            Employee1.DataLinkID = 0;
            Employee1.Visible = false;
            Notes1.DataLinkID = 0;
            Notes1.Visible = false;
            Documents1.DataLinkID = 0;
            Documents1.Visible = false;
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
            if (!String.IsNullOrEmpty(txtPosition_Posting_ID.Text))
                sb.Append(String.Format(" AND Position_Posting_ID = {0}", Int32.Parse(txtPosition_Posting_ID.Text.Replace("'", "''"))));
            if (!string.IsNullOrEmpty(Position_Control_Number.Text))
                sb.Append(String.Format(" AND Position_Control_Number = {0}", Position_Control_Number.Text.Replace("'", "''")));
            if (ddlPositions.SelectedIndex > 0)
                sb.Append(String.Format(" AND Position_ID = {0}", ddlPositions.SelectedValue));
            if (!String.IsNullOrEmpty(txtDate_of_Posting.EndDate.ToString()))
                sb.Append(txtDate_of_Posting.FilterExpression("Date_of_Posting"));
            if (ddlEmployees.SelectedIndex > 0)
                sb.Append(String.Format(" AND tblEmployeeID = {0}", ddlEmployees.SelectedValue));
            if (ddlOpCode.SelectedIndex > 0)
                sb.Append(String.Format(" AND opCode = '{0}'", ddlOpCode.SelectedValue));
            if (ddlLocal.SelectedIndex > 0)
                sb.Append(String.Format(" AND LocalID = {0}", ddlLocal.SelectedValue));
            if (chkCandidate_Selected.Checked)
                sb.Append(" AND Candidate_Selected = 1");
            if (chkInterviewing.Checked)
                sb.Append(" AND Interviewing = 1");
            if (chkExternal_Recruitment.Checked)
                sb.Append(" AND External_Recruitment = 1");
            if (chkInternal_Posting.Checked)
                sb.Append(" AND Internal_Posting = 1");
            if (!String.IsNullOrEmpty(txtEffective_Date.EndDate.ToString()))
                sb.Append(txtEffective_Date.FilterExpression("Effective_Date"));

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

            GridView1.PageIndex = 0;
            GridView1.DataBind();
            pnlSearch.Visible = false;
            pnlResults.Visible = true;
            pnlDetail.Visible = false;
            Employee1.DataLinkID = 0;
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
            PositionPosting1.positionPostingID = Int32.Parse(GridView1.SelectedDataKey[0].ToString());
            PositionPosting1.DataBind();
            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed positionPostingID:{0}", PositionPosting1.positionPostingID),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
            Employee1.DataLinkID = Int32.Parse(GridView1.SelectedValue.ToString());
            Employee1.Visible = true;
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

        protected void PositionPostings1_DataBinding(object sender, EventArgs e)
        {
            Notes1.AllowEdit =
                Documents1.AllowEdit =
                PositionPosting1.AllowEdit =
                Employee1.AllowEdit = 
                CanEdit;
            Notes1.AllowDelete =
                Documents1.AllowDelete =
                PositionPosting1.AllowDelete =
                Employee1.AllowDelete = 
                CanDelete;
        }
    }
}
