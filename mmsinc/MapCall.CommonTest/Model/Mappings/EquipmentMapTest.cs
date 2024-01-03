using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Linq;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class EquipmentMapTest : MapCallMvcInMemoryDatabaseTestBase<Equipment>
    {
        [TestMethod]
        public void TestRemovingEquipmentSensorDeletesTheEquipmentSensorRecord()
        {
            var equip = GetEntityFactory<Equipment>().Create();
            var sensor = GetEntityFactory<Sensor>().Create();
            var equipmentSensor = GetEntityFactory<EquipmentSensor>().Create(new {
                Equipment = equip,
                Sensor = sensor
            });

            // Don't know why but this flush is needed in order for the sql to actually
            // fire off even though I'm pretty sure the factories all flush.
            Session.Flush();

            Assert.IsTrue(equip.Sensors.Contains(equipmentSensor), "Sanity");
            Assert.IsTrue(sensor.Equipment == equipmentSensor, "Sanity x2");

            equip.Sensors.Remove(equipmentSensor);
            //  equipmentSensor.Equipment = null;
            //  equipmentSensor.Sensor = null;
            //  sensor.Equipment = null;
            Session.Save(equip);
            Session.Flush();

            Session.Evict(equipmentSensor);

            equipmentSensor = Session.Query<EquipmentSensor>().SingleOrDefault(x => x.Id == equipmentSensor.Id);
            Assert.IsNull(equipmentSensor, "The sensor did not get removed from the EquipmentSensors table.");

            // Make sure it didn't do something stupid like delete the Equipment itself.
            Session.Evict(equip);
            Assert.IsNotNull(Session.Query<Equipment>().Single(x => x.Id == equip.Id));
        }

        [TestMethod]
        public void TestEquipmentIsTank()
        {
            var saq = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var saqGood = GetFactory<EquipmentTypeTankFactory>().Create();

            var eqOther = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = saq
            });
            var eqTank = GetEntityFactory<Equipment>().Create(new {
                EquipmentType = saqGood
            });
            var eqEmpty = GetEntityFactory<Equipment>().Create();
            
            Assert.IsTrue(eqTank.EquipmentIsTank);
            Assert.IsFalse(eqOther.EquipmentIsTank);
            Assert.IsFalse(eqEmpty.EquipmentIsTank);
        }
    }
}
