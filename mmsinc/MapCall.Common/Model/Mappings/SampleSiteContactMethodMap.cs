using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class SampleSiteCustomerContactMethodMap : EntityLookupMap<SampleSiteCustomerContactMethod>
    {
        public SampleSiteCustomerContactMethodMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
