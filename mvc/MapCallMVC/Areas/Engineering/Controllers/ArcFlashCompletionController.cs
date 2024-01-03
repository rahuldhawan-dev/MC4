using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Engineering.Controllers
{
    public class ArcFlashCompletionController : ControllerBaseWithPersistence<IOperatingCenterRepository, OperatingCenter, User>
    {
        #region Constructors

        public ArcFlashCompletionController(ControllerBaseWithPersistenceArguments<IOperatingCenterRepository, OperatingCenter, User> args) : base(args) {}

        #endregion

        #region Exposed Methods

        [HttpGet, RequiresRole(RoleModules.EngineeringArcFlash)]
        public ActionResult Index(SearchArcFlashCompletion search)
        {
            var args = new ActionHelperDoIndexArgs {
                SearchOverrideCallback = () => Repository.GetArcFlashCompletions(search)
            };
            return this.RespondTo((f) => {
                f.View(() => ActionHelper.DoIndex(search, args));
                f.Fragment(() => ActionHelper.DoIndex(search, new ActionHelperDoIndexArgs {
                    IsPartial = true,
                    ViewName = "_Index",
                    OnNoResults = () => PartialView("_NoResults")
                }));
                f.Excel(() => ActionHelper.DoExcel(search, args));
            });
        }

        [HttpGet, RequiresRole(RoleModules.EngineeringArcFlash)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion
    }
}