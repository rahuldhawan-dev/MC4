using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SystemDeliveryEntryTypeMap : EntityLookupMap<SystemDeliveryEntryType>
    {
        public SystemDeliveryEntryTypeMap()
        {
            References(x => x.SystemDeliveryType).Not.Nullable();
        }
    }
}
