using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Model.Mappings
{
    public class OneCallMarkoutTicketMap : ClassMap<OneCallMarkoutTicket>
    {
        public OneCallMarkoutTicketMap()
        {
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.MessageType).Not.Nullable();
            References(x => x.Town).Nullable();
            References(x => x.Street).Nullable();
            References(x => x.NearestCrossStreet).Nullable();
            References(x => x.OperatingCenter).Not.Nullable();

            Map(x => x.RequestNumber)
               .Column("RequestNumber")
               .Not.Nullable()
               .Precision(10)
               .UniqueKey(AlterConstraintsOnOneCallMarkoutTicketsForBug2647.GetConstraintName(
                    nameof(OneCallMarkoutTicket) + "s"));
            Map(x => x.DateTransmitted).Not.Nullable();
            Map(x => x.DateReceived).Not.Nullable();
            Map(X => X.CountyText).Column("County").Length(50).Not.Nullable();
            Map(x => x.TownText).Column("Town").Length(50).Not.Nullable();
            Map(x => x.StreetText).Column("Street").Length(50).Not.Nullable();
            Map(x => x.NearestCrossStreetText).Column("NearestCrossStreet").Length(50);
            Map(x => x.FullText).CustomType("StringClob").CustomSqlType("text").Column("FullText").Not.Nullable();
            Map(x => x.WorkingFor);
            Map(x => x.TypeOfWork);
            Map(x => x.Excavator);
            Map(x => x.CDCCode)
               .Column("CDCCode")
               .Not.Nullable()
               .UniqueKey(AlterConstraintsOnOneCallMarkoutTicketsForBug2647.GetConstraintName(
                    nameof(OneCallMarkoutTicket) + "s"));

            Map(x => x.RelatedRequestNumber).Nullable();
            References(x => x.RelatedRequest).DbSpecificFormula(
                "(SELECT Top 1 child.Id from OneCallMarkoutTickets child where child.RequestNumber = RelatedRequestNumber and child.CDCCode = CDCCode)",
                "(SELECT child.Id from OneCallMarkoutTickets child where child.RequestNumber = RelatedRequestNumber and child.CDCCode = CDCCode LIMIT 1)");

            Map(x => x.HasResponse)
               .Formula(string.Format(
                    "(CASE WHEN (SELECT COUNT(1) FROM {0} WHERE {0}.OneCallMarkoutTicketId = Id) > 0 THEN 1 ELSE 0 END)",
                    nameof(OneCallMarkoutResponse) + "s"));

            Map(x => x.ExcavatorIsAmericanWater)
               .Formula(
                    "(CASE WHEN EXISTS (SELECT 1 FROM AmericanWaterAliases awa WHERE awa.Description = Excavator) THEN 1 ELSE 0 END)");

            HasMany(x => x.Responses)
               .KeyColumn("OneCallMarkoutTicketId").Inverse().Cascade.All();
            HasMany(x => x.OneCallMarkoutTicketDocuments)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
            HasMany(x => x.OneCallMarkoutTicketNotes)
               .KeyColumn("LinkedId").Inverse().Cascade.None();
        }
    }
}
