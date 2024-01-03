using System;
using System.Web.UI;

namespace MapCall.Modules.Maps
{
    public partial class iEvent : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkLightviewCss.Href = ResolveUrl("~/includes/lightview-3.2.7/css/lightview/lightview.css");
            int id = Int32.Parse(Request["RecordID"]);
            hl.NavigateUrl = string.Format("~/Modules/mvc/Events/Event/Show/" + id);
            DataElement1.DataElementID = id;
            ntsMain.DataLinkID = id;
            dcsMain.DataLinkID = id;
        }
    }
}
