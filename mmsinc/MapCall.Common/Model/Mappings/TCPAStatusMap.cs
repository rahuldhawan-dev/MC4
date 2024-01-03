using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class TCPAStatusMap : ClassMap<TCPAStatus>
    {
        public TCPAStatusMap()
        {
            Table(FixTableAndColumnNamesForBug1623.NewTableNames.TCPA_STATUSES);
            Id(x => x.Id);
            Map(x => x.Description).Not.Nullable().Unique();
        }
    }
}
