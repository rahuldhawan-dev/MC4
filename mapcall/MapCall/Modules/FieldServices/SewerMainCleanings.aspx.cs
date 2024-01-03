using System;
using System.Web.UI;
using MMSINC.Page;
using mod = MapCall.Common.Utility.Permissions.Modules;

namespace MapCall.Modules.FieldServices
{
    public partial class SewerMainCleanings : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Mvc/FieldOperations/SewerMainCleaing/Search");
        }
    }
}
