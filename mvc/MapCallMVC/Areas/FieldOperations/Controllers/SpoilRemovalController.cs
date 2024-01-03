using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class SpoilRemovalController : ControllerBaseWithPersistence<SpoilRemoval, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Exposed Methods
        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Search(SearchSpoilRemoval search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchSpoilRemoval search)
        {
            return this.RespondTo(formatter => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(x => new {
                        x.Id,
                        x.RemovedFrom.OperatingCenter.State,
                        x.RemovedFrom.OperatingCenter,
                        x.RemovedFrom,
                        x.FinalDestination.Name,
                        x.DateRemoved,
                        x.Quantity
                    });
                    return this.Excel(results);
                });
            });
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateSpoilRemoval(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(CreateSpoilRemoval model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditSpoilRemoval>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(EditSpoilRemoval model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public SpoilRemovalController(ControllerBaseWithPersistenceArguments<IRepository<SpoilRemoval>, SpoilRemoval, User> args) : base(args) { }
    }
}
