using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.BPU.Controllers;
using MapCallMVC.Areas.BPU.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.BPU.Controllers
{
    [TestClass]
    public class MarkoutViolationControllerTest : MapCallMvcControllerTestBase<MarkoutViolationController, MarkoutViolation>
    {
        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.BPUGeneral;

            Authorization.Assert(a => {
                a.RequiresRole("~/BPU/MarkoutViolation/Show/", role, RoleActions.Read);
                a.RequiresRole("~/BPU/MarkoutViolation/Search/", role, RoleActions.Read);
                a.RequiresRole("~/BPU/MarkoutViolation/Index/", role, RoleActions.Read);
                a.RequiresRole("~/BPU/MarkoutViolation/New/", role, RoleActions.Add);
                a.RequiresRole("~/BPU/MarkoutViolation/Create/", role, RoleActions.Add);
                a.RequiresRole("~/BPU/MarkoutViolation/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/BPU/MarkoutViolation/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/BPU/MarkoutViolation/Destroy/", role, RoleActions.Delete);
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcelForExportableProperties()
        {
            var entity0 = GetEntityFactory<MarkoutViolation>().Create(new {
                Id = 96,
                MarkoutViolationStatus = "Pending review-Frank Hadley",
                OperatingCenter = GetEntityFactory<OperatingCenter>().Create(),
                Violation = "Did not properly mark",
                DateOfViolationNotice = DateTime.Now,
                OCNumber = "455126-173380545",
                OperatorOfFacility = "NJAW",
                Location = "Pequest Rd",
                Town = GetEntityFactory<Town>().Create(),
                DateOfProbableViolation = DateTime.Now,
                MarkoutPerformedBy = "David",
                RootCause = "David",
                Contest = false,
                FineAmount = (decimal?)1000,
                WorkOrder = GetEntityFactory<WorkOrder>().Create(),
                Coordinate = GetEntityFactory<Coordinate>().Create(),
            });
            var search = new SearchMarkoutViolation();
            
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity0.MarkoutViolationStatus, "MarkoutViolationStatus");
                helper.AreEqual(entity0.OperatingCenter, "OperatingCenter");
                helper.AreEqual(entity0.Violation, "Violation");
                helper.AreEqual(entity0.DateOfViolationNotice, "DateOfViolationNotice");
                helper.AreEqual(entity0.OCNumber, "Case Number");
                helper.AreEqual(entity0.OperatorOfFacility, "OperatorOfFacility");
                helper.AreEqual(entity0.Location, "Location");
                helper.AreEqual(entity0.Town, "Town");
                helper.AreEqual(entity0.DateOfProbableViolation, "DateOfProbableViolation");
                helper.AreEqual(entity0.MarkoutPerformedBy, "MarkoutPerformedBy");
                helper.AreEqual(entity0.RootCause, "RootCause");
                helper.AreEqual(entity0.Contest, "Contest");
                helper.AreEqual(entity0.FineAmount, "FineAmount");
                helper.AreEqual(entity0.WorkOrder, "WorkOrder");
                helper.AreEqual(entity0.Coordinate, "Coordinate");

                helper.DoesNotContainColumn("Icon");
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestNewSetsPropertiesFromWorkOrderID()
        {
            var wo = GetEntityFactory<WorkOrder>().Create();
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, wo.OperatingCenter, _currentUser, RoleActions.Add);
            var mo = GetEntityFactory<Markout>().Create(new {WorkOrder = wo});

            var result = (CreateMarkoutViolation)((ViewResult)_target.New(wo.Id)).Model;

            Assert.AreEqual(wo.Id, result.WorkOrder);
            Assert.AreEqual(wo.OperatingCenter.Id, result.OperatingCenter);
            Assert.AreEqual(wo.Town.Id, result.Town);
            Assert.AreEqual(wo.StreetAddress, result.Location);
            MyAssert.AreClose(DateTime.Now, result.DateOfViolationNotice.Value);
            Assert.AreEqual(mo.MarkoutNumber, result.MarkoutRequestNumber); 
        }

        #endregion
    }
}
