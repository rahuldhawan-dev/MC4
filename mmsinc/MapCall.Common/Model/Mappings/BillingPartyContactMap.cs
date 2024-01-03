using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BillingPartyContactMap : ClassMap<BillingPartyContact>
    {
        public const string TABLE_NAME = "BillingPartiesContacts";

        public BillingPartyContactMap()
        {
            Table(TABLE_NAME);
            Id(x => x.Id).GeneratedBy.Identity().Not.Nullable();
            References(x => x.BillingParty).Not.Nullable();
            References(x => x.Contact).Not.Nullable();
            References(x => x.ContactType).Not.Nullable();
        }
    }
}
