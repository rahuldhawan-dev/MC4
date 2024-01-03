using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SAP.DataTest.Model.Entities;
using StructureMap;

namespace SAP.DataTest.Model.Repositories
{
    [TestClass()]
    public class SAPManufacturerRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPManufacturerRepository _target;

        [TestInitialize]
        public void TestInitialize()
        {
            // We injected an 
            ObjectFactory.Inject<ISAPHttpClient>(new SAPHttpClient());

            _target = new SAPManufacturerRepository();

            _sapHttpClient = new Mock<ISAPHttpClient>();

            //_sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);

        }


        [TestMethod()]
        public void SAPWBSElementRepositorySearchTestByWBS()
        {
            var searchSAPManufacturer = new SAPManufacturerTest().SetSAPManufacturerSearchValues();

            SAPManufacturerCollection actual = _target.Search(searchSAPManufacturer);
            for (int i = 0; i < actual.Items.Count; i++)
            {
                Assert.AreEqual("No data displayed due to maximum limit 100 is exceeded", actual.Items[i].SAPErrorCode, actual.Items[i].SAPErrorCode);
            }

        }
    }
}
