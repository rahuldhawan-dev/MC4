using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.ViewModels.PreJobSafetyBriefs
{
    [TestClass]
    public class EditProductionPreJobSafetyBriefWithWorkOrderTest
        : EditProductionPreJobSafetyBriefTestBase<EditProductionPreJobSafetyBriefWithWorkOrder>
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
    }
}