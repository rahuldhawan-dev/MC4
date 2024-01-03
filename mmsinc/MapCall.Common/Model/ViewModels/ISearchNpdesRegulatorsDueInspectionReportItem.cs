using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchNpdesRegulatorsDueInspectionReportItem : ISearchSet<NpdesRegulatorsDueInspectionReportItem>
    {
        [SearchAlias("OperatingCenter", "opc", "Id")]
        int? OperatingCenter { get; set; }

        [Search(CanMap = false)]
        int? OperatingCenterId { get; set; }

        [SearchAlias("Town", "twn", "Id")]
        int? Town { get; set; }

        [Search(CanMap = false)]
        int? TownId { get; set; }

        [Search(CanMap = false)]
        int? Status { get; set; }

        [Search(CanMap = false)]
        int? StatusId { get; set; }

        [Search(CanMap = false)]
        RequiredDateRange DepartureDateTime { get; set; }
    }
}
