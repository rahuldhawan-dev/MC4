using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.WaterQuality
{
    public class WaterQualityAreaRegistration : BaseAreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            var namespaces = GetControllerNamespace();
            context.Routes.AddManyToManyRoutes("WaterQualityComplaint", "WaterQualityComplaintSampleResult",
                namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("BacterialWaterSampleAnalyst", "OperatingCenter", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("SampleSite", "SamplePlan", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("SampleSite", "BracketSite", namespaces, area: AreaName);
            context.Routes.AddManyToManyRoutes("WaterConstituent", "WaterConstituentStateLimit", namespaces,
                area: AreaName);
            base.RegisterArea(context);
        }
    }
}