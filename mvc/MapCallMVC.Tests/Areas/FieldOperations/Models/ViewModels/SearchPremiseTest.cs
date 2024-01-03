using System.Linq;
using System.Reflection;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Customer.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class SearchPremiseTest : InMemoryDatabaseTest<Premise>
    {
        #region Fields

        private SearchPremise _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SearchPremise();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void Test_Search_DoesNotRequireSomeProperties()
        {
            ValidationAssert.PropertyIsNotRequired(_target, x => x.OperatingCenter);
            ValidationAssert.PropertyIsNotRequired(_target, x => x.PublicWaterSupply);
            ValidationAssert.PropertyIsNotRequired(_target, x => x.PremiseType);
        }

        [TestMethod]
        public void Test_CastToSearchPremiseForMap_MapsAllSettablePropertyValues()
        {
            new CastPropertyTester<SearchPremise, SearchPremiseForMap>(x => x)
               .AssertAllSettablePropertiesCanMap(
                    typeof(SearchSet<>)
                       .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                       .Select(p => p.Name)
                       .ToArray());
        }

        #endregion
    }
}