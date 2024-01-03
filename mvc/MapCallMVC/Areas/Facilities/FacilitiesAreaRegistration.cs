using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.Facilities
{
    public class FacilitiesAreaRegistration : BaseAreaRegistration
    {
        public override void RegisterArea(System.Web.Mvc.AreaRegistrationContext context)
        {
            context.Routes.AddManyToManyRoutes("FacilityProcess", "FacilityProcessStep", GetControllerNamespace(), area: AreaName);

            base.RegisterArea(context);
        }   
    }
}
