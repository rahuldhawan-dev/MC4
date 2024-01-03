using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MapCall.SAP.Model.Entities.Tests;
using StructureMap;
using MapCall.SAP;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;

namespace MapCall.SAP.Model.Repositories.Tests
{
    [TestClass()]
    public class SAPInspectionRepositoryTests
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPInspectionRepository _target;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            // We injected an 
            _container.Inject<ISAPHttpClient>(new SAPHttpClient());

            _target = _container.GetInstance<SAPInspectionRepository>();

            _sapHttpClient = new Mock<ISAPHttpClient>();

            //_sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPInspectionRepositorySaveTestForHydrant()
        {
            var SAPInspection = new SAPInspection(new SAPInspectionTests().SetHydrantInspectionValues());

            SAPInspection actual = _target.Save(SAPInspection);

            Assert.AreEqual("Successful", actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPInspectionRepositorySaveTestForValve()
        {
            var SAPInspection = new SAPInspection(new SAPInspectionTests().SetValveInspectionValues());

            SAPInspection actual = _target.Save(SAPInspection);

            Assert.AreEqual("Successful", actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPInspectionRepositorySaveTestForBlowOff()
        {
            var SAPInspection = new SAPInspection(new SAPInspectionTests().SetBlowOffInspectionValues());

            SAPInspection actual = _target.Save(SAPInspection);

            Assert.AreEqual("Successful", actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPInspectionRepositorySaveTestForSewerMainCleaning()
        {
            var SAPInspection = new SAPInspection(new SAPInspectionTests().SetOpeningInspectionValues());

            SAPInspection actual = _target.Save(SAPInspection);

            Assert.AreEqual("Successful", actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSaveSetsErrorCodeIfSiteIsNotRunning()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
            _container.Inject(_sapHttpClient.Object);

            var entity = new SAPInspection();

            var result = _target.Save(entity);

            Assert.AreEqual(SAPEquipmentRepository.ERROR_NO_SITE_CONNECTION, result.SAPErrorCode);
        }
    }
}
