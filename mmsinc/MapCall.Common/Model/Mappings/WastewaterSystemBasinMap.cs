using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class WasteWaterSystemBasinMap : ClassMap<WasteWaterSystemBasin>
    {
        #region Constructors

        public WasteWaterSystemBasinMap()
        {
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.BasinName)
               .Length(WasteWaterSystemBasin.StringLengths.MAX_NAME_LENGTH)
               .Not.Nullable();
            Map(x => x.FirmCapacity, "FirmCapacity").Scale(3).Precision(6).Nullable();
            Map(x => x.FirmCapacityUnderStandbyPower, "FirmCapacityUnderStandbyPower").Scale(3).Precision(6).Nullable();
            Map(x => x.FirmCapacityDateUpdated, "FirmCapacityDateUpdated").Nullable();
            References(x => x.WasteWaterSystem).Not.Nullable();
            HasMany(x => x.Notes)
               .KeyColumn("LinkedId").LazyLoad().Inverse().Cascade.None();
        }

        #endregion
    }
}
