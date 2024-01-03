using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Controllers
{
    public class OperatingCenterController : ControllerBaseWithPersistence<IOperatingCenterRepository, OperatingCenter, User>
    {
        #region Private Members

        private IRepository<AggregateRole> _roleRepo;

        #endregion

        #region Private Methods

        protected ActionResult ByStateIdForRole(int? stateId, RoleModules role, bool? isContractedOperations = null, bool? isActive = null)
        {
            IQueryable<OperatingCenter> opCenters;
            var matches = AuthenticationService.CurrentUser.GetQueryableMatchingRoles(_roleRepo, role, RoleActions.Read);

            if (stateId == null)
            {
                opCenters = (matches.HasWildCardMatch || AuthenticationService.CurrentUserIsAdmin)
                    ? Repository.GetAll()
                    : Repository.Where(oc => matches.OperatingCenters.Contains(oc.Id));
            }
            else
            {
                opCenters = (matches.HasWildCardMatch || AuthenticationService.CurrentUserIsAdmin)
                    ? Repository.GetByStateId(stateId.Value)
                    : Repository.GetByStateId(stateId.Value).Where(oc => matches.OperatingCenters.Contains(oc.Id));
            }

            if (isContractedOperations.HasValue)
            {
                opCenters = opCenters.Where(x => x.IsContractedOperations == isContractedOperations.Value);
            }

            if (isActive.HasValue)
            {
                opCenters = opCenters.Where(x => x.IsActive == isActive.Value);
            }

            return new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(opCenters)
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            base.SetLookupData(action);

            this.AddDropDownData<State>(x => x.Id, x => x.Abbreviation);

            if (action == ControllerAction.Edit)
            {
                this.AddDropDownData<RecurringFrequencyUnit>("HydrantInspectionFrequencyUnit");
                this.AddDropDownData<RecurringFrequencyUnit>("LargeValveInspectionFrequencyUnit");
                this.AddDropDownData<RecurringFrequencyUnit>("SmallValveInspectionFrequencyUnit");
            }
            else if (action == ControllerAction.Show)
            {
                this.AddDropDownData<AssetType>();
                this.AddDropDownData<WaterSystem>();
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(RoleModules.FieldServicesDataLookups)]
        public ActionResult Index()
        {
            var model = Repository.GetAll().OrderBy(x => x.OperatingCenterCode).ToList();

            return this.RespondTo((f) =>
            {
                f.View(() => ActionHelper.DoIndexWithResults(model));
                f.Excel(() => this.Excel(model.Select(x => new
                {
                    x.Id,
                    x.OperatingCenterCode,
                    x.OperatingCenterName,
                    x.MarkoutsEditable
                })));
            });
        }

        // Leaving out New/Create for the time being.

        [HttpGet, RequiresRole(RoleModules.FieldServicesDataLookups)]
        public ActionResult Show(int id)
        {
            return this.RespondTo(x => {
                x.View(() => ActionHelper.DoShow(id));
                x.Json(() => {
                    var model = Repository.Find(id);
                    if (model == null)
                        return HttpNotFound();
                    return new JsonResult {
                        Data = new { model.MaximumOverflowGallons },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                });
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet]
        [RequiresAdmin]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditOperatingCenter>(id);
        }

        [HttpPost]
        [RequiresAdmin]
        public ActionResult Update(EditOperatingCenter model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnError = () => {
                    var opc = Repository.Find(model.Id);
                    // Needs to be reset since it's not a posted back value.
                    if (opc != null) { model.OperatingCenterCode = opc.OperatingCenterCode; }
                    return null; // defer to default 
                }
            });
        }

        #endregion

        #region Add/Remove asset types

        [HttpPost, RequiresAdmin]
        public ActionResult AddAssetType(AddOperatingCenterAssetType model)
        {
            return ActionHelper.DoUpdate(model);
        }
        
        [HttpPost, RequiresAdmin]
        public ActionResult RemoveAssetType(RemoveOperatingCenterAssetType model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Add/Remove water systems

        [HttpPost, RequiresAdmin]
        public ActionResult AddWaterSystem(AddOperatingCenterWaterSystem model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresAdmin]
        public ActionResult RemoveWaterSystem(RemoveOperatingCenterWaterSystem model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region Ajaxy Methods

        #region ByStateId

        [HttpGet]
        public ActionResult ByStateIds(int[] stateIds)
        {
            return new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(Repository.Where(oc => stateIds.Contains(oc.State.Id))) {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ByStateId(int stateId)
        {
            return new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(Repository.GetByStateId(stateId)) {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ByStateIdOrAll(int? stateId)
        {
            var data = stateId.HasValue ? Repository.GetByStateId(stateId.Value) : Repository.GetAllSorted();
            return new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(data);
        }

        [HttpGet]
        public ActionResult ByStateIdAndContracted(int? stateId, bool? isContractedOperations)
        {
            IQueryable<OperatingCenter> results = null;
            if (stateId.HasValue && isContractedOperations.HasValue)
            {
                results = Repository.GetByStateId(stateId.Value)
                    .Where(x => x.IsContractedOperations == isContractedOperations);
            }
            else if (!stateId.HasValue && !isContractedOperations.HasValue)
            {
                results = Repository.GetAll();
            }
            else
            {
                results = (stateId.HasValue) ? Repository.GetByStateId(stateId.Value) : Repository.Where(x => x.IsContractedOperations == isContractedOperations);
            }
            
            return new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(results)
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult WorkOrdersEnabledByStateId(int stateId)
        {
          //  bool WorkOrdersEnabledP(OperatingCenter oc) => oc.WorkOrdersEnabled;
            IQueryable<OperatingCenter> opCenters;

            if (AuthenticationService.CurrentUserIsAdmin)
            {
                opCenters = Repository.GetByStateId(stateId);
            }
            else
            {
                var matches =
                    AuthenticationService.CurrentUser.GetCachedMatchingRoles(RoleModules.FieldServicesWorkManagement,
                        RoleActions.Read);
                opCenters = matches.HasWildCardMatch
                    ? Repository.GetByStateId(stateId)
                    : Repository.GetByStateId(stateId).Where(oc => matches.OperatingCenters.Contains(oc.Id));
            }

            return new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(opCenters.Where(x => x.WorkOrdersEnabled)) {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ByStateIdForFieldServicesAssets(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.FieldServicesAssets);
        }

        [HttpGet]
        public ActionResult ActiveByStateIdForFieldServicesAssets(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.FieldServicesAssets, isActive: true);
        }

        [HttpGet]
        public ActionResult ByStateIdForEngineeringRiskRegisterAssets(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.EngineeringRiskRegister);
        }

        [HttpGet]
        public ActionResult ActiveByStateIdForEngineeringRiskRegisterAssets(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.EngineeringRiskRegister, isActive: true);
        }

        [HttpGet]
        public ActionResult ByStateIdForFieldServicesWorkManagement(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.FieldServicesWorkManagement);
        }

        [HttpGet]
        public ActionResult ByStateIdForEnvironmentalGeneral(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.EnvironmentalGeneral);
        }

        [HttpGet]
        public ActionResult ByStateIdForEnvironmentalChemicalData(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.EnvironmentalChemicalData);
        }

        [HttpGet]
        public ActionResult ByStateIdForWaterQualityGeneral(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.WaterQualityGeneral);
        }

        [HttpGet]
        public ActionResult ByStateIdForHumanResourcesEmployeeLimited(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.HumanResourcesEmployeeLimited);
        }

        [HttpGet]
        public ActionResult ByStateIdForProductionWorkManagement(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.ProductionWorkManagement);
        }

        [HttpGet]
        public ActionResult ByStateIdForProductionFacilities(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.ProductionFacilities);
        }

        [HttpGet]
        public ActionResult ActiveByStateIdForHealthAndSafety(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.OperationsHealthAndSafety, isActive: true);
        }

        [HttpGet]
        public ActionResult ByStateIdForHealthAndSafety(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.OperationsHealthAndSafety);
        }

        [HttpGet]
        public ActionResult ByStateIdForProductionEquipment(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.ProductionEquipment);
        }

        [HttpGet]
        public ActionResult ByStateIdOrIsContractedOperationsForHumanResourcesCovid(int? stateId, bool? isContractedOperations)
        {
            return ByStateIdForRole(stateId, RoleModules.HumanResourcesCovid, isContractedOperations);
        }

        [HttpGet]
        public ActionResult ByStateIdOrIsContractedOperationsForHealthAndSafetyNearMiss(int? stateId, bool? isContractedOperations)
        {
            return ByStateIdForRole(stateId, RoleModules.OperationsHealthAndSafety, isContractedOperations);
        }

        [HttpGet]
        public ActionResult ByStateIdForHealthAndSafetyLockoutForm(int? stateId)
        {
            return ByStateIdForRole(stateId, RoleModules.OperationsHealthAndSafety);
        }
        
        [HttpGet]
        public ActionResult ActiveByTownId(int townId)
        {
            return new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(Repository.GetByTownId(townId).Where(x => x.IsActive));
        }

        [HttpGet]
        public ActionResult ByTownId(int townId)
        {
            return new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(Repository.GetByTownId(townId)) {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ActiveByStateIdOrAll(int? stateId)
        {
            var data = stateId.HasValue ? Repository.GetByStateId(stateId.Value).Where(x => x.IsActive) : Repository.GetAllSorted().Where(x => x.IsActive);
            return new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(data);
        }

        [HttpGet]
        public ActionResult ActiveByStateIdsOrAll(int[] stateIds)
        {
            var data = stateIds == null || !stateIds.Any()
                ? Repository.GetAllSorted().Where(x => x.IsActive)
                : Repository.Where(x => x.IsActive && stateIds.Contains(x.State.Id));
            return new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(data) {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        #region GetInfoMasterInfo

        [HttpGet]
        public ActionResult GetInfoMasterInfo(int id)
        {
            var operatingCenter = Repository.Find(id) ?? new OperatingCenter();
            var res = new JsonResult() { JsonRequestBehavior = JsonRequestBehavior.AllowGet};
            res.Data = new {
                operatingCenter.InfoMasterMapId,
                operatingCenter.InfoMasterMapLayerName
            };
            return res;
        }

        #endregion

        #region GetStateByOperatingCenterId

        [HttpGet]
        public ActionResult GetStateByOperatingCenterId(int id)
        {
            var opCntr = Repository.Find(id);

            return opCntr == null
                ? (ActionResult)DoHttpNotFound($"Operating Center with id '{id}' could not be found.")
                : Content(opCntr.State.Abbreviation);
        }

        [HttpGet]
        public ActionResult GetStateIdByOperatingCenterId(int id)
        {
            var opCntr = Repository.Find(id);

            return opCntr == null
                ? (ActionResult)DoHttpNotFound($"Operating Center with id '{id}' could not be found.")
                : Content(opCntr.State.Id.ToString());
        }

        #endregion

        #region GetByPublicWaterSupplyForWaterQuality

        [HttpGet]
        public ActionResult GetByPublicWaterSupplyForWaterQuality(int id)
        {
            return GetByPublicWaterSuppliesForWaterQuality(new[] { id });
        }

        [HttpGet]
        public ActionResult GetByPublicWaterSuppliesForWaterQuality(int[] id)
        {
            // I'm putting this here because it returns OperatingCenters rather than in PublicWaterSupplyController,
            // even though it would make sense there too since it deals with the PublicWaterSupplyRepository. This is
            // for bug-3987 cascading nonsense.

            var result = _container.GetInstance<IPublicWaterSupplyRepository>().GetDistinctOperatingCentersByPublicWaterSupplies(id);
            return new CascadingActionResult<OperatingCenter, OperatingCenterDisplayItem>(result);
        }

        #endregion

        #region IsContractedOperations

        [HttpGet]
        public ActionResult IsContractedOperations(int id)
        {
            var opc = Repository.Find(id);
            var result = opc?.IsContractedOperations;
            return Json(new {IsContractedOperations = result}, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region IsSapEnabled

        [HttpGet]
        public ActionResult IsSAPEnabled(int id)
        {
            var opc = Repository.Find(id);
            var result = (opc != null) && (opc.SAPEnabled && !opc.IsContractedOperations);
            
            return Json(new {IsSAPEnabled = result}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult IsSAPWorkOrdersEnabled(int id)
        {
            var opc = Repository.Find(id);
            var result = (opc != null) && (opc.SAPEnabled && opc.SAPWorkOrdersEnabled && !opc.IsContractedOperations);

            return Json(new { IsSAPWorkOrdersEnabled = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        public OperatingCenterController(
            ControllerBaseWithPersistenceArguments<IOperatingCenterRepository, OperatingCenter, User> args) : base(args)
        {
            _roleRepo = _container.GetInstance<IRepository<AggregateRole>>();
        }
    }
}
