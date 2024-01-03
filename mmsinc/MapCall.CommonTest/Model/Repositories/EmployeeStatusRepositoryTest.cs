using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class EmployeeStatusRepositoryTest : InMemoryDatabaseTest<EmployeeStatus, EmployeeStatusRepository>
    {
        #region Tests

        [TestMethod]
        public void TestGetActiveStatusReturnsActiveStatus()
        {
            var active = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var inactive = GetFactory<InactiveEmployeeStatusFactory>().Create();

            Assert.AreSame(active, Repository.GetActiveStatus());
        }

        [TestMethod]
        public void TestGetInactiveStatusreturnsInactiveStatus()
        {
            var active = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var inactive = GetFactory<InactiveEmployeeStatusFactory>().Create();

            Assert.AreSame(inactive, Repository.GetInactiveStatus());
        }

        [TestMethod]
        public void TestGetStatusByDescription()
        {
            var active = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var inactive = GetFactory<InactiveEmployeeStatusFactory>().Create();

            Session.Flush();

            Assert.AreEqual(inactive.Id, Repository.GetStatusByDescription("Inactive").Id);
            Assert.AreEqual(active.Id, Repository.GetStatusByDescription("Active").Id);
        }

        [TestMethod]
        public void TestGetStatusByDescriptionThrowsAnExceptionWhenNoMatchingStatusIsFound()
        {
            MyAssert.Throws(() => Repository.GetStatusByDescription("oh okay"));
        }

        #endregion
    }
}
