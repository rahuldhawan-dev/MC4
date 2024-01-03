using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyPressureZones;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class PublicWaterSupplyPressureZoneControllerTest : MapCallMvcControllerTestBase<PublicWaterSupplyPressureZoneController, PublicWaterSupplyPressureZone>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const string urlPathPart = "~/Environmental/PublicWaterSupplyPressureZone";

            Authorization.Assert(a => {
                a.RequiresRole($"{urlPathPart}/Search/", RoleModules.EnvironmentalGeneral);
                a.RequiresRole($"{urlPathPart}/Show/", RoleModules.EnvironmentalGeneral);
                a.RequiresRole($"{urlPathPart}/Index/", RoleModules.EnvironmentalGeneral);
                a.RequiresSiteAdminUser($"{urlPathPart}/New/");
                a.RequiresSiteAdminUser($"{urlPathPart}/Create/");
                a.RequiresSiteAdminUser($"{urlPathPart}/Edit/");
                a.RequiresSiteAdminUser($"{urlPathPart}/Update/");
                a.RequiresSiteAdminUser($"{urlPathPart}/Destroy/");
                a.RequiresLoggedInUserOnly($"{urlPathPart}/ByPublicWaterSupply/");
            });
        }

        #endregion

        #region Creation Notification

        [TestMethod]
        public void TestCreateSendsNotification()
        {
            var operatingCenter1 = GetEntityFactory<OperatingCenter>().Create();

            ISet<OperatingCenterPublicWaterSupply> operatingCenterPublicWaterSupply = new HashSet<OperatingCenterPublicWaterSupply> {
                    GetEntityFactory<OperatingCenterPublicWaterSupply>().Create(new {
                        OperatingCenter = operatingCenter1,
                        Abbreviation = "AZ"
                    })
                };

            var publicWaterSupply1 = GetEntityFactory<PublicWaterSupply>().Create(new {
                Id = 123,
                Identifier = "52033541",
                OperatingArea = "OA",
                System = "S",
                OperatingCenterPublicWaterSupplies = operatingCenterPublicWaterSupply
            });
            
            var pressureZone = GetEntityFactory<PublicWaterSupplyPressureZone>().Create();
            
            var model = _viewModelFactory.BuildWithOverrides<PublicWaterSupplyPressureZoneViewModel, PublicWaterSupplyPressureZone>(pressureZone, new
            {
                HydraulicModelName = "the name",
                PublicWaterSupply = publicWaterSupply1.Id
            });

            _target.Create(model);

            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity = GetEntityFactory<PublicWaterSupplyPressureZone>().Create();
            var search = new SearchPublicWaterSupplyPressureZoneViewModel();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity.Id, "Id");
                helper.AreEqual(entity.PublicWaterSupply, "PublicWaterSupply");
                helper.AreEqual(entity.PublicWaterSupplyFirmCapacity, "PublicWaterSupplyFirmCapacity");
                helper.AreEqual(entity.HydraulicModelName, "HydraulicModelName");
                helper.AreEqual(entity.HydraulicGradientMin, "Hydraulic Gradient (HGL) Min");
                helper.AreEqual(entity.HydraulicGradientMax, "Hydraulic Gradient (HGL) Max");
                helper.AreEqual(entity.PressureMin, "PressureMin");
                helper.AreEqual(entity.PressureMax, "PressureMax");
                helper.AreEqual(entity.CommonName, "CommonName");
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            const string expectedName = "Pressure Zone - 01";

            var pressureZone = GetEntityFactory<PublicWaterSupplyPressureZone>().Create();

            _target.Update(_viewModelFactory.BuildWithOverrides<PublicWaterSupplyPressureZoneViewModel, PublicWaterSupplyPressureZone>(pressureZone, new {
                HydraulicModelName = expectedName
            }));

            var updatedPressureZone = Session.Get<PublicWaterSupplyPressureZone>(pressureZone.Id);

            Assert.AreEqual(expectedName, updatedPressureZone.HydraulicModelName);
        }

        #endregion

        #region Cascading Endpoints

        [TestMethod]
        public void TestByPublicWaterSupply()
        {
            var validPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var invalidPublicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var validPublicWaterSupplyPressureZone = GetEntityFactory<PublicWaterSupplyPressureZone>().Create(new { PublicWaterSupply = validPublicWaterSupply });
            var invalidPublicWaterSupplyPressureZone = GetEntityFactory<PublicWaterSupplyPressureZone>().Create(new { PublicWaterSupply = invalidPublicWaterSupply });

            var actionResult = (CascadingActionResult)_target.ByPublicWaterSupply(validPublicWaterSupply.Id);
            var publicWaterSupplyPressureZoneDisplayItems = ((IEnumerable<PublicWaterSupplyPressureZoneDisplayItem>)actionResult.Data).ToList();

            Assert.AreEqual(validPublicWaterSupplyPressureZone.Id, publicWaterSupplyPressureZoneDisplayItems.FirstOrDefault()?.Id);
            Assert.IsNull(publicWaterSupplyPressureZoneDisplayItems.FirstOrDefault(x => x.Id == invalidPublicWaterSupplyPressureZone.Id));
        }

        #endregion
    }
}
