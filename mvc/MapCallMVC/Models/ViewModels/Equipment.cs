using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Historian.Data.Client.Entities;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Controllers;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using DataType = System.ComponentModel.DataAnnotations.DataType;

namespace MapCallMVC.Models.ViewModels
{
    public class EquipmentViewModel : ViewModel<Equipment>
    {
        #region Constants

        public const string SAP_EQUIPMENTID_ALREADY_IN_USE = "The SAPEquipmentId is already in use.";
        public const string SHORT_REPLACEMENT_PROD_WORK_ORDER_ID_LABEL = "Production Work Order ID";
        
        #endregion

        #region Properties

        [AutoMap(MapDirections.ToViewModel)]
        public override int Id { get; set; }

        [AutoMap(MapDirections.None)]
        [View(Description = "Check if you have the SAP Equipment # and want to enter it.")]
        public virtual bool SAPEquipmentIdOverride { get; set; }

        [RequiredWhen("SAPEquipmentIdOverride", ComparisonType.EqualTo, true)]
        [View("SAPEquipment #")]
        public virtual int? SAPEquipmentId { get; set; }

        [DropDown, EntityMap("Facility.OperatingCenter", MapDirections.ToPrimary), Required]
        public int? OperatingCenter { get; set; }

        [AutoMap(MapDirections.None)]
        [DropDown(Area = "", Controller = "PlanningPlant", Action = "ByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public virtual int? PlanningPlant { get; set; }

        [DropDown(Area = "", Controller = "Facility", Action = "ActiveByOperatingCenterOrPlanningPlant", DependsOn = "OperatingCenter,PlanningPlant", PromptText = "Please select an operating center above", DependentsRequired = DependentRequirement.One)]
        [View("Facility")]
        [EntityMustExist(typeof(Facility))]
        [EntityMap]
        [Required]
        public virtual int? Facility { get; set; }

        [DropDown(Area = "", Controller = "FacilityFacilityArea", Action = "ByFacilityId", DependsOn = "Facility", PromptText = "Please select Facility above")]
        [View("Facility Area")]
        [EntityMustExist(typeof(FacilityFacilityArea))]
        [EntityMap]
        public virtual int? FacilityFacilityArea { get; set; }

        [View(SHORT_REPLACEMENT_PROD_WORK_ORDER_ID_LABEL)]
        [DropDown("Production", "ProductionWorkOrder", "CorrectiveWorkOrdersForReplacedEquipment", DependsOn = "ReplacedEquipment")]
        [EntityMap, EntityMustExist(typeof(ProductionWorkOrder))]
        public virtual int? ReplacementProductionWorkOrder { get; set; }

        [Coordinate, View("Coordinates")]
        [EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        public virtual int? Coordinate { get; set; }

        [EntityMap, EntityMustExist(typeof(Equipment))]
        public virtual int? ReplacedEquipment { get; set; }

        [Required, DropDown]
        [View("Equipment Status")]
        [EntityMustExist(typeof(EquipmentStatus))]
        [EntityMap]
        public virtual int? EquipmentStatus { get; set; }

        [View("Equipment Group"), DoesNotAutoMap("Used for display only, set in Map.")]
        public virtual EquipmentGroup EquipmentGroup { get; set; }

        [DropDown]
        [View("Equipment Type")]
        [EntityMustExist(typeof(EquipmentType))]
        [EntityMap]
        [Required]
        public virtual int? EquipmentType { get; set; }

        [DropDown("", "EquipmentPurpose", "ByEquipmentTypeId", DependsOn = "EquipmentType", PromptText = "Please select an Equipment Type above")]
        [EntityMustExist(typeof(EquipmentPurpose))]
        [EntityMap]
        public virtual int? EquipmentPurpose { get; set; }

        [DropDown, EntityMustExist(typeof(ABCIndicator)), EntityMap]
        public virtual int? ABCIndicator { get; set; }

        [View("Equipment Model")]
        [DropDown("EquipmentModel", "ByManufacturerID", DependsOn = "EquipmentManufacturer", PromptText = "Please select a manufacturer above")]
        [EntityMustExist(typeof(EquipmentModel))]
        [EntityMap]
        public virtual int? EquipmentModel { get; set; }

        [CheckBoxList]
        [EntityMap(MapDirections.None /* This is manually implemented*/), EntityMustExist(typeof(ProductionPrerequisite))]
        public virtual int[] Prerequisites { get; set; }
        [RoleSecured(EquipmentController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public virtual bool HasProcessSafetyManagement { get; set; }
        [RoleSecured(EquipmentController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public virtual bool HasCompanyRequirement { get; set; }
        [RoleSecured(EquipmentController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public virtual bool HasRegulatoryRequirement { get; set; }
        [RoleSecured(EquipmentController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public virtual bool HasOshaRequirement { get; set; }
        [RoleSecured(EquipmentController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        public virtual bool OtherCompliance { get; set; }
        [RoleSecured(EquipmentController.ROLE, RoleActions.UserAdministrator, AppliesToAdmins = false)]
        [StringLength(255), RequiredWhen("OtherCompliance", ComparisonType.EqualTo, true)]
        public virtual string OtherComplianceReason { get; set; }

        [View("SAP Function Location")]
        [StringLength(Equipment.StringLengths.FUNCTIONAL_LOCATION)]
        [ClientCallback("EquipmentForm.validateFunctionalLocation", ErrorMessage = "The Functional Location field is required; please set Functional Location on the Facility.")]
        [UIHint("FunctionalLocation")]
        public virtual string FunctionalLocation { get; set; }

        [StringLength(Equipment.StringLengths.WBS_NUMBER)]
        public virtual string WBSNumber { get; set; }

        [EntityMap, EntityMustExist(typeof(Equipment))]
        public virtual int? ParentEquipment { get; set; }

        [Required]
        [StringLength(Equipment.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        [DropDown, EntityMustExist(typeof(Employee)), EntityMap]
        public virtual int? RequestedBy { get; set; }

        [DropDown, EntityMustExist(typeof(Employee)), EntityMap]
        public virtual int? AssetControlSignOffBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public virtual DateTime? AssetControlSignOffDate { get; set; }

        [RequiredWhen("EquipmentStatus", ComparisonType.EqualTo, MapCall.Common.Model.Entities.EquipmentStatus.Indices.RETIRED, ErrorMessage = "DateRetired is required when a piece of equipment is retired.")]
        public virtual DateTime? DateRetired { get; set; }

        [StringLength(Equipment.StringLengths.CRITICAL_NOTES)]
        public virtual string CriticalNotes { get; set; }

        [StringLength(Equipment.StringLengths.SERIAL_NUMBER)]
        public virtual string SerialNumber { get; set; }

        public virtual DateTime? DateInstalled { get; set; }
        public virtual bool PSMTCPA { get; set; }

        [StringLength(Equipment.StringLengths.LEGACY)]
        public virtual string Legacy { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual string SafetyNotes { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual string MaintenanceNotes { get; set; }

        [DataType(DataType.MultilineText)]
        public virtual string OperationNotes { get; set; }

        [RequiredWhen("IsReplacement", true, ErrorMessage = "Required when equipment is a replacement.")]
        public virtual int? SAPEquipmentIdBeingReplaced { get; set; }

        [View("Equipment Type"), DoesNotAutoMap("Used for display only, set in Map.")]
        public virtual EquipmentPurpose EquipmentPurposeObj { get; set; }

        [View("Equipment Type"), DoesNotAutoMap("Used for display only, set in Map.")]
        public virtual EquipmentType EquipmentTypeObj { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ScadaTagName))]
        public virtual int? ScadaTagName { get; set; }

        public virtual decimal? ArcFlashHierarchy { get; set; }

        [StringLength(Equipment.StringLengths.ARC_FLASH_RATING)]
        public virtual string ArcFlashRating { get; set; }

        public virtual bool? IsReplacement { get; set; }

        [CheckBox]
        public virtual bool? Portable { get; set; }

        // Not an actual view property, set by MapToEntity and used by controller. 
        [DoesNotAutoMap]
        public bool SendToSAP { get; set; }

        [View("Equipment Manufacturer")]
        [EntityMustExist(typeof(EquipmentManufacturer)), EntityMap]
        public virtual int? EquipmentManufacturer { get; set; }

        [StringLength(Equipment.StringLengths.MANUFACTURER_OTHER)]
        public virtual string ManufacturerOther { get; set; }

        [DoesNotAutoMap]
        public virtual int? Department { get; set; }
        public virtual bool HasOpenLockoutForms { get; set; }

        [DoesNotAutoMap]
        public string EquipmentTypeIdsWithRedTagPermitEligibility { get; set; }

        #endregion

        #region Constructors

        public EquipmentViewModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods
        
        protected void SetNumber(Equipment entity)
        {
            // Generate Number/Identifier
            var facilityRepo = _container.GetInstance<IRepository<Facility>>();
            var facility = facilityRepo.Find(Facility.Value);
            if (EquipmentPurpose.HasValue)
            {
                var equipmentPurpose =
                    _container.GetInstance<IRepository<EquipmentPurpose>>().Find(EquipmentPurpose.Value);
                var number = facilityRepo.GetNextEquipmentNumberForFacilityByEquipmentPurposeId(Facility.Value, EquipmentPurpose.Value);
                
                entity.Number = number;
            }
        }

        protected void SetEquipmentToPendingRetirement(int replacedEquipmenId, Equipment entity)
        {
            var equipmentRepository = _container.GetInstance<IRepository<Equipment>>();
            var equipment = equipmentRepository.Find(replacedEquipmenId);
            var pendingRetirement = _container.GetInstance<IRepository<EquipmentStatus>>().Find(MapCall.Common.Model.Entities.EquipmentStatus.Indices.PENDING_RETIREMENT);
            equipment.EquipmentStatus = pendingRetirement;
            equipmentRepository.Save(equipment);
            //TODO: Once this gets less hacky, fix here?
            //equipment.RecordUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
            //                   Url.Action("Show", "Equipment", new { id = equipment.Id });

            _container.GetInstance<INotificationService>().Notify(new NotifierArgs
            {
                OperatingCenterId = equipment.Facility.OperatingCenter.Id,
                Module = EquipmentController.ROLE,
                Purpose = EquipmentController.PENDING_RETIREMENT_NOTIFICATION,
                Data = equipment
            });
        }

        private IEnumerable<ValidationResult> ValidateSAPEquipmentIdUniqueness()
        {
            if (SAPEquipmentId != null)
            {
                if (_container.GetInstance<IRepository<Equipment>>().Any(x => x.SAPEquipmentId == SAPEquipmentId && x.Id != Id))
                    yield return new ValidationResult(SAP_EQUIPMENTID_ALREADY_IN_USE, new[] { nameof(SAPEquipmentId) });
            }
        }

        private EquipmentFailureRiskRating CalculateFailureRiskRating()
        {
            var facility = _container.GetInstance<IRepository<Facility>>().Find(Facility.Value);

            if (facility.StrategyTier == null)
            {
                return null;
            }

            return new EquipmentFailureRiskRating
            {
                Id = EquipmentRiskOfFailureCalculator.CalculateRisk(
                    facility.StrategyTier.Id, LikelyhoodOfFailure.Value, ConsequenceOfFailure.Value)
            };
        }

        private void CalculateRiskCharacteristics(Equipment entity)
        {
            if (Condition != null && Performance != null && StaticDynamicType != null)
            {
                LikelyhoodOfFailure = (StaticDynamicType == EquipmentStaticDynamicType.Indices.STATIC)
                    ? Equipment.StaticLikelyhoodOfFailureMatrix[Condition.Value - 1, Performance.Value - 1]
                    : Equipment.DynamicLikelyhoodOfFailureMatrix[Condition.Value - 1, Performance.Value - 1];
                Reliability = (StaticDynamicType == EquipmentStaticDynamicType.Indices.STATIC)
                    ? Equipment.StaticEquipmentReliabilityMatrix[Condition.Value - 1, Performance.Value - 1]
                    : Equipment.DynamicEquipmentReliabilityMatrix[Condition.Value - 1, Performance.Value - 1];
                LocalizedRiskOfFailure = LikelyhoodOfFailure * ConsequenceOfFailure;
            }

            if (ConsequenceOfFailure != null && LikelyhoodOfFailure != null && Facility != null)
            {
                entity.RiskOfFailure = CalculateFailureRiskRating();
            }
        }

        private void MapPrerequisites(Equipment entity)
        {
            var prerequisitesRepo = _container.GetInstance<IRepository<ProductionPrerequisite>>();
            var equipmentTypeRepo = _container.GetInstance<IRepository<EquipmentType>>();
            entity.ProductionPrerequisites.Clear();

            if (Prerequisites == null)
            {
                Prerequisites = new int[] { };
            }

            foreach (var prerequisite in Prerequisites)
            {
                entity.ProductionPrerequisites.Add(prerequisitesRepo.Find(prerequisite));
            }

            if (!Prerequisites.Contains(ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT) &&
                EquipmentType.HasValue &&
                equipmentTypeRepo.Find(EquipmentType.Value)?.IsLockoutRequired == true)
            {
                entity.ProductionPrerequisites.Add(prerequisitesRepo.Find(ProductionPrerequisite.Indices.HAS_LOCKOUT_REQUIREMENT));
            }

            if (!Prerequisites.Contains(ProductionPrerequisite.Indices.RED_TAG_PERMIT) &&
                EquipmentType.HasValue &&
                equipmentTypeRepo.Find(EquipmentType.Value)?.IsEligibleForRedTagPermit == true)
            {
                entity.ProductionPrerequisites.Add(prerequisitesRepo.Find(ProductionPrerequisite.Indices.RED_TAG_PERMIT));
            }
        }
        
        private void SetLastUpdatedIfRiskCharacteristicsHaveBeenModified(Equipment entity)
        {
            var conditionChanged = entity.Condition?.Id != Condition;
            var performanceChanged = entity.Performance?.Id != Performance;
            var reliabilityChanged = entity.Reliability?.Id != Reliability;
            var consequenceOfFailureChanged = entity.ConsequenceOfFailure?.Id != ConsequenceOfFailure;
            var likelyhoodOfFailureChanged = entity.LikelyhoodOfFailure?.Id != LikelyhoodOfFailure;
            var staticDynamicTypeChanged = entity.StaticDynamicType?.Id != StaticDynamicType;

            if (conditionChanged || performanceChanged || reliabilityChanged || consequenceOfFailureChanged ||
                likelyhoodOfFailureChanged || staticDynamicTypeChanged)
            {
                entity.RiskCharacteristicsLastUpdatedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                entity.RiskCharacteristicsLastUpdatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            }
        }

        #endregion

        #region Exposed Methods

        public override void Map(Equipment entity)
        {
            base.Map(entity);
            PlanningPlant = entity.Facility?.PlanningPlant?.Id;
            Department = entity.Facility?.Department?.Id;
            EquipmentPurposeObj = entity.EquipmentPurpose;
            EquipmentTypeObj = entity.EquipmentType;
            EquipmentGroup = entity.EquipmentGroup; 
            //TODO: Shouldn't this be automatic? nHibernate? fluentNHibernate? anyone?
            Prerequisites = entity.ProductionPrerequisites?.Select(x => x.Id).ToArray() ?? new int[] { };
        }

        public override Equipment MapToEntity(Equipment entity)
        {
            if (entity.AssetControlSignOffBy == null && AssetControlSignOffBy.HasValue && !AssetControlSignOffDate.HasValue)
            {
                AssetControlSignOffDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }

            //Reset the Date Retired if the Equipment Status is other than retired.
            if ((EquipmentStatus != MapCall.Common.Model.Entities.EquipmentStatus.Indices.RETIRED) && (EquipmentStatus != MapCall.Common.Model.Entities.EquipmentStatus.Indices.PENDING_RETIREMENT))
            {
                DateRetired = null;
            }

            if (entity.OperatingCenter != null || OperatingCenter.HasValue)
            {
                var operatingCenter = entity.OperatingCenter ?? _container.GetInstance<IRepository<OperatingCenter>>().Find(OperatingCenter.Value);
                SendToSAP = operatingCenter.CanSyncWithSAP;
                var equipmentTypeId = entity.EquipmentType?.Id ?? EquipmentType;
                var facility = entity.Facility ?? _container.GetInstance<IRepository<Facility>>().Find(Facility.Value);
                if (equipmentTypeId.HasValue && !MapCall.Common.Model.Entities.EquipmentType.SyncronizedEquipmentTypes.Contains(equipmentTypeId.Value))
                    SendToSAP = false;
                if (facility?.Department != null && !MapCall.Common.Model.Entities.Department.SAP_DEPARTMENTS.Contains(facility.Department.Id))
                    SendToSAP = false;
                if (facility?.Department == null)
                    SendToSAP = false;
            }

            MapPrerequisites(entity);
            CalculateRiskCharacteristics(entity);
            SetLastUpdatedIfRiskCharacteristicsHaveBeenModified(entity);

            entity = base.MapToEntity(entity);
            entity.FunctionalLocation = entity.Facility?.FunctionalLocation;

            // MC-1502 need to remove the value on ManufacturerOther unless the manufacturer is actually "OTHER"
            if (entity.EquipmentManufacturer?.MapCallDescription != MapCall.Common.Model.Entities.EquipmentManufacturer.MAPCALLDESCRIPTION_OTHER)
            {
                entity.ManufacturerOther = null;
            }
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateSAPEquipmentIdUniqueness());
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            var redTagPermitEquipmentPurposes = _container.GetInstance<IRepository<EquipmentType>>()
                                                          .Where(x => x.IsEligibleForRedTagPermit)
                                                          .ToArray();

            EquipmentTypeIdsWithRedTagPermitEligibility = string.Join(",",
                redTagPermitEquipmentPurposes.Where(x => x.IsEligibleForRedTagPermit).Select(x => x.Id));
        }

        #endregion

        #region Risk Characteristics

        [DropDown]
        [EntityMap, EntityMustExist(typeof(EquipmentCondition))]
        public virtual int? Condition { get; set; }

        [DropDown]
        [EntityMap, EntityMustExist(typeof(EquipmentPerformanceRating))]
        public virtual int? Performance { get; set; }

        [DropDown]
        [EntityMap, EntityMustExist(typeof(EquipmentStaticDynamicType))]
        public virtual int? StaticDynamicType { get; set; }

        [DropDown]
        [EntityMap, EntityMustExist(typeof(EquipmentConsequencesOfFailureRating))]
        public virtual int? ConsequenceOfFailure { get; set; }

        [EntityMap, EntityMustExist(typeof(EquipmentLikelyhoodOfFailureRating))]
        public virtual int? LikelyhoodOfFailure { get; set; }

        [EntityMap, EntityMustExist(typeof(EquipmentReliabilityRating))]
        public virtual int? Reliability { get; set; }

        public virtual int? LocalizedRiskOfFailure { get; set; }

        #endregion
    }

    public static class EquipmentRiskOfFailureCalculator
    {
        #region Exposed Methods

        public static int CalculateRisk(int strategyTier, int likelihood, int consequence)
        {
            switch (strategyTier)
            {
                case FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_1:
                    var thresholds = likelihood == EquipmentLikelyhoodOfFailureRating.Indices.LOW
                        ? (Low: EquipmentFailureRiskRating.Indices.LOW, High: EquipmentFailureRiskRating.Indices.MEDIUM)
                        : (Low: EquipmentFailureRiskRating.Indices.MEDIUM, High: EquipmentFailureRiskRating.Indices.HIGH);
                    return consequence == EquipmentConsequencesOfFailureRating.Indices.LOW
                        ? thresholds.Low : thresholds.High;
                case FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_2:
                    switch (likelihood)
                    {
                        case EquipmentLikelyhoodOfFailureRating.Indices.LOW:
                            return consequence == EquipmentConsequencesOfFailureRating.Indices.HIGH
                                ? EquipmentFailureRiskRating.Indices.MEDIUM
                                : EquipmentFailureRiskRating.Indices.LOW;
                        case EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM:
                            return consequence == EquipmentConsequencesOfFailureRating.Indices.LOW
                                ? EquipmentFailureRiskRating.Indices.LOW
                                : EquipmentFailureRiskRating.Indices.MEDIUM;
                        case EquipmentLikelyhoodOfFailureRating.Indices.HIGH:
                            return consequence == EquipmentConsequencesOfFailureRating.Indices.HIGH
                                ? EquipmentFailureRiskRating.Indices.HIGH
                                : EquipmentFailureRiskRating.Indices.MEDIUM;
                    }
                    break;
                case FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_3:
                    return likelihood == EquipmentLikelyhoodOfFailureRating.Indices.HIGH &&
                        consequence == EquipmentConsequencesOfFailureRating.Indices.HIGH
                        ? EquipmentFailureRiskRating.Indices.MEDIUM
                        : EquipmentFailureRiskRating.Indices.LOW;
            }

            throw new ArgumentException($"{strategyTier} is not a valid value.", nameof(strategyTier));
        }

        #endregion
    }

    public class AddEquipmentSensor : ViewModel<Equipment>
    {
        #region Properties

        [DoesNotAutoMap("This is manually added to a collection")]
        [Required, ComboBox, EntityMustExist(typeof(Sensor))]
        public int? Sensor { get; set; }

        #endregion

        #region Constructors

        public AddEquipmentSensor(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateSensor()
        {
            var sensor = _container.GetInstance<ISensorRepository>().Find(Sensor.GetValueOrDefault());
            if (sensor == null)
            {
                // Handled by Required attribute.
                yield break;
            }

            if (sensor.Equipment != null)
            {
                yield return new ValidationResult("Sensor is already attached to a piece of equipment(" + sensor.Equipment.Equipment + ").", new[] { "Sensor" });
            }
        }

        #endregion

        #region Exposed Methods

        public override Equipment MapToEntity(Equipment entity)
        {
            var sens = _container.GetInstance<ISensorRepository>().Find(Sensor.GetValueOrDefault());
            entity.Sensors.Add(new EquipmentSensor
            {
                Equipment = entity,
                Sensor = sens
            });
            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // TODO: Validate the sensor is not already attached to a piece of equipment.
            return base.Validate(validationContext).Concat(ValidateSensor());
        }

        #endregion
    }

    public class RemoveEquipmentSensor : ViewModel<Equipment>
    {
        #region Properties

        [DoesNotAutoMap("This is used to remove a sensor from a collection.")]
        [Required, EntityMustExist(typeof(Sensor))]
        public int? Sensor { get; set; }

        #endregion

        #region Constructors

        public RemoveEquipmentSensor(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Equipment MapToEntity(Equipment entity)
        {
            var matching = entity.Sensors.Where(x => x.Sensor.Id == Sensor.GetValueOrDefault()).ToArray();
            foreach (var match in matching)
            {
                entity.Sensors.Remove(match);
            }
            return entity;
        }

        #endregion
    }

    public class SearchEquipmentReadings : IValidatableObject
    {
        #region Private Members

        #region Fields

        private readonly IEquipmentRepository _equipmentRepository;

        #endregion

        #endregion

        #region Properties

        [Required, EntityMustExist(typeof(Equipment))]
        public int? Id { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        [Required, CompareTo("EndDate", ComparisonType.LessThanOrEqualTo, TypeCode.DateTime)]
        public DateTime? StartDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        [Required, CompareTo("StartDate", ComparisonType.GreaterThanOrEqualTo, TypeCode.DateTime)]
        public DateTime? EndDate { get; set; }

        // Not a posted value.
        public IEnumerable<Reading> Readings { get; set; }

        // NOTE: Required does not work with client-side validation when using CheckBoxLists. -Ross 8/28/2019
        [CheckBoxList, Required(ErrorMessage = "You must select at least one sensor.")]
        public int[] Sensors { get; set; }

        [DropDown]
        public ReadingGroupType Interval { get; set; }

        #endregion

        #region Constructors

        public SearchEquipmentReadings(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        #endregion

        #region Exposed Methods

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Handled by Required validator
            if (Sensors == null || !Sensors.Any())
            {
                yield break;
            }

            var equip = _equipmentRepository.Find(Id.GetValueOrDefault());
            // Handled by Required validator.
            if (equip == null)
            {
                yield break;
            }

            var mismatchedSensors = Sensors.Except(equip.Sensors.Select(x => x.Sensor.Id));
            if (mismatchedSensors.Any())
            {
                // This should never come up unless we start injecting the wrong sensors into the checkboxlist.
                yield return new ValidationResult("At least one sensor does not belong to this equipment.");
            }
        }

        #endregion
    }

    public class SearchEquipmentScadaReadings
    {
        #region Properties

        public int Id { get; set; }

        // Not a posted value.
        public IEnumerable<RawData> Readings { get; set; }

        // also Not a posted value.
        public ScadaTagName TagName { get; set; }

        [Display(Name = "Use Raw?")]
        public bool UseRaw { get; set; }

        [DateTimePicker]
        public DateTime? StartDate { get; set; }

        [DateTimePicker]
        public DateTime? EndDate { get; set; }

        #endregion

        #region Exposed Methods

        public object ToRouteValuesForExcel()
        {
            return new
            {
                ext = ResponseFormatter.KnownExtensions.EXCEL_2003,
                TagName,
                UseRaw,
                StartDate,
                EndDate
            };
        }

        #endregion
    }

    public class AddEquipmentLink : ViewModel<Equipment>
    {
        #region Properties

        [Required]
        [DataAnnotationsExtensions.Url]
        [StringLength(EquipmentLink.StringLengths.PAYMENT_METHOD_URL)]
        [DoesNotAutoMap("Not a property on Equipment")]
        public string Url { get; set; }

        [Required, DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(LinkType))]
        public int? LinkType { get; set; }

        #endregion

        #region Constructors

        public AddEquipmentLink(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Equipment MapToEntity(Equipment entity)
        {
            entity.Links.Add(new EquipmentLink {
                Equipment = entity,
                Url = Url,
                // this id has already been validated when the model was bound and passed to the controller
                LinkType = new LinkType {Id = LinkType.Value}
            });
            return entity;
        }

        #endregion
    }

    public class RemoveEquipmentLink : ViewModel<Equipment>
    {
        #region Properties

        [Required, EntityMustExist(typeof(EquipmentLink))]
        [DoesNotAutoMap("Not a property on Equipment")]
        public int? EquipmentLink { get; set; }

        #endregion

        #region Constructors

        public RemoveEquipmentLink(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override Equipment MapToEntity(Equipment entity)
        {
            var repo = _container.GetInstance<IRepository<EquipmentLink>>();
            var link = repo.Find(EquipmentLink.Value);
            entity.Links.Remove(link);
            return entity;
        }

        #endregion
    }
}
