using System;
using System.Web.UI;

namespace MapCall.Modules.Maps
{
    public partial class iWQComplaint : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            head.DataBind();
            int id = Int32.Parse(Request["RecordID"]);
            hl.NavigateUrl = string.Format("~/Modules/mvc/WaterQuality/WaterQualityComplaint/Show/" + id);
           // btnRecentActivity.OnClientClick=String.Format("ComplaintRecentActivity({0});return false;", id);
            DataElement1.DataElementID = id;
            //Documents1.DataLinkID = id;
            //Notes1.DataLinkID = id;
        }
        protected void DataElement1_ItemInserted(object sender, EventArgs e)
        {

        }
    }
}
