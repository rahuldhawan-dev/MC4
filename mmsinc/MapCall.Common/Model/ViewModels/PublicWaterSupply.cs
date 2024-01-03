using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.ViewModels
{
    // NOTE: If you add another search field to this then you need to make sure its
    // value is copied in PublicWaterSupplyRepository.SearchYearlyWaterSampleComplianceReport
    public interface IBasicYearlyWaterSampleComplianceReport
    {
        int? State { get; set; }

        [SearchAlias("OperatingCenterPublicWaterSupplies", "OperatingCenter.Id")]
        int[] OperatingCenter { get; set; }

        int[] EntityId { get; set; }

        [Search(CanMap = false)] // This is done manually in the repository search method
        int? CertifiedYear { get; set; }
    }

    public interface ISearchYearlyWaterSampleComplianceReport : ISearchSet<YearlyWaterSampleComplianceReportItem>,
        IBasicYearlyWaterSampleComplianceReport { }

    public class YearlyWaterSampleComplianceReportItem
    {
        public PublicWaterSupply PublicWaterSupply { get; set; }

        [View("January")]
        public WaterSampleComplianceForm JanuaryForm { get; set; }

        [View("February")]
        public WaterSampleComplianceForm FebruaryForm { get; set; }

        [View("March")]
        public WaterSampleComplianceForm MarchForm { get; set; }

        [View("April")]
        public WaterSampleComplianceForm AprilForm { get; set; }

        [View("May")]
        public WaterSampleComplianceForm MayForm { get; set; }

        [View("June")]
        public WaterSampleComplianceForm JuneForm { get; set; }

        [View("July")]
        public WaterSampleComplianceForm JulyForm { get; set; }

        [View("August")]
        public WaterSampleComplianceForm AugustForm { get; set; }

        [View("September")]
        public WaterSampleComplianceForm SeptemberForm { get; set; }

        [View("October")]
        public WaterSampleComplianceForm OctoberForm { get; set; }

        [View("November")]
        public WaterSampleComplianceForm NovemberForm { get; set; }

        [View("December")]
        public WaterSampleComplianceForm DecemberForm { get; set; }
    }
}
