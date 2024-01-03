using MMSINC.ClassExtensions;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.FleetManagement
{
    public class FleetManagementAreaRegistration : BaseAreaRegistration 
    {
        public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context)
        {
            context.Routes.AddManyToManyRoutes("Vehicle", "VehicleAudit", GetControllerNamespace(), area: AreaName);
            base.RegisterArea(context);
        }
    }
}