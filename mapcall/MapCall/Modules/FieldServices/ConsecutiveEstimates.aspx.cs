using System;
using System.Configuration;
using System.Text;
using MMSINC;
using MMSINC.DataPages;
using MMSINC.Extensions;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls.Data;
using MapCall.Controls.HR;

namespace MapCall.Modules.FieldServices
{
    public partial class ConsecutiveEstimates : DataElementPage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get { return HumanResources.SampleSites; }
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
        protected void DataElement1_PreInit(Object sender, EventArgs e)
        {
            ((DataElement)sender).AllowDelete = Page.User.IsAdmin();
        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (Object ctrl in pnlSearch.Controls)
                if (ctrl is DataField)
                    sb.Append(((IDataField)ctrl).FilterExpression());

            if (sb.Length > 0)
            {
                SqlDataSource1.FilterExpression = sb.ToString().Substring(5);
                Filter = SqlDataSource1.FilterExpression;
                hidFilter.Value = Filter;
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
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataElement1.DataElementID = Int32.Parse(GridView1.SelectedDataKey[0].ToString());
            DataElement1.DataBind();
            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed {0} ID:{1}", DataElement1.DataElementName, DataElement1.DataElementID),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );

            Notes1.DataLinkID = DataElement1.DataElementID;
            Notes1.Visible = true;
            Documents1.DataLinkID = DataElement1.DataElementID;
            Documents1.Visible = true;

            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
        }

    }
}
