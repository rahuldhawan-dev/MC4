using System;
using System.Configuration;
using System.Text;
using System.Web.UI.WebControls;
using MMSINC;
using MMSINC.DataPages;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Controls.Data;

namespace MapCall.Modules.HR.Administrator
{
    public partial class BusPerformanceGoals : DataElementPageWithRoles
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.BusinessPerformance.General;
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
            btnAdd.Visible =
                Notes1.AllowAdd =
                Documents1.AllowAdd = CanAdd;
            
            CheckQueryString(DataElement1, Documents1, Notes1, pnlDetail, pnlSearch, pnlResults);
        }
        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

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

            //if (ddlOrderBy.SelectedIndex > 0)
            //    GridView1.Sort(ddlOrderBy.SelectedValue, SortDirection.Ascending);

            base.btnSearch_Click(sender, e);
            lblRecordCount.Text = String.Format("Total Records: {0}", GridView1.Rows.Count.ToString());
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataElement1.ChangeViewMode(DetailsViewMode.ReadOnly);
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

        protected void btnAddStrategicElement_Click(object sender, EventArgs e)
        {
            dsGoalsStrategicElements.Insert();
            dsGoalsStrategicElements.DataBind();
        }   

        #endregion
    }
}
