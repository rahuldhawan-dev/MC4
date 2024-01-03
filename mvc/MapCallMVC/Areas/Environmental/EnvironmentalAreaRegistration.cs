using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.Environmental
{
    public class EnvironmentalAreaRegistration : BaseAreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            var controllerNameSpaces = GetControllerNamespace();

            context.Routes.AddManyToManyRoutes("AllocationPermitWithdrawalNode", "Equipment", controllerNameSpaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("AllocationPermitWithdrawalNode", "AllocationPermit", controllerNameSpaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("EnvironmentalPermit", "EnvironmentalPermitFee", controllerNameSpaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("EnvironmentalPermit", "Equipment", controllerNameSpaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("EnvironmentalPermit", "Facility", controllerNameSpaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("EnvironmentalPermit", "EnvironmentalPermitRequirement", controllerNameSpaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("EnvironmentalNonComplianceEvent", "EnvironmentalNonComplianceEventActionItem", controllerNameSpaces, area: AreaName, delete: false);

            base.RegisterArea(context);
        }
    }
}