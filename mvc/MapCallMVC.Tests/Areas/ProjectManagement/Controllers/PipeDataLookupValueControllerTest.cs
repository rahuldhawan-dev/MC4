using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.ProjectManagement.Controllers;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Controllers
{
    [TestClass]
    public class PipeDataLookupValueControllerTest : MapCallMvcControllerTestBase<PipeDataLookupValueController, PipeDataLookupValue>
    {
        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = PipeDataLookupValueController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/ProjectManagement/PipeDataLookupValue/Show/", role);
                a.RequiresRole("~/ProjectManagement/PipeDataLookupValue/Index/", role);
                a.RequiresRole("~/ProjectManagement/PipeDataLookupValue/Search/", role);
                a.RequiresRole("~/ProjectManagement/PipeDataLookupValue/New/", role, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/PipeDataLookupValue/Create/", role, RoleActions.Add);
                a.RequiresRole("~/ProjectManagement/PipeDataLookupValue/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/ProjectManagement/PipeDataLookupValue/Update/", role, RoleActions.Edit);
			});
		}				

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<PipeDataLookupValue>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<PipeDataLookupValue>().Create(new {Description = "description 1"});
            var search = new SearchPipeDataLookupValue();
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
            var eq = GetEntityFactory<PipeDataLookupValue>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditPipeDataLookupValue, PipeDataLookupValue>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<PipeDataLookupValue>(eq.Id).Description);
        }

        #endregion
	}
}
