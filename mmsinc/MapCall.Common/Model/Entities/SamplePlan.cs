using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SamplePlan : IThingWithDocuments, IThingWithNotes, IEntityLookup
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        [Required]
        public virtual PublicWaterSupply PWSID { get; set; }

        [Required]
        public virtual Employee ContactPerson { get; set; }

        [Required]
        public virtual bool Cws { get; set; }

        [Required]
        public virtual bool Ntnc { get; set; }

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime MonitoringPeriodFrom { get; set; }

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime MonitoringPeriodTo { get; set; }

        [Required]
        public virtual bool Standard { get; set; }

        [Required]
        public virtual bool Reduced { get; set; }

        [Required]
        public virtual int MinimumSamplesRequired { get; set; }

        [Required]
        [StringLength(50)]
        public virtual string NameOfCertifiedLaboratory { get; set; }

        [Required,
         DisplayName(
             "Are the same sampling sites used as in the previous monitoring period? If required by your state, complete and submit the appropriate Lead and Copper Change form for your state (i.e. NJ BSDW-56)")]
        public virtual bool SameAsPreviousPeriod { get; set; }

        [Required, DisplayName("Are all samples from Tier 1 sites?")]
        public virtual bool AllSamplesTier1 { get; set; }

        [Required, DisplayName("If insufficient Tier 1 sites are available, are Tier 2 sites used?")]
        public virtual bool Tier2Sites { get; set; }

        [Required, DisplayName("If insufficient Tier 2 sites are available, are Tier 3 sites used?")]
        public virtual bool Tier3Sites { get; set; }

        [Required,
         DisplayName(
             "Have the Tier 1 sites been verified to meet the requirements of a Tier 1 site? (i.e. documentation that can be provided proving the site meets the requirements)")]
        public virtual bool Tier1SitesVerified { get; set; }

        [Required, DisplayName("Does the system have lead service lines? If yes, write in comments section how many")]
        public virtual bool LeadServiceLines { get; set; }

        [Required,
         DisplayName(
             "Has the system verified which lines are lead service lines? (i.e. visual inspection, record drawings, county appraisal records, interviews with residents, etc.)")]
        public virtual bool LeadLinesVerified { get; set; }

        [Required,
         DisplayName(
             "If the distribution system contains lead service lines, are 50% of the samples collected from sites with lead service lines?")]
        public virtual bool FiftyPercent { get; set; }

        public virtual string Comments { get; set; }

        public virtual int NumberOfSamplesTaken => EnumerateSamples().Count();
        public virtual IList<WaterSample> SamplesTaken => EnumerateSamples().ToList();

        public virtual IList<SampleSite> SampleSitesUsed
        {
            get
            {
                return SamplesTaken.Map<WaterSample, SampleSite>(s => s.SampleIdMatrix.SampleSite).Distinct().ToList();
            }
        }

        public virtual IList<SampleSite> SampleSites { get; set; }

        public virtual IEnumerable<SampleSite> ActiveSampleSites
        {
            get
            {
                return SampleSites?.Where(x =>
                    x.Status?.Id == SampleSiteStatus.Indices.ACTIVE && x.LeadCopperSite == true);
            }
        }

        public virtual bool HasActiveSampleSites { get; set; }

        private IEnumerable<WaterSample> EnumerateSamples()
        {
            if (ActiveSampleSites == null) yield break;
            foreach (var site in ActiveSampleSites)
            {
                foreach (
                    var matrix in
                    site.SampleIdMatrices.Where(
                        m => m.WaterConstituent.Description == "Lead" ||
                             m.WaterConstituent.Description == "Copper")
                )
                {
                    foreach (var sample in matrix.WaterSamples.Where(SampleInMonitoringRangeP))
                    {
                        yield return sample;
                    }
                }
            }
        }

        private bool SampleInMonitoringRangeP(WaterSample s)
        {
            return s.SampleDate >= MonitoringPeriodFrom && s.SampleDate <= MonitoringPeriodTo;
        }

        public virtual IList<SamplePlanNote> PlanNotes { get; set; }
        public virtual IList<SamplePlanDocument> PlanDocuments { get; set; }

        public virtual IList<INoteLink> LinkedNotes => PlanNotes.Map(n => (INoteLink)n);
        public virtual IList<IDocumentLink> LinkedDocuments => PlanDocuments.Map(n => (IDocumentLink)n);

        [DoesNotExport]
        public virtual string TableName => nameof(SamplePlan) + "s";

        public virtual string Description => (new SamplePlanDisplayItem {
            Id = Id,
            PWSID = PWSID,
            MonitoringPeriodFrom = MonitoringPeriodFrom,
            MonitoringPeriodTo = MonitoringPeriodTo
        }).Display;

        #endregion

        #region Constructors

        public SamplePlan()
        {
            PlanNotes = new List<SamplePlanNote>();
            PlanDocuments = new List<SamplePlanDocument>();
            SampleSites = new List<SampleSite>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class SamplePlanDisplayItem : DisplayItem<SamplePlan>
    {
        public PublicWaterSupply PWSID { get; set; }

        public DateTime MonitoringPeriodFrom { get; set; }
        public DateTime MonitoringPeriodTo { get; set; }

        public override string Display =>
            $"{Id} - {PWSID.Description} From {MonitoringPeriodFrom.ToShortDateString()} To {MonitoringPeriodTo.ToShortDateString()} ";
    }
}
