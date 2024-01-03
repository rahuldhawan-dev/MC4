using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class ScheduleOfValueControllerTest : MapCallMvcControllerTestBase<ScheduleOfValueController, ScheduleOfValue>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/FieldOperations/ScheduleOfValue/ByScheduleOfValueCategoryId/");
            });
        }

        [TestMethod]
        public void TestByScheduleOfValueCategoryIdReturnsCascadingThings()
        {
            var scheduleOfValueCategories = GetEntityFactory<ScheduleOfValueCategory>().CreateList(2);
            var scheduleOfValues1 = GetEntityFactory<ScheduleOfValue>()
                .CreateList(3, new {ScheduleOfValueCategory = scheduleOfValueCategories[0]});
            var scheduleOfValues2 = GetEntityFactory<ScheduleOfValue>()
                .CreateList(13, new {ScheduleOfValueCategory = scheduleOfValueCategories[1]});

            var results = _target.ByScheduleOfValueCategoryId(scheduleOfValueCategories[1].Id) as CascadingActionResult;

            Assert.AreEqual(13, results.GetSelectListItems().Count()-1);
        }
    }
}