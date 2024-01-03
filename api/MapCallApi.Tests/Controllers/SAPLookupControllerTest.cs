using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class SAPLookupControllerTest : MapCallApiControllerTestBase<SAPLookupController, ServiceInstallationPosition>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            var user = GetFactory<AdminUserFactory>().Create(new
            {
                DefaultOperatingCenter = GetFactory<OperatingCenterFactory>().Create()
            });

            Session.Save(user.UserType);

            return user;
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = SAPLookupController.ROLE;
            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/SAPLookup/Index", module);
            });
        }

        [TestMethod]
        public override void TestIndexCanPerformSearchForAllSearchModelProperties()
        {
            // override because not ISearchSet and also because SAP
            Assert.Inconclusive("Test me");
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // override: not ISearchSet
            Assert.Inconclusive("noop me if TestIndexReturnsANumberOfDataThings covers this");
        }

        [TestMethod]
        public void TestIndexReturnsANumberOfDataThings()
        {
            var l1 = GetEntityFactory<ServiceInstallationPosition>().Create(new { SAPCode = "Foo", CodeGroup = "A"});
            var l2 = GetEntityFactory<SAPWorkOrderPurpose>().Create(new { Code = "bar", CodeGroup = "B" });

            var result = _target.Index(new SearchSAPLookup());
            var helper = new JsonResultTester(result);

            helper.AreEqual(l1.SAPCode, "Code");
            helper.AreEqual(l2.Code, "Code", 1);
        }

        #endregion
    }
}
