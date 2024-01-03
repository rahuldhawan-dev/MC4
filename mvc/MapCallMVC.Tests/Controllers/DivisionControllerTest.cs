using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Results;
using MMSINC.Testing;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class DivisionControllerTest : MapCallMvcControllerTestBase<DivisionController, Division>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDivisionRepository>().Use<DivisionRepository>();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.HumanResourcesUnion;
                a.RequiresRole("~/Division/Show", module);
                a.RequiresRole("~/Division/Index", module);
                a.RequiresRole("~/Division/Search", module);
                a.RequiresRole("~/Division/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/Division/Update", module, RoleActions.Edit);
                a.RequiresRole("~/Division/Create", module, RoleActions.Add);
                a.RequiresRole("~/Division/New", module, RoleActions.Add);
                a.RequiresRole("~/Division/Destroy", module, RoleActions.Delete);

                a.RequiresLoggedInUserOnly("~/Division/ByStateId/");
            });
        }

        [TestMethod]
        public void TestByStateIdReturnsCascadingActionResult()
        {
            var nj = GetEntityFactory<State>().Create(new {Abbreviation = "NJ"});
            var ny = GetEntityFactory<State>().Create(new {Abbreviation = "NY"});
            var divisionValid = GetEntityFactory<Division>().Create(new {State = nj});
            var divisionInvalid = GetEntityFactory<Division>().Create(new { State = ny });

            var results = (CascadingActionResult)_target.ByStateId(nj.Id);
            var data = results.GetSelectListItems().ToArray();

            Assert.AreEqual(2, data.Count());
            Assert.AreEqual(divisionValid.Description, data.Last().Text);
            Assert.AreEqual(divisionValid.Id.ToString(), data.Last().Value);
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<Division>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<Division>().Create(new {Description = "description 1"});
            var search = new SearchDivision();
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
            var eq = GetEntityFactory<Division>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditDivision, Division>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<Division>(eq.Id).Description);
        }

        #endregion
    }
}
