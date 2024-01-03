using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using NHibernate.Linq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class EquipmentCharacteristicFieldTest : MapCallMvcInMemoryDatabaseTestBase<EquipmentCharacteristicField>
    {
        #region Init/Cleanup

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return _container.With(typeof(EquipmentTypeFactory).Assembly).GetInstance<TestDataFactoryService>();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IEquipmentTypeRepository>().Use<EquipmentTypeRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            CreateTablesForBug1891.FIELD_TYPES.Each(o => GetEntityFactory<EquipmentCharacteristicFieldType>().Create(o));
        }

        #endregion

        #region Validation

        [TestMethod]
        public void  TestModelIsValidSometimesForReasons()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var model = _viewModelFactory.Build<CreateEquipmentCharacteristicField, EquipmentCharacteristicField>( GetEntityFactory<EquipmentCharacteristicField>().Build(new {
                EquipmentType = equipmentType,
                FieldName = "SomeField",
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "Number")
            }));

            ValidationAssert.WholeModelIsValid(model);
        }

        [TestMethod]
        public void TestModelIsNotValidWhenFieldNameIsPropertyOnEquipment()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var model = _viewModelFactory.Build<CreateEquipmentCharacteristicField, EquipmentCharacteristicField>( GetEntityFactory<EquipmentCharacteristicField>().Build(new {
                EquipmentType = equipmentType,
                FieldName = "SAPEquipmentId",
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "Number")
            }));

            ValidationAssert.SomethingAboutModelIsNotValid(model);
        }

        [TestMethod]
        public void TestModelIsNotValidIfFieldNameIsAlreadyUsed()
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var existingfield = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = equipmentType,
                FieldName = "Foo",
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == "Number")
            });
            Session.Clear();
            Session.Flush();
            var model = _viewModelFactory.Build<CreateEquipmentCharacteristicField, EquipmentCharacteristicField>(existingfield);

            ValidationAssert.SomethingAboutModelIsNotValid(model);
        }
    


        #endregion
    }
}
