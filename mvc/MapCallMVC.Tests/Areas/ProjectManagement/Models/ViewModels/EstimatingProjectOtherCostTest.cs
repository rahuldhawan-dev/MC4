using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Models.ViewModels
{
    [TestClass]
    public abstract class EstimatingProjectOtherCostTest<TViewModel> : ViewModelTestBase<EstimatingProjectOtherCost, TViewModel>
        where TViewModel : EstimatingProjectOtherCostViewModel
    {
        #region Tests

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.EstimatingProject)
                     .CanMapBothWays(x => x.Quantity)
                     .CanMapBothWays(x => x.Description)
                     .CanMapBothWays(x => x.Cost);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.EstimatingProject)
                            .PropertyIsRequired(x => x.Description)
                            .PropertyIsRequired(x => x.Quantity)
                            .PropertyIsRequired(x => x.Cost);
        }
        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.EstimatingProject, GetEntityFactory<EstimatingProject>().Create());
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.Description, EstimatingProjectOtherCost.StringLengths.DESCRIPTION);
        }

        #endregion
    }
}