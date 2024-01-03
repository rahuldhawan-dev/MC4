using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SAP.DataTest.Model.Entities;
using StructureMap;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;

namespace SAP.DataTest.Model.Repositories
{
    [TestClass()]
    public class SAPNewServiceInstallationRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPNewServiceInstallationRepository _target;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            // We injected an 
            _container.Inject<ISAPHttpClient>(_container.GetInstance<SAPHttpClient>());

            _target = _container.GetInstance<SAPNewServiceInstallationRepository>();

            //_sapHttpClient = _container.GetInstance<Mock<ISAPHttpClient>>();  
            _sapHttpClient = new Mock<ISAPHttpClient>();
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SAPNewServiceInstallationSaveTest()
        {
            var SAPNewServiceInstallation =
                _container.GetInstance<SAPNewServiceInstallationTest>().SetNewServiceInstallation();
            SAPNewServiceInstallation actual = _target.Save(SAPNewServiceInstallation);
            Assert.AreEqual("Month 00 is not plausible", actual.SAPStatus, actual.SAPStatus);
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SAPServiceSaveTest()
        {
            var SAPNewServiceInstallation = _container.GetInstance<SAPNewServiceInstallationTest>().SetService();
            SAPNewServiceInstallation actual = _target.SaveService(SAPNewServiceInstallation);
            Assert.AreEqual("Device Location Updated Successfully", actual.SAPStatus, actual.SAPStatus);
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SAPServiceSaveTestForNewServiceMaterial()
        {
            var SAPNewServiceInstallation = _container.GetInstance<SAPNewServiceInstallationTest>()
                                                      .SetServiceForNewServiceMaterial();
            SAPNewServiceInstallation actual = _target.SaveService(SAPNewServiceInstallation);
            Assert.AreEqual("Device Location Updated Successfully", actual.SAPStatus, actual.SAPStatus);
        }
    }
}
