using System.Text.RegularExpressions;
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
    public class SAPCreatePreventiveWorkOrderRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPCreatePreventiveWorkOrderRepository _target;
        private IContainer _container;

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container();
            // We injected an 
            _container.Inject<ISAPHttpClient>(_container.GetInstance<SAPHttpClient>());

            _target = _container.GetInstance<SAPCreatePreventiveWorkOrderRepository>();
            _sapHttpClient = _container.GetInstance<Mock<ISAPHttpClient>>();

            //_sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SapCreatePreventiveWorkOrderTestForEmptyValues()
        {
            var SapCreatePreventiveWorkOrder = _container.GetInstance<SAPCreatePreventiveWorkOrderTest>()
                                                         .GetSAPCreatePreventiveWorkOrderTestForEmptyValues();

            var actual = _target.Search(SapCreatePreventiveWorkOrder);

            Assert.AreEqual("Successfully", actual.Items[0].SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SapCreatePreventiveWorkOrderTestForPlanningPlat()
        {
            var SapCreatePreventiveWorkOrder = _container.GetInstance<SAPCreatePreventiveWorkOrderTest>()
                                                         .GetSAPCreatePreventiveWorkOrderTestForPlanningPlat();

            var actual = _target.Search(SapCreatePreventiveWorkOrder);

            Assert.AreEqual("Successfully", actual.Items[0].SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SapCreatePreventiveWorkOrderTestForCompanyCode()
        {
            var SapCreatePreventiveWorkOrder = _container.GetInstance<SAPCreatePreventiveWorkOrderTest>()
                                                         .GetSAPCreatePreventiveWorkOrderTestForCompanyCode();

            var actual = _target.Search(SapCreatePreventiveWorkOrder);

            Assert.AreEqual("Successfully", actual.Items[0].SAPErrorCode);
        }
    }
}
