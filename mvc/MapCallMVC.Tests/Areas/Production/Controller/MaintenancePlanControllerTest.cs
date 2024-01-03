using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class MaintenancePlanControllerTest : MapCallMvcControllerTestBase<MaintenancePlanController, MaintenancePlan, IRepository<MaintenancePlan>>
    {
        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateMaintenancePlan)vm;
                model.DeactivationReason = "I dunno";
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditMaintenancePlan)vm;
                model.DeactivationReason = "I dunno";
            };
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.ProductionPlannedWork;
                const string path = "~/Production/MaintenancePlan/";
                a.RequiresRole(path + "Search", role);
                a.RequiresRole(path + "Show", role);
                a.RequiresRole(path + "Index", role);
                a.RequiresRole(path + "Forecast", role);
                a.RequiresRole(path + "Create", role, RoleActions.Add);
                a.RequiresRole(path + "New", role, RoleActions.Add);
                a.RequiresRole(path + "Copy", role, RoleActions.Add);
                a.RequiresRole(path + "Edit", role, RoleActions.Edit);
                a.RequiresRole(path + "Update", role, RoleActions.Edit);
                a.RequiresRole(path + "AddEquipmentMaintenancePlan", role, RoleActions.Edit);
                a.RequiresRole(path + "RemoveEquipmentMaintenancePlan", role, RoleActions.Edit);
                a.RequiresRole(path + "RemoveAllEquipmentMaintenancePlan", role, RoleActions.Edit);
                a.RequiresRole(path + "Destroy", role, RoleActions.Delete);
                a.RequiresRole(path + "AddScheduledAssignments", role, RoleActions.Edit);
                a.RequiresRole(path + "RemoveScheduledAssignments", role, RoleActions.Edit);
                a.RequiresLoggedInUserOnly(path + "ByFacilityIdAndEquipmentTypeId");
            });
        }

        #region ByFacilityIdAndEquipmentTypeId

        [TestMethod]
        public void TestByFacilityIdAndEquipmentTypeIdCascadingResultForMatchingFacilityAndEquipmentPurpose()
        {
            var goodPlan = GetEntityFactory<MaintenancePlan>().Create();
            var badPlan = GetEntityFactory<MaintenancePlan>().Create();

            var goodFacility = GetFactory<FacilityFactory>().Create();
            var badFacility = GetFactory<FacilityFactory>().Create();

            var goodEquipmentTypes = GetEntityFactory<EquipmentType>().CreateList();
            var badEquipmentTypes = GetEntityFactory<EquipmentType>().CreateList();

            goodPlan.Facility = goodFacility;
            goodPlan.EquipmentTypes = goodEquipmentTypes;
            badPlan.Facility = badFacility;
            badPlan.EquipmentTypes = badEquipmentTypes;

            Session.Flush();

            var result = (CascadingActionResult)_target.ByFacilityIdAndEquipmentTypeId(goodFacility.Id, goodEquipmentTypes[0].Id);

            var actual = ((IEnumerable<MaintenancePlanDisplayItem>)result.Data);

            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(goodPlan.Id, actual.First().Id);
        }

        #endregion

        #region Copy

        [TestMethod]
        public void TestCopyReturnsErrorWhenPlanNotFound()
        {
            var result = (HttpNotFoundResult)_target.Copy(0);
            Assert.IsNotNull(result);
            MvcAssert.IsNotFound(result);
            Assert.AreEqual("Could not find a maintenance plan with id 0", result.StatusDescription);
        }

        [TestMethod]
        public void TestCopyCreatesCopyAndRedirectsToEdit()
        {
            var maintenancePlan = GetFactory<MaintenancePlanFactory>().Create();

            var result = _target.Copy(maintenancePlan.Id);

            MvcAssert.RedirectsToRoute(result, "Edit", new { id = maintenancePlan.Id + 1 });
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<MaintenancePlan>().Create();
            const bool expected = false;

            _target.Update(_viewModelFactory.BuildWithOverrides<EditMaintenancePlan, MaintenancePlan>(eq, new {
                IsActive = expected
            }));

            Assert.AreEqual(expected, Session.Get<MaintenancePlan>(eq.Id).IsActive);
        }

        #region Children

        [TestMethod]
        public void TestAddScheduledAssignmentsAddsScheduledAssignmentsToMaintenancePlan()
        {
            var plan = GetEntityFactory<MaintenancePlan>().Create();
            var employee = GetEntityFactory<Employee>().Create();
            
            MyAssert.CausesIncrease(
                () => _target.AddScheduledAssignments(
                    _viewModelFactory.BuildWithOverrides<CreateScheduledAssignments, MaintenancePlan>(plan, new {
                        OperatingCenter = plan.OperatingCenter.Id,
                        AssignedTo = new[] { employee.Id },
                        AssignedFor = _dateTimeProvider.GetCurrentDate(),
                        ScheduledDates = new[] { _dateTimeProvider.GetCurrentDate().AddDays(1) }
                    })),
                () => _container.GetInstance<IRepository<ScheduledAssignment>>().GetAll().Count());
        }

        [TestMethod]
        public void TestRemoveScheduledAssignmentsRemovesScheduledAssignmentsFromMaintenancePlan()
        {
            var plan = GetEntityFactory<MaintenancePlan>().Create();
            var assignment = GetEntityFactory<ScheduledAssignment>().Create(new { MaintenancePlan = plan });

            Session.Evict(plan);

            MyAssert.CausesDecrease(
                () => _target.RemoveScheduledAssignments(
                    _viewModelFactory.BuildWithOverrides<RemoveScheduledAssignments, MaintenancePlan>(plan, new {
                        SelectedAssignments = new[] { assignment.Id }
                    })),
                () => _container.GetInstance<IRepository<ScheduledAssignment>>().GetAll().Count());
        }

        [TestMethod]
        public void TestAddEquipmentMaintenancePlanAddsEquipmentToMaintenancePlan()
        {
            var plan = GetEntityFactory<MaintenancePlan>().Create();
            var eq = GetEntityFactory<Equipment>().Create();

            MyAssert.CausesIncrease(
                () => _target.AddEquipmentMaintenancePlan(
                    _viewModelFactory.BuildWithOverrides<AddEquipmentMaintenancePlan, MaintenancePlan>(plan, new {
                        Equipment = new[] { eq.Id }
                    })),
                () => _container.GetInstance<RepositoryBase<EquipmentMaintenancePlan>>().GetAll().Count());
        }

        [TestMethod]
        public void TestRemoveEquipmentMaintenancePlanRemovesEquipmentFromMaintenancePlan()
        {
            var plan = GetEntityFactory<MaintenancePlan>().Create();
            var eq = GetEntityFactory<Equipment>().Create();
            var eqmp1 = GetEntityFactory<EquipmentMaintenancePlan>()
               .Create(new { Equipment = eq, MaintenancePlan = plan });

            Session.Evict(plan);

            MyAssert.CausesDecrease(
                () => _target.RemoveEquipmentMaintenancePlan(
                    _viewModelFactory.BuildWithOverrides<RemoveEquipmentMaintenancePlan, MaintenancePlan>(plan, new {
                        Equipment = new[] { eqmp1.Id }
                    })),
                () => _container.GetInstance<RepositoryBase<EquipmentMaintenancePlan>>().GetAll().Count());
        }

        [TestMethod]
        public void TestRemoveAllEquipmentMaintenancePlaneRemovesEquipmentFromMaintenancePlan()
        {
            var plan = GetEntityFactory<MaintenancePlan>().Create();
            var eq = GetEntityFactory<Equipment>().Create();
            var eqmp1 = GetEntityFactory<EquipmentMaintenancePlan>().Create(new { Equipment = eq, MaintenancePlan = plan });
            var eqmp2 = GetEntityFactory<EquipmentMaintenancePlan>().Create(new { Equipment = eq, MaintenancePlan = plan });

            Session.Evict(plan);

            MyAssert.CausesDecrease(
                () => _target.RemoveAllEquipmentMaintenancePlan(
                    _viewModelFactory.Build<RemoveAllEquipmentMaintenancePlan, MaintenancePlan>(plan)),
                () => _container.GetInstance<RepositoryBase<EquipmentMaintenancePlan>>().GetAll().Count(), 2);
        }

        #endregion

        #endregion
    }
}
