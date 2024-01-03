using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Page;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;

namespace MapCall.Modules.WaterQuality
{
    public partial class SampleResultsBactiInput : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Mvc/WaterQuality/BacterialWaterSample/Search");

        }
    }
}
