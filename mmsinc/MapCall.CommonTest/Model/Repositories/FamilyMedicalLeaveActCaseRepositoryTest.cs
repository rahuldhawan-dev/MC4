using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class FamilyMedicalLeaveActCaseRepositoryTest : MapCallEmployeeSecuredRepositoryTestBase<
        FamilyMedicalLeaveActCase, FamilyMedicalLeaveActCaseRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        [TestMethod]
        public void TestGetByEmployeeReturnsCasesForEmployee()
        {
            var employee = GetEntityFactory<Employee>().Create();
            var validFmlaCase = GetEntityFactory<FamilyMedicalLeaveActCase>().Create(new {Employee = employee});
            var invalidFmlaCase = GetEntityFactory<FamilyMedicalLeaveActCase>().Create();

            var result = Repository.GetByEmployeeId(employee.Id);

            Assert.AreEqual(1, result.Count());
            Assert.IsFalse(result.Contains(invalidFmlaCase));
        }
    }
}
