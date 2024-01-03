using System;
using System.Web.UI;

namespace LINQTo271
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("/Login.aspx?" + Request.QueryString);
        }
    }
}
