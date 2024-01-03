using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using Migration = MapCall.Common.Model.Migrations.CreateTablesForBug1510;

namespace MapCall.Common.Model.Mappings
{
    public class FilterMediaWashTypeMap : ClassMap<FilterMediaWashType>
    {
        public FilterMediaWashTypeMap()
        {
            Table(Migration.TableNames.FILTER_MEDIA_WASH_TYPES);
            Id(x => x.Id).GeneratedBy.Identity().Column(Migration.ColumnNames.FilterMediaWashTypes.ID);
            Map(x => x.Description).Column(Migration.ColumnNames.FilterMediaWashTypes.DESCRIPTION).Not.Nullable()
                                   .Length(Migration.StringLengths.FilterMediaWashTypes.DESCRIPTION);
        }
    }
}
