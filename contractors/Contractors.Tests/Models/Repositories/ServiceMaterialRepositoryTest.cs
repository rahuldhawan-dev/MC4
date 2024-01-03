using System.Linq;

using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class ServiceMaterialRepositoryTest : ContractorsControllerTestBase<ServiceMaterial, ServiceMaterialRepository>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            Repository = _container.GetInstance<ServiceMaterialRepository>();
        }

        #endregion

        #region GetAllButUnknown

        [TestMethod]
        public void TestGetAllButUnknownReturnsAllButTheUnknown()
        {
            var expected = GetFactory<ServiceMaterialFactory>().Create();
            var extra = GetFactory<ServiceMaterialFactory>().Create(new { Description = "Unknown"});

            var result = Repository.GetAllButUnknown().ToList();

            Assert.AreEqual(1, result.Count());
            Assert.IsFalse(result.Contains(extra));
        }

        #endregion
    }
}