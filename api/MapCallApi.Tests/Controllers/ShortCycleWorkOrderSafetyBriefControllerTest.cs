using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models.ShortCycleWorkOrders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using Newtonsoft.Json;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class ShortCycleWorkOrderSafetyBriefControllerTest : MapCallApiControllerTestBase<ShortCycleWorkOrderSafetyBriefController, ShortCycleWorkOrderSafetyBrief, RepositoryBase<ShortCycleWorkOrderSafetyBrief>>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.OperationsHealthAndSafety;

            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/ShortCycleWorkOrderSafetyBrief/Index/", module);
            });
        }

        #endregion

        #region Search/Index/Show

        [TestMethod]
        public void TestIndexJSONExportsJSON()
        {
            var now = DateTime.Today;
            var employee1 = GetFactory<EmployeeFactory>().Create(new { FirstName = "Tina", LastName = "Belcher"});
            var employee2 = GetFactory<EmployeeFactory>().Create(new { FirstName = "Gene", LastName = "Belcher"});
            var entity0 = GetEntityFactory<ShortCycleWorkOrderSafetyBrief>().Create(new { FSR = employee1, DateCompleted = now.AddDays(-1) });
            var entity1 = GetEntityFactory<ShortCycleWorkOrderSafetyBrief>().Create(new { FSR = employee1, DateCompleted = now });
            var entity2 = GetEntityFactory<ShortCycleWorkOrderSafetyBrief>().Create(new { FSR = employee2, DateCompleted = now });
            var search = new SearchShortCycleWorkOrderSafetyBrief
            {
                DateCompleted = new DateRange
                {
                    End = now,
                    Operator = RangeOperator.Equal
                }
            };

            var result = _target.Index(search) as ContentResult;
            var resultObj = (dynamic)JsonConvert.DeserializeObject(result.Content);

            void assertAsStringsAreEqual(object x, object y) => Assert.AreEqual(x.ToString(), y.ToString());
            assertAsStringsAreEqual(entity1.Id, resultObj[0].Id);
            assertAsStringsAreEqual(entity2.Id, resultObj[1].Id);
            assertAsStringsAreEqual(employee1, resultObj[0].FSR);
            assertAsStringsAreEqual(employee2, resultObj[1].FSR);
        }
        
        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            //noop goes the dynamite or so they say
            //json is returned in this instance, tested above
        }

        #endregion
    }
}
