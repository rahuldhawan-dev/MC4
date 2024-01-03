using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class BillingPartyMap : ClassMap<BillingParty>
    {
        public BillingPartyMap()
        {
            Table("BillingParties");

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.Description).Not.Nullable()
                                   .Length(CreateTrafficControlTicketsForBug2341
                                          .StringLengths.BillingParties.DESCRIPTION);
            Map(x => x.EstimatedHourlyRate).Length(6).Precision(2).Nullable();
            Map(x => x.Payee).Nullable();

            HasMany(x => x.BillingPartyContacts)
               .KeyColumn("BillingPartyId").Cascade.AllDeleteOrphan().Inverse();
        }
    }
}
