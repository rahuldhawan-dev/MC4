using System;
using System.Text;
using MMSINC.DataPages;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Controls.Data;
using dotnetCHARTING;

namespace MapCall.Reports.Data.Services
{
    public partial class FireServiceEstimatedBackBilling : DataElementPage
    {
        #region Properties

        protected override IModulePermissions ModulePermissions
        {
            get
            {
                return Common.Utility.Permissions.Modules.FieldServices.MeterChangeOuts;
            }
        }

        #endregion

        #region Exposed Methods

        protected override void Page_Load(object sender, EventArgs e)
        {
            if (!CanView)
            {
                pnlSearch.Visible = false;
                lblPermissionErrors.Text = String.Format("Access Denied => {0} : {1}", ModulePermissions.Application, ModulePermissions.Module);
            }
            
            //Call the search_click if the refresh button on the chart is clicked.
            cws.ChartRefreshClick += btnSearch_Click;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            //Chart Setup
            cws.ChartDataSource = SqlDataSource1;
            cws.ChartDataSource.FilterExpression = hidFilter.Value;
            cws.ColumnNames = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            cws.SeriesNames = new[] { "Year", "AreaCodeDescription", "DistrictCode" };
            cws.SeriesDefault = SeriesType.Line;
            cws.ShowSettingsDiv = true;
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

        #endregion
    }
}
