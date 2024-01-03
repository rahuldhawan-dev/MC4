using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class HydrantBillingMap : EntityLookupMap<HydrantBilling>
    {
        public HydrantBillingMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();
        }
    }
}
