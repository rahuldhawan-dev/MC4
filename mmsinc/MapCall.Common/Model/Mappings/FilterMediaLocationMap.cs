using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using Migration = MapCall.Common.Model.Migrations.CreateTablesForBug1510;

namespace MapCall.Common.Model.Mappings
{
    public class FilterMediaLocationMap : ClassMap<FilterMediaLocation>
    {
        public FilterMediaLocationMap()
        {
            Table(Migration.TableNames.FILTER_MEDIA_LOCATIONS);
            Id(x => x.Id).GeneratedBy.Identity().Column(Migration.ColumnNames.FilterMediaLocations.ID);
            Map(x => x.Description).Column(Migration.ColumnNames.FilterMediaLocations.DESCRIPTION).Not.Nullable()
                                   .Length(Migration.StringLengths.FilterMediaLocations.DESCRIPTION);
        }
    }
}
