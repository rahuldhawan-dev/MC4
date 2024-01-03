using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.ClassExtensions.IOrderedDictionaryExtensions;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Controls.Data;

namespace MapCall.Modules.FieldServices
{
    public partial class MeterTests : DataElementPageWithDetailView
    {
        #region Constants

        private struct DataElement
        {
            internal const string DataElementName = "MeterTest";
            internal const string DataElementID = "MeterTestID";
        }

        #endregion

        #region Private Members

        private string _connectionString;

        #endregion

        #region Properties

        public override DetailsView DetailView
        {
            get { return dvMeterTest; }
        }

        public override SqlDataSource DataSource
        {
            get { return dsMeterTest; }
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

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.FieldServices.Meters;
            }
        }

        #endregion

        #region Private Methods

        private void ShowDetailView(string param)
        {
            var id = Int32.Parse(param);

            DetailView.ChangeMode(DetailsViewMode.ReadOnly);
            dsMeterTest.SelectParameters[0].DefaultValue = id.ToString();
            DetailView.DataBind();
            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed {0} ID:{1}", DataElement.DataElementName,
                              id),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
                );

            BindRelatedControls(id);

            Notes1.DataLinkID = id;
            Notes1.Visible = true;
            Documents1.DataLinkID = id;
            Documents1.Visible = true;

            pnlSearch.Visible = false;
            pnlResults.Visible = false;
            pnlDetail.Visible = true;
        }


        private void ValidateUserRights()
        {
            if (!CanView)
            {
                pnlSearch.Visible = false;
                lblPermissionErrors.Text = String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module);
            }

            btnAdd.Visible = Notes1.AllowAdd = Documents1.AllowAdd = CanAdd;
        }

        private void BindRelatedControls(int id)
        {
            MeterTestResults1.MeterTestID = id;
            dsPreviousTests.SelectParameters[0].DefaultValue =
                dsPremiseTests.SelectParameters[0].DefaultValue =
                dsMeterTests.SelectParameters[0].DefaultValue = id.ToString();

            MeterTestResults1.Visible = true;
            MeterTestResults1.DataBind();
            dsPreviousTests.DataBind();
            dsPremiseTests.DataBind();
            dsMeterTests.DataBind();
        }

        #endregion

        #region Event Handlers

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            base.btnAdd_Click(sender, e);
            BindRelatedControls(-1);
        }

        protected override void Page_Load(object sender, EventArgs e)
        {
            ValidateUserRights();

            //Handle "arg"
            if (IsPostBack) return;
            var param = Request.QueryString["arg"];
            if (param != null)
            {
                ShowDetailView(param);

            }
        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            if (ddlOpCntr.SelectedIndex > 0)
                sb.AppendFormat(" AND OperatingCenterID = {0}", ddlOpCntr.SelectedValue);

            var selectedTown = IOrderedDictionaryExtensions.CleanValue(cddTowns.SelectedValue);
            if (!string.IsNullOrEmpty(selectedTown))
                sb.AppendFormat(" AND TownID = {0}", selectedTown);

            var selectedStreet = IOrderedDictionaryExtensions.CleanValue(cddStreets.SelectedValue);
            if (!string.IsNullOrEmpty(selectedStreet))
                sb.AppendFormat(" And StreetID = {0}", selectedStreet);

            foreach (var ctrl in pnlSearch.Controls.OfType<DataField>())
            {
                sb.Append(ctrl.FilterExpression());
            }

            Filter = hidFilter.Value = (sb.Length > 0) ? sb.ToString().Substring(5) : String.Empty;
            SqlDataSource1.FilterExpression = Filter;
            base.btnSearch_Click(sender, e);
            lblRecordCount.Text = String.Format("Total Records: {0}",
                                                GridView1.Rows.Count);
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = Int32.Parse(GridView1.SelectedDataKey[0].ToString());
            DetailView.ChangeMode(DetailsViewMode.ReadOnly);
            DataSource.SelectParameters[DataElement.DataElementID].DefaultValue =
                id.ToString();
            DetailView.DataBind();

            BindRelatedControls(id);

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

        protected override void DataElement1_ItemInserted(object sender, EventArgs e)
        {
            base.DataElement1_ItemInserted(sender, e);
            var id = (int)GridView1.SelectedDataKey.Value;
            BindRelatedControls(id);
        }

        protected override void SqlDataSource1_Inserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            base.SqlDataSource1_Inserted(sender, e);

            var id = int.Parse(e.Command.Parameters[0].Value.ToString());
            BindRelatedControls(id);

            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Added {0} ID:{1}", DataElement.DataElementName, id),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
                );
        }

        protected void DataElement1_DataBinding(object sender, EventArgs e)
        {
            Notes1.AllowAdd =
                Documents1.AllowAdd = CanAdd;
            Notes1.AllowEdit =
                Documents1.AllowEdit =
                CanEdit;
            Notes1.AllowDelete =
                Documents1.AllowDelete =
                CanDelete;
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
    }
}

