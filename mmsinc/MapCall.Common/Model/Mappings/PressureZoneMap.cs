using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class PressureZoneMap : ClassMap<PressureZone>
    {
        #region Constructors

        public PressureZoneMap()
        {
            Id(x => x.Id, "PressureZoneID");

            Map(x => x.Description).Not.Nullable();
        }

        #endregion
    }
}
