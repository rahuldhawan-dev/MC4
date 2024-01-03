using System;
using System.Web.UI;
using MMSINC.Page;

namespace MapCall.Modules.WaterQuality
{
    public partial class SampleResultsBacti : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Mvc/WaterQuality/BacterialWaterSample/Search");
        }
    }
}
