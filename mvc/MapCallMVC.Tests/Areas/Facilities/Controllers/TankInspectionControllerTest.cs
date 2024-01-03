using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.Facilities.Controllers;
using MapCallMVC.Areas.Facilities.Models.ViewModels;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class TankInspectionControllerTest : MapCallMvcControllerTestBase<TankInspectionController, TankInspection, IRepository<TankInspection>>
    {
        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);

            // The Questions property needs to be initialized for some of the automatic tests because
            // the property itself is set via SetDefaults. In practice, the Questions property will never
            // be null, and if it is null then something has gone horribly wrong.
            options.InitializeCreateViewModel = (model) => {
                ((CreateTankInspection)model).TankInspectionQuestions = new System.Collections.Generic.List<TankInspectionQuestionViewModel>();
            };
            options.InitializeUpdateViewModel = (model) => {
                ((EditTankInspection)model).TankInspectionQuestions = new System.Collections.Generic.List<TankInspectionQuestionViewModel>();
            };
        }

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var role = TankInspectionController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/Facilities/TankInspection/Search/", role);
                a.RequiresRole("~/Facilities/TankInspection/Show/", role);
                a.RequiresRole("~/Facilities/TankInspection/Index/", role);
                a.RequiresRole("~/Facilities/TankInspection/New/", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/TankInspection/Create/", role, RoleActions.Add);
                a.RequiresRole("~/Facilities/TankInspection/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/Facilities/TankInspection/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/Facilities/TankInspection/Destroy/", role, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<TankInspection>().Create(new { ZipCode = "ZipCode 0" });
            var entity1 = GetEntityFactory<TankInspection>().Create(new { ZipCode = "ZipCode 1" });
            var search = new SearchTankInspection();
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

        #region New

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // noop, other tests needed to cover this.
        }

        [TestMethod]
        public void TestNewLoadsNewFormWithValuesFromOrder()
        {
            _currentUser.IsAdmin = true;
            DateTime thisDate = new DateTime(2021, 1, 1);
            var town = GetEntityFactory<Town>().Create();
            var pub = GetEntityFactory<PublicWaterSupply>().Create();
            var pubpz = GetEntityFactory<PublicWaterSupplyPressureZone>().Create();
            var fac = GetEntityFactory<Facility>().Create(new {
                PublicWaterSupplyPressureZone = pubpz,
                PublicWaterSupply = pub,
                Town = town,
                ZipCode = "900001"
            });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new { Facility = fac});
            var emp = GetEntityFactory<Employee>().Create();
            var employeeAssignment = GetEntityFactory<EmployeeAssignment>().Create(new { AssignedTo = emp, AssignedFor = thisDate, DateStarted = thisDate });
            order.CurrentAssignments.Add(employeeAssignment);

            var result = (ViewResult)_target.New(order.Id);

            MyAssert.IsInstanceOfType<ActionResult>(result);
            MyAssert.IsInstanceOfType<CreateTankInspection>(result.Model);
            MvcAssert.IsViewNamed(result, "New");
            var foo = (CreateTankInspection)result.Model;

            Assert.AreEqual(order.Id, foo.ProductionWorkOrder);
            Assert.AreEqual(order.OperatingCenter.Id, foo.OperatingCenter);
            Assert.AreEqual(order.Facility.Id, foo.Facility);
            Assert.AreEqual(order.Facility.ZipCode, foo.ZipCode.ToString());
            Assert.AreEqual(order.Facility.Town.Id, foo.Town);
            Assert.AreEqual(order.Facility.Coordinate.Id, foo.Coordinate);
            Assert.AreEqual(order.Facility.PublicWaterSupply.Id, foo.PublicWaterSupply);
            Assert.AreEqual(employeeAssignment.DateStarted, foo.ObservationDate);
            Assert.AreEqual(employeeAssignment.AssignedTo.Id, foo.TankObservedBy);
            _currentUser.IsAdmin = false;
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<TankInspection>().Create();
            var expected = "ZipCode field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditTankInspection, TankInspection>(eq, new {
                ZipCode = expected
            }));

            Assert.AreEqual(expected, Session.Get<TankInspection>(eq.Id).ZipCode);
        }

        #endregion
    }
}
