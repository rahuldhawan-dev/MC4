using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Utilities.Excel;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Equipment
        : IEntityWithCreationTracking<User>,
            IEntityWithUpdateTimeTracking,
            IAsset,
            IThingWithDocuments,
            IThingWithNotes,
            IThingWithVideos,
            ISAPEquipment,
            IRetirableWorkOrderAsset
    {
        #region Constants

        public struct StringLengths
        {
            public const int DESCRIPTION = CreateEquipmentEquipmentTypeForBug1442.StringLengths.EQUIPMENT_DESCRIPTION,
                             SERIAL_NUMBER = CreateEquipmentEquipmentTypeForBug1442.StringLengths.SERIAL_NUMBER,
                             IDENTIFIER = CreateEquipmentEquipmentTypeForBug1442.StringLengths.IDENTIFIER,
                             CRITICAL_NOTES = 150,
                             FUNCTIONAL_LOCATION = 30,
                             ARC_FLASH_RATING = 255,
                             MANUFACTURER_OTHER = 50,
                             WBS_NUMBER = 18,
                             LEGACY = 50,
                             OTHER_COMPLIANCE_REASON = 255,
                             EXTENDED_USEFUL_LIFE_COMMENT = 255;
        }

        public const string EQUIPMENT_IDENTIFIER_PATTERN = @"(\w+-\d+)-(\w+)-(\d+)",
                            EQUIPMENT_IDENTIFIER_FORMAT_STRING = "{0}-{1}-{2}";

        public const int T_AND_D_DEPARTMENT_ID = 1, PRODUCTION_DEPARTMENT_ID = 3;

        public const string REPLACEMENT_PROD_WORK_ORDER_ID_LABEL = "Replacement Prod Work Order ID";

        public struct Risks
        {
            public const string LOW = "Low", MEDIUM = "Medium", HIGH = "High";
        }

        public struct DisplayNames
        {
            public const string HAS_COMPLIANCE_REQUIREMENT = "Compliance Flags",
                                NUMBER = "Equipment Number",
                                LEGACY = "Legacy - Drawing ID",
                                HAS_PROCESS_SAFETY_MANAGEMENT = "Process Safety Management",
                                HAS_COMPANY_REQUIREMENT = "Company Requirement",
                                HAS_REGULATORY_REQUIREMENT = "Environmental / Water Quality Regulatory Requirement",
                                HAS_OSHA_REQUIREMENT = "OSHA Requirement",
                                OTHER_COMPLIANCE = "Other",
                                OTHER_COMPLIANCE_REASON = "Other Compliance Reason",
                                CONDITION = "Equipment Condition",
                                PERFORMANCE = "Equipment Performance",
                                STATIC_DYNAMIC_TYPE = "Equipment [Static/Dynamic]",
                                CONSEQUENCE_OF_FAILURE = "Equipment COF",
                                LIKELYHOOD_OF_FAILURE = "Equipment LOF",
                                RELIABILITY = "Equipment Reliability",
                                RISK_OF_FAILURE = "Equipment Risk of Failure Rating - System Level",
                                LOCALIZED_RISK_OF_FAILURE = "Equipment Risk of Failure Score - Facility Level",
                                LOCALIZED_RISK_OF_FAILURE_TEXT = "Equipment Risk - Facility Level",
                                FUNCTIONAL_LOCATION = "Functional Location",
                                ABC_INDICATOR = "Equipment Criticality",
                                SAP_EQUIPMENT_MANUFACTURER = "Manufacturer",
                                HAS_AT_LEAST_ONE_WELL_TEST = "Has Well Tests",
                                IDENTIFIER = "MapCall EquipmentID",
                                REMAINING_USEFUL_LIFE = "Remaining Useful Life (Years)",
                                EXTENDED_REMAINING_USEFUL_LIFE = "Extended Remaining Useful Life (Years)",
                                SERVICE_LIFE = "Service Life (Years)",
                                ESTIMATED_REPLACE_COST = "Estimated Replacement Cost ($)",
                                EXTENDED_USEFUL_LIFE_WORK_ORDER_ID = "Extended Useful Life WO #",
                                RISK_CHARACTERISTICS_LAST_UPDATED_BY = "Last Updated By",
                                RISK_CHARACTERISTICS_LAST_UPDATED_ON = "Last Updated On";
        }

        #endregion

        #region Private Members

        private EquipmentDisplayItem _display;

        [NonSerialized] 
        private IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; } //=> Facility?.OperatingCenter;

        public virtual Coordinate Coordinate { get; set; }
        public virtual MapIcon Icon => Coordinate?.Icon;

        public virtual IEnumerable<IVideoLink> LinkedVideos => Videos;
        public virtual IList<EquipmentVideo> Videos { get; set; }

        [Required, StringLength(StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        [View(DisplayNames.NUMBER)]
        public virtual int? Number { get; set; }

        public virtual int? CriticalRating { get; set; }
        public virtual string CriticalNotes { get; set; }

        [StringLength(StringLengths.SERIAL_NUMBER)]
        public virtual string SerialNumber { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateInstalled { get; set; }

        public virtual bool? PSMTCPA { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string SafetyNotes { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string MaintenanceNotes { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string OperationNotes { get; set; }

        public virtual string WBSNumber { get; set; }
        public virtual int? SAPEquipmentId { get; set; }
        public virtual int? SAPEquipmentIdBeingReplaced { get; set; }

        public virtual bool HasSensorAttached { get; set; }
        public virtual bool HasNoSAPEquipmentId { get; set; }
        public virtual bool IsSignedOffByAssetControl { get; set; }
        public virtual bool IsReplacement { get; set; }

        public virtual ProductionWorkOrder ReplacementProductionWorkOrder { get; set; }

        public virtual bool Portable { get; set; }

        public virtual decimal? ArcFlashHierarchy { get; set; }
        public virtual string ArcFlashRating { get; set; }

        public virtual DateTime CreatedAt { get; set; }
        public virtual DateTime? AssetControlSignOffDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateRetired { get; set; }

        public virtual DateTime UpdatedAt { get; set; }
        public virtual string SAPErrorCode { get; set; }

        //Added to fix Production Equipment issue
        public virtual Equipment ParentEquipment { get; set; }

        [View(DisplayNames.LEGACY)]
        public virtual string Legacy { get; set; }

        [View(DisplayNames.HAS_PROCESS_SAFETY_MANAGEMENT)]
        public virtual bool HasProcessSafetyManagement { get; set; }

        [View(DisplayNames.HAS_COMPANY_REQUIREMENT)]
        public virtual bool HasCompanyRequirement { get; set; }

        [View(DisplayNames.HAS_REGULATORY_REQUIREMENT)]
        public virtual bool HasRegulatoryRequirement { get; set; }

        [View(DisplayNames.HAS_OSHA_REQUIREMENT)]
        public virtual bool HasOshaRequirement { get; set; }

        [View(DisplayNames.OTHER_COMPLIANCE)]
        public virtual bool OtherCompliance { get; set; }

        [View(DisplayNames.OTHER_COMPLIANCE_REASON), StringLength(255)]
        public virtual string OtherComplianceReason { get; set; }

        [View(DisplayNames.HAS_COMPLIANCE_REQUIREMENT)]
        public virtual bool HasComplianceRequirement { get; set; }

        public virtual int? PlannedReplacementYear { get; set; }

        [View(DisplayNames.ESTIMATED_REPLACE_COST, FormatStyle.DecimalMaxTwoDecimalPlacesWithTrailingZeroes)]
        public virtual decimal? EstimatedReplaceCost { get; set; }

        [View(DisplayNames.EXTENDED_USEFUL_LIFE_WORK_ORDER_ID)]
        public virtual int? ExtendedUsefulLifeWorkOrderId { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? LifeExtendedOnDate { get; set; }

        [StringLength(StringLengths.EXTENDED_USEFUL_LIFE_COMMENT)]
        public virtual string ExtendedUsefulLifeComment { get; set; }

        [View(DisplayNames.SERVICE_LIFE)]
        public virtual int? ServiceLife { get; set; }

        #region Risk Characteristics

        [View(DisplayNames.CONDITION)]
        public virtual EquipmentCondition Condition { get; set; }

        [View(DisplayNames.PERFORMANCE)]
        public virtual EquipmentPerformanceRating Performance { get; set; }

        [View(DisplayNames.STATIC_DYNAMIC_TYPE)]
        public virtual EquipmentStaticDynamicType StaticDynamicType { get; set; }

        [View(DisplayNames.CONSEQUENCE_OF_FAILURE)]
        public virtual EquipmentConsequencesOfFailureRating ConsequenceOfFailure { get; set; }

        [View(DisplayNames.LIKELYHOOD_OF_FAILURE)]
        public virtual EquipmentLikelyhoodOfFailureRating LikelyhoodOfFailure { get; set; }

        public static int[,] StaticLikelyhoodOfFailureMatrix => new[,] {
            {
                EquipmentLikelyhoodOfFailureRating.Indices.HIGH, EquipmentLikelyhoodOfFailureRating.Indices.HIGH,
                EquipmentLikelyhoodOfFailureRating.Indices.HIGH
            },
            {
                EquipmentLikelyhoodOfFailureRating.Indices.HIGH, EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM,
                EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM
            },
            {
                EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, EquipmentLikelyhoodOfFailureRating.Indices.LOW,
                EquipmentLikelyhoodOfFailureRating.Indices.LOW
            }
        };

        public static int[,] DynamicLikelyhoodOfFailureMatrix => new[,] {
            {
                EquipmentLikelyhoodOfFailureRating.Indices.HIGH, EquipmentLikelyhoodOfFailureRating.Indices.HIGH,
                EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM
            },
            {
                EquipmentLikelyhoodOfFailureRating.Indices.HIGH, EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM,
                EquipmentLikelyhoodOfFailureRating.Indices.LOW
            },
            {
                EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, EquipmentLikelyhoodOfFailureRating.Indices.LOW,
                EquipmentLikelyhoodOfFailureRating.Indices.LOW
            }
        };

        [View(DisplayNames.RELIABILITY)]
        public virtual EquipmentReliabilityRating Reliability { get; set; }

        public static int[,] StaticEquipmentReliabilityMatrix => new[,] {
            {
                EquipmentLikelyhoodOfFailureRating.Indices.LOW, EquipmentLikelyhoodOfFailureRating.Indices.LOW,
                EquipmentLikelyhoodOfFailureRating.Indices.LOW
            },
            {
                EquipmentLikelyhoodOfFailureRating.Indices.LOW, EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM,
                EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM
            },
            {
                EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, EquipmentLikelyhoodOfFailureRating.Indices.HIGH,
                EquipmentLikelyhoodOfFailureRating.Indices.HIGH
            }
        };

        public static int[,] DynamicEquipmentReliabilityMatrix => new[,] {
            {
                EquipmentLikelyhoodOfFailureRating.Indices.LOW, EquipmentLikelyhoodOfFailureRating.Indices.LOW,
                EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM
            },
            {
                EquipmentLikelyhoodOfFailureRating.Indices.LOW, EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM,
                EquipmentLikelyhoodOfFailureRating.Indices.HIGH
            },
            {
                EquipmentLikelyhoodOfFailureRating.Indices.MEDIUM, EquipmentLikelyhoodOfFailureRating.Indices.HIGH,
                EquipmentLikelyhoodOfFailureRating.Indices.HIGH
            }
        };

        [View(DisplayNames.RISK_OF_FAILURE)]
        public virtual EquipmentFailureRiskRating RiskOfFailure { get; set; }

        [DoesNotExport]
        [View(DisplayNames.LOCALIZED_RISK_OF_FAILURE)]
        public virtual int? LocalizedRiskOfFailure { get; set; }

        [View(DisplayNames.LOCALIZED_RISK_OF_FAILURE_TEXT)]
        public virtual string LocalizedRiskOfFailureText
        {
            get
            {
                if (!LocalizedRiskOfFailure.HasValue)
                    return null;
                return (LocalizedRiskOfFailure.Value <= 2)
                    ? Risks.LOW
                    : (LocalizedRiskOfFailure.Value < 6)
                        ? Risks.MEDIUM
                        : Risks.HIGH;
            }
        }

        [View(DisplayNames.RISK_CHARACTERISTICS_LAST_UPDATED_ON)]
        public virtual DateTime? RiskCharacteristicsLastUpdatedOn { get; set; }

        #endregion

        #endregion

        #region References

        public virtual EquipmentGroup EquipmentGroup => EquipmentType?.EquipmentGroup;

        public virtual EquipmentType EquipmentType { get; set; }

        public virtual EquipmentPurpose EquipmentPurpose { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual FacilityFacilityArea FacilityFacilityArea { get; set; }

        public virtual EquipmentStatus EquipmentStatus { get; set; }

        public virtual EquipmentModel EquipmentModel { get; set; }

        [View(DisplayNames.FUNCTIONAL_LOCATION)]
        public virtual string FunctionalLocation { get; set; }

        [View(DisplayNames.ABC_INDICATOR)]
        public virtual ABCIndicator ABCIndicator { get; set; }

        [View(DisplayNames.SAP_EQUIPMENT_MANUFACTURER)]
        public virtual EquipmentManufacturer EquipmentManufacturer { get; set; }

        public virtual string ManufacturerOther { get; set; }

        public virtual ScadaTagName ScadaTagName { get; set; }

        public virtual Generator Generator { get; set; }
        public virtual Employee RequestedBy { get; set; }
        public virtual Employee AssetControlSignOffBy { get; set; }
        public virtual User CreatedBy { get; set; }

        public virtual Equipment ReplacedEquipment { get; set; }

        public virtual IList<EquipmentDocument> EquipmentDocuments { get; set; } = new List<EquipmentDocument>();
        public virtual IList<EquipmentNote> EquipmentNotes { get; set; } = new List<EquipmentNote>();
        public virtual ISet<FilterMedia> FilterMediae { get; set; } = new HashSet<FilterMedia>();
        public virtual IList<EquipmentCharacteristic> Characteristics { get; set; } = new List<EquipmentCharacteristic>();

        public virtual IList<EquipmentCharacteristic> ActiveCharacteristics =>
            Characteristics.Where(x => x.Field.IsActive).ToList();

        public virtual ISet<RedTagPermit> RedTagPermits { get; set; } = new HashSet<RedTagPermit>();
        public virtual IList<EquipmentSensor> Sensors { get; set; } = new List<EquipmentSensor>();
        public virtual IList<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();
        public virtual IList<TankInspection> TankInspections { get; set; } = new List<TankInspection>();
        public virtual IList<ProductionPrerequisite> ProductionPrerequisites { get; set; } = new List<ProductionPrerequisite>();
        public virtual IList<EnvironmentalPermit> EnvironmentalPermits { get; set; } = new List<EnvironmentalPermit>();
        public virtual ISet<EquipmentLink> Links { get; set; } = new HashSet<EquipmentLink>();
        public virtual ISet<AssetReliability> AssetReliabilities { get; set; } = new HashSet<AssetReliability>();
        public virtual IList<EquipmentMaintenancePlan> MaintenancePlans { get; set; } = new List<EquipmentMaintenancePlan>();

        public virtual ISet<WellTest> WellTests { get; set; } = new HashSet<WellTest>();
        
        public virtual User RiskCharacteristicsLastUpdatedBy { get; set; }

        #endregion

        #region Logical Properties

        [View(DisplayNames.HAS_AT_LEAST_ONE_WELL_TEST)]
        public virtual bool HasAtLeastOneWellTest => WellTests.Any();

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return EquipmentDocuments.Map(e => (IDocumentLink)e); }
        }

        public virtual IList<Document> Documents
        {
            get { return EquipmentDocuments.Map(e => e.Document); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return EquipmentNotes.Map(e => (INoteLink)e); }
        }

        public virtual IList<Note> Notes
        {
            get { return EquipmentNotes.Map(e => e.Note); }
        }

        [DoesNotExport]
        public virtual string TableName => EquipmentMap.TABLE_NAME;

        // TODO: This needs to be cleaned up. Holding off for the next equipment lifespan
        public virtual bool IsGenerator =>
            (EquipmentPurpose != null
             && EquipmentPurpose.EquipmentLifespan != null
             && EquipmentPurpose.EquipmentLifespan.Id == EquipmentLifespan.Indices.GENERATOR);

        /// <summary>
        /// A terrible terrible hack property for passing the url
        /// for this record to a notification template.
        /// </summary>
        public virtual string RecordUrl { get; set; }

        // NOTE: if this needs to change, Display on Equipment needs to change as well
        public virtual string Display => (_display ?? (_display = new EquipmentDisplayItem {
            Description = Description,
            Id = Id,
            Facility = Facility,
            EquipmentPurpose = EquipmentPurpose?.Abbreviation,
            EquipmentTypeDescription = EquipmentType?.Description,
            EquipmentTypeAbbreviation = EquipmentType?.Abbreviation,
            Status = EquipmentStatus?.Description
        })).Display;

        public virtual bool WorkOrderAccessible => Facility?.Department != null &&
                                                   (Facility.Department.Id == T_AND_D_DEPARTMENT_ID ||
                                                    (Facility.Department.Id == PRODUCTION_DEPARTMENT_ID &&
                                                     Facility.OperatingCenter.HasWorkOrderInvoicing));

        public virtual int? StandardOperatingProcedureDocumentId =>
            Documents
              ?.OrderBy(x => x.CreatedAt)
               .LastOrDefault(x =>
                    x.DocumentType?.Id == DocumentType.Indices.STANDARD_OPERATING_PROCEDURE)
              ?.Id;

        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual decimal? Longitude => Coordinate?.Longitude;

        public virtual bool SAPSync { get; set; }

        [DoesNotExport]
        public virtual bool CanBeCopied => EquipmentStatus.CanBeCopiedStatuses.Contains(EquipmentStatus.Id);

        public virtual IList<ProductionWorkOrder> ProductionWorkOrders =>
            ProductionWorkOrderEquipment.Select(x => x.ProductionWorkOrder).ToList();

        public virtual IList<ProductionWorkOrderEquipment> ProductionWorkOrderEquipment { get; set; } = new List<ProductionWorkOrderEquipment>();

        [View(DisplayNames.IDENTIFIER)]
        public virtual string Identifier => string.Format(Equipment.EQUIPMENT_IDENTIFIER_FORMAT_STRING,
            Facility?.FacilityId,
            EquipmentPurpose?.Abbreviation, Id);

        [View(DisplayNames.REMAINING_USEFUL_LIFE)]
        public virtual decimal? RemainingUsefulLife
        {
            get
            {
                if (EquipmentPurpose?.EquipmentLifespan?.EstimatedLifespan == null || !DateInstalled.HasValue)
                {
                    return null;
                }

                return EquipmentPurpose.EquipmentLifespan.EstimatedLifespan -
                       (_dateTimeProvider.GetCurrentDate().Year - DateInstalled.Value.Year);
            }
        }

        [View(DisplayNames.EXTENDED_REMAINING_USEFUL_LIFE)]
        public virtual decimal? ExtendedRemainingUsefulLife
        {
            get
            {
                if (ExtendedUsefulLifeWorkOrderId == null || EquipmentPurpose?.EquipmentLifespan?.ExtendedLifeMajor == null || RemainingUsefulLife == null)
                {
                    return null;
                }

                return EquipmentPurpose.EquipmentLifespan.ExtendedLifeMajor + RemainingUsefulLife;
            }
        }

        [SetterProperty]
        public virtual IDateTimeProvider DateTimeProvider
        {
            set => _dateTimeProvider = value;
        }

        public virtual bool HasOpenLockoutForms { get; set; }
        public virtual string EquipmentDescriptionWithRegionalPlanningArea => $"{Description} - {Facility.RegionalPlanningArea}";

        public virtual bool HasOpenRedTagPermits { get; set; }

        public virtual bool HasTankInspections => TankInspections.Any();

        [DoesNotExport]
        public virtual bool EquipmentIsTank => EquipmentType?.Abbreviation.StartsWith(EquipmentType.ComparisonValue.TNK) ?? false;

        #endregion

        #endregion

        #region Constructors

        public Equipment()
        {
            //EquipmentPurpose = new EquipmentPurpose();
            //Facility = new Facility();
            //ProductionWorkOrders = new List<ProductionWorkOrder>();
            EnvironmentalPermits = new List<EnvironmentalPermit>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext) =>
            Enumerable.Empty<ValidationResult>();

        public override string ToString() => Identifier;

        public virtual string GetCharacteristicValue(string characteristicName)
        {
            return Characteristics != null
                ? Characteristics.FirstOrDefault(x => x.Field?.FieldName == characteristicName)?.DisplayValue
                : string.Empty;
        }

        public virtual object ToJson()
        {
            return new {
                Description,
                EquipmentPurpose = EquipmentPurpose?.Description,
                Facility = Facility.FacilityName,
                FunctionalLocation,
                HasRegulatoryRequirement,
                HasSensorAttached,
                Id,
                IsGenerator,
                MapCallEquipmentId = Identifier,
                Number,
                OperatingCenter = OperatingCenter.Description,
                SAPEquipmentId,
                EquipmentType = EquipmentType?.Description,
                ScadaTagName = ScadaTagName?.TagName
            };
        }

        #endregion
    }

    [Serializable]
    public class EquipmentDisplayItem : DisplayItem<Equipment>
    {
        public string Identifier =>
            $"{string.Format(Equipment.EQUIPMENT_IDENTIFIER_FORMAT_STRING, Facility?.FacilityId, EquipmentPurpose, Id)}";

        public Facility Facility { get; set; }

        [SelectDynamic("Abbreviation", Field = "EquipmentPurpose")]
        public string EquipmentPurpose { get; set; }

        [SelectDynamic("Abbreviation", Field = "EquipmentType")]
        public string EquipmentTypeAbbreviation { get; set; }

        [SelectDynamic("Description", Field = "EquipmentType")]
        public string EquipmentTypeDescription { get; set; }

        public string Description { get; set; }

        [SelectDynamic("Description", Field = "EquipmentStatus")]
        public string Status { get; set; }

        public virtual string EquipmentDescriptionWithRegionalPlanningArea => $"{Description} - {Facility.RegionalPlanningArea}";

        public override string Display =>
            $"{Identifier} {Facility?.FacilityName} - {(EquipmentTypeAbbreviation == null ? "" : $"{EquipmentTypeAbbreviation} - {EquipmentTypeDescription} - ")}{Description} - {Status}";
    }
}