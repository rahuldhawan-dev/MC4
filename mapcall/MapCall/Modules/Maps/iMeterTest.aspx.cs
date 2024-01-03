using System;
using System.Web.UI;
using MapCall.Controls.HR;

namespace MapCall.Modules.Maps
{
    public partial class iMeterTest : Page
    {
        #region Control Declarations

        protected MeterTestResults MeterTestResults1;

        #endregion
        
        #region Event Handlers

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = Int32.Parse(Request["RecordID"]);
            DataElement1.DataElementID = id;
            ntsMain.DataLinkID = id;
            dcsMain.DataLinkID = id;
            MeterTestResults1.MeterTestID = id;
        }

        #endregion

    }
}
