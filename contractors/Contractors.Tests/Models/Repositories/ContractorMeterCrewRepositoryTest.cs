using System.Linq;
using Contractors.Data.Models.Repositories;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class ContractorMeterCrewRepositoryTest : ContractorsControllerTestBase<ContractorMeterCrew, ContractorMeterCrewRepository>
    {
        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            Repository =
                _container.GetInstance<ContractorMeterCrewRepository>();
        }

        #endregion

        #region Tests


        [TestMethod]
        public void TestLinqFiltersByContractor()
        {
            var contractor1 = GetFactory<ContractorFactory>().Create();
            var contractor2 = GetFactory<ContractorFactory>().Create();
            var meterCrew1 = GetFactory<ContractorMeterCrewFactory>().Create(new { Contractor = contractor1 });
            var meterCrew2 = GetFactory<ContractorMeterCrewFactory>().Create(new { Contractor = contractor2 });

            _currentUser.Contractor = contractor1;

            var result = Repository.GetAll().Single();
            Assert.AreSame(meterCrew1, result);
        }

        [TestMethod]
        public void TestCriteriaFiltersByContractor()
        {
            var contractor1 = GetFactory<ContractorFactory>().Create();
            var contractor2 = GetFactory<ContractorFactory>().Create();
            var meterCrew1 = GetFactory<ContractorMeterCrewFactory>().Create(new { Contractor = contractor1 });
            var meterCrew2 = GetFactory<ContractorMeterCrewFactory>().Create(new { Contractor = contractor2 });

            _currentUser.Contractor = contractor1;

            var result =
                Repository.Search(new EmptySearchSet<ContractorMeterCrew>())
                    .Single();
            Assert.AreSame(meterCrew1, result);
        }

        #endregion
    }
}
