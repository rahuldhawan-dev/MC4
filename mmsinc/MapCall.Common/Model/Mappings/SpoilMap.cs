using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SpoilMap : ClassMap<Spoil>
    {
        #region Constructors

        public SpoilMap()
        {
            Id(x => x.Id, "SpoilID");

            Map(x => x.Quantity)
               .Not.Nullable();

            References(x => x.SpoilStorageLocation)
               .Not.Nullable();
            References(x => x.WorkOrder)
               .Not.Nullable();
        }

        #endregion
    }
}
