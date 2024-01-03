using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BelowGroundHazardMap : ClassMap<BelowGroundHazard>
    {
        public const string TABLE_NAME = "BelowGroundHazards";

        public BelowGroundHazardMap()
        {
            Id(x => x.Id);

            LazyLoad();

            References(x => x.Coordinate, "CoordinateId").Not.Nullable();
            References(x => x.WorkOrder, "WorkOrderId").Nullable();
            References(x => x.OperatingCenter, "OperatingCenterId").Not.Nullable();
            References(x => x.Town, "TownId").Not.Nullable();
            References(x => x.TownSection, "TownSectionId").Nullable();
            Map(x => x.StreetNumber).Nullable();
            References(x => x.Street, "StreetId").Not.Nullable();
            References(x => x.CrossStreet, "CrossStreetId").Nullable();
            Map(x => x.HazardArea, "HazardAreaFt").Not.Nullable();
            References(x => x.HazardType, "HazardTypeId").Not.Nullable();
            Map(x => x.DepthOfHazard, "DepthOfHazardInch").Nullable();
            References(x => x.AssetStatus, "AssetStatusId").Not.Nullable();
            Map(x => x.HazardDescription).Not.Nullable()
                                         .Length(BelowGroundHazard.StringLengths.HAZARD_DESCRIPTION_MAX);
            Map(x => x.ProximityToAmWaterAsset, "ProximityFromAmWaterAssetFt").Nullable();
            References(x => x.HazardApproachRecommendedType, "ApproachRecommendedId").Nullable();

            HasMany(x => x.BelowGroundHazardDocuments)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
            HasMany(x => x.BelowGroundHazardNotes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }
    }
}