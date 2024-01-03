using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCallMVC.Areas.Production.Models.ViewModels.WellTests;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Production.ViewModels.WellTests
{
    [TestClass]
    public class NewWellTestViewModelTest : ViewModelTestBase<WellTest, NewWellTestViewModel> 
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

        // CS0534: must implement inherited abstract members
        [TestMethod]
        public override void TestStringLengthValidation() { }

        #endregion
    }
}