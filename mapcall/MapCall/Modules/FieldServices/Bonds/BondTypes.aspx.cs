using System;
using System.Web.UI;

namespace MapCall.Modules.FieldServices.Bonds
{
    public partial class BondTypes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("~/Modules/Mvc/FieldOperations/BondType/Search");
        }
    }
}
