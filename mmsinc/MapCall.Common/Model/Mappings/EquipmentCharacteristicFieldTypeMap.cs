using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentCharacteristicFieldTypeMap : ClassMap<EquipmentCharacteristicFieldType>
    {
        public EquipmentCharacteristicFieldTypeMap()
        {
            Table("EquipmentCharacteristicFieldTypes");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.DataType).Not.Nullable().Length(255);
            Map(x => x.Regex).Nullable().Length(255);
        }
    }
}
