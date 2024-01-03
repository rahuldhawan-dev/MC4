using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;

namespace MapCallMVC.Controllers
{
    public class EquipmentManufacturerController : ControllerBaseWithPersistence<IEquipmentManufacturerRepository, EquipmentManufacturer, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionWorkManagement;

        #endregion

        #region Private Methods

        private IEnumerable<EquipmentManufacturer> GetCascadeResultsForbyEquipmentTypeId(IEnumerable<int> equipmentTypeIds)
        {
            return Repository.Where(x => equipmentTypeIds.Contains(x.EquipmentType.Id));
        }

        #endregion

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            if (action == ControllerAction.Search)
            {
                this.AddDynamicDropDownData<EquipmentType, EquipmentTypeDisplayItem>(t => t.Id, t => t.Display);
            }
        }

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search(SearchEquipmentManufacturer search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchEquipmentManufacturer search)
        {
            return this.RespondTo((formatter) =>
            {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Excel(() => ActionHelper.DoExcel(search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateEquipmentManufacturer(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateEquipmentManufacturer model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => RedirectToAction("Search", "EquipmentManufacturer")
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEquipmentManufacturer>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEquipmentManufacturer model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs
            {
                OnSuccess = () => RedirectToAction("Search", "EquipmentManufacturer")
            });
        }

        #endregion

        #region ByEquipmentTypeId

        [HttpGet]
        public ActionResult ByEquipmentTypeId(int[] equipmentTypeId)
        {
            var cascadeResults = GetCascadeResultsForbyEquipmentTypeId(equipmentTypeId);
            return new CascadingActionResult(cascadeResults, "Display", "Id");
        }

        #endregion

        public EquipmentManufacturerController(ControllerBaseWithPersistenceArguments<IEquipmentManufacturerRepository, EquipmentManufacturer, User> args) : base(args) { }
    }
}