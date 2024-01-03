using System;
using System.Web.UI;

namespace MapCall
{
    public partial class ViewServiceReport : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("{0}?{1}&ok=1", Request.AppRelativeCurrentExecutionFilePath.Replace("~", "~/Reports/ActiveReports"), Request.QueryString.ToString()));
        }
    }
}
