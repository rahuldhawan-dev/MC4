using System;
using System.Web.UI;

namespace MapCall.Modules.Maps
{
    public partial class iMeterRecorderStorageLocations : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            lnkLightviewCss.Href = ResolveUrl("~/includes/lightview-3.2.7/css/lightview/lightview.css");
            var id = Int32.Parse(Request.QueryString["recordID"]);
            DataElement1.DataElementID = id;
            ntsMain.DataLinkID = id;
            dcsMain.DataLinkID = id;
        }
    }
}
