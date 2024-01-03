using System.Web.Mvc;
using Contractors.Controllers;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest : ControllerTestBase<HomeController, IRepository<ContractorUser>>
    {
        [TestInitialize]
        public void HomeControllerTestInitialize()
        {
            BaseInitialize();
        }

        [TestMethod]
        public void Index()
        {
            // Act
            var result = _target.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestWhyDoIUseThisBaseClass()
        {
            Assert.Inconclusive("TODO: I should be using the regular ControllerBaseTest shouldn't I?");
        }
    }
}
