﻿using System;
﻿using System.Collections.Generic;
﻿using System.Linq;
 using System.Web.Mvc;
 using System.Web.UI;
 using MapCall.Common.ClassExtensions;
﻿using MapCall.Common.Configuration;
﻿using MapCall.Common.Metadata;
﻿using MapCall.Common.Model.Entities;
﻿using MapCall.Common.Model.Entities.Users;
﻿using MapCall.Common.Model.Repositories;
﻿using MapCallMVC.Areas.Facilities.Models.ViewModels;
﻿using MapCallMVC.ClassExtensions;
﻿using MapCallMVC.Models.ViewModels;
﻿using MMSINC;
﻿using MMSINC.ClassExtensions;
 using MMSINC.ClassExtensions.DateTimeExtensions;
 using MMSINC.Controllers;
﻿using MMSINC.Data.NHibernate;
﻿using MMSINC.Helpers;
﻿using MMSINC.Metadata;
using MMSINC.Utilities;
using Facility = MapCall.Common.Model.Entities.Facility;

 namespace MapCallMVC.Controllers
{
    public class FacilityController : ControllerBaseWithPersistence<IFacilityRepository, Facility, User>
    {
        #region Consts

        public const string 
            ARC_FLASH_STUDY_REQUIRED = "An arc flash study is required for this facility but one does not exist.",
            ARC_FLASH_STUDIES_EXPIRED= "An arc flash study is expired and there are no current ones",
            ARC_FLASH_STUDY_EXPIRING = "The arc flash study for this facility is expiring within the next year.", 
            SYSTEM_DELIVERY_TAB = "#SystemDeliveryTab";

        public const RoleModules ROLE = RoleModules.ProductionFacilities,
                                 FACILITY_AREA_MANAGEMENT_ROLE = RoleModules.ProductionFacilityAreaManagement,
                                 ASSET_RELIABILITY_ROLE = RoleModules.ProductionAssetReliability,
                                 J100_ROLE = RoleModules.EngineeringJ100AssessmentData;

        #endregion

        #region Constructors

        public FacilityController(ControllerBaseWithPersistenceArguments<IFacilityRepository, Facility, User> args) : base(args) {}

        #endregion

        #region Private Methods

        private void DisplayShowNotifications(Facility facility)
        {
            if (facility.ArcFlashStudyRequired != null && facility.ArcFlashStudyRequired.Value)
            {
                // If there aren't any studies and it's required, then show it's required
                var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                if (facility.MostRecentArcFlashStudies == null || !facility.MostRecentArcFlashStudies.Any())
                {
                    DisplayErrorMessage(ARC_FLASH_STUDY_REQUIRED);
                }
                // If there are studies and no studies valid studies i.e. DateLabelsApplied + 5 >= getDate
                else if (!facility.MostRecentArcFlashStudies.Any(s => s.DateLabelsApplied.Value.AddYears(5) > now))
                {
                    DisplayErrorMessage(ARC_FLASH_STUDIES_EXPIRED);
                }
                // if there are any expiring within the year
                else if (facility.MostRecentArcFlashStudies.Any( x => x.ExpiringWithinAYear))
                {
                    DisplayErrorMessage(ARC_FLASH_STUDY_EXPIRING);
                }
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Show:
                    this.AddEnumDropDownData<ReadingGroupType>("Interval");
                    this.AddDropDownData<ProcessStage>(); // Needed for facility process tab
                    this.AddDropDownData<FacilityArea>();
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, extraFilterP: x => x.IsActive); // Need this for system delivery tab
                    break;
                case ControllerAction.Edit:
                    this.AddDropDownData<FacilityOwner>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<FacilityStatus>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<Department>(d => d.GetAllSorted(x => x.Description), d => d.Id, d => d.Description);
                    this.AddDropDownData<FEMAFloodRating>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    //   this.AddDropDownData<ITownRepository, Town>(t => t.GetAllSorted(), t => t.Id, t => t.ShortName);
                    this.AddDropDownData<IFunctionalLocationRepository, FunctionalLocation>(f => f.GetByAssetTypeId(AssetType.Indices.FACILITY), f => f.Id, f => f.Description);
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    this.AddDropDownData<ArcFlashStatus>();
                    this.AddDropDownData<Voltage>();
                    this.AddDropDownData<PowerPhase>();
                    break;
                case ControllerAction.New:
                    this.AddDropDownData<FacilityOwner>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<FacilityStatus>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<Department>(d => d.GetAllSorted(x => x.Description), d => d.Id, d => d.Description);
                    this.AddDropDownData<FEMAFloodRating>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    //   this.AddDropDownData<ITownRepository, Town>(t => t.GetAllSorted(), t => t.Id, t => t.ShortName);
                    this.AddDropDownData<IFunctionalLocationRepository, FunctionalLocation>(f => f.GetByAssetTypeId(AssetType.Indices.FACILITY), f => f.Id, f => f.Description);
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add,extraFilterP: x => x.IsActive);
                    this.AddDropDownData<CompanySubsidiary>(d => d.GetAllSorted(x => x.Description), d => d.Id, d => d.Description);
                    break;
                case ControllerAction.Search:
                    this.AddDropDownData<IRepository<State>, State>("TownState", s => s.Id, s => s.Name);
                    this.AddDropDownData<FacilityOwner>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<FacilityStatus>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<Department>(d => d.GetAllSorted(x => x.Description), d => d.Id, d => d.Description);
                    this.AddDropDownData<FEMAFloodRating>(f => f.Id, f => f.Description);
                    this.AddDropDownData<IFunctionalLocationRepository, FunctionalLocation>(f => f.GetByAssetTypeId(AssetType.Indices.FACILITY), f => f.Id, f => f.Description);
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Search()
        {
            return ActionHelper.DoSearch(new SearchFacility());
        }

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult Index(SearchFacility search)
        {
            return this.RespondTo(f => {
                f.View(() => ActionHelper.DoIndex(search));
                f.Excel(() => ActionHelper.DoExcel(search));
                f.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        [HttpGet, RequiresRole(ROLE)] 
        public ActionResult Show(int id)
        {
            var notFoundError = string.Format("Facility with id '{0}' was not found.", id);
            return this.RespondTo((f) => {
                f.View(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    NotFound = notFoundError,
                    GetEntityOverride = () => Repository.FindWithEagerJoin(id)
                }, DisplayShowNotifications));
                f.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs {
                    NotFound = notFoundError,
                    ViewName = "_ShowPopup",
                    IsPartial = true
                }));
                f.Json(() => {
                    var entity = Repository.Find(id);
                    if (entity == null)
                        DoHttpNotFound(notFoundError);
                    return Json(
                        new {
                            entity.Address, 
                            entity.Coordinate?.Latitude, 
                            entity.Coordinate?.Longitude,
                            CoordinateId = entity.Coordinate?.Id, 
                            DepartmentId = entity.Department?.Id,
                            entity.HasConfinedSpaceRequirement,
                            Town = entity.Town?.Id,
                            TownSection = entity.TownSection?.Id,
                            County = entity.Town?.County?.Id,
                            Street = entity.Street?.Id,
                            entity.StreetNumber,
                            entity.ZipCode
                        }, JsonRequestBehavior.AllowGet);
                });
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New()
        {
            return ActionHelper.DoNew(new CreateFacility(_container));
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateFacility model)
        {
            return ActionHelper.DoCreate(model, new ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    var entity = Repository.Find(model.Id);
                    if(entity.ArcFlashStudyRequired.HasValue && entity.ArcFlashStudyRequired.Value)
                    { 
                        return DoRedirectionToAction("New", "ArcFlashStudy",
                            new {
                                area = "Engineering", 
                                Facility = entity.Id, 
                                State = entity.OperatingCenter.State.Id,
                                OperatingCenter = entity.OperatingCenter.Id
                            });
                    }

                    return DoRedirectionToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit(id, new MMSINC.Utilities.ActionHelperDoEditArgs<Facility, EditFacility>{
                NotFound = "Facility not found."
            });
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditFacility facility)
        {
            return ActionHelper.DoUpdate(facility, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                NotFound = "Facility not found"
            });
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete, RequiresRole(ROLE, RoleActions.Delete)]
        public ActionResult Destroy(int id)
        {
            // TODO: Why does this redirect to Index rather than Search?
            return ActionHelper.DoDestroy(id, new MMSINC.Utilities.ActionHelperDoDestroyArgs {
                NotFound = $"Facility with id {id} was not found",
                OnSuccessRedirectAction = "Index"
            });
        }

        #endregion

        #region ByTownId

        [HttpGet]
        public ActionResult ByTownId(int? townId)
        {
            return new CascadingActionResult<Facility, FacilityDisplayItem>(Repository.GetByTownId(townId));
        }

        #endregion

        #region ByOperatingCenterId

        [HttpGet]
        public ActionResult ByOperatingCenterId(int operatingCenterId)
        {
            return new CascadingActionResult<Facility, FacilityDisplayItem>(Repository.GetByOperatingCenterId(operatingCenterId).OrderBy(x => x.OperatingCenter.OperatingCenterCode).ThenBy(x => x.Id), "Display");
        }

        #endregion

        #region ByFunctionalLocationId

        /// <summary>
        /// Note is comparing a string value functional location, not an entity
        /// E.g. NJRB-BM-RMTP.
        /// </summary>
        /// <param name="functionalLocation"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ByFunctionalLocation(string functionalLocation)
        {
            var parentFunctionalLocation = functionalLocation.LastIndexOf("-") > 0 ? functionalLocation.Substring(0, functionalLocation.LastIndexOf("-")) : string.Empty;
            return new CascadingActionResult<Facility,FacilityDisplayItem>(Repository.Where(x => x.FunctionalLocation == parentFunctionalLocation).OrderBy(x => x.OperatingCenter.OperatingCenterCode).ThenBy(x => x.Id), "Display", "Id");
        }

        #endregion

        #region GetByOperatingCenterId

        [HttpGet]
        public ActionResult GetByOperatingCenterId(int? opCenterId)
        {
            return new CascadingActionResult<Facility, FacilityDisplayItem>(Repository.GetByOperatingCenterId(opCenterId).OrderBy(x => x.OperatingCenter.OperatingCenterCode).ThenBy(x => x.Id), "Display");
        }

        [HttpGet]
        public ActionResult GetByOperatingCenterIdAndCommunityRightToKnowIsTrue(int? opCenterId)
        {
            return new CascadingActionResult<Facility, FacilityDisplayItem>(Repository.GetByOperatingCenterIdAndCommunityRightToKnowIsTrue(opCenterId).OrderBy(x => x.OperatingCenter.OperatingCenterCode).ThenBy(x => x.Id), "Display");
        }

        #endregion

        #region GetActiveByOperatingCenterId

        [HttpGet]
        public ActionResult GetActiveByOperatingCenterId(int? opCenterId)
        { 
            return new CascadingActionResult<Facility, FacilityDisplayItem>(Repository.GetByOperatingCenterId(opCenterId).Where(x => x.FacilityStatus.Id == FacilityStatus.Indices.ACTIVE).OrderBy(x => x.OperatingCenter.OperatingCenterCode).ThenBy(x => x.Id), "Display");
        }

        #endregion

        #region GetActiveByOperatingCentersWithPointOfEntryAndSystemDeliveryType

        [HttpGet]
        public ActionResult GetActiveByOperatingCentersWithPointOfEntryAndSystemDeliveryType(int[] opCenters, int systemDeliveryType)
        {
            CascadingActionResult<Facility, FacilityDisplayItem> facilities =
                new CascadingActionResult<Facility, FacilityDisplayItem>(
                    Repository.Where(x =>
                                   x.FacilityStatus.Id == FacilityStatus.Indices.ACTIVE && x.PointOfEntry &&
                                   opCenters.Contains(x.OperatingCenter.Id) &&
                                   x.SystemDeliveryType.Id == systemDeliveryType &&
                                   x.FacilitySystemDeliveryEntryTypes.Any(y =>
                                       y.IsEnabled && y.SystemDeliveryEntryType.Id !=
                                       SystemDeliveryEntryType.Indices.TRANSFERRED_FROM))
                              .OrderBy(x => x.FacilityName),
                    "FacilityIdWithFacilityName") {
                    //Need to set this to false as order by is different from the display text.
                    SortItemsByTextField = false
                };
            return facilities;
        }

        #endregion

        #region GetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply

        [HttpGet]
        public ActionResult GetActiveByOperatingCentersWithPointOfEntrySystemDeliveryTypeAndPublicWaterSupply(int[] opCenters, int? systemDeliveryType, int[] publicWaterSupplies)
        {
            var query = Repository.Where(x =>
                x.FacilityStatus.Id == FacilityStatus.Indices.ACTIVE && x.PointOfEntry &&
                opCenters.Contains(x.OperatingCenter.Id) &&
                x.SystemDeliveryType.Id == systemDeliveryType &&
                x.FacilitySystemDeliveryEntryTypes.Any(y =>
                    y.IsEnabled && y.SystemDeliveryEntryType.Id !=
                    SystemDeliveryEntryType.Indices.TRANSFERRED_FROM));
                
            if (publicWaterSupplies != null)
            {
                query = query.Where(x => publicWaterSupplies.Contains(x.PublicWaterSupply.Id));
            }
            
            query = query.OrderBy(x => x.FacilityName);

            CascadingActionResult<Facility, FacilityDisplayItem> facilities = 
                new CascadingActionResult<Facility, FacilityDisplayItem>(query, "FacilityIdWithFacilityName") {
                    //Need to set this to false as order by is different from the display text.
                    SortItemsByTextField = false
                };
            return facilities;
        }

        #endregion

        #region GetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId

        [HttpGet]
        public ActionResult GetActiveByOperatingCentersWithSystemDeliveryTypeAndPublicWaterSupplyIdAndWasteWaterSystemId(int[] opCenters, int? systemDeliveryType, int[] publicWaterSupplyIds, int[] wasteWaterSystemIds)
        {
            var query = Repository.Where(x => true);
            if (opCenters == null || opCenters.Length == 0 || systemDeliveryType == null)
            {
                query = Enumerable.Empty<Facility>().AsQueryable();
            }
            else
            {
                query = Repository.Where(x =>
                                       x.FacilityStatus.Id == FacilityStatus.Indices.ACTIVE && x.PointOfEntry &&
                                       opCenters.Contains(x.OperatingCenter.Id) &&
                                       x.SystemDeliveryType.Id == systemDeliveryType &&
                                       x.FacilitySystemDeliveryEntryTypes.Any(y =>
                                           y.IsEnabled && y.SystemDeliveryEntryType.Id !=
                                           SystemDeliveryEntryType.Indices.TRANSFERRED_FROM))
                                  .OrderBy(x => x.FacilityName);

                if (systemDeliveryType == SystemDeliveryType.Indices.WATER && publicWaterSupplyIds != null && publicWaterSupplyIds.Length > 0)
                {
                    //Filter results based on the publicWaterSupplyId 
                    query = query.Where(x => publicWaterSupplyIds.Contains(x.PublicWaterSupply.Id)); 
                }
                else if (systemDeliveryType == SystemDeliveryType.Indices.WASTE_WATER && wasteWaterSystemIds != null && wasteWaterSystemIds.Length > 0)
                {
                    //Filter results based on the wasteWaterSystemId
                    query = query.Where(x => wasteWaterSystemIds.Contains(x.WasteWaterSystem.Id));
                }
            }

            CascadingActionResult<Facility, FacilityDisplayItem> facilities =
                new CascadingActionResult<Facility, FacilityDisplayItem>(query, "FacilityIdWithFacilityName") {
                    //Need to set this to false as order by is different from the display text.
                    SortItemsByTextField = false
                };
            return facilities;
        }

        #endregion

        #region GetByActiveOperatingCenterWithPointOfEntry

        [HttpGet]
        public ActionResult GetActiveByOperatingCenterWithPointOfEntry(int opCenter)
        { 
            return new CascadingActionResult<Facility, FacilityDisplayItem>(Repository
                                                                           .Where(x => x.FacilityStatus.Id == FacilityStatus.Indices.ACTIVE && x.PointOfEntry
                                                                                && x.OperatingCenter.Id == opCenter)
                                                                           .OrderBy(x => x.OperatingCenter.OperatingCenterCode)
                                                                           .ThenBy(x => x.Id), "Display");
        }

        #endregion

        #region ActiveByOperatingCenterOrPlanningPlant
        
        [HttpGet]
        public ActionResult ActiveByOperatingCenterOrPlanningPlant(int operatingCenterId, int? planningPlantId)
        {
            var query = Repository.GetByOperatingCenterId(operatingCenterId);
            
            if (planningPlantId.HasValue)
            {
                query = query.Where(x => x.PlanningPlant != null && x.PlanningPlant.Id == planningPlantId);
            }

            query = query.Where(x => x.FacilityStatus.Id == FacilityStatus.Indices.ACTIVE).OrderBy(x => x.OperatingCenter).ThenBy(x => x.Id);
            return new CascadingActionResult<Facility, FacilityDisplayItem>(query, "Display", "Id");
        }

        [HttpGet]
        public ActionResult ActiveByOperatingCentersOrPlanningPlant(int[] operatingCenterIds, int? planningPlantId)
        {
            var query = Repository.GetByOperatingCenterIds(operatingCenterIds);
            
            if (planningPlantId.HasValue)
            {
                query = query.Where(x => x.PlanningPlant != null && x.PlanningPlant.Id == planningPlantId);
            }

            query = query.Where(x => x.FacilityStatus.Id == FacilityStatus.Indices.ACTIVE).OrderBy(x => x.OperatingCenter).ThenBy(x => x.Id);
            return new CascadingActionResult<Facility, FacilityDisplayItem>(query, "Display", "Id");
        }

        #endregion

        #region ActiveAndPendingRetirementByOperatingCenterOrPlanningPlant

        [HttpGet]
        public ActionResult ActiveAndPendingRetirementByOperatingCenterOrPlanningPlant(int operatingCenterId, int? planningPlantId)
        {
            var query = Repository.GetByOperatingCenterId(operatingCenterId);

            if (planningPlantId.HasValue)
            {
                query = query.Where(x => x.PlanningPlant != null && x.PlanningPlant.Id == planningPlantId);
            }

            query = query.Where(x => x.FacilityStatus.Id == FacilityStatus.Indices.ACTIVE || x.FacilityStatus.Id == FacilityStatus.Indices.PENDING_RETIREMENT).OrderBy(x => x.OperatingCenter).ThenBy(x => x.Id);
            return new CascadingActionResult<Facility, FacilityDisplayItem>(query, "Display", "Id");
        }
        
        #endregion

        #region ByOperatingCenterIds

        [HttpGet]
        public ActionResult ByOperatingCenterIds(int[] operatingCenterIds)
        {
            var results = Repository.GetByOperatingCenterIds(operatingCenterIds).OrderBy(x=>x.OperatingCenter.OperatingCenterCode).ThenBy(x => x.Id).AsQueryable();
            return new CascadingActionResult<Facility, FacilityDisplayItem>(results, "Display", "Id");
        }

        #endregion

        #region ByPublicWaterSupplyId

        [HttpGet]
        public ActionResult GetByPublicWaterSupplyId(int publicWaterSupplyId)
        {
            var results = Repository.GetByPublicWaterSupplyId(publicWaterSupplyId).OrderBy(x => x.FacilityName).ThenBy(x => x.Id).AsQueryable();
            return new CascadingActionResult<Facility, FacilityDisplayItem>(results, "Display", "Id");
        }

        #endregion

        #region ByOperatingCenterOrPlanningPlant

        [HttpGet]
        public ActionResult ByOperatingCenterOrPlanningPlant(int operatingCenterId, int? planningPlantId)
        {
            var query = Repository.GetByOperatingCenterId(operatingCenterId);

            if (planningPlantId.HasValue)
            {
                query = query.Where(x => x.PlanningPlant != null && x.PlanningPlant.Id == planningPlantId);
            }

            query = query
                   .OrderBy(x => x.OperatingCenter).ThenBy(x => x.Id);

            return new CascadingActionResult<Facility, FacilityDisplayItem>(query, "Display", "Id");
        }

        #endregion

        #region ByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId

        public ActionResult ByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId(int operatingCenterId, int? planningPlantId)
        {
            var query = Repository.GetByOperatingCenterId(operatingCenterId);

            if (planningPlantId.HasValue)
            {
                query = query.Where(x => x.PlanningPlant != null && x.PlanningPlant.Id == planningPlantId);
            }

            query = query
                   .OrderBy(x => x.OperatingCenter).ThenBy(x => x.Id);

            return new CascadingActionResult<Facility, FacilityDisplayItem>(query, "Display", "Id");
        }

        #endregion

        #region UnarchivedByOperatingCenterOrPlanningPlant

        [HttpGet]
        public ActionResult UnarchivedByOperatingCenterOrPlanningPlant(int operatingCenterId, int? planningPlantId)
        {
            var query = Repository.GetByOperatingCenterId(operatingCenterId);

            if (planningPlantId.HasValue)
            {
                query = query.Where(x => x.PlanningPlant != null && x.PlanningPlant.Id == planningPlantId);
            }

            query = query
                   .Where(x => x.FacilityStatus.Id != FacilityStatus.Indices.INACTIVE &&
                               !x.FacilityName.ToLower().Contains("archive"))
                   .OrderBy(x => x.OperatingCenter).ThenBy(x => x.Id);

            return new CascadingActionResult<Facility, FacilityDisplayItem>(query, "Display", "Id");
        }

        #endregion

        #region UnarchivedByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId

        [HttpGet]
        public ActionResult UnarchivedByOperatingCenterAndSometimesPlanningPlantDisplayNameAndId(int operatingCenterId, int? planningPlantId)
        {
            var query = Repository.GetByOperatingCenterId(operatingCenterId);

            if (planningPlantId.HasValue)
            {
                query = query.Where(x => x.PlanningPlant != null && x.PlanningPlant.Id == planningPlantId);
            }

            query = query
                   .Where(x => x.FacilityStatus.Id != FacilityStatus.Indices.INACTIVE &&
                               !x.FacilityName.ToLower().Contains("archive"))
                   .OrderBy(x => x.OperatingCenter).ThenBy(x => x.Id);

            return new CascadingActionResult<Facility, FacilityDisplayItem>(query, "Display", "Id");
        }

        #endregion

        #region ByEquipment
        [HttpGet]
        public ActionResult ByEquipment(int equipmentId)
        {
            return new CascadingActionResult<Facility, FacilityDisplayItem>(Repository.Where(f => f.Equipment.Any(e => e.Id == equipmentId)), "Display", "Id");
        }

        #endregion

        #region ByPlanningPlant

        [HttpGet]
        public ActionResult ByPlanningPlant(int planningPlantId)
        {
            return new CascadingActionResult<Facility, FacilityDisplayItem>(Repository.Where(f => f.PlanningPlant.Id == planningPlantId), "Display", "Id");
        }

        #endregion

        #region ByPlanningPlants

        [HttpGet]
        public ActionResult ByPlanningPlants(int[] planningPlants)
        {
            return new CascadingActionResult<Facility, FacilityDisplayItem>(Repository.Where(f => planningPlants.Contains(f.PlanningPlant.Id)), "Display", "Id");
        }

        #endregion

        #region Readings

        [NoCache, HttpGet, RequiresRole(ROLE)]
        public ActionResult Readings(SearchFacilityReadings model)
        {
            if (ModelState.IsValid)
            {
                this.AddEnumDropDownData<ReadingGroupType>("Interval");

                // We can use GetValueOrDefault here because the Id value is 
                // validated to make sure the facility exists.
                var facility = Repository.Find(model.Id.GetValueOrDefault());
                var readings = Repository.GetReadings(facility.Id, model.Interval, model.StartDate.GetValueOrDefault(), model.EndDate.GetValueOrDefault());

                // Eventually this might need to do different calculations based on the sensor's measurement type.
                // For right now, all the linked sensors are just kW. If they actually use this then there will
                // need to be a way to spit out multiple charts based on the sensor measurement type.

                var cb = new ChartBuilder<DateTime, double>();

                switch (model.Interval)
                {
                    case ReadingGroupType.Daily:
                        cb.Interval = ChartIntervalType.Daily;
                        break;
                    case ReadingGroupType.Hourly:
                        cb.Interval = ChartIntervalType.Hourly;
                        break;
                    case ReadingGroupType.QuarterHour:
                        cb.Interval = ChartIntervalType.Minute;
                        break;
                }
                cb.SortType = ChartSortType.LowToHigh;
                cb.YMinValue = (double)0;
                cb.YAxisLabel = "kWh";
                cb.Title = "Daily Summary of Facility Sensor Readings";
                // Since there's only a single series, the legend isn't useful here.
                cb.LegendPosition = ChartLegendPosition.None;
                cb.NumberPrecision = 2;

                var costs = new List<FacilityReadingCost>(readings.Count);
                // Use this for nulls so we're not creating a bunch of useless objects.
                var nullKwhCost = new FacilityKwhCost();

                foreach (var byDate in readings)
                {
                    cb.AddSeriesValue("Total", byDate.Key, byDate.Value);

                    var cost = new FacilityReadingCost
                    {
                        Date = byDate.Key,
                        ReadingValue = byDate.Value
                    };

                    var kwhCost =
                        facility.KwhCosts.SingleOrDefault(x => x.StartDate <= cost.Date && cost.Date <= x.EndDate) ??
                        nullKwhCost;
                    cost.KwhCost = kwhCost.CostPerKwh;

                    costs.Add(cost);
                }
                model.ReadingCosts = costs.OrderBy(x => x.Date).ToList();

                ViewData["Chart"] = cb;
            }

            return PartialView("_Readings", model);
        }

        #endregion

        #region Facility Processes

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddFacilityProcess(AddFacilityProcessForFacilityController model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(FACILITY_AREA_MANAGEMENT_ROLE, RoleActions.Edit)]
        public ActionResult AddFacilityFacilityArea(AddFacilityFacilityArea model)
        {
            return ActionHelper.DoUpdate(model);
        }
 
        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveFacilityProcess(RemoveFacilityProcessForFacilityController model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region System Delivery

        [HttpPost, RequiresRole(RoleModules.ProductionSystemDeliveryConfiguration, RoleActions.Add)]
        public ActionResult AddSystemDeliveryEntryType(AddFacilitySystemDeliveryEntryType model)
        {
            var entity = _repository.Find(model.Id);

            if (entity.FacilitySystemDeliveryEntryTypes.Any(x =>
                x.IsEnabled && x.SystemDeliveryEntryType.Id == model.SystemDeliveryEntryType &&
                x.Facility.Id == model.Id && model.IsEnabled.Value))
            {
                this.DisplayNotification("Facility can have only one active entry per entry type configured at a time");
                return RedirectToReferrerOr("Show", "Facility", new {area = string.Empty, model.Id},
                    SYSTEM_DELIVERY_TAB);
            }
            else
            {
                return ActionHelper.DoUpdate(model, new ActionHelperDoUpdateArgs {
                    OnSuccess = () => RedirectToReferrerOr("Show", "Facility", new {area = string.Empty, model.Id},
                        SYSTEM_DELIVERY_TAB)
                });
            }
        }

        [HttpPost, RequiresRole(RoleModules.ProductionSystemDeliveryConfiguration, RoleActions.Delete)]
        public ActionResult RemoveSystemDeliveryEntryType(RemoveFacilitySystemDeliveryEntryType model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #region SystemDeliveryHistory

        [HttpGet, RequiresRole(ROLE)]
        public ActionResult GetSystemDeliveryHistoryForFacility(SearchFacilitySystemDeliveryHistory model)
        {
            var dateTimeProvider = _container.GetInstance<IDateTimeProvider>();
            var startDate = dateTimeProvider.GetCurrentDate().AddMonths(-6).Date;
            var endDate = dateTimeProvider.GetCurrentDate().Date;
            var repository = _container.GetInstance<SystemDeliveryFacilityEntryRepository>();

            model.SystemDeliveryHistory = repository.GetEntriesForFacility(model.Id, startDate, endDate).ToList();

            return PartialView("_FacilitySystemDeliveryHistoryDisplay", model);
        }

        #endregion

        [HttpPost, RequiresRole(FACILITY_AREA_MANAGEMENT_ROLE, RoleActions.Delete)]
        public ActionResult RemoveFacilityFacilityArea(RemoveFacilityFacilityArea model)
        {
            return ActionHelper.DoUpdate(model);
        }
    }
}
