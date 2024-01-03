using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class ReasonForDepartureMap : ClassMap<ReasonForDeparture>
    {
        public ReasonForDepartureMap()
        {
            Table(FixTableAndColumnNamesForBug1623.NewTableNames.REASONS_FOR_DEPARTURE);
            Id(x => x.Id);
            Map(x => x.Description).Not.Nullable().Unique();
        }
    }
}
