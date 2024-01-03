using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class CovidStatusReportControllerTest : MapCallMvcControllerTestBase<CovidStatusReportController, CovidIssue>
    {
        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = (tester) => {
                tester.TestPropertyValues[nameof(SearchCovidStatusReport.State)] = GetEntityFactory<State>().Create().Id;
                tester.TestPropertyValues[nameof(SearchCovidStatusReport.OperatingCenter)] = GetEntityFactory<OperatingCenter>().Create().Id;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const RoleModules role = RoleModules.HumanResourcesCovid;
                const string path = "~/Reports/CovidStatusReport/";
                a.RequiresRole(path + "Search", role, RoleActions.Read);
                a.RequiresRole(path + "Index", role, RoleActions.Read);
            });
        }

        #region Index

        protected override User CreateUser()
        {
            var user = base.CreateUser();
            user.IsAdmin = true;
            return user;
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var employee1 = GetEntityFactory<Employee>().Create(new {EmployeeId = "12345678"});
            var entity0 = GetEntityFactory<CovidIssue>().Create(new {
                Employee = employee1,
                QuarantineStatus = GetEntityFactory<CovidQuarantineStatus>().Create(),
                EstimatedReleaseDate = new DateTime(1984,4,24),
                ReleaseDate = new DateTime(1985,5,25),
                StartDate = new DateTime(1986,6,26),
                ReleaseReason = GetEntityFactory<ReleaseReason>().Create(),
                RequestType = GetEntityFactory<CovidRequestType>().Create()
            });
            var search = new SearchCovidStatusReport();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(employee1.EmployeeId, "EmployeeId");
                helper.AreEqual(entity0.OperatingCenter, "OperatingCenter");
                helper.AreEqual(entity0.PersonnelArea, "PersonnelArea");
                helper.AreEqual(entity0.SubmissionDate, "SubmissionDate");
                helper.AreEqual(entity0.SubmissionStatus, "SubmissionStatus");
                helper.AreEqual(entity0.QuarantineStatus, "QuarantineStatus");
                helper.AreEqual(entity0.StartDate, "StartDate");
                helper.AreEqual(entity0.EstimatedReleaseDate, "EstimatedReleaseDate");
                helper.AreEqual(entity0.ReleaseDate, "ReleaseDate");
                helper.AreEqual(entity0.TotalDays, "TotalDays");
                helper.AreEqual(entity0.ReleaseReason, "ReleaseReason");
                helper.AreEqual(entity0.RequestType, "RequestType");
            }
        }

        #endregion

    }
}
