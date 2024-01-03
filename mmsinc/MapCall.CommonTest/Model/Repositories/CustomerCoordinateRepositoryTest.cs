using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    /// <summary>
    /// Summary description for CustomerCoordinateRepositoryTest
    /// </summary>
    [TestClass]
    public class
        CustomerCoordinateRepositoryTest : InMemoryDatabaseTest<CustomerCoordinate, CustomerCoordinateRepository>
    {
        #region Fields

        #endregion

        #region Tests

        [TestMethod]
        public void TestSavingACustomerCoordinateAsVerifiedSetsOtherCustomerCoordinatesToNotVerified()
        {
            var location = GetFactory<CustomerLocationFactory>().Create();
            var existing = GetFactory<CustomerCoordinateFactory>()
               .Create(new {CustomerLocation = location, Verified = true});
            var notVerified = new CustomerCoordinate {CustomerLocation = location, Latitude = 0, Longitude = 0};

            Repository.Save(notVerified);

            // Doesn't change existing record because the new one wasn't set as Verified
            Assert.IsTrue(Repository.Find(existing.Id).Verified);
            Assert.IsFalse(Repository.Find(notVerified.Id).Verified);

            var verified = new CustomerCoordinate
                {CustomerLocation = location, Latitude = 0, Longitude = 0, Verified = true};

            Repository.Save(verified);

            Assert.IsTrue(Repository.Find(verified.Id).Verified);
            Assert.IsFalse(Repository.Find(existing.Id).Verified);
            Assert.IsFalse(Repository.Find(notVerified.Id).Verified);
        }

        #endregion
    }
}
