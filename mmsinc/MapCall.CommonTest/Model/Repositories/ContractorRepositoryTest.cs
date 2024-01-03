using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ContractorRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<Contractor, ContractorRepository>
    {
        [TestMethod]
        public void TestGetByOperatingCenterIdReturnsByOperatingCenterId()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var contractor = GetEntityFactory<Contractor>().Create();
            var invalid = GetEntityFactory<Contractor>().Create();
            opc.Contractors.Add(contractor);
            Session.Save(opc);
            Session.Flush();

            var result = Repository.GetByOperatingCenterId(opc.Id).ToArray();

            Assert.AreEqual(1, result.Count());
            Assert.IsFalse(result.Contains(invalid));
        }

        [TestMethod]
        public void TestGetAWRContractorsGetOnlyTheAWRContractors()
        {
            var validContractor = GetEntityFactory<Contractor>().Create(new {AWR = true});
            var invalidContractor = GetEntityFactory<Contractor>().Create();

            var result = Repository.GetAwrContractorsForDropDown();

            Assert.AreEqual(1, result.Count());
            Assert.IsFalse(result.Contains(invalidContractor));
        }
    }
}
