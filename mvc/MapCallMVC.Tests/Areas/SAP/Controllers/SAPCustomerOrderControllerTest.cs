using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MapCall.SAP.Model.Entities;
using MapCallMVC.Areas.SAP.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using Moq;
using SAP.DataTest.Model.Repositories;

namespace MapCallMVC.Tests.Areas.SAP.Controllers
{
    [TestClass]
    public class SAPCustomerOrderControllerTest : MapCallMvcControllerTestBase<SAPCustomerOrderController, WorkOrder>
    {
        #region Private Members

        private Mock<ISAPCustomerOrderRepository> _sapCustomerOrderRepository;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _sapCustomerOrderRepository = new Mock<ISAPCustomerOrderRepository>();
            _container.Inject(_sapCustomerOrderRepository.Object);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/SAP/SAPCustomerOrder/Index", role, RoleActions.Read);
                a.RequiresRole("~/SAP/SAPCustomerOrder/Show", role, RoleActions.Read);
                a.RequiresRole("~/SAP/SAPCustomerOrder/Search", role, RoleActions.Read);
            });
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // noop: not an ISearchSet, doesn't use ActionHelper.
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfModelStateIsInvalid()
        {
            Assert.Inconclusive("Implement and test me");
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search model is not ISearchSet.
            var expected = new SAPCustomerOrderCollection();
            _sapCustomerOrderRepository.Setup(x => x.Search(It.IsAny<SearchSapCustomerOrder>())).Returns(expected);

            var result = _target.Index(new SearchSapCustomerOrder()) as ViewResult;
            var resultModel = result.Model as IEnumerable<SAPCustomerOrder>;

            Assert.IsNotNull(result);
            Assert.IsNotNull(resultModel);
            Assert.AreSame(expected, resultModel);
            MvcAssert.IsViewNamed(result, "Index");
        }

        [TestMethod]
        public void TestIndexReturnsShowWhenSearchingOnWorkOrder()
        {
            var expected = new SAPCustomerOrderCollection();
            _sapCustomerOrderRepository.Setup(x => x.Search(It.IsAny<SearchSapCustomerOrder>())).Returns(expected);

            var result = _target.Index(new SearchSapCustomerOrder { WorkOrder = "1234" }) as RedirectToRouteResult;
            
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["action"], "Show");
        }

        [TestMethod]
        public override void TestIndexRedirectsToSearchIfThereAreZeroResults()
        {
            // override needed because model is not ISearchSet
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // override needed due to SAP repo stuff.
            var expectedCollection = new SAPCustomerOrderCollection();
            var expected = new SAPCustomerOrder();
            expectedCollection.Items.Add(expected);

            _sapCustomerOrderRepository.Setup(x => x.Search(It.IsAny<SearchSapCustomerOrder>())).Returns(expectedCollection);

            var result = _target.Show(666) as ViewResult;
            var resultModel = result.Model as SAPCustomerOrder;

            Assert.IsNotNull(result);
            Assert.IsNotNull(resultModel);
            Assert.AreSame(expected, resultModel);
            MvcAssert.IsViewNamed(result, "Show");
        }

        [TestMethod]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            Assert.Inconclusive("I don't do this, but I probably should.");
        }
    }
}
