using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class EmployeeStatusMap : ClassMap<EmployeeStatus>
    {
        public const string TABLE_NAME = FixTableAndColumnNamesForBug1623.NewTableNames.STATUSES;

        public EmployeeStatusMap()
        {
            Table(FixTableAndColumnNamesForBug1623.NewTableNames.STATUSES);

            Id(x => x.Id).GeneratedBy.Assigned();
            Map(x => x.Description).Not.Nullable().Unique();
        }
    }
}
