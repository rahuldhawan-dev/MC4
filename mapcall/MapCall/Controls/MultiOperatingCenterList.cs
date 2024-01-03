using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Controls.Data;

namespace MapCall.Controls
{
    [ParseChildren(true)]
    public class MultiOperatingCenterList : MultiCheckBoxList
    {

        protected override void OnInit(EventArgs e)
        {
            var ds = new SqlDataSource();
            ds.ConnectionString = ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString;
            ds.SelectCommand = "select OperatingCenterID, OperatingCenterCode + ' - ' + OperatingCenterName as OpCntr from OperatingCenters order by OperatingCenterCode";

            this.DataSource = ds;

            DataTextField = "OpCntr";
            DataValueField = "OperatingCenterID";

            // Call the base after creating data sources.
            base.OnInit(e);
        }
    }
}