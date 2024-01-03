using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TrainingClassificationSumControllerTest : MapCallMvcControllerTestBase<TrainingClassificationSumController, PositionGroup>
    {
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = (tester) => {
                tester.TestPropertyValues[nameof(SearchTrainingClassificationSum.Year)] = 2020;
            };
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = TrainingClassificationSumController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/TrainingClassificationSum/Search/", role);
                a.RequiresRole("~/Reports/TrainingClassificationSum/Index/", role);
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
