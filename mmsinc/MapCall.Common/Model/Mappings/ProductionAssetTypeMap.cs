using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ProductionAssetTypeMap : EntityLookupMap<ProductionAssetType>
    {
        protected override int DescriptionLength => ProductionAssetType.StringLengths.DESCRIPTION;
    }
}
