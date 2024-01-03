using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.WaterQuality.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.WaterQuality.Controllers
{
    public class WaterConstituentStateLimitController : ControllerBaseWithPersistence<WaterConstituentStateLimit, User>
    {
        public const RoleModules ROLE = WaterConstituentController.ROLE;
        public WaterConstituentStateLimitController(ControllerBaseWithPersistenceArguments<IRepository<WaterConstituentStateLimit>, WaterConstituentStateLimit, User> args) : base(args) { }

        [HttpPost, RequiresRole(ROLE)] // Why doesn't this require the Add role action?
        public ActionResult Create(AddWaterConstituentStateLimit model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs
            {
                // TODO: Why is this querying the db to get WaterConstituent when it would need to be on the viewmodel in the first place?
                OnSuccess = () => RedirectToAction("Show", "WaterConstituent", new { id = Repository.Find(model.Id).WaterConstituent.Id })
            });
        }

        [HttpDelete, RequiresRole(ROLE)] // Why doesn't this require the Add role action?
        public ActionResult Destroy(int stateLimitId)
        {
            var constituentId = Repository.Find(stateLimitId)?.WaterConstituent.Id;
            return ActionHelper.DoDestroy(stateLimitId, new MMSINC.Utilities.ActionHelperDoDestroyArgs
            {
                OnSuccess = () => RedirectToAction("Show", "WaterConstituent", new { id = constituentId })
            });
        }
    }
}   