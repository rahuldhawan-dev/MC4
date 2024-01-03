using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Models.ViewModels.RedTagPermits
{
    [TestClass]
    public class NewRedTagPermitViewModelTest : ViewModelTestBase<RedTagPermit, NewRedTagPermitViewModel>
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ProductionWorkOrder, GetEntityFactory<ProductionWorkOrder>().Create());
            _vmTester.CanMapBothWays(x => x.Equipment, GetEntityFactory<Equipment>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProductionWorkOrder);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Equipment);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(_viewModel, x => x.ProductionWorkOrder, GetEntityFactory<ProductionWorkOrder>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(_viewModel, x => x.Equipment, GetEntityFactory<Equipment>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation() { }

        #endregion
    }
}
