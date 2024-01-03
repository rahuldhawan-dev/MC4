using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class AddEquipmentSensorTest : MapCallMvcInMemoryDatabaseTestBase<Equipment>
    {
        #region Fields

        private AddEquipmentSensor _target;

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
            _target = new AddEquipmentSensor(_container);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityAddsANewEquipmentSensorRecord()
        {
            var sensor = GetEntityFactory<Sensor>().Create();
            var eq = GetEntityFactory<Equipment>().Create(new
            {
                Identifier = "Some equipment identifier"
            });

            _target.Sensor = sensor.Id;

            Assert.IsFalse(eq.Sensors.Any());

            _target.MapToEntity(eq);

            Assert.AreSame(eq, eq.Sensors.Single().Equipment);
            Assert.AreSame(sensor, eq.Sensors.Single().Sensor);
        }
        

        [TestMethod]
        public void TestSensorIsRequired()
        {
            ValidationAssert.PropertyIsRequired(_target, x => x.Sensor);
        }
        
        [TestMethod]
        public void TestValidateReturnsErrorIfSensorDoesNotExist()
        {
            _target.Sensor = 4311;
            ValidationAssert.ModelStateHasError(_target, x => x.Sensor, "Sensor's value does not match an existing object.");
        }

        [TestMethod]
        public void TestValidateReturnsErrorIfSensorIsAlreadyAttachedToAnEquipmentRecord()
        {
            var sensor = GetEntityFactory<Sensor>().Create();
            var eq = GetEntityFactory<Equipment>().Create(new {
            });
            sensor.Equipment = new EquipmentSensor {
                Equipment = eq,
                Sensor = sensor
            };
            Session.Save(sensor);
            Session.Flush();

            var someOtherEquipment = GetEntityFactory<Equipment>().Create();

            _target.Id = someOtherEquipment.Id;
            _target.Sensor = sensor.Id;

            ValidationAssert.ModelStateHasError(_target, x => x.Sensor, "Sensor is already attached to a piece of equipment(NJ7-1-ETTT-1).");
        }

        #endregion

    }
}
