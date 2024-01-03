using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class OneCallMarkoutAuditMap : ClassMap<OneCallMarkoutAudit>
    {
        public OneCallMarkoutAuditMap()
        {
            Table("OneCallMarkoutAudits");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            Map(x => x.DateReceived).Not.Nullable();
            Map(x => x.DateTransmitted).Not.Nullable();
            Map(x => x.Success).Nullable();
            Map(x => x.FullText).CustomType("StringClob").CustomSqlType("text").Not.Nullable();

            HasMany(x => x.TicketNumbers)
               .KeyColumn("AuditId").Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
