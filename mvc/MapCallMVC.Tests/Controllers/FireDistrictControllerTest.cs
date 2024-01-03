using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class FireDistrictControllerTest
        : MapCallMvcControllerTestBase<FireDistrictController, FireDistrict, FireDistrictRepository>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesDataLookups;
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/FireDistrict/ByTownId");
                a.RequiresLoggedInUserOnly("~/FireDistrict/GetPremiseNumber");
                a.RequiresRole("~/FireDistrict/Search", module);
                a.RequiresRole("~/FireDistrict/Show", module);
                a.RequiresRole("~/FireDistrict/Index", module);
                a.RequiresRole("~/FireDistrict/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/FireDistrict/Update", module, RoleActions.Edit);
                a.RequiresRole("~/FireDistrict/New", module, RoleActions.Add);
                a.RequiresRole("~/FireDistrict/Create", module, RoleActions.Add);
                a.RequiresRole("~/FireDistrict/Destroy", module, RoleActions.Delete);
            });
        }

        #endregion

        #region Ajaxy Stuff

        [TestMethod]
        public void TestGetPremiseNumberReturnsPremiseNumberOfFireDistrict()
        {
            var premNum = "12345";
            var fd = GetFactory<FireDistrictFactory>().Create(new{ PremiseNumber = premNum });
            var result = (JsonResult)_target.GetPremiseNumber(fd.Id);
            dynamic model = result.Data;
            Assert.AreEqual(premNum, model.premiseNumber);
        }

        [TestMethod]
        public void TestGetPremiseNumberReturnsEmptyPremiseNumberIfTheFireDistrictDoesNotExist()
        {
            var result = (JsonResult)_target.GetPremiseNumber(0);
            dynamic model = result.Data;
            Assert.IsNull(model.premiseNumber);
        }
        
        #endregion
    }
}
