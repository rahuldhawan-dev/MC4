using System;
using System.Web.UI;
using mod = MapCall.Common.Utility.Permissions.Modules;

namespace MapCall.Modules.FieldServices
{
    public partial class SewerManholes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Mvc/FieldOperations/SewerManhole/Search");
        }
    }
}
