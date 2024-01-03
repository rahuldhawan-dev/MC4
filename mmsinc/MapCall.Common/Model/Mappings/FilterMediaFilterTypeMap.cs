using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FilterMediaFilterTypeMap : ClassMap<FilterMediaFilterType>
    {
        public FilterMediaFilterTypeMap()
        {
            Table(Migrations.CreateTablesForBug1510.TableNames.FILTER_MEDIA_FILTER_TYPES);
            Id(x => x.Id).GeneratedBy.Identity()
                         .Column(Migrations.CreateTablesForBug1510.ColumnNames.FilterMediaFilterTypes.ID);
            Map(x => x.Description)
               .Column(Migrations.CreateTablesForBug1510.ColumnNames.FilterMediaFilterTypes.DESCRIPTION)
               .Not.Nullable()
               .Length(Migrations.CreateTablesForBug1510.StringLengths.FilterMediaFilterTypes.DESCRIPTION);
        }
    }
}
