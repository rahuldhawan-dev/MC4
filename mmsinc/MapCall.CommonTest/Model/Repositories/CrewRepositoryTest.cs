using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class CrewRepositoryTest : MapCallMvcSecuredRepositoryTestBase<Crew, CrewRepository, User>
    {
        [TestMethod]
        public void TestLinqPropertyAppliesRoleFilters()
        {
            this.TestLinqPropertyAppliesRoleFilters(CrewRepository.ROLE);
        }
    }
}
