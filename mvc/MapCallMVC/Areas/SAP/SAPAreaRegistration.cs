using System.Web.Mvc;
using MMSINC.Configuration;

namespace MapCallMVC.Areas.SAP
{
    public class SAPAreaRegistration : BaseAreaRegistration
    {
        public const string AREA_NAME = "SAP";
        public override string AreaName { get { return AREA_NAME; } }
        public override void RegisterArea(AreaRegistrationContext context)
        {
            var namespaces = GetControllerNamespace();
            base.RegisterArea(context);
        }
    }
}