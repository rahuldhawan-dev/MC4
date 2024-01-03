using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallMVC.Tests.Areas.Production.ViewModels
{
    [TestClass]
    public class SearchProductionWorkOrderTests : InMemoryDatabaseTest<ProductionWorkOrder>
    {
        #region Fields

        private SearchProductionWorkOrder _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SearchProductionWorkOrder();
        }

        #endregion

        #region Tests
        
        [TestMethod]
        public void TestSearchDoesNotRequire()
        {
            ValidationAssert.PropertyIsNotRequired(_target, x => x.IsLockoutFormStillOpen);
            ValidationAssert.PropertyIsNotRequired(_target, x => x.TaskGroup);
            ValidationAssert.PropertyIsNotRequired(_target, x => x.TaskType);
            ValidationAssert.PropertyIsNotRequired(_target, x => x.ProductionWorkDescription);
        }

        #endregion
    }
}
