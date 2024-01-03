using System;
using System.Web.UI;

namespace MapCall
{
    public partial class h : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Data/Hydrants/HydrantLink.aspx?" + Request.QueryString);
        }
    }
}
