using System;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.DataPages;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Controls.Data;
using MapCall.Controls.HR;

namespace MapCall.Modules.WaterQuality
{
    public partial class Complaints : DataElementPage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.WaterQuality.General;
            }
        }

        #endregion

        #region Event Handlers

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!CanView)
            {
                pnlSearch.Visible = false;
                lblPermissionErrors.Text = String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module);
            }
        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            if (ddlComplaintStatus.SelectedValue == "0")
                sb.Append(String.Format(" And Complaint_Close_Date is null"));
            if (ddlComplaintStatus.SelectedValue == "1")
                sb.Append(String.Format(" And Complaint_Close_Date is not null"));

            if (ddlComplaintCategory.SelectedIndex > 0)
                sb.AppendFormat(" AND WQ_Complaint_Type in ({0})", ddlComplaintCategory.SelectedValue);

            foreach (Object ctrl in pnlSearch.Controls)
                if (ctrl is DataField)
                    sb.Append(((IDataField)ctrl).FilterExpression());

            if (sb.Length > 0)
            {
                SqlDataSource1.FilterExpression = sb.ToString().Substring(5);
                this.Filter = SqlDataSource1.FilterExpression;
                hidFilter.Value = this.Filter;
            }
            else
            {
                SqlDataSource1.FilterExpression = String.Empty;
                this.Filter = String.Empty;
                hidFilter.Value = this.Filter;
            }

            base.btnSearch_Click(sender, e);
            lblRecordCount.Text = String.Format("Total Records: {0}", GetRecordCount(SqlDataSource1));
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

        protected void DataElement1_PreInit(Object sender, EventArgs e)
        {
            // TODO: Replace when the db merge is done.
            ((DataElement) sender).AllowDelete = CanDelete;
        }

        #endregion
    }
}