using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchNpdesRegulatorDueInspection<TSearchSet> : ISearchSet<TSearchSet>
    {
        int? SewerOpeningId { get; set; }

        [SearchAlias("OperatingCenter", "opc", "Id")]
        int? OperatingCenter { get; set; }

        [SearchAlias("Town", "twn", "Id")]
        int? Town { get; set; }

        [Search(CanMap = false)]
        int? Status { get; set; }

        [Search(CanMap = false)]
        string NpdesPermitNumber { get; set; }

        [Search(CanMap = false)]
        string SewerOpeningNumber { get; set; }

        [Search(CanMap = false)]
        string BodyOfWater { get; set; }

        [Search(CanMap = false)]
        RequiredDateRange DepartureDateTime { get; set; }
    }

    public interface ISearchNpdesRegulatorsDueInspectionItem : ISearchNpdesRegulatorDueInspection<SewerOpening> { }

    public interface ISearchNpdesRegulatorsDueInspectionForMap : ISearchNpdesRegulatorDueInspection<SewerOpeningAssetCoordinate> { }
}
