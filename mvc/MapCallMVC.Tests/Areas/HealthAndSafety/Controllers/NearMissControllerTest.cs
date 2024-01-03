using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.HealthAndSafety.Controllers;
using MapCallMVC.Areas.HealthAndSafety.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.HealthAndSafety.Controllers
{
    [TestClass]
    public class NearMissControllerTest : MapCallMvcControllerTestBase<NearMissController, NearMiss>
    {
        #region Init

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditNearMiss)vm;
                model.State = GetEntityFactory<State>().Create().Id;
                model.OperatingCenter = GetEntityFactory<OperatingCenter>().Create().Id;
                model.Type = GetEntityFactory<NearMissType>().Create().Id;
                model.Category = GetEntityFactory<NearMissCategory>().Create().Id;
                model.SystemType = GetEntityFactory<SystemType>().Create().Id;
                model.CompletedCorrectiveActions = false;
            };
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = NearMissController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/HealthAndSafety/NearMiss/Search/", role);
                a.RequiresRole("~/HealthAndSafety/NearMiss/Show/", role);
                a.RequiresRole("~/HealthAndSafety/NearMiss/Index/", role);
                a.RequiresRole("~/HealthAndSafety/NearMiss/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/HealthAndSafety/NearMiss/Update/", role, RoleActions.Edit);
                a.RequiresLoggedInUserOnly("~/HealthAndSafety/NearMiss/ReportedByEmployeePartialFirstOrLastName");
            });
        }

        #endregion

        [TestMethod]
        public void TestReportedByEmployeePartialFirstOrLastName()
        {
            var entity0 = GetEntityFactory<NearMiss>().Create(new { ReportedBy = "Mohit Patel" });
            var entity1 = GetEntityFactory<NearMiss>().Create(new { ReportedBy = "Andrew Symonds" });
            var entity2 = GetEntityFactory<NearMiss>().Create(new { ReportedBy = "Partiv Patel" });
            var result = (AutoCompleteResult)_target.ReportedByEmployeePartialFirstOrLastName("Pat");
            var actual = (IEnumerable<dynamic>)result.Data;
            Assert.AreEqual(2, actual.Count());
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<NearMiss>().Create(new {
                Id = 96,
                OperatingCenter = GetEntityFactory<OperatingCenter>().Create(),
                ReportedBy = "Andrew Symonds",
                Type = GetEntityFactory<NearMissType>().Create(),
                Category = GetEntityFactory<NearMissCategory>().Create(),
                CreatedAt = DateTime.Now,
                OccurredAt = DateTime.Today,
                Severity = "Green Continue",
                ReviewedDate = DateTime.Now,
                DateCompleted = DateTime.Today,
            });
            var search = new SearchNearMiss();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity0.OperatingCenter, "OperatingCenter");
                helper.AreEqual(entity0.ReportedBy, "ReportedBy");
                helper.AreEqual(entity0.Type, "Near Miss Type");
                helper.AreEqual(entity0.Category, "Category");
                helper.AreEqual(entity0.CreatedAt, "CreatedAt");
                helper.AreEqual(entity0.OccurredAt, "OccurredAt");
                helper.AreEqual(entity0.Severity, "Severity");
                helper.AreEqual(entity0.ReviewedDate, "ReviewedDate");
                helper.AreEqual(entity0.DateCompleted, "DateCompleted");
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<NearMiss>().Create();
            var opc = GetEntityFactory<OperatingCenter>().Create();

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditNearMiss, NearMiss>(eq, new {
                OperatingCenter = opc.Id
            })) as RedirectToRouteResult;

            Assert.AreEqual(opc.Id, Session.Get<NearMiss>(eq.Id).OperatingCenter.Id);
        }

        #endregion
    }
}