using System.Web.UI;
using MapCall.Common.Controls;

namespace MapCall.Modules.Maps
{
    public partial class RealTimeOperations : AssetLatLonPage
    {
        protected void Page_Load()
        {
            DataBind();
        }
    }
}
