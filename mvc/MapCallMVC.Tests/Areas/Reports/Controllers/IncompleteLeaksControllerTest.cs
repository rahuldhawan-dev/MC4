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
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.FieldServicesWorkManagement;
            Authorization.Assert(a => {
                a.RequiresRole("~/Reports/IncompleteLeaks/Search", role);
                a.RequiresRole("~/Reports/IncompleteLeaks/Index", role);
            });
        }

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            var workOrder = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = operatingCenter });
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, operatingCenter, _currentUser, RoleActions.UserAdministrator);

            var search = new SearchIncompleteLeaks {
                OperatingCenter = operatingCenter.Id
            };

            DependencyResolver.Current.GetService<NHibernateQueryInterface.NHibernateQueryInterface>().ShowWindow();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchIncompleteLeaks)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreSame(workOrder, resultModel[0]);
        }
    }
}