using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class BacterialWaterSampleAnalystRepositoryTest : InMemoryDatabaseTest<BacterialWaterSampleAnalyst,
        BacterialWaterSampleAnalystRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetAllActiveByOperatingCenterReturnsAnalystsInTheSameOperatingCenter()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var analyst1 = GetFactory<BacterialWaterSampleAnalystFactory>().Create(new {IsActive = true});
            analyst1.OperatingCenters.Add(opc1);
            var analyst2 = GetFactory<BacterialWaterSampleAnalystFactory>().Create(new {IsActive = false});
            analyst2.OperatingCenters.Add(opc1);

            Session.Flush();
            Assert.IsTrue(analyst1.IsActive);
            Assert.IsFalse(analyst2.IsActive);

            Assert.AreSame(analyst1, Repository.GetAllActiveByOperatingCenter(opc1.Id).Single());
        }

        [TestMethod]
        public void TestGetAllByOperatingCenterReturnsAnalystsInTheSameOperatingCenter()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();

            var analyst1 = GetFactory<BacterialWaterSampleAnalystFactory>().Create();
            analyst1.OperatingCenters.Add(opc1);
            var analyst2 = GetFactory<BacterialWaterSampleAnalystFactory>().Create();
            analyst2.OperatingCenters.Add(opc2);

            Session.Flush();

            Assert.AreSame(analyst1, Repository.GetAllByOperatingCenter(opc1.Id).Single());
            Assert.AreSame(analyst2, Repository.GetAllByOperatingCenter(opc2.Id).Single());
        }

        #endregion
    }
}
