using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Modules.HR.Administrator
{
    public partial class Log : Page
    {
        public string Filter
        {
            get
            {
                if (this.ViewState["Filter"] != null)
                    return this.ViewState["Filter"].ToString();
                else
                    return null;
            }
            set { this.ViewState["Filter"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //WebProfile wp = new WebProfile((System.Web.Profile.ProfileBase)System.Web.Profile.ProfileBase.Create(Page.User.Identity.Name));
            //Response.Write(wp.FullName);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
        }
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = true;
            pnlResults.Visible = false;
            pnlDetail.Visible = false;
        }
        protected void btnBackToResults_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = false;
            pnlResults.Visible = true;
            pnlDetail.Visible = false;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            if (!String.IsNullOrEmpty(txtDate.EndDate.ToString()))
                sb.Append(txtDate.FilterExpression("CreatedOn"));
            if (ddlCategory.SelectedIndex > 0)
                sb.Append(String.Format(" AND AuditCategoryID = {0}", ddlCategory.SelectedValue));
            if (ddlUser.SelectedIndex > 0)
                sb.Append(String.Format(" AND CreatedBy = '{0}'", ddlUser.SelectedValue));
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
        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.Filter))
                SqlDataSource1.FilterExpression = Filter;
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
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

