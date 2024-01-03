using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class MeterChangeOutStatusMap : EntityLookupMap<MeterChangeOutStatus>
    {
        public MeterChangeOutStatusMap()
        {
            Table("MeterChangeOutStatuses");
        }
    }
}
