using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class EmployeeNJDEPLicenseControllerTest : MapCallMvcControllerTestBase<EmployeeNJDEPLicenseController, Employee, EmployeeRepository>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetFactory<ActiveEmployeeFactory>().Create();
            options.InitializeSearchTester = (tester) => {
                // The tester fails because it doesn't know that the search's State property
                // is not referring to Employee.State. So it messes up trying to do set a State
                // object on a string property.
                tester.TestPropertyValues[nameof(SearchEmployeeNJDEPLicense.State)] = GetEntityFactory<State>().Create().Id;
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = EmployeeNJDEPLicenseController.ROLE;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/EmployeeNJDEPLicense/Search/", role, RoleActions.Read);
                a.RequiresRole("~/Reports/EmployeeNJDEPLicense/Index/", role, RoleActions.Read);
            });
        }
        
        #region Index

        [TestMethod]
        public void TestIndexReturnsIndexViewWithArrayOfEmployees()
        {
            var employeeStatus = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var eq1 = GetEntityFactory<Employee>().Create(new { Status = employeeStatus });
            var eq2 = GetEntityFactory<Employee>().Create(new { Status = employeeStatus });
            var search = new SearchEmployeeNJDEPLicense();
            _target.ControllerContext = new ControllerContext();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchEmployeeNJDEPLicense)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreSame(eq1, resultModel[0]);
            Assert.AreSame(eq2, resultModel[1]);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var employeeStatus = GetFactory<ActiveEmployeeStatusFactory>().Create();
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new {OperatingCenterName = "Shrewsbury", OperatingCenterCode = "NJ7"});
            var entity0 = GetEntityFactory<Employee>().Create(new {LastName = "smith", FirstName="matt", OperatingCenter = operatingCenter, Status = employeeStatus});
            var entity1 = GetEntityFactory<Employee>().Create(new {LastName = "tennant", FirstName="david", OperatingCenter = operatingCenter, Status = employeeStatus });
            var search = new SearchEmployeeNJDEPLicense();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.OperatingCenter, "OperatingCenter");
                helper.AreEqual(entity1.OperatingCenter, "OperatingCenter", 1);
                helper.AreEqual(entity0.FullName, "FullName");
                helper.AreEqual(entity1.FullName, "FullName", 1);
            }
        }

        #endregion

        #endregion
    }
}