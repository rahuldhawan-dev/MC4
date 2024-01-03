using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAPTest.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SAP.DataTest.Model.Entities;
using StructureMap;

namespace MapCall.SAPTest.Model.Repositories
{
    [TestClass]
    public class SAPProgressWorkOrderRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPWorkOrderRepository _target;
        private IContainer _container;

        #region Exposed methods

        public SAPWorkOrder CreateWorkOrder()
        {
            var SAPWorkOrder = new SAPWorkOrder(new SAPWorkOrderTest().GetTestWorkOrderForService());

            SAPWorkOrder actual = _target.Save(SAPWorkOrder);

            return actual;
        }

        #endregion

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            // We injected an 
            _container.Inject<ISAPHttpClient>(new SAPHttpClient());

            _target = _container.GetInstance<SAPWorkOrderRepository>();

            _sapHttpClient = new Mock<ISAPHttpClient>();

            //_sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SAPProgressWorkOrderRepositorySaveTestWithCrew()
        {
            var sapWorkOrder = CreateWorkOrder();

            var SAPProgressWorkOrder =
                new SAPProgressWorkOrder(new SAPProcessWorkOrderTest().GetTestProcesWorkOrderWithCrew(sapWorkOrder));

            SAPProgressWorkOrder actual = _target.Update(SAPProgressWorkOrder);

            Assert.AreEqual("Order Updated Successfully", actual.Status, actual.Status);
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SAPProgressWorkOrderRepositorySaveTestWithMaterial()
        {
            var sapWorkOrder = CreateWorkOrder();

            var SAPProgressWorkOrder =
                new SAPProgressWorkOrder(
                    new SAPProcessWorkOrderTest().GetTestProcesWorkOrderWithMaterial(sapWorkOrder));

            SAPProgressWorkOrder actual = _target.Update(SAPProgressWorkOrder);

            Assert.AreEqual("Material 1411970 not found in plant D205 (check entry)", actual.Status, actual.Status);
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SAPProgressWorkOrderRepositorySaveTestWithMaterialNCrew()
        {
            var sapWorkOrder = CreateWorkOrder();

            var SAPProgressWorkOrder =
                new SAPProgressWorkOrder(new SAPProcessWorkOrderTest().GetTestProcesWorkOrderWithCrew(sapWorkOrder));

            SAPProgressWorkOrder actual = _target.Update(SAPProgressWorkOrder);

            Assert.AreEqual("Order Updated Successfully", actual.Status, actual.Status);
        }

        //[TestMethod()]
        //public void SAPProgressWorkOrderRepositorySaveTestForCancel()
        //{
        //    var sapWorkOrder = CreateWorkOrder();

        //    var SAPProgressWorkOrder = new SAPProgressWorkOrder(new SAPProcessWorkOrderTest().GetTestProcesWorkOrderCancel(sapWorkOrder));

        //    SAPProgressWorkOrder actual = _target.Update(SAPProgressWorkOrder);

        //    Assert.AreEqual("Order Cancelled Successfully", actual.Status, actual.Status);
        //}

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SAPProgressWorkOrderRepositorySaveTestForStatusNotAllowToChange()
        {
            var SAPProgressWorkOrder =
                new SAPProgressWorkOrder(new SAPProcessWorkOrderTest().GetTestProcesWorkOrderWithMaterialNCrew(null));

            SAPProgressWorkOrder actual = _target.Update(SAPProgressWorkOrder);

            Assert.AreEqual("Notification cannot be associated with the given work order", actual.Status,
                actual.Status);
        }
    }
}
