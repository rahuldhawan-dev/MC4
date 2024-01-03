using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class EquipmentCharacteristicFieldTest : MapCallMvcInMemoryDatabaseTestBase<EquipmentCharacteristicField>
    {
        #region Init/Cleanup

        private EquipmentCharacteristicField _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _target = GetEntityFactory<EquipmentCharacteristicField>().Build();
        }

        #endregion

        [TestMethod, DoNotParallelize]
        public void TestRequiredProperties()
        {
            ValidationAssert.PropertyIsRequired(_target, m => m.EquipmentType);
            ValidationAssert.PropertyIsRequired(_target, m => m.FieldType);
            ValidationAssert.PropertyIsRequired(_target, m => m.FieldName);
        }
    }
}
