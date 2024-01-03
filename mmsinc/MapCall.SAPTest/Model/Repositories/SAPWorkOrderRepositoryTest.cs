using System.Text.RegularExpressions;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCall.SAPTest.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using SAP.DataTest.Model.Entities;
using StructureMap;

namespace MapCall.SAPTest.Model.Repositories
{
    [TestClass()]
    public class SAPWorkOrderRepositoryTest
    {
        private Mock<ISAPHttpClient> _sapHttpClient;

        private SAPWorkOrderRepository _target;
        private IContainer _container;

        public object SAPWorkOrderNotification { get; private set; }

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

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWorkOrderRepositorySaveNullTest()
        {
            //new SAPWorkOrderTest().GetTestWorkOrder()
            var SAPWorkOrder = new SAPWorkOrder();

            SAPWorkOrder actual = _target.Save(SAPWorkOrder);

            Assert.AreEqual("No data found", actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWorkOrderRepositorySaveTestForHydrant()
        {
            //
            var SAPWorkOrder = new SAPWorkOrder(new SAPWorkOrderTest().GetTestWorkOrderForHydrant());

            SAPWorkOrder actual = _target.Save(SAPWorkOrder);

            //string sPattern = "^Order  was saved with number \\d{8} and notification \\d{8} successfully";

            MyAssert.IsMatch(new Regex("^Order  was saved with number \\d+ and notification \\d+ successfully"),
                actual.SAPErrorCode, $"Returned Status did not match : {actual.SAPErrorCode}");
            //var RegValue = Regex.IsMatch(actual.SAPErrorCode, sPattern);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWorkOrderRepositorySaveTestForValve()
        {
            var SAPWorkOrder = new SAPWorkOrder(new SAPWorkOrderTest().GetTestWorkOrderForValve());

            SAPWorkOrder actual = _target.Save(SAPWorkOrder);

            MyAssert.IsMatch(new Regex("^Order  was saved with number \\d+ and notification \\d+ successfully"),
                actual.SAPErrorCode, $"Returned Status did not match : {actual.SAPErrorCode}");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWorkOrderRepositorySaveTestForSewerOpening()
        {
            var SAPWorkOrder = new SAPWorkOrder(new SAPWorkOrderTest().GetTestWorkOrderForSewerOpening());

            SAPWorkOrder actual = _target.Save(SAPWorkOrder);

            MyAssert.IsMatch(new Regex("^Order  was saved with number \\d+ and notification \\d+ successfully"),
                actual.SAPErrorCode, $"Returned Status did not match : {actual.SAPErrorCode}");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWorkOrderRepositorySaveTestForMain()
        {
            //
            var SAPWorkOrder = new SAPWorkOrder(new SAPWorkOrderTest().GetTestWorkOrderForMain());

            SAPWorkOrder actual = _target.Save(SAPWorkOrder);

            MyAssert.IsMatch(new Regex("^Order  was saved with number \\d+ and notification \\d+ successfully"),
                actual.SAPErrorCode, $"Returned Status did not match : {actual.SAPErrorCode}");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWorkOrderRepositorySaveTestForSewerMain()
        {
            //
            var SAPWorkOrder = new SAPWorkOrder(new SAPWorkOrderTest().GetTestWorkOrderForSewerMain());

            SAPWorkOrder actual = _target.Save(SAPWorkOrder);

            Assert.AreEqual("Enter a plant", actual.SAPErrorCode, actual.SAPErrorCode);
            //MyAssert.IsMatch(new Regex("^Order  was saved with number \\d+ and notification \\d+ successfully"), actual.SAPErrorCode, $"Returned Status did not match : {actual.SAPErrorCode}");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWorkOrderRepositorySaveTestForService()
        {
            //
            var SAPWorkOrder = new SAPWorkOrder(new SAPWorkOrderTest().GetTestWorkOrderForService());

            SAPWorkOrder actual = _target.Save(SAPWorkOrder);

            MyAssert.IsMatch(new Regex("^Order  was saved with number \\d+ and notification \\d+ successfully"),
                actual.SAPErrorCode, $"Returned Status did not match : {actual.SAPErrorCode}");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWorkOrderRepositorySaveTestForSewerLateral()
        {
            var SAPWorkOrder = new SAPWorkOrder(new SAPWorkOrderTest().GetTestWorkOrderForSewerLateral());
            SAPWorkOrder actual = _target.Save(SAPWorkOrder);
            Assert.AreEqual("Order type MAT code comb. not maintianed in Z tables of settl rule enhan.",
                actual.SAPErrorCode, actual.SAPErrorCode);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWorkOrderRepositoryCompleteWorkOrder()
        {
            //
            var SAPCompleteWorkOrder =
                new SAPCompleteWorkOrder(new SAPCompleteWorkOrderTest().GetTestCompleteWorkOrder());

            SAPCompleteWorkOrder actual = _target.Complete(SAPCompleteWorkOrder);

            MyAssert.IsMatch(new Regex("^Confirmation of order \\d+ saved Successfully"), actual.Status,
                $"Returned Status did not match : {actual.Status}");
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWorkOrderRepositoryCompleteWorkOrderForNULL()
        {
            //
            var SAPCompleteWorkOrder =
                new SAPCompleteWorkOrder(new SAPCompleteWorkOrderTest().GetTestCompleteWorkOrderForNULL());

            SAPCompleteWorkOrder actual = _target.Complete(SAPCompleteWorkOrder);

            Assert.AreEqual("Confirmation of order 90365156 saved Successfully", actual.Status, actual.Status);
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void SAPWorkOrderRepositoryApproveGoodsIssue()
        {
            //
            var sapGoodsIssue = new SAPGoodsIssue(new SAPCompleteWorkOrderTest().GetTestGoodsIssue());

            SAPGoodsIssueCollection actual = _target.Approve(sapGoodsIssue);

            foreach (SAPGoodsIssue GoodsIssue in actual)
            {
                Assert.AreEqual("No items were transferred", GoodsIssue.Status, GoodsIssue.Status);
            }
        }

        //[TestMethod] //TODO: Fix this once we can stand up something for SAP
        public void TestSaveSetsErrorCodeIfSiteIsNotRunning()
        {
            _sapHttpClient.Setup(x => x.IsSiteRunning).Returns(false);
            _container.Inject(_sapHttpClient.Object);

            var entity = new SAPWorkOrder();

            var result = _target.Save(entity);

            Assert.AreEqual(SAPWorkOrderRepository.ERROR_NO_SITE_CONNECTION, result.SAPErrorCode);
        }

        [TestMethod]
        public void SAPWorkOrderRepositoryApproveGoodsIssueFailsProperlyForRetryErrors()
        {
            //
            var sapGoodsIssue = new SAPGoodsIssue(new SAPCompleteWorkOrderTest().GetTestGoodsIssue());
            _target.SAPHttpClient.Password = "BAD P";

            SAPGoodsIssueCollection actual = _target.Approve(sapGoodsIssue);

            foreach (SAPGoodsIssue goodsIssue in actual)
            {
                Assert.IsTrue(goodsIssue.Status.StartsWith("RETRY"));
            }
        }
    }
}
