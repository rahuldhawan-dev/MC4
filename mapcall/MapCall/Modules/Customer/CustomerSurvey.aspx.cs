using System;
using System.Configuration;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.DataPages;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Controls.Data;
using MapCall.Controls.HR;

namespace MapCall.Modules.Customer
{
    public partial class CustomerSurvey : DataElementPage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.Customer.General;
            }
        }

        #endregion

        #region Private Methods

        private void ShowDetailView(string param)
        {
            var id = Int32.Parse(param);

            DataElement1.ChangeMode(DetailsViewMode.ReadOnly);
            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed {0} ID:{1}", "Customer Survey",
                              id),
                ConfigurationManager.ConnectionStrings["MCProd"].ToString()
                );

            DataElement1.DataElementID = Notes1.DataLinkID = Documents1.DataLinkID = id;
            DataElement1.DataBind();
            Notes1.Visible = Documents1.Visible = true;

            pnlSearch.Visible = pnlResults.Visible = false;
            pnlDetail.Visible = true;
        }

        #endregion

        #region Event Handlers

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!CanView)
            {
                pnlSearch.Visible = false;
                lblPermissionErrors.Text = String.Format("Access Denied => {0} : {1}", ModulePermissions.Application,
                                                         ModulePermissions.Module);
            }
            btnAdd.Visible =
                Notes1.AllowAdd =
                Documents1.AllowAdd = CanAdd;


            //Handle "arg"
            if (!IsPostBack)
            {
                var param = Request.QueryString["arg"];
                if (param != null)
                    ShowDetailView(param);
            }
        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            foreach (Control ctrl in pnlSearch.Controls)
            {
                if (ctrl is DataField)
                    sb.Append(((IDataField) ctrl).FilterExpression());
            }

            if (sb.Length > 0)
            {
                SqlDataSource1.FilterExpression = sb.ToString().Substring(5);
                this.Filter = SqlDataSource1.FilterExpression;
                hidFilter.Value = this.Filter;
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
            DataElement1.DetailsView1.DataBind();

            Audit.Insert(
                (int)AuditCategory.DataView,
                Page.User.Identity.Name,
                String.Format("Viewed {0} ID:{1}", DataElement1.DataElementName,
                              DataElement1.DataElementID),
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

        protected void DataElement1_DataBinding(object sender, EventArgs e)
        {
            Notes1.AllowAdd =
                Documents1.AllowAdd = DataElement1.AllowNew = CanAdd;
            Notes1.AllowEdit =
                Documents1.AllowEdit =
                DataElement1.AllowEdit =
                CanEdit;
            Notes1.AllowDelete =
                Documents1.AllowDelete =
                DataElement1.AllowDelete =
                CanDelete;
        }

        protected void DataElement1_PreInit(Object sender, EventArgs e)
        {
            ((DataElement)sender).AllowDelete = CanDelete;
        }

        #endregion
    }
}
