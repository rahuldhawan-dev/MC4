using System;
using System.Web.UI;

namespace MapCall
{
    public partial class logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // We don't want logout.aspx as part of the return url or else the user will
            // get stuck in a loop when they try to log in.
            ((Global)Context.ApplicationInstance).LogOutUser(includeReturnUrl: false);
        }
    }
}
