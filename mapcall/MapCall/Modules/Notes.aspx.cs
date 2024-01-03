using System;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;

namespace MapCall.Modules.HR
{
    public partial class Notes : DataElementRolePage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get { return HumanResources.Admin; }
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SqlDataSource1.FilterExpression = String.Format(" NOTE like '%{0}%'", txtSearch.Text);
            GridView1.DataBind();
            pnlDetail.Visible = false;
            pnlSearch.Visible = false;
            pnlResults.Visible = true;
        }
    }
}
