using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Data;
using MMSINC.Testing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class CompletedWorkOrdersControllerTest : MapCallMvcControllerTestBase<CompletedWorkOrdersController, WorkOrder, WorkOrderRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/CompletedWorkOrders/Search", role);
                a.RequiresRole("~/Reports/CompletedWorkOrders/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var workOrder = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = operatingCenter });
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, operatingCenter, _currentUser, RoleActions.UserAdministrator);
            var search = new SearchCompletedWorkOrders {
                OperatingCenter = operatingCenter.Id
            };

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchCompletedWorkOrders)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreSame(workOrder, resultModel[0]);
        }
    }
}