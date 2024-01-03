using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.MainBreaks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.MainBreaks
{
    [TestClass]
    public class MainBreakViewModelTest<TViewModel> : ViewModelTestBase<MainBreak, TViewModel> where TViewModel : MainBreakViewModel
    {
        #region Tests

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.WorkOrder, GetEntityFactory<WorkOrder>().Create())
                            .EntityMustExist(x => x.ServiceSize, GetEntityFactory<ServiceSize>().Create())
                            .EntityMustExist(x => x.MainBreakDisinfectionMethod, GetEntityFactory<MainBreakDisinfectionMethod>().Create())
                            .EntityMustExist(x => x.MainBreakFlushMethod, GetEntityFactory<MainBreakFlushMethod>().Create())
                            .EntityMustExist(x => x.MainBreakSoilCondition, GetEntityFactory<MainBreakSoilCondition>().Create())
                            .EntityMustExist(x => x.MainFailureType, GetEntityFactory<MainFailureType>().Create())
                            .EntityMustExist(x => x.MainBreakMaterial, GetEntityFactory<MainBreakMaterial>().Create())
                            .EntityMustExist(x => x.MainCondition, GetEntityFactory<MainCondition>().Create())
                            .EntityMustExist(x => x.ReplacedWith, GetEntityFactory<MainBreakMaterial>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.WorkOrder)
                     .CanMapBothWays(x => x.ServiceSize)
                     .CanMapBothWays(x => x.MainBreakMaterial)
                     .CanMapBothWays(x => x.MainBreakDisinfectionMethod)
                     .CanMapBothWays(x => x.MainBreakFlushMethod)
                     .CanMapBothWays(x => x.MainBreakSoilCondition)
                     .CanMapBothWays(x => x.MainCondition)
                     .CanMapBothWays(x => x.MainFailureType)
                     .CanMapBothWays(x => x.ReplacedWith);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.WorkOrder)
                            .PropertyIsRequired(x => x.ServiceSize)
                            .PropertyIsRequired(x => x.MainBreakMaterial)
                            .PropertyIsRequired(x => x.MainCondition)
                            .PropertyIsRequired(x => x.MainBreakSoilCondition)
                            .PropertyIsRequired(x => x.MainFailureType)
                            .PropertyIsRequired(x => x.MainBreakDisinfectionMethod)
                            .PropertyIsRequired(x => x.MainBreakFlushMethod)
                            .PropertyIsRequired(x => x.CustomersAffected)
                            .PropertyIsRequired(x => x.ShutdownTime)
                            .PropertyIsRequired(x => x.ChlorineResidual)
                            .PropertyIsRequired(x => x.Depth);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            // no strings to validate string length
        }

        #endregion
    }
}
