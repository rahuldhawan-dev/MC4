using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.ProjectManagement
{
    public class ProjectManagementAreaRegistration : BaseAreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            var namespaces = GetControllerNamespace();
            context.Routes.AddManyToManyRoutes("EstimatingProject", "EstimatingProjectOtherCost", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("EstimatingProject", "EstimatingProjectMaterial", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("EstimatingProject", "EstimatingProjectContractorLaborCost", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("EstimatingProject", "EstimatingProjectCompanyLaborCost", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("EstimatingProject", "EstimatingProjectPermit", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("ContractorLaborCost", "OperatingCenter", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("RecurringProject", "RecurringProjectEndorsement", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("RoadwayImprovementNotification", "RoadwayImprovementNotificationStreet", namespaces, area: AreaName);

            base.RegisterArea(context);
        }
    }
}
