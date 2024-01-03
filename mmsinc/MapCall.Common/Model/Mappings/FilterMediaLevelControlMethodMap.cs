using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using Migration = MapCall.Common.Model.Migrations.CreateTablesForBug1510;

namespace MapCall.Common.Model.Mappings
{
    public class FilterMediaLevelControlMethodMap : ClassMap<FilterMediaLevelControlMethod>
    {
        public FilterMediaLevelControlMethodMap()
        {
            Table(Migration.TableNames.FILTER_MEDIA_LEVEL_CONTROL_METHODS);
            Id(x => x.Id).GeneratedBy.Identity().Column(Migration.ColumnNames.FilterMediaLevelControlMethods.ID);
            Map(x => x.Description).Column(Migration.ColumnNames.FilterMediaLevelControlMethods.DESCRIPTION).Not
                                   .Nullable()
                                   .Length(Migration.StringLengths.FilterMediaLevelControlMethods.DESCRIPTION);
        }
    }
}
