using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class SewerOpeningInspectionControllerTest : MapCallMvcControllerTestBase<SewerOpeningInspectionController, SewerOpeningInspection, SewerOpeningInspectionRepository>
    {
        #region Fields

        private User _user;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = SewerOpeningInspectionController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/SewerOpeningInspection/Search/", role);
                a.RequiresRole("~/FieldOperations/SewerOpeningInspection/Show/", role);
                a.RequiresRole("~/FieldOperations/SewerOpeningInspection/Index/", role);
                a.RequiresRole("~/FieldOperations/SewerOpeningInspection/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/SewerOpeningInspection/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/SewerOpeningInspection/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/SewerOpeningInspection/Update/", role, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/FieldOperations/SewerOpeningInspection/Destroy/");
            });
        }

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var eq1 = GetEntityFactory<SewerOpeningInspection>().Create(new { DateInspected = DateTime.Now });
            var eq2 = GetEntityFactory<SewerOpeningInspection>().Create(new { DateInspected = DateTime.Now.AddMinutes(-1) });
            var search = new SearchSewerOpeningInspection();
            _target.ControllerContext = new ControllerContext();

            var result = (ViewResult)_target.Index(search);
            var resultModel = ((SearchSewerOpeningInspection)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreEqual(eq1.Id, resultModel[0].Id);
            Assert.AreEqual(eq2.Id, resultModel[1].Id);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var date = DateTime.Today;
            var entity0 = GetEntityFactory<SewerOpeningInspection>().Create(new { Remarks = "I should be the first inspection", DateInspected = date.AddDays(1) });
            var entity1 = GetEntityFactory<SewerOpeningInspection>().Create(new { Remarks = "I should be the second inspection", DateInspected = date });
            var search = new SearchSewerOpeningInspection();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Remarks, "Remarks");
                helper.AreEqual(entity1.Remarks, "Remarks", 1);

                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
            }
        }

        #endregion

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // Override due to New parameter
            var sewerOpening = GetEntityFactory<SewerOpening>().Create();
            var result = (ViewResult)_target.New(sewerOpening.Id);

            MvcAssert.IsViewNamed(result, "New");
            MyAssert.IsInstanceOfType<CreateSewerOpeningInspection>(result.Model);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<SewerOpeningInspection>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditSewerOpeningInspection, SewerOpeningInspection>(eq, new
            {
                Remarks = expected
            }));

            Assert.AreEqual(expected, Session.Get<SewerOpeningInspection>(eq.Id).Remarks);
        }

        #endregion
    }
}
