using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using System.Web.Mvc;
using MapCallMVC.Models.ViewModels;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class OperatingCenterStockedMaterialControllerTest : MapCallMvcControllerTestBase<OperatingCenterStockedMaterialController, OperatingCenterStockedMaterial>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Index" };
            options.DestroyRedirectsToRouteOnSuccessArgs = (vm) => new { action = "Index" };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateOperatingCenterStockedMaterial)vm;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
                model.Material = GetEntityFactory<Material>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = OperatingCenterStockedMaterialController.ROLE;
                a.RequiresRole("~/OperatingCenterStockedMaterial/Index", module, RoleActions.Read);
                a.RequiresRole("~/OperatingCenterStockedMaterial/Search", module, RoleActions.Read);
                a.RequiresRole("~/OperatingCenterStockedMaterial/New", module, RoleActions.Add);
                a.RequiresRole("~/OperatingCenterStockedMaterial/Create", module, RoleActions.Add);
                a.RequiresRole("~/OperatingCenterStockedMaterial/Destroy", module, RoleActions.Delete);
                a.RequiresLoggedInUserOnly("~/OperatingCenterStockedMaterial/StockMaterialSearchByOperatingCenter");
            });
        }

        #region Tests

        [TestMethod]
        public void TestStockMaterialSearchByOperatingCenterReturnsData()
        {
            var opc = GetEntityFactory<OperatingCenter>().Create();
            var material1 = GetEntityFactory<Material>().Create(new {
                IsActive = true,
                PartNumber = "1410464"
            });
            var material2 = GetEntityFactory<Material>().Create(new {
                IsActive = true,
                Description = "CROSS,DI,IMP,MJ,10"
            });
            var material3 = GetEntityFactory<Material>().Create(new {
                IsActive = true,
                Description = "CROSS,DI,IMP,MJ,10X6"
            });
            var material4 = GetEntityFactory<Material>().Create(new {
                IsActive = true,
                PartNumber = "PN1234"
            });

            var sm1 = GetEntityFactory<OperatingCenterStockedMaterial>().Create(new {
                OperatingCenter = opc,
                Material = material1
            });
            var sm2 = GetEntityFactory<OperatingCenterStockedMaterial>().Create(new {
                OperatingCenter = opc,
                Material = material2
            });
            var sm3 = GetEntityFactory<OperatingCenterStockedMaterial>().Create(new {
                OperatingCenter = opc,
                Material = material3
            });
            var sm4 = GetEntityFactory<OperatingCenterStockedMaterial>().Create(new {
                OperatingCenter = opc,
                Material = material4
            });

            var result = _target.StockMaterialSearchByOperatingCenter("10", opc.Id) as JsonResult;

            var resultData = (SelectList)result.Data.GetPropertyValueByName("Options");

            Assert.AreEqual(3, resultData.Count());
            Assert.IsTrue(resultData.Any(x => x.Text == sm1.Material.FullDescription));
            Assert.IsTrue(resultData.Any(x => x.Text == sm2.Material.FullDescription));
            Assert.IsTrue(resultData.Any(x => x.Text == sm3.Material.FullDescription));
            Assert.IsFalse(resultData.Any(x => x.Text == sm4.Material.FullDescription));

            Assert.IsTrue(resultData.Any(x => x.Value == sm1.Material.Id.ToString()));
            Assert.IsTrue(resultData.Any(x => x.Value == sm2.Material.Id.ToString()));
            Assert.IsTrue(resultData.Any(x => x.Value == sm3.Material.Id.ToString()));
            Assert.IsFalse(resultData.Any(x => x.Value == sm4.Material.Id.ToString()));
        }

        #endregion
    }
}
