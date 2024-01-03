using System;
using System.Configuration;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.ClassExtensions.IOrderedDictionaryExtensions;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using mod = MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls.Data;

namespace MapCall.Modules.FieldServices
{
    public partial class StormWaterAssets : DataElementPageWithDetailView
    {
        #region Constants

        private struct DataElement
        {
            internal const string DataElementName = "StormWaterAsset";
            internal const string DataElementID = "StormWaterAssetID";
        }

        #endregion

        #region Properties

        public string Filter
        {
            get
            {
                return ViewState["Filter"] != null ? ViewState["Filter"].ToString() : null;
            }
            set { ViewState["Filter"] = value; }
        }

        public override DetailsView DetailView
        {
            get { return dvStormWaterAsset; }
        }

        public override SqlDataSource DataSource
        {
            get { return dsStormWaterAsset; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return mod.FieldServices.Assets;
            }
        }

        #endregion

        #region Private Methods

        private void ValidateUserRights()
        {
            if (!CanView)
            {
                pnlSearch.Visible = false;
                lblPermissionErrors.Text = String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module);
            }

            btnAdd.Visible = Notes1.AllowAdd = Documents1.AllowAdd = CanAdd;
        }

        #endregion

        #region Event Handlers

        protected override void Page_Load(object sender, EventArgs e)
        {
            ValidateUserRights();
        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (Control ctrl in pnlSearch.Controls)
            {
                if (ctrl is DataField)
                    sb.Append(((DataField)ctrl).FilterExpression());
            }

            if (ddlOpCntr.SelectedIndex > 0)
                sb.AppendFormat(" AND OperatingCenterID = {0}", ddlOpCntr.SelectedValue);
            else
            {
                var opCntrs = new StringBuilder();
                foreach (ListItem item in ddlOpCntr.Items)
                    opCntrs.Append(item.Value + ",");
                sb.AppendFormat(" AND OperatingCenterID in ({0})", opCntrs.ToString().TrimEnd(',').TrimStart(','));
            }

            var selectedTown = IOrderedDictionaryExtensions.CleanValue(cddTowns.SelectedValue);
            if (!string.IsNullOrEmpty(selectedTown))
                sb.AppendFormat(" AND TownID = {0}", selectedTown);

            var selectedStreet = IOrderedDictionaryExtensions.CleanValue(cddStreets.SelectedValue);
            if (!string.IsNullOrEmpty(selectedStreet))
                sb.AppendFormat(" And StreetID = {0}", selectedStreet);

            Filter = (sb.Length > 0) ? sb.ToString().Substring(5) : String.Empty;
            SqlDataSource1.FilterExpression = Filter;
            base.btnSearch_Click(sender, e);
            lblRecordCount.Text = String.Format("Total Records: {0}",
                                                GridView1.Rows.Count);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = int.Parse(GridView1.SelectedDataKey.Value.ToString());
            DetailView.ChangeMode(DetailsViewMode.ReadOnly);
            DataSource.SelectParameters[DataElement.DataElementID].
                DefaultValue = id.ToString();
            DetailView.DataBind();

            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed {0} ID:{1}", DataElement.DataElementName, id),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
                );

            Notes1.DataLinkID = id;
            Notes1.Visible = true;

            Documents1.DataLinkID = id;
            Documents1.Visible = true;

            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
        }

        #region Data

        protected override void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            base.SqlDataSource1_Inserted(sender, e);
            var id = int.Parse(e.Command.Parameters[0].Value.ToString());
            Audit.Insert(
            (int)AuditCategory.DataView,
            Page.User.Identity.Name,
            String.Format("Added {0} ID:{1}", DataElement.DataElementName, id),
            ConfigurationManager.ConnectionStrings["MCProd"].ToString()
            );
        }

        protected void DetailView_DataBound(object sender, EventArgs e)
        {
            var name = Page.User.Identity.Name;
            var opCntr = MMSINC.Utility.GetFirstControlInstance(DetailView,
                                                                "lblOpCntr");
            if (opCntr == null) return;

            Notes1.AllowEdit =
                Documents1.AllowEdit = CanEdit;
            Notes1.AllowDelete =
                Documents1.AllowDelete = CanDelete;

            var btnEdit = MMSINC.Utility.GetFirstControlInstance(DetailView, "btnEdit");
            var btnDelete = MMSINC.Utility.GetFirstControlInstance(DetailView, "btnDelete");

            if (btnEdit != null)
                btnEdit.Visible = CanEdit;
            if (btnDelete != null)
                btnDelete.Visible = CanDelete;
        }

        #endregion

        #endregion

    }
}
