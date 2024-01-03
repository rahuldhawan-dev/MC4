using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CustomerCoordinateMap : ClassMap<CustomerCoordinate>
    {
        #region Constructors

        public CustomerCoordinateMap()
        {
            Id(x => x.Id, "CustomerCoordinateID");

            References(x => x.CustomerLocation);

            Map(x => x.Latitude).Not.Nullable();
            Map(x => x.Longitude).Not.Nullable();
            Map(x => x.Source).Not.Nullable();
            Map(x => x.Accuracy);
            Map(x => x.Verified);
        }

        #endregion
    }
}
