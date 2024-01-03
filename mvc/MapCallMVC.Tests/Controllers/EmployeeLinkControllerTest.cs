using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Exceptions;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class EmployeeLinkControllerTest : MapCallMvcControllerTestBase<EmployeeLinkController, EmployeeLink>
    {
        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            var admin = GetFactory<AdminUserFactory>().Create();
            _authenticationService.SetupGet(x => x.CurrentUser).Returns(admin);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.DestroyRedirectsToReferrerOnError = true;
            options.DestroyRedirectErrorUrlFragment = "#EmployeesTab";
            options.DestroyRedirectsToReferrerOnSuccess = true;
            options.DestroyRedirectSuccessUrlFragment = "#EmployeesTab";
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/EmployeeLink/New/");
                a.RequiresLoggedInUserOnly("~/EmployeeLink/Create/");
                a.RequiresLoggedInUserOnly("~/EmployeeLink/Destroy/");
            });
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override needed to setup TableName on view model.
            var dataType = GetFactory<DataTypeFactory>().Create();
            var model = new NewEmployeeLink(_container) { TableName = dataType.TableName };

            var result = _target.New(model) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
        }

        [TestMethod]
        public void TestNewThrowsExceptionIfTableNameIsNotSet()
        {
            new[] {"", " ", null}.Each(
                str => MyAssert.Throws<ModelValidationException>(
                    () => _target.New(new NewEmployeeLink(_container) {
                        TableName = str
                    })));
        }

        [TestMethod]
        public override void TestCreateSavesNewRecordWhenModelStateIsValid()
        {
            // override because this creates several records and then does a redirect
            // to referrer.
            var dataType = GetFactory<DataTypeFactory>().Create();
            var employees = GetFactory<EmployeeFactory>().CreateArray(3);
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => null);
            RedirectToRouteResult result = null;

            MyAssert.CausesIncrease(() => result = _target.Create(new NewEmployeeLink(_container)
            {
                DataTypeId = dataType.Id,
                EmployeeIds = employees.Map<Employee, int>(x => x.Id).ToArray(),
                LinkedId = 666
            }) as RedirectToRouteResult, () => Repository.GetAll().Count(), 3);
        }

        [TestMethod]
        public override void TestCreateRedirectsToTheRecordsShowPageAfterSuccessfullySaving()
        {
            // override because this creates several records and then does a redirect
            // to referrer.
            var dataType = GetFactory<DataTypeFactory>().Create();
            var employees = GetFactory<EmployeeFactory>().CreateArray(3);
            Request.Request.Setup(x => x.UrlReferrer).Returns(() => null);
            RedirectToRouteResult result = null;

            MyAssert.CausesIncrease(() => result = _target.Create(new NewEmployeeLink(_container) {
                DataTypeId = dataType.Id,
                EmployeeIds = employees.Map<Employee, int>(x => x.Id).ToArray(),
                LinkedId = 666
            }) as RedirectToRouteResult, () => Repository.GetAll().Count(), 3);
        }

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("Doesn't return validation result to client");
        }

        [TestMethod]
        public void TestNewWorksForRegularUserAsWellAsAdmin()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            GetFactory<EmployeeFactory>().CreateList(3, new {OperatingCenter = operatingCenter});
            var role = GetFactory<RoleFactory>().Create(RoleModules.HumanResourcesUnion, operatingCenter);
            var user = GetFactory<UserFactory>().Create(new {
                Roles = new List<Role> {role}
            });
            _authenticationService.SetupGet(x => x.CurrentUser).Returns(user);

            var dataType = GetFactory<DataTypeFactory>().Create();
            var model = new NewEmployeeLink(_container) {TableName = dataType.TableName};

            var result = _target.New(model) as ViewResult;
            
            Assert.IsNotNull(result);
            Assert.AreEqual(model, result.Model);
        }

        [TestMethod]
        public void TestNewDoesNotListInactiveEmployees()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var good = GetFactory<EmployeeFactory>().Create(new { Status = GetFactory<ActiveEmployeeStatusFactory>().Create(), OperatingCenter = opc });
            var bad = GetFactory<EmployeeFactory>().Create(new { Status = GetFactory<InactiveEmployeeStatusFactory>().Create(), OperatingCenter = opc });

            var dataType = GetFactory<DataTypeFactory>().Create();
            var model = new NewEmployeeLink(_container) { TableName = dataType.TableName };

            var result = _target.New(model) as ViewResult;
            
            var employees = (IEnumerable<SelectListItem>)_target.ViewData[EmployeeLinkController.EMPLOYEES_KEY];
            Assert.AreEqual(1, employees.Count()); // 2 because argh, --select here-- ?
            Assert.AreEqual(employees.First().Value, good.Id.ToString());
            Assert.AreNotEqual(employees.First().Value, bad.Id.ToString());
        }
    }
}
