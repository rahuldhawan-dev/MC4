using System;
using System.Data;
using System.Text;
using System.Web.UI;
using MMSINC.DataPages;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Controls.Data;

namespace MapCall.Modules.Customer
{
    public partial class CustomerSurveyData : DataElementPage
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

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!CanView)
            {
                pnlSearch.Visible = false;
                lblPermissionErrors.Text = String.Format("Access Denied => {0} : {1}", ModulePermissions.Application,
                                                         ModulePermissions.Module);
            }
        }

        protected override void btnSearch_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            var sbOr = new StringBuilder();

            foreach (var ctrl in pnlSearch.Controls)
            {
                if (ctrl is ICustomerSurveyDataField)
                    sbOr.Append(((ICustomerSurveyDataField) ctrl).FilterExpression());
                else if (ctrl is IDataField)
                    sb.Append(((IDataField)ctrl).FilterExpression());
            }

            if (sbOr.Length > 0)
                sb.Append(" AND (" + sbOr.ToString().Substring(4) + ")");

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
            
            // TODO: This is a terrible way to get the rowcount. Paging FTW!
            var dv = (DataView) SqlDataSource1.Select(DataSourceSelectArguments.Empty);
            lblRecordCount.Text = String.Format("Total Records: {0}", dv.Count);
            dv.Dispose();
        }
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}

