using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class CreateMaterialUsedTest : MaterialUsedModelTest<CreateMaterialUsed>
    {
        public override void TestEntityMustExistValidation()
        {
            base.TestEntityMustExistValidation();
            ValidationAssert.EntityMustExist(x => x.WorkOrder, GetEntityFactory<WorkOrder>().Create());
        }

        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();
            ValidationAssert.PropertyIsRequired(x => x.WorkOrder);
        }

        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();
            _vmTester.CanMapBothWays(x => x.WorkOrder);
        }
    }
}
