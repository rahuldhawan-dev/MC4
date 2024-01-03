using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Environmental.Controllers;
using MapCallMVC.Areas.Environmental.Models.ViewModels.PublicWaterSupplyFirmCapacities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Environmental.Controllers
{
    [TestClass]
    public class PublicWaterSupplyFirmCapacityControllerTest : MapCallMvcControllerTestBase<PublicWaterSupplyFirmCapacityController, PublicWaterSupplyFirmCapacity>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole($"~/Environmental/PublicWaterSupplyFirmCapacity/Search/", RoleModules.EngineeringPWSIDCapacity);
                a.RequiresRole($"~/Environmental/PublicWaterSupplyFirmCapacity/Show/", RoleModules.EngineeringPWSIDCapacity);
                a.RequiresRole($"~/Environmental/PublicWaterSupplyFirmCapacity/Index/", RoleModules.EngineeringPWSIDCapacity);
                a.RequiresRole($"~/Environmental/PublicWaterSupplyFirmCapacity/New/", RoleModules.EngineeringPWSIDCapacity, RoleActions.Add);
                a.RequiresRole($"~/Environmental/PublicWaterSupplyFirmCapacity/Create/", RoleModules.EngineeringPWSIDCapacity, RoleActions.Add);
                a.RequiresRole($"~/Environmental/PublicWaterSupplyFirmCapacity/Edit/", RoleModules.EngineeringPWSIDCapacity, RoleActions.Edit);
                a.RequiresRole($"~/Environmental/PublicWaterSupplyFirmCapacity/Update/", RoleModules.EngineeringPWSIDCapacity, RoleActions.Edit);
            });
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var firmCapacity = GetEntityFactory<PublicWaterSupplyFirmCapacity>().Create();
            const decimal expectedFirmCapacityMultiplier = 0.5M;

            _target.Update(_viewModelFactory.BuildWithOverrides<PublicWaterSupplyFirmCapacityViewModel, PublicWaterSupplyFirmCapacity>(firmCapacity, new {
                FirmCapacityMultiplier = expectedFirmCapacityMultiplier
            }));

            Assert.AreEqual(expectedFirmCapacityMultiplier, Session.Get<PublicWaterSupplyFirmCapacity>(firmCapacity.Id).FirmCapacityMultiplier);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var state = GetEntityFactory<State>().Create();
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create(new {State = state});
            var publicWaterSupplyFirmCapacity = GetEntityFactory<PublicWaterSupplyFirmCapacity>().Create(new {PublicWaterSupply = publicWaterSupply});
            
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var search = new SearchPublicWaterSupplyFirmCapacityViewModel {
                State = new[] {state.Id},
                PublicWaterSupply = new[] {publicWaterSupply.Id}
            };

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(publicWaterSupplyFirmCapacity.PublicWaterSupply.State, "State");
                helper.AreEqual(publicWaterSupplyFirmCapacity.PublicWaterSupply, "PublicWaterSupply");
                helper.AreEqual(publicWaterSupplyFirmCapacity.CurrentSystemPeakDailyDemandMGD, "CurrentSystemPeakDailyDemandMGD");
                helper.AreEqual(publicWaterSupplyFirmCapacity.CurrentSystemPeakDailyDemandYearMonth, "CurrentSystemPeakDailyDemandYearMonth");
                helper.AreEqual(publicWaterSupplyFirmCapacity.TotalSystemSourceCapacityMGD, "TotalSystemSourceCapacityMGD");
                helper.AreEqual(publicWaterSupplyFirmCapacity.TotalCapacityFacilitySumMGD, "TotalCapacityFacilitySumMGD");
                helper.AreEqual(publicWaterSupplyFirmCapacity.FirmCapacityMultiplier, "FirmCapacityMultiplier");
                helper.AreEqual(publicWaterSupplyFirmCapacity.FirmSystemSourceCapacityMGD, "FirmSystemSourceCapacityMGD");
                helper.AreEqual(publicWaterSupplyFirmCapacity.UpdatedAt, "DateUpdated");
            }
        }

        #endregion
    }
}
