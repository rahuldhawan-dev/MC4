using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Controllers;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Repositories;

namespace MapCallMVC.Areas.Admin.Controllers
{
    public class ModuleController : ControllerBaseWithPersistence<IModuleRepository, Module, User>
    {
        [HttpGet]
        public ActionResult ByApplication(int[] applicationIds)
        {
            return new CascadingActionResult<Module, ModuleDisplayItem>(
                Repository.Where(x => applicationIds.Contains(x.Application.Id))
                          .OrderBy(x => x.Name));
        }

        public ModuleController(ControllerBaseWithPersistenceArguments<IModuleRepository, Module, User> args) :
            base(args) { }
    }
}
