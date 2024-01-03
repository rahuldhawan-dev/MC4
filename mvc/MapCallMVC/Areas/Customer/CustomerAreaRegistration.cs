using System.Web.Mvc;
using MMSINC.ClassExtensions;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.Customer
{
    public class CustomerAreaRegistration : BaseAreaRegistration
    {
        public const string AREA_NAME = "Customer";

        public override string AreaName { get { return AREA_NAME; } }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            var namespaces = GetControllerNamespace();
            
            base.RegisterArea(context);
        }
    }
}