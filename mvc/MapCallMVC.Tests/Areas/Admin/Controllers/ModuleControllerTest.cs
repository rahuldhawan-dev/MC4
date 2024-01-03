using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Areas.Admin.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Areas.Admin.Controllers
{
    [TestClass]
    public class ModuleControllerTest : MapCallMvcControllerTestBase<ModuleController, Module, ModuleRepository>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/Admin/Module/ByApplication/");
            });
        }

        #endregion

        #region ByApplication

        [TestMethod]
        public void TestByApplicationReturnsModuleRecordsFilteredByApplication()
        {
            var waterQualityApplication = GetEntityFactory<Application>().Create(new {
                Id = RoleApplications.WaterQuality
            });
            var productionApplication = GetEntityFactory<Application>().Create(new {
                Id = RoleApplications.Production
            });

            var waterQualityModule = GetEntityFactory<Module>().Create(new {
                Id = RoleModules.WaterQualityGeneral, 
                Application = waterQualityApplication
            });
            var productionModule1 = GetEntityFactory<Module>().Create(new {
                Id = RoleModules.ProductionFacilities, 
                Application = productionApplication
            });
            var productionModule2 = GetEntityFactory<Module>().Create(new {
                Id = RoleModules.ProductionEquipment, 
                Application = productionApplication
            });

            Session.Flush();

            var actionResult = (CascadingActionResult)_target.ByApplication(new [] { waterQualityApplication.Id, productionApplication.Id });
            var moduleDisplayItems = ((IEnumerable<ModuleDisplayItem>)actionResult.Data).ToList();

            Assert.AreEqual(1, moduleDisplayItems.Count(x => x.Id == waterQualityModule.Id));
            Assert.AreEqual(1, moduleDisplayItems.Count(x => x.Id == productionModule1.Id));
            Assert.AreEqual(1, moduleDisplayItems.Count(x => x.Id == productionModule2.Id));
        }

        #endregion
    }
}
