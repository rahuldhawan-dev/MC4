using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Entities.Tests;
using MapCall.SAP.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace SAP.DataTest.Model.Repositories
{
    [TestClass()]
    public class SAPFunctionalLocationRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPFunctionalLocationRepository _target;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            // We injected an 
            _container.Inject<ISAPHttpClient>(_container.GetInstance<SAPHttpClient>());

            _target = _container.GetInstance<SAPFunctionalLocationRepository>();

            _sapHttpClient = _container.GetInstance<Mock<ISAPHttpClient>>();

            //_sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SAPFunctionalLoactionSearchTest()
        {
            var sapFunctionalLocation =
                _container.GetInstance<SAPFunctionalLocationTest>().SetFunctionalLoactionSearchValues();

            SAPFunctionalLocationCollection actual = _target.Search(sapFunctionalLocation);

            for (int i = 0; i < actual.Items.Count; i++)
            {
                Assert.AreEqual("Successful", actual.Items[i].SAPErrorCode, actual.Items[i].SAPErrorCode);
            }
        }
    }
}
