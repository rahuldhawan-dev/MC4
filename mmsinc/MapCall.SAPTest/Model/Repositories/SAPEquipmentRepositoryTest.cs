using Microsoft.VisualStudio.TestTools.UnitTesting;
using MapCall.SAP;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using Moq;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Entities.Tests;
using StructureMap;

namespace MapCall.SAP.Model.Repositories.Tests
{
    [TestClass]
    public class SAPEquipmentRepositoryTest
    {
        #region Private Members

        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPEquipmentRepository _target;
        private IContainer _container;

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject<ISAPHttpClient>(new SAPHttpClient());
            _target = _container.GetInstance<SAPEquipmentRepository>();
            _sapHttpClient = new Mock<ISAPHttpClient>();
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(true);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSaveSetsErrorCodeIfSiteIsNotRunning()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
            _container.Inject(_sapHttpClient.Object);

            var entity = new SAPEquipment();

            var result = _target.Save(entity);

            Assert.AreEqual(SAPEquipmentRepository.ERROR_NO_SITE_CONNECTION, result.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPEquipmentRepositorySaveTestForHydrant()
        {
            var SAPEquipment = new SAPEquipment(new SapEquipmentTest().GetTestHydrant());

            SAPEquipment actual = _target.Save(SAPEquipment);

            Assert.AreEqual("Equipment Created Successfully", actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPEquipmentRepositorySaveTestForOpening()
        {
            var SAPEquipment = new SAPEquipment(new SapEquipmentTest().GetOpening());

            SAPEquipment actual = _target.Save(SAPEquipment);

            Assert.AreEqual("Equipment Created Successfully", actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPEquipmentRepositorySaveTestForValve()
        {
            var SAPEquipment = new SAPEquipment(new SapEquipmentTest().GetTestValve());

            SAPEquipment actual = _target.Save(SAPEquipment);

            Assert.AreEqual("Equipment Created Successfully", actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPEquipmentRepositorySaveTestForBlowOff()
        {
            var SAPEquipment = new SAPEquipment(new SapEquipmentTest().GetTestBlowOff());

            SAPEquipment actual = _target.Save(SAPEquipment);

            Assert.AreEqual("Equipment Created Successfully", actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPEquipmentRepositorySaveTestForEquipment()
        {
            var SAPEquipment = new SAPEquipment(new SapEquipmentTest().GetTestEquipmentENG());

            SAPEquipment actual = _target.Save(SAPEquipment);

            Assert.AreEqual("Equipment Created Successfully", actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPEquipmentRepositorySaveTestForGenericEquipment()
        {
            var SAPEquipment = new SAPEquipment(new SapEquipmentTest().GetTestEquipmentGeneric());

            SAPEquipment actual = _target.Save(SAPEquipment);

            Assert.AreEqual("Equipment Created Successfully", actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSaveSendsSapEquipmentToHttpClientGetEquipmentResponse()
        {
            //arrange
            var SAPEquipment = new SAPEquipment(new SapEquipmentTest().GetTestHydrant());
            _sapHttpClient.Setup(x => x.GetEquipmentResponse(SAPEquipment)).Returns(() => SAPEquipment);

            _container.Inject(_sapHttpClient.Object);

            //act
            var result = _target.Save(SAPEquipment);

            //assert
            Assert.AreEqual(SAPEquipment.InventoryNumber, result.InventoryNumber);
            _sapHttpClient.Verify(x => x.GetEquipmentResponse(SAPEquipment), Times.Exactly(1));
        }
    }
}
