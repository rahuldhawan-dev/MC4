using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Metadata;
using MapCall.Common.Model.ViewModels;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class PublicWaterSupplyViewModel : ViewModel<PublicWaterSupply>
    {
        #region Properties

        [StringLength(PublicWaterSupply.StringLengths.OPERATING_AREA)]
        public string OperatingArea { get; set; }
        [StringLength(PublicWaterSupply.StringLengths.SYSTEM)]
        public string System { get; set; }
        [StringLength(PublicWaterSupply.StringLengths.PWSID)]
        public string Identifier { get; set; }
        [Coordinate(IconSet = IconSets.SingleDefaultIcon), EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        public int? Coordinate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupplyStatus))]
        public int? Status { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupplyOwnership))]
        public int? Ownership { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupplyType))]
        public int? Type { get; set; }
        [Max(9999)]
        public int? LIMSProfileNumber { get; set; }

        // Required for LIMS.
        [Required, StringLength(PublicWaterSupply.StringLengths.LOCAL_CERTIFIED_STATE_ID)]
        public string LocalCertifiedStateId { get; set; }

        public bool? AWOwned { get; set; }

        [DropDown, EntityMustExist(typeof(State)), EntityMap]
        public int? State { get; set; }

        public int? JanuaryRequiredBacterialWaterSamples { get; set; }
        public int? FebruaryRequiredBacterialWaterSamples { get; set; }
        public int? MarchRequiredBacterialWaterSamples { get; set; }
        public int? AprilRequiredBacterialWaterSamples { get; set; }
        public int? MayRequiredBacterialWaterSamples { get; set; }
        public int? JuneRequiredBacterialWaterSamples { get; set; }
        public int? JulyRequiredBacterialWaterSamples { get; set; }
        public int? AugustRequiredBacterialWaterSamples { get; set; }
        public int? SeptemberRequiredBacterialWaterSamples { get; set; }
        public int? OctoberRequiredBacterialWaterSamples { get; set; }
        public int? NovemberRequiredBacterialWaterSamples { get; set; }
        public int? DecemberRequiredBacterialWaterSamples { get; set; }

        [DisplayName("Usage Last Year (kgal)")]
        public int? UsageLastYear { get; set; }

        public bool FreeChlorineReported { get; set; }
        public bool TotalChlorineReported { get; set; }
        [RequiredWhen(nameof(Status),PublicWaterSupplyStatus.Indices.PENDING)]
        public DateTime? AnticipatedActiveDate { get; set; }
        [Required]
        public bool? HasConsentOrder { get; set; }
        public DateTime? DateOfOwnership { get; set; }
        [RequiredWhen(nameof(HasConsentOrder), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        public DateTime? ConsentOrderStartDate { get; set; }
        [RequiredWhen(nameof(ConsentOrderStartDate), ComparisonType.NotEqualTo, null, FieldOnlyVisibleWhenRequired = true)]
        public DateTime? ConsentOrderEndDate { get; set; }
        public DateTime? NewSystemInitialSafetyAssessmentCompleted { get; set; }
        public DateTime? DateSafetyAssessmentActionItemsCompleted { get; set; }
        public DateTime? NewSystemInitialWQEnvAssessmentCompleted { get; set; }
        public DateTime? DateWQEnvAssessmentActionItemsCompleted { get; set; }

        #endregion

        #region Constructors

        public PublicWaterSupplyViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreatePublicWaterSupply : PublicWaterSupplyViewModel
    {
        #region Constructors

		public CreatePublicWaterSupply(IContainer container) : base(container) {}

        #endregion

        public override void SetDefaults()
        {
            base.SetDefaults();
            // MC-547: This should be true by default. Not that this is mentioned in the ticket, it's just in my notes. -Ross 8/1/2018
            FreeChlorineReported = true;
        }
    }

    public class EditPublicWaterSupply : PublicWaterSupplyViewModel
    {
        #region Properties

        [RequiredWhen(nameof(Status), PublicWaterSupplyStatus.Indices.PENDING_MERGER)]
        public DateTime? AnticipatedMergerDate { get; set; }
        public DateTime? ValidTo { get; set; }
        public DateTime? ValidFrom { get; set; }
        [RequiredWhen(nameof(Status), PublicWaterSupplyStatus.Indices.PENDING_MERGER)]
        [DropDown("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = nameof(OperatingCenter)), EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        public int? AnticipatedMergePublicWaterSupply { get; set; }

        [DoesNotAutoMap("Done manually in Map. Needed for cascading.")]
        public int[] OperatingCenter { get; set; }
        
        [DoesNotAutoMap("Done manually in Map. Needed for cascading.")]
        public int[] PlanningPlant { get; set; }

        #endregion

        #region Constructors

        public EditPublicWaterSupply(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void Map(PublicWaterSupply entity)
        {
            base.Map(entity);

            // OperatingCenter isn't an editable field on the page. Those are added
            // from a tab on the Show page. But this is needed for cascading.
            OperatingCenter = entity.OperatingCenterPublicWaterSupplies.Select(x => x.OperatingCenter.Id).ToArray();
            PlanningPlant = entity.PlanningPlantPublicWaterSupplies.Select(x => x.PlanningPlant.Id).ToArray();
        }

        #endregion
    }

    public class SearchPublicWaterSupply : SearchSet<PublicWaterSupply>
    {
        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("OperatingCenterPublicWaterSupplies", "OperatingCenter.Id")]
        public int? OperatingCenter { get; set; }
        public string OperatingArea { get; set; }
        public string System { get; set; }
        public bool? AWOwned { get; set; }
        [DisplayName("PWSID")]
        [AutoComplete("PublicWaterSupply", "ByPartialPWSIDMatch")]
        public string Identifier { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupplyStatus))]
        public int? Status { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupplyOwnership))]
        public int? Ownership { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(PublicWaterSupplyType))]
        public int? Type { get; set; }
       
        public bool? HasConsentOrder { get; set; }
        
        [View(FormatStyle.Date)]
        public DateRange DateOfOwnership { get; set; }
        
        [View(FormatStyle.Date)]
        public DateRange ConsentOrderStartDate { get; set; }
        
        [View(FormatStyle.Date)]
        public DateRange ConsentOrderEndDate { get; set; }
        
        [View(FormatStyle.Date)]
        public DateRange NewSystemInitialSafetyAssessmentCompleted { get; set; }

        [View(FormatStyle.Date)]
        public DateRange NewSystemInitialWQEnvAssessmentCompleted { get; set; }

        #endregion
    }

    // This is here because the report is based on PublicWaterSupply and uses PublicWaterSupplyRepository.
    public class SearchYearlyWaterSampleComplianceReport : SearchSet<YearlyWaterSampleComplianceReportItem>, ISearchYearlyWaterSampleComplianceReport
    {
        // NOTE: SearchAliases are in the ISearchYearlyWaterSampleComplianceReport interface

        #region Properties

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [MultiSelect("", "OperatingCenter", "ByStateId", DependsOn = nameof(State)), EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }

        [View("Public Water Supply")]
        [MultiSelect("", "PublicWaterSupply", "ByOperatingCenterIdsAndAWOwned", DependsOn = nameof(OperatingCenter))]
        public int[] EntityId { get; set; }

        [Required, Min(2000)] // They must put in a four digit year and we didn't start doing this until 2018. 
        public int? CertifiedYear { get; set; }

        #endregion
    }
}