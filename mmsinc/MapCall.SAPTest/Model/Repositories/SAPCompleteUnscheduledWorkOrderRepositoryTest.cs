using System.Text.RegularExpressions;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using SAP.DataTest.Model.Entities;
using StructureMap;
using MapCall.SAPTest.Model.Entities;

namespace MapCall.SAPTest.Model.Repositories
{
    [TestClass]
    public class SAPCompleteUnscheduledWorkOrderRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPCompleteUnscheduledWorkOrderRepository _target;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_sapHttpClient = new Mock<ISAPHttpClient>()).Object);
            _target = _container.GetInstance<SAPCompleteUnscheduledWorkOrderRepository>();
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SapCompleteUnscheduledWorkOrderTestForSuccess()
        {
            var sapCompleteUnscheduledWorkOrder =
                new SAPCompleteUnscheduledWorkOrder(new SapCompleteUnscheduledWorkOrderTest()
                   .GetTestProductionWorkOrderComplete());

            SAPCompleteUnscheduledWorkOrder actual = _target.Save(sapCompleteUnscheduledWorkOrder);

            Assert.AreEqual("Order Updated Successfully", actual.SAPErrorCode);
        }

        [TestMethod]
        public void TestSaveDoesNotCallIntoSapWhenOrderTypeIsNotSapEnabled()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(true);
            _sapHttpClient.Setup(x => x.CompleteUnscheduleWorkOrder(It.IsAny<SAPCompleteUnscheduledWorkOrder>()));

            _target.Save(new SAPCompleteUnscheduledWorkOrder { IsSAPEnabled = false });

            _sapHttpClient.Verify(x => x.CompleteUnscheduleWorkOrder(It.IsAny<SAPCompleteUnscheduledWorkOrder>()), Times.Never);
        }

        [TestMethod]
        public void TestSaveCallsIntoSapWhenOrderTypeIsSapEnabled()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(true);
            _sapHttpClient.Setup(x => x.CompleteUnscheduleWorkOrder(It.IsAny<SAPCompleteUnscheduledWorkOrder>()));

            _target.Save(new SAPCompleteUnscheduledWorkOrder { IsSAPEnabled = true });

            _sapHttpClient.Verify(x => x.CompleteUnscheduleWorkOrder(It.IsAny<SAPCompleteUnscheduledWorkOrder>()), Times.Once);
        }
    }
}
