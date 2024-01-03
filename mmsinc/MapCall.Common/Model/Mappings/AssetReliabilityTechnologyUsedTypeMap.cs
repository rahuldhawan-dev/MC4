using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class AssetReliabilityTechnologyUsedTypeMap : EntityLookupMap<AssetReliabilityTechnologyUsedType>
    {
        public AssetReliabilityTechnologyUsedTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
