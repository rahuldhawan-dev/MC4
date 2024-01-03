using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallApi.Controllers;
using MapCallApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using Newtonsoft.Json;

namespace MapCallApi.Tests.Controllers
{
    [TestClass]
    public class WasteWaterSystemControllerTest : MapCallApiControllerTestBase<WasteWaterSystemController, WasteWaterSystem,IRepository<WasteWaterSystem>>
    {
        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = WasteWaterSystemController.ROLE;

            Authorization.Assert(a => {
                SetupHttpAuth(a);
                a.RequiresRole("~/WasteWaterSystem/Index", module);
            });
        }

        [TestMethod]
        // Adding this override as this test is running but is not applicable for this controller
        public override void TestIndexReturnsResults() { }

        [TestMethod]
        public void Test_GetWasteWaterSystems_ReturnsProperJson()
        {
            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new { State = state });
            var type = GetEntityFactory<WasteWaterSystemType>().Create();
            var subType = GetEntityFactory<WasteWaterSystemSubType>().Create();
            var status = GetEntityFactory<WasteWaterSystemStatus>().Create();
            var ownership = GetEntityFactory<WasteWaterSystemOwnership>().Create();
            var planningPlant = GetEntityFactory<PlanningPlant>().Create();
            
            var wasteWaterSystems = GetEntityFactory<WasteWaterSystem>().CreateList(5, new {Status = status, Ownership = ownership, OperatingCenter = operatingCenter, Type = type, SubType = subType});
            
            var planningPlantWasteWaterSystem = GetEntityFactory<PlanningPlantWasteWaterSystem>().CreateSet(1, new {
                PlanningPlant = planningPlant,
                WasteWaterSystem = wasteWaterSystems.First()
            });
            
            wasteWaterSystems.First().PlanningPlantWasteWaterSystems = planningPlantWasteWaterSystem;
            
            var search = new SearchWasteWaterSystem { State = state.Id };

            var results = (JsonResult)_target.Index(search);
            var helper = new JsonResultTester(results);

            for (int i = 0; i < wasteWaterSystems.Count; i++)
            {
                var wasteWaterSystem = wasteWaterSystems[i];

                helper.AreEqual(wasteWaterSystem.Id, nameof(wasteWaterSystem.Id), i);
                helper.AreEqual(wasteWaterSystem.OperatingCenter.Description, "OperatingCenter", i);
                helper.AreEqual(wasteWaterSystem.WasteWaterSystemId, "WWSID", i);
                helper.AreEqual(wasteWaterSystem.WasteWaterSystemName, nameof(wasteWaterSystem.WasteWaterSystemName), i);
                helper.AreEqual(wasteWaterSystem.PermitNumber, nameof(wasteWaterSystem.PermitNumber), i);
                helper.AreEqual(wasteWaterSystem.Type.Description, "Type", i);
                helper.AreEqual(wasteWaterSystem.SubType.Description, "SubType", i);
                helper.AreEqual(wasteWaterSystem.GravityLength, nameof(wasteWaterSystem.GravityLength), i);
                helper.AreEqual(wasteWaterSystem.ForceLength, nameof(wasteWaterSystem.ForceLength), i);
                helper.AreEqual(wasteWaterSystem.NumberOfLiftStations, nameof(wasteWaterSystem.NumberOfLiftStations), i);
                helper.AreEqual(wasteWaterSystem.State.Abbreviation, "State", i);
                helper.AreEqual(wasteWaterSystem.Description, nameof(wasteWaterSystem.Description), i);
            }
            Assert.AreEqual(wasteWaterSystems.First().PlanningPlantWasteWaterSystems?.First().PlanningPlant.Description, planningPlantWasteWaterSystem.First().PlanningPlant.Description);
        }
    }
}
