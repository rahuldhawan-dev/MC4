using LINQTo271.Views.SpoilFinalProcessingLocations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.DesignPatterns;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace _271ObjectTests.Tests.Unit.Views.SpoilFinalProcessingLocations
{
    /// <summary>
    /// Summary description for SpoilFinalProcessingLocationSearchViewTest.
    /// </summary>
    [TestClass]
    public class SpoilFinalProcessingLocationSearchViewTest
    {
        #region Private Members

        private TestSpoilFinalProcessingLocationSearchView _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void SpoilFinalProcessingLocationSearchViewTestInitialize()
        {
            _target = new TestSpoilFinalProcessingLocationSearchViewBuilder();
        }

        #endregion
    }

    internal class TestSpoilFinalProcessingLocationSearchViewBuilder : TestDataBuilder<TestSpoilFinalProcessingLocationSearchView>
    {
        #region Exposed Methods

        public override TestSpoilFinalProcessingLocationSearchView Build()
        {
            var obj = new TestSpoilFinalProcessingLocationSearchView();
            return obj;
        }

        #endregion
    }

    internal class TestSpoilFinalProcessingLocationSearchView : SpoilFinalProcessingLocationSearchView
    {
    }
}
