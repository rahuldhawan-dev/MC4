using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.Production
{
    public class ProductionAreaRegistration : BaseAreaRegistration
    {
        public const string AREA_NAME = "Production";
        public override string AreaName => AREA_NAME;

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var namespaces = GetControllerNamespace();
            context.Routes.AddManyToManyRoutes("ProductionWorkOrder", "EmployeeAssignment", namespaces, area: AREA_NAME);
            context.Routes.AddManyToManyRoutes("ProductionWorkOrder", "ProductionWorkOrderMaterialUsed", namespaces, area: AREA_NAME);
            context.Routes.AddManyToManyRoutes("ProductionWorkOrder", "ProductionWorkOrderProductionPrerequisite", namespaces, area: AREA_NAME);
            context.Routes.AddManyToManyRoutes("EmployeeAssignment", "Employee", namespaces, area: AREA_NAME);
            context.Routes.AddManyToManyRoutes("MaintenancePlan", "EquipmentMaintenancePlan", namespaces, area: AREA_NAME);
            context.Routes.AddManyToManyRoutes("SystemDeliveryEntry", "SystemDeliveryEquipmentEntryReversal", namespaces, area: AREA_NAME);
            context.Routes.AddManyToManyRoutes("NonRevenueWaterEntry", "NonRevenueWaterAdjustment", namespaces, area: AREA_NAME);
            base.RegisterArea(context);
        }
    }
}
