using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MeterScheduleTimeMap : EntityLookupMap<MeterScheduleTime>
    {
        public MeterScheduleTimeMap()
        {
            Map(x => x.AM).Not.Nullable();
        }
    }
}
