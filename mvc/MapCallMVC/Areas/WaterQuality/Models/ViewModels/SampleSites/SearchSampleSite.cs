using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels.SampleSites
{
    public class SearchSampleSite : SearchSet<SampleSite>
    {
        #region Properties

        [View("MapCall Id")]
        public virtual int? EntityId { get; set; }

        [DropDown,
         EntityMap,
         EntityMustExist(typeof(SampleSiteLocationType))]
        public virtual int? LocationType { get; set; }

        [DropDown,
         EntityMap,
         EntityMustExist(typeof(SampleSiteAddressLocationType)),
         View(SampleSite.Display.SAMPLE_SITE_ADDRESS_LOCATION_TYPE)]
        public virtual int? SampleSiteAddressLocationType { get; set; }

        [EntityMap,
         EntityMustExist(typeof(SampleSite)),
         DropDown("WaterQuality", "SampleSite", "GetPrimaryEligibleSampleSitesByStateId", DependsOn = nameof(State), PromptText = "Please select a State above.")]
        public virtual int? ParentSite { get; set; }

        [MultiSelect("", "OperatingCenter", "ByStateIdForWaterQualityGeneral", DependsOn = nameof(State), PromptText = "Please select a State above.")]
        public virtual int[] OperatingCenter { get; set; }

        [DropDown("",
            "PublicWaterSupply",
            "ByOperatingCenterId",
            DependsOn = nameof(OperatingCenter),
            PromptText = "Please select an operating center above.")]
        public virtual int? PublicWaterSupply { get; set; }

        [DropDown, EntityMustExist(typeof(SampleSiteAvailability))]
        public virtual int? Availability { get; set; }

        [MultiSelect, EntityMustExist(typeof(SampleSiteStatus))]
        public virtual int[] Status { get; set; }

        [DropDown, 
         EntityMap,
         EntityMustExist(typeof(SampleSiteInactivationReason))]
        public virtual int? SampleSiteInactivationReason { get; set; }

        [DropDown("", "County", "ByStateId", DependsOn = "State")]
        [EntityMustExist(typeof(County))]
        public virtual int? County { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public virtual int? State { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterIds", DependsOn = nameof(OperatingCenter))]
        [EntityMustExist(typeof(Facility))]
        public virtual int? Facility { get; set; }

        [DropDown("FieldOperations", "Hydrant", "ByTownId", DependsOn = nameof(Town), PromptText = "Please select a town below.")]
        public virtual int? Hydrant { get; set; }

        [DropDown("FieldOperations", "Valve", "ByTownId", DependsOn = nameof(Town), PromptText = "Please select a town below.")]
        public virtual int? Valve { get; set; }

        [DropDown("", "Gradient", "ByTownId", DependsOn = nameof(Town), PromptText = "Please select a town above.")]
        [EntityMustExist(typeof(Gradient))]
        public int? Gradient { get; set; }

        [MultiSelect("", "Town", "ByOperatingCenterIds", DependsOn = nameof(OperatingCenter))]
        [EntityMustExist(typeof(Town))]
        public virtual int[] Town { get; set; }

        [DropDown("", "TownSection", "ByTownId", DependsOn = nameof(Town))]
        [EntityMustExist(typeof(TownSection))]
        public virtual int? TownSection { get; set; }

        [StringLength(SampleSite.StringLengths.COMMON_SITE_NAME)]
        public virtual string CommonSiteName { get; set; }

        public virtual bool? BactiSite { get; set; }

        [View(SampleSite.Display.LEAD_COPPER_SITE)]
        public virtual bool? LeadCopperSite { get; set; }

        public virtual bool? IsCertified { get; set; }

        [View("Sample Site Id")]
        public virtual string AgencyId { get; set; }

        [StringLength(SampleSite.StringLengths.STREET_NUMBER), View("Street Number")]
        public virtual string StreetNumber { get; set; }

        [EntityMap, 
         EntityMustExist(typeof(Street)),
         DropDown("Street", "ByTownId", DependsOn = nameof(Town), PromptText = "Please select a town above.", Area = "")]
        public virtual int? Street { get; set; }

        [EntityMap, 
         EntityMustExist(typeof(Street)),
         DropDown("Street", "ByTownId", DependsOn = nameof(Town), PromptText = "Please select a town above.", Area = "")]
        public virtual int? CrossStreet { get; set; }

        [StringLength(SampleSite.StringLengths.TOWN), View("Town (string)")]
        public virtual string TownText { get; set; }

        [StringLength(SampleSite.StringLengths.ZIP_CODE)]
        public virtual string ZipCode { get; set; }

        public virtual bool? OutOfServiceArea { get; set; }
        public virtual bool? PreviousMonitoringPeriod { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(SampleSiteCollectionType))]
        public virtual int[] CollectionType { get; set; }
        
        [SearchAlias("Premise", "p", "PremiseNumber")]
        public virtual SearchString PremiseNumber { get; set; }

        [DropDown, 
         EntityMap, 
         EntityMustExist(typeof(SampleSiteLeadCopperTierClassification)),
         View(SampleSite.Display.LEAD_COPPER_TIER_CLASSIFICATION)]
        public virtual int? LeadCopperTierClassification { get; set; }

        [DropDown,
         EntityMap,
         EntityMustExist(typeof(SampleSiteLeadCopperTierSampleCategory)),
         View(SampleSite.Display.LEAD_COPPER_TIER_SAMPLE_CATEGORY)]
        public virtual int? LeadCopperTierSampleCategory { get; set; }

        [DropDown, 
         EntityMap, 
         EntityMustExist(typeof(SampleSiteCustomerContactMethod)),
         View(SampleSite.Display.CUSTOMER_CONTACT_METHOD)]
        public virtual int? CustomerContactMethod { get; set; }

        [DropDown, 
         EntityMap, 
         EntityMustExist(typeof(SampleSiteLeadCopperValidationMethod)), 
         View(SampleSite.Display.LEAD_COPPER_VALIDATION_METHOD)]
        public virtual int? LeadCopperValidationMethod { get; set; }

        [DropDown, 
         EntityMap, 
         EntityMustExist(typeof(ServiceMaterial)),
         SearchAlias("s.ServiceMaterial", "sm", "Id")]
        public virtual int? ServiceMaterial { get; set; }

        [DropDown, 
         EntityMap, 
         EntityMustExist(typeof(ServiceMaterial)),
         SearchAlias("s.CustomerSideMaterial", "csm", "Id")]
        public virtual int? CustomerSideMaterial { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(ServiceMaterial))]
        public virtual int? CustomerPlumbingMaterial { get; set; }

        public virtual bool? FieldVerifiedServiceMaterial { get; set; }
        public virtual bool? FieldVerifiedServiceCustomerSideMaterial { get; set; }
        public virtual bool? FieldVerifiedCustomerPlumbingMaterial { get; set; }

        [DropDown, SearchAlias("SamplePlans", "Id")]
        public virtual int? SamplePlan { get; set; }

        public virtual bool? IsAlternateSite { get; set; }

        public virtual bool? CustomerParticipationConfirmed { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SampleSiteValidationStatus))]
        [View(SampleSite.Display.SAMPLE_SITE_VALIDATION_STATUS)]
        public int? SampleSiteValidationStatus { get; set; }

        [View(SampleSite.Display.IS_LIMS_LOCATION)]
        public virtual bool? IsLimsLocation { get; set; }

        [View(SampleSite.Display.LIMS_FACILITY_ID)]
        public virtual string LimsFacilityId { get; set; }

        [View(SampleSite.Display.LIMS_SITE_ID)]
        public virtual string LimsSiteId { get; set; }

        [View(SampleSite.Display.LIMS_PRIMARY_STATION_CODE)]
        public virtual string LimsPrimaryStationCode { get; set; }

        [View(SampleSite.Display.LIMS_SEQUENCE_NUMBER)]
        public virtual int? LimsSequenceNumber { get; set; }

        [EntityMap,
         EntityMustExist(typeof(SampleSiteProfile)),
         DropDown("WaterQuality", "SampleSiteProfile", "ByPublicWaterSupply", DependsOn = nameof(PublicWaterSupply), PromptText = "Please select a public water supply above")]
        public virtual int? SampleSiteProfile { get; set; }

        [View(SampleSite.Display.LSLR)]
        public virtual bool? LeadServiceLineReplacementSite { get; set; }

        #endregion
    }
}