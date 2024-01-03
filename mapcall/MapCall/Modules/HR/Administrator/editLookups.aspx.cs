using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;

namespace MapCall.Modules.HR.Administrator
{
    public partial class editLookups : DataElementRolePage
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
                Response.Write(String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module));
                Response.End();
            }
        }

        protected void GridView1_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) != 0)
                {
                    Control c = e.Row.Cells[3].Controls[0];
                    c.Focus();
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtAddLookupType.Text) && !string.IsNullOrEmpty(txtAddLookupValue.Text) && ddlAddTableName.SelectedIndex>0)
                SqlDataSource1.Insert();
            
        }
    }
}
