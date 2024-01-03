using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class StormWaterAssetMap : ClassMap<StormWaterAsset>
    {
        public StormWaterAssetMap()
        {
            Id(x => x.Id, "StormWaterAssetID");

            Map(x => x.AssetNumber)
               .Not.Nullable()
               .Length(StormWaterAsset.StringLengths.MAX_ASSET_NUMBER_LENGTH)
               .Unique();

            Map(x => x.CreatedBy)
               .Not.Nullable()
               .Length(StormWaterAsset.StringLengths.MAX_CREATED_BY_LENGTH);

            Map(x => x.TaskNumber)
               .Nullable()
               .Length(StormWaterAsset.StringLengths.MAX_TASK_NUMBER_LENGTH);

            References(x => x.AssetType, "StormWaterAssetTypeID").Not.Nullable();
            References(x => x.Coordinate)
               .Nullable()
               .Fetch.Join(); // Fetch.Join() needed so we aren't querying the db 900 times on a map page.

            // Map(x => x.OperatingCenterId, "OperatingCenterID").Nullable().Not.Update().Not.Insert();
            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.Town).Not.Nullable();

            // These two models/maps don't exist yet.
            //References(x => x.Street);
            //References(x => x.IntersectingStreet);
        }
    }
}
