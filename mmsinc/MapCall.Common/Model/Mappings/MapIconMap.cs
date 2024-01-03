using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class MapIconMap : ClassMap<MapIcon>
    {
        public const string TABLE_NAME = "MapIcon";

        public MapIconMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id, "iconID").GeneratedBy.Assigned();

            // These are all nullable in the database for some reason.
            Map(x => x.FileName, "iconURL").Nullable();
            Map(x => x.Height, "height").Not.Nullable();
            Map(x => x.Width, "width").Not.Nullable();

            References(x => x.Offset).Not.Nullable().Not.LazyLoad();

            HasManyToMany(x => x.IconSets)
               .Table(AddIconSetsAndLinkingTable.TableNames.MAP_ICON_ICON_SETS)
               .ParentKeyColumn(AddIconSetsAndLinkingTable.ColumnNames.MapIconIconSets.ICON_ID)
               .ChildKeyColumn(AddIconSetsAndLinkingTable.ColumnNames.MapIconIconSets.ICON_SET_ID);
        }
    }
}
