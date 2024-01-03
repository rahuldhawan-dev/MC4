using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class SpoilFinalProcessingLocationController : ControllerBaseWithPersistence<IRepository<SpoilFinalProcessingLocation>, SpoilFinalProcessingLocation, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Exposed Methods

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Search(SearchSpoilFinalProcessingLocation search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchSpoilFinalProcessingLocation search)
        {
            return ActionHelper.DoIndex(search);
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
            return ActionHelper.DoNew(new CreateSpoilFinalProcessingLocation(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(CreateSpoilFinalProcessingLocation model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditSpoilFinalProcessingLocation>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(EditSpoilFinalProcessingLocation model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult GetByOperatingCenter(int id)
        {
            return new CascadingActionResult(Repository.Where(x => x.OperatingCenter.Id == id), "Name", "Id");
        }

        #endregion

        public SpoilFinalProcessingLocationController(ControllerBaseWithPersistenceArguments<IRepository<SpoilFinalProcessingLocation>, SpoilFinalProcessingLocation, User> args) : base(args) { }
    }
}