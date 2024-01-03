using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class ValveInspectionControllerTest : MapCallMvcControllerTestBase<ValveInspectionController, ValveInspection, ValveInspectionRepository>
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

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateValveInspection)vm;
                model.Turns = 3;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditValveInspection)vm;
                model.Turns = 3;
            };
        }

        #endregion

        #region Authorization

        [TestMethod]		
        public override void TestControllerAuthorization()
        {
            var role = ValveInspectionController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/ValveInspection/Search/", role);
                a.RequiresRole("~/FieldOperations/ValveInspection/Show/", role);
                a.RequiresRole("~/FieldOperations/ValveInspection/Index/", role);
                a.RequiresRole("~/FieldOperations/ValveInspection/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ValveInspection/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/ValveInspection/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/ValveInspection/Update/", role, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/FieldOperations/ValveInspection/Destroy/");
			});
		}

        #endregion

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because search returns view model rather than entity.
            var eq1 = GetEntityFactory<ValveInspection>().Create(new { DateInspected = DateTime.Now });
            var eq2 = GetEntityFactory<ValveInspection>().Create(new { DateInspected = DateTime.Now.AddMinutes(-1) });
            var search = new SearchValveInspection();
            _target.ControllerContext = new ControllerContext();

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchValveInspection)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreEqual(eq1.Id, resultModel[0].Id);
            Assert.AreEqual(eq2.Id, resultModel[1].Id);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var date = DateTime.Today;
            var entity0 = GetEntityFactory<ValveInspection>().Create(new {Remarks = "I should be the first inspection", DateInspected = date.AddDays(1), Inspected = true});
            var entity1 = GetEntityFactory<ValveInspection>().Create(new {Remarks = "I should be the second inspection", DateInspected = date, Inspected = true });
            var search = new SearchValveInspection();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                // If the Remarks check fails, then it's possible the entities are getting 
                // queried out of order somehow. Possible sqlite has a similar thing with SQLite
                // where it will optimize specific tables to output in a random order based on something.
                helper.AreEqual(entity0.Remarks, "Remarks");
                helper.AreEqual(entity1.Remarks, "Remarks", 1);

                // If this outright fails, though, then that probably means that somewhere there's still
                // a ValveInspection lingering with Id=1 that this test did not create.
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);

                helper.AreEqual(entity0.Inspected, "Inspected");
                helper.AreEqual(entity1.Inspected, "Inspected", 1);
            }
        }

        [TestMethod]
        public void TestIndexRespondsToMap()
        {
            InitializeControllerAndRequest("~/FieldOperations/ValveInspection/Index.map");
            var search = new SearchValveInspection();
            var result = _target.Index(search);
            Assert.IsInstanceOfType(result, typeof(MapResult));
        }

        [TestMethod]
        public void TestIndexForcesAFewSearchParametersWhenRespondingToMap()
        {
            InitializeControllerAndRequest("~/FieldOperations/ValveInspection/Index.map");
            var search = new SearchValveInspection();

            // Setting these search values because they should all be replaced by the controller.
            search.EnablePaging = true;
            search.SortBy = "This will throw if it isn't changed";
            search.SortAscending = false;

            var valve1 = GetFactory<ValveFactory>().Create();
            var valve2 = GetFactory<ValveFactory>().Create();

            // Add these from latest date to earliest because the result should return earliest to latest instead.
            GetFactory<ValveInspectionFactory>().Create(new { Valve = valve1 });
            GetFactory<ValveInspectionFactory>().Create(new { Valve = valve2 });

            _target.Index(search);
            Assert.IsFalse(search.EnablePaging, "EnablePaging needs to be disabled for maps");
            Assert.AreEqual("DateInspected", search.SortBy, "This map MUST be sorted by DateInspected for the lines to draw correctly.");
            Assert.IsTrue(search.SortAscending, "Must sort by earliest to latest");
        }

        [TestMethod]
        public void TestIndexCreatesLineLayerForMaps()
        {
            InitializeControllerAndRequest("~/FieldOperations/ValveInspection/Index.map");
            var search = new SearchValveInspection();
            search.DateInspected = new DateRange
            {
                Start = new DateTime(2015, 5, 15),
                End = new DateTime(2015, 5, 25)
            };

            // Setting these search values because they should all be replaced by the controller.
            search.EnablePaging = true;
            search.SortBy = "This will throw if it isn't changed";
            search.SortAscending = false;

            var valve1 = GetFactory<ValveFactory>().Create();
            var valve2 = GetFactory<ValveFactory>().Create();

            // Add these from latest date to earliest because the result should return earliest to latest instead.
            GetFactory<ValveInspectionFactory>().Create(new { Valve = valve1, DateInspected = search.DateInspected.End });
            GetFactory<ValveInspectionFactory>().Create(new { Valve = valve2, DateInspected = search.DateInspected.Start, WorkOrderRequestOne = typeof(ValveWorkOrderRequestFactory) });

            var result = (MapResult)_target.Index(search);
            var lineLayer = result.CoordinateSets.First();
            Assert.AreEqual("lineLayer", lineLayer.LayerId, "The first layer MUST be the line layer.");
            var lineCoords = lineLayer.Coordinates.ToArray();
            Assert.AreEqual(2, lineCoords.Count(), "The line layer MUST include all the coordinates!");
            Assert.AreEqual(valve2.Id, lineCoords[0].Id, "The earliest inspection must have its coordinate first.");
            Assert.AreEqual(valve1.Id, lineCoords[1].Id, "The latest inspection must have its coordinate last.");
        }

        [TestMethod]
        public void TestIndexSetsCoordinateRouteValuesToGoToValveShowPageWhenRespondingToMap()
        {
            InitializeControllerAndRequest("~/FieldOperations/ValveInspection/Index.map");
            var search = new SearchValveInspection();

            // Setting these search values because they should all be replaced by the controller.
            search.EnablePaging = true;
            search.SortBy = "This will throw if it isn't changed";
            search.SortAscending = false;

            var valve1 = GetFactory<ValveFactory>().Create();

            // Add these from latest date to earliest because the result should return earliest to latest instead.
            GetFactory<ValveInspectionFactory>().Create(new { Valve = valve1 });

            var result = (MapResult)_target.Index(search);
            var iconLayer = result.CoordinateSets[1];
            var iconRVD = iconLayer.CoordinateRouteValues;

            Assert.AreEqual("FieldOperations", iconRVD["area"]);
            Assert.AreEqual("Valve", iconRVD["controller"]);
            Assert.AreEqual("Show", iconRVD["action"]);
            Assert.AreEqual("frag", iconRVD["ext"]);
        }

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // overrided needed for New parameter
            var valve = GetEntityFactory<Valve>().Create();
            var result = (ViewResult)_target.New(valve.Id);

            MvcAssert.IsViewNamed(result, "New");
            MyAssert.IsInstanceOfType<CreateValveInspection>(result.Model);
        }

        [TestMethod]
        public void TestNewReturns404IfValveDoesNotExist()
        {
            MvcAssert.IsNotFound(_target.New(0));
        }

        [TestMethod]
        public void TestCreateRedirectsToEditPageWhenIsMapPopupIsTrue()
        {
            var inspection = GetFactory<ValveInspectionFactory>().BuildWithConcreteDependencies();
            var model = _viewModelFactory.Build<CreateValveInspection, ValveInspection>( inspection);
            model.IsMapPopup = true;
            var result = _target.Create(model);
            MvcAssert.RedirectsToRoute(result, "ValveInspection", "Edit", new { id = model.Id });
        }

        #region SAP

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsErrorCodeUponFailure()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var valve = GetEntityFactory<Valve>().Create(new { OperatingCenter = opCntr, Town = town });
            var inspection = GetFactory<ValveInspectionFactory>().BuildWithConcreteDependencies(new { Valve = valve });
            var model = _viewModelFactory.Build<CreateValveInspection, ValveInspection>( inspection);

            _target.Create(model);

            var hydrantInspection = Repository.Find(model.Id);
            Assert.IsTrue(hydrantInspection.SAPErrorCode.StartsWith(ValveInspectionController.SAP_UPDATE_FAILURE));
        }

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsSAPEquipmentIdAndNoErrorMessage()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var valve = GetEntityFactory<Valve>().Create(new { OperatingCenter = opCntr, Town = town });
            var inspection = GetFactory<ValveInspectionFactory>().BuildWithConcreteDependencies(new { Valve = valve });
            var model = _viewModelFactory.Build<CreateValveInspection, ValveInspection>( inspection);
            var repository = new Mock<ISAPInspectionRepository>();
            var sapInspection = new SAPInspection { SAPErrorCode = string.Empty, SAPNotificationNumber = "2345" };
            repository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Returns(sapInspection);
            _container.Inject(repository.Object);

            _target.Create(model);
            var actual = Repository.Find(model.Id);

            Assert.AreEqual(string.Empty, actual.SAPErrorCode);
            Assert.AreEqual(sapInspection.SAPNotificationNumber, actual.SAPNotificationNumber);
        }

        #endregion

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ValveInspection>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditValveInspection, ValveInspection>(eq, new {
                Remarks = expected
            }));

            Assert.AreEqual(expected, Session.Get<ValveInspection>(eq.Id).Remarks);
        }

        #region SAP

        [TestMethod]
        public void TestUpdateEffectivelyCreatesInSAPWhenSAPNotificationNumberIsNullish()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var valve = GetEntityFactory<Valve>().Create(new { OperatingCenter = opCntr, Town = town });
            var inspection = GetFactory<ValveInspectionFactory>().Create(new { Valve = valve });
            var model = _viewModelFactory.Build<EditValveInspection, ValveInspection>( inspection);
            var repository = new Mock<ISAPInspectionRepository>();
            var sapInspection = new SAPInspection { SAPErrorCode = string.Empty, SAPNotificationNumber = "" };
            repository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Returns(sapInspection);
            _container.Inject(repository.Object);

            _target.Update(model);
            var actual = Repository.Find(model.Id);

            Assert.AreEqual(string.Empty, actual.SAPErrorCode);
            Assert.AreEqual(sapInspection.SAPNotificationNumber, actual.SAPNotificationNumber);
        }

        [TestMethod]
        public void TestUpdateDoesNotAttemptToCreatesInSAPWhenSAPNotificationNumberIsNotNull()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var valve = GetEntityFactory<Valve>().Create(new { OperatingCenter = opCntr, Town = town });
            var inspection = GetFactory<ValveInspectionFactory>().Create(new { Valve = valve, SAPNotificationNumber = "1234" });
            var model = _viewModelFactory.Build<EditValveInspection, ValveInspection>( inspection);
            var repository = new Mock<ISAPInspectionRepository>();
            _container.Inject(repository.Object);

            _target.Update(model);

            repository.Verify(x => x.Save(It.IsAny<SAPInspection>()), Times.Never);
        }

        #endregion

        #endregion
	}
}
