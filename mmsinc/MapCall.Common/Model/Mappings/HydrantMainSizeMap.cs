using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantMainSizeMap : EntityLookupMap<HydrantMainSize>
    {
        public HydrantMainSizeMap()
        {
            Map(x => x.Size).Not.Nullable();
            Map(x => x.SortOrder).Not.Nullable();
        }
    }
}
