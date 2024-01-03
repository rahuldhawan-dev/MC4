using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using SAP.DataTest.Model.Entities;
using StructureMap;
using MapCall.SAP.Model.Repositories;

namespace SAP.DataTest.Model.Repositories
{
    [TestClass()]
    public class SAPWBSElementRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPWBSElementRepository _target;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            // We injected an 
            _container.Inject<ISAPHttpClient>(_container.GetInstance<SAPHttpClient>());

            _target = _container.GetInstance<SAPWBSElementRepository>();

            _sapHttpClient = _container.GetInstance<Mock<ISAPHttpClient>>();

            //_sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWBSElementRepositorySearchTestByWBS()
        {
            var sapWBSElement = _container.GetInstance<SAPWBSElementTest>().SetWBSElementSearchValues();

            SAPWBSElementCollection actual = _target.Search(sapWBSElement);
            for (int i = 0; i < actual.Items.Count; i++)
            {
                Assert.AreEqual("No data displayed due to maximum limit 100 is exceeded", actual.Items[i].SAPErrorCode,
                    actual.Items[i].SAPErrorCode);
            }
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWBSElementRepositorySearchTestNULL()
        {
            var sapWBSElement = _container.GetInstance<SAPWBSElementTest>().SetWBSElementSearchValuesByWSB();

            SAPWBSElementCollection actual = _target.Search(sapWBSElement);
            for (int i = 0; i < actual.Items.Count; i++)
            {
                Assert.AreEqual("No data found for selection criteria", actual.Items[i].SAPErrorCode,
                    actual.Items[i].SAPErrorCode);
            }
        }
    }
}
