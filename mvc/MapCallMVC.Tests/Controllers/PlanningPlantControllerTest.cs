using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using MMSINC;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class PlanningPlantControllerTest : MapCallMvcControllerTestBase<PlanningPlantController, PlanningPlant>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesDataLookups;
                a.RequiresRole("~/PlanningPlant/Show/", module, RoleActions.Read);
                a.RequiresRole("~/PlanningPlant/Index/", module, RoleActions.Read);
                a.RequiresRole("~/PlanningPlant/Search/", module, RoleActions.Read);
                a.RequiresRole("~/PlanningPlant/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/PlanningPlant/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/PlanningPlant/New/", module, RoleActions.Add);
                a.RequiresRole("~/PlanningPlant/Create/", module, RoleActions.Add);
                a.RequiresRole("~/PlanningPlant/Destroy/", module, RoleActions.Delete);
                a.RequiresLoggedInUserOnly("~/PlanningPlant/ByOperatingCenter");
                a.RequiresLoggedInUserOnly("~/PlanningPlant/ByOperatingCenters");
                a.RequiresLoggedInUserOnly("~/PlanningPlant/ByState");
                a.RequiresLoggedInUserOnly("~/PlanningPlant/ByOperatingCenterCodeAndNotId");
            });
        }

        [TestMethod]
        public void TestByOperatingCenterReturnsByOperatingCenter()
        {
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().CreateList(2);
            var entityValid = GetEntityFactory<PlanningPlant>().Create(new {OperatingCenter = opCntr.First()});
            var entityInvalid = GetEntityFactory<PlanningPlant>().Create(new { OperatingCenter = opCntr.Last() });

            var result = _target.ByOperatingCenter(opCntr.First().Id) as CascadingActionResult;
            var actual = result.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1); // because --select here--
            Assert.AreEqual(entityValid.Id.ToString(), actual.Last().Value);
            Assert.AreNotEqual(entityInvalid.Id.ToString(), actual.Last().Value); // redundant
        }

        [TestMethod]
        public void TestByStateReturnsByState()
        {
            var state = GetFactory<StateFactory>().Create();
            var opCntr = GetFactory<OperatingCenterFactory>().Create(new {State = state});
            var planningPlants = GetFactory<PlanningPlantFactory>().CreateList(2, new {OperatingCenter = opCntr});

            var result = (CascadingActionResult)_target.ByState(state.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(planningPlants.Count(), actual.Count() - 1); // -1 accounts for the select here
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<PlanningPlant>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<PlanningPlantViewModel, PlanningPlant>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<PlanningPlant>(eq.Id).Description);
        }

        #endregion

    }
}
