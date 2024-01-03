using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class LateralSizeMap : EntityLookupMap<LateralSize>
    {
        public LateralSizeMap()
        {
            Map(x => x.Size).Not.Nullable();
            Map(x => x.SortOrder).Not.Nullable();
        }
    }
}
