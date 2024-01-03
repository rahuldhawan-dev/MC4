using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Linq;
using StructureMap;
using System.Linq;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class EnvironmentalPermitMapTest : MapCallMvcInMemoryDatabaseTestBase<EnvironmentalPermit>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
        }

        [TestInitialize]
        public void InitializeTest()
        {
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestRemovingAFeeFromAPermitDeletesTheFee()
        {
            var permit = GetEntityFactory<EnvironmentalPermit>().Create();
            var fee = GetEntityFactory<EnvironmentalPermitFee>().Create(new {EnvironmentalPermit = permit});

            Session.Refresh(permit);
            Assert.IsTrue(permit.Fees.Contains(fee), "Sanity");

            permit.Fees.Remove(fee);
            Session.Save(permit);
            Session.Flush();

            fee = Session.Query<EnvironmentalPermitFee>().SingleOrDefault(x => x.Id == fee.Id);
            Assert.IsNull(fee);
        }

        #endregion
    }
}
