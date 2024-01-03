using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using System.Linq;
using System.Web.Mvc;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class LocalControllerTest : MapCallMvcControllerTestBase<LocalController, Local>
    {
        #region Setup/Teardown

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _authenticationService.SetupGet(x => x.CurrentUser).Returns(_currentUser);
            _target = Request.CreateAndInitializeController<LocalController>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.IndexRedirectsToShowForSingleResult = true;
        }

        #endregion

        #region Roles

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.HumanResourcesUnion;
                a.RequiresRole("~/Local/Search", module, RoleActions.Read);
                a.RequiresRole("~/Local/Show", module, RoleActions.Read);
                a.RequiresRole("~/Local/Index", module, RoleActions.Read);
                a.RequiresRole("~/Local/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/Local/Update", module, RoleActions.Edit);
                a.RequiresRole("~/Local/New", module, RoleActions.Add);
                a.RequiresRole("~/Local/Create", module, RoleActions.Add);

                a.RequiresRole("~/Local/ByOperatingCenterId", module, RoleActions.Read);
                a.RequiresLoggedInUserOnly("~/Local/ByUnionId/");
            });
        }

        #endregion

        #region Search/Index/Show

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            InitializeControllerForRequest("~/Local/Index.map");
            var good = GetFactory<LocalFactory>().Create(new { Union = typeof(UnionFactory) });
            var bad = GetFactory<LocalFactory>().Create(new { Union = typeof(UnionFactory) });
            var model = new SearchLocal
            {
                Union = good.Union.Id
            };
            var result = (MapResult)_target.Index(model);

            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();
            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(good));
            Assert.IsFalse(resultModel.Contains(bad));
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithoutModelsIfModelStateIsNotValid()
        {
            InitializeControllerForRequest("~/Local/Index.map");
            var good = GetFactory<LocalFactory>().Create(new { Union = typeof(UnionFactory) });

            var model = new SearchLocal();
            var validResult = (MapResult)_target.Index(model);

            Assert.AreEqual(1, validResult.CoordinateSets.Single().Coordinates.Count());
            Assert.IsTrue(validResult.CoordinateSets.Single().Coordinates.Contains(good));

            _target.ModelState.AddModelError("error", "error");
            var badResult = (MapResult)_target.Index(model);

            Assert.IsFalse(badResult.CoordinateSets.Any());
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var ent0 = GetEntityFactory<Local>().Create(new { Description = "Foo" });
            var ent1 = GetEntityFactory<Local>().Create(new { Description = "Bar" });
            var search = new SearchLocal();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(ent0.Id, "Id");
                helper.AreEqual(ent1.Id, "Id", 1);
                helper.AreEqual(ent0.Description, "Description");
                helper.AreEqual(ent1.Description, "Description", 1);
            }
        }

        #endregion
    }
}
