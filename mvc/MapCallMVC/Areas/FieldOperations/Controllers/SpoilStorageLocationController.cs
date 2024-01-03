using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Controllers
{
    public class SpoilStorageLocationController : ControllerBaseWithPersistence<IRepository<SpoilStorageLocation>, SpoilStorageLocation, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.FieldServicesWorkManagement;

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            if (action == ControllerAction.Search)
            {
                this.AddDropDownData<IStateRepository, State>(r => r.GetAll());
            }

            base.SetLookupData(action);
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Search(SearchSpoilStorageLocation search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Index(SearchSpoilStorageLocation search)
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
            return ActionHelper.DoNew(new CreateSpoilStorageLocation(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Create(CreateSpoilStorageLocation model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditSpoilStorageLocation>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.UserAdministrator)]
        public ActionResult Update(EditSpoilStorageLocation model)
        {
            return ActionHelper.DoUpdate(model);
        }
        #endregion

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int? opcId)
        {
            var data = opcId.HasValue ? 
                Repository.Where(x => x.OperatingCenter.Id == opcId.Value && x.Active).ToList() :
                Repository.GetAll().ToList();
            return new CascadingActionResult(data, "Name", "Id");
        }

        #endregion

        public SpoilStorageLocationController(ControllerBaseWithPersistenceArguments<IRepository<SpoilStorageLocation>, SpoilStorageLocation, User> args) : base(args) {}
    }
}