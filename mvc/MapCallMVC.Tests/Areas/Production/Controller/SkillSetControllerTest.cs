using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class SkillSetControllerTest : MapCallMvcControllerTestBase<SkillSetController, SkillSet>
    {
        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionDataAdministration;
            Authorization.Assert(auth => {
                auth.RequiresRole("~/Production/SkillSet/Show", role);
                auth.RequiresRole("~/Production/SkillSet/Search", role);
                auth.RequiresRole("~/Production/SkillSet/Index", role);
                auth.RequiresRole("~/Production/SkillSet/New", role, RoleActions.Add);
                auth.RequiresRole("~/Production/SkillSet/Create", role, RoleActions.Add);
                auth.RequiresRole("~/Production/SkillSet/Edit", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/SkillSet/Update", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/SkillSet/Destroy", role, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<SkillSet>().Create(new { Name = "Test 1" });
            var entity1 = GetEntityFactory<SkillSet>().Create(new { Name = "Test 2" });
            var search = new SearchSkillSet();

            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
            }
        }

        #endregion

        #endregion
    }
}