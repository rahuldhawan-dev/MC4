using LINQTo271.Views.OperatingCenterSpoilRemovalCosts;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.OperatingCenterSpoilRemovalCosts
{
    /// <summary>
    /// Summary description for OperatingCenterSpoilRemovalCostResourceViewPageTest.
    /// </summary>
    [TestClass]
    public class OperatingCenterSpoilRemovalCostResourceViewPageTest
    {
        #region Private Members

        private OperatingCenterSpoilRemovalCostResourceViewPage _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void OperatingCenterSpoilRemovalCostResourceViewPageTestInitialize()
        {
            _target = new TestOperatingCenterSpoilRemovalCostResourceViewPageBuilder();
        }

        #endregion

        [TestMethod]
        public void TestConstructorDoesNotThrowException()
        {
            MyAssert.DoesNotThrow(
                () => _target = new OperatingCenterSpoilRemovalCostResourceViewPage());
        }
    }

    internal class TestOperatingCenterSpoilRemovalCostResourceViewPageBuilder : TestDataBuilder<OperatingCenterSpoilRemovalCostResourceViewPage>
    {
        #region Exposed Methods

        public override OperatingCenterSpoilRemovalCostResourceViewPage Build()
        {
            var obj = new OperatingCenterSpoilRemovalCostResourceViewPage();
            return obj;
        }

        #endregion
    }
}
