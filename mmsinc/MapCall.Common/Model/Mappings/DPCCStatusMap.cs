using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class DPCCStatusMap : ClassMap<DPCCStatus>
    {
        public DPCCStatusMap()
        {
            Table(FixTableAndColumnNamesForBug1623.NewTableNames.DPCC_STATUSES);
            Id(x => x.Id);
            Map(x => x.Description).Not.Nullable().Unique();
        }
    }
}
