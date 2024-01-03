using LINQTo271.Views.SpoilFinalProcessingLocations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.SpoilFinalProcessingLocations
{
    /// <summary>
    /// Summary description for SpoilFinalProcessingLocationResourceViewPageTest.
    /// </summary>
    [TestClass]
    public class SpoilFinalProcessingLocationResourceViewPageTest
    {
        #region Private Members

        private TestSpoilFinalProcessingLocationResourceViewPage _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SpoilFinalProcessingLocationResourceViewPageTestInitialize()
        {
            _target = new TestSpoilFinalProcessingLocationResourceViewPageBuilder();
        }

        #endregion
    }

    internal class TestSpoilFinalProcessingLocationResourceViewPageBuilder : TestDataBuilder<TestSpoilFinalProcessingLocationResourceViewPage>
    {
        #region Exposed Methods

        public override TestSpoilFinalProcessingLocationResourceViewPage Build()
        {
            var obj = new TestSpoilFinalProcessingLocationResourceViewPage();
            return obj;
        }

        #endregion
    }

    internal class TestSpoilFinalProcessingLocationResourceViewPage : SpoilFinalProcessingLocationResourceViewPage
    {
    }
}
