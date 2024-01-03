using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using DataType = System.ComponentModel.DataAnnotations.DataType;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class FacilityViewModel : ViewModel<Facility>
    {
        #region Constants

        public const string PARENT_FUNCTIONAL_LOCATION_MISMATCH = "";
        private const string INSURANCE_VISIT_DATE_LABEL = "Last Visit Date";

        #endregion

        #region Properties

        [AutoMap(MapDirections.ToViewModel)]
        public override int Id { get; set; }

        [DisplayName("Facility Ownership"), DropDown]
        [EntityMustExist(typeof(FacilityOwner))]
        [EntityMap]
        public virtual int? FacilityOwner { get; set; }

        [DisplayName("Facility Status"), DropDown]
        [EntityMustExist(typeof(FacilityStatus))]
        [EntityMap]
        public virtual int? FacilityStatus { get; set; }

        [DisplayName("Company Subsidiary"), DropDown]
        [EntityMustExist(typeof(CompanySubsidiary))]
        [EntityMap]
        public virtual int? CompanySubsidiary { get; set; }

        [EntityMustExist(typeof(FEMAFloodRating))]
        [DisplayName("FEMA Flood Rating"), DropDown]
        [EntityMap]
        public virtual int? FEMAFloodRating { get; set; }

        [DisplayName("Department"), DropDown, Required]
        [EntityMustExist(typeof(Department))]
        [EntityMap]
        public virtual int? Department { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [DisplayName("Town")]
        [EntityMustExist(typeof(Town))]
        [EntityMap]
        public virtual int? Town { get; set; }

        public virtual int? TownSection { get; set; }

        [DropDown("", "PlanningPlant", "ByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMustExist(typeof(PlanningPlant)), EntityMap]
        public virtual int? PlanningPlant { get; set; }

        [EntityMap,
         EntityMustExist(typeof(WasteWaterSystem)),
         DropDown("Environmental", "WasteWaterSystem", "ByOperatingCenterAndTown", DependsOn = "OperatingCenter,Town", PromptText = "Select an operating center and town")]
        public virtual int? WasteWaterSystem { get; set; }

        [EntityMap,
         EntityMustExist(typeof(WasteWaterSystemBasin)),
         DropDown("Environmental", "WasteWaterSystemBasin", "ByWasteWaterSystem", DependsOn = nameof(WasteWaterSystem), PromptText = "Select a waste water system basin")]
        public virtual int? WasteWaterSystemBasin { get; set; }

        [Coordinate, View("Coordinates")]
        [EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        public virtual int? CoordinateId { get; set; }

        [DisplayName("Operating Center"), DropDown, Required]
        [EntityMustExist(typeof(OperatingCenter))]
        [EntityMap]
        public virtual int? OperatingCenter { get; set; }

        //[DropDown("FieldOperations", "FunctionalLocation", "ByTownForFacilityAssetType", DependsOn = "Town", PromptText = "Please select a town above")]
        //[EntityMustExist(typeof(FunctionalLocation))]
        //[EntityMap]
        [UIHint("FunctionalLocation")]
        public virtual string FunctionalLocation { get; set; }

        [DropDown("", "Facility", "ByFunctionalLocation", DependsOn = "FunctionalLocation", PromptText = "Please enter a Functional Location"), EntityMap, EntityMustExist(typeof(Facility))]
        public virtual int? ParentFacility { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FacilityAWSecurityTier))]
        public int? AWSecurityTier { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ProcessStage))]
        public int? Process { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SystemDeliveryType))]
        [RequiredWhen(nameof(PointOfEntry), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        public int? SystemDeliveryType { get; set; }

        //TODO: These are columns, but not used on mapcall proper
        //public virtual string CountyID { get; set; }
        //public virtual string StateID { get; set; }
        //public virtual int tblTownsID { get; set; }
        //public virtual int tblTownSectionID { get; set; }
        //public virtual string pNodeID { get; set; }

        //public virtual DateTime DateCreated { get; set; }
        [StringLength(Facility.StringLengths.OPERATIONS)]
        public virtual string Operations { get; set; }
        [StringLength(Facility.StringLengths.SYSTEM)]
        public virtual string System { get; set; }

        [StringLength(Facility.StringLengths.FACILITY_NAME)]
        public virtual string FacilityName { get; set; }
        [StringLength(Facility.StringLengths.STREET_NUMBER)]
        public virtual string StreetNumber { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town"), EntityMap, EntityMustExist(typeof(Street))]
        public virtual int? Street { get; set; }

        [DropDown("", "Street", "ByTownId", DependsOn = "Town"), EntityMap, EntityMustExist(typeof(Street))]
        public virtual int? NearestCrossStreet { get; set; }

        [EntityMap,
         EntityMustExist(typeof(PublicWaterSupply)),
         DropDown("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an operating center")]
        public virtual int? PublicWaterSupply { get; set; }

        [EntityMap,
         EntityMustExist(typeof(PublicWaterSupplyPressureZone)),
         DropDown("Environmental", "PublicWaterSupplyPressureZone", "ByPublicWaterSupply", DependsOn = nameof(PublicWaterSupply), PromptText = "Please select a public water supply")]
        public virtual int? PublicWaterSupplyPressureZone { get; set; }

        [StringLength(Facility.StringLengths.ZIP_CODE)]
        public virtual string ZipCode { get; set; }
        public virtual decimal? FacilityTotalCapacityMGD { get; set; }
        public virtual int? CriticalRating { get; set; }
        [StringLength(Facility.StringLengths.YEAR_IN_SERVICE)]
        public virtual string YearInService { get; set; }
        public virtual int? SICNumber { get; set; }
        [StringLength(Facility.StringLengths.ENVIRONMENTAL_REGULATOR_ID_NUMBER)]
        public virtual string EnvironmentalRegulatorIDNumber { get; set; }
        [StringLength(Facility.StringLengths.WATER_SHED)]
        public virtual string WaterShed { get; set; }
        [StringLength(Facility.StringLengths.REGIONAL_PLANNING_AREA)]
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
        public virtual bool WaterStress { get; set; }
        public virtual bool WaterTreatmentFacility { get; set; }
        public virtual bool ChemicalFeed { get; set; }

        [DropDown("Environmental", "ChemicalStorageLocation", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [EntityMap, EntityMustExist(typeof(ChemicalStorageLocation))]
        [RequiredWhen("ChemicalFeed", ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        public virtual int? ChemicalStorageLocation { get; set; }

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
        [CheckBox]
        public virtual bool HasConfinedSpaceRequirement { get; set; }
        public virtual bool CellularAntenna { get; set; }
        [StringLength(Facility.StringLengths.DESIGNATION_TREATMENT_PLANT)]
        public virtual string DesignationTreatmentPlant { get; set; }
        [StringLength(Facility.StringLengths.DESIGNATION_PUMP_STATION)]
        public virtual string DesignationPumpStation { get; set; }
        public virtual decimal? FacilityReliableCapacityMGD { get; set; }
        public virtual decimal? FacilityOperatingCapacityMGD { get; set; }
        public virtual decimal? FacilityRatedCapacityMGD { get; set; }
        public virtual decimal? FacilityAuxPowerCapacityMGD { get; set; }
        public virtual bool? UsedInProductionCapacityCalculation { get; set; }
        [StringLength(Facility.StringLengths.FACILITY_INSPECTION_FREQUENCY)]
        public virtual string FacilityInspectionFrequency { get; set; }
        [StringLength(Facility.StringLengths.FACILITY_LOOP_GROUPING)]
        public virtual string FacilityLoopGrouping { get; set; }
        [StringLength(Facility.StringLengths.FACILITY_LOOP_GROUPING_SUB)]
        public virtual string FacilityLoopGroupingSub { get; set; }
        public virtual int? FacilityLoopSequence { get; set; }
        [StringLength(Facility.StringLengths.SECURITY_CATEGORY)]
        public virtual string SecurityCategory { get; set; }
        [StringLength(Facility.StringLengths.SECURITY_GROUPING)]
        public virtual string SecurityGrouping { get; set; }
        [StringLength(Facility.StringLengths.SECURITY_INSPECTION_FREQUENCY)]
        public virtual string SecurityInspectionFrequency { get; set; }
        [StringLength(Facility.StringLengths.SECURITY_LOOP_SEQUENCE)]
        public virtual string SecurityLoopSequence { get; set; }
        public virtual bool SCADAIntrusionAlarm { get; set; }
        public virtual bool RMP { get; set; }
        [RequiredWhen("RMP", ComparisonType.EqualTo, true)]
        public virtual long? RMPNumber { get; set; }
        [StringLength(Facility.StringLengths.NOTES, ErrorMessage = "The field Notes must be a string with a maximum length of 255.")]
        [DataType(DataType.MultilineText)]
        [View("Notes")]
        public virtual string EntityNotes { get; set; }
        [StringLength(Facility.StringLengths.FACILITY_CONTACT_INFO)]
        public virtual string FacilityContactInfo { get; set; }
        public bool? ArcFlashStudyRequired { get; set; } = true;

        [DropDown, EntityMap, EntityMustExist(typeof(FacilityCondition))]
        [RoleSecured(RoleModules.ProductionFacilityAreaManagement, RoleActions.Edit, AppliesToAdmins = false)]
        public virtual int? Condition { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FacilityPerformance))]
        [RoleSecured(RoleModules.ProductionFacilityAreaManagement, RoleActions.Edit, AppliesToAdmins = false)]
        public virtual int? Performance { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FacilityConsequenceOfFailure))]
        public virtual int? ConsequenceOfFailure { get; set; }

        [EntityMap(MapDirections.ToViewModel)]
        public virtual FacilityLikelihoodOfFailure LikelihoodOfFailure { get; set; }

        [EntityMap(MapDirections.ToViewModel)]
        public virtual FacilityMaintenanceRiskOfFailure MaintenanceRiskOfFailure { get; set; }

        [EntityMap(MapDirections.ToViewModel)]
        [View("Facility COF Factor")]
        public virtual float? ConsequenceOfFailureFactor { get; set; }
        [EntityMap(MapDirections.ToViewModel)]
        [View("Facility Weighted Risk")]
        public virtual double? WeightedRiskOfFailureScore { get; set; }
        [EntityMap(MapDirections.ToViewModel)]
        [View("Facility Asset Maintenance Strategy Tier")]
        public virtual FacilityAssetManagementMaintenanceStrategyTier StrategyTier { get; set; }
        public virtual bool SWMStation { get; set; }
        public virtual bool WellProd { get; set; }
        public virtual bool WellMonitoring { get; set; }
        public virtual bool ClearWell { get; set; }
        public virtual bool RawWaterIntake { get; set; }
        [AutoMap(MapDirections.ToEntity)]
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual bool Radionuclides { get; set; }
        public virtual bool CommunityRightToKnow { get; set; }
        public virtual bool IgnitionEnterprisePortal { get; set; }
        public virtual bool ArcFlashLabelRequired { get; set; }

        [CheckBox]
        public virtual bool? IsInVamp { get; set; }
        [DataAnnotationsExtensions.Url]
        [StringLength(Facility.StringLengths.VAMP_URL)]
        public virtual string VampUrl { get; set; }

        public virtual DateTime? RiskBasedCompletedDate { get; set; }

        public virtual bool? CriticalFacilityIdentified { get; set; }

        [RequiredWhen("CriticalFacilityIdentified", ComparisonType.NotEqualTo, null)] 
        public virtual DateTime? AssessmentCompletedOn { get; set; }

        public virtual bool BasicGroundWaterSupply { get; set; }

        public virtual bool RawWaterPumpStation { get; set; }
        
        public virtual string InsuranceId { get; set; }
        public virtual decimal? InsuranceScore { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(InsuranceScoreQuartile))]
        public int? InsuranceScoreQuartile { get; set; }

        [View(INSURANCE_VISIT_DATE_LABEL)]
        public virtual DateTime? InsuranceVisitDate { get; set; }

        #endregion

        #region Constructors

        public FacilityViewModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        protected FacilityLikelihoodOfFailure CalculateLikelihoodOfFailure(int condition, int performance)
        {
            int id = -1;

            switch (condition)
            {
                case FacilityCondition.Indices.GOOD:
                    id = performance == FacilityPerformance.Indices.POOR
                        ? FacilityLikelihoodOfFailure.Indices.MEDIUM
                        : FacilityLikelihoodOfFailure.Indices.LOW;
                    break;
                case FacilityCondition.Indices.AVERAGE:
                    id = performance;
                    break;
                case FacilityCondition.Indices.POOR:
                    id = performance == FacilityPerformance.Indices.GOOD
                        ? FacilityLikelihoodOfFailure.Indices.MEDIUM
                        : FacilityLikelihoodOfFailure.Indices.HIGH;
                    break;
            }

            return new FacilityLikelihoodOfFailure {Id = id};
        }

        protected int CalculateRiskScore(int consequence, int likelihood)
        {
            return consequence * likelihood;
        }

        private FacilityAssetManagementMaintenanceStrategyTier GetStrategyTier(int value)
        {
            if (value > 5)
            {
                return new FacilityAssetManagementMaintenanceStrategyTier
                    {Id = FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_1};
            }

            if (value > 2)
            {
                return new FacilityAssetManagementMaintenanceStrategyTier
                    {Id = FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_2};
            }

            return new FacilityAssetManagementMaintenanceStrategyTier
                {Id = FacilityAssetManagementMaintenanceStrategyTier.Indices.TIER_3};
        }

        private float? CalculateConsequenceOfFailureFactor(PublicWaterSupply thisPWS)
        {
            if (thisPWS?.UsageLastYear == null)
            {
                return null;
            }

            var pwsRepo = _container.GetInstance<IRepository<PublicWaterSupply>>();
            var usages = pwsRepo.Where(p =>
                p.State.Id == thisPWS.State.Id &&
                p.UsageLastYear.HasValue);
            var minUsage = usages.Min(x => x.UsageLastYear);
            var maxUsage = usages.Max(x => x.UsageLastYear);
            var thisUsage = thisPWS.UsageLastYear.Value;

            var topHalf = (0.5f * thisUsage) + (0.5f * maxUsage) - minUsage;
            var bottomHalf = maxUsage - minUsage;

            return bottomHalf == 0 ? null : topHalf / bottomHalf;
        }

        protected void CalculateRiskCharacteristics(Facility entity)
        {
            if (!Condition.HasValue || !Performance.HasValue)
            {
                entity.LikelihoodOfFailure = null;
                entity.StrategyTier = null;
            }
            else
            {
                entity.LikelihoodOfFailure = CalculateLikelihoodOfFailure(Condition.Value, Performance.Value);
                if (!ConsequenceOfFailure.HasValue)
                {
                    entity.MaintenanceRiskOfFailure = null;
                    entity.StrategyTier = null;
                }
                else
                {
                    int score = CalculateRiskScore(ConsequenceOfFailure.Value, entity.LikelihoodOfFailure.Id);
                    entity.MaintenanceRiskOfFailure = _container.GetInstance<IRepository<FacilityMaintenanceRiskOfFailure>>().Where(x => x.RiskScore == score).FirstOrDefault();
                    entity.StrategyTier = GetStrategyTier(score);
                }
            }

            entity.ConsequenceOfFailureFactor = CalculateConsequenceOfFailureFactor(entity.PublicWaterSupply);

            if (entity.MaintenanceRiskOfFailure != null && entity.ConsequenceOfFailureFactor.HasValue)
            {
                entity.WeightedRiskOfFailureScore = Math.Round(((double)entity.ConsequenceOfFailureFactor.Value) * entity.MaintenanceRiskOfFailure.RiskScore, 6);
            }
        }

        private void CalculateEquipmentRiskCharacteristics(Facility entity)
        {
            if (entity.StrategyTier == null)
            {
                return;
            }

            foreach (var equipment in entity.Equipment.Where(e => e.LikelyhoodOfFailure != null && e.ConsequenceOfFailure != null))
            {
                var previousEquipmentRiskOfFailureId = equipment.RiskOfFailure?.Id;
                
                equipment.RiskOfFailure = new EquipmentFailureRiskRating {
                    Id = EquipmentRiskOfFailureCalculator.CalculateRisk(entity.StrategyTier.Id,
                        equipment.LikelyhoodOfFailure.Id, equipment.ConsequenceOfFailure.Id)
                };

                if (equipment.RiskOfFailure.Id == previousEquipmentRiskOfFailureId)
                {
                    continue;
                }

                equipment.RiskCharacteristicsLastUpdatedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
                equipment.RiskCharacteristicsLastUpdatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
            }
        }

        private IEnumerable<ValidationResult> ValidateParentFacility()
        {
            if (ParentFacility.HasValue && !string.IsNullOrWhiteSpace(FunctionalLocation))
            {
                var parentFacility = _container.GetInstance<IRepository<Facility>>().Find(ParentFacility.Value);
                if (!FunctionalLocation.StartsWith(parentFacility.FunctionalLocation))
                    yield return new ValidationResult(PARENT_FUNCTIONAL_LOCATION_MISMATCH, new[]{"ParentFacility"});
            }
        }

        #endregion

        #region Exposed Methods

        public override Facility MapToEntity(Facility entity)
        {
            entity = base.MapToEntity(entity);

            CalculateRiskCharacteristics(entity);
            CalculateEquipmentRiskCharacteristics(entity);

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateParentFacility());
        }

        #endregion
    }

    public class CreateFacilityBase : FacilityViewModel
    {
        #region Constructor

        public CreateFacilityBase(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateFacility : CreateFacilityBase
    {
        #region Properties

        [DisplayName("Operating Center"), DropDown, Required]
        [EntityMustExist(typeof(OperatingCenter))]
        [EntityMap]
        public override int? OperatingCenter { get; set; }

        [DropDown("", "TownSection", "ActiveByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        [EntityMustExist(typeof(TownSection))]
        [EntityMap]
        public override int? TownSection { get; set; }

        [RequiredWhen(nameof(WasteWaterSystem), ComparisonType.EqualTo, null, ErrorMessage = "Please select either a public water supply or a waste water system")]
        public override int? PublicWaterSupply { get; set; }

        [RequiredWhen(nameof(PublicWaterSupply), ComparisonType.NotEqualTo, null, ErrorMessage = "Please select a public water supply pressure zone")]
        public override int? PublicWaterSupplyPressureZone { get; set; }

        [RequiredWhen(nameof(PublicWaterSupply), ComparisonType.EqualTo, null, ErrorMessage = "Please select either a public water supply or a waste water system")]
        public override int? WasteWaterSystem { get; set; }

        [RequiredWhen(nameof(WasteWaterSystem), ComparisonType.NotEqualTo, null, ErrorMessage = "Please select a waste water system basin")]
        public override int? WasteWaterSystemBasin { get; set; }

        #endregion

        #region Constructors

        public CreateFacility(IContainer container) : base(container) { }

        #endregion
    }

    public abstract class EditFacilityBase : FacilityViewModel
    {
        #region Properties

        [DropDown("", "TownSection", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above")]
        [EntityMustExist(typeof(TownSection))]
        [EntityMap]
        public override int? TownSection { get; set; }

        #endregion

        #region Constructors

        public EditFacilityBase(IContainer container) : base(container) { }

        #endregion
    }

    public class EditFacility : EditFacilityBase
    {
        [DisplayName("Operating Center"), DropDown, RequiredWhen("ArcFlashStudyRequired", ComparisonType.EqualTo, true)]
        [EntityMustExist(typeof(OperatingCenter))]
        [EntityMap]
        public override int? OperatingCenter { get; set; }

        [DisplayName("Department"), DropDown]
        [EntityMustExist(typeof(Department))]
        [EntityMap, RequiredWhen("ArcFlashStudyRequired", ComparisonType.EqualTo, true)]
        public override int? Department { get; set; }

        public EditFacility(IContainer container) : base(container) { }
    }

    public class SearchFacility : SearchSet<Facility>
    {
        public int? EntityId { get; set; }

        [StringLength(Facility.StringLengths.FACILITY_NAME)]
        public string FacilityName { get; set; }
        //Corporation - ddl
        //Region - ddl
        //Department - ddl
        [DropDown]
        public int? Department { get; set; }
        //Company Subsidiary - ddl
        //Operations - ddl
        //System - ddl
        //PWSID - ddl
        //OpCode - ddl
        //FacilityId - ddl
        //District -ddl
        [DropDown("", "OperatingCenter", "ByStateIdForProductionFacilities", DependsOn = "State", PromptText = "Please select a state above")]
        [EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [RequiredWhen("State", ComparisonType.NotEqualTo, null, ErrorMessage = "Required with State")]
        public int? OperatingCenter { get; set; }

        [DropDown("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter), PromptText = "Please select an operating center")]
        public int? PublicWaterSupply { get; set; }

        [DropDown("Environmental", "PublicWaterSupplyPressureZone", "ByPublicWaterSupply", DependsOn = nameof(PublicWaterSupply), PromptText = "Please select a public water supply")]
        public int? PublicWaterSupplyPressureZone { get; set; }

        [DropDown(Area = "", Controller = "PlanningPlant", Action = "ByOperatingCenter", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public virtual int? PlanningPlant { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State)), Search(CanMap = false)]
        public int? State { get; set; }

        [DropDown("", "County", "ByStateId", DependsOn = "State", PromptText = "Please select a state above")]
        [SearchAlias("Town", "T", "County.Id")]
        [DisplayName("County")]
        public int? TownCounty { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Town { get; set; }

        [DropDown("Environmental", "WasteWaterSystem", "ByOperatingCenterAndTown", DependsOn = "OperatingCenter,Town", PromptText = "Select an operating center and town (optional)")]
        [View(MapCall.Common.Model.Entities.WasteWaterSystem.DisplayNames.WASTEWATER_SYSTEM)]
        public int? WasteWaterSystem { get; set; }

        [DropDown("Environmental", "WasteWaterSystemBasin", "ByWasteWaterSystem", DependsOn = nameof(WasteWaterSystem), PromptText = "Select a waste water system basin")]
        [View(MapCall.Common.Model.Entities.WasteWaterSystemBasin.DisplayNames.WASTEWATER_SYSTEM_BASIN)]
        public int? WasteWaterSystemBasin { get; set; }

        [StringLength(Facility.StringLengths.FUNCTIONAL_LOCATION)]
        [UIHint("FunctionalLocation")]
        public string FunctionalLocation { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(FacilityAWSecurityTier))]
        public int? AWSecurityTier { get; set; }

        public int? CriticalRating { get; set; }
        [DropDown]
        public int? FacilityOwner { get; set; }
        [DropDown]
        public int? FacilityStatus { get; set; }
        public string FacilityInspectionFrequency { get; set; }
        public string FacilityLoopGrouping { get; set; }
        public bool? PropertyOnly { get; set; }
        public bool? Administration { get; set; }
        public bool? EmergencyPower { get; set; }
        public bool? GroundWaterSupply { get; set; }
        public bool? SurfaceWaterSupply { get; set; }
        public bool? Reservoir { get; set; }
        public bool? Dam { get; set; }
        public bool? Interconnection { get; set; }
        public bool? PointOfEntry { get; set; }
        public bool? WaterStress { get; set; }
        public bool? WaterTreatmentFacility { get; set; }
        public bool? ChemicalFeed { get; set; }
        public bool? DPCC { get; set; }
        public bool? PSM { get; set; }
        public bool? RMP { get; set; }
        public bool? SWMStation { get; set; }
        [View("Well – Prod")]
        public bool? WellProd { get; set; }
        [View("Well – Monitoring")]
        public bool? WellMonitoring { get; set; }
        public bool? ClearWell { get; set; }
        public bool? RawWaterIntake { get; set; }
        public bool? Filtration { get; set; }
        public bool? ResidualsGeneration { get; set; }
        public bool? TReport { get; set; }
        public bool? DistributivePumping { get; set; }
        public bool? BoosterStation { get; set; }
        public bool? PressureReducing { get; set; }
        public bool? GroundStorage { get; set; }
        public bool? ElevatedStorage { get; set; }
        public bool? OnSiteAnalyticalInstruments { get; set; }
        public bool? SampleStation { get; set; }
        public bool? SewerLiftStation { get; set; }
        public bool? WasteWaterTreatmentFacility { get; set; }
        public bool? Radionuclides { get; set; }
        public bool? CommunityRightToKnow { get; set; }
        public bool? IgnitionEnterprisePortal { get; set; }
        public bool? ArcFlashLabelRequired { get; set; }

        [CheckBox]
        public bool? HasConfinedSpaceRequirement { get; set; }
        [DropDown]
        public int? FEMAFloodRating { get; set; }
        public bool? CellularAntenna { get; set; }
        public bool? FieldOperations { get; set; }
        public bool? SpoilsStaging { get; set; }

        public bool? HasSensorAttached { get; set; }

        [SearchAlias("MostRecentArcFlashStudies", "ExpiringWithinAYear")]
        public bool? HasExpiringArcFlashStudy { get; set; }
        [SearchAlias("MostRecentArcFlashStudies", "DateLabelsApplied")]
        public DateRange ArcFlashLastDateValidated { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(CompanySubsidiary))]
        public virtual int? CompanySubsidiary { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(ProcessStage))]
        public virtual int? Process { get; set; }

        [CheckBox]
        [View("Is In VAMP")]
        public bool? IsInVamp { get; set; }

        [View(Facility.DisplayNames.BASIC_GROUND_WATER_SUPPLY)]
        public bool? BasicGroundWaterSupply { get; set; }

        public bool? RawWaterPumpStation { get; set; }

        [View(Facility.DisplayNames.INSURANCE_ID)]
        public string InsuranceId { get; set; }

        [View(Facility.DisplayNames.INSURANCE_SCORE)]
        public decimal? InsuranceScore { get; set; }

        [View(Facility.DisplayNames.INSURANCE_SCORE_QUARTILE)]
        [DropDown, EntityMap, EntityMustExist(typeof(InsuranceScoreQuartile))]
        public int? InsuranceScoreQuartile { get; set; }
        
        [View(Facility.DisplayNames.INSURANCE_LAST_VISIT_DATE)]
        public DateTime? InsuranceVisitDate { get; set; }
    }

    public class SearchFacilityReadings
    {
        [Required, EntityMustExist(typeof(Facility))]
        public int? Id { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        [Required, CompareTo("EndDate", ComparisonType.LessThanOrEqualTo, TypeCode.DateTime)]
        public DateTime? StartDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        [Required, CompareTo("StartDate", ComparisonType.GreaterThanOrEqualTo, TypeCode.DateTime)]
        public DateTime? EndDate { get; set; }

        [DropDown]
        public ReadingGroupType Interval { get; set; }

        // Not a posted value, but needed for the view
        public IEnumerable<FacilityReadingCost> ReadingCosts { get; set; }

        /// <summary>
        /// Gets the sum total of all the Total values from ReadingCosts.
        /// </summary>
        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public decimal? Total => ReadingCosts?.Sum(x => x.Total);
    }

    public class FacilityReadingCost
    {
        [DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS)]
        public DateTime Date { get; set; }
        public double ReadingValue { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public decimal KwhCost { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY)]
        public decimal Total => KwhCost * Convert.ToDecimal(ReadingValue);
    }

    public class SearchArcFlashCompletion : SearchSet<ArcFlashCompletionReportItem> { }

    public class SearchFacilitySystemDeliveryHistory
    {
        [Required, EntityMustExist(typeof(Facility))]
        public int Id { get; set; }
        public List<FacilitySystemDeliveryHistoryViewModel> SystemDeliveryHistory { get; set;}
    }
}