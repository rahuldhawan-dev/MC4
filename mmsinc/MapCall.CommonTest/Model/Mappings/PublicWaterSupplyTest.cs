using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using NHibernate.Linq;
using StructureMap;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class PublicWaterSupplyTest : InMemoryDatabaseTest<PublicWaterSupply>
    {
        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use(new TestDateTimeProvider());
        }

        #region Tests

        [TestMethod]
        public void TestMarkingAPWSIDasAnticipatedMergedWillAppearforthePendingPWSID()
        {
            //Assemble
            var pwsidFactory = GetEntityFactory<PublicWaterSupply>();
            var pwsid1 = pwsidFactory.Create();
            var anticipatedPwsid1 = pwsidFactory.Create(new {AnticipatedMergePublicWaterSupply = pwsid1});

            var pwsid2 = pwsidFactory.Create();
            var anticipatedPwsid2 = pwsidFactory.Create(new {AnticipatedMergePublicWaterSupply = pwsid2});
            var anticipatedPwsid3 = pwsidFactory.Create(new {AnticipatedMergePublicWaterSupply = pwsid2});

            // evict/requery the parent pwsids because the PendingMergerPublicWaterSupplies
            // property won't have the linked entities. Need the proxy entities NHibernate
            // generates.
            Session.Evict(pwsid1);
            Session.Evict(pwsid2);

            // Assert
            pwsid1 = Session.Query<PublicWaterSupply>().Single(x => x.Id == pwsid1.Id);
            Assert.AreEqual(1, pwsid1.PendingMergerPublicWaterSupplies.Count);
            Assert.IsTrue(pwsid1.PendingMergerPublicWaterSupplies.Contains(anticipatedPwsid1));

            pwsid2 = Session.Query<PublicWaterSupply>().Single(x => x.Id == pwsid2.Id);
            Assert.AreEqual(2, pwsid2.PendingMergerPublicWaterSupplies.Count);
            Assert.IsTrue(pwsid2.PendingMergerPublicWaterSupplies.Contains(anticipatedPwsid2));
            Assert.IsTrue(pwsid2.PendingMergerPublicWaterSupplies.Contains(anticipatedPwsid3));
        }
    }

    #endregion
}
