using System.Linq;
using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Results;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class NPDESRegulatorInspectionControllerTest : MapCallMvcControllerTestBase<NPDESRegulatorInspectionController, NpdesRegulatorInspection>
    {
        #region Private Members

        private User _user;

        #endregion

        #region Private Methods

        #region Init/Cleanup

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = (tester) => {
                tester.TestPropertyValues[nameof(SewerOpening)] = GetEntityFactory<SewerOpening>().Create().Id;
                tester.TestPropertyValues[nameof(SewerOpening.BodyOfWater)] = GetEntityFactory<BodyOfWater>().Create().Id;
                tester.TestPropertyValues[nameof(SewerOpening.OutfallNumber)] = "003";
                tester.TestPropertyValues[nameof(SewerOpening.LocationDescription)] = "Describe this";
            };
        }

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        #endregion

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            var inspection = GetEntityFactory<NpdesRegulatorInspection>().Create(new { DepartureDateTime = DateTime.Now });
            var inspection2 = GetEntityFactory<NpdesRegulatorInspection>().Create(new { DepartureDateTime = DateTime.Now.AddMinutes(-1) });
            var search = new SearchNpdesRegulatorInspection();
            _target.ControllerContext = new ControllerContext();

            var result = (ViewResult)_target.Index(search);
            var resultModel = ((SearchNpdesRegulatorInspection)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreEqual(inspection.Id, resultModel[0].Id);
            Assert.AreEqual(inspection2.Id, resultModel[1].Id);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var date = DateTime.Today;
            var entity0 = GetEntityFactory<NpdesRegulatorInspection>().Create(new { Remarks = "I should be the first inspection", DepartureDateTime = date.AddDays(1) });
            var entity1 = GetEntityFactory<NpdesRegulatorInspection>().Create(new { Remarks = "I should be the second inspection", DepartureDateTime = date });
            var search = new SearchNpdesRegulatorInspection();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(search);

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.SewerOpening, "SewerOpening");
                helper.AreEqual(entity1.SewerOpening, "SewerOpening", 1);

                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
            }
        }

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to parameter on New action.
            var inspection = GetFactory<NpdesRegulatorInspectionFactory>().Create();
            var result = (ViewResult)_target.New(inspection.Id);
            MvcAssert.IsViewNamed(result, "New");

            MyAssert.IsInstanceOfType<CreateNpdesRegulatorInspection>(result.Model);
        }

        [TestMethod]
        public void TestNewReturns404IfRegulatorDoesNotExist()
        {
            MvcAssert.IsNotFound(_target.New(0));
        }

        #endregion

        [TestMethod]
        public void TestCreateRedirectsToTheInspectionShowPageAfterSuccessfullySaving()
        {
            var sewerOpening = GetEntityFactory<SewerOpening>().Create();
            var viewModel = _viewModelFactory.BuildWithOverrides<CreateNpdesRegulatorInspection>(new {
                SewerOpening = sewerOpening.Id,
                DepartureDateTime = _now
            });

            var result = _target.Create(viewModel);

            MvcAssert.RedirectsToRoute(
                result,
                "NPDESRegulatorInspection",
                "Show",
                new { sewerOpening.Id });
        }

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<NpdesRegulatorInspection>().Create();
            var expected = "description field";

            _target.Update(_viewModelFactory.BuildWithOverrides<EditNpdesRegulatorInspection, NpdesRegulatorInspection>(eq, new {
                Remarks = expected
            }));

            Assert.AreEqual(expected, Session.Get<NpdesRegulatorInspection>(eq.Id).Remarks);
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = NPDESRegulatorInspectionController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/NPDESRegulatorInspection/Show/", role);
                a.RequiresRole("~/FieldOperations/NPDESRegulatorInspection/Search/", role);
                a.RequiresRole("~/FieldOperations/NPDESRegulatorInspection/Index/", role);
                a.RequiresRole("~/FieldOperations/NPDESRegulatorInspection/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/NPDESRegulatorInspection/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/NPDESRegulatorInspection/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/NPDESRegulatorInspection/Update/", role, RoleActions.Edit);
            });
        }

        #endregion
    }
}
