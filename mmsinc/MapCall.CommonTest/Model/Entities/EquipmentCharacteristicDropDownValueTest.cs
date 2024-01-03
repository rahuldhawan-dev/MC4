using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class
        EquipmentCharacteristicDropDownValueTest : MapCallMvcInMemoryDatabaseTestBase<
            EquipmentCharacteristicDropDownValue>
    {
        #region Private Members

        private EquipmentCharacteristicDropDownValue _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = GetEntityFactory<EquipmentCharacteristicDropDownValue>().Build();
        }

        #endregion

        [TestMethod, DoNotParallelize]
        public void TestRequiredProperties()
        {
            ValidationAssert.PropertyIsRequired(_target, m => m.Field);
            ValidationAssert.PropertyIsRequired(_target, m => m.Value);
        }
    }
}
