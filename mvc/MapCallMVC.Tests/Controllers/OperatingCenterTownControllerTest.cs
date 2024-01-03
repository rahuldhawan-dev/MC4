using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class OperatingCenterTownControllerTest : MapCallMvcControllerTestBase<OperatingCenterTownController, OperatingCenterTown>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
                var town = GetEntityFactory<Town>().Create();
                var operatingCenterTown = GetEntityFactory<OperatingCenterTown>().Create(new { OperatingCenter = operatingCenter, Town = town, Abbreviation = "FOO" });
                return operatingCenterTown;
            };
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Show", controller = "Town", area = "", id = vm.Town };
            options.DestroyRedirectsToRouteOnSuccessArgs = (id) => {
                var town = Repository.Find(id)?.Town.Id;
                return new { action = "Show", controller = "Town", area = "", id = town };
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresSiteAdminUser("~/OperatingCenterTown/Create");
                a.RequiresSiteAdminUser("~/OperatingCenterTown/Destroy");
            });
        }
    }
}