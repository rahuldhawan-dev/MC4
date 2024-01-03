using System.Net;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Testing;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class FireDistrictControllerTest : MapCallApiControllerTestBase<FireDistrictController, FireDistrict,
        FireDistrictRepository>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesDataLookups;
            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/FireDistrict/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // no op
        }

        [TestMethod]
        public void TestIndexReturnsNoResultsWhenTownIsNotProvided()
        {
            _target.ModelState.AddModelError("Town", "Required");
            var result = (JsonHttpStatusCodeResult)_target.Index(new SearchFireDistrict());

            Assert.AreEqual((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.AreEqual(FireDistrictController.TOWN_REQUIRED_ERROR, result.StatusDescription);
        }

        [TestMethod]
        public void TestIndexReturnsResultsForTown()
        {
            var town = GetEntityFactory<Town>().CreateList(2);
            var fireDistricts1 = GetEntityFactory<FireDistrict>().Create(new { DistrictName = "abc" });
            var fireDistricts2 = GetEntityFactory<FireDistrict>().Create(new { DistrictName = "xyz" });

            GetEntityFactory<FireDistrictTown>().Create(new {
                Town = town[0],
                FireDistrict = fireDistricts1,
            });
            GetEntityFactory<FireDistrictTown>().Create(new {
                Town = town[0],
                FireDistrict = fireDistricts2,
            });
            GetEntityFactory<FireDistrictTown>().Create(new {
                Town = town[1],
                FireDistrict = fireDistricts1,
            });

            var search = new SearchFireDistrict { Town = town[0].Id };

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);

            Assert.AreEqual(2, helper.Count);
            helper.AreEqual(1, "Id", 0);
            helper.AreEqual(2, "Id", 1);
            helper.AreEqual("abc", "DistrictName", 0);
            helper.AreEqual("xyz", "DistrictName", 1);
        }

        #endregion
    }
}
