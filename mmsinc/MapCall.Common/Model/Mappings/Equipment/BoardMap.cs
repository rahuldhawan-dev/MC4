using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class BoardMap : ClassMap<Board>
    {
        public BoardMap()
        {
            Id(x => x.Id, "BoardId");

            Map(x => x.Name, "BoardName")
               .Length(Board.MAX_NAME_LENGTH);

            References(x => x.Site).Not.Nullable();

            HasMany(x => x.Sensors);
        }
    }
}
