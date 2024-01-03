using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Areas.Production.Controllers
{
    [DisplayName("System Delivery / System Flows Entries")]
    public class SystemDeliveryEntryController : ControllerBaseWithPersistence<ISystemDeliveryEntryRepository, SystemDeliveryEntry, User>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionSystemDeliveryEntry;

        public const RoleModules VALIDATOR_ROLE = RoleModules.ProductionSystemDeliveryApprover;

        #endregion

        #region Constructor

        public SystemDeliveryEntryController(ControllerBaseWithPersistenceArguments<ISystemDeliveryEntryRepository, SystemDeliveryEntry, User> args) : base(args) { }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            switch (action)
            {
                case ControllerAction.Search:
                    this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);
                    break;
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit, extraFilterP: x => x.IsActive, key: "OperatingCenters");
                    break;
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add, extraFilterP: x => x.IsActive, key: "OperatingCenters");
                    break;
            }
        }

        private void SetNotification(SystemDeliveryEntry sde)
        {
            if (sde.IsValidated != null && sde.IsValidated.Value)
            {
                DisplayNotification("This record is now locked, if changes need to be made please enter a adjustment.");
            }
            else
            {
                DisplayNotification("This record has not been validated");
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchSystemDeliveryFacilityEntry search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            return ActionHelper.DoShow(id, onModelFound: sde => { SetNotification(sde); });
        }

        //Index is in SystemDeliveryFacilityEntryController.cs

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(ViewModelFactory.Build<CreateSystemDeliveryEntryViewModel>());
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateSystemDeliveryEntryViewModel model)
        {
            var pwsOrWwsIds = GetPwsidsOrWwsidsFromFacilities(model.Facilities, model.SystemDeliveryType);
            if (model.SystemDeliveryType == SystemDeliveryType.Indices.WATER)
            {
                model.PublicWaterSupplies = pwsOrWwsIds;
            }
            else
            {
                model.WasteWaterSystems = pwsOrWwsIds;
            }

            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => { return RedirectToAction("Edit", new {id = model.Id}); }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            var entity = _repository.Find(id);

            if (entity?.IsValidated == true)
            {
                return RedirectToAction("Show", new {Id = id});
            }

            return ActionHelper.DoEdit<EditSystemDeliveryEntryViewModel>(id);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditSystemDeliveryEntryViewModel model)
        {
            var previousFacilities = model.Original?.Facilities.Select(x => x.Id).ToArray() ?? new[]{ 0 };

            var pwsOrWwsIds = GetPwsidsOrWwsidsFromFacilities(model.Facilities, model.SystemDeliveryType);
            if (model.SystemDeliveryType == SystemDeliveryType.Indices.WATER)
            {
                model.PublicWaterSupplies = pwsOrWwsIds;
            }
            else
            {
                model.WasteWaterSystems = pwsOrWwsIds;
            }

            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnError = () => null,
                OnSuccess = () =>
                    !model.Facilities.OrderBy(x => x).SequenceEqual(previousFacilities.OrderBy(x => x))
                        ? RedirectToAction("Edit", new { id = model.Id })
                        : null
            });
        }

        [HttpPost, RequiresRole(VALIDATOR_ROLE, RoleActions.Add), RequiresSecureForm]
        public ActionResult ValidateAndSubmit(ValidateSystemDeliveryEntryViewModel model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnSuccess = () => {
                    return RedirectToAction("Show", new {id = model.Id});
                }
            });
        }

        #endregion
        
        #region Delete

        [HttpDelete, RequiresAdmin]
        public ActionResult Destroy(int id) => ActionHelper.DoDestroy(id);

        #endregion

        #region Copy

        [HttpGet, RequiresRole(ROLE, RoleActions.Add), Crumb(Action = "New")]
        public ActionResult Copy(int id)
        {
            var entity = Repository.Find(id);
            if (entity == null)
            {
                return DoHttpNotFound($"Could not find system delivery entry with id {id}.");
            }

            var viewModel = ViewModelFactory.Build<CreateSystemDeliveryEntryViewModel>();
            viewModel.OperatingCenters = entity.OperatingCenters.Select(x => x.Id).ToArray();
            viewModel.Facilities = entity.Facilities.Select(x => x.Id).ToArray();
            viewModel.SystemDeliveryType = entity.SystemDeliveryType.Id;
            viewModel.PublicWaterSupplies = entity.PublicWaterSupplies.Select(x => x.Id).ToArray();
            viewModel.WasteWaterSystems = entity.WasteWaterSystems.Select(x => x.Id).ToArray();

            return ActionHelper.DoNew(viewModel);
        }

        #endregion

        #region Child Elements

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddSystemDeliveryEquipmentEntryReversal(AddSystemDeliveryEquipmentEntryReversal model)
        {
            return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                OnError = () => {
                    return RedirectToReferrerOr("Show", "SystemDeliveryEntry", new {area = "Production", model.Id}, "#AdjustmentsTab");
                }
            });
        }

        #endregion

        #region Helper Methods
        //returns list of PWS ids or WWS ids from given facilities depending on the system delivery type
        private int[] GetPwsidsOrWwsidsFromFacilities(int[] facilityIds, int? systemDeliveryType)
        {
            var facilityRepo = _container.GetInstance<IFacilityRepository>();
            var facilities = facilityRepo.FindManyByIds(facilityIds);

            List<int> pwsOrWwsIds = new List<int>();
            foreach (var f in facilities)
            {
                if (systemDeliveryType == SystemDeliveryType.Indices.WATER &&
                    f.Value.PublicWaterSupply != null &&
                    !pwsOrWwsIds.Contains(f.Value.PublicWaterSupply.Id))
                {
                    pwsOrWwsIds.Add(f.Value.PublicWaterSupply.Id);
                }
                else if (systemDeliveryType == SystemDeliveryType.Indices.WASTE_WATER &&
                         f.Value.WasteWaterSystem != null &&
                         !pwsOrWwsIds.Contains(f.Value.WasteWaterSystem.Id))
                {
                    pwsOrWwsIds.Add(f.Value.WasteWaterSystem.Id);
                }
            }

            return pwsOrWwsIds.ToArray();
        }

        #endregion
    }
}