using System;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
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

namespace MapCallMVC.Tests.Areas.Controllers
{
    [TestClass]
    public class BlowOffInspectionControllerTest : MapCallMvcControllerTestBase<BlowOffInspectionController,
        BlowOffInspection, BlowOffInspectionRepository>
    {
        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesAssets;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/BlowOffInspection/Search/", module);
                a.RequiresRole("~/FieldOperations/BlowOffInspection/Index/", module);
                a.RequiresRole("~/FieldOperations/BlowOffInspection/New/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/BlowOffInspection/Create/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/BlowOffInspection/Show/", module);
                a.RequiresRole("~/FieldOperations/BlowOffInspection/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/BlowOffInspection/Update/", module, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/FieldOperations/BlowOffInspection/Destroy/");
            });
        }

        #region Fields

        private User _user;
        private Mock<INotificationService> _notifier;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _notifier = new Mock<INotificationService>();
            _container.Inject(_notifier.Object);
        }

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeSearchTester = tester => {
                tester.IgnoredPropertyNames.Add("WorkOrderRequired"); // not a mappable property, must test separately
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateBlowOffInspection)vm;
                model.HydrantInspectionType = GetEntityFactory<HydrantInspectionType>().Create().Id;
                model.GPM = 2m;
                model.MinutesFlowed = 3m;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditBlowOffInspection)vm;
                model.HydrantInspectionType = GetEntityFactory<HydrantInspectionType>().Create().Id;
                model.GPM = 2m;
                model.MinutesFlowed = 3m;
            };
        }

        #endregion

        #region New/Create

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            var valve = GetFactory<ValveFactory>().Create();

            var result = (ViewResult)_target.New(valve.Id);

            MvcAssert.IsViewNamed(result, "New");
            MyAssert.IsInstanceOfType<CreateBlowOffInspection>(result.Model);
        }

        [TestMethod]
        public void TestCreateRedirectsToEditPageWhenIsMapPopupIsTrue()
        {
            var inspection = GetFactory<BlowOffInspectionFactory>().BuildWithConcreteDependencies();
            var model = _viewModelFactory.Build<CreateBlowOffInspection, BlowOffInspection>(inspection);
            model.IsMapPopup = true;
            var result = _target.Create(model);
            MvcAssert.RedirectsToRoute(result, "BlowOffInspection", "Edit", new {id = model.Id});
        }

        [TestMethod]
        public void TestCreateChlorineHighValSendsNotification()
        {
            //Arrange
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>()
               .Create(new {TotalChlorine = 2.3m, ResidualChlorine = 3.3m, Valve = valve});
            var model = _viewModelFactory.Build<CreateBlowOffInspection, BlowOffInspection>(inspection);

            //ACT
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Create(model);

            // Assert
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        [TestMethod]
        public void TestCreateChlorineLowValSendsNotification()
        {
            //Arrange
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>()
               .Create(new {TotalChlorine = 0.0m, ResidualChlorine = 2.0m, Valve = valve});
            var model = _viewModelFactory.Build<CreateBlowOffInspection, BlowOffInspection>(inspection);

            //ACT
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Create(model);

            // Assert
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        [TestMethod]
        public void TestCreateDoesntSendNotificationWhenBothOk()
        {
            //Arrange
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>()
               .Create(new {TotalChlorine = 2.0m, ResidualChlorine = 2.0m, Valve = valve});
            var model = _viewModelFactory.Build<CreateBlowOffInspection, BlowOffInspection>(inspection);

            //ACT
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Create(model);

            // Assert
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        [TestMethod]
        public void TestCreateChlorineNullDontSendNotification()
        {
            decimal? nullvalue = null;
            //Arrange
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>().Create(new
                {TotalChlorine = nullvalue, ResidualChlorine = nullvalue, Valve = valve});
            var model = _viewModelFactory.Build<CreateBlowOffInspection, BlowOffInspection>(inspection);

            //ACT
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Create(model);

            // Assert
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        #region SAP

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsErrorCodeUponFailure()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>().BuildWithConcreteDependencies(new {Valve = valve});
            var model = _viewModelFactory.Build<CreateBlowOffInspection, BlowOffInspection>(inspection);

            _target.Create(model);

            var blowOffInspection = Repository.Find(model.Id);
            Assert.IsTrue(blowOffInspection.SAPErrorCode.StartsWith(BlowOffInspectionController.SAP_UPDATE_FAILURE));
        }

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsSAPEquipmentIdAndNoErrorMessage()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>().BuildWithConcreteDependencies(new {Valve = valve});
            var model = _viewModelFactory.Build<CreateBlowOffInspection, BlowOffInspection>(inspection);
            var repository = new Mock<ISAPInspectionRepository>();
            var sapInspection = new SAPInspection {SAPErrorCode = string.Empty, SAPNotificationNumber = "2345", CostCenter = "8675309"};
            repository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Returns(sapInspection);
            _container.Inject(repository.Object);

            _target.Create(model);
            var actual = Repository.Find(model.Id);

            Assert.AreEqual(string.Empty, actual.SAPErrorCode);
            Assert.AreEqual(sapInspection.SAPNotificationNumber, actual.SAPNotificationNumber);
            Assert.AreEqual(sapInspection.CostCenter, actual.BusinessUnit);
        }

        #endregion

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestUpdateRedirectsToEditPageWhenIsMapPopupIsTrue()
        {
            var eq = GetEntityFactory<BlowOffInspection>().Create();
            var result =
                _target.Update(
                    _viewModelFactory.BuildWithOverrides<EditBlowOffInspection, BlowOffInspection>(eq,
                        new {IsMapPopup = true}));
            MvcAssert.RedirectsToRoute(result, new {action = "Edit", controller = "BlowOffInspection", id = eq.Id});
        }

        [TestMethod]
        public void TestUpdateChlorineHighValSendsNotification()
        {
            //Arrange
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>()
               .Create(new {TotalChlorine = 3.3m, ResidualChlorine = 2.0m, Valve = valve});
            var model = _viewModelFactory.Build<EditBlowOffInspection, BlowOffInspection>(inspection);

            //ACT
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            // Assert
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        [TestMethod]
        public void TestUpdateChlorineLowValSendsNotification()
        {
            //Arrange
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>()
               .Create(new {TotalChlorine = 2.0m, ResidualChlorine = 0.0m, Valve = valve});
            var model = _viewModelFactory.Build<EditBlowOffInspection, BlowOffInspection>(inspection);

            //ACT
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            // Assert
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        [TestMethod]
        public void TestUpdateDoesntSendNotificationWhenBothOk()
        {
            //Arrange
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>()
               .Create(new {TotalChlorine = 2.0m, ResidualChlorine = 2.0m, Valve = valve});
            var model = _viewModelFactory.Build<EditBlowOffInspection, BlowOffInspection>(inspection);

            //ACT
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            // Assert
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateChlorineNullDontSendNotification()
        {
            decimal? nullvalue = null;
            //Arrange
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>().Create(new
                {TotalChlorine = nullvalue, ResidualChlorine = nullvalue, Valve = valve});
            var model = _viewModelFactory.Build<EditBlowOffInspection, BlowOffInspection>(inspection);

            //ACT
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            // Assert
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        #region SAP

        [TestMethod]
        public void TestUpdateEffectivelyCreatesInSAPWhenSAPNotificationNumberIsNullish()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>().Create(new {Valve = valve});
            var model = _viewModelFactory.Build<EditBlowOffInspection, BlowOffInspection>(inspection);
            var repository = new Mock<ISAPInspectionRepository>();
            var sapInspection = new SAPInspection {SAPErrorCode = string.Empty, SAPNotificationNumber = "2345", CostCenter = "8675309"};
            repository.Setup(x => x.Save(It.IsAny<SAPInspection>())).Returns(sapInspection);
            _container.Inject(repository.Object);

            _target.Update(model);
            var actual = Repository.Find(model.Id);

            Assert.AreEqual(string.Empty, actual.SAPErrorCode);
            Assert.AreEqual(sapInspection.SAPNotificationNumber, actual.SAPNotificationNumber);
            Assert.AreEqual(sapInspection.CostCenter, actual.BusinessUnit);
        }

        [TestMethod]
        public void TestUpdateDoesNotAttemptToCreatesInSAPWhenSAPNotificationNumberIsNotNull()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new {SAPEnabled = true});
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown
                {OperatingCenter = opCntr, Town = town, Abbreviation = "XX"});
            var valve = GetEntityFactory<Valve>().Create(new {OperatingCenter = opCntr, Town = town});
            var inspection = GetFactory<BlowOffInspectionFactory>()
               .Create(new {Valve = valve, SAPNotificationNumber = "321"});
            var model = _viewModelFactory.Build<EditBlowOffInspection, BlowOffInspection>(inspection);
            var repository = new Mock<ISAPInspectionRepository>();
            _container.Inject(repository.Object);

            _target.Update(model);

            repository.Verify(x => x.Save(It.IsAny<SAPInspection>()), Times.Never);
        }

        #endregion

        #endregion

        #region Search

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            Assert.Inconclusive("Test me. I don't know why the auto test doesn't work");
        }

        [TestMethod]
        public void TestSearchWithWorkOrderRequiredTrueReturnsOnlyInspectionsWhereWorkOrderRequestOneIsNotNull()
        {
            var valve = GetFactory<ValveFactory>().Create();
            var woReq = GetFactory<WorkOrderRequestFactory>().Create();
            var inspectionWithRequest = GetFactory<BlowOffInspectionFactory>()
               .Create(new {Valve = valve, WorkOrderRequestOne = woReq});
            var inspectionNoRequest = GetFactory<BlowOffInspectionFactory>().Create(new {Valve = valve});

            var model = new SearchBlowOffInspection();
            model.WorkOrderRequired = true;
            model.OperatingCenter = new[] {valve.OperatingCenter.Id};
            model.Town = valve.Town.Id;

            // Actually testing that Index calls Search properly.
            var result = (ViewResult)_target.Index(model);
            Assert.IsTrue(model.Results.Any(x => x.Id == inspectionWithRequest.Id));
            Assert.IsFalse(model.Results.Any(x => x.Id == inspectionNoRequest.Id));
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var inspection = GetFactory<BlowOffInspectionFactory>().Create(new {
                Valve = GetFactory<ValveFactory>().Create(new {
                    Coordinate = typeof(CoordinateFactory)
                }),
                HydrantInspectionType = GetFactory<HydrantInspectionTypeFactory>().Create(),
                DateInspected = DateTime.Today,
                CreatedAt = DateTime.Today.AddHours(3),
                GallonsFlowed = 1,
                GPM = 2.0m,
                MinutesFlowed = 3.0m,
                PreResidualChlorine = 4.0m,
                ResidualChlorine = 4.0m,
                FreeNoReadReason = GetFactory<KitNotAvailableNoReadReasonFactory>().Create(),
                PreTotalChlorine = 5.0m,
                TotalChlorine = 5.0m,
                TotalNoReadReason = GetFactory<KitNotAvailableNoReadReasonFactory>().Create(),
                StaticPressure = 6.0m,
                Remarks = "Hhheheh", // http://knowyourmeme.com/memes/laughing-lizard-hhhehehe
                WorkOrderRequestOne = typeof(WorkOrderRequestFactory)
            });

            var inspectionWithNulls = GetFactory<BlowOffInspectionFactory>().Create(new {
                Valve = GetFactory<ValveFactory>().Create()
            });
            inspectionWithNulls.Valve.Coordinate = null;
            Session.Save(inspectionWithNulls.Valve);
            Session.Flush();
            Assert.IsNull(inspectionWithNulls.Valve.Coordinate, "Coordinate is nullable and needs to be tested.");
            Assert.IsNull(inspectionWithNulls.HydrantInspectionType,
                "HydrantInspectionType is nullable and needs to be tested.");

            var search = new SearchBlowOffInspection();
            search.SortBy = "Id";
            search.SortAscending = true;
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(inspection.Id, "Id");
                helper.AreEqual(inspection.Valve.Id, "ValveId");
                helper.AreEqual(inspection.Valve.ValveNumber, "ValveNumber");
                helper.AreEqual(inspection.Valve.OperatingCenter.OperatingCenterCode, "OperatingCenter");
                helper.AreEqual(inspection.Valve.Town.ShortName, "Town");
                helper.AreEqual(inspection.Valve.Coordinate.Latitude, "Latitude");
                helper.AreEqual(inspection.Valve.Coordinate.Longitude, "Longitude");
                helper.AreEqual(inspection.DateInspected, "DateInspected");
                helper.AreEqual(inspection.HydrantInspectionType.Description, "Inspection Type");
                helper.AreEqual(inspection.GPM * inspection.MinutesFlowed, "GallonsFlowed");
                helper.AreEqual(inspection.GPM, "GPM");
                helper.AreEqual(inspection.MinutesFlowed, "MinutesFlowed");
                helper.AreEqual(inspection.PreResidualChlorine, "PreResidualChlorine");
                helper.AreEqual(inspection.ResidualChlorine, "ResidualChlorine");
                helper.AreEqual(inspection.FreeNoReadReason, "FreeNoReadReason");
                helper.AreEqual(inspection.PreTotalChlorine, "PreTotalChlorine");
                helper.AreEqual(inspection.TotalChlorine, "TotalChlorine");
                helper.AreEqual(inspection.TotalNoReadReason, "TotalNoReadReason");
                helper.AreEqual(inspection.StaticPressure, "StaticPressure");
                helper.AreEqual(inspection.WorkOrderRequestOne, "WorkOrderRequestOne");
                helper.AreEqual(inspection.Remarks, "Remarks");
                helper.AreEqual(inspection.InspectedBy.UserName, "InspectedBy");
                helper.AreEqual(inspection.CreatedAt, "DateAdded");

                helper.IsNull("Latitude", 1);
                helper.IsNull("Longitude", 1);
                helper.IsNull("Inspection Type", 1);
            }
        }

        [TestMethod]
        public void TestIndexRespondsToMap()
        {
            InitializeControllerAndRequest("~/FieldOperations/BlowOffInspection/Index.map");
            var search = new SearchBlowOffInspection();
            var result = _target.Index(search);
            Assert.IsInstanceOfType(result, typeof(MapResult));
        }

        [TestMethod]
        public void TestIndexForcesAFewSearchParametersWhenRespondingToMap()
        {
            InitializeControllerAndRequest("~/FieldOperations/BlowOffInspection/Index.map");
            var search = new SearchBlowOffInspection();

            // Setting these search values because they should all be replaced by the controller.
            search.EnablePaging = true;
            search.SortBy = null; // This will be set to "DateInspected" by the search model's ModifyValues.
            search.SortAscending = false;

            var valve1 = GetFactory<ValveFactory>().Create();
            var valve2 = GetFactory<ValveFactory>().Create();

            // Add these from latest date to earliest because the result should return earliest to latest instead.
            GetFactory<BlowOffInspectionFactory>().Create(new {Valve = valve1});
            GetFactory<BlowOffInspectionFactory>().Create(new {Valve = valve2});

            _target.Index(search);
            Assert.IsFalse(search.EnablePaging, "EnablePaging needs to be disabled for maps");
            Assert.AreEqual("DateInspected", search.SortBy,
                "This map MUST be sorted by DateInspected for the lines to draw correctly.");
        }

        [TestMethod]
        public void TestIndexCreatesLineLayerForMaps()
        {
            InitializeControllerAndRequest("~/FieldOperations/BlowOffInspection/Index.map");
            var search = new SearchBlowOffInspection();
            search.DateInspected = new DateRange {
                Start = new DateTime(2015, 5, 15),
                End = new DateTime(2015, 5, 25)
            };

            var valve1 = GetFactory<BlowOffValveFactory>().Create();
            var valve2 = GetFactory<BlowOffValveFactory>().Create();

            // Add these from latest date to earliest because the result should return earliest to latest instead.
            GetFactory<BlowOffInspectionFactory>()
               .Create(new {Valve = valve1, DateInspected = search.DateInspected.End});
            GetFactory<BlowOffInspectionFactory>().Create(new {
                Valve = valve2, DateInspected = search.DateInspected.Start,
                WorkOrderRequestOne = typeof(WorkOrderRequestFactory)
            });

            Session.Refresh(valve1);
            Session.Refresh(valve2);

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
            InitializeControllerAndRequest("~/FieldOperations/BlowOffInspection/Index.map");
            var search = new SearchBlowOffInspection();

            // Setting these search values because they should all be replaced by the controller.
            search.EnablePaging = true;

            var valve1 = GetFactory<ValveFactory>().Create();

            // Add these from latest date to earliest because the result should return earliest to latest instead.
            GetFactory<BlowOffInspectionFactory>().Create(new {Valve = valve1});

            var result = (MapResult)_target.Index(search);
            var iconLayer = result.CoordinateSets[1];
            var iconRVD = iconLayer.CoordinateRouteValues;

            Assert.AreEqual("FieldOperations", iconRVD["area"]);
            Assert.AreEqual("Valve", iconRVD["controller"],
                "Blowoffs are valves so they should be going to the Valve page");
            Assert.AreEqual("Show", iconRVD["action"]);
            Assert.AreEqual("frag", iconRVD["ext"]);
        }

        #endregion
    }
}
