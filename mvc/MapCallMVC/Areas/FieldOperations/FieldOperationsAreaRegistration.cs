using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.FieldOperations
{
    public class FieldOperationsAreaRegistration : BaseAreaRegistration
    {
        public const string AREA_NAME = "FieldOperations";

        public override string AreaName
        {
            get
            {
                return AREA_NAME;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var namespaces = GetControllerNamespace();
            context.Routes.AddManyToManyRoutes("SewerOpening", "SewerOpeningSewerOpeningConnection", namespaces, area: AREA_NAME);
            context.Routes.AddManyToManyRoutes("WorkOrderInvoice", "ScheduleOfValue", namespaces, area: AREA_NAME);
            context.Routes.AddManyToManyRoutes("Service", "WorkOrder", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes(nameof(Service), nameof(ServicePremiseContact), namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes(nameof(Service), nameof(ServiceFlush), namespaces, area: AreaName);
            base.RegisterArea(context);
        }
    }
}