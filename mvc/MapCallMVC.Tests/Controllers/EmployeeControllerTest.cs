using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EmployeeControllerTest
        : MapCallMvcControllerTestBase<EmployeeController, Employee, EmployeeRepository>
    {
        #region Private Members

        private RoleAction _readAction;
        private RoleAction _addAction;
        private RoleAction _editAction;
        private RoleAction _deleteAction;
        private RoleAction _userAdminAction;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            SetupSupportData();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexRedirectsToShowForSingleResult = true;
        }

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        private void SetupSupportData()
        {
            GetFactory<InProgramCommercialDriversLicenseProgramStatusFactory>().Create();
            GetFactory<PursingCommercialDriversLicenseProgramStatusFactory>().Create();
            GetFactory<NotInProgramCommercialDriversLicenseProgramStatusFactory>().Create();

            _readAction = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            _addAction = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Add });
            _editAction = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Edit });
            _deleteAction = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Delete });
            _userAdminAction = GetFactory<ActionFactory>().Create(new {
                Id = RoleActions.UserAdministrator
            });
        }

        #endregion

        #region Private Methods

        private void AssertActiveEmployeesByOperatingCenterIdAndSomeRoleWorksProperly(
            Func<int, ActionResult> testFn,
            RoleApplications roleApplication,
            RoleModules roleModule)
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<ActiveEmployeeFactory>().Create();
            var user = GetEntityFactory<User>().Create(new { Employee = employee });

            // Test employee with user without any matching roles does not end up in list 
            var result = (CascadingActionResult)testFn(opc.Id);
            Assert.IsFalse(result.Data.Any());

            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>()
                   .Create(new { Id = roleApplication }),
                Module =
                    GetFactory<ModuleFactory>().Create(new { Id = roleModule }),
                Action = _readAction,
                User = user
            });

            void doTest(RoleAction action, OperatingCenter opcForRole, bool mustBeInResult)
            {
                role.OperatingCenter = opcForRole;
                role.Action = action;
                Session.Save(role);
                Session.Flush();

                var result1 = (CascadingActionResult)testFn(opc.Id);
                var testableResult = result1.Data.Any();
                if (mustBeInResult)
                {
                    Assert.IsTrue(testableResult,
                        $"No result was found for action '{action.Description}' and operating " +
                        $"center '{opcForRole}'.");
                }
                else
                {
                    Assert.IsFalse(testableResult,
                        $"An unexpected result was found for action '{action.Description}' and operating " +
                        $"center '{opcForRole}'.");
                }
            }

            foreach (var action in new[] {
                         _readAction,
                         _addAction,
                         _editAction,
                         _deleteAction,
                         _userAdminAction
                     })
            {
                // Do wildcard match
                doTest(_addAction, null, true);
                // Do opc match
                doTest(action, opc, true);
            }

            // inactive user with role can not access
            employee.Status = GetFactory<InactiveEmployeeStatusFactory>().Create();
            Session.Save(employee);
            doTest(_userAdminAction, null, false);
        }

        #endregion

        #region ActiveEmployeesByOperatingCenterIdForJobSiteCheckLists

        [TestMethod]
        public void TestActiveEmployeesByOperatingCenterIdForJobSiteCheckListsReturnsNJ6EmployeesforSOVOperatingCenter()
        {
            var nj6 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ6" });
            var sov = GetFactory<OperatingCenterFactory>().Create(new {
                OperatingCenterCode = "SOV",
                OperatedByOperatingCenter = nj6
            });
            var other = GetFactory<UniqueOperatingCenterFactory>().Create();
            var good = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = nj6
            });
            var bad = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = other
            });

            var result = (CascadingActionResult)_target
               .ActiveEmployeesByOperatingCenterIdForJobSiteCheckLists(sov.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1); // -1 for the empty select item.
            Assert.AreEqual(good.Id.ToString(), actual[1].Value);
        }

        #endregion

        #region ActiveEmployeesByOperatingCenterId

        [TestMethod]
        public void
            TestActiveEmployeesByOperatingCenterIdReturnsCascadingResultForMatchingOperatingCentersAndActiveEmployees()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var good = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });
            var bad = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<InactiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });

            var result = (CascadingActionResult)_target.ActiveEmployeesByOperatingCenterId(opc.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(good.Id.ToString(), actual[1].Value);
        }

        [TestMethod]
        public void
            TestActiveEmployeesByOperatingCenterIdWithStatusReturnsCascadingResultForMatchingOperatingCentersAndActiveEmployees()
        {
            //Assemble
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var otherOpc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var validEmployee1 = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });
            var invalidEmployee1 = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<InactiveEmployeeStatusFactory>().Create(),
                OperatingCenter = otherOpc
            });

            var result = _target
               .ActiveEmployeesByOperatingCenterIdWithStatus(opc.Id) as CascadingActionResult;
            var resultData = (IEnumerable<EmployeeDisplayItem>)result.Data;
            var actual = result.GetSelectListItems().ToArray();

            //Assert
            Assert.AreEqual(1, actual.Count() - 1);
            Assert.IsFalse(actual.Select(x => x.Value).Contains(invalidEmployee1.Id.ToString()));
            Assert.IsTrue(actual.Select(x => x.Value).Contains(validEmployee1.Id.ToString()));
            Assert.AreEqual(validEmployee1.Id.ToString(), actual[1].Value);
            Assert.IsFalse(actual.Select(x => x.Value).Contains(invalidEmployee1.Id.ToString()));
        }

        #endregion

        #region EmployeesByOperatingCenterIdAndStatusId

        [TestMethod]
        public void TestEmployeesByOperatingCenterIdAndStatusIdReturnsCascadingResultForMatchingOperatingCentersAndEmployees()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var activeStatus = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var inactiveStatus = GetFactory<InactiveEmployeeStatusFactory>().Create();
            
            var activeEmployee = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });

            var inactiveEmployee = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<InactiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });

            var result = (CascadingActionResult)_target.EmployeesByOperatingCenterIdAndStatusId(opc.Id, activeStatus.Id);
            var actual = result.GetSelectListItems().ToArray();
            var result2 = (CascadingActionResult)_target.EmployeesByOperatingCenterIdAndStatusId(opc.Id, inactiveStatus.Id);
            var actual2 = result2.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(activeEmployee.Id.ToString(), actual[1].Value);
            Assert.AreEqual(1, actual2.Count() - 1);
            Assert.AreEqual(inactiveEmployee.Id.ToString(), actual2[1].Value);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.HumanResourcesEmployee;
                a.RequiresRole("~/Employee/Search/", module);
                a.RequiresRole("~/Employee/Index/", module);
                a.RequiresRole("~/Employee/Show/", module);
                a.RequiresRole("~/Employee/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Employee/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Employee/New/", module, RoleActions.Add);
                a.RequiresRole("~/Employee/Create/", module, RoleActions.Add);
                a.RequiresRole("~/Employee/Index/", module);

                a.RequiresRole("~/Employee/AddProductionSkillSets/", module, RoleActions.Edit);
                a.RequiresRole("~/Employee/RemoveProductionSkillSets/", module, RoleActions.Edit);

                a.RequiresLoggedInUserOnly("~/Employee/GetByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/Employee/ActiveEmployeesByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/Employee/ActiveEmployeesByOperatingCenterIds/");
                a.RequiresLoggedInUserOnly("~/Employee/ActiveEmployeesByOperatingCenterIdWithStatus/");
                a.RequiresLoggedInUserOnly("~/Employee/ActiveEmployeesByStateId/");
                a.RequiresLoggedInUserOnly("~/Employee/ActiveEmployeesByStateIdAndPartial/");
                a.RequiresLoggedInUserOnly("~/Employee/ActiveEmployeesWithNoCDLByOperatingCenterId/");
                a.RequiresLoggedInUserOnly(
                    "~/Employee/ActiveEmployeesByOperatingCenterIdForJobSiteCheckLists/");
                a.RequiresLoggedInUserOnly("~/Employee/ActiveLockoutFormEmployeesByOperatingCenterId/");
                a.RequiresLoggedInUserOnly(
                    "~/Employee/ActiveProductionWorkManagementEmployeesByOperatingCenterId/");
                a.RequiresLoggedInUserOnly(
                    "~/Employee/ActiveFieldServicesWorkManagementEmployeesByOperatingCenterId/");
                a.RequiresLoggedInUserOnly(
                    "~/Employee/ProductionWorkManagementEmployeesByOperatingCenterId/");
                a.RequiresLoggedInUserOnly(
                    "~/Employee/EmployeesWithCurrentProductionAssignmentsByOperatingCenterId/");
                a.RequiresLoggedInUserOnly(
                    "~/Employee/EmployeesWithCurrentProductionAssignmentsByOperatingCenterIds/");
                a.RequiresLoggedInUserOnly("~/Employee/ByOperatingCentersOrState/");
                a.RequiresLoggedInUserOnly(
                    "~/Employee/ActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/Employee/LockoutFormEmployeesByOperatingCenterId/");
                a.RequiresLoggedInUserOnly("~/Employee/HealthAndSafetyActiveEmployeesByRoleAndPartial/");
                a.RequiresLoggedInUserOnly("~/Employee/CovidIssueActiveEmployeesByState/");
                a.RequiresLoggedInUserOnly("~/Employee/ByOperatingCentersOrStateForHumanResourcesCovid/");
                a.RequiresLoggedInUserOnly(
                    "~/Employee/GetEmployeeByOperatingCentersWhoHaveEnteredSystemDeliveryEntries/");
                a.RequiresLoggedInUserOnly("~/Employee/PlanActiveEmployeesByRoleAndPartial/");
                a.RequiresLoggedInUserOnly("~/Employee/ActiveEmployeesByEmployeePartialFirstOrLastName/");
                a.RequiresLoggedInUserOnly("~/Employee/ActiveEmployeesDescriptionByEmployeePartialFirstOrLastName/");
                a.RequiresLoggedInUserOnly("~/Employee/AllEmployeesByEmployeePartialFirstOrLastName/");
                a.RequiresLoggedInUserOnly("~/Employee/RiskRegisterEmployeesByOperatingCenterId/");
                a.RequiresLoggedInUserOnly(
                    "~/Employee/ActiveEmployeesByOperatingCenterIdForPreJobSafetyBriefs/");
                a.RequiresLoggedInUserOnly(
                    "~/Employee/GetEmployeesForProductionWorkOrderSchedulingByOperatingCenterAndProductionSkillSet/");
                a.RequiresLoggedInUserOnly(
                    "~/Employee/GetEmployeesForProductionWorkOrderSchedulingByOperatingCenter/");

                a.RequiresRole("~/Employee/HasOneDayDoctorsNoteRestriction/", module);
                a.RequiresLoggedInUserOnly("~/Employee/EmployeesByStateIdOrOperatingCenterIdAndStatusIdWithStatus/");
                a.RequiresLoggedInUserOnly("~/Employee/EmployeesByOperatingCenterIdAndStatusId/");
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            var employee0 = GetFactory<EmployeeFactory>().Create();
            var employee1 = GetFactory<EmployeeFactory>().Create();

            var search = new SearchEmployee();

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(employee0.Id, "Id");
                helper.AreEqual(employee0.FullName, "FullName");
                helper.AreEqual(employee1.Id, "Id", 1);
                helper.AreEqual(employee1.FullName, "FullName", 1);
            }
        }

        #endregion

        #region Search

        [TestMethod]
        public void TestSearchEmployeeAccountabilityActionsReturnsRightResults()
        {
            var employee = GetFactory<EmployeeFactory>().Create();
            var badEmployee = GetFactory<EmployeeFactory>().Create();
            var empAccAc = GetFactory<EmployeeAccountabilityActionFactory>().Create(new {
                Employee = employee
            });

            employee.EmployeeAccountabilityActions.Add(empAccAc);

            var model = new SearchEmployee {
                AccountabilityActionTakenType = empAccAc.AccountabilityActionTakenType.Id,
                EmployeeAccountabilityActions = true,
                OperatingCenter = badEmployee.OperatingCenter.Id
            };

            _target.Index(model);
            CollectionAssert.Contains(model.Results.ToList(), employee);
            CollectionAssert.DoesNotContain(model.Results.ToList(), badEmployee);
            Assert.AreEqual(1, model.Results.Count());
        }

        #endregion

        #region ByOperatingCentersOrState

        [TestMethod]
        public void TestByOperatingCentersOrStateReturnsEmployeesByMultipleOperatingCenters()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var emp1 = GetFactory<EmployeeFactory>().Create(new { OperatingCenter = opc1 });
            var emp2 = GetFactory<EmployeeFactory>().Create(new { OperatingCenter = opc2 });

            // Test that it filters correctly with just one op center
            var result = (CascadingActionResult)_target.ByOperatingCentersOrState(null, new[] { opc1.Id });
            var data = (IEnumerable<EmployeeDisplayItem>)result.Data;
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(emp1.Id, data.Single().Id);

            // Test that it filters wth multiple op centers
            result = (CascadingActionResult)_target.ByOperatingCentersOrState(null, new[] {
                opc1.Id,
                opc2.Id
            });
            data = (IEnumerable<EmployeeDisplayItem>)result.Data;
            Assert.AreEqual(2, data.Count());
            Assert.IsTrue(data.Any(x => x.Id == emp1.Id));
            Assert.IsTrue(data.Any(x => x.Id == emp2.Id));
        }

        [TestMethod]
        public void TestByOperatingCenterOrStateReturnsEmployeesByStateIfOperatingCenterIsNullOrEmpty()
        {
            var state1 = GetFactory<StateFactory>().Create();
            var state2 = GetFactory<StateFactory>().Create(new { Abbreviation = "QQ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state2 });
            var emp1 = GetFactory<EmployeeFactory>().Create(new { OperatingCenter = opc1 });
            var emp2 = GetFactory<EmployeeFactory>().Create(new { OperatingCenter = opc2 });

            // Test that this filters by state correctly with a null operating center param.
            var result = (CascadingActionResult)_target.ByOperatingCentersOrState(state1.Id, null);
            var data = (IEnumerable<EmployeeDisplayItem>)result.Data;
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(emp1.Id, data.Single().Id);

            // Also test that it works when the operating center param is an empty array.
            result = (CascadingActionResult)_target.ByOperatingCentersOrState(state1.Id, new int[] { });
            data = (IEnumerable<EmployeeDisplayItem>)result.Data;
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(emp1.Id, data.Single().Id);
        }

        [TestMethod]
        public void TestByOperatingCenterOrStateReturnsEmptyCascadingActionResultIfOperatingCenterAndStateAreBothNull()
        {
            var result = (CascadingActionResult)_target.ByOperatingCentersOrState(null, null);
            Assert.IsFalse(result.Data.Any());
        }

        #endregion

        #region ActiveByState

        [TestMethod]
        public void TestByStateReturnsEmployeesByState()
        {
            var activeEmployeeStatuses = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var inactiveEmployeeStatuses = GetFactory<InactiveEmployeeStatusFactory>().Create();
            var state1 = GetFactory<StateFactory>().Create();
            var state2 = GetFactory<StateFactory>().Create(new { Abbreviation = "QQ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state2 });
            var emp1 = GetFactory<EmployeeFactory>().Create(new {
                OperatingCenter = opc1,
                Status = activeEmployeeStatuses
            });
            var emp2 = GetFactory<EmployeeFactory>().Create(new {
                OperatingCenter = opc2,
                Status = activeEmployeeStatuses
            });
            var emp3 = GetFactory<EmployeeFactory>().Create(new {
                OperatingCenter = opc1,
                Status = inactiveEmployeeStatuses
            });

            var result = (CascadingActionResult)_target.ActiveEmployeesByStateId(state1.Id);
            var data = (IEnumerable<EmployeeDisplayItem>)result.Data;
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(emp1.Id, data.Single().Id);
        }

        // DOES NOT PASS WHEN ALL TESTS ARE RUN
        [TestMethod]
        public void TestByStateAndPartialReturnsCorrectEmployees()
        {
            var activeEmployeeStatuses = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var inactiveEmployeeStatuses = GetFactory<InactiveEmployeeStatusFactory>().Create();
            var state1 = GetFactory<StateFactory>().Create();
            var state2 = GetFactory<StateFactory>().Create(new { Abbreviation = "QQ" });
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state1 });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state2 });
            var emp1 = GetFactory<EmployeeFactory>().Create(new {
                OperatingCenter = opc1,
                Status = activeEmployeeStatuses,
                FirstName = "Bill",
                LastName = "S. Preston, Esq."
            });
            var emp2 = GetFactory<EmployeeFactory>().Create(new {
                OperatingCenter = opc2,
                Status = activeEmployeeStatuses
            });
            var emp3 = GetFactory<EmployeeFactory>().Create(new {
                OperatingCenter = opc1,
                Status = inactiveEmployeeStatuses
            });

            var result = (AutoCompleteResult)_target.ActiveEmployeesByStateIdAndPartial("bill", state1.Id);
            var data = (IEnumerable<Employee>)result.Data;
            Assert.AreEqual(1, data.Count());
            Assert.AreEqual(emp1.Id, data.Single().Id);
        }

        #endregion

        #region ActiveEmployeesByOperatingCenterIdForPreJobSafetyBriefs

        [TestMethod]
        public void TestActiveEmployeesByOperatingCenterIdForPreJobSafetyBriefsReturnsActiveEmployeesBasedOnRole()
        {
            AssertActiveEmployeesByOperatingCenterIdAndSomeRoleWorksProperly(
                _target.ActiveEmployeesByOperatingCenterIdForPreJobSafetyBriefs,
                RoleApplications.Operations,
                ProductionPreJobSafetyBriefController.ROLE);
        }

        #endregion

        #region ActiveLockoutFormEmployeesByOperatingCenterId

        [TestMethod]
        public void TestActiveLockoutFormEmployeesByOperatingCenterIdReturnsActiveEmployeesBasedOnRole()
        {
            AssertActiveEmployeesByOperatingCenterIdAndSomeRoleWorksProperly(
                id => _target.ActiveLockoutFormEmployeesByOperatingCenterId(id),
                RoleApplications.Operations,
                LockoutFormController.ROLE);
        }

        #endregion

        #region LockoutFormEmployeesByOperatingCenterId

        [TestMethod]
        public void TestLockoutFormEmployeesByOperatingCenterIdReturnsActiveAndInactiveEmployeesBasedOnRole()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<ActiveEmployeeFactory>().Create();
            var user = GetEntityFactory<User>().Create(new { Employee = employee });

            // Test employee with user without any matching roles does not end up in list 
            var result = (CascadingActionResult)_target.LockoutFormEmployeesByOperatingCenterId(opc.Id);
            Assert.IsFalse(result.Data.Any());

            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {
                    Id = RoleApplications.Operations
                }),
                Module = GetFactory<ModuleFactory>().Create(new {
                    Id = RoleModules.OperationsLockoutForms
                }),
                Action = _readAction,
                User = user
            });

            void doTest(RoleAction action, OperatingCenter opcForRole)
            {
                role.OperatingCenter = opcForRole;
                role.Action = action;
                Session.Save(role);
                Session.Flush();

                var result1 = (CascadingActionResult)_target
                   .LockoutFormEmployeesByOperatingCenterId(opc.Id);
                var testableResult = result1.Data.Any();
                Assert.IsTrue(
                    testableResult,
                    $"No result was found for action '{action.Description}' and operating " +
                    $"center '{opcForRole}'.");
            }

            foreach (var action in new[] {
                         _readAction,
                         _addAction,
                         _editAction,
                         _deleteAction,
                         _userAdminAction
                     })
            {
                // Do wildcard match
                doTest(_addAction, null);
                // Do opc match
                doTest(action, opc);
            }

            // inactive user with role should show up
            employee.Status = GetFactory<InactiveEmployeeStatusFactory>().Create();
            Session.Save(employee);
            doTest(_userAdminAction, null);
        }

        #endregion

        #region ActiveEmployeesByOpedratingCenterAndSupervisor

        [TestMethod]
        public void
            TestActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterIdGetsCascadingResultForMatchingOperatingCentersAndActiveEmployees()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var supervisor = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });
            var invalidSupervisor = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });
            var good = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc,
                ReportsTo = supervisor
            });
            var invalidEmployee1 = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc,
                ReportsTo = invalidSupervisor
            });
            var invalidEmployee2 = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });
            var invalidEmployee3 = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<InactiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });
            _currentUser.IsAdmin = false;
            _currentUser.Employee = supervisor;

            var result = (CascadingActionResult)_target
               .ActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterId(opc.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(good.Id.ToString(), actual[1].Value);
        }

        [TestMethod]
        public void
            TestActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterIdGetsAllCascadingResultForMatchingOperatingCentersWhenSiteAdmin()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var otherOpc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var supervisor = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });
            var invalidSupervisor = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });
            var validEmployee1 = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc,
                ReportsTo = supervisor
            });
            var validEmployee2 = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc,
                ReportsTo = invalidSupervisor
            });
            var validEmployee3 = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<ActiveEmployeeStatusFactory>().Create(),
                OperatingCenter = opc
            });
            var invalidEmployee1 = GetFactory<EmployeeFactory>().Create(new {
                Status = GetFactory<InactiveEmployeeStatusFactory>().Create(),
                OperatingCenter = otherOpc
            });

            var result = (CascadingActionResult)_target
               .ActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterId(opc.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(5, actual.Count() - 1);
            Assert.IsFalse(actual.Select(x => x.Value).Contains(invalidEmployee1.Id.ToString()));
        }

        [TestMethod]
        public void
            TestActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterIdReturnsDirectReportsUpToThreeLevelsDeep()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var currentUserEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc
            });
            var supervisor2 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc,
                ReportsTo = currentUserEmployee
            });
            var supervisor3 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc,
                ReportsTo = supervisor2
            });
            var supervisor4 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc,
                ReportsTo = supervisor3
            });
            var supervisor5 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc,
                ReportsTo = supervisor4
            });
            _currentUser.IsAdmin = false;
            _currentUser.Employee = currentUserEmployee;

            var result = (CascadingActionResult)_target
               .ActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterId(opc.Id);
            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(3, actual.Count());
            Assert.IsFalse(
                actual.Any(x => x.Id == currentUserEmployee.Id),
                "The current user shouldn't appear in these results.");
            Assert.IsTrue(actual.Any(x => x.Id == supervisor2.Id));
            Assert.IsTrue(actual.Any(x => x.Id == supervisor3.Id));
            Assert.IsTrue(actual.Any(x => x.Id == supervisor4.Id));
            Assert.IsFalse(
                actual.Any(x => x.Id == supervisor5.Id),
                "Any employees that are more than three ReportsTo levels away from the current user " +
                "employee should not be visible.");
        }

        [TestMethod]
        public void
            TestActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterIdReturnsAnyEmployeeThatMatchesWithTheCurrentUsersUserAdmiRoleForOperationsIncidents()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();

            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>()
                   .Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.OperationsIncidents }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.UserAdministrator }),
                User = _currentUser,
                OperatingCenter = opc
            });
            Assert.IsTrue(_currentUser.Roles.Contains(role));

            var currentUserEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc
            });
            var goodEmployee = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = opc });
            var badEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create()
            });

            _currentUser.IsAdmin = false;
            _currentUser.Employee = currentUserEmployee;

            var result =
                (CascadingActionResult)_target.ActiveEmployeesForCurrentUsersDirectReportsByOperatingCenterId(opc.Id);
            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.AreEqual(2, actual.Count());
            Assert.IsTrue(
                actual.Any(x => x.Id == currentUserEmployee.Id),
                "User should see themselves if they have the same operating center as their role.");
            Assert.IsTrue(
                actual.Any(x => x.Id == goodEmployee.Id),
                "User should see employees with same operating center even if they aren't a direct " +
                "report to the current user.");
            Assert.IsFalse(
                actual.Any(x => x.Id == badEmployee.Id),
                "User should not see employees with a different operating center if they aren't a direct " +
                "report to the current user.");
        }

        [TestMethod]
        public void TestHealthAndSafetyActiveEmployeesByRoleAndPartialReturnsTheEmployeesForAdmins()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var fName = "flip";
            var currentUserEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc
            });
            var goodEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc,
                FirstName = fName
            });
            var goodEmployee2 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                FirstName = fName
            });

            _currentUser.IsAdmin = true;
            _currentUser.Employee = currentUserEmployee;

            var result = (AutoCompleteResult)_target.HealthAndSafetyActiveEmployeesByRoleAndPartial(fName);
            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.IsTrue(actual.Any(x => x.Id == goodEmployee.Id));
            Assert.IsTrue(actual.Any(x => x.Id == goodEmployee2.Id));
        }

        [TestMethod]
        public void TestCovidIssueActiveEmployeesByStateReturnsTheCorrectEmployeesByRoleAndOperatingCenter()
        {
            var opcntrs = GetFactory<UniqueOperatingCenterFactory>().CreateList(2);
            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {
                    Id = RoleApplications.HumanResources
                }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.HumanResourcesCovid }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.UserAdministrator }),
                User = _currentUser,
                OperatingCenter = opcntrs[0]
            });
            var goodEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opcntrs[0]
            });
            var badEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opcntrs[1]
            });
            _currentUser.IsAdmin = false;

            var result = (CascadingActionResult)_target
               .CovidIssueActiveEmployeesByState(opcntrs[0].State.Id);
            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.IsTrue(
                actual.Any(x => x.Id == goodEmployee.Id),
                "employee is not in the operating center for the role ");
            Assert.IsFalse(
                actual.Any(x => x.Id == badEmployee.Id),
                "employee is in the operating center for the role when it should not be");
        }

        [TestMethod]
        public void TestByOperatingCentersOrStateForHumanResourcesCovidReturnsTheCorrectEmployeesByRoleAndState()
        {
            var nj = GetEntityFactory<State>().Create(new { Abbreviation = "NJ", Name = "New Jersey" });
            var ny = GetEntityFactory<State>().Create(new { Abbreviation = "NY", Name = "New York" });
            var opcntrs = GetFactory<UniqueOperatingCenterFactory>().CreateList(2, new { State = nj });
            var role1 = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {
                    Id = RoleApplications.HumanResources
                }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.HumanResourcesCovid }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.UserAdministrator }),
                User = _currentUser,
                OperatingCenter = opcntrs[0]
            });
            var role2 = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {
                    Id = RoleApplications.HumanResources
                }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.HumanResourcesCovid }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.UserAdministrator }),
                User = _currentUser,
                OperatingCenter = opcntrs[1]
            });
            var badOpCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = ny });
            var goodEmployee1 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opcntrs[0]
            });
            var goodEmployee2 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opcntrs[1]
            });
            var badEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = badOpCntr
            });
            _currentUser.IsAdmin = false;

            var result = (CascadingActionResult)_target
               .ByOperatingCentersOrStateForHumanResourcesCovid(nj.Id, null);
            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.IsTrue(
                actual.Any(x => x.Id == goodEmployee1.Id),
                "employee is not in the operating center for the role ");
            Assert.IsTrue(
                actual.Any(x => x.Id == goodEmployee2.Id),
                "employee is not in the operating center for the role ");
            Assert.IsFalse(
                actual.Any(x => x.Id == badEmployee.Id),
                "employee is in the operating center for the role when it should not be");
        }

        [TestMethod]
        public void
            TestByOperatingCenterOrStateForHumanResourcesCovidReturnsTheCorrectEmployeesByRoleAndOperatingCenter()
        {
            var nj = GetEntityFactory<State>().Create(new { Abbreviation = "NJ", Name = "New Jersey" });
            var ny = GetEntityFactory<State>().Create(new { Abbreviation = "NY", Name = "New York" });
            var opcntrs = GetFactory<UniqueOperatingCenterFactory>().CreateList(2, new { State = nj });
            var role1 = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {
                    Id = RoleApplications.HumanResources
                }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.HumanResourcesCovid }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.UserAdministrator }),
                User = _currentUser,
                OperatingCenter = opcntrs[0]
            });
            var role2 = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {
                    Id = RoleApplications.HumanResources
                }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.HumanResourcesCovid }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.UserAdministrator }),
                User = _currentUser,
                OperatingCenter = opcntrs[1]
            });
            var badOpCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = ny });
            var goodEmployee1 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opcntrs[0]
            });
            var goodEmployee2 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opcntrs[1]
            });
            var badEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = badOpCntr
            });
            _currentUser.IsAdmin = false;

            var result = (CascadingActionResult)_target
               .ByOperatingCentersOrStateForHumanResourcesCovid(nj.Id, new[] { opcntrs[1].Id });
            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.IsFalse(
                actual.Any(x => x.Id == goodEmployee1.Id),
                "employee is in the operating center for the role ");
            Assert.IsTrue(
                actual.Any(x => x.Id == goodEmployee2.Id),
                "employee is not in the operating center for the role ");
            Assert.IsFalse(
                actual.Any(x => x.Id == badEmployee.Id),
                "employee is in the operating center for the role when it should not be");
        }

        [TestMethod]
        public void
            TestActiveEmployeesByCurrentUsersOperatingCentersAndSelectedOperatingCentersReturnsAnyEmployeesThatMatchesWithTheCurrentUsersUserRoleOrMultipleOperatingCenters()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var sName = "flip";
            int[] opcIds = { opc.Id, opc2.Id };
            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>()
                   .Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>()
                   .Create(new { Id = RoleModules.OperationsHealthAndSafety }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.UserAdministrator }),
                User = _currentUser,
                OperatingCenter = opc
            });
            var role2 = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>()
                   .Create(new { Id = RoleApplications.Operations }),
                Module = GetFactory<ModuleFactory>()
                   .Create(new { Id = RoleModules.OperationsHealthAndSafety }),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.UserAdministrator }),
                User = _currentUser,
                OperatingCenter = opc2
            });
            Assert.IsTrue(_currentUser.Roles.Contains(role));
            Assert.IsTrue(_currentUser.Roles.Contains(role2));

            var currentUserEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc,
                LastName = sName
            });
            var goodEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc,
                FirstName = sName
            });
            var goodEmployeeJr = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc2,
                FirstName = sName
            });
            var goodEmployeeJrSr = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = opc2,
                LastName = sName
            });
            var badEmployee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(),
                FirstName = sName
            });

            _currentUser.IsAdmin = false;
            _currentUser.Employee = currentUserEmployee;

            var result = (AutoCompleteResult)_target.HealthAndSafetyActiveEmployeesByRoleAndPartial(sName);
            var actual = (IEnumerable<dynamic>)result.Data;

            Assert.IsTrue(actual.Any(x => x.Id == goodEmployee.Id));
            Assert.IsTrue(actual.Any(x => x.Id == goodEmployeeJr.Id));
            Assert.IsFalse(actual.Any(x => x.Id == badEmployee.Id));
        }

        #endregion

        #region GetEmployeeByOperatingCentersWhoHaveEnteredSystemDeliveryEquipmentEntries

        [TestMethod]
        public void TestGetEmployeeByOperatingCentersWhoHaveEnteredSystemDeliveryEntries()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenter2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var facility = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter });
            var facility2 = GetEntityFactory<Facility>().Create(new { OperatingCenter = operatingCenter2 });
            var employee = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = operatingCenter
            });
            var employee2 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = operatingCenter2
            });
            var employee3 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = operatingCenter
            });
            var employee4 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = operatingCenter2
            });
            var employee5 = GetFactory<ActiveEmployeeFactory>().Create(new {
                OperatingCenter = operatingCenter
            });
            var facilityEntry = GetEntityFactory<SystemDeliveryFacilityEntry>()
               .Create(new { Facility = facility, EnteredBy = employee });
            var facilityEntry2 = GetEntityFactory<SystemDeliveryFacilityEntry>()
               .Create(new { Facility = facility2, EnteredBy = employee2 });

            Session.Save(employee);
            Session.Save(employee2);
            Session.Save(employee3);
            Session.Save(employee4);
            Session.Save(employee5);
            Session.Save(facilityEntry);
            Session.Save(facilityEntry2);

            Session.Flush();

            var result = (CascadingActionResult)_target
               .GetEmployeeByOperatingCentersWhoHaveEnteredSystemDeliveryEntries(new[] {
                    operatingCenter.Id,
                    operatingCenter2.Id
                });
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(actual.Count() - 1, 2);
            Assert.IsTrue(actual.Any(x => x.Value == employee.Id.ToString()));
            Assert.IsTrue(actual.Any(x => x.Value == employee2.Id.ToString()));
            Assert.IsFalse(actual.Any(x => x.Value == employee3.Id.ToString()));
            Assert.IsFalse(actual.Any(x => x.Value == employee4.Id.ToString()));
            Assert.IsFalse(actual.Any(x => x.Value == employee5.Id.ToString()));
        }

        #endregion

        #region EmployeesWithCurrentProductionAssignmentsByOperatingCenterIds

        [TestMethod]
        public void TestEmployeesWithCurrentProductionAssignmentsByOperatingCenterIds()
        {
            var nj6 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ6" });
            var sov = GetFactory<OperatingCenterFactory>().Create(new {
                OperatingCenterCode = "SOV",
                OperatedByOperatingCenter = nj6
            });
            var other = GetFactory<UniqueOperatingCenterFactory>().Create();
            var emp1 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = nj6 });
            var emp2 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = sov });
            var emp3 = GetFactory<ActiveEmployeeFactory>().Create(new { OperatingCenter = other });
            var ea1 = GetFactory<EmployeeAssignmentFactory>().Create(new {
                AssignedTo = emp1,
                ProductionWorkOrder = GetFactory<ProductionWorkOrderFactory>().Create(new {
                    OperatingCenter = nj6
                })
            });
            var ea2 = GetFactory<EmployeeAssignmentFactory>().Create(new {
                AssignedTo = emp2,
                ProductionWorkOrder = GetFactory<ProductionWorkOrderFactory>().Create(new {
                    OperatingCenter = sov
                })
            });
            var ea3 = GetFactory<EmployeeAssignmentFactory>().Create(new {
                AssignedTo = emp3,
                ProductionWorkOrder = GetFactory<ProductionWorkOrderFactory>().Create(new {
                    OperatingCenter = other
                })
            });
            emp1.ProductionAssignments.Add(ea1);
            emp2.RelatedProductionAssignments.Add(ea2);
            emp3.ProductionAssignments.Add(ea3);

            var result = (CascadingActionResult)_target
               .EmployeesWithCurrentProductionAssignmentsByOperatingCenterIds(new[] { sov.Id, nj6.Id });
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(2, actual.Count() - 1); // -1 for the empty select item.
        }

        #endregion

        #region ActiveEmployeesByOperatingCenterAndEmployee

        [TestMethod]
        public void TestActiveEmployeesByOperatingCenterAndEmployee()
        {
            var activeEmployeeStatus = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var inactiveEmployeeStatus = GetFactory<InactiveEmployeeStatusFactory>().Create();
            var emp1 = GetFactory<EmployeeFactory>().Create(new {
                Status = activeEmployeeStatus,
                FirstName = "Bond",
                LastName = "James"
            });
            var emp2 = GetFactory<EmployeeFactory>().Create(new {
                Status = activeEmployeeStatus,
                FirstName = "James",
                LastName = "S. Preston, Esq."
            });
            var emp3 = GetFactory<EmployeeFactory>().Create(new {
                Status = inactiveEmployeeStatus,
                FirstName = "James",
                LastName = "Preston"
            });

            var result1 = (AutoCompleteResult)_target
               .ActiveEmployeesByEmployeePartialFirstOrLastName("bon");
            var data1 = (IEnumerable<Employee>)result1.Data;
            Assert.AreEqual(1, data1.Count());
            Assert.AreEqual(emp1.Id, data1.Single().Id);

            var result2 = (AutoCompleteResult)_target
               .ActiveEmployeesByEmployeePartialFirstOrLastName("jam");
            var data2 = (IEnumerable<Employee>)result2.Data;
            Assert.AreEqual(2, data2.Count());
        }

        [TestMethod]
        public void TestActiveEmployeesDescriptionByEmployeePartialFirstOrLastName()
        {
            var activeEmployeeStatus = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var inactiveEmployeeStatus = GetFactory<InactiveEmployeeStatusFactory>().Create();
            var emp1 = GetFactory<EmployeeFactory>().Create(new {
                Status = activeEmployeeStatus,
                FirstName = "Bond",
                LastName = "James"
            });
            var emp2 = GetFactory<EmployeeFactory>().Create(new {
                Status = activeEmployeeStatus,
                FirstName = "James",
                LastName = "S. Preston, Esq."
            });
            var emp3 = GetFactory<EmployeeFactory>().Create(new {
                Status = inactiveEmployeeStatus,
                FirstName = "James",
                LastName = "Preston"
            });

            var result1 = (AutoCompleteResult)_target
               .ActiveEmployeesDescriptionByEmployeePartialFirstOrLastName("bon");
            var data1 = (IEnumerable<Employee>)result1.Data;
            Assert.AreEqual(1, data1.Count());
            Assert.AreEqual(emp1.Id, data1.Single().Id);

            var result2 = (AutoCompleteResult)_target
               .ActiveEmployeesDescriptionByEmployeePartialFirstOrLastName("jam");
            var data2 = (IEnumerable<Employee>)result2.Data;
            Assert.AreEqual(2, data2.Count());
        }

        #endregion

        #region AllEmployeesByOperatingCenterAndEmployee

        [TestMethod]
        public void TestAllEmployeesByOperatingCenterAndEmployee()
        {
            var activeEmployeeStatus = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var inactiveEmployeeStatus = GetFactory<InactiveEmployeeStatusFactory>().Create();
            var emp1 = GetFactory<EmployeeFactory>().Create(new {
                Status = activeEmployeeStatus,
                FirstName = "Bill",
                LastName = "S. Preston, Esq."
            });
            var emp2 = GetFactory<EmployeeFactory>().Create(new {
                Status = inactiveEmployeeStatus,
                FirstName = "Bond",
                LastName = "James"
            });

            var result1 = (AutoCompleteResult)_target.AllEmployeesByEmployeePartialFirstOrLastName("bil");
            var data1 = (IEnumerable<Employee>)result1.Data;
            Assert.AreEqual(1, data1.Count());
            Assert.AreEqual(emp1.Id, data1.Single().Id);

            var result2 = (AutoCompleteResult)_target.AllEmployeesByEmployeePartialFirstOrLastName("Jame");
            var data2 = (IEnumerable<Employee>)result2.Data;
            Assert.AreEqual(1, data2.Count());
            Assert.AreEqual(emp2.Id, data2.Single().Id);

            var result3 = (AutoCompleteResult)_target.AllEmployeesByEmployeePartialFirstOrLastName("b");
            var data3 = (IEnumerable<Employee>)result3.Data;
            Assert.AreEqual(0, data3.Count());
        }

        #endregion

        #region RiskRegisterEmployeesByOperatingCenterId

        [TestMethod]
        public void TestRiskRegisterEmployeesByOperatingCenterId()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<ActiveEmployeeFactory>().Create();
            var user = GetEntityFactory<User>().Create(new { Employee = employee });

            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new {
                    Id = RoleApplications.Engineering
                }),
                Module = GetFactory<ModuleFactory>().Create(new {
                    Id = RoleModules.EngineeringRiskRegister
                }),
                Action = _readAction,
                User = user
            });

            void doTest(RoleAction action, OperatingCenter opcForRole)
            {
                role.OperatingCenter = opcForRole;
                role.Action = action;
                Session.Save(role);
                Session.Flush();

                var result1 = (CascadingActionResult)_target
                   .RiskRegisterEmployeesByOperatingCenterId(opc.Id);
                var testableResult = result1.Data.Any();
                Assert.IsTrue(
                    testableResult,
                    $"No result was found for action '{action.Description}' and operating " +
                    $"center '{opcForRole}'.");
            }

            foreach (var action in new[] {
                         _readAction,
                         _addAction,
                         _editAction,
                         _deleteAction,
                         _userAdminAction
                     })
            {
                // Do opc match
                doTest(action, opc);
            }

            // inactive user with role should show up
            employee.Status = GetFactory<InactiveEmployeeStatusFactory>().Create();
            Session.Save(employee);
            doTest(_userAdminAction, null);
        }

        #endregion

        #region ProductionWorkOrderAndSchedulingByOperatingCenterAndProductionSkillSet

        [TestMethod]
        public void TestGetEmployeesForProductionWorkOrderAndSchedulingByOperatingCenterAndProductionSkillSetReturnsCascadingResultForMatchingActiveEmployees()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<ActiveEmployeeFactory>().Create();
            var employeeProductionSkillSet = GetFactory<EmployeeProductionSkillSetFactory>().Create();
            employee.ProductionSkillSets.Add(employeeProductionSkillSet);
            employeeProductionSkillSet.Employee = employee;

            var user = GetEntityFactory<User>().Create(new { Employee = employee });

            var application = GetFactory<ApplicationFactory>().Create(new {
                Id = RoleApplications.Production
            });

            var module = GetFactory<ModuleFactory>().Create(new {
                Id = RoleModules.ProductionWorkManagement
            });

            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = _editAction,
                User = user
            });

            var role1 = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = _userAdminAction,
                User = user
            });

            void doTest(RoleAction action, OperatingCenter opcForRole)
            {
                role.OperatingCenter = opcForRole;
                role.Action = action;

                role1.OperatingCenter = opcForRole;
                role1.Action = action;
                
                Session.Flush();

                var result1 = (CascadingActionResult)_target
                   .GetEmployeesForProductionWorkOrderSchedulingByOperatingCenterAndProductionSkillSet(opc.Id, null);
                var actual1 = result1.GetSelectListItems().ToArray();
                Assert.AreEqual(actual1[1].Text, employee.Description);

                var result2 = (CascadingActionResult)_target
                   .GetEmployeesForProductionWorkOrderSchedulingByOperatingCenterAndProductionSkillSet(opc.Id, employeeProductionSkillSet.Id);
                var actual2 = result2.GetSelectListItems().ToArray();
                Assert.AreEqual(actual2[1].Text, employee.Description);
            }

            foreach (var action in new[] {
                         _editAction,
                         _userAdminAction
                     })
            {
                doTest(action, opc);
            }
        }

        #endregion

        #region ProductionWorkManagementEmployeesByOperatingCenterId
        
        [TestMethod]
        public void TestProductionWorkManagementEmployeesByOperatingCenterIdReturnsCascadingResultForMatchingEmployees()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<EmployeeFactory>().Create();

            var user = GetEntityFactory<User>().Create(new { Employee = employee });

            var application = GetFactory<ApplicationFactory>().Create(new {
                Id = RoleApplications.Production
            });

            var module = GetFactory<ModuleFactory>().Create(new {
                Id = RoleModules.ProductionWorkManagement
            });

            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = _editAction,
                User = user
            });

            var role1 = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = _userAdminAction,
                User = user
            });

            void doTest(RoleAction action, OperatingCenter opcForRole)
            {
                role.OperatingCenter = opcForRole;
                role.Action = action;

                role1.OperatingCenter = opcForRole;
                role1.Action = action;

                Session.Flush();

                var result1 = (CascadingActionResult)_target
                   .ProductionWorkManagementEmployeesByOperatingCenterId(opc.Id);
                var actual1 = result1.GetSelectListItems().ToArray();
                Assert.AreEqual(actual1[1].Text, employee.Description);
            }

            foreach (var action in new[] {
                         _editAction,
                         _userAdminAction
                     })
            {
                doTest(action, opc);
            }
        }

        #endregion

        #region ProductionWorkOrderAndSchedulingByOperatingCenter

        [TestMethod]
        public void TestGetEmployeesForProductionWorkOrderAndSchedulingByOperatingCenterReturnsCascadingResultForMatchingActiveEmployees()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var employee = GetFactory<ActiveEmployeeFactory>().Create();

            var user = GetEntityFactory<User>().Create(new { Employee = employee });

            var application = GetFactory<ApplicationFactory>().Create(new {
                Id = RoleApplications.Production
            });

            var module = GetFactory<ModuleFactory>().Create(new {
                Id = RoleModules.ProductionWorkManagement
            });

            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = _editAction,
                User = user
            });

            var role1 = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = _userAdminAction,
                User = user
            });

            void doTest(RoleAction action, OperatingCenter opcForRole)
            {
                role.OperatingCenter = opcForRole;
                role.Action = action;

                role1.OperatingCenter = opcForRole;
                role1.Action = action;

                Session.Flush();

                var result1 = (CascadingActionResult)_target
                   .GetEmployeesForProductionWorkOrderSchedulingByOperatingCenter(opc.Id);
                var actual1 = result1.GetSelectListItems().ToArray();
                Assert.AreEqual(actual1[1].Text, employee.Description);
            }

            foreach (var action in new[] {
                         _editAction,
                         _userAdminAction
                     })
            {
                doTest(action, opc);
            }
        }

        #endregion
    }
}
