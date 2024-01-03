using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Spoils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.Spoils
{
    [TestClass]
    public class SpoilViewModelTest<TViewModel> : ViewModelTestBase<Spoil, TViewModel> where TViewModel : SpoilViewModel
    {
        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.WorkOrder, GetEntityFactory<WorkOrder>().Create())
                            .EntityMustExist(x => x.SpoilStorageLocation, GetEntityFactory<SpoilStorageLocation>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.WorkOrder);
            _vmTester.CanMapBothWays(x => x.SpoilStorageLocation);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.WorkOrder)
                            .PropertyIsRequired(x => x.SpoilStorageLocation);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no strings to validate string length
        }

        #endregion
    }
}
