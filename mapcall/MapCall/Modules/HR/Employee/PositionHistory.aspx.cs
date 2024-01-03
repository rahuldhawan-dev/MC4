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
    public partial class PositionHistory : DataElementRolePage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return HumanResources.PositionHistory;
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
            if (!String.IsNullOrEmpty(Request.QueryString["EmployeeID"]) && !IsPostBack)
                ManualSearch();
            btnAdd.Visible = Notes1.AllowAdd = Documents1.AllowAdd = CanAdd;

        }
        private void ManualSearch()
        {
            SqlDataSource1.FilterExpression = String.Format(" tblEmployeeID = {0}", Request.QueryString["EmployeeID"]);
            this.Filter = SqlDataSource1.FilterExpression;
            if (ddlOrderBy.SelectedIndex > 0)
                GridView1.Sort(ddlOrderBy.SelectedValue, SortDirection.Ascending);
            GridView1.PageIndex = 0;
            GridView1.DataBind();
            pnlSearch.Visible = false;
            pnlResults.Visible = true;
            pnlDetail.Visible = false;
            Documents1.DataLinkID = 0;
            Notes1.DataLinkID = 0;
        }
        protected void PositionHistory1_ItemInserted(object sender, EventArgs e)
        {
            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
            Notes1.DataLinkID = PositionHistory1.positionHistoryID;
            Notes1.Visible = true;
            Documents1.DataLinkID = PositionHistory1.positionHistoryID;
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
            PositionHistory1.ChangeMode(DetailsViewMode.Insert);
            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
            Documents1.Visible = false;
            Notes1.Visible = false;
            Notes1.DataLinkID = 0;
            Documents1.DataLinkID = 0;
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
            var sb = new StringBuilder();
            if (ddlOpCode.SelectedIndex > 0)
                sb.AppendFormat(" AND OpCode = '{0}'", ddlOpCode.SelectedValue.Replace("'", "''"));
            if (ddlLocal.SelectedIndex > 0)
                sb.AppendFormat(" AND LocalID = {0}", ddlLocal.SelectedValue);
            if (ddlStatus.SelectedIndex > 0)
                sb.AppendFormat(" AND Status = '{0}'", ddlStatus.SelectedValue.Replace("'", "''"));
            if (ddlStatus_Change_Reason.SelectedIndex > 0)
                sb.AppendFormat(" AND Status_Change_Reason = '{0}'", ddlStatus_Change_Reason.SelectedValue.Replace("'", "''"));
            if (ddlDepartment.SelectedIndex > 0)
                sb.AppendFormat(" AND Department = '{0}'", ddlDepartment.SelectedValue.Replace("'", "''"));
            if (ddlDepartmentName.SelectedIndex > 0)
                sb.AppendFormat(" AND DepartmentName = '{0}'", ddlDepartmentName.SelectedValue.Replace("'", "''"));
            if (ddlPositions.SelectedIndex > 0)
                sb.AppendFormat(" AND Position_ID = {0}", ddlPositions.SelectedValue);

            if (!string.IsNullOrWhiteSpace(txtPCN.Text))
                sb.AppendFormat(" AND PositionControlNumber like '%{0}%'", txtPCN.Text.Replace("'", "''"));
            if (!String.IsNullOrEmpty(txtEmployeeID.Text))
                sb.AppendFormat(" AND EmployeeID like '%{0}%'", txtEmployeeID.Text.Replace("'", "''"));
            if (!String.IsNullOrEmpty(txtLast_Name.Text))
                sb.AppendFormat(" AND Last_Name like '%{0}%'", txtLast_Name.Text.Replace("'", "''"));
            if (!String.IsNullOrEmpty(txtPosition_Posting_ID.Text))
                sb.AppendFormat(" AND Position_Posting_ID like '%{0}%'", txtPosition_Posting_ID.Text.Replace("'", "''"));
            if (!String.IsNullOrEmpty(txtPosition_Sub_Category.Text))
                sb.AppendFormat(" AND Position_Sub_Category like '%{0}%'", txtPosition_Sub_Category.Text.Replace("'", "''"));
            if (!String.IsNullOrEmpty(txtVacation_Grouping.Text))
                sb.AppendFormat(" AND Vacation_Grouping like '%{0}%'", txtVacation_Grouping.Text.Replace("'", "''"));
            
            if (!String.IsNullOrEmpty(txtPosition_Start_Date.EndDate))
                sb.Append(txtPosition_Start_Date.FilterExpression("Position_Start_Date"));
            if (!String.IsNullOrEmpty(txtPosition_End_Date.EndDate))
                sb.Append(txtPosition_End_Date.FilterExpression("Position_End_Date"));

            if (ddlReportingFacilityId.SelectedIndex > 0)
                sb.AppendFormat(" AND ReportingFacilityID = {0}", ddlReportingFacilityId.SelectedValue);
            if (ddlFully_Qualified.SelectedIndex > 0)
                sb.AppendFormat(" AND Fully_Qualified = {0}", ddlFully_Qualified.SelectedValue);
            if (ddlSchedule.SelectedIndex > 0)
                sb.AppendFormat(" AND ScheduleTypeID = '{0}'", ddlSchedule.SelectedValue);
            if (ddlLaborCategoryTypeID.SelectedIndex > 0)
                sb.AppendFormat(" AND LaborCategoryTypeID = {0}", ddlLaborCategoryTypeID.SelectedValue);
            if (ddlEmploymentAgencyTypeID.SelectedIndex > 0)
                sb.AppendFormat(" AND EmploymentAgencyTypeID = {0}", ddlEmploymentAgencyTypeID.SelectedValue);
            if (ddlEmploymentLevelTypeID.SelectedIndex > 0)
                sb.AppendFormat(" AND EmploymentLevelTypeID = {0}", ddlEmploymentLevelTypeID.SelectedValue);

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
            PositionHistory1.positionHistoryID = Int32.Parse(GridView1.SelectedDataKey[0].ToString());
            PositionHistory1.DataBind();
            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed positionHistoryID:{0}", PositionHistory1.positionHistoryID),
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

        protected void PositionHistory1_DataBinding(object sender, EventArgs e)
        {
            Notes1.AllowEdit =
                Documents1.AllowEdit =
                PositionHistory1.AllowEdit =
                CanEdit;
            Notes1.AllowDelete =
                Documents1.AllowDelete =
                PositionHistory1.AllowDelete =
                CanDelete;
        }
    }
}
