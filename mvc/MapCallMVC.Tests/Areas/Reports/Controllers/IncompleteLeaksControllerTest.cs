using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Areas.Reports.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class IncompleteLeaksControllerTest : MapCallMvcControllerTestBase<IncompleteLeaksController, WorkOrder, WorkOrderRepository>
    {
        private WorkDescription _workDescription1;

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _workDescription1 = GetEntityFactory<WorkDescription>().Create(new { Id = (int)WorkDescription.Indices.VALVE_LEAKING });
        }

        #endregion
        
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/IncompleteLeaks/Search", role);
                a.RequiresRole("~/Reports/IncompleteLeaks/Index", role);
            });
        }

        //[TestMethod]
        //public override void TestIndexReturnsResults()
        //{
        //    var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
        //    var workOrder = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = operatingCenter, WorkDescription = _workDescription1 });
        //    GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, operatingCenter, _currentUser, RoleActions.UserAdministrator);

        //    var search = new SearchIncompleteLeaks {
        //        OperatingCenter = operatingCenter.Id
        //    };

        //    var result = _target.Index(search) as ViewResult;
        //    var resultModel = ((SearchIncompleteLeaks)result.Model).Results.ToList();

        //    MvcAssert.IsViewNamed(result, "Index");
        //    Assert.AreEqual(1, resultModel.Count);
        //    Assert.AreSame(workOrder, resultModel[0]);
        //}
    }
}