using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentManufacturerMap : ClassMap<EquipmentManufacturer>
    {
        public EquipmentManufacturerMap()
        {
            Table("EquipmentManufacturers");
            LazyLoad();
            Id(x => x.Id);
            References(x => x.EquipmentType).Not.Nullable();
            Map(x => x.Description).Not.Nullable().Length(50);
            Map(x => x.MapCallDescription).Nullable().Length(50);
        }
    }
}
