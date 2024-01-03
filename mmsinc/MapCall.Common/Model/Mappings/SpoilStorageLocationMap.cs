using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SpoilStorageLocationMap : ClassMap<SpoilStorageLocation>
    {
        #region Constructors

        public SpoilStorageLocationMap()
        {
            Id(x => x.Id, "SpoilStorageLocationId");

            Map(x => x.Name).Not.Nullable();
            Map(x => x.Active).Not.Nullable();

            References(x => x.OperatingCenter);
            References(x => x.Town);
            References(x => x.Street);
        }

        #endregion
    }
}
