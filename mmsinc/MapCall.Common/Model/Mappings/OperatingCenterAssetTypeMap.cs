using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OperatingCenterAssetTypeMap : ClassMap<OperatingCenterAssetType>
    {
        public OperatingCenterAssetTypeMap()
        {
            Id(x => x.Id, "OperatingCenterAssetTypeID").GeneratedBy.Identity();

            References(x => x.OperatingCenter).Not.Nullable();
            References(x => x.AssetType).Not.Nullable();
        }
    }
}
