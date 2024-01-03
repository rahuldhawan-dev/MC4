using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class FuelTypeMap : ClassMap<FuelType>
    {
        #region Constructors

        public FuelTypeMap()
        {
            Id(x => x.Id, "FuelTypeID");

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
