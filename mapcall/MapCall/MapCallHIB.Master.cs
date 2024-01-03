using System;
using System.Web.UI;

namespace MapCall
{
    public partial class MapCallHIB : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkLightviewCSS.Href = ResolveUrl("~/includes/lightview-3.2.7/css/lightview/lightview.css");
            Page.Header.DataBind();
        }
    }
}
