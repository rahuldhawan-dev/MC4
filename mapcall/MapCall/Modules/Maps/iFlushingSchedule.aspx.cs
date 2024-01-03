using System;
using System.Web.UI;

namespace MapCall.Modules.Maps
{
    public partial class iFlushingSchedule : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request["ID"]);
            DataElement1.DataElementID = id;
            ntsMain.DataLinkID = id;
            dcsMain.DataLinkID = id;
        }
    }
}
