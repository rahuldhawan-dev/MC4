using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Historian.Data.Client.Repositories;
using MapCall.Common.ClassExtensions;
using MapCall.Common.Configuration;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.ClassExtensions;
using MapCallMVC.Models.ViewModels;
using MMSINC;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCallMVC.Controllers
{
    [DisplayName("Equipment")]
    public class EquipmentController : SapSyncronizedControllerBaseWithPersisence<IEquipmentRepository, Equipment, User>
    {
        #region Constants

        public const string NOT_FOUND = "Equipment with the id '{0}' was not found.",
            ARC_FLASH_STATUS_MESSAGE = "This equipment is in a facility that has an Arc Flash status(es) of {0}.",
            CREATED_NOTIFICATION = "Equipment Record Created",
            IN_SERVICE_NOTIFICATION = "Equipment In Service",
            MANUFACTURER_UNKNOWN = "Equipment Manufacturer Unknown",
            PENDING_RETIREMENT_NOTIFICATION = "Pending Retirement", 
            RETIRED_NOTIFICATION = "Retired",
            FIELD_INSTALLED_NOTIFICATION = "Field Installed",
            REGULATORY_REQUIREMENT_NOTIFICATION = "Please note that this piece of equipment is marked as Environmental\\WQ Compliance.";

        public const RoleModules ROLE = RoleModules.ProductionEquipment,
                                 EAM_ROLE = RoleModules.EngineeringEAMAssetManagement;

        #endregion

        #region Private Methods

        /// <summary>
        /// This exists solely for the unit tests for this controller. These tests are doing validation testing
        /// rather than view model tests. They need to be rewritten.
        /// </summary>
        /// <param name="model"></param>
        internal void RunModelValidation(object model)
        {
            TryValidateModel(model);
        }

        private void SetReadingsSensorsLookupData(int equipmentId)
        {
            // This is set before we know if the model is actually valid
            // on either the Show or Readings actions.
            var equip = Repository.Find(equipmentId);

            // NOTE: This is different from the Sensor multilist data that's added
            //       during SetLookupData.
            if (equip != null)
            {
                this.AddDynamicDropDownData<Sensor, SensorDisplayItem>(x => x.Id, x => x.Display, "Sensors", _ => equip.Sensors.Select(x => x.Sensor).AsQueryable());
            }
            else
            {
                // doesn't matter what key/value getters are, collection will be empty
                this.AddDropDownData("sensors", Enumerable.Empty<Sensor>(), x => x.Id, x => x.Description);
            }

            this.AddEnumDropDownData<ReadingGroupType>("Interval");
        }

        private IQueryable<Equipment> GetEquipmentQueryActiveFacilityInServiceOrPendingEquipment(IQueryable<Equipment> query)
        {
            var result = query.Where(x => x.SAPEquipmentId.HasValue &&
                                          (x.EquipmentStatus.Id == EquipmentStatus.Indices.IN_SERVICE || x.EquipmentStatus.Id == EquipmentStatus.Indices.PENDING) &&
                                          x.Facility.FacilityStatus.Id != FacilityStatus.Indices.INACTIVE);
            return result;
        }

        #region Notifications

        private void SendEquipmentStatusChangeNotification(EquipmentStatus originalEquipmentStatus, EditEquipment equipment, int id)
        {
            if (!equipment.EquipmentStatus.HasValue)
                return;

            if ((originalEquipmentStatus == null || originalEquipmentStatus.Id != EquipmentStatus.Indices.IN_SERVICE) && equipment.EquipmentStatus.Value == EquipmentStatus.Indices.FIELD_INSTALLED)
                SendFieldInstalledNotification(Repository.Find(id));
            if ((originalEquipmentStatus == null || originalEquipmentStatus.Id != EquipmentStatus.Indices.IN_SERVICE) && equipment.EquipmentStatus.Value == EquipmentStatus.Indices.IN_SERVICE)
                SendInServiceNotificationToRequestedBy(Repository.Find(id));
            if ((originalEquipmentStatus == null || originalEquipmentStatus.Id != EquipmentStatus.Indices.PENDING_RETIREMENT) && equipment.EquipmentStatus.Value == EquipmentStatus.Indices.PENDING_RETIREMENT)
                SendPendingRetirementNotifications(Repository.Find(id));
            if ((originalEquipmentStatus == null || originalEquipmentStatus.Id != EquipmentStatus.Indices.RETIRED) &&
                equipment.EquipmentStatus.Value == EquipmentStatus.Indices.RETIRED)
            {
                var eq = Repository.Find(id);
                SendRetiredNotifications(eq);
                SendRetiredNotificationToRequestedBy(eq);
            }
        }

        private void SendCreationNotification(Equipment entity)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "Equipment");
            _container.GetInstance<INotificationService>().Notify(new NotifierArgs {
                OperatingCenterId = entity.Facility.OperatingCenter.Id,
                Module = ROLE,
                Purpose = CREATED_NOTIFICATION,
                Data = entity
            });
        }

        private void SendInServiceNotificationToRequestedBy(Equipment entity)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "Equipment");

            _container.GetInstance<INotificationService>().Notify(new NotifierArgs {
                OperatingCenterId = entity.Facility.OperatingCenter.Id,
                Module = ROLE,
                Purpose = IN_SERVICE_NOTIFICATION,
                Data = entity,
                Address = entity.RequestedBy.EmailAddress
            });
        }

        private void SendFieldInstalledNotification(Equipment entity)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "Equipment");

            _container.GetInstance<INotificationService>().Notify(new NotifierArgs
            {
                OperatingCenterId = entity.Facility.OperatingCenter.Id,
                Module = ROLE,
                Purpose = FIELD_INSTALLED_NOTIFICATION,
                Data = entity,
                Address = entity.RequestedBy.EmailAddress
            });
        }

        private void SendPendingRetirementNotifications(Equipment entity)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "Equipment");

            _container.GetInstance<INotificationService>().Notify(new NotifierArgs {
                OperatingCenterId = entity.Facility.OperatingCenter.Id,
                Module = ROLE,
                Purpose = PENDING_RETIREMENT_NOTIFICATION,
                Data = entity
            });
        }

        private void SendRetiredNotifications(Equipment entity)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "Equipment");

            _container.GetInstance<INotificationService>().Notify(new NotifierArgs
            {
                OperatingCenterId = entity.Facility.OperatingCenter.Id,
                Module = ROLE,
                Purpose = RETIRED_NOTIFICATION,
                Data = entity
            });
        }

        private void SendRetiredNotificationToRequestedBy(Equipment entity)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "Equipment");

            _container.GetInstance<INotificationService>().Notify(new NotifierArgs
            {
                OperatingCenterId = entity.Facility.OperatingCenter.Id,
                Module = ROLE,
                Purpose = RETIRED_NOTIFICATION,
                Data = entity,
                Address = entity.RequestedBy.EmailAddress
            });
        }

        private void SendEquipmentManufacturerUnknown(Equipment entity)
        {
            entity.RecordUrl = GetUrlForModel(entity, "Show", "Equipment");
            _container.GetInstance<INotificationService>().Notify(new NotifierArgs {
                OperatingCenterId = entity.Facility.OperatingCenter.Id,
                Module = ROLE,
                Purpose = MANUFACTURER_UNKNOWN,
                Data = entity
            });
        }

        #endregion

        protected override void UpdateEntityForSap(Equipment entity)
        {
            var equipment = new SAPEquipment(entity);
            var sapEquipment = _container.GetInstance<ISAPEquipmentRepository>().Save(equipment);

            if (!string.IsNullOrWhiteSpace(sapEquipment.SAPEquipmentNumber))
                entity.SAPEquipmentId = int.Parse(sapEquipment.SAPEquipmentNumber);
            entity.SAPErrorCode = sapEquipment.SAPErrorCode;
        }

        private void ShowOnModelFound(Equipment model)
        {
            ViewData["HasLinkedEquipment"] = Repository.SearchLinkedEquipment(new SearchEquipment {
                Facility = model.Facility.Id,
                EquipmentPurpose = new[] { model.EquipmentPurpose.Id },
                NotEqualEntityId = model.Id,
                OriginalEquipmentId = ((model.ReplacedEquipment != null))
                    ? model.ReplacedEquipment.Id
                    : (int?)null
            }).Any();
            DisplayNotifications(model);
        }

        private void DisplayNotifications(Equipment model)
        {
            foreach (var permit in model.EnvironmentalPermits)
            {
                DisplayNotification($"This piece of equipment needs to continue to function within the requirements of permit number {permit.PermitNumber}");
            }

            if (model.HasRegulatoryRequirement)
            {
                DisplayNotification(REGULATORY_REQUIREMENT_NOTIFICATION);
            }

            DisplayErrorMessage(model);
        }

        private void DisplayErrorMessage(Equipment model)
        {
            if (!string.IsNullOrWhiteSpace(model.SAPErrorCode) && !model.SAPErrorCode.ToUpper().Contains("SUCCESS"))
            {
                DisplayErrorMessage(model.SAPErrorCode);
            }
            if (model.Facility != null && model.Facility.ArcFlashStudies.Any(s => s.ArcFlashStatus != null && s.ArcFlashStatus.Id != ArcFlashStatus.Indices.N_A))
            {
                DisplayErrorMessage(string.Format(ARC_FLASH_STATUS_MESSAGE, string.Join(",",model.Facility.ArcFlashStudies.Select(s => s.ArcFlashStatus))));
            }
        }

        #endregion

        #region Exposed Methods

        public override void SetLookupData(ControllerAction action)
        {
            switch (action)
            {
                case ControllerAction.Edit:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Edit);
                    this.AddDropDownData<EquipmentType>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<EquipmentStatus>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<ABCIndicator>();
                    this.AddDropDownData<IFunctionalLocationRepository, FunctionalLocation>(f => f.GetByAssetTypeId(AssetType.Indices.EQUIPMENT), f => f.Id, f => f.Description);
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("RequestedBy", r => r.GetActiveEmployeesForSelectWhere(e => e.EmailAddress != null));
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("AssetControlSignOffBy", r => r.GetActiveEmployeesForSelectWhere(_ => true));
                    this.AddDynamicDropDownData<ScadaTagName, ScadaTagNameDisplayItem>(n => n.Id, n => n.Display);
                    break;
                case ControllerAction.New:
                    this.AddOperatingCenterDropDownDataForRoleAndAction(ROLE, RoleActions.Add, extraFilterP: x => x.IsActive);
                    this.AddDropDownData<EquipmentType>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<EquipmentStatus>(f => f.GetAllSorted(x => x.Description).Where(x => x.Description.ToUpper() != "IN SERVICE"), f => f.Id, f => f.Description);
                    this.AddDropDownData<ABCIndicator>();
                    this.AddDropDownData<IFunctionalLocationRepository, FunctionalLocation>(f => f.GetByAssetTypeId(AssetType.Indices.EQUIPMENT), f => f.Id, f => f.Description);
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("RequestedBy", r => r.GetActiveEmployeesForSelectWhere(e => e.EmailAddress != null));
                    this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>("AssetControlSignOffBy", r => r.GetActiveEmployeesForSelectWhere(_ => true));
                    this.AddDynamicDropDownData<Equipment, EquipmentDisplayItem>( e => e.Id, e => e.Display, "ReplacedEquipment");
                    this.AddDynamicDropDownData<ScadaTagName, ScadaTagNameDisplayItem>(n => n.Id, n => n.Display);
                    break;
                case ControllerAction.Search:
                    this.AddDropDownData<ABCIndicator>();
                    this.AddDropDownData<Department>();
                    this.AddDropDownData<EquipmentStatus>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<EquipmentLifespan>(f => f.GetAllSorted(x => x.Description), f => f.Id, f => f.Description);
                    this.AddDropDownData<IFunctionalLocationRepository, FunctionalLocation>(f => f.GetByAssetTypeId(AssetType.Indices.EQUIPMENT), f => f.Id, f => f.Description);
                    break;
                case ControllerAction.Show:
                    this.AddDynamicDropDownData<Sensor, SensorDisplayItem>(x => x.Id, x => x.Display);
                    this.AddDropDownData<LinkType>();
                    break;
            }
        }

        #endregion

        #region Search/Index/Show

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Search(SearchEquipment search)
        {
            return ActionHelper.DoSearch(search);
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Show(int id)
        {
            var x = this.RespondTo((f) => {
                Equipment entity;
                f.View(() => {
                    SetReadingsSensorsLookupData(id);
                    return ActionHelper.DoShow(id,
                        new ActionHelperDoShowArgs { NotFound = string.Format(NOT_FOUND, id) },
                        onModelFound: ShowOnModelFound);
                });
                f.Json(() => {
                    entity = Repository.Find(id);
                    if (entity == null)
                        DoHttpNotFound(string.Format(NOT_FOUND, id));
                    var coordinate = entity.Coordinate ?? entity.Facility?.Coordinate;
                    return Json(
                        new {
                            coordinate?.Latitude,
                            coordinate?.Longitude,
                            CoordinateId = coordinate?.Id,
                            entity.ProductionPrerequisites,
                            entity.StandardOperatingProcedureDocumentId
                        }, JsonRequestBehavior.AllowGet);
                });
                f.Fragment(() => ActionHelper.DoShow(id, new MMSINC.Utilities.ActionHelperDoShowArgs
                {
                    NotFound = $"Equipment with id '{id}' was not found.",
                    IsPartial = true,
                    ViewName = "_ShowPopup"
                }));
            });
            return x;
        }

        [HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Index(SearchEquipment search)
        {
            return this.RespondTo((formatter) => {
                formatter.View(() => ActionHelper.DoIndex(search));
                formatter.Fragment(() => {
                    search.EnablePaging = true;
                    return ActionHelper.DoIndex(search,
                        new ActionHelperDoIndexArgs {
                            SearchOverrideCallback = () => Repository.SearchLinkedEquipment(search),
                            IsPartial = true,
                            ViewName = "_Equipments",
                            OnNoResults = () => PartialView("_NoResults")
                        });
                });
                formatter.Excel(() => {
                    search.EnablePaging = false;
                    var results = Repository.Search(search).Select(e => new Dictionary<string, object> {
                        {nameof(e.Id), e.Id},
                        {nameof(e.Description), e.Description},
                        {nameof(e.EquipmentPurpose.EquipmentLifespan), e.EquipmentPurpose?.EquipmentLifespan},
                        {nameof(e.EquipmentPurpose), e.EquipmentPurpose},
                        {nameof(e.EquipmentStatus), e.EquipmentStatus},
                        {nameof(e.DateInstalled), e.DateInstalled},
                        {nameof(e.DateRetired), e.DateRetired},
                        {nameof(e.EquipmentManufacturer), e.EquipmentManufacturer},
                        {nameof(e.EquipmentModel), e.EquipmentModel},
                        {nameof(e.SerialNumber), e.SerialNumber},
                        {nameof(e.WBSNumber), e.WBSNumber},
                        {nameof(e.Legacy), e.Legacy},
                        {nameof(e.HasComplianceRequirement), e.HasComplianceRequirement},
                        {nameof(e.OperatingCenter), e.OperatingCenter},
                        {nameof(e.Facility), e.Facility},
                        {nameof(e.FacilityFacilityArea.FacilityArea),e.FacilityFacilityArea?.FacilityArea},
                        {nameof(e.FacilityFacilityArea.FacilitySubArea),e.FacilityFacilityArea?.FacilitySubArea},
                        {nameof(e.EquipmentType), e.EquipmentType},
                        {nameof(e.CriticalNotes), e.CriticalNotes},
                        {nameof(e.HasAtLeastOneWellTest), e.HasAtLeastOneWellTest},
                        {nameof(e.Coordinate), e.Coordinate},
                        {nameof(e.Icon), e.Icon},
                        {nameof(e.Identifier), e.Identifier},
                        {nameof(e.Number), e.Number},
                        {nameof(e.CriticalRating), e.CriticalRating},
                        {nameof(e.SafetyNotes), e.SafetyNotes},
                        {nameof(e.MaintenanceNotes), e.MaintenanceNotes},
                        {nameof(e.OperationNotes), e.OperationNotes},
                        {nameof(e.SAPEquipmentId), e.SAPEquipmentId},
                        {nameof(e.HasSensorAttached), e.HasSensorAttached},
                        {nameof(e.HasNoSAPEquipmentId), e.HasNoSAPEquipmentId},
                        {nameof(e.IsSignedOffByAssetControl), e.IsSignedOffByAssetControl},
                        {nameof(e.Portable), e.Portable},
                        {nameof(e.ArcFlashHierarchy), e.ArcFlashHierarchy},
                        {nameof(e.ArcFlashRating), e.ArcFlashRating},
                        {nameof(e.CreatedAt), e.CreatedAt},
                        {nameof(e.AssetControlSignOffDate), e.AssetControlSignOffDate},
                        {nameof(e.SAPErrorCode), e.SAPErrorCode},
                        {nameof(e.ParentEquipment), e.ParentEquipment},
                        {nameof(e.Condition), e.Condition},
                        {nameof(e.Performance), e.Performance},
                        {nameof(e.StaticDynamicType), e.StaticDynamicType},
                        {nameof(e.ConsequenceOfFailure), e.ConsequenceOfFailure},
                        {nameof(e.LikelyhoodOfFailure), e.LikelyhoodOfFailure},
                        {nameof(e.Reliability), e.Reliability},
                        {nameof(e.RiskOfFailure), e.RiskOfFailure},
                        {nameof(e.LocalizedRiskOfFailureText), e.LocalizedRiskOfFailureText},
                        {nameof(e.FunctionalLocation), e.FunctionalLocation},
                        {nameof(e.ABCIndicator), e.ABCIndicator},
                        {nameof(e.ManufacturerOther), e.ManufacturerOther},
                        {nameof(e.ProductionPrerequisites), string.Join(", ", e.ProductionPrerequisites)},
                        {nameof(e.ScadaTagName), e.ScadaTagName},
                        {nameof(e.Generator), e.Generator},
                        {nameof(e.RequestedBy), e.RequestedBy},
                        {nameof(e.AssetControlSignOffBy), e.AssetControlSignOffBy},
                        {nameof(e.CreatedBy), e.CreatedBy},
                        {nameof(e.ReplacedEquipment), e.ReplacedEquipment},
                        {nameof(e.IsGenerator), e.IsGenerator},
                        {nameof(e.RecordUrl), e.RecordUrl},
                        {nameof(e.Display), e.Display},
                        {nameof(e.WorkOrderAccessible), e.WorkOrderAccessible},
                        {nameof(e.StandardOperatingProcedureDocumentId), e.StandardOperatingProcedureDocumentId},
                        {nameof(e.Latitude), e.Latitude},
                        {nameof(e.Longitude), e.Longitude},
                        {nameof(e.SAPSync), e.SAPSync},
                        {nameof(e.TankInspections), string.Join(", ", e.TankInspections)}, 
                        {nameof(e.HasTankInspections), e.HasTankInspections},
                        {nameof(e.Characteristics), e.Characteristics}, {
                            "CharacteristicFields", e.EquipmentType?.CharacteristicFields
                        }, // need to do this because if EquipmentType is null then Object Reference exception is thrown
                        {nameof(e.HasProcessSafetyManagement), e.HasProcessSafetyManagement},
                        {nameof(e.HasCompanyRequirement), e.HasCompanyRequirement},
                        {nameof(e.HasRegulatoryRequirement), e.HasRegulatoryRequirement},
                        {nameof(e.HasOshaRequirement), e.HasOshaRequirement},
                        {nameof(e.OtherCompliance), e.OtherCompliance},
                        {nameof(e.OtherComplianceReason), e.OtherComplianceReason},
                        {nameof(e.EquipmentGroup), e.EquipmentGroup}
                    });

                    IList<Dictionary<string, object>> resultslist = new List<Dictionary<string, object>>();
                    var displayDict = new Dictionary<string, string>();

                    foreach (var prop in results.First().Keys)
                    {
                        var displayNameFromVM = typeof(EquipmentViewModel)
                                               .GetProperty(prop)?.GetCustomAttribute<ViewAttribute>();

                        var displayNameFromEntity = displayNameFromVM != null
                            ? displayNameFromVM
                            : typeof(Equipment)
                             .GetProperty(prop)?.GetCustomAttribute<ViewAttribute>();

                        var headerName = displayNameFromVM != null && displayNameFromVM.DisplayName != null
                            ? displayNameFromVM.DisplayName
                            : prop;

                        var actualHeaderName =
                            headerName == prop && displayNameFromEntity != null &&
                            displayNameFromEntity.DisplayName != null
                                ? displayNameFromEntity.DisplayName
                                : headerName;
                        displayDict.Add(prop, actualHeaderName);
                    }

                    foreach (var x in results)
                    {
                        var dictionary = x.Keys.ToDictionary(prop => displayDict[prop], prop => x[prop]);

                        if (x["EquipmentType"] != null)
                        {
                            var charfields = (IList<EquipmentCharacteristicField>)x["CharacteristicFields"];
                            var Dictcharacteristics = (IList<EquipmentCharacteristic>)x["Characteristics"];

                            foreach (var k in charfields)
                            {
                                if (!dictionary.ContainsKey(k.DisplayField))
                                {
                                    var characteristic = Dictcharacteristics.SingleOrDefault(c => c.Field.Id == k.Id);
                                    dictionary.Add(k.DisplayField, characteristic != null ? characteristic.DisplayValue : string.Empty);
                                }
                            }
                        }

                        dictionary.Remove("Characteristics");
                        dictionary.Remove("CharacteristicFields"); // Need this to remove the field as there is no reason for this to be on the export
                        // BUT we need this field present in the object to get the characteristics fields - Greg - 02/06/2020
                        resultslist.Add(dictionary);
                    }

                    return this.Excel(resultslist);
                });
                formatter.Map(() => _container.GetInstance<IMapResultFactory>().Build(ModelState, Repository, search));
            });
        }

        #endregion

        #region New/Create

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult New(CreateEquipment model)
        {
            // I guess we're passing values to this action, but we don't want to validate
            // any of them?
            ModelState.Clear();
            return ActionHelper.DoNew(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Create(CreateEquipment model)
        {
            return ActionHelper.DoCreate(model, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    var ent = Repository.Find(model.Id);
                    SendCreationNotification(ent);

                    if (!string.IsNullOrWhiteSpace(model.ManufacturerOther))
                    {
                        SendEquipmentManufacturerUnknown(ent);
                    }
                    OnEditUpdateSuccess(model);
                    // Why does this redirect to Edit?
                    return DoRedirectionToAction("Edit", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Copy

        [HttpPost, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Copy(int id)
        {
            var equipment = Repository.Find(id);
            var preqCopy = equipment.ProductionPrerequisites.Select(x => x.Id).ToArray();
            if (equipment == null || !equipment.CanBeCopied)
            {
                return DoHttpNotFound($"Could not find equipment with id {id}");
            }

            var copy = ViewModelFactory.BuildWithOverrides<CopyEquipment, Equipment>(equipment, new {
                EquipmentPurpose = 0,
                Prerequisites = preqCopy
            });

            return ActionHelper.DoCreate(copy, new MMSINC.Utilities.ActionHelperDoCreateArgs {
                OnSuccess = () => {
                    SendCreationNotification(Repository.Find(copy.Id));
                    return RedirectToAction("Edit", new { id = copy.Id });
                }
            });

        }

        #endregion

        #region Edit/Update

        [HttpGet, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Edit(int id)
        {
            return ActionHelper.DoEdit<EditEquipment>(id, null, onModelFound: DisplayNotifications);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult Update(EditEquipment equipment, FormCollection form)
        {
            equipment.Form = form;

            if (!equipment.EquipmentStatus.HasValue)
            {
                return ActionHelper.DoUpdate(equipment, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                    OnSuccess = () => {
                        OnEditUpdateSuccess(equipment);
                        return RedirectToAction("Show", new { id = equipment.Id });
                    }
                });
            }

            //var original = Repository.Find(equipment.Id);
            //var newStatus =
            //    _container.GetInstance<IRepository<EquipmentStatus>>().Find(equipment.EquipmentStatus.Value);
            //var sendNotification = (original.EquipmentStatus == null || original.EquipmentStatus.Description != "In Service") &&
            //                       newStatus.Description == "In Service";

            // TODO: Move this check to the ViewModel so we're not calling DoUpdate in two ways. -Ross 7/13/2018
            var originalEquipmentStatus = Repository.Find(equipment.Id).EquipmentStatus;

            return ActionHelper.DoUpdate(equipment, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                NotFound = string.Format(NOT_FOUND, equipment.Id),
                OnSuccess = () => {
                    OnEditUpdateSuccess(equipment);
                    SendEquipmentStatusChangeNotification(originalEquipmentStatus, equipment, equipment.Id);
                    return DoRedirectionToAction("Show", new { equipment.Id });
                }
            });
        }

        private void OnEditUpdateSuccess(EquipmentViewModel model)
        {
            if (model.SendToSAP)
            {
                UpdateSAP(model.Id, ROLE);
            }
        }

        #region Children
        
        [HttpPost, RequiresRole(ROLE, RoleActions.Edit), RequiresSecureForm]
        public ActionResult AddEquipmentMaintenancePlan(AddSingleEquipmentMaintenancePlan model)
        {
            return ActionHelper.DoUpdate(model);
        }

        #endregion

        #endregion

        #region Links

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddLink(AddEquipmentLink model)
        {
            return ActionHelper.DoUpdate(model);
        }

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveLink(RemoveEquipmentLink model)
        {
            return ActionHelper.DoUpdate(model);
        }
        #endregion

        #region Replace

        [HttpGet, RequiresRole(ROLE, RoleActions.Add)]
        public ActionResult Replace(int id)
        {
            var equipment = Repository.Find(id);
            var prereqs = equipment.ProductionPrerequisites.Select(x => x.Id).ToArray();

            if (equipment == null)
            {
                return DoHttpNotFound(String.Format("Could not find equipment with identifier {0}.", id));
            }
            if (equipment.HasNoSAPEquipmentId)
            {
                return RedirectToAction("Show", new {id});
            }

            var replace = ViewModelFactory.Build<ReplaceEquipment, Equipment>(equipment);
            replace.Prerequisites = prereqs;
            return New(replace);
        }

        #endregion

        #region AddSensor

        [HttpPost, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult AddSensor(AddEquipmentSensor model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnError = () => {
                    DisplayModelStateErrors();
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region RemoveSensor

        [HttpDelete, RequiresRole(ROLE, RoleActions.Edit)]
        public ActionResult RemoveSensor(RemoveEquipmentSensor model)
        {
            return ActionHelper.DoUpdate(model, new MMSINC.Utilities.ActionHelperDoUpdateArgs {
                OnError = () => {
                    DisplayModelStateErrors();
                    return RedirectToAction("Show", new { id = model.Id });
                }
            });
        }

        #endregion

        #region Readings

        [NoCache, HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult Readings(SearchEquipmentReadings model)
        {
            // This needs to be called regardless of the model being valid.
            // If the equipment doesn't exist, an empty collection of sensors
            // is sent back rather than throwing an exception.
            SetReadingsSensorsLookupData(model.Id.GetValueOrDefault());

            if (ModelState.IsValid)
            {
                var sensorRepo = _container.GetInstance<ISensorRepository>();
                var sensorIds = model.Sensors ?? new int[] { };

                // ToArray this so we aren't constantly doing sensor lookups.
                var modelSensors = sensorIds.Select(x => sensorRepo.Find(x)).ToArray();

                var measType =
                    _container.GetInstance<ISensorMeasurementTypeRepository>()
                        .GetByDescription(SensorMeasurementType.Descriptions.KILOWATT);

                var startDate = model.StartDate.GetValueOrDefault().Date; // Ensure we're at the start of the day
                var endDate = model.EndDate.GetValueOrDefault();
                var rcs = sensorRepo.GetGroupedReadingCalculations(modelSensors, model.Interval, startDate, endDate, true);
                model.Readings = rcs.Readings.OrderByDescending(x => x.DateTimeStamp).ToArray();

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
                cb.Title = "Hourly Summary of Sensor Readings";
                cb.NumberPrecision = 2;

                var displayTotal = modelSensors.Count() > 1;
                if (displayTotal)
                {
                    foreach (var totes in rcs.Totals)
                    {
                        cb.AddSeriesValue("Total", totes.Key, totes.Value);
                    }
                }

                foreach (var calc in rcs.Calculations)
                {
                    cb.AddSeriesValue(calc.Sensor.Name, calc.Date, calc.Value);
                }

                ViewData["Chart"] = cb;
            }

            return PartialView("_Readings", model);
        }

        #endregion

        #region ScadaReadings

        [NoCache, HttpGet, RequiresRole(ROLE, RoleActions.Read)]
        public ActionResult ScadaReadings(SearchEquipmentScadaReadings model)
        {
            var equipment = Repository.Find(model.Id);
            model.TagName = equipment.ScadaTagName;
            model.Readings =
                _container.GetInstance<IRawDataRepository>()
                    .FindByTagName(equipment.ScadaTagName.TagName, model.UseRaw, model.StartDate, model.EndDate);

            return this.RespondTo(f => {
                f.View(() => PartialView("_ScadaReadings", model));
                f.Excel(() => this.Excel(model.Readings));
            });
        }

        #endregion

        #region ByFunctionalLocation

        [HttpGet]
        public ActionResult ByFunctionalLocation(string functionalLocation)
        {
            // we want to compare against the first two parts of the functional location 
            // e.g. just 1A-2B from 1A-2B-3C-4D
            var functionalLocations = functionalLocation.Split('-');
            if (functionalLocations.Length >= 2)
            {
                return new CascadingActionResult<Equipment, EquipmentDisplayItem>(Repository.Where(x =>
                        x.FunctionalLocation != null && x.FunctionalLocation != "" &&
                        x.FunctionalLocation.StartsWith($"{functionalLocation}")),
                    "Display", "Id") {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return new CascadingActionResult(Enumerable.Empty<Equipment>());
        }

        [HttpGet]
        public ActionResult EquipmentTypesByFunctionalLocation(string functionalLocation)
        {
            var EquipmentTypes =
                Repository.Where(x => x.FunctionalLocation != null && x.FunctionalLocation != "" &&
                                      x.FunctionalLocation.StartsWith(functionalLocation))
                          .Select(x => new EquipmentType {
                               Description = x.EquipmentType.Description,
                               Abbreviation = x.EquipmentType.Abbreviation
                           }).Distinct().ToList();
            return new CascadingActionResult(EquipmentTypes, "Description", "Abbreviation");
        }

        public ActionResult ByFacilityFunctionalLocation(int facilityId)
        {
            var facility = _container.GetInstance<IRepository<Facility>>().Find(facilityId);
            var facilityFunctionalLocation = facility.FunctionalLocation;
            // we want to compare against the first two parts of the functional location 
            // e.g. just 1A-2B from 1A-2B-3C-4D
            var functionalLocations = facilityFunctionalLocation != null ?
                facilityFunctionalLocation.Split('-') : null;
            if (functionalLocations?.Length >= 2)
            {
                return new CascadingActionResult<Equipment, EquipmentDisplayItem>(Repository.Where(x =>
                        x.FunctionalLocation != null && x.FunctionalLocation != "" &&
                        x.FunctionalLocation.StartsWith($"{facilityFunctionalLocation}")),
                    "Display", "Id")
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            return new CascadingActionResult(Enumerable.Empty<Equipment>());
        }

        #endregion

        #region ByFacilityId

        [HttpGet]
        public ActionResult ByFacilityId(int facilityId)
        {
            return new CascadingActionResult<Equipment, EquipmentDisplayItem>(Repository.GetByFacilityId(facilityId), "Display", "Id") 
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

       
        [HttpGet]
        public ActionResult ByFacilityIds(List<int> facilityIds)
        {
            return new CascadingActionResult<Equipment, EquipmentDisplayItem>(Repository.GetByFacilityIds(facilityIds.ToArray()), "Display", "Id")
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ByFacilityIdsForSystemDelivery(int[] facilityId)
        {
            return new CascadingActionResult<Equipment, EquipmentDisplayItem>(Repository.GetByFacilityIds(facilityId)
               .Where(x => x.EquipmentType.Id == EquipmentType.Indices.FLO_MET
                           && x.EquipmentPurpose.EquipmentSubCategory != null), "EquipmentDescriptionWithRegionalPlanningArea", "Id");
        }

        [HttpGet]
        public ActionResult ByFacilityIdAndIsEligibleForRedTagPermitEquipmentTypes(int facilityId)
        {
            var query = Repository.GetByFacilityId(facilityId)
                                  .Where(x => x.EquipmentType.IsEligibleForRedTagPermit);

            return new CascadingActionResult<Equipment, EquipmentDisplayItem>(query, "Description", "Id");
        }

        //This is currently used by Production only, so we are displaying "Description" as per their request.
        [HttpGet]
        public ActionResult ByFacilityIdAndSometimesEquipmentTypeIdAndProductionWorkOrder(int facilityId, int? equipmentTypeId, int? productionWorkOrder)
        {
            var query = Repository.GetByFacilityId(facilityId);

            if (productionWorkOrder.HasValue)
            {
                var productionWo = _container.GetInstance<MMSINC.Data.NHibernate.RepositoryBase<ProductionWorkOrder>>().Find(productionWorkOrder.Value);
                var result = new CascadingActionResult {
                    TextField = nameof(EquipmentDisplayItem.Description),
                    ValueField = nameof(EquipmentDisplayItem.Id),
                    Data = productionWo.Equipments.Where(x => x.Equipment != null).Select(x => x.Equipment).Distinct()
                };
                return result;
            }

            else if (equipmentTypeId.HasValue)
            {
                query = query.Where(x => x.EquipmentType.Id == equipmentTypeId);
            }

            return new CascadingActionResult<Equipment, EquipmentDisplayItem>(query, "Description", "Id");
        }

        public ActionResult ByFacilityIdAndOrFacilityFacilityAreaIdAndOrEquipmentTypeId(int? facilityId, int? facilityFacilityAreaId, int? equipmentTypeId)
        {
            var query = Repository.Where(x => true);
            if (!facilityId.HasValue && !equipmentTypeId.HasValue)
            {
                // This isn't likely to ever happen. But we don't currently deal with 
                // validation in cascading action results, and we don't want to return 
                // every piece of equipment unfiltered if neither parameter has a value.
                // So have this act like any other cascading action when it receives a 
                // value that doesn't coorespond to anything in the database by returning
                // an empty result. -Ross 3/26/2020
                query = Enumerable.Empty<Equipment>().AsQueryable();
            }
            else
            {
                if (facilityId.HasValue)
                {
                    query = Repository.GetByFacilityId(facilityId.Value);
                }
                if (facilityFacilityAreaId.HasValue)
                {
                    query = query.Where(x => x.FacilityFacilityArea.Id == facilityFacilityAreaId);
                }
                if (equipmentTypeId.HasValue)
                {
                    query = query.Where(x => x.EquipmentType.Id == equipmentTypeId);
                }
            }
            return new CascadingActionResult<Equipment, EquipmentDisplayItem>(query, "Description", "Id");
        }

        [HttpGet]
        public ActionResult ByFacilityIdForEquipmentTypeOfWell(int facilityId)
        {
            var query = Repository.GetByFacilityId(facilityId)
                                  .Where(x => x.EquipmentType.Id == EquipmentType.Indices.WELL);

            return new CascadingActionResult<Equipment, EquipmentDisplayItem>(query, "Description", "Id");
        }

        [HttpGet]
        public ActionResult GetActiveInServiceSAPEquipmentByFacilityIdOrFacilityFacilityAreaIdOrEquipmentTypeId(int facilityId, int? facilityFacilityAreaId, int? equipmentTypeId)
        {
            var query = Repository.GetByFacilityId(facilityId);

            if (facilityFacilityAreaId.HasValue)
            {
                query = query.Where(x => x.FacilityFacilityArea.Id == facilityFacilityAreaId);
            }

            if (equipmentTypeId.HasValue)
            {
                query = query.Where(x => x.EquipmentType.Id == equipmentTypeId);
            }

            query = query.Where(x => x.SAPEquipmentId.HasValue && x.EquipmentStatus.Id != EquipmentStatus.Indices.RETIRED && x.Facility.FacilityStatus.Id != FacilityStatus.Indices.INACTIVE);

            return new CascadingActionResult<Equipment, EquipmentDisplayItem>(query, "Description", "Id");
        }

        //This is currently used by Production only, so we are displaying "Description" as per their request.
        // For adding equipment to a new maintenance plan
        [HttpGet]
        public ActionResult GetActiveInServiceSAPEquipmentByFacilityIdsOrEquipmentTypeIds(int[] facilityIds, int[] equipmentTypeIds)
        {
            var query = Repository.GetByFacilityIds(facilityIds);

            if (equipmentTypeIds.Length != 0)
            {
                query = query.Where(x => equipmentTypeIds.Contains(x.EquipmentType.Id));
            }

            query = GetEquipmentQueryActiveFacilityInServiceOrPendingEquipment(query);

            return new CascadingActionResult(query.Select(e => new {e.Id, e.Description}), "Description", "Id");
        }

        //This is currently used by Production only, so we are displaying "Description" as per their request.
        // For adding equipment when editing an existing maintenance plan
        [HttpGet]
        public ActionResult GetActiveInServiceSAPEquipmentWhereNotPresentInPlan(int[] facilityIds, int[] equipmentTypeIds, int maintenancePlanId)
        {
            var query = Repository.GetByFacilityIds(facilityIds);

            if (equipmentTypeIds.Length != 0)
            {
                query = query.Where(x => equipmentTypeIds.Contains(x.EquipmentType.Id));
            }

            query = GetEquipmentQueryActiveFacilityInServiceOrPendingEquipment(query);
            query = query.Where(equipment => equipment.MaintenancePlans.All(
                equipmentMaintenancePlans => equipmentMaintenancePlans.MaintenancePlan.Id != maintenancePlanId));
            return new CascadingActionResult(query.Select(e => new { e.Id, e.Description }), "Description", "Id");
        }

        #endregion

        #region ByTownId

        [HttpGet]
        public ActionResult ByTownIdForWorkOrders(int townId)
        {
            return new CascadingActionResult<Equipment, EquipmentDisplayItem>(Repository.GetByTownIdForWorkOrders(townId)) {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        #endregion

        #region GasDetectorsAndWaterTanksByOperatingCenter

        public ActionResult GasDetectorsByOperatingCenter(int operatingCenterId)
        {
            var results = Repository.Where(x => x.OperatingCenter.Id == operatingCenterId
                                            && x.EquipmentType.Id == EquipmentType.Indices.SAFGASDT
                                            && x.EquipmentPurpose.Abbreviation == EquipmentPurpose.PERSONAL_GAS_DETECTOR_ABBREVIATION);
            return new CascadingActionResult<Equipment, EquipmentDisplayItem>(results, "Description", "Id");
        }

        [HttpGet]
        public ActionResult ByOperatingCenterOnlyPotableWaterTanks(int operatingCenterId)
        {
            var results = Repository.Where(e => e.OperatingCenter.Id == operatingCenterId
                                                && e.EquipmentType.Id == EquipmentType.Indices.TNK_WPOT);
            return new CascadingActionResult<Equipment, EquipmentDisplayItem>(results, "Description", "Id");
        }
        
        #endregion

        #region GetCriticalNotes

        [HttpGet]
        public ActionResult GetCriticalNotes(int id)
        {
            return Content(Repository.Find(id).CriticalNotes);
        }

        #endregion

        public EquipmentController(ControllerBaseWithPersistenceArguments<IEquipmentRepository, Equipment, User> args) : base(args) {}
    }
}
