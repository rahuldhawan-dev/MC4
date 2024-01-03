using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class LockoutDeviceControllerTest : MapCallMvcControllerTestBase<LockoutDeviceController, LockoutDevice>
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsHealthAndSafety;
        public const RoleModules ROLE_LOCKOUT_FORM = RoleModules.OperationsLockoutForms;

        #endregion

        #region Fields

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            Repository = _container.GetInstance<RepositoryBase<LockoutDevice>>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.OperationsHealthAndSafety;
                a.RequiresRole("~/HealthAndSafety/LockoutDevice/Index/", module, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/LockoutDevice/Search/", module, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/LockoutDevice/Show/", module, RoleActions.Read);
                a.RequiresRole("~/HealthAndSafety/LockoutDevice/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/LockoutDevice/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/LockoutDevice/New/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/LockoutDevice/Create/", module, RoleActions.Add);
                a.RequiresRole("~/HealthAndSafety/LockoutDevice/Destroy/", module, RoleActions.Delete);
                a.RequiresLoggedInUserOnly("~/HealthAndSafety/LockoutDevice/ByOperatingCenterForCurrentUserEmployee/");
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<LockoutDevice>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<LockoutDevice>().Create(new {Description = "description 1"});
            var search = new SearchLockoutDevice();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<LockoutDevice>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditLockoutDevice, LockoutDevice>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<LockoutDevice>(eq.Id).Description);
        }

        #endregion

        #region CustomTests

        [TestMethod]
        public void TestByOperatingCenterForCurrentUserEmployeeReturnsLockoutDevicesForActiveUsersWithOpCntr()
        { 
            var opCenter1 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ3" });
            var opCenter2 = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "NJ4" });
            var user1 = GetFactory<UserFactory>().Create(new { DefaultOperatingCenter = opCenter1 });
            _authenticationService.Setup(x => x.CurrentUser).Returns(user1);

            var lockoutDevice1 = GetEntityFactory<LockoutDevice>().Create(new {OperatingCenter = opCenter1, Person = user1 });
            var lockoutDevice2 = GetEntityFactory<LockoutDevice>().Create(new { OperatingCenter = opCenter2, Person = user1 });
            Session.Flush();

            // Test that it filters correctly by op center and user
            var result = (CascadingActionResult)_target.ByOperatingCenterForCurrentUserEmployee(opCenter1.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1); // --select here--
            Assert.AreEqual(lockoutDevice1.ToString(), actual.Last().Text);

            // test for another user and op center
            var user2 = GetFactory<UserFactory>().Create(new { DefaultOperatingCenter = opCenter2 });
            _authenticationService.Setup(x => x.CurrentUser).Returns(user2);
            lockoutDevice2 = GetEntityFactory<LockoutDevice>().Create(new { OperatingCenter = opCenter2, Person = user2 });
            Session.Flush();

            // Test that it filters correctly by op center and user
            result = (CascadingActionResult)_target.ByOperatingCenterForCurrentUserEmployee(opCenter2.Id);
            actual = result.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1); // --select here--
            Assert.AreEqual(lockoutDevice2.ToString(), actual.Last().Text);
        }

        #endregion

        #endregion
    }
}
