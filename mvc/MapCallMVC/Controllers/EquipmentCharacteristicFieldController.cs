using System;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Models.ViewModels;
using MMSINC.Authentication;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class EquipmentCharacteristicFieldController : ControllerBaseWithPersistence<EquipmentCharacteristicField, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionFacilities;

        #endregion

        #region New/Create

        [RequiresRole(ROLE, RoleActions.Edit), HttpPost]
        public ActionResult Create(CreateEquipmentCharacteristicField model)
        {
            Func<ActionResult> onResult = () => RedirectToAction("Show", "EquipmentType", new {id = model.EquipmentType});

            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = onResult,
                OnError = onResult
            });
        }

        #endregion

        #region Delete/Destroy

        [RequiresRole(ROLE, RoleActions.Edit), HttpDelete]
        public ActionResult Destroy(int id)
        {
            var field = Repository.Find(id);

            if (field == null)
            {
                return HttpNotFound();
            }

            var equipmentPurposeId = field.EquipmentType.Id;
            Func<ActionResult> onResult = () => RedirectToAction("Show", "EquipmentType", new {id = equipmentPurposeId});

            if (field.EquipmentCharacteristics != null && field.EquipmentCharacteristics.Count > 0)
            {
                ModelState.AddModelError("id", "Cannot delete a field that has associated characteristcs on equipment.");
            }

            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs
            {
                OnSuccess = onResult,
                OnError = onResult
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEquipmentCharacteristicField>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEquipmentCharacteristicField model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    return RedirectToAction("Show", "EquipmentType", new { Id = entity.EquipmentType.Id });
                },
                OnError = () => RedirectToAction("Edit", "EquipmentCharacteristicField", new { model.Id })
            });
        }

        #endregion
        
        #region Child Elements

        [HttpPost, RequiresAdmin]
        public ActionResult RemoveDropDownValue(RemoveEquipmentCharacteristicFieldDropDownValue model)
        {
            var onResult = new Func<ActionResult>(() => RedirectToAction("Edit", "EquipmentCharacteristicField", new { model.Id }));
            
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnSuccess = onResult,
                OnError = onResult
            });
        }

        #endregion

        #region Constructors

        public EquipmentCharacteristicFieldController(ControllerBaseWithPersistenceArguments<IRepository<EquipmentCharacteristicField>, EquipmentCharacteristicField, User> args) : base(args) {}

        #endregion
    }
}