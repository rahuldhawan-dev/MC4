using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentCharacteristicDropDownValueMap : ClassMap<EquipmentCharacteristicDropDownValue>
    {
        public EquipmentCharacteristicDropDownValueMap()
        {
            Table("EquipmentCharacteristicDropDownValues");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            References(x => x.Field).Not.Nullable();
            Map(x => x.Value).Not.Nullable().Length(255);
        }
    }
}
