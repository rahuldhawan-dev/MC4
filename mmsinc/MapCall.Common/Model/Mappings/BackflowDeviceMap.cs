using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class BackflowDeviceMap : EntityLookupMap<BackflowDevice>
    {
        public BackflowDeviceMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Column("BackflowDeviceId").Not.Nullable();
        }
    }
}
