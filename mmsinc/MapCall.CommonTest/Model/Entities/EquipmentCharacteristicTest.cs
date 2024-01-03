using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing;
using NHibernate.Linq;
using System.Linq;
using MapCall.Common.Testing;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass, DoNotParallelize]
    public class EquipmentCharacteristicTest : MapCallMvcInMemoryDatabaseTestBase<EquipmentCharacteristic>
    {
        #region Init/Cleanup

        private EquipmentCharacteristic _target;

        [TestInitialize]
        public void TestInitialize()
        {
            CreateTablesForBug1891.FIELD_TYPES.Each(o =>
                GetEntityFactory<EquipmentCharacteristicFieldType>().Create(o));
        }

        private void CreateCharacteristic(string dataType, string fieldName, string fieldValue)
        {
            var equipmentType = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var equipmentPurpose = GetEntityFactory<EquipmentPurpose>().Create(new {
                EquipmentType = equipmentType
            });
            var equipment = GetEntityFactory<Equipment>().Create(new {
                EquipmentPurpose = equipmentPurpose
            });
            var field = GetEntityFactory<EquipmentCharacteristicField>().Create(new {
                EquipmentType = equipmentType,
                FieldType = Session.Query<EquipmentCharacteristicFieldType>().Single(t => t.DataType == dataType),
                FieldName = fieldName
            });
            _target = GetEntityFactory<EquipmentCharacteristic>().Build(new {
                Equipment = equipment,
                Field = field,
                Value = fieldValue
            });
        }

        #endregion

        [TestMethod]
        public void TestRequiredProperties()
        {
            CreateCharacteristic("Number", "SomeField", "4");

            ValidationAssert.PropertyIsRequired(_target, m => m.Equipment);
            ValidationAssert.PropertyIsRequired(_target, m => m.Field);
            ValidationAssert.PropertyIsRequired(_target, m => m.Value);
        }

        [TestMethod]
        public void TestStringValueCanBeAnyString()
        {
            CreateCharacteristic("String", "SomeField", "AnyString");

            ValidationAssert.WholeModelIsValid(_target);

            _target.Value = " ";

            ValidationAssert.SomethingAboutModelIsNotValid(_target);

            _target.Value = null;

            ValidationAssert.SomethingAboutModelIsNotValid(_target);
        }

        [TestMethod]
        public void TestNumberValueMustBeNumber()
        {
            CreateCharacteristic("Number", "SomeField", "4");

            ValidationAssert.WholeModelIsValid(_target);

            _target.Value = "NaN";

            ValidationAssert.SomethingAboutModelIsNotValid(_target);
        }

        [TestMethod]
        public void TestCurrencyValueMustBeANumber()
        {
            CreateCharacteristic("Currency", "SomeField", "4");

            ValidationAssert.WholeModelIsValid(_target);

            _target.Value = "NaN";

            ValidationAssert.SomethingAboutModelIsNotValid(_target);
        }

        [TestMethod]
        public void TestDateValueMustBeADate()
        {
            CreateCharacteristic("Date", "SomeField", "");

            ValidationAssert.SomethingAboutModelIsNotValid(_target);

            // no month has 31 days
            _target.Value = "01/32/2000";

            ValidationAssert.SomethingAboutModelIsNotValid(_target);

            // there is no 13th month
            _target.Value = "13/01/2000";

            ValidationAssert.SomethingAboutModelIsNotValid(_target);

            // this should be fine
            _target.Value = "01/01/2000";

            ValidationAssert.WholeModelIsValid(_target);

            // this should be fine too
            _target.Value = "1/1/2000";

            ValidationAssert.WholeModelIsValid(_target);
        }
    }
}
