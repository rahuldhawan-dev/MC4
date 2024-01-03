using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HumanResources.Models.ViewModels;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class BusinessUnitControllerTest : MapCallMvcControllerTestBase<BusinessUnitController, BusinessUnit>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                return GetEntityFactory<BusinessUnit>().Create(new {
                    Department = typeof(DepartmentFactory)
                });
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = BusinessUnitController.ROLE;

            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/BusinessUnit/FindByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/BusinessUnit/FindByOperatingCenterIdForWasteWaterOrCFS/");
                a.RequiresLoggedInUserOnly("~/BusinessUnit/FindByOperatingCenterIdForTDWorkOrders/");
                a.RequiresRole("~/BusinessUnit/Show/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/BusinessUnit/Search/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/BusinessUnit/Index/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/BusinessUnit/Create/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/BusinessUnit/New/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/BusinessUnit/Edit/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/BusinessUnit/Update/", role, RoleActions.UserAdministrator);
            });
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<BusinessUnit>().Create();
            var expected = "Some notes of sorts";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditBusinessUnit, BusinessUnit>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<BusinessUnit>(eq.Id).Description);
        }

        [TestMethod]
        public void TestFindByOperatingCenterIdForWasteWaterAndCFSReturnsItemsByOperatingCenterForWasteWaterAndCFS()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var department = GetEntityFactory<Department>().CreateList(20);
            var businessUnitWasteWater = GetEntityFactory<BusinessUnit>().Create(new{Id = 1, OperatingCenter = operatingCenter, Department = department.FirstOrDefault(x => x.Id == Department.Indices.WASTE_WATER), BU = "foo"});
            var businessUnitCFS = GetEntityFactory<BusinessUnit>().Create(new { Id = 2, OperatingCenter = operatingCenter, Department = department.FirstOrDefault(x => x.Id == Department.Indices.CFS), BU = "foo" });
            var businessUnitProduction = GetEntityFactory<BusinessUnit>().Create(new { Id = 3, OperatingCenter = operatingCenter, Department = department.FirstOrDefault(x => x.Id == Department.Indices.PRODUCTION), BU = "foo" });

            var result = (CascadingActionResult)_target.FindByOperatingCenterIdForWasteWaterOrCFS(operatingCenter.Id);

            var items = ((IEnumerable<dynamic>)result.Data).Count();

            Assert.AreEqual(2, items);
        }

        [TestMethod]
        public void TestFindByOperatingCenterIdForTDWorkOrders()
        {
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var department = GetEntityFactory<Department>().CreateList(20);
            var businessUnitTD = GetEntityFactory<BusinessUnit>().Create(new{Id = 1, OperatingCenter = operatingCenter, Department = department.FirstOrDefault(x => x.Id == Department.Indices.T_AND_D), BU = "foo"});
            var businessUnitCFS = GetEntityFactory<BusinessUnit>().Create(new { Id = 2, OperatingCenter = operatingCenter, Department = department.FirstOrDefault(x => x.Id == Department.Indices.CFS), BU = "foo" });

            var result = (CascadingActionResult)_target.FindByOperatingCenterIdForWasteWaterOrCFS(operatingCenter.Id);

            var items = ((IEnumerable<dynamic>)result.Data).Count();

            Assert.AreEqual(1, items);
        }

        #endregion
    }
}
