using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantSizeMap : EntityLookupMap<HydrantSize>
    {
        public HydrantSizeMap()
        {
            Map(x => x.Size).Not.Nullable();
            Map(x => x.SortOrder).Not.Nullable();
        }
    }
}
