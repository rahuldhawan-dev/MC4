using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class ZoneTypeMap : EntityLookupMap<ZoneType>
    {
        public ZoneTypeMap()
        {
            Id(x => x.Id, "ZoneTypeID").Not.Nullable();
        }
    }
}
