using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class ValveBillingMap : ClassMap<ValveBilling>
    {
        public ValveBillingMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Description);
        }
    }
}
