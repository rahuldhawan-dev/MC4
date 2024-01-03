using System.Collections.Generic;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class ValveSizeControllerTest : MapCallApiControllerTestBase<ValveSizeController, ValveSize, IRepository<ValveSize>>
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
            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/ValveSize/Index", ValveSizeController.ROLE);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var valveSize1 = GetEntityFactory<ValveSize>().Create(new {
                Size = 1.5m,
                SizeRange = Valve.Display.SIZE_RANGE_SMALL_VALVE,
                Description = "1 1/2"
            });
            var valveSize13 = GetEntityFactory<ValveSize>().Create(new {
                Size = 13m,
                SizeRange = Valve.Display.SIZE_RANGE_LARGE_VALVE,
                Description = "13"
            });
            var search = new SearchValveSize();

            var result = _target.Index(search) as JsonResult;
            var helper = new JsonResultTester(result);
            
            Assert.AreEqual(2, helper.Count);
            helper.AreEqual(1.5m, "Size", 0);
            helper.AreEqual(Valve.Display.SIZE_RANGE_SMALL_VALVE, "SizeRange", 0);
            helper.AreEqual("1.5", "Description", 0);
            helper.AreEqual(13m, "Size", 1);
            helper.AreEqual(Valve.Display.SIZE_RANGE_LARGE_VALVE, "SizeRange", 1);
            helper.AreEqual("13", "Description", 1);
        }

        #endregion
    }
}
