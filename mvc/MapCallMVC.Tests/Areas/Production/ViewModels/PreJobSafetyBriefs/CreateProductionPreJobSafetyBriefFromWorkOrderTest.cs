using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.ViewModels.PreJobSafetyBriefs
{
    [TestClass]
    public class CreateProductionPreJobSafetyBriefFromWorkOrderTest
        : CreateProductionPreJobSafetyBriefTestBase<CreateProductionPreJobSafetyBriefFromWorkOrder>
    {
        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();
            ValidationAssert.PropertyIsRequired(x => x.ProductionWorkOrder);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            base.TestEntityMustExistValidation();
            ValidationAssert.EntityMustExist<ProductionWorkOrder>(x => x.ProductionWorkOrder);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();
            _vmTester.CanMapBothWays(x => x.ProductionWorkOrder);
        }

        [TestMethod]
        public void Test_MapToEntity_SetsOperatingCenterAndFacilityFromWorkOrder()
        {
            var result = _vmTester.MapToEntity();
            
            Assert.IsNotNull(result.ProductionWorkOrder);
            Assert.IsNotNull(result.ProductionWorkOrder.OperatingCenter);
            Assert.IsNotNull(result.ProductionWorkOrder.Facility);
            Assert.AreSame(result.ProductionWorkOrder.OperatingCenter, result.OperatingCenter);
            Assert.AreSame(result.ProductionWorkOrder.Facility, result.Facility);
        }
    }
}
