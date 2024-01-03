using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents;
using MapCallMVC.Controllers;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Environmental.Controllers
{
    [ActionHelperViewVirtualPathFormat("~/Areas/Environmental/Views/EnvironmentalNonComplianceEventSubType/{0}.cshtml")]
    public class EnvironmentalNonComplianceEventSubTypeController : EntityLookupControllerBase<IRepository<EnvironmentalNonComplianceEventSubType>, EnvironmentalNonComplianceEventSubType, EnvironmentalNonComplianceEventSubTypeViewModel>
    {
        [HttpGet]
        public ActionResult ByTypeId(int typeId)
        {
            var data = Repository.Where(x => x.EnvironmentalNonComplianceEventType.Id == typeId).Select(x => new {x.Id, x.Description});
            return new CascadingActionResult(data, "Description", "Id");
        }

        public EnvironmentalNonComplianceEventSubTypeController(ControllerBaseWithPersistenceArguments<IRepository<EnvironmentalNonComplianceEventSubType>, EnvironmentalNonComplianceEventSubType, User> args) : base(args) { }
    }
}