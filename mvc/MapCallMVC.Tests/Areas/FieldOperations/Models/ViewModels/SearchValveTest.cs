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
    public class SearchValveTest : InMemoryDatabaseTest<Valve>
    {
        #region Fields

        private SearchValve _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SearchValve();
        }

        #endregion

        #region Tests

        /// <summary>
        /// Requiring this field will break the Map link on the show page.
        /// </summary>
        [TestMethod]
        public void Test_Search_DoesNotRequireOperatingCenter()
        {
            ValidationAssert.PropertyIsNotRequired(_target, x => x.OperatingCenter);
        }

        [TestMethod]
        public void Test_CastToSearchValveForMap_MapsAllSettablePropertyValues()
        {
            new CastPropertyTester<SearchValve, SearchValveForMap>(x => x)
               .AssertAllSettablePropertiesCanMap(
                    typeof(SearchSet<>)
                       .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                       .Select(p => p.Name)
                       .ToArray());
        }

        [TestMethod]
        public void Test_CastToSearchBlowOffForMap_MapsAllSettablePropertyValues()
        {
            new CastPropertyTester<SearchValve, SearchBlowOffForMap>(x => x)
               .AssertAllSettablePropertiesCanMap(
                    typeof(SearchSet<>)
                       .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                       .Select(p => p.Name)
                       .ToArray());
        }

        #endregion
    }
}
