using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Models.ViewModels.PreJobSafetyBriefs;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.Production.ViewModels.PreJobSafetyBriefs
{
    [TestClass]
    public class CreateProductionPreJobSafetyBriefNoWorkOrderTest
        : CreateProductionPreJobSafetyBriefTestBase<CreateProductionPreJobSafetyBriefNoWorkOrder>
    {
        [TestMethod]
        public override void TestRequiredValidation()
        {
            base.TestRequiredValidation();

            ValidationAssert
               .PropertyIsRequired(x => x.OperatingCenter)
               .PropertyIsRequired(x => x.Facility)
               .PropertyIsRequired(x => x.DescriptionOfWork);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            base.TestPropertiesCanMapBothWays();

            _vmTester
               .CanMapBothWays(x => x.OperatingCenter)
               .CanMapBothWays(x => x.Facility);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            base.TestEntityMustExistValidation();

            ValidationAssert
               .EntityMustExist<OperatingCenter>(x => x.OperatingCenter)
               .EntityMustExist<Facility>(x => x.Facility);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            base.TestStringLengthValidation();

            ValidationAssert.PropertyHasMaxStringLength(x => x.DescriptionOfWork,
                ProductionPreJobSafetyBrief.StringLengths.DESCRIPTION_OF_WORK);
        }
    }
}
