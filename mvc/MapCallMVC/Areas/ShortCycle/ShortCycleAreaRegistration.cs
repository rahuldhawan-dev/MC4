using System.Web.Mvc;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.ShortCycle
{
    public class ShortCycleAreaRegistration : BaseAreaRegistration
    {
        public const string AREA_NAME = "ShortCycle";
        public override string AreaName => AREA_NAME;

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var namespaces = GetControllerNamespace();
            base.RegisterArea(context);
        }
    }
}
