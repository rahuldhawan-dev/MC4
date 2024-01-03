using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.HealthAndSafety.Controllers
{
    public class IncidentActionItemController : ControllerBaseWithPersistence<ActionItem<Incident>, User>
    {
        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;

        public IncidentActionItemController(ControllerBaseWithPersistenceArguments<IRepository<ActionItem<Incident>>, ActionItem<Incident>, User> args) : base(args) { }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchIncidentActionItem search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchIncidentActionItem search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));

                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(e => new {
                        e.Id,
                        IncidentId = e.LinkedId,
                        e.Entity.OperatingCenter.State,
                        e.Entity.OperatingCenter,
                        e.ActionItem.Type,
                        ResponsibleOwner = e.ActionItem.ResponsibleOwner?.FullName,
                        e.ActionItem.NotListedType,
                        e.ActionItem.TargetedCompletionDate,
                        e.ActionItem.DateCompleted
                    });
                    return this.Excel(results);
                });
            });
        }

        #endregion
    }
}