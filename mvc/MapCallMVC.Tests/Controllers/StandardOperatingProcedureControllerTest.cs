using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class StandardOperatingProcedureControllerTest : MapCallMvcControllerTestBase<StandardOperatingProcedureController, StandardOperatingProcedure>
    {
        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<StandardOperatingProcedure>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<StandardOperatingProcedure>().Create(new {Description = "description 1"});
            var search = new SearchStandardOperatingProcedure();
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
            var eq = GetEntityFactory<StandardOperatingProcedure>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditStandardOperatingProcedure, StandardOperatingProcedure>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<StandardOperatingProcedure>(eq.Id).Description);
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresRole("~/StandardOperatingProcedure/Search/", StandardOperatingProcedureController.ROLE);
                a.RequiresRole("~/StandardOperatingProcedure/Index/", StandardOperatingProcedureController.ROLE);
                a.RequiresRole("~/StandardOperatingProcedure/Show/", StandardOperatingProcedureController.ROLE);
                a.RequiresRole("~/StandardOperatingProcedure/New/", StandardOperatingProcedureController.ROLE, RoleActions.Add);
                a.RequiresRole("~/StandardOperatingProcedure/Create/", StandardOperatingProcedureController.ROLE, RoleActions.Add);
                a.RequiresRole("~/StandardOperatingProcedure/Edit/", StandardOperatingProcedureController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/StandardOperatingProcedure/Update/", StandardOperatingProcedureController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/StandardOperatingProcedure/AddStandardOperatingProcedureQuestion/", StandardOperatingProcedureController.ROLE, RoleActions.UserAdministrator);
                a.RequiresRole("~/StandardOperatingProcedure/RemoveStandardOperatingProcedureQuestion/", StandardOperatingProcedureController.ROLE, RoleActions.UserAdministrator);
                a.RequiresRole("~/StandardOperatingProcedure/AddStandardOperatingProcedurePositionGroupCommonNameRequirement/", StandardOperatingProcedureController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/StandardOperatingProcedure/RemoveStandardOperatingProcedurePositionGroupCommonNameRequirement/", StandardOperatingProcedureController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/StandardOperatingProcedure/AddTrainingModule/", StandardOperatingProcedureController.ROLE, RoleActions.Edit);
                a.RequiresRole("~/StandardOperatingProcedure/RemoveTrainingModule/", StandardOperatingProcedureController.ROLE, RoleActions.Edit);
            });
        }

        #endregion
    }
}
