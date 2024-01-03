using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SAP.DataTest.Model.Entities;
using StructureMap;
// ReSharper disable InconsistentNaming

namespace SAP.DataTest.Model.Repositories
{
    [TestClass()]
    public class SAPProgressUnscheduledWorkOrderRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPProgressUnscheduledWorkOrderRepository _target;

        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_sapHttpClient = new Mock<ISAPHttpClient>()).Object);

            _target = _container.GetInstance<SAPProgressUnscheduledWorkOrderRepository>();
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SapCreateUnscheduledWorkOrderTest()
        {
            var sapProgressUnscheduledWorkOrder =
                new SAPProgressUnscheduledWorkOrder(new SAPProgressUnscheduledWorkOrderTest()
                   .GetTestProductionWorkOrderProgress());

            SAPProgressUnscheduledWorkOrder actual = _target.Save(sapProgressUnscheduledWorkOrder);

            Assert.AreEqual("Order Updated Successfully", actual.SAPErrorCode);
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SapCreateUnscheduledWorkOrderTestForMaterial()
        {
            var sapProgressUnscheduledWorkOrder = new SAPProgressUnscheduledWorkOrder(
                new SAPProgressUnscheduledWorkOrderTest().GetTestProductionWorkOrderProgressForMaterial());

            SAPProgressUnscheduledWorkOrder actual = _target.Save(sapProgressUnscheduledWorkOrder);

            //Assert.AreEqual("Entry Material = ABC does not exist in MARA (check entry)", actual.SAPErrorCode);
            Assert.AreEqual("Order Updated Successfully", actual.SAPErrorCode);
        }

        //[TestMethod]
        //TODO: Fix this once we can stand up something for SAP
        public void SapProgressWorkOrderChildNotificationTest()
        {
            var sapProgressUnscheduledWorkOrder =
                new SAPProgressUnscheduledWorkOrder(new SAPProgressUnscheduledWorkOrderTest()
                   .GetTestScheduleWorkOrderProgress());

            SAPProgressUnscheduledWorkOrder actual = _target.Save(sapProgressUnscheduledWorkOrder);

            Assert.AreEqual("Order Updated Successfully", actual.SAPErrorCode);
        }

        [TestMethod]
        public void TestSaveDoesNotCallIntoSapWhenOrderTypeIsNotSapEnabled()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(true);
            _sapHttpClient.Setup(x => x.ProgressUnscheduleWorkOrder(It.IsAny<SAPProgressUnscheduledWorkOrder>()));

            _target.Save(new SAPProgressUnscheduledWorkOrder { IsSAPEnabled = false });

            _sapHttpClient.Verify(x => x.ProgressUnscheduleWorkOrder(It.IsAny<SAPProgressUnscheduledWorkOrder>()), Times.Never);
        }

        [TestMethod]
        public void TestSaveCallsIntoSapWhenOrderTypeIsSapEnabled()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(true);
            _sapHttpClient.Setup(x => x.ProgressUnscheduleWorkOrder(It.IsAny<SAPProgressUnscheduledWorkOrder>()));

            _target.Save(new SAPProgressUnscheduledWorkOrder { IsSAPEnabled = true });

            _sapHttpClient.Verify(x => x.ProgressUnscheduleWorkOrder(It.IsAny<SAPProgressUnscheduledWorkOrder>()), Times.Once);
        }
    }
}
