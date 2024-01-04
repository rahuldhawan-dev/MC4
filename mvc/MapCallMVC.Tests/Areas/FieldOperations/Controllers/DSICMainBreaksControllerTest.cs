using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class DSICMainBreaksControllerTest : MapCallMvcControllerTestBase<DSICMainBreaksController, MainBreak, MainBreakRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/DSICMainBreaks/Search", role);
                a.RequiresRole("~/FieldOperations/DSICMainBreaks/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var workOrder = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = operatingCenter });
            var mainBreak = GetFactory<MainBreakFactory>().Create(new { WorkOrder = workOrder });

            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, operatingCenter, _currentUser, RoleActions.UserAdministrator);

            var search = new SearchDSICMainBreaks {
                OperatingCenter = operatingCenter.Id
            };

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchDSICMainBreaks)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreSame(mainBreak, resultModel[0]);
        }
    }
}