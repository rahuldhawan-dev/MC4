using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SAP.DataTest.Model.Entities;
using StructureMap;

namespace SAP.DataTest.Model.Repositories
{
    [TestClass()]
    public class SAPCustomerOrderRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPCustomerOrderRepository _target;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            // We injected an 
            _container.Inject<ISAPHttpClient>(new SAPHttpClient());
            _target = _container.GetInstance<SAPCustomerOrderRepository>();
            //_sapHttpClient = _container.GetInstance<Mock<ISAPHttpClient>>();
            //_sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SAPDeviceRepositorySearchTest()
        {
            var searchSapCustomerOrder = _container.GetInstance<SAPCustomOrderTest>().SetCustomerOrderRequest();

            SAPCustomerOrderCollection actual = _target.Search(searchSapCustomerOrder);

            for (int i = 0; i < actual.Items.Count; i++)
            {
                Assert.AreEqual("Successful", actual.Items[i].SAPErrorCode, actual.Items[i].SAPErrorCode);
            }
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SAPDeviceRepositorySearchTestForFSR()
        {
            var searchSapCustomerOrder = _container.GetInstance<SAPCustomOrderTest>().SetCustomerOrderRequestForFRS();

            SAPCustomerOrderCollection actual = _target.Search(searchSapCustomerOrder);

            for (int i = 0; i < actual.Items.Count; i++)
            {
                Assert.AreEqual("No Data found for the given selection", actual.Items[i].SAPErrorCode,
                    actual.Items[i].SAPErrorCode);
            }
        }
    }
}
