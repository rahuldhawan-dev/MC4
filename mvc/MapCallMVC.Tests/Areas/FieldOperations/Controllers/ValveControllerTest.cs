using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MapCall.Common.Configuration;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using StructureMap;
using ValveSize = MapCall.Common.Model.Entities.ValveSize;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class ValveControllerTest : MapCallMvcControllerTestBase<ValveControllerTest.ValveController, Valve, ValveRepository>
    {
        #region Fields

        private User _user;
        private AssetStatus _activeStatus, _retiredStatus, _otherStatus, _removedStatus, _cancelledStatus;
        private Mock<INotificationService> _notifier;
        private Mock<ISAPEquipmentRepository> _sapRepository;

        protected override User CreateUser()
        {
            _user = GetFactory<AdminUserFactory>().Create();
            return _user;
        }

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _notifier = e.For<INotificationService>().Mock();
            _sapRepository = e.For<ISAPEquipmentRepository>().Mock();
            e.For<IRoleService>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            _otherStatus = GetFactory<PendingAssetStatusFactory>().Create();
            _cancelledStatus = GetFactory<CancelledAssetStatusFactory>().Create();
            _removedStatus = GetFactory<RemovedAssetStatusFactory>().Create();

            GetFactory<TownSectionAbbreviationTypeFactory>().Create();
            GetFactory<FireDistrictAbbreviationTypeFactory>().Create();
            GetFactory<ValveAssetTypeFactory>().Create();

            Application.ViewEngine = new MapCallMvcViewEngine();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateValve)vm;
                var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = operatingCenter, Town = town, Abbreviation = "XX" });
                model.OperatingCenter = operatingCenter.Id;
                model.Town = town.Id;
                model.ValveType = GetEntityFactory<ValveType>().Create().Id;
                model.NormalPosition = GetEntityFactory<ValveNormalPosition>().Create().Id;
                model.DateInstalled = DateTime.Now;
                model.Turns = 1;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditValve)vm;
                model.ValveType = GetEntityFactory<ValveType>().Create().Id;
                model.NormalPosition = GetEntityFactory<ValveNormalPosition>().Create().Id;
                model.DateInstalled = DateTime.Now;
                model.Turns = 1;
            };
            options.InitializeSearchTester = (tester) => {
                // Route is an int, but the tester tries to create an entity for it.
                tester.IgnoredPropertyNames.Add("Route");
            };
        }

        #endregion

        #region Usual Controller Tests

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesAssets;
                var path = "~/FieldOperations/Valve/";

                a.RequiresRole($"{path}Search/", module);
                a.RequiresRole($"{path}Show/", module);
                a.RequiresRole($"{path}Index/", module);
                a.RequiresRole($"{path}New/", module, RoleActions.UserAdministrator);
                a.RequiresRole($"{path}Create/", module, RoleActions.UserAdministrator);
                a.RequiresRole($"{path}Edit/", module, RoleActions.Edit);
                a.RequiresRole($"{path}Update/", module, RoleActions.Edit);
                a.RequiresRole($"{path}Replace/", module, RoleActions.UserAdministrator);
                a.RequiresRole($"{path}Copy/", module, RoleActions.UserAdministrator);

                a.RequiresLoggedInUserOnly($"{path}ByStreetId/");
                a.RequiresLoggedInUserOnly($"{path}ByStreetIdForWorkOrders/");
                a.RequiresLoggedInUserOnly($"{path}ByTownId/");
                a.RequiresLoggedInUserOnly($"{path}ByTownIdAndOperatingCenterId/");
                a.RequiresLoggedInUserOnly($"{path}RouteByTownId/");
                a.RequiresLoggedInUserOnly($"{path}RouteByOperatingCenterIdAndOrTownId/");
                a.RequiresLoggedInUserOnly($"{path}ByOperatingCenterAndValveNumber/");
                a.RequiresLoggedInUserOnly($"{path}ValidateUnusedFoundValveSuffix/");
                a.RequiresLoggedInUserOnly($"{path}ByOperatingCenter/");
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowSetsArcCollectorUrlInViewData()
        {
            var entity = GetFactory<ValveFactory>().Create();

            _target.Show(entity.Id);

            MyAssert.StringsAreEqual(
                string.Format(ArcCollectorLinkGenerator.ARCGIS_COLLECTOR_HTML_FORMAT_STRING,
                    string.Format(
                        ArcCollectorLinkGenerator.ARCGIS,
                        ArcCollectorLinkGenerator.ARCGIS_COLLECTOR_URL,
                        entity.OperatingCenter.ArcMobileMapId,
                        string.Empty,
                        $"%7B%22MapCall_ID%22:%221%22,%22ValveID%22:%22VAB-100%22,%22Application%22:%229999%22,%22Diameter%22:%220%22,%22LifeCycleStatus%22:%221%22,%22SAPID%22:%22000000000000000001%22%7D")),
                _target.ViewData["ArcCollectorLink"].ToString());

            entity.OperatingCenter.DataCollectionMapUrl = "https://arcgis-collector://";

            _target.Show(entity.Id);

            Assert.AreEqual(
                string.Format(ArcCollectorLinkGenerator.ARCGIS_COLLECTOR_HTML_FORMAT_STRING,
                    string.Format(
                        ArcCollectorLinkGenerator.ARCGIS,
                        "https://arcgis-collector://",
                        entity.OperatingCenter.ArcMobileMapId,
                        string.Empty,
                        $"%7B%22MapCall_ID%22:%221%22,%22ValveID%22:%22VAB-100%22,%22Application%22:%229999%22,%22Diameter%22:%220%22,%22LifeCycleStatus%22:%221%22,%22SAPID%22:%22000000000000000001%22%7D")),
                _target.ViewData["ArcCollectorLink"].ToString());
        }

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            Application.ViewEngine.ThrowIfViewIsNotRegistered = false;
            var entity = GetFactory<ValveFactory>().Create();
            InitializeControllerAndRequest("~/FieldOperations/Valve/Show" + entity.Id + ".pdf");

            var result = _target.Show(entity.Id) as PdfResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(entity, result.Model);
            Assert.AreEqual(result.ViewName, "Pdf");
        }

        [TestMethod]
        public void TestShowRespondsWithIndianaPdfForIndianaValve()
        {
            Application.ViewEngine.ThrowIfViewIsNotRegistered = false;
            var indiana = GetEntityFactory<State>().Create(new {Abbreviation = "IN", Name = "Indiana"});
            var town = GetEntityFactory<Town>().Create(new {State = indiana});
            var entity = GetFactory<ValveFactory>().Create(new {Town = town});
            InitializeControllerAndRequest("~/FieldOperations/Valve/Show" + entity.Id + ".pdf");

            var result = _target.Show(entity.Id) as PdfResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(entity, result.Model);
            Assert.AreEqual("Pdf\\IN", result.ViewName);
        }

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var entity = GetFactory<ValveFactory>().Create();
            InitializeControllerAndRequest("~/FieldOperations/Valve/Show" + entity.Id + ".frag");

            var result = _target.Show(entity.Id);
            MvcAssert.IsViewWithNameAndModel(result, "_ShowPopup", entity);
        }

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var entity = GetFactory<ValveFactory>().Create();
            InitializeControllerAndRequest("~/FieldOperations/Valve/Show" + entity.Id + ".map");

            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(MapResult));
        }

        [TestMethod]
        public void TestShowDisplaysNotificationMessageIfItCannotBeCopied()
        {
            var entity = GetEntityFactory<Valve>().Create();

            var result = _target.Show(entity.Id) as ViewResult;
            _target.AssertTempDataContainsMessage(ValveController.CANNOT_COPY_ERROR, ValveController.NOTIFICATION_MESSAGE_KEY);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexForMapExtensionAlsoReturnsBlowOffsInSameFunctionalLocation()
        {
            var functionalLocations = GetEntityFactory<FunctionalLocation>().CreateList(2);
            var coordinate = GetEntityFactory<Coordinate>().Create(new { Latitude = 40m, Longitude = -70m});
            var eq1 = GetEntityFactory<Valve>().Create(new {FunctionalLocation = functionalLocations[0], Coordinate = coordinate});
            var noFindyFind = GetEntityFactory<Valve>().Create(new { FunctionalLocation = functionalLocations[1] });
            var eq3 = GetFactory<BlowOffValveFactory>().Create(new { FunctionalLocation = functionalLocations[0], Coordinate = coordinate });
            // the valves can't be due inspection, otherwise an additional coordinate is returned with the due inspection icon
            // so we're creating a valid inspection
            GetEntityFactory<ValveInspection>().Create(new { Valve = eq1, Inspected = true, DateInspected = DateTime.Now });
            var search = new SearchValve{FunctionalLocation = functionalLocations[0].Id, OperatingCenter = eq1.OperatingCenter.Id};
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.MAP;

            var result = _target.Index(search) as MapResult;
            var resultModel = result.CoordinateSets;

            Assert.AreEqual(2, resultModel.Count, "There should be two coordinate sets. One for valves, one for blowoffs.");
            Assert.AreEqual(1, resultModel.First().Coordinates.Count());
            Assert.AreEqual(2, resultModel.Last().Coordinates.Count());
            Assert.AreEqual(eq1.Coordinate.Latitude, resultModel[0].Coordinates.First().Coordinate.Latitude);
            Assert.AreEqual(eq1.Coordinate.Longitude, resultModel[0].Coordinates.First().Coordinate.Longitude);

            Assert.AreEqual(eq1.Coordinate.Latitude, resultModel[1].Coordinates.First().Coordinate.Latitude);
            Assert.AreEqual(eq1.Coordinate.Longitude, resultModel[1].Coordinates.First().Coordinate.Longitude);

            Assert.AreEqual(eq3.Coordinate.Latitude, resultModel[1].Coordinates.Last().Coordinate.Latitude);
            Assert.AreEqual(eq3.Coordinate.Longitude, resultModel[1].Coordinates.Last().Coordinate.Longitude);
        }

        [TestMethod]
        public void TestIndexForMapExtensionIncludesRelatedAssetsUrlForHydrantsInMapResult()
        {
            var search = new SearchValve();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =ResponseFormatter.KnownExtensions.MAP;
            _target.ModelState.Add("RelatedParameter", new ModelState { Value = new ValueProviderResult("not this", "42",CultureInfo.CurrentCulture)});

            var result = _target.Index(search) as AssetMapResult;
            Assert.AreEqual("/Hydrant/Index.map?RelatedParameter=42&IsRelatedAssetSearch=True", result.RelatedAssetsUrl);
        }
        
        [TestMethod]
        public void TestIndexForMapExtensionOnlyReturnsSingleValveEntityWhenEntityIdSearched()
        {
            var functionalLocations = GetEntityFactory<FunctionalLocation>().CreateList(2);
            var coordinate = GetEntityFactory<Coordinate>().Create(new { Latitude = 40m, Longitude = -70m });
            var eq1 = GetEntityFactory<Valve>().Create(new { FunctionalLocation = functionalLocations[0], Coordinate = coordinate });
            var eq2 = GetEntityFactory<Valve>().Create(new { FunctionalLocation = functionalLocations[1] });
            var oeq1 = GetEntityFactory<Hydrant>().Create(new { FunctionalLocation = functionalLocations[0] });
            var oeq2 = GetEntityFactory<Hydrant>().Create(new { FunctionalLocation = functionalLocations[1] });
            var search = new SearchValve {EntityId = eq1.Id, OperatingCenter = eq1.OperatingCenter.Id};
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.MAP;

            var result = _target.Index(search) as MapResult;
            var resultModel = result.CoordinateSets;

            Assert.AreEqual(1, resultModel.Count);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<Valve>().Create(new { ValveNumber = "VAB-0" });
            var entity1 = GetEntityFactory<Valve>().Create(new { ValveNumber = "VAB-1" });
            var search = new SearchValve { OperatingCenter = entity0.OperatingCenter.Id };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.ValveNumber, "ValveNumber");
                helper.AreEqual(entity1.ValveNumber, "ValveNumber", 1);
            }
        }

        #endregion

        #region New/Create

        #region SAP Syncronization

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsErrorCodeUponFailure()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });

            var model = _viewModelFactory.Build<CreateValve, Valve>( GetEntityFactory<Valve>().BuildWithConcreteDependencies(new { OperatingCenter = opCntr, Town = town }));

            _target.Create(model);

            // FAILS BECAUSE WE DID NOT INJECT AN SAPRepository
            var valve = Repository.Find(model.Id);
            Assert.IsTrue(valve.SAPErrorCode.StartsWith(ValveController.SAP_UPDATE_FAILURE));
        }

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsSAPEquipmentIdAndNoErrorMessage()
        {
            //ARRANGE
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var sapEquipment = new SAPEquipment { SAPEquipmentNumber = "123456789", SAPErrorCode = string.Empty };
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);
            var model = _viewModelFactory.Build<CreateValve, Valve>( GetEntityFactory<Valve>().BuildWithConcreteDependencies(new { OperatingCenter = opCntr, Town = town }));

            //ACT
            _target.Create(model);
            var valve = Repository.Find(model.Id);

            //ASSERT
            Assert.AreEqual(string.Empty, valve.SAPErrorCode);
            Assert.AreEqual(int.Parse(sapEquipment.SAPEquipmentNumber), valve.SAPEquipmentId);
        }

        #endregion

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<Valve>().Create(new { ValveSuffix = 1 });
            var expected = "VAB-1";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditValve, Valve>(eq, new {
                ValveNumber = expected
            }));

            Assert.AreEqual(expected, Session.Get<Valve>(eq.Id).Description);
        }

        #region SAP Syncronization

        [TestMethod]
        public void TestUpdateCallsSAPRepositorySaveAndRecordsErrorCodeUponFailure()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });

            var model = _viewModelFactory.Build<EditValve, Valve>( GetEntityFactory<Valve>().Create(new { OperatingCenter = opCntr, Town = town }));

            _target.Update(model);

            // FAILS BECAUSE WE DID NOT INJECT AN SAPRepository
            var valve = Repository.Find(model.Id);
            Assert.IsTrue(valve.SAPErrorCode.StartsWith(ValveController.SAP_UPDATE_FAILURE));
        }

        [TestMethod]
        public void TestUpdateCallsSAPRepositorySaveAndDoesNotModifySapEquipmentIdOrError()
        {
            //ARRANGE
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var sapEquipmentId = "420311";
            var sapEquipment = new SAPEquipment { SAPErrorCode = string.Empty, SAPEquipmentNumber = sapEquipmentId};
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);
            var entity = GetEntityFactory<Valve>().Create(new { OperatingCenter = opCntr, Town = town, SAPEquipmentId = int.Parse(sapEquipmentId) });
            var model = _viewModelFactory.Build<EditValve, Valve>( entity);

            //ACT
            _target.Update(model);
            var valve = Repository.Find(model.Id);

            //ASSERT
            Assert.IsTrue(String.IsNullOrWhiteSpace(valve.SAPErrorCode));
            Assert.AreEqual(int.Parse(sapEquipmentId), valve.SAPEquipmentId);
        }

        /// <summary>
        /// We send an sapequipmentId of 0 and are returned the actual one.
        /// </summary>
        [TestMethod]
        public void TestUpdateEffectivelyCreatesInSAPWhenSAPEquipmentIdEqualsZero()
        {
            //ARRANGE
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var sapEquipmentId = "420311";
            var sapEquipment = new SAPEquipment { SAPErrorCode = string.Empty, SAPEquipmentNumber = sapEquipmentId };
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);
            var entity = GetEntityFactory<Valve>().Create(new { OperatingCenter = opCntr, Town = town, SAPEquipmentId = 0 });
            var model = _viewModelFactory.Build<EditValve, Valve>( entity);

            //ACT
            _target.Update(model);
            var valve = Repository.Find(model.Id);

            //ASSERT
            Assert.IsTrue(String.IsNullOrWhiteSpace(valve.SAPErrorCode));
            Assert.AreEqual(int.Parse(sapEquipmentId), valve.SAPEquipmentId);
        }

        #endregion

        #endregion

        #region Replace

        [TestMethod]
        public void TestReplaceSavesTheCorrectData()
        {
            var expectedDate = _now;

            var requestRetirement = GetFactory<RequestRetirementAssetStatusFactory>().Create();
            var pending = GetFactory<PendingAssetStatusFactory>().Create();
            var sapOPC = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "SAP", SAPEnabled = true, IsContractedOperations = false });

            var sapEquipment = new SAPEquipment { SAPEquipmentNumber = "123456789", SAPErrorCode = string.Empty };
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);

            var existingValve = GetFactory<ValveFactory>().Create(new { OperatingCenter = sapOPC, DateInstalled = expectedDate });
            Assert.IsTrue(existingValve.OperatingCenter.CanSyncWithSAP, "Sanity");
            Assert.AreNotSame(pending, existingValve.Status, "Sanity");

            var existingValveInspection = GetFactory<ValveInspectionFactory>().Create(new { Valve = existingValve });
            existingValve.ValveInspections.Add(existingValveInspection);

            var existingBlowOffInspection = GetFactory<BlowOffInspectionFactory>().Create(new { Valve = existingValve });
           // existingValve.BlowOffInspections.Add(existingBlowOffInspection);

            _target.Replace(existingValve.Id);

            var lastReplacementModel = _target.LastReplaceValveVM;

            Assert.AreSame(requestRetirement, existingValve.Status, "The existing valve's status must be set to REQUEST RETIREMENT");
            Assert.AreNotEqual(existingValve.Id, lastReplacementModel.Id, "The Id must be the REPLACEMENT hydrant's id.");

            var replacement = Repository.Find(lastReplacementModel.Id);

            // Fields that are copied from the existing hydrant.
            Assert.AreSame(existingValve.OperatingCenter, replacement.OperatingCenter);
            Assert.AreSame(existingValve.Town, replacement.Town);
            Assert.AreSame(existingValve.TownSection, replacement.TownSection);
            Assert.AreSame(existingValve.Facility, replacement.Facility);
            Assert.AreEqual(existingValve.StreetNumber, replacement.StreetNumber);
            Assert.AreSame(existingValve.Street, replacement.Street);
            Assert.AreSame(existingValve.CrossStreet, replacement.CrossStreet);
            Assert.AreEqual(existingValve.Route, replacement.Route);
            Assert.AreEqual(existingValve.Stop, replacement.Stop);
            Assert.AreEqual(existingValve.MapPage, replacement.MapPage);
            Assert.AreEqual(existingValve.ValveLocation, replacement.ValveLocation);
            Assert.AreEqual(existingValve.Elevation, replacement.Elevation);
            Assert.AreEqual(existingValve.ValveNumber, replacement.ValveNumber);
            Assert.AreEqual(existingValve.ValveSuffix, replacement.ValveSuffix);
            Assert.AreEqual(existingValve.Critical, replacement.Critical);
            Assert.AreEqual(existingValve.CriticalNotes, replacement.CriticalNotes);
            Assert.AreEqual(existingValve.InspectionFrequency, replacement.InspectionFrequency);
            Assert.AreEqual(existingValve.BPUKPI, replacement.BPUKPI);
            Assert.AreSame(existingValve.InspectionFrequencyUnit, replacement.InspectionFrequencyUnit);
            Assert.AreSame(existingValve.WaterSystem, replacement.WaterSystem);
            Assert.AreSame(existingValve.ValveBilling, replacement.ValveBilling);
            Assert.AreSame(existingValve.ValveControls, replacement.ValveControls);
            Assert.AreSame(existingValve.NormalPosition, replacement.NormalPosition);
            Assert.AreSame(existingValve.ValveZone, replacement.ValveZone);

            // Fields that are not copied from the existing hydrant.
            Assert.AreSame(pending, replacement.Status);
            Assert.AreSame(_user, replacement.Initiator);
            Assert.AreEqual(expectedDate, replacement.UpdatedAt);
            Assert.AreEqual(expectedDate, replacement.CreatedAt);
            Assert.IsNull(replacement.DateInstalled);
            Assert.IsNotNull(existingValve.DateInstalled);
            Assert.AreEqual(expectedDate, existingValve.DateInstalled);
            
            Assert.AreNotEqual(existingValve.Coordinate.Id, replacement.Coordinate.Id, "A new Coordinate record must be created.");
            Assert.AreEqual(existingValve.Coordinate.Latitude, replacement.Coordinate.Latitude);
            Assert.AreEqual(existingValve.Coordinate.Longitude, replacement.Coordinate.Longitude);
            Assert.AreSame(existingValve.Coordinate.Icon, replacement.Coordinate.Icon);

            Assert.IsTrue(existingValve.ValveInspections.Any(), "Sanity");
            Assert.IsFalse(replacement.ValveInspections.Any(), "Valve inspections must not be copied.");

            var replacementInspection = replacement.BlowOffInspections.Single();
            Assert.AreNotEqual(existingBlowOffInspection.Id, replacementInspection.Id, "A new inspection record must be created.");
            Assert.AreSame(replacement, replacementInspection.Valve, "The new record must belong to the new replacement valve.");
        }

        [TestMethod]
        public void TestReplaceReturns404IfValveDoesNotExist()
        {
            MvcAssert.IsNotFound(_target.Replace(0));
        }

        [TestMethod]
        public void TestReplaceReturns404IfValveIsNotActive()
        {
            var inactive = GetFactory<ValveFactory>().Create(new {
                Status = typeof(PendingAssetStatusFactory)
            });

            MvcAssert.IsNotFound(_target.Replace(inactive.Id));
        }

        #endregion

        #region Cascades

        [TestMethod]
        public void TestByStreetIdReturnsCascadeResultOfValvesFilteredByStreetId()
        {
            var invalidValve = GetFactory<ValveFactory>().Create();
            var street = GetFactory<StreetFactory>().Create();
            var goodValve = GetFactory<ValveFactory>().Create(new {Street = street, ValveNumber = "123"});

            var result = (CascadingActionResult)_target.ByStreetId(street.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1); // -1 accounts for the select here
            Assert.AreEqual(goodValve.Id.ToString(), actual[1].Value);
            Assert.AreEqual("123", actual[1].Text);
        }

        [TestMethod]
        public void TestByTownIdReturnsCascadeResultOfValvesFilteredByTownId()
        {
            var invalidValve = GetFactory<ValveFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var goodValve = GetFactory<ValveFactory>().Create(new { Town = town, ValveNumber = "123" });

            var result = (CascadingActionResult)_target.ByTownId(town.Id);
            var actual = result.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1); // -1 accounts for the select here
            Assert.AreEqual(goodValve.Id.ToString(), actual[1].Value);
            Assert.AreEqual("123", actual[1].Text);
        }

        [TestMethod]
        public void TestByOperatingCenterAndValveNumberReturnsSingleValveFilteredByOperatingCenterAndValveNumber()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var street = GetFactory<StreetFactory>().Create();
            var crossStreet = GetFactory<StreetFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var townSection = GetFactory<TownSectionFactory>().Create(new {Town = town});
            var expectedNormalPosition = GetFactory<ValveNormalPositionFactory>().Create();
            var expectedOpenDirection = GetFactory<ValveOpenDirectionFactory>().Create();
            var valveSize = GetEntityFactory<ValveSize>().Create(new { Size = 1.4m });

            var expectedValve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                ValveNumber = "123",
                Street = street,
                TownSection = townSection,
                StreetNumber = "12",
                Turns = 14m,
                ValveSize = valveSize,
                CrossStreet = crossStreet,
                ValveLocation = "some location",
                NormalPosition = expectedNormalPosition,
                OpenDirection = expectedOpenDirection,
                DateInstalled = new DateTime(1984, 4, 24)
            });
            var badValve = GetFactory<ValveFactory>().Create();

            var model = new SearchOperatingCenterValveNumber {
                OperatingCenterIdentifier = opc.Id,
                ValveNumberSearch = expectedValve.ValveNumber
            };

            var result = (JsonResult)_target.ByOperatingCenterAndValveNumber(model);
            var data = (Dictionary<string, object>)result.Data;

            Assert.AreEqual(true, data["success"]);
            Assert.AreEqual(expectedValve.Id, data["valveId"]);
            Assert.AreEqual(expectedValve.ValveNumber, data["valveNumber"]);
            Assert.AreEqual(expectedValve.Street.Id, data["streetId"]);
            Assert.AreEqual(expectedValve.Town.Id, data["townId"]);
            Assert.AreEqual(expectedValve.OperatingCenter.Id, data["operatingCenterId"]);
            Assert.AreEqual(expectedValve.TownSection.Description, data["townSection"]);
            Assert.AreEqual(expectedValve.StreetNumber, data["streetNumber"]);
            Assert.AreEqual(expectedValve.Turns, data["turns"]);
            Assert.AreEqual(expectedValve.ValveSize.Size.ToString(), data["valveSize"]);
            Assert.AreEqual(expectedValve.CrossStreet.FullStName, data["crossStreet"]);
            Assert.AreEqual(expectedValve.ValveLocation, data["location"]);
            Assert.AreEqual(expectedNormalPosition.Id, data["normalPosition"]);
            Assert.AreEqual(expectedOpenDirection.Id, data["openDirection"]);
            Assert.AreEqual("4/24/1984", data["dateCompleted"]);
            Assert.AreEqual("True", data["isDefaultImageForValve"]);

            var valveImage = GetFactory<ValveImageFactory>().Create(new { Valve = expectedValve, IsDefaultImageForValve = true });
            Session.Clear();

            result = (JsonResult)_target.ByOperatingCenterAndValveNumber(model);
            data = (Dictionary<string, object>)result.Data;

            Assert.AreEqual("False", data["isDefaultImageForValve"]);
        }

        [TestMethod]
        public void TestByOperatingCenterAndValveNumberReturnsZeroForStreetIdIfStreetIsNull()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var expectedValve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                ValveNumber = "123",
            });
            expectedValve.Street = null;

            var model = new SearchOperatingCenterValveNumber {
                OperatingCenterIdentifier = opc.Id,
                ValveNumberSearch = expectedValve.ValveNumber
            };

            var result = (JsonResult)_target.ByOperatingCenterAndValveNumber(model);
            var data = (Dictionary<string, object>)result.Data;

            Assert.AreEqual(0, data["streetId"]);
        }

        [TestMethod]
        public void TestByOperatingCenterAndValveNumberDoesNotSetDateCompletedIfValveDateInstalledIsNull()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var expectedValve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                ValveNumber = "123",
            });

            var model = new SearchOperatingCenterValveNumber {
                OperatingCenterIdentifier = opc.Id,
                ValveNumberSearch = expectedValve.ValveNumber
            };

            var result = (JsonResult)_target.ByOperatingCenterAndValveNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.IsFalse(data.ContainsKey("dateCompleted"));
        }

        [TestMethod]
        public void
            TestByOperatingCenterAndValveNumberDoesNotSetNormalPositionIfMatchingValveNormalPositionCanNotBeFound()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var expectedValve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                ValveNumber = "123",
                //NorPos = "uh"
            });

            var model = new SearchOperatingCenterValveNumber {
                OperatingCenterIdentifier = opc.Id,
                ValveNumberSearch = expectedValve.ValveNumber
            };

            var result = (JsonResult)_target.ByOperatingCenterAndValveNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.IsFalse(data.ContainsKey("normalPosition"));
        }

        [TestMethod]
        public void TestByOperatingCenterAndValveNumberDoesNotSetOpenDirectionIfMatchingValveOpenDirectionCanNotBeFound()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var expectedOpensDirection = GetFactory<ValveOpenDirectionFactory>().Create();
            var expectedValve = GetFactory<ValveFactory>().Create(new {
                OperatingCenter = opc,
                ValveNumber = "123"
            });

            var model = new SearchOperatingCenterValveNumber {
                OperatingCenterIdentifier = opc.Id,
                ValveNumberSearch = expectedValve.ValveNumber
            };

            var result = (JsonResult)_target.ByOperatingCenterAndValveNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.IsFalse(data.ContainsKey("openDirection"));
        }

        [TestMethod]
        public void TestByOperatingCenterAndValveNumberReturnsSuccessFalseWithMessageIfNoMatchesAreFound()
        {
            var model = new SearchOperatingCenterValveNumber {
                OperatingCenterIdentifier = 0,
                ValveNumberSearch = "WUBBA LUBBA DUB DUUBS!"
            };

            var result = (JsonResult)_target.ByOperatingCenterAndValveNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual(false, data["success"]);
            Assert.AreEqual("There are no valves that match the search parameters.", data["message"]);
        }

        [TestMethod]
        public void TestByOperatingCenterAndValveNumberReturnsSuccessFalseWithMessageIfMoreThanOneMatcheIsFound()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var v2 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opc, ValveNumber = "123",});
            var v1 = GetFactory<ValveFactory>().Create(new {OperatingCenter = opc, ValveNumber = "123",});
            var model = new SearchOperatingCenterValveNumber {
                OperatingCenterIdentifier = opc.Id,
                ValveNumberSearch = "123"
            };

            var result = (JsonResult)_target.ByOperatingCenterAndValveNumber(model);
            var data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual(false, data["success"]);
            Assert.AreEqual(
                "Unable to retrieve valve data because there are multiple valves matching the search parameters.",
                data["message"]);
        }

        [TestMethod]
        public void TestByOperatingCenterAndValveNumberReturnsSuccessFalseIfModelStateIsInvalid()
        {
            _target.ModelState.AddModelError("Oops", "Oops");
            var result = (JsonResult)_target.ByOperatingCenterAndValveNumber(null);
            var data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual(false, data["success"]);
            Assert.AreEqual("Invalid search parameters.", data["message"]);
        }

        [TestMethod]
        public void TestRouteByTownIdReturnsRoutesInTown()
        {
            var towns = GetEntityFactory<Town>().CreateList(2);
            var valveValid = GetEntityFactory<Valve>().Create(new { Town = towns[0], Route = 1 });
            var valveInvalid = GetEntityFactory<Valve>().Create(new { Town = towns[1], Route = 2});

            var result = (CascadingActionResult)_target.RouteByTownId(towns[0].Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(2, actual.Count()); // because the first is --select here--
            Assert.AreEqual(valveValid.Route.ToString(), actual.Last().Text);
        }

        [TestMethod]
        public void TestRouteByOperatingCenterIdAndOrTownIdInTown()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var towns = GetEntityFactory<Town>().CreateList(2);
            var optown1 = GetEntityFactory<OperatingCenterTown>().Create(new {Town = towns[0], OperatingCenter = operatingCenter});
            var optown2 = GetEntityFactory<OperatingCenterTown>().Create(new {Town = towns[1], OperatingCenter = operatingCenter});
            var valveValid = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, Town = towns[0], Route = 1 });
            var valveInvalid = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenter, Town = towns[1], Route = 2 });
            var result = (CascadingActionResult)_target.RouteByOperatingCenterIdAndOrTownId(operatingCenter.Id, null);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(3, actual.Count()); // because the first is --select here--

            result = (CascadingActionResult)_target.RouteByOperatingCenterIdAndOrTownId(operatingCenter.Id, towns[0].Id);
            actual = result.GetSelectListItems();

            Assert.AreEqual(2, actual.Count()); // because the first is --select here--
            Assert.AreEqual(valveValid.Route.ToString(), actual.Last().Text);
        }

        [TestMethod]
        public void TestGetByTownIdAndOperatingCenterId()
        {
            var oc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var towns = GetEntityFactory<Town>().CreateList(2);
            var optown1 = GetEntityFactory<OperatingCenterTown>().Create(new { Town = towns[0], OperatingCenter = oc });
            var optown2 = GetEntityFactory<OperatingCenterTown>().Create(new { Town = towns[1], OperatingCenter = oc });
            var goodValve = GetEntityFactory<Valve>().Create(new {OperatingCenter = oc, Town = towns[0], ValveNumber = "123" });
            var badValve = GetEntityFactory<Valve>().Create(new {OperatingCenter = oc, Town = towns[1], ValveNumber = "321" });
            var result = (CascadingActionResult)_target.ByTownIdAndOperatingCenterId(towns[0].Id, oc.Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(2, actual.Count());
            Assert.AreEqual(goodValve.DescriptionWithStatus, actual.Last().Text);

            result = (CascadingActionResult)_target.ByTownIdAndOperatingCenterId(towns[1].Id, oc.Id);
            actual = result.GetSelectListItems();

            Assert.AreNotEqual(goodValve.DescriptionWithStatus, actual.Last().Text);
            Assert.AreEqual(badValve.DescriptionWithStatus, actual.Last().Text);
        }

        [TestMethod]
        public void TestGetByOperatingCenter()
        {
            var operatingCenterA = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterB = GetFactory<UniqueOperatingCenterFactory>().Create();
            var valveAInOperatingCenterA = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenterA, ValveNumber = "A" });
            var valveBInOperatingCenterB = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenterB, ValveNumber = "B" });
            var valveCInOperatingCenterB = GetEntityFactory<Valve>().Create(new { OperatingCenter = operatingCenterB, ValveNumber = "C" });
            
            var actionResult = (CascadingActionResult)_target.ByOperatingCenter(operatingCenterA.Id);
            var selectListItems = actionResult.GetSelectListItems().ToList();

            Assert.AreEqual(2, selectListItems.Count);
            Assert.AreEqual(valveAInOperatingCenterA.DescriptionWithStatus, selectListItems[1].Text);

            actionResult = (CascadingActionResult)_target.ByOperatingCenter(operatingCenterB.Id);
            selectListItems = actionResult.GetSelectListItems().ToList();

            Assert.AreEqual(3, selectListItems.Count);
            Assert.AreEqual(valveBInOperatingCenterB.DescriptionWithStatus, selectListItems[1].Text);
            Assert.AreEqual(valveCInOperatingCenterB.DescriptionWithStatus, selectListItems[2].Text);

            actionResult = (CascadingActionResult)_target.ByOperatingCenter(operatingCenterA.Id, operatingCenterB.Id);
            selectListItems = actionResult.GetSelectListItems().ToList();

            Assert.AreEqual(4, selectListItems.Count);
            Assert.AreEqual(valveAInOperatingCenterA.DescriptionWithStatus, selectListItems[1].Text);
            Assert.AreEqual(valveBInOperatingCenterB.DescriptionWithStatus, selectListItems[2].Text);
            Assert.AreEqual(valveCInOperatingCenterB.DescriptionWithStatus, selectListItems[3].Text);
        }

        #endregion

        #endregion

        #region Notifications

        [TestMethod]
        public void TestCreateSendsNotificationEmailForCorrectAssetStatuses()
        {
            // Sends for new Active or Retired valves
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc1, Town = town, Abbreviation = "XX" });
            var ent = GetEntityFactory<Valve>().Create();
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var model = _viewModelFactory.BuildWithOverrides<CreateValve, Valve>(ent, new {IsFoundValve = false, Coordinate = coordinate.Id, ValveSuffix = 1, Town = town.Id, OperatingCenter = opc1.Id });
            
            foreach (var valveStatus in new[] {_activeStatus.Id, _retiredStatus.Id})
            {
                model.Id = 0;
                model.Status = valveStatus;
                NotifierArgs resultArgs = null;
             //   ValidationAssert.ModelStateIsValid(model);
                _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

                var result = _target.Create(model);
                var entity = Repository.Find(model.Id);

                Assert.AreSame(entity, ((ValveNotification)resultArgs.Data).Valve);
                Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
                Assert.AreEqual(ValveController.ROLE, resultArgs.Module);
                Assert.AreEqual(ValveController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
                Assert.IsNull(resultArgs.Subject);
            }
        }

        [TestMethod]
        public void TestCreateDoesNotSendNotificationForInvalidAssetStatus()
        {
            // Does not send if not active or retired.
            var ent = GetEntityFactory<Valve>().Create();
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var invalidStatus = GetFactory<PendingAssetStatusFactory>().Create();
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown{OperatingCenter=opc1,Town=town,Abbreviation = "XX"});

            var model = _viewModelFactory.BuildWithOverrides<CreateValve, Valve>(ent, new { IsFoundValve = false, Coordinate = coordinate.Id, Status = invalidStatus.Id, ValveSuffix = 1, Town = town.Id, OperatingCenter = opc1.Id });
            model.Id = 0;
            
            //ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Create(model);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
            Assert.IsNull(resultArgs);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenSwitchedToActive()
        {
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var entity = GetEntityFactory<Valve>().Create(new { Status = _otherStatus, Coordinate = coordinate });
            var model = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(entity, new { Status = _activeStatus.Id});

            //ValidationAssert.ModelStateIsValid(model);
            
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);
            entity = Repository.Find(model.Id);

            Assert.AreSame(entity, ((ValveNotification)resultArgs.Data).Valve);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenSwitchedFromActive()
        {
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var entity = GetEntityFactory<Valve>().Create(new { Status = _activeStatus, Coordinate = coordinate});
            var model = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(entity, new { Status = _otherStatus.Id});

           // ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);
            entity = Repository.Find(model.Id);

            Assert.AreSame(entity, ((ValveNotification)resultArgs.Data).Valve);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenSwitchedToRetired()
        {
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var entity = GetEntityFactory<Valve>().Create(new { Status = _otherStatus, Coordinate = coordinate });
            var model = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(entity, new { Status = _retiredStatus.Id });

           // ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);
            entity = Repository.Find(model.Id);

            Assert.AreSame(entity, ((ValveNotification)resultArgs.Data).Valve);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenSwitchedFromRetired()
        {
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var entity = GetEntityFactory<Valve>().Create(new { Status = _retiredStatus, Coordinate = coordinate });
            var model = _viewModelFactory.BuildWithOverrides<EditValve, Valve>(entity, new { Status = _otherStatus.Id });

          //  ValidationAssert.ModelStateIsValid(model);

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);
            entity = Repository.Find(model.Id);

            Assert.AreSame(entity, ((ValveNotification)resultArgs.Data).Valve);
        }

        [TestMethod]
        public void TestCopySendsNotificationEmailWhenActive()
        {
            _user.IsAdmin = true;
            var valveSize = GetEntityFactory<ValveSize>().Create();
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc1, Town = town, Abbreviation = "XX" });
            var valve = GetEntityFactory<Valve>().Create(new { Town = town, OperatingCenter = opc1, Status = _activeStatus, ValveSize = valveSize, Turns = 1m });

            _target.Copy(valve.Id);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        #endregion

        #region Test classes

        // This has to be named exactly the same as the base controller or else some tests involving RouteContexts start failing.
        public class ValveController : MapCallMVC.Areas.FieldOperations.Controllers.ValveController
        {
            public ReplaceValve LastReplaceValveVM { get; set; }

            public ValveController(ControllerBaseWithPersistenceArguments<IValveRepository, Valve, User> args) : base(args) { }

            protected override ReplaceValve GetReplaceValveModel()
            {
                LastReplaceValveVM = base.GetReplaceValveModel();
                return LastReplaceValveVM;
            }
        }

        #endregion
    }
}
