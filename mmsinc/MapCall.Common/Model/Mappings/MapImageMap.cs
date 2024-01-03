using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class MapImageMap : ClassMap<MapImage>
    {
        public MapImageMap()
        {
            Id(x => x.Id, "MapImageID")
               .Not.Nullable();

            Map(x => x.DateRevised)
               .Length(50)
               .Nullable();

            Map(x => x.Directory, "fld")
               .Length(30)
               .Not.Nullable();

            Map(x => x.East)
               .Length(50)
               .Nullable();

            Map(x => x.FileName, "FileList")
               .Length(50)
               .Not.Nullable();

            Map(x => x.Gradient)
               .Length(50)
               .Nullable();

            Map(x => x.MapPage)
               .Length(50)
               .Nullable();

            Map(x => x.North)
               .Length(50)
               .Nullable();

            Map(x => x.South)
               .Length(50)
               .Nullable();

            Map(x => x.TownSection)
               .Length(50)
               .Nullable();

            Map(x => x.West)
               .Length(50)
               .Nullable();

            References(x => x.Town)
               .Not.Nullable();
        }
    }
}
