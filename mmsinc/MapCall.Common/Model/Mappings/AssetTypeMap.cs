using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class AssetTypeMap : ClassMap<AssetType>
    {
        #region Constructors

        public AssetTypeMap()
        {
            Id(x => x.Id, "AssetTypeID").GeneratedBy.Assigned();

            Map(x => x.Description);
            Map(x => x.OneMapFeatureSource).Nullable();

            HasMany(x => x.OperatingCenterAssetTypes)
               .KeyColumn("AssetTypeId")
               .Cascade.All()
               .Inverse();
        }

        #endregion
    }
}
