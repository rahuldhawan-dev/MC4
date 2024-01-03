using System;
using System.Web.UI;

namespace MapCall.Modules.Maps
{
    public partial class iMeterRecorderHistory : Page
    {
        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request["RecordID"]);
            DataElement1.DataElementID = id;
            ntsMain.DataLinkID = id;
            dcsMain.DataLinkID = id;
        }

        #endregion
    }
}
