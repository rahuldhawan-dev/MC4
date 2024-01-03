using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.Operations
{
    public class OperationsAreaRegistration : BaseAreaRegistration
    {
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.AddManyToManyRoutes("AtRiskBehaviorSection", "AtRiskBehaviorSubSection", GetControllerNamespace(), area: AreaName);
            base.RegisterArea(context);
        }
    }
}