using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Controls.Data;

namespace MapCall.Controls
{
    [ParseChildren(true)]
    public class MultiHighCostFactorsList : MultiCheckBoxList
    {
        protected override void OnInit(EventArgs e)
        {
            var ds = new SqlDataSource();
            ds.ConnectionString = ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString;
            ds.SelectCommand = "Select HighCostFactorID, Description from HighCostFactors order by [Description]";

            this.DataSource = ds;

            DataTextField = "Description";
            DataValueField = "HighCostFactorID";

            base.OnInit(e);
        }
    }
}