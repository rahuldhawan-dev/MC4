using System;
using System.Configuration;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls.Data;
using MapCall.Controls.HR;

namespace MapCall.Modules
{
    public partial class Notifications : DataElementPage
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

        #region Event Handlers

        protected override void Page_Load(object sender, EventArgs e)
        {
            //Everyone can view
            btnAdd.Visible = CanAdd;
            btnMap.Visible = false;
        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Control ctrl in pnlSearch.Controls)
            {
                if (ctrl is DataField)
                    sb.Append(((DataField)ctrl).FilterExpression());
            }


            Filter = (sb.Length > 0) ? sb.ToString().Substring(5) : String.Empty;
            SqlDataSource1.FilterExpression = Filter;
            base.btnSearch_Click(sender, e);
            lblRecordCount.Text = String.Format("Total Records: {0}",
                                                GridView1.Rows.Count);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataElement1.SetDetailsViewMode(DetailsViewMode.ReadOnly);
            DataElement1.DataElementID = Int32.Parse(GridView1.SelectedDataKey[0].ToString());
            DataElement1.DataBind();
            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed {0} ID:{1}", DataElement1.DataElementName,
                              DataElement1.DataElementID),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
                );

            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
        }

        protected void DataElement1_DataBinding(object sender, EventArgs e)
        {
            DataElement1.AllowNew = CanAdd;
            DataElement1.AllowEdit = CanEdit;
            DataElement1.AllowDelete = CanDelete;
        }

        protected void DataElement1_PreInit(Object sender, EventArgs e)
        {
            ((DataElement)sender).AllowDelete = CanDelete;
        }

        #endregion
    }
}

