using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class BodyOfWaterRepositoryTest : InMemoryDatabaseTest<BodyOfWater, BodyOfWaterRepository>
    {
        [TestMethod]
        public void TestGetByOperatingCenterIdReturnsBodiesOfWaterWithOperatingCenterId()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var bow = new TestDataFactory<BodyOfWater>(_container).Create(new {OperatingCenter = opc});
            var invalid = new TestDataFactory<BodyOfWater>(_container).Create();
            var result = Repository.GetByOperatingCenterId(opc.Id);

            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(bow));
            Assert.IsFalse(result.Contains(invalid));
        }
    }
}
