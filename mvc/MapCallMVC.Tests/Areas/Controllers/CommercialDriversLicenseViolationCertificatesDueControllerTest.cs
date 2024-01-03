using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class CommercialDriversLicenseViolationCertificatesDueControllerTest : MapCallMvcControllerTestBase<CommercialDriversLicenseViolationCertificatesDueController, Employee, EmployeeRepository>
    {
        #region Fields

        private User _user;
        private OperatingCenter _opCenter;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _opCenter = GetFactory<OperatingCenterFactory>().Create();
            _user = GetFactory<UserFactory>().Create(new { DefaultOperatingCenter = _opCenter });
            _authenticationService.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.HumanResourcesEmployeeLimited;
                a.RequiresRole("~/Reports/CommercialDriversLicenseViolationCertificatesDue/Search", module);
                a.RequiresRole("~/Reports/CommercialDriversLicenseViolationCertificatesDue/Index", module);
            });
        }

        [TestMethod]
        public void TestIndexXLSExportExportsExcel()
        {
            var e = GetEntityFactory<Employee>().Create(new { OperatingCenter = _opCenter });
            var search = new SearchCommercialDriversLicenseViolationCertificatesDue();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(e.EmployeeId, "EmployeeId");
                helper.IsNull("Status");
            }
        }
    }
}