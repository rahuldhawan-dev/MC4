using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.WaterQuality.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Areas.WaterQuality.Controllers
{
    [TestClass]
    public class WaterConstituentStateLimitControllerTest : MapCallMvcControllerTestBase<WaterConstituentStateLimitController, WaterConstituentStateLimit>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "WaterConstituent", id = vm.WaterConstituent };
            options.DestroyRedirectsToRouteOnSuccessArgs = (id) => {
                var constituentId = Repository.Find(id)?.WaterConstituent.Id;
                return new { action = "Show", controller = "WaterConstituent", id = constituentId };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = WaterConstituentStateLimitController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/WaterQuality/WaterConstituentStateLimit/Create/", role);
                a.RequiresRole("~/WaterQuality/WaterConstituentStateLimit/Destroy/", role);
            });
        }
    }
}
