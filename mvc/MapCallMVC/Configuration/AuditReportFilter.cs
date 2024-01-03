using System.Drawing;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Configuration
{
    /// <summary>
    /// This filter's purpose is to audit that a user ran a report.
    /// </summary>
    public class AuditReportFilter : IActionFilter
    {
        private readonly IContainer _container;

        public AuditReportFilter(IContainer container)
        {
            _container = container;
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) { }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // check and set the AuditReportAttribute if we have one. if we don't have it, return 
            if (!(filterContext.ActionDescriptor.GetCustomAttributes(typeof(AuditReportAttribute), true)
                               .FirstOrDefault() is AuditReportAttribute attribute))
            {
                return;
            }

            // Not entirely sure the context of where/when this would be set, but putting it
            // in here to future-proof. A canceled action won't have the result executed, so 
            // the user wouldn't actually view the report in this case.
            if (filterContext.Canceled)
            {
                return;
            }

            var dtProvider = _container.GetInstance<IDateTimeProvider>();
            var curUser = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            var repo = _container.GetInstance<IRepository<ReportViewing>>();

            repo.Save(new ReportViewing {
                User = curUser,
                DateViewed = dtProvider.GetCurrentDate(),
                ReportName = attribute.ReportName
            });
        }
    }
}
