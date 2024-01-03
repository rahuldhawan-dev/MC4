using FluentNHibernate.Mapping;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model.Mappings
{
    public class SensorMap : ClassMap<Sensor>
    {
        public SensorMap()
        {
            Id(x => x.Id, "SensorID");

            Map(x => x.Description, "SensorDesc")
               .Length(Sensor.MAX_DESCRIPTION_LENGTH)
               .Not.Nullable();
            Map(x => x.Name, "SensorName")
               .Length(Sensor.MAX_NAME_LENGTH)
               .Not.Nullable();
            Map(x => x.Location, "SensorLocation")
               .Length(Sensor.MAX_LOCATION_LENGTH)
               .Nullable();

            References(x => x.MeasurementType, "SensorMeasurementTypeId")
               .Nullable();
            References(x => x.Board)
               .Not.Nullable();
            HasOne(x => x.Equipment)
               .PropertyRef(x => x.Sensor)
                // No cascading here. Equipment is in control of this, not Sensor or EquipmentSensor.
                // Change/fix this if we suddenly have to delete sensor records.
               .Cascade.None();
            HasMany(x => x.Readings);
        }
    }
}
