using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class AssetCategoryMap : EntityLookupMap<AssetCategory>
    {
        public AssetCategoryMap()
        {
            Table("AssetCategories");
        }
    }
}
