using System.Linq;
using System.Reflection;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.MostRecentlyInstalledServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.Utilities;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels
{
    [TestClass]
    public class SearchCurrentMaterialTest
    {
        [TestMethod]
        public void Test_CastToSearchCurrentMaterialForMap_MapsAllSettablePropertyValues()
        {
            new CastPropertyTester<SearchCurrentMaterial, SearchCurrentMaterialForMap>(x => x)
               .AssertAllSettablePropertiesCanMap(
                    typeof(SearchSet<>)
                       .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                       .Select(p => p.Name)
                       .ToArray());
        }
    }
}
