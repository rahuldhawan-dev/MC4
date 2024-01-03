using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Modules.Maps
{
    public partial class iSampleResult : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            int id = Int32.Parse(Request["recordID"]);
            hlResultDetails.NavigateUrl = "~/Modules/Mvc/WaterQuality/WaterSample/Show/" + id;
            DataElement1.DataElementID = id;
        }
    }
}