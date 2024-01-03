using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class FacilityPerformanceMap : EntityLookupMap<FacilityPerformance>
    {
        public FacilityPerformanceMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
