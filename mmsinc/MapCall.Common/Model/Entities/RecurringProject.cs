using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class RecurringProject
        : IEntityWithCreationTracking<User>,
            IValidatableObject,
            IThingWithDocuments,
            IThingWithNotes,
            IThingWithCoordinate
    {
        #region Constants

        public const string SERVICE_PERIOD_FORMAT = "{0}Q{1}",
                            ASSET_INVESTMENT_CATEGORY_HELP =
                                @"<a href='https://mapcall.amwater.com/Modules/Engineering/DSICReinvestmentCategories.pdf' target='_new'>Click here</a> for help on this topic.";

        public struct LookupTypeDescriptions
        {
            public const string DECADE_INSTALLED = "Decade Installed",
                                PIPE_DIAMETER = "Pipe Diameter",
                                PIPE_MATERIAL = "Pipe Material";
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        [DoesNotExport]
        public virtual MapIcon Icon { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }
        public virtual RecurringProjectType RecurringProjectType { get; set; }
        public virtual PipeDiameter ProposedDiameter { get; set; }
        public virtual PipeMaterial ProposedPipeMaterial { get; set; }

        [Description(ASSET_INVESTMENT_CATEGORY_HELP)]
        public virtual AssetInvestmentCategory AcceleratedAssetInvestmentCategory { get; set; }

        [Description(ASSET_INVESTMENT_CATEGORY_HELP)]
        public virtual AssetInvestmentCategory SecondaryAssetInvestmentCategory { get; set; }

        public virtual RecurringProjectRegulatoryStatus RegulatoryStatus { get; set; }

        [Description("Will always be set to COMPLETE if an Actual In Service Date is entered")]
        public virtual RecurringProjectStatus Status { get; set; }

        public virtual Coordinate Coordinate { get; set; }
        public virtual FoundationalFilingPeriod FoundationalFilingPeriod { get; set; }
        public virtual AssetCategory AssetCategory { get; set; }
        public virtual AssetType AssetType { get; set; }
        public virtual PipeDiameter CorrectDiameter { get; set; }
        public virtual PipeMaterial CorrectMaterial { get; set; }

        public virtual string ProjectTitle { get; set; }
        public virtual string ProjectDescription { get; set; }
        public virtual int? District { get; set; }
        public virtual int? OriginationYear { get; set; }
        public virtual string HistoricProjectID { get; set; }

        [DisplayName("NJAW Estimate (Dollars)"),
         DisplayFormat(DataFormatString = CommonStringFormats.CURRENCY_NO_DECIMAL)]
        public virtual int NJAWEstimate { get; set; }

        [DisplayName("Proposed Length (ft)")]
        public virtual int? ProposedLength { get; set; }

        [DisplayName("CPS Ref #")]
        public virtual string CPSReferenceId { get; set; }

        public virtual string Justification { get; set; }

        [DisplayName("Estimated Project Duration (days)")]
        public virtual int? EstimatedProjectDuration { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? EstimatedInServiceDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ActualInServiceDate { get; set; }

        [View(DisplayName = "Secondary Criteria Score",
            Description = "Once approved the final criteria score will be calculated and locked.")]
        public virtual decimal? FinalCriteriaScore { get; set; }

        [View(DisplayName = "Secondary Raw Score",
            Description = "Once approved the final raw score will be calculated and locked.")]
        public virtual decimal? FinalRawScore { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual string WBSNumber { get; set; }
        public virtual string ExistingPipeMaterialOverride { get; set; }
        public virtual int? DecadeInstalledOverride { get; set; }
        public virtual decimal? ExistingDiameterOverride { get; set; }
        public virtual DateTime? CorrectInstallationDate { get; set; }

        public virtual IList<RecurringProjectEndorsement> ProjectEndorsements { get; set; }
        public virtual IList<HighCostFactor> HighCostFactors { get; set; }

        public virtual IList<RecurringProjectMain> RecurringProjectMains { get; set; }

        /// <summary>
        /// This is weird, why are there properties to hold both of these?
        /// 1. Is so that they can be populated when a new record is created. 
        /// 2. Is so that they can be displayed with an edit link.
        /// </summary>
        public virtual IList<PipeDataLookupValue> PipeDataLookupValues { get; set; }

        public virtual IList<RecurringProjectPipeDataLookupValue> RecurringProjectsPipeDataLookupValues { get; set; }
        public virtual IList<GISDataInaccuracyType> GISDataInaccuracies { get; set; }
        public virtual IList<WorkOrder> MainBreakOrders { get; set; }

        #endregion

        #region Logical Properties

        #region Documents/Notes

        [DoesNotExport]
        public virtual string TableName => nameof(RecurringProject) + "s";

        public virtual IList<Document<RecurringProject>> Documents { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();
        public virtual IList<Note<RecurringProject>> Notes { get; set; }
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        /// <summary>
        /// 2013Q4
        /// </summary>
        public virtual string EstimatedInServicePeriod
        {
            get
            {
                if (EstimatedInServiceDate.HasValue)
                {
                    return String.Format("{0}Q{1}",
                        EstimatedInServiceDate.Value.Year,
                        EstimatedInServiceDate.Value.GetQuarter());
                }

                return string.Empty;
            }
        }

        [View(Description = "Largest linked main's decade installed. Override if blank.")]
        public virtual int? MainsDecadeInstalled
        {
            get
            {
                if (!RecurringProjectMains.Any())
                    return null;
                var largestPipe = RecurringProjectMains?.OrderByDescending(x => x.Length).First();
                return largestPipe?.DateInstalled?.Decade();
            }
        }

        [View(Description = "Largest linked main's diameter. Override if blank.")]
        public virtual decimal? MainsExistingDiameter
        {
            get
            {
                if (!RecurringProjectMains.Any())
                    return null;
                var largestPipe = RecurringProjectMains?.OrderByDescending(x => x.Length).First();
                return largestPipe?.Diameter;
            }
        }

        [View(Description = "Largest linked main's material. Override if blank.")]
        public virtual string MainsExistingPipeMaterial
        {
            get
            {
                if (!RecurringProjectMains.Any())
                    return null;
                var largestPipe = RecurringProjectMains.OrderByDescending(x => x.Length).First();
                return largestPipe == null ? string.Empty : largestPipe.Material;
            }
        }

        [View(DisplayName = "Decade Installed")]
        public virtual int? DecadeInstalled => DecadeInstalledOverride ?? MainsDecadeInstalled;

        [View(DisplayName = "Existing DIameter")]
        public virtual decimal? ExistingDiameter => ExistingDiameterOverride ?? MainsExistingDiameter;

        [View(DisplayName = "Existing Pipe Material")]
        public virtual string ExistingPipeMaterial => ExistingPipeMaterialOverride ?? MainsExistingPipeMaterial;

        public virtual bool RequiresScoring { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal EstimatedVariableScore
        {
            get
            {
                return (PipeDataLookupValues.Any(x => x.VariableScore > 0))
                    ? decimal.Round(PipeDataLookupValues.Where(x => x.VariableScore > 0).Average(x => x.VariableScore),
                        2)
                    : 0;
            }
        }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public virtual decimal EstimatedPriorityWeightedScore
        {
            get
            {
                return (PipeDataLookupValues.Any(x => x.VariableScore > 0))
                    ? decimal.Round(
                        PipeDataLookupValues.Where(x => x.VariableScore > 0).Average(x => x.PriorityWeightedScore), 2)
                    : 0;
            }
        }

        // These InfoMaster fields have to come after the EstimatedPriorityWeightedScore in the excel export.
        public virtual bool? OverrideInfoMasterDecision { get; set; }
        public virtual OverrideInfoMasterReason OverrideInfoMasterReason { get; set; }
        public virtual string OverrideInfoMasterJustification { get; set; }
        public virtual string TotalInfoMasterScore { get; set; }

        public virtual bool HasMainsSelected { get; set; }

        public virtual decimal? Latitude => Coordinate?.Latitude;
        public virtual decimal? Longitude => Coordinate?.Longitude;

        #endregion

        #endregion

        #region Constructors

        public RecurringProject()
        {
            ProjectEndorsements = new List<RecurringProjectEndorsement>();
            HighCostFactors = new List<HighCostFactor>();
            PipeDataLookupValues = new List<PipeDataLookupValue>();
            RecurringProjectMains = new List<RecurringProjectMain>();
            GISDataInaccuracies = new List<GISDataInaccuracyType>();
            MainBreakOrders = new List<WorkOrder>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
