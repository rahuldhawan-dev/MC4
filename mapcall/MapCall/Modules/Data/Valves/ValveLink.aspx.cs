using System;
using System.Web.UI;

namespace MapCall.Modules.Data.Valves
{
    public partial class ValveLink : Page
    {
        //TODO: Remove this page entirely and let American Water GIS know the new direct location.
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Redirect(String.Format("~/modules/mvc/FieldOperations/Valve?OperatingCenter=&ValveNumber.Value={0}" +
            //                                "&ValveNumber.MatchType=0&WorkOrderNumber=&StreetNumber=&DateInstalled.Start=&" +
            //                                "DateInstalled.Operator=0&DateInstalled.End=&MapPage=&ValveStatus=&ValveSize=&" +
            //                                "ValveControls=&ValveBilling=&ValveZone=&LastUpdated.Start=&LastUpdated.Operator=0&" +
            //                                "LastUpdated.End=&FunctionalLocationDescription=&Traffic=&RequiresInspection=&" +
            //                                "IsLargeValve=&RequiresBlowOffInspection=&Critical=&LastInspectionBroken=", Request.QueryString["ValNum"]));
        }
    }
}
