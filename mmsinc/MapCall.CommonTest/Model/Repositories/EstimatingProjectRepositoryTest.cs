using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class EstimatingProjectRepositoryTest : MapCallMvcSecuredRepositoryTestBase<EstimatingProject,
        EstimatingProjectRepository, User>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use(new TestDateTimeProvider());
        }

        #endregion

        [TestMethod]
        public void TestLinqPropertyAppliesRoleFilters()
        {
            this.TestLinqPropertyAppliesRoleFilters(EstimatingProjectRepository.ROLE);
        }
    }
}
