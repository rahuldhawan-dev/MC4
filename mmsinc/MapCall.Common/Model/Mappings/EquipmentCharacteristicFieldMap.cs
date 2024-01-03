using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentCharacteristicFieldMap : ClassMap<EquipmentCharacteristicField>
    {
        public EquipmentCharacteristicFieldMap()
        {
            Table("EquipmentCharacteristicFields");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();

            References(x => x.EquipmentType).Not.Nullable();
            References(x => x.FieldType).Not.Nullable();

            Map(x => x.FieldName).Not.Nullable().Length(255);
            Map(x => x.Required).Not.Nullable();
            Map(x => x.IsSAPCharacteristic).Not.Nullable();
            Map(x => x.IsActive).Not.Nullable();
            Map(x => x.Order, "OrderBy").Nullable();
            Map(x => x.Description).Nullable();

            HasMany(x => x.DropDownValues).KeyColumn("FieldId").Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.EquipmentCharacteristics).KeyColumn("FieldId").ReadOnly();
        }
    }
}
