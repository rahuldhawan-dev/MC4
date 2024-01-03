using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CoordinateMap : ClassMap<Coordinate>
    {
        #region Constructors

        public CoordinateMap()
        {
            Id(x => x.Id, "CoordinateID");

            Map(x => x.Latitude).Scale(8).Not.Nullable();
            Map(x => x.Longitude).Scale(8).Not.Nullable();

            References(x => x.Icon).Nullable();
        }

        #endregion
    }
}
