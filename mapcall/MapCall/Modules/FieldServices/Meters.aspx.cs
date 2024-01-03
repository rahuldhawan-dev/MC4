using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.DataPages;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;

namespace MapCall.Modules.FieldServices
{
    public partial class Meters : DataElementPageWithDetailView
    {
        #region Constants
        
        private struct DataElement
        {
            internal const string DataElementName = "Meter";
            internal const string DataElementID = "MeterID";
        }

        #endregion

        #region Private Members

        private string _connectionString;
   
        #endregion

        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.FieldServices.Meters;
            }
        }

        public override DetailsView DetailView
        {
            get { return dvMeter; }
        }

        public override SqlDataSource DataSource
        {
            get { return dsMeter; }
        }

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                    _connectionString =
                        ConfigurationManager.ConnectionStrings["MCProd"].ToString();
                return _connectionString;
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

            btnAdd.Visible =
                Notes1.AllowAdd =
                Documents1.AllowAdd = CanAdd;
        }

        #endregion

        #region Event Handlers

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ValidateUserRights();

            if (!IsPostBack)
            {
                // This is being done to match how DataPageBase will handle it.
                var queryMeterId = Request.QueryString["view"];
                int meterId;
                if (int.TryParse(queryMeterId, out meterId))
                {
                    ShowDetailView(meterId);   
                }

            }

        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (var ctrl in pnlSearch.Controls.OfType<IDataField>())
            {
                    sb.Append(ctrl.FilterExpression());
            }

            if (!String.IsNullOrEmpty(ddlMeterProfileSearch.SelectedValue))
            {
                sb.AppendFormat(" AND MeterProfileID = {0}", int.Parse(ddlMeterProfileSearch.SelectedValue));
            }

            if (!string.IsNullOrEmpty(ddlMeterStatus.SelectedValue))
            {
                sb.AppendFormat(" AND Status = {0}", int.Parse(ddlMeterStatus.SelectedValue));
            }

            Filter = hidFilter.Value = (sb.Length > 0) ? sb.ToString().Substring(5) : String.Empty;
            SqlDataSource1.FilterExpression = Filter;
            base.btnSearch_Click(sender, e);
            lblRecordCount.Text = String.Format("Total Records: {0}",
                                                GridView1.Rows.Count);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = int.Parse(GridView1.SelectedDataKey.Value.ToString());
            ShowDetailView(id);
        }

        private void ShowDetailView(int meterId)
        {
            DetailView.ChangeMode(DetailsViewMode.ReadOnly);
            DataSource.SelectParameters[DataElement.DataElementID].DefaultValue = dsRelatedMeterTests.SelectParameters[DataElement.DataElementID].DefaultValue = meterId.ToString();
            DetailView.DataBind();
            dsRelatedMeterTests.DataBind();
      
            Audit.Insert(
                            (int)AuditCategory.DataView,
                            Page.User.Identity.Name,
                            String.Format("Viewed {0} ID:{1}", DataElement.DataElementName, meterId),
                            ConfigurationManager.ConnectionStrings["MCProd"].ToString()
                            );

            Notes1.DataLinkID = meterId;
            Notes1.Visible = true;
            Documents1.DataLinkID = meterId;
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
            dsRelatedMeterTests.SelectParameters[0].DefaultValue = id.ToString();
            gvRelatedMeterTests.Visible = true;
            gvRelatedMeterTests.DataBind();

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

