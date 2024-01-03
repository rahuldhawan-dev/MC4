using System;
using System.Web.UI;

namespace MapCall
{
    public partial class wo : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(string.Format(
                "~/modules/WorkOrders/Views/WorkOrders/ReadOnly/WorkOrderReadOnlyRPCPage.aspx?cmd=view&arg={0}", 
                Request.QueryString["ID"]));
        }
    }
}
