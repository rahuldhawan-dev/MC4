using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using Migration = MapCall.Common.Model.Migrations.CreateTablesForBug1510;

namespace MapCall.Common.Model.Mappings
{
    public class FilterMediaTypeMap : ClassMap<FilterMediaType>
    {
        public FilterMediaTypeMap()
        {
            Table(Migration.TableNames.FILTER_MEDIA_TYPES);
            Id(x => x.Id).GeneratedBy.Identity().Column(Migration.ColumnNames.FilterMediaTypes.ID);
            Map(x => x.Description).Column(Migration.ColumnNames.FilterMediaTypes.DESCRIPTION).Not.Nullable()
                                   .Length(Migration.StringLengths.FilterMediaTypes.DESCRIPTION);
        }
    }
}
