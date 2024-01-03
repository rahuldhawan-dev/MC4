using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class RemoveEquipmentSensorTest : MapCallMvcInMemoryDatabaseTestBase<Equipment>
    {
        #region Fields

        private RemoveEquipmentSensor _target;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ISensorRepository>().Use<SensorRepository>();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new RemoveEquipmentSensor(_container);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityRemovesExistingEquipmentSensorRecordFromEquipment()
        {
            var sensor = GetEntityFactory<Sensor>().Create();
            var eq = GetEntityFactory<Equipment>().Create(new
            {
                Identifier = "Some equipment identifier"
            });

            eq.Sensors.Add(new EquipmentSensor {
                Equipment = eq,
                Sensor = sensor
            });
            Session.Save(eq);

            _target.Sensor = sensor.Id;

            Assert.IsTrue(eq.Sensors.Any());

            _target.MapToEntity(eq);

            Assert.IsFalse(eq.Sensors.Any());
        }


        #endregion

    }
}
