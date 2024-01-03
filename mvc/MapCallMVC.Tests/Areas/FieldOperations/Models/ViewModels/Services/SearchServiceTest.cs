using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models.ViewModels.Services
{
    [TestClass]
    public class SearchServiceTest : InMemoryDatabaseTest<Service>
    {
        #region Fields

        private SearchService _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SearchService();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestSearchDoesNotRequireOperatingCenter()
        {
            ValidationAssert.PropertyIsNotRequired(_target, x => x.OperatingCenter);
        }

        #endregion
    }
}