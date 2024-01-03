using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FleetManagement.Controllers;
using MapCallMVC.Areas.FleetManagement.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FleetManagement.Controllers
{
    [TestClass]
    public class VehicleControllerTest : MapCallMvcControllerTestBase<VehicleController, Vehicle>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IVehicleRepository>().Use<VehicleRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FleetManagementGeneral;
            Authorization.Assert(a => {
                a.RequiresRole("~/Vehicle/Search", role, RoleActions.Read);
                a.RequiresRole("~/Vehicle/Show", role, RoleActions.Read);
                a.RequiresRole("~/Vehicle/Index", role, RoleActions.Read);
                a.RequiresRole("~/Vehicle/New", role, RoleActions.Add);
                a.RequiresRole("~/Vehicle/Create", role, RoleActions.Add);
                a.RequiresRole("~/Vehicle/Edit", role, RoleActions.Edit);
                a.RequiresRole("~/Vehicle/Update", role, RoleActions.Edit);
                a.RequiresRole("~/Vehicle/AddVehicleAudit", role, RoleActions.Edit);
                a.RequiresLoggedInUserOnly("~/Vehicle/ByOperatingCenterId/");
            });
        }

        #region Lookup Data

        [TestMethod]
        public void TestSetLookUpDataForOperatingCenterSetsCorrectlyOnNew()
        {
            var opc1 = GetEntityFactory<OperatingCenter>().CreateList(5);
            var opc2 = GetEntityFactory<OperatingCenter>().Create(new { IsActive = true });

            _target.SetLookupData(ControllerAction.New);

            var opcs = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            Assert.AreNotEqual(opc1.Count, opcs.Count());
            Assert.AreEqual(1, opcs.Count());
            Assert.IsTrue(opc2.Id.ToString() == opcs.First().Value);
        }

        [TestMethod]
        public void TestSetLookUpDataForManagerAndFleetContactPersonSetsCorrectly()
        {
            var employees = GetEntityFactory<Employee>().CreateList(5);
            var employee = GetFactory<ActiveEmployeeFactory>().Create();

            _target.SetLookupData(ControllerAction.New);

            var managers = (IEnumerable<Employee>)_target.ViewData["Manager"];
            var fleetContactPersons = (IEnumerable<Employee>)_target.ViewData["FleetContactPerson"];
            
            Assert.AreEqual(1, managers.Count());
            Assert.AreEqual(1, fleetContactPersons.Count());
            Assert.IsTrue(employee.Id == managers.First().Id);
            Assert.IsTrue(employee.Id == fleetContactPersons.First().Id);

            _target.SetLookupData(ControllerAction.Edit);

            managers = (IEnumerable<Employee>)_target.ViewData["Manager"];
            fleetContactPersons = (IEnumerable<Employee>)_target.ViewData["FleetContactPerson"];

            Assert.AreEqual(6, managers.Count());
            Assert.AreEqual(6, fleetContactPersons.Count());
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetFactory<VehicleFactory>().Create();
            var expected = "BAH-1";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditVehicle, Vehicle>(eq, new {
                ARIVehicleNumber = expected
            }));

            Assert.AreEqual(expected, Session.Get<Vehicle>(eq.Id).ARIVehicleNumber);
        }

        #endregion

        #region ByOperatingCenterId

        [TestMethod]
        public void TestByOperatingCenterIdReturnsCascadingResultForMatchingOperatingCenters()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var good = GetFactory<VehicleFactory>().Create(new { OperatingCenter = opc });
            var bad = GetFactory<VehicleFactory>().Create();

            var result = (CascadingActionResult)_target.ByOperatingCenterId(opc.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count()-1);
            Assert.AreEqual(good.Id.ToString(), actual[1].Value);
        }

        #endregion

        #endregion
    }
}