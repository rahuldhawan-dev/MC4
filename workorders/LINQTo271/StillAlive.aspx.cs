using System;
using System.Web.UI;

namespace LINQTo271
{
    public partial class StillAlive : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("DEFINED COMPILER CONSTANTS:<br/>");
            #if DEBUG
            Response.Write("DEBUG<br/>");
            #endif
            #if STAGING
            Response.Write("STAGING<br/>");
            #endif
            #if LOCAL
            Response.Write("LOCAL<br/>");
            #endif
        }
    }
}
