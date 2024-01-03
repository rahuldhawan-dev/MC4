using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteAddressLocationTypeMap : EntityLookupMap<SampleSiteAddressLocationType>
    {
        public SampleSiteAddressLocationTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
