using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class SearchServicesRenewedTest : InMemoryDatabaseTest<Service>
    {
        #region Fields

        private SearchServicesRenewed _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new SearchServicesRenewed { DateInstalled = new RequiredDateRange()};
        }

        #endregion

        [TestMethod]
        public void TestRequiredDateRange()
        {
            ValidationAssert.ModelStateIsValid(_target);
        }
    }
}
