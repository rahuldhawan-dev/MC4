using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Areas.Reports.Controllers
{
    public class MarkoutDamageBPUController : ControllerBaseWithPersistence<IMarkoutDamageRepository, MarkoutDamage, User>
    {
        #region Constants

        public const RoleModules ROLE = MarkoutDamageController.ROLE_MODULE;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                this.AddDropDownData<MarkoutDamageToType>();
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchMarkoutDamageReport search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchMarkoutDamageReport search)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search));
                f.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository
                        .Search(search)
                        .Select(x => new {
                            x.DateAndRequestNumber,
                            x.TownAndLocation,
                            x.DamageComments,
                            x.ExcavatorDescription,
                            Mark = x.IsMarkedOut,
                            Mismark = x.IsMismarked,
                            Discover = x.ExcavatorDiscoveredDamage,
                            Cause = x.ExcavatorCausedDamage,
                            Picture = x.WerePicturesTaken
                        });
                    return this.Excel(results);
                });
            });
        }

        #endregion

        public MarkoutDamageBPUController(ControllerBaseWithPersistenceArguments<IMarkoutDamageRepository, MarkoutDamage, User> args) : base(args) {}
    }
}