using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchBactiSamplesChlorineHighLow : ISearchSet<BacterialWaterSample>
    {
        #region Properties

        [SearchAlias("SampleSite", "SS", "PublicWaterSupply.Id",
            Required = true)] // alias is required for BacterialWaterSampleRepository.SearchBactiSamplesChlorineHighLowReport
        int? PublicWaterSupply { get; set; }

        DateRange SampleCollectionDTM { get; set; }

        [SearchAlias("SampleSite", "SS", "Town.Id",
            Required = true)] // alias is required for BacterialWaterSampleRepository.SearchBactiSamplesChlorineHighLowReport
        int[] Town { get; set; }

        #endregion
    }
}
