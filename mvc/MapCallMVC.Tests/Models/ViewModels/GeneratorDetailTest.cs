using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class GeneratorTest : InMemoryDatabaseTest<Generator>
    {
        #region Fields

        private Generator _entity;
        private GeneratorViewModel _target;
        private ViewModelTester<GeneratorViewModel, Generator> _vmTester;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IEquipmentManufacturerRepository>().Use<EquipmentManufacturerRepository>();
            e.For<IEquipmentModelRepository>().Use<EquipmentModelRepository>();
            e.For<IDateTimeProvider>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetFactory<GeneratorFactory>().Create();
            _target = new GeneratorViewModel(_container);
            _vmTester = new ViewModelTester<GeneratorViewModel, Generator>(_target, _entity);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMappings()
        {
            _vmTester.CanMapToViewModel(x => x.Id, 13);
            _vmTester.DoesNotMapToEntity(x => x.Id, 31);

            _vmTester.CanMapBothWays(x => x.AQPermitNumber);
            _vmTester.CanMapBothWays(x => x.BTU);
            //_vmTester.CanMapBothWays(x => x.EmergencyPowerType);
            //_vmTester.CanMapBothWays(x => x.EngineManufacturer);
            //_vmTester.CanMapBothWays(x => x.EngineModel);
            _vmTester.CanMapBothWays(x => x.EngineSerialNumber);
            //_vmTester.CanMapBothWays(x => x.Equipment);
            _vmTester.CanMapBothWays(x => x.FuelGPH);
            //_vmTester.CanMapBothWays(x => x.FuelType);
            _vmTester.CanMapBothWays(x => x.GVWR);
            //_vmTester.CanMapBothWays(x => x.GeneratorManufacturer);
            //_vmTester.CanMapBothWays(x => x.GeneratorModel);
            _vmTester.CanMapBothWays(x => x.GeneratorSerialNumber);
            _vmTester.CanMapBothWays(x => x.HP);
            _vmTester.CanMapBothWays(x => x.HasAutomaticPowerTransfer);
            _vmTester.CanMapBothWays(x => x.HasAutomaticStart);
            _vmTester.CanMapBothWays(x => x.HasParallelElectricOperation);
            _vmTester.CanMapBothWays(x => x.IsPortable);
            _vmTester.CanMapBothWays(x => x.LoadCapacity);
            _vmTester.CanMapBothWays(x => x.OutputKW);
            _vmTester.CanMapBothWays(x => x.OutputVoltage);
            _vmTester.CanMapBothWays(x => x.SCADA);
            _vmTester.CanMapBothWays(x => x.TrailerVIN);
        }

        [TestMethod]
        public void TestGeneratorViewModelMapSetsPropertiesAndIds()
        {
            var equipment = GetFactory<EquipmentFactory>().Create();
            var emergencyPowerType = GetFactory<EmergencyPowerTypeFactory>().Create();
            var sapManufacturer = GetFactory<EquipmentManufacturerFactory>().Create();
            var generatorManufacturer = GetFactory<EquipmentManufacturerFactory>().Create();
            var generatorModel = GetFactory<EquipmentModelFactory>().Create(new { EquipmentManufacturer = sapManufacturer });
            var engineManufacturer = GetFactory<EquipmentManufacturerFactory>().Create();
            var engineModel = GetFactory<EquipmentModelFactory>().Create(new {EquipmentManufacturer = sapManufacturer });
            var fuelType = GetFactory<FuelTypeFactory>().Create();
            var generator = GetFactory<GeneratorFactory>().Create(new {
                Equipment = equipment,
                EmergencyPowerType = emergencyPowerType,
                EngineManufacturer = engineManufacturer,
                EngineModel = engineModel,
                GeneratorManufacturer = generatorManufacturer,
                GeneratorModel = generatorModel,
                FuelType = fuelType
            });

            var target = new GeneratorViewModel(_container);
            target.Map(generator);

            Assert.AreEqual(equipment.Id, target.Equipment, "EquipmentId");
            Assert.AreEqual(emergencyPowerType.Id, target.EmergencyPowerType, "EmergencyPowerTypeId");
            Assert.AreEqual(engineManufacturer.Id, target.EngineManufacturer, "EngineManufacturerId");
            Assert.AreEqual(engineModel.Id, target.EngineModel, "EngineModelId");
            Assert.AreEqual(generatorManufacturer.Id, target.GeneratorManufacturer, "GeneratorManufacturerId");
            Assert.AreEqual(generatorModel.Id, target.GeneratorModel, "GeneratorModelId");
            Assert.AreEqual(fuelType.Id, target.FuelType, "FueltTypeId");
        }

        [TestMethod]
        public void TestGeneratorViewModelMapToEntitySetsProperties()
        {
            var equipment = GetFactory<EquipmentFactory>().Create();
            var sapManufacturer = GetFactory<EquipmentManufacturerFactory>().Create();
            var emergencyPowerType = GetFactory<EmergencyPowerTypeFactory>().Create();
            var generatorManufacturer = GetFactory<EquipmentManufacturerFactory>().Create();
            var generatorModel = GetFactory<EquipmentModelFactory>().Create(new { EquipmentManufacturer = sapManufacturer });
            var engineManufacturer = GetFactory<EquipmentManufacturerFactory>().Create();
            var engineModel = GetFactory<EquipmentModelFactory>().Create(new { EquipmentManufacturer = sapManufacturer });
            var fuelType = GetFactory<FuelTypeFactory>().Create();

            var target = new GeneratorViewModel(_container) {
                Equipment = equipment.Id,
                EmergencyPowerType = emergencyPowerType.Id,
                EngineManufacturer = engineManufacturer.Id,
                EngineModel = engineModel.Id,
                GeneratorManufacturer = generatorManufacturer.Id,
                GeneratorModel = generatorModel.Id,
                FuelType = fuelType.Id
            };
            var entity = new Generator();

            target.MapToEntity(entity);

            Assert.AreEqual(equipment.Id, entity.Equipment.Id, "EquipmentId");
            Assert.AreEqual(emergencyPowerType.Id, entity.EmergencyPowerType.Id, "EmergencyPowerTypeId");
            Assert.AreEqual(engineManufacturer.Id, entity.EngineManufacturer.Id, "EngineManufacturerId");
            Assert.AreEqual(engineModel.Id, entity.EngineModel.Id, "EngineModelId");
            Assert.AreEqual(generatorManufacturer.Id, entity.GeneratorManufacturer.Id, "GeneratorManufacturerId");
            Assert.AreEqual(generatorModel.Id, entity.GeneratorModel.Id, "GeneratorModelId");
            Assert.AreEqual(fuelType.Id, entity.FuelType.Id, "FueltTypeId");
        }

        #endregion
    }
}
