using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.HealthAndSafety
{
    public class HealthAndSafetyAreaRegistration : BaseAreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            var namespaces = GetControllerNamespace();
            context.Routes.AddManyToManyRoutes("GasMonitor", "GasMonitorCalibration", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("ConfinedSpaceForm", "ConfinedSpaceFormEntrant", namespaces, area: AreaName);
            base.RegisterArea(context);
        }
    }
}
