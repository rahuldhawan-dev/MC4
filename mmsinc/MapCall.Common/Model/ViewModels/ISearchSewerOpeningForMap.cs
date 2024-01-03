using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public interface ISearchSewerOpeningForMap : ISearchSet<SewerOpeningAssetCoordinate>
    {
        [SearchAlias("OperatingCenter", "opc", "Id")]
        int? OperatingCenter { get; set; }

        [SearchAlias("Town", "twn", "Id")]
        int? Town { get; set; }

        int? WasteWaterSystem { get; set; }

        int? SewerOpeningType { get; set; }

        int? TownSection { get; set; }

        string StreetNumber { get; set; }

        int? Street { get; set; }

        int? IntersectingStreet { get; set; }

        int? SAPEquipmentId { get; set; }

        bool? HasSAPErrorCode { get; set; }

        bool? Critical { get; set; }

        string SAPErrorCode { get; set; }

        int? Status { get; set; }

        string TaskNumber { get; set; }

        SearchString OpeningNumber { get; set; }

        int? OpeningSuffix { get; set; }

        [SearchAlias("FunctionalLocation", "fl", "Description")]
        string FunctionalLocationDescription { get; set; }

        int? FunctionalLocation { get; set; }

        bool? IsDoghouseOpening { get; set; }

        DateRange DateInstalled { get; set; }

        DateRange CreatedAt { get; set; }

        int? Route { get; set; }

        SearchString GeoEFunctionalLocation { get; set; }

        IntRange InspectionFrequency { get; set; }

        int? InspectionFrequencyUnit { get; set; }

        string OldNumber { get; set; }

        DateRange DateRetired { get; set; }

        int? BodyOfWater { get; set; }

        SearchString OutfallNumber { get; set; }

        SearchString LocationDescription { get; set; }
    }
}
