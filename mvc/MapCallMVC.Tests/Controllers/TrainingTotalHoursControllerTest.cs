using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TrainingTotalHoursControllerTest : MapCallMvcControllerTestBase<TrainingTotalHoursController, TrainingRecord>
    {
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = (tester) => {
                tester.TestPropertyValues[nameof(SearchTrainingTotalHours.Year)] = 2020;
            };
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = TrainingTotalHoursController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/TrainingTotalHours/Search/", role);
                a.RequiresRole("~/Reports/TrainingTotalHours/Index/", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            Assert.Inconclusive("Test me");
        }
    }
}
