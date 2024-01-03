using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SampleSite : 
        IEntityLookup, 
        IThingWithDocuments, 
        IThingWithNotes, 
        IThingWithCoordinate,
        IThingWithSyncing
    {
        #region Constants

        public const string LONG_DESCRIPTION_FORMAT_STRING = "{0}-{1}-{2}-{3}-{4}-{5}-{6}-{7}";

        public struct SupplementalValidationDescriptions
        {
            public const string
                RESOURCE_DISTRIBUTION_MAPS = "Distribution system maps and record drawings",
                RESOURCE_CAPITAL_IMPROVEMENT = "Capital improvement plans and/or master plans for distribution system development.",
                RESOURCE_UTILITY_RECORDS = "Utility records including meter installation records, customer complaint investigations and all historical documentation which indicate and/or confirm the location of lead service connections",
                RESOURCE_SAMPLING_RESULTS = "Results from service line sampling where lead service lines are suspected to exist but their presence is not confirmed",
                RESOURCE_INTERVIEWS_PERSONNEL = "Documented interviews of senior personnel",
                RESOURCE_COMMUNITY_SURVEY = "Results from community survey",
                RESOURCE_COUNTY_APPRAISAL = "County appraisal district records",
                RESOURCE_CONTACTS = "Contacts within the water system, municipal office or other local officials",
                RESOURCE_SURVEY_RESULTS = "Survey results from area plumbers who are asked about when and where copper pipe with lead solder was used",
                RESOURCE_INTERVIEWS_RESIDENTS = "Documented interviews of residents-letters, phone survey, personal contact, etc.",
                RESOURCE_INTERVIEWS_CONTRACTORS = "Documented interviews of local contractors, developers, and builders",
                RESOURCE_LEAD_CHECK_SWABS = "Lead check swab samples";
        }

        public struct Display
        {
            public const string AGENCY_ID = "Agency Id",
                                SAMPLE_SITE_VALIDATION_STATUS = "Validation",
                                SAMPLE_SITE_ADDRESS_LOCATION_TYPE = "AW Sample Location Type",
                                SAMPLE_SITE_POINT_OF_USE_TREATMENT_TYPE = "Point of Use Treatment",
                                SAMPLE_SITE_INACTIVATION_REASON = "Reason for Inactivation",
                                LEAD_COPPER_SITE = "Lead Copper Compliance Site",
                                LEAD_COPPER_TIER_SAMPLE_CATEGORY = "Sample Category",
                                LEAD_COPPER_TIER_CLASSIFICATION = "Tier Classification",
                                LEAD_COPPER_VALIDATION_METHOD = "Validation Method",
                                LEAD_COPPER_TIER_THREE_EXPLANATION = "Tier 3 Explanation",
                                LIMS_FACILITY_ID = "Facility Id",
                                LIMS_PRIMARY_STATION_CODE = "Primary Station Code",
                                LIMS_SITE_ID = "Site Id",
                                LIMS_SEQUENCE_NUMBER = "Location Sequence Number",
                                SAMPLE_SITE_PROFILE = "Profile",
                                CUSTOMER_CONTACT_METHOD = "Preferred Contact Method",
                                IS_LIMS_LOCATION = "Is LIMS Location",
                                POINT_OF_USE_TREATMENT_TYPE_OTHER_REASON = "Other Reason",
                                LSLR = "Lead Service Line Replacement (LSLR) Site";                    
        }

        public struct StringLengths
        {
            public const int
                AGENCY_ID = 40,
                COMMON_SITE_NAME = 20,
                CUSTOMER_EMAIL = 255,
                CUSTOMER_ALT_PHONE = 20,
                CUSTOMER_HOME_PHONE = 20,
                CUSTOMER_NAME = 255,
                LOCATION_NAME_DESCRIPTION = 40,
                LIMS_FACILITY_ID = 30,
                LIMS_SITE_ID = 20, 
                LIMS_PRIMARY_STATION_CODE = 30,
                LEAD_COPPER_TIER_THREE_EXPLANATION = 255,
                STREET_NUMBER = 50,
                TOWN = 50,
                ZIP_CODE = 10,
                POINT_OF_USE_TREATMENT_TYPE_OTHER_REASON = 1024;
        }

        #endregion

        #region Fields

        [NonSerialized] private IIconSetRepository _iconSetRepository;

        [NonSerialized] private IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }

        public virtual bool BactiSite { get; set; }

        [View(Display.LEAD_COPPER_SITE)]
        public virtual bool? LeadCopperSite { get; set; }

        public virtual string StreetNumber { get; set; }

        public virtual string TownText { get; set; }

        public virtual string ZipCode { get; set; }

        public virtual bool OutOfServiceArea { get; set; }

        [View(Display.AGENCY_ID)]
        public virtual string AgencyId { get; set; }

        [View(Display.LEAD_COPPER_TIER_CLASSIFICATION)]
        public virtual SampleSiteLeadCopperTierClassification LeadCopperTierClassification { get; set; }

        [View(Display.LEAD_COPPER_TIER_SAMPLE_CATEGORY)]
        public virtual SampleSiteLeadCopperTierSampleCategory LeadCopperTierSampleCategory { get; set; }

        [View(Display.LEAD_COPPER_VALIDATION_METHOD)]
        public virtual SampleSiteLeadCopperValidationMethod LeadCopperValidationMethod { get; set; }

        [View(Description = "A regular site is not an alternate site.")]
        public virtual bool IsRegularSite => !IsAlternateSite;

        public virtual bool IsAlternateSite { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_DISTRIBUTION_MAPS)]
        public virtual bool? ResourceDistributionMaps { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_CAPITAL_IMPROVEMENT)]
        public virtual bool? ResourceCapitalImprovement { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_UTILITY_RECORDS)]
        public virtual bool? ResourceUtilityRecords { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_SAMPLING_RESULTS)]
        public virtual bool? ResourceSamplingResults { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_INTERVIEWS_PERSONNEL)]
        public virtual bool? ResourceInterviewsPersonnel { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_COMMUNITY_SURVEY)]
        public virtual bool? ResourceCommunitySurvey { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_COUNTY_APPRAISAL)]
        public virtual bool? ResourceCountyAppraisal { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_CONTACTS)]
        public virtual bool? ResourceContacts { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_SURVEY_RESULTS)]
        public virtual bool? ResourceSurveyResults { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_INTERVIEWS_RESIDENTS)]
        public virtual bool? ResourceInterviewsResidents { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_INTERVIEWS_CONTRACTORS)]
        public virtual bool? ResourceInterviewsContractors { get; set; }

        [View(SupplementalValidationDescriptions.RESOURCE_LEAD_CHECK_SWABS)]
        public virtual bool? ResourceLeadCheckSwabs { get; set; }

        [View(Description = "Explain")]
        public virtual string OtherSources { get; set; }

        public virtual bool? PreviousMonitoringPeriod { get; set; }

        public virtual bool? CustomerParticipationConfirmed { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string CustomerHomePhone { get; set; }
        public virtual string CustomerAltPhone { get; set; }
        public virtual string CustomerEmail { get; set; }

        [View(Display.POINT_OF_USE_TREATMENT_TYPE_OTHER_REASON)]
        public virtual string PointOfUseTreatmentTypeOtherReason { get; set; }

        [View(Display.CUSTOMER_CONTACT_METHOD)]
        public virtual SampleSiteCustomerContactMethod CustomerContactMethod { get; set; }

        public virtual bool? FieldVerifiedServiceMaterial { get; set; }
        public virtual bool? FieldVerifiedServiceCustomerSideMaterial { get; set; }
        public virtual bool? FieldVerifiedCustomerPlumbingMaterial { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? CertifiedDate { get; set; }

        /* this is a formula property */
        public virtual bool IsCertified { get; protected set; }

        public virtual DateTime? ValidatedAt { get; set; }
        public virtual Employee ValidatedBy { get; set; }
        public virtual bool? IsComplianceSampleSite { get; set; }
        public virtual bool? IsProcessSampleSite { get; set; }
        public virtual bool? IsResearchSampleSite { get; set; }
        public virtual string CommonSiteName { get; set; }
        public virtual string LocationNameDescription { get; set; }

        [View(Display.IS_LIMS_LOCATION)]
        public virtual bool? IsLimsLocation { get; set; }

        public virtual bool NeedsToSync { get; set; }
        public virtual DateTime? LastSyncedAt { get; set; }
        
        [View(Display.LEAD_COPPER_TIER_THREE_EXPLANATION)]
        public virtual string LeadCopperTierThreeExplanation { get; set; }

        [View(Display.LIMS_FACILITY_ID)]
        public virtual string LimsFacilityId { get; set; }

        [View(Display.LIMS_SITE_ID)]
        public virtual string LimsSiteId { get; set; }

        [View(Display.LIMS_PRIMARY_STATION_CODE)]
        public virtual string LimsPrimaryStationCode { get; set; }

        [View(Display.SAMPLE_SITE_PROFILE)]
        public virtual SampleSiteProfile SampleSiteProfile { get; set; }
        
        [View(Display.LIMS_SEQUENCE_NUMBER)]
        public virtual int? LimsSequenceNumber { get; set; }

        [View(Display.LSLR)]
        public virtual bool? LeadServiceLineReplacementSite { get; set; }

        #endregion

        #region References

        public virtual State State { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual PublicWaterSupply PublicWaterSupply { get; set; }
        public virtual SampleSiteAvailability Availability { get; set; }
        public virtual SampleSiteStatus Status { get; set; }
        public virtual Premise Premise { get; set; }
        public virtual IList<SampleIdMatrix> SampleIdMatrices { get; set; }
        public virtual County County { get; set; }
        public virtual Facility Facility { get; set; }
        public virtual Hydrant Hydrant { get; set; }
        public virtual Valve Valve { get; set; }
        public virtual Gradient Gradient { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual Town Town { get; set; }
        public virtual TownSection TownSection { get; set; }
        public virtual IList<Document<SampleSite>> SampleSiteDocuments { get; set; } = new List<Document<SampleSite>>();
        public virtual IList<Note<SampleSite>> SampleSiteNotes { get; set; } = new List<Note<SampleSite>>();

        public virtual Street Street { get; set; }
        public virtual Street CrossStreet { get; set; }
        public virtual ServiceMaterial CustomerPlumbingMaterial { get; set; }
        public virtual IList<SamplePlan> SamplePlans { get; set; } = new List<SamplePlan>();
        public virtual User CertifiedBy { get; set; }
        public virtual SampleSiteCollectionType CollectionType { get; set; }
        public virtual SampleSiteSubCollectionType SubCollectionType { get; set; }
        public virtual SampleSiteLocationType LocationType { get; set; }
        public virtual SampleSite ParentSite { get; set; }
        public virtual IList<SampleSite> ChildSites { get; set; } = new List<SampleSite>();

        [View(Display.SAMPLE_SITE_POINT_OF_USE_TREATMENT_TYPE)]
        public virtual SampleSitePointOfUseTreatmentType SampleSitePointOfUseTreatmentType { get; set; }

        [View(Display.SAMPLE_SITE_ADDRESS_LOCATION_TYPE)]
        public virtual SampleSiteAddressLocationType SampleSiteAddressLocationType { get; set; }

        [View(Display.SAMPLE_SITE_VALIDATION_STATUS)]
        public virtual SampleSiteValidationStatus SampleSiteValidationStatus { get; set; }

        [View(Display.SAMPLE_SITE_INACTIVATION_REASON)]
        public virtual SampleSiteInactivationReason SampleSiteInactivationReason { get; set; }

        #endregion

        #region Logical Properties

        public virtual string FullAddress => Premise != null ? Premise.FullStreetAddress : string.Empty;

        public virtual string Description => new SampleSiteDisplayItem {
            TownText = TownText,
            Town = Town?.ShortName,
            Facility = Facility,
            CommonSiteName = CommonSiteName,
            LocationNameDescription = LocationNameDescription
        }.Display;

        public virtual string LongDescription =>
            string.Format(LONG_DESCRIPTION_FORMAT_STRING,
                OperatingCenter != null ? OperatingCenter.OperatingCenterCode : string.Empty,
                PublicWaterSupply != null ? PublicWaterSupply.Identifier : string.Empty,
                County != null ? County.Name : string.Empty,
                Town != null ? Town.ShortName : string.Empty,
                CommonSiteName,
                StreetNumber,
                BactiSite == true ? "Bacti" : "Non-Bacti",
                Status != null ? Status.ToString() : string.Empty);

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return SampleSiteNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return SampleSiteDocuments.Map(d => (IDocumentLink)d); }
        }

        [DoesNotExport]
        public virtual string TableName => nameof(SampleSite) + "s";

        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual decimal? Longitude => Coordinate?.Longitude;

        [DoesNotExport]
        public virtual MapIcon Icon
        {
            get
            {
                if (Coordinate == null)
                {
                    return null;
                }

                return LeadCopperSite.HasValue && LeadCopperSite.Value ? GetIconForLeadCopperSite() : Coordinate.Icon;
            }
        }

        [DoesNotExport]
        public virtual string AllTheCodes
        {
            get
            {
                var serviceMaterialCode = Premise?.MostRecentService?.Service.ServiceMaterial?.Code;
                var customerSideMaterialCode = Premise?.MostRecentService?.Service?.CustomerSideMaterial?.Code;
                var customerPlumbingMaterialCode = CustomerPlumbingMaterial?.Code;
                return $"{serviceMaterialCode},{customerSideMaterialCode},{customerPlumbingMaterialCode}";
            }
        }

        public virtual IList<SampleSiteBracketSite> BracketSites { get; set; } = new List<SampleSiteBracketSite>();

        [SetterProperty]
        public virtual IIconSetRepository IconSetRepository
        {
            set => _iconSetRepository = value;
        }

        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        [SetterProperty]
        public virtual IDateTimeProvider DateTimeProvider
        {
            set => _dateTimeProvider = value;
        }

        public virtual bool CanBeCertified => !CertifiedDate.HasValue || _dateTimeProvider.GetCurrentDate().Date >= NextCertificationDate;

        [View(FormatStyle.Date)]
        public virtual DateTime NextCertificationDate => !CertifiedDate.HasValue ? _dateTimeProvider.GetCurrentDate() : CertifiedDate.Value.Date.AddYears(3);

        public virtual string Notes => string.Join("\r\n", SampleSiteNotes.Select(n => n.Note.Text));

        public virtual string ServiceMaterialUsed => Premise?.MostRecentService?.Service.ServiceMaterial?.Description;

        public virtual string CustomerSideMaterialUsed => Premise?.MostRecentService?.Service?.CustomerSideMaterial?.Description;
        
        #endregion

        #endregion

        #region Private Methods

        private MapIcon GetIconForLeadCopperSite()
        {
            if (Status == null)
            {
                return Coordinate.Icon;
            }

            var iconSet = _iconSetRepository.Find(IconSets.Beakers);
            if (IsAlternateSite == true && Status.Id != SampleSiteStatus.Indices.INACTIVE)
            {
                return iconSet.Icons.Single(x => x.FileName.Contains("yellow"));
            }

            switch (Status.Id)
            {
                case SampleSiteStatus.Indices.ACTIVE:
                    return iconSet.Icons.Single(x => x.FileName.Contains("green"));
                case SampleSiteStatus.Indices.INACTIVE:
                    return iconSet.Icons.Single(x => x.FileName.Contains("black"));
                // return a default icon instead of causing the entire map to error due to any unhandled cases
                default:
                    return iconSet.Icons.Single(x => x.FileName.Contains("red"));
            }
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString() => Description;

        #endregion
    }
}
