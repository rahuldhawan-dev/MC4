using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites
{
    public class SampleSiteViewModel : ViewModel<SampleSite>
    {
        #region Constants

        public const string LIMS_FACILITY_ID_VALIDATION_ERROR = "The LIMS Facility Id field is required.";
        public const string LIMS_PRIMARY_STATION_CODE_VALIDATION_ERROR = "The LIMS Primary Station Code field is required.";
        public const string LIMS_SITE_ID_VALIDATION_ERROR = "The LIMS Site Id field is required.";
        public const string AGENCY_ID_VALIDATION_ERROR = "The AgencyId field is required.";

        public static readonly int[] VALID_STATES_FOR_LIMS_FACILITY_ID_REQUIRED_VALIDATION = {
            MapCall.Common.Model.Entities.State.Indices.CA,
            MapCall.Common.Model.Entities.State.Indices.PA
        };

        public static readonly int[] VALID_STATES_FOR_LIMS_SITE_ID_REQUIRED_VALIDATION = {
            MapCall.Common.Model.Entities.State.Indices.CA
        };

        public static readonly int[] INVALID_STATES_FOR_LIMS_PRIMARY_STATION_CODE_REQUIRED_VALIDATION = {
            MapCall.Common.Model.Entities.State.Indices.CA
        };

        #endregion

        #region Private Members

        private Premise _displayPremise;
        private User _certifiedByDisplay;

        #endregion

        #region Properties

        [DoesNotAutoMap("Display only")]
        public Premise DisplayPremise
        {
            get
            {
                if (_displayPremise == null && Premise.HasValue)
                {
                    _displayPremise = _container.GetInstance<IPremiseRepository>().Find(Premise.Value);
                }

                return _displayPremise;
            }
        }

        [DoesNotAutoMap("Display only")]
        public User CertifiedByDisplay
        {
            get
            {
                if (_certifiedByDisplay == null && CertifiedBy.HasValue)
                {
                    _certifiedByDisplay = _container.GetInstance<IRepository<User>>().Find(CertifiedBy.Value);
                }

                return _certifiedByDisplay;
            }
        }

        [DoesNotAutoMap]
        public bool IsBeingValidated { get; set; }

        [AutoMap(MapDirections.ToPrimary)]
        public DateTime? ValidatedAt { get; set; }

        [AutoMap(MapDirections.ToPrimary)]
        public User ValidatedBy { get; set; }
        
        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public virtual int? State { get; set; }

        [EntityMap, EntityMustExist(typeof(OperatingCenter)), Required]
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = nameof(State))]
        public virtual int? OperatingCenter { get; set; }

        [Required,
         EntityMap, 
         EntityMustExist(typeof(PublicWaterSupply)),
         DropDown("", 
             "PublicWaterSupply", 
             "ActiveOrPendingByOperatingCenterId", 
             DependsOn = nameof(OperatingCenter),
             PromptText = "Please select an operating center above.")]
        public virtual int? PublicWaterSupply { get; set; }

        [Required, EntityMap, DropDown, EntityMustExist(typeof(SampleSiteAvailability))]
        public virtual int? Availability { get; set; }

        [Required, EntityMap, DropDown, EntityMustExist(typeof(SampleSiteStatus))]
        public virtual int? Status { get; set; }

        [EntityMap, 
         DropDown, 
         EntityMustExist(typeof(SampleSiteInactivationReason)),
         RequiredWhen(
             nameof(Status), 
             ComparisonType.EqualTo, 
             SampleSiteStatus.Indices.INACTIVE, 
             FieldOnlyVisibleWhenRequired = true)]
        public virtual int? SampleSiteInactivationReason { get; set; }

        [EntityMap, DropDown("", "County", "ByStateId", DependsOn = "State"),
         EntityMustExist(typeof(County))]
        public virtual int? County { get; set; }

        [EntityMap, DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter")]
        [RequiredWhen(nameof(SampleSiteAddressLocationType), ComparisonType.EqualTo, MapCall.Common.Model.Entities.SampleSiteAddressLocationType.Indices.FACILITY, FieldOnlyVisibleWhenRequired = true)]
        [EntityMustExist(typeof(Facility))]
        public virtual int? Facility { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Hydrant)),
         DropDown("FieldOperations", "Hydrant", "ByOperatingCenter", DependsOn = nameof(OperatingCenter)),
         RequiredWhen(
             nameof(SampleSiteAddressLocationType), 
             ComparisonType.EqualTo, 
             MapCall.Common.Model.Entities.SampleSiteAddressLocationType.Indices.HYDRANT, 
             FieldOnlyVisibleWhenRequired = true)]
        public virtual int? Hydrant { get; set; }

        [EntityMap,
         EntityMustExist(typeof(Valve)),
         DropDown("FieldOperations", "Valve", "ByOperatingCenter", DependsOn = nameof(OperatingCenter)),
         RequiredWhen(
             nameof(SampleSiteAddressLocationType),
             ComparisonType.EqualTo,
             MapCall.Common.Model.Entities.SampleSiteAddressLocationType.Indices.VALVE,
             FieldOnlyVisibleWhenRequired = true)]
        public virtual int? Valve { get; set; }

        [EntityMap, 
         EntityMustExist(typeof(Premise)),
         RequiredWhen(
             nameof(SampleSiteAddressLocationType), 
             ComparisonType.EqualTo, 
             MapCall.Common.Model.Entities.SampleSiteAddressLocationType.Indices.PREMISE, 
             FieldOnlyVisibleWhenRequired = true)]
        public int? Premise { get; set; }

        [EntityMap, DropDown("", "Gradient", "ByTownId", DependsOn = "Town", PromptText = "Please select a town above.")]
        [EntityMustExist(typeof(Gradient))]
        public int? Gradient { get; set; }

        [Required, Coordinate(IconSet = IconSets.Beakers), EntityMap]
        [EntityMustExist(typeof(Coordinate))]
        public virtual int? Coordinate { get; set; }

        [EntityMap, DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter")]
        [EntityMustExist(typeof(Town))]
        [RequiredWhen("OutOfServiceArea", false)]
        public virtual int? Town { get; set; }

        [EntityMap, DropDown("", "TownSection", "ByTownId", DependsOn = "Town")]
        [EntityMustExist(typeof(TownSection))]
        public virtual int? TownSection { get; set; }

        [EntityMap, EntityMustExist(typeof(ServiceMaterial)), DropDown]
        public virtual int? CustomerPlumbingMaterial { get; set; }

        [StringLength(SampleSite.StringLengths.CUSTOMER_NAME)]
        public virtual string CustomerName { get; set; }

        [StringLength(SampleSite.StringLengths.CUSTOMER_HOME_PHONE)]
        public virtual string CustomerHomePhone { get; set; }

        [StringLength(SampleSite.StringLengths.CUSTOMER_ALT_PHONE)]
        public virtual string CustomerAltPhone { get; set; }

        [StringLength(SampleSite.StringLengths.CUSTOMER_EMAIL)]
        public virtual string CustomerEmail { get; set; }

        [DropDown,
         EntityMap,
         EntityMustExist(typeof(SampleSiteCustomerContactMethod)),
         View(SampleSite.Display.CUSTOMER_CONTACT_METHOD)]
        public int? CustomerContactMethod { get; set; }

        public virtual bool? CustomerParticipationConfirmed { get; set; }
        public virtual bool? FieldVerifiedServiceMaterial { get; set; }
        public virtual bool? FieldVerifiedServiceCustomerSideMaterial { get; set; }
        public virtual bool? FieldVerifiedCustomerPlumbingMaterial { get; set; }

        public virtual bool? PreviousMonitoringPeriod { get; set; }

        [Required,
         StringLength(SampleSite.StringLengths.LOCATION_NAME_DESCRIPTION)]
        public virtual string LocationNameDescription { get; set; }

        public virtual bool? BactiSite { get; set; }

        [CheckBox, 
         DoesNotAutoMap("This is just a visual aid and used for certifying/recertifying a sample site. Checkout Entitymap for deets.")]
        public virtual bool CertificationAuthorization { get; set; }

        [Required]
        public virtual bool? LeadCopperSite { get; set; }

        [DropDown,
         EntityMap,
         RequiredWhen(nameof(LeadCopperSite), ComparisonType.EqualTo, true),
         EntityMustExist(typeof(SampleSiteLeadCopperValidationMethod)),
         View(SampleSite.Display.LEAD_COPPER_VALIDATION_METHOD)]
        public int? LeadCopperValidationMethod { get; set; }

        [View(SampleSite.Display.LIMS_FACILITY_ID), 
         StringLength(SampleSite.StringLengths.LIMS_FACILITY_ID),
         ClientCallback("SampleSites.validateLimsFacilityId", ErrorMessage = LIMS_FACILITY_ID_VALIDATION_ERROR)]
        public virtual string LimsFacilityId { get; set; }

        [View(SampleSite.Display.LIMS_SITE_ID), 
         StringLength(SampleSite.StringLengths.LIMS_SITE_ID),
         ClientCallback("SampleSites.validateLimsSiteId", ErrorMessage = LIMS_SITE_ID_VALIDATION_ERROR)]
        public virtual string LimsSiteId { get; set; }

        [View(SampleSite.Display.LIMS_PRIMARY_STATION_CODE), 
         StringLength(SampleSite.StringLengths.LIMS_PRIMARY_STATION_CODE),
         ClientCallback("SampleSites.validateLimsPrimaryStationCode", ErrorMessage = LIMS_PRIMARY_STATION_CODE_VALIDATION_ERROR)]
        public virtual string LimsPrimaryStationCode { get; set; }

        public virtual int? LimsSequenceNumber { get; set; }

        [EntityMap, 
         EntityMustExist(typeof(SampleSiteProfile)),
         RequiredWhen(nameof(IsLimsLocation), ComparisonType.EqualTo, true),
         DropDown("WaterQuality", "SampleSiteProfile", "ByPublicWaterSupply", DependsOn = nameof(PublicWaterSupply), PromptText = "Please select a public water supply above")]
        public virtual int? SampleSiteProfile { get; set; }

        [StringLength(SampleSite.StringLengths.STREET_NUMBER)]
        public virtual string StreetNumber { get; set; }

        [StringLength(SampleSite.StringLengths.TOWN), View(Description = "(if outside service area)")]
        [RequiredWhen("OutOfServiceArea", true)]
        public virtual string TownText { get; set; }

        [StringLength(SampleSite.StringLengths.ZIP_CODE)]
        public virtual string ZipCode { get; set; }

        public virtual bool OutOfServiceArea { get; set; }

        [Required,
         StringLength(SampleSite.StringLengths.AGENCY_ID),
         ClientCallback("SampleSites.validateAgencyId", ErrorMessage = AGENCY_ID_VALIDATION_ERROR)]
        public virtual string AgencyId { get; set; }

        [DoesNotAutoMap("Set in MapToEntity to be used by controller.")]
        public bool SendSampleSiteAddedToServiceNotificationOnSave { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SampleSiteLocationType))]
        [RequiredWhen(nameof(BactiSite), ComparisonType.EqualTo, true)]
        public virtual int? LocationType { get; set; }

        [EntityMap,
         EntityMustExist(typeof(SampleSite)),
         RequiredWhen(nameof(LocationType), 
             ComparisonType.EqualToAny, 
             nameof(GetEligibleForParentSiteLocationTypeIds), 
             typeof(SampleSiteViewModel), 
             FieldOnlyVisibleWhenRequired = true),
         DropDown("WaterQuality", "SampleSite", "GetPrimarySampleSitesByStateId", DependsOn = nameof(State), PromptText = "Please select a State above.")]
        public virtual int? ParentSite { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SampleSiteCollectionType)), RequiredWhen(nameof(BactiSite), ComparisonType.EqualTo, true)]
        public virtual int? CollectionType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SampleSiteSubCollectionType))]
        public virtual int? SubCollectionType { get; set; }

        [Required] 
        public virtual bool? IsComplianceSampleSite { get; set; }

        [Required] 
        public virtual bool? IsProcessSampleSite { get; set; }

        [Required] 
        public virtual bool? IsResearchSampleSite { get; set; }

        [Required]
        [StringLength(SampleSite.StringLengths.COMMON_SITE_NAME)]
        public virtual string CommonSiteName { get; set; }

        [Required]
        public virtual bool? IsLimsLocation { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SampleSiteAddressLocationType)), Required]
        public int? SampleSiteAddressLocationType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SampleSiteValidationStatus))]
        public int? SampleSiteValidationStatus { get; set; }

        [DoesNotAutoMap("Not an actual View Property - set by MapToEntity and Used by Controller")]
        public bool SendChemicalSiteCreatedNotificationOnSave { get; set; }

        [DoesNotAutoMap("Not an actual View Property - set by MapToEntity and Used by Controller")]
        public bool SendChemicalSiteStatusChangedNotificationOnSave { get; set; }

        [EntityMap,
         RequiredWhen(nameof(LeadCopperSite), ComparisonType.EqualTo, true),
         EntityMustExist(typeof(SampleSiteLeadCopperTierClassification)),
         DropDown("WaterQuality",
             "SampleSiteLeadCopperTierClassification",
             "ByState",
             DependsOn = nameof(State),
             PromptText = "Please select a state above.")]
        public int? LeadCopperTierClassification { get; set; }

        [StringLength(SampleSite.StringLengths.LEAD_COPPER_TIER_THREE_EXPLANATION),
         RequiredWhen(
             nameof(LeadCopperTierClassification), 
             ComparisonType.EqualTo,
             SampleSiteLeadCopperTierClassification.Indices.TIER_3_SINGLE_FAMILY_RESIDENCES_WITH_COPPER_PIPES_AND_LEAD_SOLDER_INSTALLED_BEFORE_1983,
             FieldOnlyVisibleWhenRequired = true)]
        public string LeadCopperTierThreeExplanation { get; set; }

        [EntityMap, 
         EntityMustExist(typeof(SampleSiteLeadCopperTierSampleCategory)),
         ClientCallback("SampleSites.validateLeadCopperTierSampleCategory", ErrorMessage = "Required when Lead Copper Site is yes."),
         DropDown("WaterQuality",
             "SampleSiteLeadCopperTierSampleCategory",
             "BySampleSiteLeadCopperTierClassification",
             DependsOn = nameof(LeadCopperTierClassification),
             PromptText = "Please select a tier classification above."),]
        public int? LeadCopperTierSampleCategory { get; set; }

        [AutoMap(Direction = MapDirections.ToPrimary), 
         EntityMustExist(typeof(User))]
        public int? CertifiedBy { get; set; }

        [AutoMap(Direction = MapDirections.ToPrimary)]
        public virtual DateTime? CertifiedDate { get; set; }

        [AutoMap(Direction = MapDirections.ToPrimary)]
        public virtual DateTime? NextCertificationDate { get; set; }

        [AutoMap(Direction = MapDirections.ToPrimary)]
        public bool CanBeCertified { get; set; }

        [Required]
        public bool? IsAlternateSite { get; set; }

        public bool? ResourceDistributionMaps { get; set; }

        public bool? ResourceCapitalImprovement { get; set; }

        public bool? ResourceUtilityRecords { get; set; }

        public bool? ResourceSamplingResults { get; set; }

        public bool? ResourceInterviewsPersonnel { get; set; }

        public bool? ResourceCommunitySurvey { get; set; }

        public bool? ResourceCountyAppraisal { get; set; }

        public bool? ResourceContacts { get; set; }

        public bool? ResourceSurveyResults { get; set; }

        public bool? ResourceInterviewsResidents { get; set; }

        public bool? ResourceInterviewsContractors { get; set; }

        public bool? ResourceLeadCheckSwabs { get; set; }

        [View(Description = "Explain")]
        public string OtherSources { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SampleSitePointOfUseTreatmentType))]
        public int? SampleSitePointOfUseTreatmentType { get; set; }

        [Multiline,
         StringLength(SampleSite.StringLengths.POINT_OF_USE_TREATMENT_TYPE_OTHER_REASON),
         RequiredWhen(
             nameof(SampleSitePointOfUseTreatmentType), 
             ComparisonType.EqualTo,
             MapCall.Common.Model.Entities.SampleSitePointOfUseTreatmentType.Indices.OTHER,
             FieldOnlyVisibleWhenRequired = true)]
        public string PointOfUseTreatmentTypeOtherReason { get; set; }

        #endregion

        #region Constructors

        public SampleSiteViewModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private static int[] GetEligibleForParentSiteLocationTypeIds() => SampleSiteLocationType.ELIGIBLE_FOR_PARENT_SITE;

        #endregion

        #region Exposed Methods

        public override SampleSite MapToEntity(SampleSite entity)
        {
            if (IsBeingValidated)
            {
                entity.ValidatedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser.Employee;
                entity.ValidatedAt = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }

            if (IsLimsLocation.GetValueOrDefault())
            {
                switch (State)
                {
                    case MapCall.Common.Model.Entities.State.Indices.CA:
                        LimsSiteId = null;
                        LimsFacilityId = null;
                        break;

                    case MapCall.Common.Model.Entities.State.Indices.PA:
                        LimsFacilityId = null;
                        LimsPrimaryStationCode = null;
                        break;

                    default:
                        LimsPrimaryStationCode = null;
                        break;
                }

                var sampleSiteProfile = _container.GetInstance<IRepository<SampleSiteProfile>>()
                                                  .Find(SampleSiteProfile.GetValueOrDefault());

                if (sampleSiteProfile != null)
                {
                    var isChemicalSampleSite = sampleSiteProfile.SampleSiteProfileAnalysisType.Id == SampleSiteProfileAnalysisType.Indices.CHEMICAL;

                    if (entity.Id == 0)
                    {
                        SendChemicalSiteCreatedNotificationOnSave = isChemicalSampleSite;
                    }
                    else
                    {
                        if (Status != null)
                        {
                            SendChemicalSiteStatusChangedNotificationOnSave = isChemicalSampleSite && entity.Status.Id != Status.Value;
                        }
                    }
                }
            }
            else
            {
                LimsSiteId = null;
                LimsFacilityId = null;
                LimsPrimaryStationCode = null;
                SampleSiteProfile = null;
                LimsSequenceNumber = null;
            }

            switch (SampleSiteAddressLocationType)
            {
                case MapCall.Common.Model.Entities.SampleSiteAddressLocationType.Indices.HYDRANT:
                    Facility = null;
                    Valve = null;
                    break;
                case MapCall.Common.Model.Entities.SampleSiteAddressLocationType.Indices.VALVE:
                    Facility = null;
                    Hydrant = null;
                    break;
                case MapCall.Common.Model.Entities.SampleSiteAddressLocationType.Indices.FACILITY:
                    Valve = null;
                    Hydrant = null;
                    break;
                default:
                    Valve = null;
                    Hydrant = null;
                    Facility = null;
                    break;
            }

            /* Certification Stuffs */
            if (CertificationAuthorization)
            {
                entity.CertifiedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
                entity.CertifiedDate = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            }

            entity.NeedsToSync = true;

            return base.MapToEntity(entity);
        }

        #endregion
        
        #region Validation

        private IEnumerable<ValidationResult> ValidateAgencyId()
        {
            if (Status != SampleSiteStatus.Indices.PENDING && IsComplianceSampleSite == true && string.IsNullOrWhiteSpace(AgencyId))
            {
                yield return new ValidationResult(AGENCY_ID_VALIDATION_ERROR, new[] { nameof(AgencyId) });
            }
        }

        private IEnumerable<ValidationResult> ValidateLeadCopperSite()
        {
            if (LeadCopperSite.GetValueOrDefault() &&
                State == MapCall.Common.Model.Entities.State.Indices.NJ &&
                !LeadCopperTierSampleCategory.HasValue)
            {
                yield return new ValidationResult("Lead Copper Site Category is required.");
            }
        }

        #region Validate Lims Fields

        private IEnumerable<ValidationResult> ValidateLimsPrimaryStationCode()
        {
            // This field is required when the sample site is a compliant lims location and the state is ca

            if (IsComplianceSampleSite.GetValueOrDefault() && 
                IsLimsLocation.GetValueOrDefault() &&
                string.IsNullOrEmpty(LimsPrimaryStationCode) &&
                INVALID_STATES_FOR_LIMS_PRIMARY_STATION_CODE_REQUIRED_VALIDATION.Contains(State.GetValueOrDefault()))
            {
                yield return new ValidationResult(LIMS_PRIMARY_STATION_CODE_VALIDATION_ERROR, new[] { nameof(LimsPrimaryStationCode) });
            }
        }

        private IEnumerable<ValidationResult> ValidateLimsSiteId()
        {
            // This field is required when the sample site is a compliant lims location and the state is not equal to ca

            if (IsComplianceSampleSite.GetValueOrDefault() &&
                IsLimsLocation.GetValueOrDefault() &&
                string.IsNullOrEmpty(LimsSiteId) &&
                !(VALID_STATES_FOR_LIMS_SITE_ID_REQUIRED_VALIDATION.Contains(State.GetValueOrDefault())))
            {
                yield return new ValidationResult(LIMS_SITE_ID_VALIDATION_ERROR, new[] { nameof(LimsSiteId) });
            }
        }

        private IEnumerable<ValidationResult> ValidateLimsFacilityId()
        {
            // This field is required when the sample site is a compliant lims location and the state is not equal to ca, pa

            if (IsComplianceSampleSite.GetValueOrDefault() && 
                IsLimsLocation.GetValueOrDefault() &&
                string.IsNullOrEmpty(LimsFacilityId) &&
                !(VALID_STATES_FOR_LIMS_FACILITY_ID_REQUIRED_VALIDATION.Contains(State.GetValueOrDefault())))
            {
                yield return new ValidationResult(LIMS_FACILITY_ID_VALIDATION_ERROR, new[] { nameof(LimsFacilityId) });
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Union(ValidateLeadCopperSite())
                       .Union(ValidateLimsPrimaryStationCode())
                       .Union(ValidateLimsFacilityId())
                       .Union(ValidateLimsSiteId())
                       .Union(ValidateAgencyId());
        }

        #endregion

        #endregion
    }
}
