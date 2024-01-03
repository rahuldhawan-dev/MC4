using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class WasteWaterSystemStatusMap : EntityLookupMap<WasteWaterSystemStatus>
    {
        public WasteWaterSystemStatusMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
