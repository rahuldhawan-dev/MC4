using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class
        WasteWaterSystemBasinControllerTest : MapCallMvcControllerTestBase<WasteWaterSystemBasinController,
            WasteWaterSystemBasin>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.EnvironmentalGeneral;
            Authorization.Assert(a => {
                a.RequiresRole("~/Environmental/WasteWaterSystemBasin/Show", role);
                a.RequiresRole("~/Environmental/WasteWaterSystemBasin/Search", role);
                a.RequiresRole("~/Environmental/WasteWaterSystemBasin/Index", role);
                a.RequiresSiteAdminUser("~/Environmental/WasteWaterSystemBasin/Edit");
                a.RequiresSiteAdminUser("~/Environmental/WasteWaterSystemBasin/New");
                a.RequiresSiteAdminUser("~/Environmental/WasteWaterSystemBasin/Create");
                a.RequiresSiteAdminUser("~/Environmental/WasteWaterSystemBasin/Update");
                a.RequiresSiteAdminUser("~/Environmental/WasteWaterSystemBasin/Destroy");
                a.RequiresLoggedInUserOnly("~/Environmental/WasteWaterSystemBasin/ByWasteWaterSystem/");
            });
        }

        #region Creation Notification

        [TestMethod]
        public void TestCreateSendsNotification()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();

            var wasteWaterSystem = GetEntityFactory<WasteWaterSystem>().Create(new  {
                OperatingCenter = operatingCenter,
                WasteWaterSystemName = "test name",
            });

            var basin = GetFactory<WasteWaterSystemBasinFactory>().Create();
            var model = _viewModelFactory.BuildWithOverrides<CreateWasteWaterSystemBasin, WasteWaterSystemBasin>(basin, new  {
                WasteWaterSystem = wasteWaterSystem.Id
            });

            _target.Create(model);

            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var wws = GetEntityFactory<WasteWaterSystem>().Create(new { Description = "description 0" });

            var entity0 = GetEntityFactory<WasteWaterSystemBasin>().Create(new { BasinName = "Basin1",WasteWaterSystem = wws });
            var entity1 = GetEntityFactory<WasteWaterSystemBasin>().Create(new { BasinName = "Basin2", WasteWaterSystem = wws });
            var search = new SearchWasteWaterSystemBasin();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.BasinName, "BasinName");
                helper.AreEqual(entity1.BasinName, "BasinName", 1);
                helper.AreEqual(entity0.WasteWaterSystem, "Wastewater System");
                helper.AreEqual(entity1.WasteWaterSystem, "Wastewater System", 1);
            }
        }

        #endregion

        #region Edit/Update

         [TestMethod]
         public override void TestUpdateSavesChangesWhenModelStateIsValid()
         {
             var eq = GetFactory<WasteWaterSystemBasinFactory>().Create();
             var expected = (decimal)22.22;

             var wws = GetFactory<WasteWaterSystemFactory>().Create(); 
             var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditWasteWaterSystemBasin, WasteWaterSystemBasin>(eq, new {
                 FirmCapacity = expected
             }));

             Assert.AreEqual(expected, Session.Get<WasteWaterSystemBasin>(eq.Id).FirmCapacity);
         }
       
        #endregion

        #region Cascading Endpoints

        [TestMethod]
        public void TestByWasteWaterSystem()
        {
            var validWasteWaterSystem = GetEntityFactory<WasteWaterSystem>().Create();
            var invalidWasteWaterSystem = GetEntityFactory<WasteWaterSystem>().Create();
            var validWasteWaterSystemBasin = GetEntityFactory<WasteWaterSystemBasin>().Create(new { WasteWaterSystem = validWasteWaterSystem });
            var invalidWasteWaterSystemBasin = GetEntityFactory<WasteWaterSystemBasin>().Create(new { WasteWaterSystem = invalidWasteWaterSystem });

            var actionResult = (CascadingActionResult)_target.ByWasteWaterSystem(validWasteWaterSystem.Id);
            var wasteWaterSystemBasinDisplayItems = ((IEnumerable<WasteWaterSystemBasinDisplayItem>)actionResult.Data).ToList();

            Assert.AreEqual(validWasteWaterSystemBasin.Id, wasteWaterSystemBasinDisplayItems.FirstOrDefault()?.Id);
            Assert.IsNull(wasteWaterSystemBasinDisplayItems.FirstOrDefault(x => x.Id == invalidWasteWaterSystemBasin.Id));
        }

        #endregion

        #endregion
    }
}
