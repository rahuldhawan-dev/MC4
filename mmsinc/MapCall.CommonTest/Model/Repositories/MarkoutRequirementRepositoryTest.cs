using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class
        MarkoutRequirementRepositoryTest : InMemoryDatabaseTest<MarkoutRequirement, MarkoutRequirementRepository>
    {
        #region Tests

        [TestMethod]
        public void TestGetEmergencyMarkoutRequirementReturnsEmergencyMarkoutRequirement()
        {
            var wrong = GetFactory<RoutineMarkoutRequirementFactory>().Create();
            var right = GetFactory<EmergencyMarkoutRequirementFactory>().Create();
            Session.Flush();
            Session.Clear();

            var result = Repository.GetEmergencyMarkoutRequirement();

            Assert.AreEqual(right.Id, result.Id);
            Assert.AreEqual(right.Description, result.Description);
        }

        #endregion
    }
}
