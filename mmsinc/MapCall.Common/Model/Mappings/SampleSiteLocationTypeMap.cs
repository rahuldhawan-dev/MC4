using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteLocationTypeMap : EntityLookupMap<SampleSiteLocationType>
    {
        public SampleSiteLocationTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
