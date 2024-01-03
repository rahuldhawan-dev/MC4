using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class CompanyLaborCostControllerTest : MapCallMvcControllerTestBase<CompanyLaborCostController,CompanyLaborCost>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesEstimatingProjects;
                a.RequiresRole("~/CompanyLaborCost/Show", module);
                a.RequiresRole("~/CompanyLaborCost/Index", module);
                a.RequiresRole("~/CompanyLaborCost/Search", module);
                a.RequiresRole("~/CompanyLaborCost/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/CompanyLaborCost/Update", module, RoleActions.Edit);
                a.RequiresRole("~/CompanyLaborCost/New", module, RoleActions.Add);
                a.RequiresRole("~/CompanyLaborCost/Create", module, RoleActions.Add);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<CompanyLaborCost>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<CompanyLaborCost>().Create(new {Description = "description 1"});
            var search = new SearchCompanyLaborCost();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<CompanyLaborCost>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditCompanyLaborCost, CompanyLaborCost>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<CompanyLaborCost>(eq.Id).Description);
        }

        #endregion
    }
}
