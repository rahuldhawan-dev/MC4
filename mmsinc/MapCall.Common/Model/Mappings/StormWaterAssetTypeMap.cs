using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class StormWaterAssetTypeMap : ClassMap<StormWaterAssetType>
    {
        public StormWaterAssetTypeMap()
        {
            Id(x => x.Id, "StormWaterAssetTypeID");

            Map(x => x.Description)
               .Not.Nullable()
               .Length(StormWaterAssetType.StringLengths.DESCRIPTION_MAX_LENGTH);
        }
    }
}
