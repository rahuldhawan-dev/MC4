using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels {
    [TestClass]
    public class AssetMapViewTest : InMemoryDatabaseTest<Valve>
    {
        #region Fields

        private AssetMapView _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new AssetMapView();
        }

        #endregion

        #region Tests

        /// <summary>
        /// This is needed so that the Map link on Show does not break for Hydrants/Valves
        /// </summary>
        [TestMethod]
        public void TestSearchDoesNotRequireOperatingCenter()
        {
            ValidationAssert.PropertyIsNotRequired(_target, x => x.OperatingCenter);
        }

        #endregion
    }
}