using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class HydrantBillingRepositoryTest : InMemoryDatabaseTest<HydrantBilling, HydrantBillingRepository>
    {
        #region Fields

        private HydrantBilling _public,
                               _other;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _public = GetFactory<PublicHydrantBillingFactory>().Create();
            _other = GetFactory<MunicipalHydrantBillingFactory>().Create();

            Session.Flush();
            Session.Clear();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetPublicHydrantBillingReturnsPublicRecord()
        {
            Assert.AreEqual(_public.Id, Repository.GetPublicHydrantBilling().Id);
        }

        #endregion
    }
}
