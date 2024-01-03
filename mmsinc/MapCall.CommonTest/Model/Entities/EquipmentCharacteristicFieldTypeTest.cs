using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class
        EquipmentCharacteristicFieldTypeTest : MapCallMvcInMemoryDatabaseTestBase<EquipmentCharacteristicFieldType>
    {
        #region Init/Cleanup

        private EquipmentCharacteristicFieldType _target;

        [TestInitialize]
        public void TestInitialize()
        {
            _target = GetEntityFactory<EquipmentCharacteristicFieldType>().Build();
        }

        #endregion

        [TestMethod, DoNotParallelize]
        public void TestRequiredProperties()
        {
            ValidationAssert.PropertyIsRequired(_target, m => m.DataType);
        }
    }
}
