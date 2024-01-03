using System;
using System.Web.UI;

namespace MapCall.Modules.Data.Hydrants
{
    public partial class HydrantLink : Page
    {
        //TODO: Remove this page entirely and let American Water GIS know the new direct location.
        protected void Page_Load(object sender, EventArgs e)
        {
            // TODO: What's with all th empty querystring parameters?
            Response.Redirect(String.Format("~/modules/mvc/FieldOperations/Hydrant?OperatingCenter=&HydrantNumber.Value={0}&HydrantNumber.MatchType=0&StreetNumber=&HydrantSuffix=&WorkOrderNumber=&HydrantStatus=&DateInstalled.Start=&DateInstalled.Operator=0&DateInstalled.End=&UpdatedAt.Start=&UpdatedAt.Operator=0&UpdatedAt.End=&MapPage=&HydrantManufacturer=&HydrantBilling=&OutOfService=&RequiresInspection=", Request.QueryString["HydNum"]));
        }
    }
}
