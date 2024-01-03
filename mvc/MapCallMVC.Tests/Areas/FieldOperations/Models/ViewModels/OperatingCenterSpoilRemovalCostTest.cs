using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class OperatingCenterSpoilRemovalCostTest<TViewModel> : ViewModelTestBase<OperatingCenterSpoilRemovalCost, TViewModel> where TViewModel : OperatingCenterSpoilRemovalCostViewModel
    {
        #region Tests

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.State);
            ValidationAssert.PropertyIsRequired(x => x.OperatingCenter);
        }

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.State, GetEntityFactory<State>().Create());
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays() { }

        [TestMethod]
        public override void TestStringLengthValidation() { }

        #endregion
    }

    [TestClass]
    public class CreateOperatingCenterSpoilRemovalCostTest : OperatingCenterSpoilRemovalCostTest<CreateOperatingCenterSpoilRemovalCost> { }

    [TestClass]
    public class EditOperatingCenterSpoilRemovalCostTest : OperatingCenterSpoilRemovalCostTest<EditOperatingCenterSpoilRemovalCost> { }

    [TestClass]
    public class SearchOperatingCenterSpoilRemovalCostTest : InMemoryDatabaseTest<OperatingCenterSpoilRemovalCost>
    {
        #region Fields

        private SearchOperatingCenterSpoilRemovalCost _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SearchOperatingCenterSpoilRemovalCost();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void Test_Search_DoesRequireOperatingCenter()
        {
            ValidationAssert.PropertyIsRequired(_target, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_target, x => x.State);
        }

        #endregion
    }
}
