using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data;

namespace MapCallMVC.Controllers
{
    public class EquipmentModelController : ControllerBaseWithPersistence<IEquipmentModelRepository, EquipmentModel, User>
    {
        #region Constants

        public const string NOT_FOUND = "Equipment model with the id '{0}' was not found.";

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.New:
                case ControllerAction.Edit:
                case ControllerAction.Search:
                    this.AddDynamicDropDownData<EquipmentManufacturer, EquipmentManufacturerDisplayItem>(e => e.Id, e => e.Display, "EquipmentManufacturer", e => e.GetAllSorted(x => x.Description));
                    break;
            }
        }

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities)]
        public ActionResult ByManufacturerId(int id)
        {
            return new CascadingActionResult(Repository.GetByEquipmentManufacturerId(id), "Description", "Id") {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities)]
        public ActionResult Search(SearchEquipmentModel search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id);
        }

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities)]
        public ActionResult Index(SearchEquipmentModel search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateEquipmentModel(_container));
        }

        [HttpPost, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Add)]
        public ActionResult Create(CreateEquipmentModel model)
        {
            return ActionHelper.DoCreate(model);
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEquipmentModel>(id);
        }

        [HttpPost, RequiresRole(RoleModules.ProductionFacilities, RoleActions.Edit)]
        public ActionResult Update(EditEquipmentModel model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        public EquipmentModelController(ControllerBaseWithPersistenceArguments<IEquipmentModelRepository, EquipmentModel, User> args) : base(args) {}
    }
}
