using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ContractorUserTest : InMemoryDatabaseTest<ContractorUser>
    {
        #region Tests

        [TestMethod]
        public void TestContractorUserHasAccess()
        {
            var contractor = new Contractor();
            var target = new ContractorUser {Contractor = contractor};

            target.IsActive = true;
            contractor.ContractorsAccess = true;
            Assert.IsTrue(target.HasAccess,
                "ContractorUser's have access if they are both active and their contractor has contractor's access.");

            target.IsActive = false;
            Assert.IsFalse(target.HasAccess, "ContractorUser does not have access if IsActive is false.");

            target.IsActive = true;
            contractor.ContractorsAccess = false;
            Assert.IsFalse(target.HasAccess, "ContractorUser does not have access if Contractor does not have access");

            // And it should straight up throw an exception if the Contractor is null, because the Contractor should never be null.
            target.Contractor = null;
            var neat = true;
            MyAssert.Throws(() => neat = target.HasAccess);
        }

        [TestMethod]
        public void
            TestIsLockedOutDueToFailedLoginAttemptsReturnsTrueIfIsActiveIsFalseAndFailedLoginAttemptCountIsGreaterThanOrEqualToSix()
        {
            var target = new ContractorUser();
            target.IsActive = false;

            target.FailedLoginAttemptCount = 0;
            Assert.IsFalse(target.IsLockedOutDueToFailedLoginAttempts);

            target.FailedLoginAttemptCount = 1;
            Assert.IsFalse(target.IsLockedOutDueToFailedLoginAttempts);

            target.FailedLoginAttemptCount = 2;
            Assert.IsFalse(target.IsLockedOutDueToFailedLoginAttempts);

            target.FailedLoginAttemptCount = 3;
            Assert.IsFalse(target.IsLockedOutDueToFailedLoginAttempts);

            target.FailedLoginAttemptCount = 4;
            Assert.IsFalse(target.IsLockedOutDueToFailedLoginAttempts);

            target.FailedLoginAttemptCount = 5;
            Assert.IsFalse(target.IsLockedOutDueToFailedLoginAttempts);

            target.FailedLoginAttemptCount = 6;
            Assert.IsTrue(target.IsLockedOutDueToFailedLoginAttempts);

            target.FailedLoginAttemptCount = 7;
            Assert.IsTrue(target.IsLockedOutDueToFailedLoginAttempts);

            target.IsActive = true;
            Assert.IsFalse(target.IsLockedOutDueToFailedLoginAttempts);
        }

        [TestMethod]
        public void TestOperatingCenterIdsReturnsOperatingCenterIdsFromUsersContractor()
        {
            var operatingCenters = GetFactory<UniqueOperatingCenterFactory>().CreateArray(2);
            var user = GetFactory<ContractorUserFactory>().Create();

            operatingCenters.Each(oc => user.Contractor.OperatingCenters.Add(oc));
            Session.SaveOrUpdate(user.Contractor);

            var expected = operatingCenters
                          .Map<OperatingCenter, int>(oc => oc.Id)
                          .ToArray();
            var actual = user.OperatingCenterIds;

            Assert.AreEqual(expected.Length, actual.Length);
            for (var i = 0; i < expected.Length; ++i)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        [TestMethod]
        public void TestContractorUserHasaccess()
        {
            var contractor = new Contractor();
            var target = new ContractorUser {Contractor = contractor};

            target.IsActive = true;
            contractor.ContractorsAccess = true;
            Assert.IsTrue(target.HasAccess,
                "ContractorUser's have access if they are both active and their contractor has contractor's access.");

            target.IsActive = false;
            Assert.IsFalse(target.HasAccess, "ContractorUser does not have access if IsActive is false.");

            target.IsActive = true;
            contractor.ContractorsAccess = false;
            Assert.IsFalse(target.HasAccess, "ContractorUser does not have access if Contractor does not have access");

            // And it should straight up throw an exception if the Contractor is null, because the Contractor should never be null.
            target.Contractor = null;
            var neat = true;
            MyAssert.Throws(() => neat = target.HasAccess);
        }

        #endregion
    }
}
