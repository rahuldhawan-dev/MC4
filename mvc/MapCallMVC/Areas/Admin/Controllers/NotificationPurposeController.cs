using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using System.Linq;
using System.Web.Mvc;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.Admin.Controllers
{
    public class NotificationPurposeController
        : ControllerBaseWithPersistence<IRepository<NotificationPurpose>, NotificationPurpose, User>
    {
        [HttpGet]
        public ActionResult ByApplication(int applicationId)
        {
            return new CascadingActionResult<NotificationPurpose, NotificationPurposeDisplayItem>(
                Repository.Where(x => x.Module.Application.Id == applicationId)
                          .OrderBy(x => x.Purpose));
        }

        [HttpGet]
        public ActionResult ByModule(int[] moduleIds)
        {
            return new CascadingActionResult<NotificationPurpose, NotificationPurposeDisplayItem>(
                Repository.Where(x => moduleIds.Contains(x.Module.Id))
                          .OrderBy(x => x.Purpose));
        }

        public NotificationPurposeController(
            ControllerBaseWithPersistenceArguments<IRepository<NotificationPurpose>, NotificationPurpose, User> args) :
            base(args) { }
    }
}
