using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteAvailabilityMap : EntityLookupMap<SampleSiteAvailability>
    {
        public SampleSiteAvailabilityMap()
        {
            Table("SampleSiteAvailability");
        }
    }
}
