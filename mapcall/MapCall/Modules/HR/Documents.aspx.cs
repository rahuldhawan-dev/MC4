using System;
using System.Configuration;
using System.Text;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MapCall.Common.Model.Repositories;
using MMSINC;
using MMSINC.Utilities.Documents;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using StructureMap;

namespace MapCall.Modules.HR
{
    public partial class DocumentsSearch : DataElementRolePage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return HumanResources.Admin;
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
       
        protected void btnBackToSearch_Click(object sender, EventArgs e)
        {
            pnlSearch.Visible = true;
            pnlResults.Visible = false;
        }
        protected void btnBackToResults_Click(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = Filter;
            GridView1.DataBind();
            pnlSearch.Visible = false;
            pnlResults.Visible = true;
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            if (ddlDataType.SelectedIndex > 0)
                sb.Append(String.Format(" AND DataTypeID= '{0}'", ddlDataType.SelectedValue.Replace("'", "''")));
            if (ddlDocumentTypeID.SelectedIndex > 0)
                sb.Append(String.Format(" AND DocumentTypeID = '{0}'", ddlDocumentTypeID.SelectedValue));
            if (!String.IsNullOrEmpty(txtFileSize.End))
                sb.Append(txtFileSize.FilterExpression("File_Size"));
            if (!String.IsNullOrEmpty(txtFile_Name.Text))
                sb.Append(String.Format(" AND File_Name like '%{0}%'", txtFile_Name.Text.Replace("'", "''")));
            if (!String.IsNullOrEmpty(txtCreatedOn.EndDate.ToString()))
                sb.Append(txtCreatedOn.FilterExpression("CreatedAt"));

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
        }

        protected void GridView1_PageIndexChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.Filter))
                SqlDataSource1.FilterExpression = Filter;
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Select")
            {
                var tmpDocID = Int32.Parse(e.CommandArgument.ToString());
                Audit.Insert(
                    (int)AuditCategory.DataView,
                    Page.User.Identity.Name,
                    String.Format("Viewed DocumentID:{0}", tmpDocID),
                    ConfigurationManager.ConnectionStrings["MCProd"].ToString()
                );

                var docRepo = DependencyResolver.Current.GetService<IDocumentRepository>();
                var doc = docRepo.Find(tmpDocID);
                var file = DependencyResolver.Current.GetService<IDocumentService>().Open(doc.DocumentData.Hash);    

                Response.AddHeader("Content-disposition", String.Format("attachment;filename={0}", Server.UrlEncode(doc.FileName)));
                Response.ContentType = "application/octet-stream";
                Response.BinaryWrite(file);
            }
        
        }

        protected void GridView1_Sorting(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(this.Filter))
                SqlDataSource1.FilterExpression = Filter;
        }
    }
}
