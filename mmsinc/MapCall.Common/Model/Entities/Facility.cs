using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.ChangeTracking;
using System.Text.RegularExpressions;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Facility
        : IEntityLookup,
            IThingWithDocuments,
            IThingWithNotes,
            IThingWithCoordinate,
            IThingWithVideos,
            IEntityWithCreationTracking<User>,
            IEntityWithUpdateTimeTracking
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int ARC_FLASH_CONTRACTOR = 50,
                             COMMUNITY_RIGHT_TO_KNOW_FACILITY_ID = 50,
                             COMU = 50,
                             CONFINED_SPACE_REQUIREMENT = 50,
                             ENVIRONMENTAL_REGULATOR_ID_NUMBER = 25,
                             ELECTRICAL_ACCOUNT_NUMBER = 14,
                             FACILITY_CONTACT_INFO = 500,
                             FACILITY_EMERGENCY_CONTACT_EMERGENCY_PHONE_NUMBER = 20,
                             FACILITY_EMERGENCY_CONTACT_NAME = 255,
                             FACILITY_EMERGENCY_CONTACT_PHONE_NUMBER = 20,
                             FACILITY_EMERGENCY_CONTACT_TITLE = 255,
                             FACILITY_ID = 25,
                             FACILITY_INSPECTION_FREQUENCY = 10,
                             FACILITY_LOOP_GROUPING = 25,
                             FACILITY_LOOP_GROUPING_SUB = 25,
                             FACILITY_NAME = 50,
                             FUNCTIONAL_LOCATION = 30,
                             DESIGNATION_TREATMENT_PLANT = 25,
                             DESIGNATION_PUMP_STATION = 25,
                             INSURANCE_ID = 20,
                             NORTH_AMERICAN_INDUSTRY_CLASSIFICATION_SYSTEM = 50,
                             NOTES = 255,
                             OPERATIONS = 50,
                             PRIMARY_FUSE_TYPE = 50,
                             PRIMARY_FUSE_MANUFACTURER = 100,
                             RD_EXEMPTION_APPROVAL_NUMBER = 50,
                             REGIONAL_PLANNING_AREA = 50,
                             RISK_MANAGEMENT_PLAN_FACILITY_ID = 50,
                             SECURITY_CATEGORY = 25,
                             SECURITY_GROUPING = 25,
                             SECURITY_INSPECTION_FREQUENCY = 25,
                             SECURITY_LOOP_SEQUENCE = 25,
                             STREET_NUMBER = 10,
                             SYSTEM = 50,
                             TOXINS_RELEASE_INVENTORY_FACILITY_ID = 50,
                             WATER_SHED = 50,
                             YEAR_IN_SERVICE = 10,
                             ZIP_CODE = 12,
                             VAMP_URL = 2000;

            #endregion
        }

        public const string ADDRESS_FORMAT = "{0} {1} {2} {3} {4}, {5} {6}";
        public const string FACILITY_ID_FORMAT = "{0}-{1}";

        public struct DisplayNames
        {
            public const string BASIC_GROUND_WATER_SUPPLY = "Ground Water Supply - GWUDI",
                                INSURANCE_ID = "Insurance ID",
                                INSURANCE_SCORE = "Insurance Score",
                                INSURANCE_SCORE_QUARTILE = "Insurance Score Quartile",
                                INSURANCE_LAST_VISIT_DATE = "Insurance Last Visit Date";
        }

        #endregion

        #region Private Members

        private FacilityDisplayItem _display;

        #endregion

        #region Properties

        #region Table Properties

        #region Normal

        public virtual int Id { get; set; }

        public virtual DateTime CreatedAt { get; set; }

        [StringLength(StringLengths.OPERATIONS)]
        public virtual string Operations { get; set; }

        [StringLength(StringLengths.SYSTEM)]
        public virtual string System { get; set; }

        /// <summary>
        /// DANGER WILL ROBINSON: 
        /// This is not the Identifier, this is the code they refer to facilities with.
        /// E.g. NJSB-39
        /// </summary>
        public virtual string FacilityId => string.Format(FACILITY_ID_FORMAT, OperatingCenter.OperatingCenterCode, Id);

        [StringLength(StringLengths.FACILITY_NAME)]
        public virtual string FacilityName { get; set; }

        [StringLength(StringLengths.STREET_NUMBER)]
        public virtual string StreetNumber { get; set; }

        public virtual Street Street { get; set; }
        public virtual Street NearestCrossStreet { get; set; }

        public virtual Town Town { get; set; }

        [StringLength(StringLengths.ZIP_CODE)]
        public virtual string ZipCode { get; set; }
        [View("Facility Total Capacity (MGD)")]
        public virtual decimal FacilityTotalCapacityMGD { get; set; }

        public virtual int CriticalRating { get; set; }

        [StringLength(StringLengths.YEAR_IN_SERVICE)]
        public virtual string YearInService { get; set; }

        public virtual int SICNumber { get; set; }

        [StringLength(StringLengths.ENVIRONMENTAL_REGULATOR_ID_NUMBER)]
        public virtual string EnvironmentalRegulatorIDNumber { get; set; }

        [StringLength(StringLengths.WATER_SHED)]
        public virtual string WaterShed { get; set; }

        [StringLength(StringLengths.REGIONAL_PLANNING_AREA)]
        [View("LEGACY ID - SD")]
        public virtual string RegionalPlanningArea { get; set; }

        public virtual bool PropertyOnly { get; set; }
        public virtual bool Administration { get; set; }
        public virtual bool EmergencyPower { get; set; }
        public virtual bool GroundWaterSupply { get; set; }
        public virtual bool SurfaceWaterSupply { get; set; }
        public virtual bool Reservoir { get; set; }
        public virtual bool Dam { get; set; }
        public virtual bool Interconnection { get; set; }
        public virtual bool PointOfEntry { get; set; }
        public virtual bool WaterTreatmentFacility { get; set; }
        public virtual bool ChemicalFeed { get; set; }
        public virtual ChemicalStorageLocation ChemicalStorageLocation { get; set; }
        public virtual bool DPCC { get; set; }
        public virtual bool PSM { get; set; }
        public virtual bool Filtration { get; set; }
        public virtual bool ResidualsGeneration { get; set; }
        public virtual bool TReport { get; set; }
        public virtual bool DistributivePumping { get; set; }
        public virtual bool BoosterStation { get; set; }
        public virtual bool PressureReducing { get; set; }
        public virtual bool GroundStorage { get; set; }
        public virtual bool ElevatedStorage { get; set; }
        public virtual bool OnSiteAnalyticalInstruments { get; set; }
        public virtual bool SampleStation { get; set; }
        public virtual bool SewerLiftStation { get; set; }
        public virtual bool WasteWaterTreatmentFacility { get; set; }
        public virtual bool FieldOperations { get; set; }
        public virtual bool SpoilsStaging { get; set; }
        public virtual bool? HasConfinedSpaceRequirement { get; set; }
        public virtual bool CellularAntenna { get; set; }
        public virtual bool Radionuclides { get; set; }
        public virtual bool CommunityRightToKnow { get; set; }

        [View(DisplayNames.BASIC_GROUND_WATER_SUPPLY)]
        public virtual bool BasicGroundWaterSupply { get; set; }

        public virtual bool RawWaterPumpStation { get; set; }

        [StringLength(StringLengths.DESIGNATION_TREATMENT_PLANT)]
        public virtual string DesignationTreatmentPlant { get; set; }

        [StringLength(StringLengths.DESIGNATION_PUMP_STATION)]
        public virtual string DesignationPumpStation { get; set; }
        [View("Facility Reliable Capacity (MGD)")]
        public virtual decimal FacilityReliableCapacityMGD { get; set; }
        [View("Facility Operating Capacity (MGD)")]
        public virtual decimal FacilityOperatingCapacityMGD { get; set; }
        [View("Facility Rated Capacity (MGD)")]
        public virtual decimal FacilityRatedCapacityMGD { get; set; }
        [View("Facility Aux Power Capacity (MGD)")]
        public virtual decimal FacilityAuxPowerCapacityMGD { get; set; }
        [View("Facility Used in Production (Treatment) Capacity Calculation")]
        public virtual bool? UsedInProductionCapacityCalculation { get; set; }

        [StringLength(StringLengths.FACILITY_INSPECTION_FREQUENCY)]
        public virtual string FacilityInspectionFrequency { get; set; }

        [StringLength(StringLengths.FACILITY_LOOP_GROUPING)]
        public virtual string FacilityLoopGrouping { get; set; }

        [StringLength(StringLengths.FACILITY_LOOP_GROUPING_SUB)]
        public virtual string FacilityLoopGroupingSub { get; set; }

        public virtual int FacilityLoopSequence { get; set; }

        [StringLength(StringLengths.SECURITY_CATEGORY)]
        public virtual string SecurityCategory { get; set; }

        [StringLength(StringLengths.SECURITY_GROUPING)]
        public virtual string SecurityGrouping { get; set; }

        [StringLength(StringLengths.SECURITY_INSPECTION_FREQUENCY)]
        public virtual string SecurityInspectionFrequency { get; set; }

        [StringLength(StringLengths.SECURITY_LOOP_SEQUENCE)]
        public virtual string SecurityLoopSequence { get; set; }

        [StringLength(StringLengths.ELECTRICAL_ACCOUNT_NUMBER)]
        public virtual string ElectricalAccountNumber { get; set; }

        public virtual bool SCADAIntrusionAlarm { get; set; }

        public virtual bool RMP { get; set; }

        public virtual long? RMPNumber { get; set; }

        [StringLength(StringLengths.NOTES), View("Notes")]
        public virtual string EntityNotes { get; set; }

        [StringLength(StringLengths.FACILITY_CONTACT_INFO)]
        public virtual string FacilityContactInfo { get; set; }

        public virtual bool SWMStation { get; set; }

        [View("Well – Prod")]
        public virtual bool WellProd { get; set; }

        [View("Well – Monitoring")]
        public virtual bool WellMonitoring { get; set; }

        [View("ClearWell")]
        public virtual bool ClearWell { get; set; }

        public virtual bool RawWaterIntake { get; set; }
        public virtual DateTime UpdatedAt { get; set; }

        [View("Is In VAMP")]
        public virtual bool? IsInVamp { get; set; }
        [View("VAMP Url")]
        public virtual string VampUrl { get; set; }

        [StringLength(StringLengths.INSURANCE_ID), View(DisplayNames.INSURANCE_ID)]
        public virtual string InsuranceId { get; set; }

        [View(DisplayNames.INSURANCE_SCORE)]
        public virtual decimal? InsuranceScore { get; set; }

        [View(DisplayNames.INSURANCE_SCORE_QUARTILE)]
        public virtual InsuranceScoreQuartile InsuranceScoreQuartile { get; set; }

        [View(DisplayNames.INSURANCE_LAST_VISIT_DATE)]
        public virtual DateTime? InsuranceVisitDate { get; set; }
        public virtual User CreatedBy { get; set; }
        public virtual IList<MaintenancePlan> MaintenancePlans { get; set; }

        #endregion

        public virtual IList<Document<Facility>> FacilityDocuments { get; set; }
        public virtual IList<Note<Facility>> FacilityNotes { get; set; }
        public virtual IList<Equipment> Equipment { get; set; }
        public virtual IList<Interconnection> Interconnections { get; set; }

        public virtual IEnumerable<FilterMedia> FilterMediae
        {
            get { return Equipment.SelectMany(e => e.FilterMediae); }
        }

        [View("Facility Ownership")]
        public virtual FacilityOwner FacilityOwner { get; set; }

        public virtual FacilityStatus FacilityStatus { get; set; }
        public virtual FEMAFloodRating FEMAFloodRating { get; set; }

        public virtual Department Department { get; set; }

        [View(WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public virtual WasteWaterSystem WasteWaterSystem { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual TownSection TownSection { get; set; }
        public virtual CompanySubsidiary CompanySubsidiary { get; set; }

        public virtual MapIcon Icon => Coordinate?.Icon;

        public virtual OperatingCenter OperatingCenter { get; set; }

        [StringLength(StringLengths.FUNCTIONAL_LOCATION)]
        public virtual string FunctionalLocation { get; set; }

        public virtual ElectricalProvider ElectricalProvider { get; set; }
        public virtual PlanningPlant PlanningPlant { get; set; }
        public virtual PublicWaterSupplyPressureZone PublicWaterSupplyPressureZone { get; set; }
        [View(WasteWaterSystemBasin.DisplayNames.WASTEWATER_SYSTEM_BASIN)]
        public virtual WasteWaterSystemBasin WasteWaterSystemBasin { get; set; }

        [View("Facility Maintenance Condition")]
        public virtual FacilityCondition Condition { get; set; }

        [View("Facility Maintenance Performance")]
        public virtual FacilityPerformance Performance { get; set; }

        [View("Facility Maintenance LOF")]
        public virtual FacilityLikelihoodOfFailure LikelihoodOfFailure { get; set; }

        [View("Facility Maintenance COF")]
        public virtual FacilityConsequenceOfFailure ConsequenceOfFailure { get; set; }

        [View("Facility Asset Maintenance Strategy Tier")]
        public virtual FacilityAssetManagementMaintenanceStrategyTier StrategyTier { get; set; }

        public virtual ProcessStage Process { get; set; }

        public virtual FacilityMaintenanceRiskOfFailure MaintenanceRiskOfFailure { get; set; }

        [View("Facility Maintenance COF Factor")]
        public virtual float? ConsequenceOfFailureFactor { get; set; }

        [View("Facility Maintenance Weighted Risk")]
        public virtual double? WeightedRiskOfFailureScore { get; set; }

        [View("Is there an ArcFlash Study at this site?")]
        public virtual bool? ArcFlashStudyRequired { get; set; }

        [View("AW Security Tier")]
        public virtual FacilityAWSecurityTier AWSecurityTier { get; set; }

        [View("Risk Based Maintenance Completed On Date", FormatStyle.Date)]
        public virtual DateTime? RiskBasedCompletedDate { get; set; }

        [View("Was the Facility Identified as a Critical Facility?")]
        public virtual bool? CriticalFacilityIdentified { get; set; }

        [View("J100 Assessment Completed On Date", FormatStyle.Date)]
        public virtual DateTime? AssessmentCompletedOn { get; set; }

        // Business Unit -- Logical
        public virtual BusinessUnit BusinessUnit
        {
            get
            {
                if (Department != null && Department.BusinessUnits.Count(x => x.OperatingCenter == OperatingCenter) > 0)
                    return Department.BusinessUnits.FirstOrDefault(x => x.OperatingCenter == OperatingCenter);
                return null;
            }
        }

        public virtual PublicWaterSupply PublicWaterSupply { get; set; }

        public virtual SystemDeliveryType SystemDeliveryType { get; set; }

        public virtual IList<FacilitySystemDeliveryEntryType> FacilitySystemDeliveryEntryTypes { get; set; }

        public virtual decimal? Latitude => Coordinate?.Latitude;

        public virtual decimal? Longitude => Coordinate?.Longitude;

        public virtual IList<FacilityKwhCost> KwhCosts { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return FacilityDocuments.Map(fd => (IDocumentLink)fd); }
        }

        public virtual IList<Document> Documents
        {
            get { return FacilityDocuments.Map(fd => fd.Document); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return FacilityNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return FacilityNotes.Map(n => n.Note); }
        }

        public virtual IList<FacilityVideo> Videos { get; set; }

        public virtual IEnumerable<IVideoLink> LinkedVideos => Videos;

        public virtual IList<FacilityProcess> FacilityProcesses { get; set; }

        public virtual IList<FacilityFacilityArea> FacilityAreas { get; set; }

        public virtual Facility ParentFacility { get; set; }

        public virtual IList<Facility> ChildFacilities { get; set; }

        public virtual ArcFlashAnalysisType TypeOfArcFlashAnalysis { get; set; }

        [View("Label Type")]
        public virtual ArcFlashLabelType ArcFlashLabelType { get; set; }

        public virtual IList<ArcFlashStudy> ArcFlashStudies { get; set; }

        [DoesNotExport]
        public virtual ISet<MostRecentArcFlashStudy> MostRecentArcFlashStudies { get; set; }

        public virtual IList<CommunityRightToKnow> CommunityRightToKnows { get; set; }

        public virtual IList<SystemDeliveryFacilityEntry> SystemDeliveryEntries { get; set; }

        #endregion

        #region Logical Properties

        [DoesNotExport]
        public virtual string Description => (_display ?? (_display = new FacilityDisplayItem {
            OperatingCenter = OperatingCenter,
            Id = Id,
            FacilityName = FacilityName
        })).Display;

        [DoesNotExport]
        public virtual string DescriptionWithDepartment => $"{Department} - {Description}";

        [DoesNotExport]
        public virtual string FacilityIdWithFacilityName => $"{FacilityId} - {FacilityName}";

        // For system delivery, they're going to populate regional planning area w/ the SAP system delivery functional location for now
        [DoesNotExport]
        public virtual string FacilityIdWithRegionalPlanningArea => $"{RegionalPlanningArea} - {FacilityId} - {FacilityName}";

        public virtual bool HasSensorAttached { get; set; }

        // Property placed down here because they want it to be the last
        // column on the excel export
        public virtual bool WaterStress { get; set; }
        public virtual bool IgnitionEnterprisePortal { get; set; }
        public virtual bool ArcFlashLabelRequired { get; set; }

        [DoesNotExport]
        public virtual string Address =>
            String.Format(
                ADDRESS_FORMAT,
                StreetNumber,
                Street?.Prefix,
                Street?.Name,
                Street?.Suffix,
                Town?.ShortName,
                Town?.State,
                ZipCode);

        public virtual IEnumerable<FacilityProcessStep> AllProcessStepsInOrder
        {
            get
            {
                return FacilityProcesses
                      .SelectMany(x => x.FacilityProcessSteps)
                      .OrderBy(x => x.FacilityProcess.Process.Sequence)
                      .ThenBy(x => x.StepNumber);
            }
        }
        
        #endregion

        [DoesNotExport]
        public virtual string TableName => FacilityMap.TABLE_NAME;

        #endregion

        #region Constructors

        public Facility()
        {
            FacilityDocuments = new List<Document<Facility>>();
            FacilityNotes = new List<Note<Facility>>();
            Equipment = new List<Equipment>();
            Interconnections = new List<Interconnection>();
            KwhCosts = new List<FacilityKwhCost>();
            FacilityProcesses = new List<FacilityProcess>();
            FacilityAreas = new List<FacilityFacilityArea>();
            Videos = new List<FacilityVideo>();
            ChildFacilities = new List<Facility>();
            ArcFlashStudies = new List<ArcFlashStudy>();
            FacilitySystemDeliveryEntryTypes = new List<FacilitySystemDeliveryEntryType>();
            SystemDeliveryEntries = new List<SystemDeliveryFacilityEntry>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        public virtual object FacilityToJson()
        {
            return new {
                Id,
                FacilityId,
                FacilityName,
                StreetNumber,
                Street = Street == null ? null : Street.Name,
                Town = Town == null ? null : Town.ShortName,
                ZipCode,
                FacilityTotalCapacityMGD,
                EmergencyPower,
                GroundWaterSupply,
                SurfaceWaterSupply,
                Reservoir,
                Dam,
                Interconnection,
                PointOfEntry,
                WaterTreatmentFacility,
                ChemicalFeed,
                DPCC,
                PSM,
                Filtration,
                ResidualsGeneration,
                TReport,
                DistributivePumping,
                BoosterStation,
                PressureReducing,
                GroundStorage,
                ElevatedStorage,
                OnSiteAnalyticalInstruments,
                SampleStation,
                SewerLiftStation,
                WasteWaterTreatmentFacility,
                UsedInProductionCapacityCalculation,
                IgnitionEnterprisePortal,
                ArcFlashLabelRequired,
                HasConfinedSpaceRequirement,
                BasicGroundWaterSupply,
                RawWaterPumpStation,
                WellProd,
                WellMonitoring,
                ClearWell,
                RawWaterIntake,
                WasteWaterSystem = WasteWaterSystem == null
                    ? null
                    : WasteWaterSystem.Description,
                OperatingCenter = OperatingCenter == null
                    ? null
                    : OperatingCenter.Description,
                FunctionalLocation,
                PlanningPlant = PlanningPlant == null
                    ? null
                    : PlanningPlant.Description,
                PublicWaterSupply = PublicWaterSupply == null
                    ? null
                    : PublicWaterSupply.Description
            };
        }

        #endregion
    }

    [Serializable]
    public class FacilityDisplayItem : DisplayItem<Facility>
    {
        public string FacilityName { get; set; }

        public string FacilityId =>
            string.Format(Facility.FACILITY_ID_FORMAT, OperatingCenter?.OperatingCenterCode, Id);

        public OperatingCenter OperatingCenter { get; set; }
        public override string Display => $"{FacilityName} - {FacilityId}";
        public Department Department { get; set; }
        public virtual string DescriptionWithDepartment => $"{Department} - {Display}";
        public string RegionalPlanningArea { get; set; }
        public virtual string FacilityIdWithRegionalPlanningArea => $"{RegionalPlanningArea} - {FacilityId} - {FacilityName}";
        public virtual string FacilityIdWithFacilityName => $"{FacilityId} - {FacilityName}";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }
    }

    [Serializable]
    public class FacilitySize : EntityLookup { }

    [Serializable]
    public class FacilityTransformerWiringType : EntityLookup { }

    [Serializable]
    public class FacilityAWSecurityTier : EntityLookup { }

    [Serializable]
    public class InsuranceScoreQuartile : ReadOnlyEntityLookup { }

    [Serializable]
    public class FacilityCondition : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int GOOD = 1, AVERAGE = 2, POOR = 3;
        }
    }

    [Serializable]
    public class FacilityPerformance : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int GOOD = 1, AVERAGE = 2, POOR = 3;
        }
    }

    [Serializable]
    public class FacilityLikelihoodOfFailure : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int LOW = 1, MEDIUM = 2, HIGH = 3;
        }
    }

    [Serializable]
    public class FacilityConsequenceOfFailure : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int LOW = 1, MEDIUM = 2, HIGH = 3;
        }
    }

    [Serializable]
    public class FacilityAssetManagementMaintenanceStrategyTier : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int TIER_1 = 1, TIER_2 = 2, TIER_3 = 3;
        }
    }
}
