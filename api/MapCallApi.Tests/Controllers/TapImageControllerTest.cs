using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using StructureMap;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class TapImageControllerTest : MapCallApiControllerTestBase<TapImageController, TapImage, TapImageRepository>
    {
        #region Private Members

        private IUrlHelper _urlHelper;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IUrlHelper>().Use(_urlHelper = new MapCallUrlHelper());
        }

        protected override User CreateUser()
        {
            var user = GetFactory<AdminUserFactory>().Create(new {
                DefaultOperatingCenter = GetFactory<OperatingCenterFactory>().Create()
            });
            return user;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesImages;

            Authorization.Assert(a => {
                SetupHttpAuth(a);

                a.RequiresRole("~/TapImage/Index/", module);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // override cause of Json result
            var expected = "1234566";
            var tapImage1 = GetEntityFactory<TapImage>().Create(new { PremiseNumber = "MPFCS02E02" });
            var tapImage2 = GetEntityFactory<TapImage>().Create(new { PremiseNumber = expected });
            var search = new SearchTapImage {
                PremiseNumber = expected
            };

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);

            helper.AreEqual(expected, "PremiseNumber");
            helper.AreEqual("http://localhost/Modules/mvc/FieldOperations/TapImage/Show/2.PDF", "URL");
            Assert.AreEqual(1, helper.Count);
        }

        [TestMethod]
        public void TestIndexJSONDoesNotReturnRecordsForPartialMatch()
        {
            var expected = "1234566";
            var tapImage1 = GetEntityFactory<TapImage>().Create(new { PremiseNumber = "MPFCS02E02" });
            var tapImage2 = GetEntityFactory<TapImage>().Create(new { PremiseNumber = expected });
            var search = new SearchTapImage
            {
                PremiseNumber = "123456" // only part of the number
            };

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);

            Assert.AreEqual(0, helper.Count);
        }

        #endregion
    }
}
