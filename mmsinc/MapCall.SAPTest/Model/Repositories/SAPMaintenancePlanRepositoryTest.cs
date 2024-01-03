using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.SAP;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using Moq;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Entities.Tests;
using StructureMap;
using MapCall.SAPTest.Model.Entities;

namespace MapCall.SAPTest.Model.Repositories
{
    [TestClass]
    public class SAPMaintenancePlanRepositoryTest
    {
        #region Private Members

        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPMaintenancePlanRepository _target;
        private IContainer _container;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject<ISAPHttpClient>(_container.GetInstance<SAPHttpClient>());
            _target = _container.GetInstance<SAPMaintenancePlanRepository>();
            _sapHttpClient = _container.GetInstance<Mock<ISAPHttpClient>>();
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(true);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSaveSetsErrorCodeIfSiteIsNotRunning()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
            _container.Inject(_sapHttpClient.Object);

            var entity = _container.GetInstance<SAPMaintenancePlanLookup>();

            var result = _target.Search(entity);

            Assert.AreEqual(SAPMaintenancePlanRepository.ERROR_NO_SITE_CONNECTION, result?.Items[0].SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SapMaintenanceRepositorySearch()
        {
            var SapMaintenancePlanLookUp =
                _container.GetInstance<SAPMaintenancePlanTest>().GetTestMaintenancePlanLookUp();

            var actual = _target.Search(SapMaintenancePlanLookUp);

            Assert.AreEqual("Successfully", actual?.Items[0].SAPErrorCode, actual?.Items[0].SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SapMaintenanceRepositorySearchForNULL()
        {
            var SapMaintenancePlanLookUp =
                _container.GetInstance<SAPMaintenancePlanTest>().GetMaintenancePlanLookUpForNULLTest();

            var actual = _target.Search(SapMaintenancePlanLookUp);

            Assert.AreEqual("Successfully", actual?.Items[0].SAPErrorCode, actual?.Items[0].SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SapMaintenanceRepositoryTestUpdate()
        {
            var SapMaintenancePlanUpdate =
                _container.GetInstance<SAPMaintenancePlanTest>().GetTestMaintenancePlanUpdate();

            var actual = _target.Save(SapMaintenancePlanUpdate);

            Assert.AreEqual("Object List Updated Successfully", actual?.Items[0].SAPErrorCode,
                actual?.Items[0].SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SapMaintenanceRepositoryTestFixCall()
        {
            var SapMaintenancePlanUpdate =
                _container.GetInstance<SAPMaintenancePlanTest>().GetTestMaintenancePlanTestFixCall();

            var actual = _target.Save(SapMaintenancePlanUpdate);

            Assert.AreEqual("Call Fixed Successfully", actual?.Items[0].SAPErrorCode, actual?.Items[0].SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SapMaintenanceRepositoryTestManualCall()
        {
            var SapMaintenancePlanUpdate =
                _container.GetInstance<SAPMaintenancePlanTest>().GetTestMaintenancePlanTestManualCall();

            var actual = _target.Save(SapMaintenancePlanUpdate);

            Assert.AreEqual("Maintenance Pack is needed for Manual Call", actual?.Items[0].SAPErrorCode,
                actual?.Items[0].SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SapMaintenanceRepositoryTestSkipCall()
        {
            var SapMaintenancePlanUpdate =
                _container.GetInstance<SAPMaintenancePlanTest>().GetTestMaintenancePlanTestSkipCall();

            var actual = _target.Save(SapMaintenancePlanUpdate);

            Assert.AreEqual("Call Skipped Successfully", actual?.Items[0].SAPErrorCode, actual?.Items[0].SAPErrorCode);
        }
    }
}
