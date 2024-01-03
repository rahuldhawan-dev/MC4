using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class OneCallMarkoutAuditTicketNumberMap : ClassMap<OneCallMarkoutAuditTicketNumber>
    {
        public OneCallMarkoutAuditTicketNumberMap()
        {
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Audit).Not.Nullable();
            References(x => x.MessageType).Not.Nullable();

            Map(x => x.RequestNumber)
               .Column("RequestNumber")
               .Not.Nullable()
               .Precision(10)
               .UniqueKey(AlterConstraintsOnOneCallMarkoutTicketsForBug2647.GetConstraintName(
                    nameof(OneCallMarkoutAuditTicketNumber) + "s"));
            Map(x => x.CDCCode)
               .Column("CDCCode")
               .UniqueKey(AlterConstraintsOnOneCallMarkoutTicketsForBug2647.GetConstraintName(
                    nameof(OneCallMarkoutAuditTicketNumber) + "s"));
        }
    }
}
