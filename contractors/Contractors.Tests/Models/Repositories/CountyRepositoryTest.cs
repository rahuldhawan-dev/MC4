using System.Linq;

using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Criterion;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class CountyRepositoryTest : ContractorsControllerTestBase<County, CountyRepository>
    {
        #region Setup/Teardown

        [TestInitialize]
        public void TownRepositoryTestInitialize()
        {
            Repository = _container.GetInstance<CountyRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetByStateItReturnsCountiesByState()
        {
            var state1 = GetFactory<StateFactory>().Create(new{ Abbreviation = "QQ" });
            var county1 = GetFactory<CountyFactory>().Create(new{ State = state1 });
            var county2 = GetFactory<CountyFactory>().Create();

            var result = Repository.GetByStateId(state1.Id);
            Assert.AreSame(county1, result.Single());
        }

        #endregion
    }
}
