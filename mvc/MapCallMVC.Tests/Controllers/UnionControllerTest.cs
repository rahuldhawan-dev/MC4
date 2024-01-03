using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class UnionControllerTest : MapCallMvcControllerTestBase<UnionController, Union>
    {
        #region Setup/Teardown

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexRedirectsToShowForSingleResult = true;
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a =>
            {
                var module = RoleModules.HumanResourcesUnion;
                a.RequiresRole("~/Union/Search/", module, RoleActions.Read);
                a.RequiresRole("~/Union/Index/", module, RoleActions.Read);
                a.RequiresRole("~/Union/Show/", module, RoleActions.Read);
                a.RequiresRole("~/Union/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/Union/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/Union/New/", module, RoleActions.Add);
                a.RequiresRole("~/Union/Create/", module, RoleActions.Add);
            });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var union = GetFactory<UnionFactory>().Create(new
            {
                BargainingUnit = "some union"
            });

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditUnion, Union>(union, new {
                BargainingUnit = "some other union"
            }));

            Assert.AreEqual("some other union", Repository.Find(union.Id).BargainingUnit);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var ent0 = GetEntityFactory<Union>().Create(new { BargainingUnit = "Foo" });
            var ent1 = GetEntityFactory<Union>().Create(new { BargainingUnit = "Bar" });
            var search = new SearchUnion();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(ent0.Id, "Id");
                helper.AreEqual(ent1.Id, "Id", 1);
                helper.AreEqual(ent0.BargainingUnit, "BargainingUnit");
                helper.AreEqual(ent1.BargainingUnit, "BargainingUnit", 1);
            }
        }

        #endregion
    }
}
