using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class EquipmentSensorMap : ClassMap<EquipmentSensor>
    {
        public const string TABLE_NAME = "TFProd_Equipment_Sensor";

        public EquipmentSensorMap()
        {
            Table(TABLE_NAME);

            Id(x => x.Id);

            References(x => x.Equipment)
               .Not.Nullable();
            References(x => x.Sensor)
               .Not.Nullable();
        }
    }
}
