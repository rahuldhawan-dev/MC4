using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MapCall.Modules.Maps
{
    public partial class iEngineeringProjectRP : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lnkLightviewCss.Href = ResolveUrl("~/includes/lightview-3.2.7/css/lightview/lightview.css");
            int id = Int32.Parse(Request["RecordID"]);
            DataElement1.DataElementID = id;
            ntsMain.DataLinkID = id;
            dcsMain.DataLinkID = id;
        }
        protected void deMain_DataBinding(object sender, EventArgs e)
        {

        }
    }
}