using System.Text.RegularExpressions;
using MapCall.Common.Model.Entities;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using SAP.DataTest.Model.Entities;
using StructureMap;

namespace SAP.DataTest.Model.Repositories
{
    [TestClass]
    public class SAPCreateUnscheduledWorkOrderRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPCreateUnscheduledWorkOrderRepository _target;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            _container.Inject((_sapHttpClient = new Mock<ISAPHttpClient>()).Object);
            _target = _container.GetInstance<SAPCreateUnscheduledWorkOrderRepository>();
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPCreateUnscheduledWorkOrderTest()
        {
            var sapCreateUnscheduledWorkOrder =
                new SAPCreateUnscheduledWorkOrder(new SAPCreateUnscheduledWorkOrderTest()
                   .GetTestProductionWorkOrderCreate());

            SAPCreateUnscheduledWorkOrder actual = _target.Save(sapCreateUnscheduledWorkOrder);

            MyAssert.IsMatch(new Regex("^Order  was saved with number \\d+ and notification \\d+ successfully"),
                actual.SAPErrorCode, $"Returned Status did not match : {actual.SAPErrorCode}");
        }

        [TestMethod]
        public void TestSaveDoesNotCallIntoSapWhenOrderTypeIsNotSapEnabled()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(true);
            _sapHttpClient.Setup(x => x.CreateUnscheduleWorkOrder(It.IsAny<SAPCreateUnscheduledWorkOrder>()));

            _target.Save(new SAPCreateUnscheduledWorkOrder { IsSAPEnabled = false });

            _sapHttpClient.Verify(x => x.CreateUnscheduleWorkOrder(It.IsAny<SAPCreateUnscheduledWorkOrder>()), Times.Never);
        }

        [TestMethod]
        public void TestSaveCallsIntoSapWhenOrderTypeIsSapEnabled()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(true);
            _sapHttpClient.Setup(x => x.CreateUnscheduleWorkOrder(It.IsAny<SAPCreateUnscheduledWorkOrder>()));

            _target.Save(new SAPCreateUnscheduledWorkOrder { IsSAPEnabled = true });

            _sapHttpClient.Verify(x => x.CreateUnscheduleWorkOrder(It.IsAny<SAPCreateUnscheduledWorkOrder>()), Times.Once);
        }
    }
}
