using System.Linq;
using System.Reflection;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class SearchHydrantTest : InMemoryDatabaseTest<Hydrant>
    {
        #region Private Members

        private SearchHydrant _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SearchHydrant();
        }

        #endregion

        #region Tests

        /// <summary>
        /// This is needed for GIS. They link to the Hydrants Index using a HydrantNumber as
        /// the search field. Making it required would break this functionality. Unless they update
        /// all of their links with an OperatingCenterID which would be quite difficult to maintain
        /// </summary>
        [TestMethod]
        public void Test_Search_DoesNotRequireOperatingCenter()
        {
            ValidationAssert.PropertyIsNotRequired(_target, x => x.OperatingCenter);
        }

        [TestMethod]
        public void Test_CastToSearchHydrantForMap_MapsAllSettablePropertyValues()
        {
            new CastPropertyTester<SearchHydrant, SearchHydrantForMap>(x => x)
               .AssertAllSettablePropertiesCanMap(
                    typeof(SearchSet<>)
                       .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                       .Select(p => p.Name)
                       .ToArray());
        }

        [TestMethod]
        public void Test_SettingRequiresInspectionToAValue_NullsRequiresPainting()
        {
            _target.RequiresPainting = false;
            _target.RequiresInspection = true;

            Assert.IsNull(_target.RequiresPainting);

            _target.RequiresPainting = true;
            _target.RequiresInspection = false;

            Assert.IsNull(_target.RequiresPainting);
        }

        [TestMethod]
        public void Test_SettingRequiresPaintingToAValue_NullsRequiresInspection()
        {
            _target.RequiresInspection = true;
            _target.RequiresPainting = false;

            Assert.IsNull(_target.RequiresInspection);

            _target.RequiresInspection = false;
            _target.RequiresPainting = true;

            Assert.IsNull(_target.RequiresInspection);
        }

        #endregion
    }
}
