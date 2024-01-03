using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class UserTypeRepositoryTest : InMemoryDatabaseTest<UserType, UserTypeRepository>
    {
        #region Tests

        [TestMethod]
        public void TestGetInternalUserTypeReturnsUserTypeWithInternalDescription()
        {
            var expected = GetFactory<InternalUserTypeFactory>().Create();
            var result = Repository.GetInternalUserType();
            Assert.AreSame(expected, result);
        }

        #endregion
    }
}
