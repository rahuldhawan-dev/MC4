using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class RedTagPermitTest
    {
        #region Fields

        private RedTagPermit _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new RedTagPermit();
        }

        #endregion

        #region Tests

        #region Logical Properties

        [TestMethod]
        public void TestFacilityAndOperatingCenterAreDerivedWhenProductionWorkOrderIsNotNull()
        {
            var productionWorkOrder = new ProductionWorkOrder {
                OperatingCenter = new OperatingCenter(),
                Facility = new Facility()
            };

            _target.ProductionWorkOrder = productionWorkOrder;

            Assert.AreEqual(productionWorkOrder.OperatingCenter, _target.OperatingCenter);
            Assert.AreEqual(productionWorkOrder.Facility, _target.Facility);
        }

        [TestMethod]
        public void TestFacilityAndOperatingCenterAreNullWhenProductionWorkOrderIsNull()
        {
            Assert.IsNull(_target.OperatingCenter);
            Assert.IsNull(_target.Facility);
        }

        #endregion

        #endregion
    }
}