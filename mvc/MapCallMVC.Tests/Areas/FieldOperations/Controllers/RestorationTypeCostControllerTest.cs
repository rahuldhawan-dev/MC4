using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class RestorationTypeCostControllerTest : MapCallMvcControllerTestBase<RestorationTypeCostController, RestorationTypeCost>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/RestorationTypeCost/Show", RestorationTypeCostController.ROLE);
                a.RequiresRole("~/FieldOperations/RestorationTypeCost/Search", RestorationTypeCostController.ROLE);
                a.RequiresRole("~/FieldOperations/RestorationTypeCost/Index", RestorationTypeCostController.ROLE);
                a.RequiresRole("~/FieldOperations/RestorationTypeCost/Edit", RestorationTypeCostController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/RestorationTypeCost/Update", RestorationTypeCostController.ROLE, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/FieldOperations/RestorationTypeCost/New");
                a.RequiresSiteAdminUser("~/FieldOperations/RestorationTypeCost/Create");
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<RestorationTypeCost>().Create(new {Cost = 8});
            var entity1 = GetEntityFactory<RestorationTypeCost>().Create(new {Cost = 17});
            var search = new SearchRestorationTypeCost();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Cost, "Cost");
                helper.AreEqual(entity1.Cost, "Cost", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<RestorationTypeCost>().Create();
            double expected = 8;

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditRestorationTypeCost, RestorationTypeCost>(eq, new {
                Cost = expected
            }));

            Assert.AreEqual(expected, Session.Get<RestorationTypeCost>(eq.Id).Cost);
        }

        #endregion

    }
}
