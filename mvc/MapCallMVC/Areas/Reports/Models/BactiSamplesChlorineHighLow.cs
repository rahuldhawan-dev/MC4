using System.ComponentModel;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchBactiSamplesChlorineHighLow : SearchSet<BacterialWaterSample>, ISearchBactiSamplesChlorineHighLow
    {
        #region Properties

        // NOTE: SearchAlias must be defined in the interface for this model due to the
        // BacterialWaterSampleRepository.SearchBactiSamplesChlorineHighLowReport method's additional criteria.

        [DropDown]
        public int? PublicWaterSupply { get; set; }
        public DateRange SampleCollectionDTM { get; set; }
        [MultiSelect]
        public int[] Town { get; set; }

        #endregion
    }

    public class SearchBacterialWaterSampleRequirement : SearchSet<BacterialWaterSampleRequirementViewModel>, ISearchBacterialWaterSampleRequirementViewModel
    {
        [Search(CanMap = false)]
        public bool? OnlyWithSamples { get; set; }
        [MultiSelect]
        public int[] Year { get; set; }
        [MultiSelect]
        public int[] BacterialSampleType { get; set; }
        [DisplayName("AW Owned?")]
        [SearchAlias("sampleSite.PublicWaterSupply", "publicWaterSupply", "AWOwned")]
        public bool? AWOwned { get; set; }
        [DropDown]
        [SearchAlias("sampleSite.PublicWaterSupply", "publicWaterSupply", "State.Id")]
        public int? State { get; set; }
    }
}