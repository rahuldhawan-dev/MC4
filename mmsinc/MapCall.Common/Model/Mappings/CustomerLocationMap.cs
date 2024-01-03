using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class CustomerLocationMap : ClassMap<CustomerLocation>
    {
        #region Constructors

        public CustomerLocationMap()
        {
            Id(x => x.Id, "CustomerLocationID");

            Map(x => x.PremiseNumber).Not.Nullable();
            Map(x => x.Address);
            Map(x => x.City);
            Map(x => x.State);
            Map(x => x.Zip);

            HasMany(x => x.CustomerCoordinates).KeyColumn("CustomerLocationID");
            //References(x => x.ActiveCoordinate).Formula("(SELECT TOP 1 CustomerCoordinate where ");
        }

        #endregion
    }
}
