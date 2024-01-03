using System;
using System.Web.UI;

namespace MapCall
{
    public partial class iMap : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();
        }
    }
}
