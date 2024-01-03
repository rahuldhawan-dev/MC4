using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentCharacteristicMap : ClassMap<EquipmentCharacteristic>
    {
        public EquipmentCharacteristicMap()
        {
            Table("EquipmentCharacteristics");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.Equipment).Not.Nullable();
            References(x => x.Field).Not.Nullable();

            Map(x => x.Value).Not.Nullable().Length(255);
            //Map(x => x.IsActive).Not.Nullable();
            //Map(x => x.Order, "OrderBy").Nullable();
        }
    }
}
