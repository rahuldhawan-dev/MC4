using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MeterDirectionMap : EntityLookupMap<MeterDirection>
    {
        public MeterDirectionMap()
        {
            Map(x => x.SAPCode);
        }
    }
}
