using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;

namespace MapCall.Common.Model.Mappings
{
    public class IconSetMap : ClassMap<IconSet>
    {
        public IconSetMap()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name);

            References(x => x.DefaultIcon, AddIconSetsAndLinkingTable.ColumnNames.IconSets.DEFAULT_ICON_ID);

            HasManyToMany(x => x.Icons)
               .Table(AddIconSetsAndLinkingTable.TableNames.MAP_ICON_ICON_SETS)
               .ParentKeyColumn(AddIconSetsAndLinkingTable.ColumnNames.MapIconIconSets.ICON_SET_ID)
               .ChildKeyColumn(AddIconSetsAndLinkingTable.ColumnNames.MapIconIconSets.ICON_ID);
        }
    }
}
