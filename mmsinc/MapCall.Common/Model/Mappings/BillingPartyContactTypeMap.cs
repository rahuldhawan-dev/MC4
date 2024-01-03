using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BillingPartyContactTypeMap : ClassMap<BillingPartyContactType>
    {
        public BillingPartyContactTypeMap()
        {
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();

            References(x => x.ContactType).Not.Nullable();
        }
    }
}
