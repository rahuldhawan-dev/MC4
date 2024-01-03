using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class ServiceRestorationContractorControllerTest : MapCallMvcControllerTestBase<ServiceRestorationContractorController, ServiceRestorationContractor>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Index" };
            options.DestroyRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Index" };
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Index" };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var role = ServiceRestorationContractorController.ROLE;
                a.RequiresRole("~/ServiceRestorationContractor/Index", role);
                a.RequiresRole("~/ServiceRestorationContractor/Search", role);
                a.RequiresRole("~/ServiceRestorationContractor/New", role, RoleActions.Add);
                a.RequiresRole("~/ServiceRestorationContractor/Create", role, RoleActions.Add);
                a.RequiresRole("~/ServiceRestorationContractor/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/ServiceRestorationContractor/Update", role, RoleActions.Edit);
                a.RequiresRole("~/ServiceRestorationContractor/Destroy", role, RoleActions.Delete);
                a.RequiresLoggedInUserOnly("~/ServiceRestorationContractor/ByOperatingCenterId/");
            });
        }

        #region ByOperatingCenterId

        [TestMethod]
        public void TestByOperatingCenterIdReturnsTownsForOperatingCenter()
        {
            var opc1 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "FOO" });
            var opc2 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "BAR" });
            var src = GetEntityFactory<ServiceRestorationContractor>().Create(new
            {
                OperatingCenter = opc1,
                Contractor = "Buh?",
                FinalRestoration = true,
                PartialRestoration = true
            });
            var invalid = GetEntityFactory<ServiceRestorationContractor>().Create(new
            {
                OperatingCenter = opc2,
                Contractor = "Buh?",
                FinalRestoration = true,
                PartialRestoration = true
            });
            var result = (CascadingActionResult)_target.ByOperatingCenterId(opc1.Id);

            var actual = result.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1); // --select here--
            foreach (var selectListItem in actual)
            {
                Assert.AreNotEqual(invalid.Id.ToString(), selectListItem.Value);
            }
        }

        #endregion
    }
}