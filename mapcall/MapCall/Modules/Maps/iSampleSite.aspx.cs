using System;
using System.Web.UI;

namespace MapCall.Modules.Maps
{
    public partial class iSampleSite : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            head.DataBind();
            int id = Int32.Parse(Request["RecordID"]);
            DataElement1.DataElementID = id;
            Documents1.DataLinkID = id;
            Notes1.DataLinkID = id;
        }
        protected void DataElement1_ItemInserted(object sender, EventArgs e)
        {
             
        }
    }
}
