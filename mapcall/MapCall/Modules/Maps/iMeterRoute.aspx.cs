using System;
using System.Web.UI;

namespace MapCall.Modules.Maps
{
    public partial class iMeterRoute : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            int id = Int32.Parse(Request["recordID"]);
            DataElement1.DataElementID = id;
            ntsMain.DataLinkID = id;
            dcsMain.DataLinkID = id;
        }
    }
}