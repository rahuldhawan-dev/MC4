using System;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
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
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Controllers;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class HydrantControllerTest : MapCallMvcControllerTestBase<HydrantControllerTest.HydrantController, Hydrant, HydrantRepository>
    {
        #region Fields

        private Mock<INotificationService> _noteServ;
        private Mock<ISAPEquipmentRepository> _sapRepository;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ILog>().Use(new Mock<ILog>().Object);
        }

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {

            _noteServ = new Mock<INotificationService>();
            _container.Inject(_noteServ.Object);

            GetFactory<AssetStatusFactory>().CreateAll();
            GetFactory<HydrantBillingFactory>().CreateAll();
            // This just needs to exist.
            GetFactory<TownSectionAbbreviationTypeFactory>().Create();
            GetFactory<HydrantAssetTypeFactory>().Create();

            var roleServ = new Mock<IRoleService>();
            _container.Inject(roleServ.Object);

            _sapRepository = new Mock<ISAPEquipmentRepository>();
            _container.Inject(_sapRepository.Object);
            var abbreviationType = GetEntityFactory<AbbreviationType>().Create(new {Description = "Town Section"});
            Application.ViewEngine = new MapCallMvcViewEngine();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateHydrant)vm;
                var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create();
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
                model.OperatingCenter = opCntr.Id;
                model.Town = town.Id;
            };
            options.InitializeSearchTester = (tester) => {
                tester.IgnoredPropertyNames.Add("Route");
            };
        }

        #endregion

        #region Tests

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            var module = RoleModules.FieldServicesAssets;
            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/Hydrant/New/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/Hydrant/Create/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/Hydrant/Replace/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/Hydrant/Copy/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/Hydrant/Show/", module);
                a.RequiresRole("~/FieldOperations/Hydrant/Index/", module);
                a.RequiresRole("~/FieldOperations/Hydrant/Search/", module);
                a.RequiresRole("~/FieldOperations/Hydrant/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Hydrant/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Hydrant/GetHydrantPrefix/", module);
                a.RequiresRole("~/FieldOperations/Hydrant/ValidateUnusedFoundHydrantSuffix/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/Hydrant/SetHydrantBackInService/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/Hydrant/SetHydrantOutOfService/", module, RoleActions.Edit);
                a.RequiresLoggedInUserOnly("~/FieldOperations/Hydrant/RouteByTownId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/Hydrant/ActiveByTownId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/Hydrant/ByTownId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/Hydrant/RouteByOperatingCenterIdAndOrTownId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/Hydrant/ByStreetId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/Hydrant/ByStreetIdForWorkOrders/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/Hydrant/ByOperatingCenter/");
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowSetsArcCollectorUrlInViewData()
        {
            var manufacturer = GetFactory<HydrantManufacturerFactory>().Create(new {Description = "Callahan"});
            var entity = GetFactory<HydrantFactory>().Create(new {HydrantManufacturer = manufacturer});

            entity.DateInstalled = null;

            _target.Show(entity.Id);

            MyAssert.StringsAreEqual(
                string.Format(ArcCollectorLinkGenerator.ARCGIS_COLLECTOR_HTML_FORMAT_STRING,
                    string.Format(
                        ArcCollectorLinkGenerator.ARCGIS,
                        ArcCollectorLinkGenerator.ARCGIS_COLLECTOR_URL,
                        entity.OperatingCenter.ArcMobileMapId,
                        string.Empty,
                        $"%7B%22MapCall_ID%22:%221%22,%22HydrantID%22:%22HAB-1%22,%22BillingType%22:%222%22,%22LifeCycleStatus%22:%221%22,%22Manufacturer%22:%221%22,%22Premise_ID%22:%22987654321%22,%22SAPID%22:%22000000000000123456%22%7D")),
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
                        $"%7B%22MapCall_ID%22:%221%22,%22HydrantID%22:%22HAB-1%22,%22BillingType%22:%222%22,%22LifeCycleStatus%22:%221%22,%22Manufacturer%22:%221%22,%22Premise_ID%22:%22987654321%22,%22SAPID%22:%22000000000000123456%22%7D")),
                _target.ViewData["ArcCollectorLink"].ToString());
        }

        [TestMethod]
        public void TestShowRespondsToFragment()
        {
            var entity = GetFactory<HydrantFactory>().Create();
            InitializeControllerAndRequest("~/FieldOperations/Hydrant/Show" + entity.Id + ".frag");

            var result = _target.Show(entity.Id);
            MvcAssert.IsViewWithNameAndModel(result, "_ShowPopup", entity);
        }

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var entity = GetFactory<HydrantFactory>().Create();
            InitializeControllerAndRequest("~/FieldOperations/Hydrant/Show" + entity.Id + ".map");

            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(MapResult));
        }

        [TestMethod]
        public void TestShowSetsDisplayNotificationForOutOfServiceHydrants()
        {
            var key = MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY;
            var entity = GetEntityFactory<Hydrant>().Create();
            
            entity.SetPropertyValueByName("OutOfService", false);
            _target.Show(entity.Id);
            Assert.IsFalse(_target.TempData.ContainsKey(key));

            entity.SetPropertyValueByName("OutOfService", true);
            _target.Show(entity.Id);
            _target.AssertTempDataContainsMessage("This hydrant is currently out of service.", key);
        }

        [TestMethod]
        public void TestShowRespondsToPdfWithPdfForIndianaHydrants()
        {
            var indiana = GetEntityFactory<State>().Create(new {Abbreviation = "IN", Name = "Indiana"});
            var town = GetEntityFactory<Town>().Create(new {State = indiana});
            var entity = GetFactory<HydrantFactory>().Create(new {Town = town});
            InitializeControllerAndRequest("~/FieldOperations/Hydrant/Show" + entity.Id + ".pdf");

            var result = _target.Show(entity.Id) as PdfResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(entity, result.Model);
            Assert.AreEqual("Pdf\\IN", result.ViewName);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            _currentUser.IsAdmin = true;
            var waterSystem = GetEntityFactory<WaterSystem>().Create();
            var hydrantThreadType = GetEntityFactory<HydrantThreadType>().Create();
            var hydrantOutletConfiguration = GetEntityFactory<HydrantOutletConfiguration>().Create();
            var hydrantType = GetEntityFactory<HydrantType>().Create();
            var mainType = GetEntityFactory<MainType>().Create();
            var hydrantTagStatus = GetEntityFactory<HydrantTagStatus>().Create();
            var entity0 = GetEntityFactory<Hydrant>().Create(new {
                HydrantNumber = "HAB-0",
                LegacyId = "301",
                GISUID = "015008-HYL00000049",
                SAPErrorCode = "Equipment Updated Successfully",
                HasWorkOrder = true,
                HasOpenWorkOrder = true,
                CrossStreet = typeof(StreetFactory),
                HydrantMainSize = typeof(HydrantMainSizeFactory),
                FunctionalLocation = typeof(FunctionalLocationFactory),
                SAPEquipmentId = 123456,
                WaterSystem = waterSystem,
                Critical = true,
                CriticalNotes = "Critical Notes Example",
                Initiator = typeof(UserFactory),
                Facility = typeof(FacilityFactory),
                MapPage = "Map Page Example",
                Gradient = typeof(GradientFactory),
                Elevation = 58.000000M,
                IsDeadEndMain = true,
                HydrantModel = typeof(HydrantModelFactory),
                LateralValve = typeof(ValveFactory),
                BranchLengthFeet = 2,
                BranchLengthInches = 15,
                HydrantThreadType = hydrantThreadType,
                HydrantOutletConfiguration = hydrantOutletConfiguration,
                HydrantType = hydrantType,
                DepthBuryFeet = 2,
                DepthBuryInches = 15,
                IsNonBPUKPI = true,
                MainType = mainType,
                HydrantTagStatus = hydrantTagStatus,
                InspectionFrequency = 1,
                HydrantDueInspection = new HydrantDueInspection {
                    RequiresInspection = true,
                    LastInspectionDate = _now
                },
                HydrantDuePainting = new HydrantDuePainting {
                    RequiresPainting = false,
                    LastPaintedAt = _now
                },
                TownSection = typeof(TownSectionFactory)
            });
            var search = new SearchHydrant{OperatingCenter = entity0.OperatingCenter.Id};
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity0.HydrantNumber, "HydrantNumber");
                helper.AreEqual(entity0.OperatingCenter.ToString(), "OperatingCenter");
                helper.AreEqual(entity0.WaterSystem.ToString(), "WaterSystem");
                helper.AreEqual(entity0.Town.ToString(), "Town");
                helper.AreEqual(entity0.TownSection.ToString(), "TownSection");
                helper.AreEqual(entity0.SAPEquipmentId, "SAPEquipmentId");
                helper.AreEqual(entity0.HydrantManufacturer.Description, "HydrantManufacturer");
                helper.IsNull("YearManufactured");
                helper.IsNull("OpenDirection");
                helper.AreEqual(entity0.WorkOrderNumber, "WorkOrderNumber");
                helper.IsNull("StreetNumber");
                helper.AreEqual(entity0.LegacyId, "LegacyId");
                helper.AreEqual(entity0.GISUID, "GISUID");
                helper.AreEqual(entity0.SAPErrorCode, "SAPErrorCode");
                helper.AreEqual(entity0.Street, "Street");
                helper.AreEqual(entity0.CrossStreet, "CrossStreet");
                helper.AreEqual(entity0.HydrantDueInspection?.RequiresInspection, "RequiresInspection");
                helper.AreEqual(entity0.HydrantDuePainting?.RequiresPainting, "RequiresPainting");
                helper.AreEqual(entity0.HydrantDuePainting?.LastPaintedAt, "LastPaintedAt");
                helper.AreEqual(entity0.DateInstalled, "DateInstalled");
                helper.AreEqual(entity0.HydrantSuffix, "HydrantSuffix");
                helper.AreEqual(entity0.Status, "HydrantStatus");
                helper.AreEqual(entity0.HydrantMainSize, "HydrantMainSize");
                helper.AreEqual(entity0.LateralSize, "LateralSize");
                helper.AreEqual(entity0.FunctionalLocation, "FunctionalLocation");
                helper.AreEqual(entity0.OutOfService, "OutOfService");
                helper.AreEqual(entity0.HasWorkOrder, "HasWorkOrder");
                helper.AreEqual(entity0.HasOpenWorkOrder, "HasOpenWorkOrder");

                helper.AreEqual(entity0.Critical, "HasCriticalNotes");
                helper.AreEqual(entity0.CriticalNotes, "CriticalNotes");
                helper.AreEqual(entity0.Initiator, "Initiator");
                helper.AreEqual(entity0.Facility, "Facility");
                helper.AreEqual(entity0.MapPage, "MapPage");
                helper.AreEqual(entity0.Gradient, "Gradient");
                helper.AreEqual(entity0.Elevation, "Elevation");
                helper.AreEqual(entity0.IsDeadEndMain, "IsDeadEndMain");
                helper.AreEqual(entity0.HydrantModel, "HydrantModel");
                helper.AreEqual(entity0.MainType, "MainType");
                helper.AreEqual(entity0.LateralValve, "LateralValve");
                helper.AreEqual(entity0.BranchLengthFeet, "BranchLengthFeet");
                helper.AreEqual(entity0.BranchLengthInches, "BranchLengthInches");
                helper.AreEqual(entity0.HydrantThreadType, "HydrantThreadType");
                helper.AreEqual(entity0.HydrantOutletConfiguration, "HydrantOutletConfiguration");
                helper.AreEqual(entity0.DepthBuryFeet, "DepthBuryFeet");
                helper.AreEqual(entity0.DepthBuryInches, "DepthBuryInches");
                helper.AreEqual(entity0.IsNonBPUKPI, "IsNonBPUKPI");
                helper.AreEqual(entity0.BillingDate, "BillingDate");
                helper.AreEqual(entity0.HydrantTagStatus, "HydrantTagStatus");
                helper.AreEqual(entity0.InspectionFrequency, "InspectionFrequency");
            }
        }

        [TestMethod]
        public void TestIndexForMapExtensionIncludesRelatedAssetsUrlForValvesInMapResult()
        {
            var search = new SearchHydrant();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.MAP;
            _target.ModelState.Add("RelatedParameter", new ModelState { Value = new ValueProviderResult("not this", "42", CultureInfo.CurrentCulture) });

            var result = _target.Index(search) as AssetMapResult;
            Assert.AreEqual("/Valve/Index.map?RelatedParameter=42&IsRelatedAssetSearch=True", result.RelatedAssetsUrl);
        }

        [TestMethod]
        public void TestIndexForMapExtensionOnlyReturnsSingleValveEntityWhenEntityIdSearched()
        {
            var functionalLocations = GetEntityFactory<FunctionalLocation>().CreateList(2);
            var coordinate = GetEntityFactory<Coordinate>().Create(new { Latitude = 40m, Longitude = -70m });
            var eq1 = GetEntityFactory<Hydrant>().Create(new { FunctionalLocation = functionalLocations[0], Coordinate = coordinate });
            var eq2 = GetEntityFactory<Hydrant>().Create(new { FunctionalLocation = functionalLocations[1] });
            var oeq1 = GetEntityFactory<Valve>().Create(new { FunctionalLocation = functionalLocations[0] });
            var oeq2 = GetEntityFactory<Valve>().Create(new { FunctionalLocation = functionalLocations[1] });
            var search = new SearchHydrant { EntityId = eq1.Id, OperatingCenter = eq1.OperatingCenter.Id };
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.MAP;

            var result = _target.Index(search) as MapResult;
            var resultModel = result.CoordinateSets;

            Assert.AreEqual(1, resultModel.Count);
        }

        [TestMethod]
        public void TestIndexForMap_ReturnsNull_WhenResultCountIsGreaterThen_TenThousand()
        {
            var state = GetEntityFactory<State>().Create();
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var hydrantSize = GetEntityFactory<HydrantSize>().Create();
            var hydrantMainSize = GetEntityFactory<HydrantMainSize>().Create();
            var assetStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            var billing = GetEntityFactory<HydrantBilling>().Create();
            var manufacturer = GetEntityFactory<HydrantManufacturer>().Create();
            var lateralSize = GetEntityFactory<LateralSize>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create();
            var fireDistrict = GetEntityFactory<FireDistrict>().Create();
            var street = GetEntityFactory<Street>().Create();
            var town = GetEntityFactory<Town>().Create();
            var functionalLocation = GetEntityFactory<FunctionalLocation>().Create();
            var hydrantList = GetEntityFactory<Hydrant>()
               .CreateList(SearchHydrantForMap.MAX_MAP_RESULT_COUNT + 1, new {
                    Coordinate = coordinate, State = state, HydrantSize = hydrantSize, HydrantMainSize = hydrantMainSize,  
                    Status = assetStatus, HydrantBilling = billing, HydrantManufacturer = manufacturer, LateralSize = lateralSize,
                    OperatingCenter = operatingCenter, FireDistrict = fireDistrict, Street = street, Town = town, FunctionalLocation = functionalLocation
                });
            var search = new SearchHydrant { State = state.Id };

            var result = _target.Index(search) as MapResult;

            Assert.IsNull(result);
        }

        [TestMethod]
        public void TestIndexForMapCanSearchAllProperties()
        {
            // This test is really annoying because the actions are all using SearchHydrant, but specifically for maps
            // the SearchHydrant is then converted to a SearchHydrantForMap object.
            var tester = _container.GetInstance<SearchModelTesterForSearchSet<SearchHydrant, Hydrant, Hydrant>>();
            tester.SearchCallback = (searchModel) => {
                var model = (SearchHydrant)searchModel;
                InitializeControllerForRequest("~/FieldOperations/Hydrant/Index.map");
                _target.Index(model);
            };
            // Ignoring Route because the property is an int?, but it's not an entity map property.
            // The tester fails since it assumes an int? property must be an entity.
            tester.IgnoredPropertyNames.Add(nameof(SearchHydrant.Route));
            tester.TestAllProperties();
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsNotification()
        {
            _currentUser.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });

            var model = _viewModelFactory.Build<CreateHydrant, Hydrant>( GetEntityFactory<Hydrant>().BuildWithConcreteDependencies(new { OperatingCenter = opCntr, Town = town }));
            model.HydrantBilling = HydrantBilling.Indices.PUBLIC;
            model.Status = AssetStatus.Indices.ACTIVE;
            model.BillingDate = DateTime.Now;
            _target.Create(model);

            _noteServ.Verify(x => x.Notify(It.Is<NotifierArgs>(args => args.Data.GetType().GetProperty("RecordUrl").GetValue(args.Data, null).ToString() == "http://localhost/FieldOperations/Hydrant/Show/1")));
        }

        #region SAP Syncronization

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsErrorCodeUponFailure()
        {
            _currentUser.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });

            var model = _viewModelFactory.Build<CreateHydrant, Hydrant>(GetEntityFactory<Hydrant>().BuildWithConcreteDependencies(new { OperatingCenter = opCntr, Town = town }));

            _target.Create(model);

            // FAILS BECAUSE WE DID NOT INJECT AN SAPRepository
            var hydrant = Repository.Find(model.Id);
            Assert.IsTrue(hydrant.SAPErrorCode.StartsWith(HydrantController.SAP_UPDATE_FAILURE));
        }

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsSAPEquipmentIdAndNoErrorMessage()
        {
            //ARRANGE
            _currentUser.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var sapEquipment = new SAPEquipment { SAPEquipmentNumber = "123456789", SAPErrorCode = string.Empty };
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);
            var model = _viewModelFactory.Build<CreateHydrant, Hydrant>(GetEntityFactory<Hydrant>().BuildWithConcreteDependencies(new { OperatingCenter = opCntr, Town = town }));

            //ACT
            _target.Create(model);
            var hydrant = Repository.Find(model.Id);

            //ASSERT
            Assert.AreEqual(string.Empty, hydrant.SAPErrorCode);
            Assert.AreEqual(int.Parse(sapEquipment.SAPEquipmentNumber), hydrant.SAPEquipmentId);
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
            var sapOPC = GetFactory<OperatingCenterFactory>().Create(new { OperatingCenterCode = "SAP", SAPEnabled = true, IsContractedOperations = false});

            var sapEquipment = new SAPEquipment { SAPEquipmentNumber = "123456789", SAPErrorCode = string.Empty };
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);

            var existingHydrant = GetFactory<HydrantFactory>().Create(new { OperatingCenter = sapOPC });
            Assert.IsTrue(existingHydrant.OperatingCenter.CanSyncWithSAP, "Sanity");
            Assert.AreNotSame(pending, existingHydrant.Status, "Sanity");

            var existingHydrantInspection = GetFactory<HydrantInspectionFactory>().Create(new {
                Hydrant = existingHydrant
            });
            existingHydrant.HydrantInspections.Add(existingHydrantInspection);

            _target.Replace(existingHydrant.Id);

            var lastReplacementModel = _target.LastReplaceHydrantVM;

            Assert.AreSame(requestRetirement, existingHydrant.Status, "The existing hydrant's status must be set to REQUEST RETIREMENT");
            Assert.AreNotEqual(existingHydrant.Id, lastReplacementModel.Id, "The Id must be the REPLACEMENT hydrant's id.");

            var replacement = Repository.Find(lastReplacementModel.Id);

            // Fields that are copied from the existing hydrant.
            Assert.AreSame(existingHydrant.OperatingCenter, replacement.OperatingCenter);
            Assert.AreSame(existingHydrant.Town, replacement.Town);
            Assert.AreSame(existingHydrant.TownSection, replacement.TownSection);
            Assert.AreSame(existingHydrant.FireDistrict, replacement.FireDistrict);
            Assert.AreSame(existingHydrant.Facility, replacement.Facility);
            Assert.AreEqual(existingHydrant.StreetNumber, replacement.StreetNumber);
            Assert.AreSame(existingHydrant.Street, replacement.Street);
            Assert.AreSame(existingHydrant.CrossStreet, replacement.CrossStreet);
            Assert.AreEqual(existingHydrant.PremiseNumber, replacement.PremiseNumber);
            Assert.AreEqual(existingHydrant.Route, replacement.Route);
            Assert.AreEqual(existingHydrant.Stop, replacement.Stop);
            Assert.AreEqual(existingHydrant.MapPage, replacement.MapPage);
            Assert.AreEqual(existingHydrant.Location, replacement.Location);
            Assert.AreEqual(existingHydrant.ValveLocation, replacement.ValveLocation);
            Assert.AreEqual(existingHydrant.Gradient, replacement.Gradient);
            Assert.AreEqual(existingHydrant.Elevation, replacement.Elevation);
            Assert.AreEqual(existingHydrant.IsDeadEndMain, replacement.IsDeadEndMain);
            Assert.AreEqual(existingHydrant.HydrantNumber, replacement.HydrantNumber);
            Assert.AreEqual(existingHydrant.HydrantSuffix, replacement.HydrantSuffix);
            Assert.AreEqual(existingHydrant.Critical, replacement.Critical);
            Assert.AreEqual(existingHydrant.CriticalNotes, replacement.CriticalNotes);
            Assert.AreSame(existingHydrant.LateralValve, replacement.LateralValve);
            Assert.AreEqual(existingHydrant.IsNonBPUKPI, replacement.IsNonBPUKPI);
            Assert.AreSame(existingHydrant.HydrantBilling, replacement.HydrantBilling);
            Assert.AreEqual(existingHydrant.BillingDate, replacement.BillingDate);
            Assert.AreEqual(existingHydrant.BillingDate, replacement.BillingDate);
            Assert.AreEqual(existingHydrant.InspectionFrequency, replacement.InspectionFrequency);
            Assert.AreSame(existingHydrant.InspectionFrequencyUnit, replacement.InspectionFrequencyUnit);
            Assert.AreSame(existingHydrant.WaterSystem, replacement.WaterSystem);

            // Fields that are not copied from the existing hydrant.
            Assert.AreSame(pending, replacement.Status);
            Assert.AreSame(_currentUser, replacement.Initiator);
            Assert.AreEqual(expectedDate, replacement.UpdatedAt);
            Assert.AreEqual(expectedDate, replacement.CreatedAt);
            Assert.IsNull(replacement.DateInstalled);
            Assert.IsNotNull(existingHydrant.DateInstalled);
            Assert.AreEqual(expectedDate.Date, existingHydrant.DateInstalled);
            
            Assert.AreNotEqual(existingHydrant.Coordinate.Id, replacement.Coordinate.Id, "A new Coordinate record must be created.");
            Assert.AreEqual(existingHydrant.Coordinate.Latitude, replacement.Coordinate.Latitude);
            Assert.AreEqual(existingHydrant.Coordinate.Longitude, replacement.Coordinate.Longitude);
            Assert.AreSame(existingHydrant.Coordinate.Icon, replacement.Coordinate.Icon);

            // Ensure the hydrant inspections have been copied to new records.
            var replacementInspection = replacement.HydrantInspections.Single();
            Assert.AreNotEqual(existingHydrantInspection.Id, replacementInspection.Id, "A new inspection record must be created.");
            Assert.AreSame(replacement, replacementInspection.Hydrant, "The new record must belong to the new replacement hydrant.");
        }

        [TestMethod]
        public void TestReplaceReturns404IfHydrantDoesNotExist()
        {
            MvcAssert.IsNotFound(_target.Replace(0));
        }

        [TestMethod]
        public void TestReplaceReturns404IfHydrantIsNotActive()
        {
            var inactive = GetFactory<HydrantFactory>().Create(new {
                Status = typeof(PendingAssetStatusFactory)
            });

            MvcAssert.IsNotFound(_target.Replace(inactive.Id));
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetFactory<HydrantFactory>().Create();
            var expected = "BAH-1";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditHydrant, Hydrant>(eq, new {
                HydrantNumber = expected
            }));

            Assert.AreEqual(expected, Session.Get<Hydrant>(eq.Id).HydrantNumber);
        }

        [TestMethod]
        public void TestUpdateSendsNotification()
        {
            _currentUser.IsAdmin = true;
            var model = _viewModelFactory.Build<EditHydrant, Hydrant>( GetEntityFactory<Hydrant>().Create(new{Status = typeof(RetiredAssetStatusFactory) }));
            model.HydrantBilling = HydrantBilling.Indices.PUBLIC;
            model.Status = AssetStatus.Indices.ACTIVE;
            model.BillingDate = DateTime.Now;
            _target.Update(model);

            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        #region SAP Syncronization

        [TestMethod]
        public void TestUpdateCallsSAPRepositorySaveAndRecordsErrorCodeUponFailure()
        {
            _currentUser.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });

            var entity = GetEntityFactory<Hydrant>().Create(new {OperatingCenter = opCntr, Town = town});
            var model = _viewModelFactory.Build<EditHydrant, Hydrant>( entity);

            _target.Update(model);

            // FAILS BECAUSE WE DID NOT INJECT AN SAPRepository
            var hydrant = Repository.Find(model.Id);
            Assert.IsTrue(hydrant.SAPErrorCode.StartsWith(HydrantController.SAP_UPDATE_FAILURE));
        }

        [TestMethod]
        public void TestUpdateCallsSAPRepositorySaveAndDoesNotModifySapEquipmentIdOrError()
        {
            //ARRANGE
            _currentUser.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var sapEquipment = new SAPEquipment { SAPErrorCode = string.Empty };
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);
            var sapEquipmentId = 420311;
            var entity = GetEntityFactory<Hydrant>().Create(new {OperatingCenter = opCntr, Town = town, SAPEquipmentId = sapEquipmentId });
            var model = _viewModelFactory.Build<EditHydrant, Hydrant>( entity);

            //ACT
            _target.Update(model);
            var hydrant = Repository.Find(model.Id);

            //ASSERT
            Assert.IsTrue(String.IsNullOrWhiteSpace(hydrant.SAPErrorCode));
            Assert.AreEqual(sapEquipmentId, hydrant.SAPEquipmentId);
        }

        /// <summary>
        /// We send an sapequipmentId of 0 and are returned the actual one.
        /// </summary>
        [TestMethod]
        public void TestUpdateEffectivelyCreatesInSAPWhenSAPEquipmentIdEqualsZero()
        {
            //ARRANGE
            _currentUser.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var sapEquipmentId = "420311";
            var sapEquipment = new SAPEquipment { SAPErrorCode = string.Empty, SAPEquipmentNumber = sapEquipmentId};
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);
            var entity = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = opCntr, Town = town, SAPEquipmentId = 0 });
            var model = _viewModelFactory.Build<EditHydrant, Hydrant>( entity);

            //ACT
            _target.Update(model);
            var hydrant = Repository.Find(model.Id);

            //ASSERT
            Assert.IsTrue(String.IsNullOrWhiteSpace(hydrant.SAPErrorCode));
            Assert.AreEqual(int.Parse(sapEquipmentId), hydrant.SAPEquipmentId);
        }

        #endregion

        #endregion

        #region Copy

        [TestMethod]
        public void TestCopySendsNotification()
        {
            _currentUser.IsAdmin = true;
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { Town = town, OperatingCenter = opc1, Abbreviation = "ZZ" });
            var hydrant = GetEntityFactory<Hydrant>().Create(new { Status = typeof(ActiveAssetStatusFactory), Town = town, OperatingCenter = opc1 });
            
            _target.Copy(hydrant.Id);

            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        #endregion

        #region SetHydrantOutOfService

        [TestMethod]
        public void TestSetHydrantOutOfServiceSendsNotifications()
        {
            var town = GetEntityFactory<Town>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new { Status = typeof(RetiredAssetStatusFactory), Town = town});
            var contact = GetEntityFactory<Contact>().CreateList(2,new { Email = "test@a.com" });
            var contactTypes = GetEntityFactory<ContactType>().CreateList(7);//new { Description = "Hydrant Out of Service"});
            town.TownContacts.Add(new TownContact { Contact = contact[0], Town = town, ContactType = contactTypes[6] });
            town.TownContacts.Add(new TownContact { Contact = contact[1], Town = town, ContactType = contactTypes[6] });
            Session.Flush();
            Session.Clear();
            var model = _viewModelFactory.BuildWithOverrides<MarkOutOfServiceHydrant, Hydrant>(hydrant,
                new {OutOfServiceDate = DateTime.Now});

            _target.SetHydrantOutOfService(model);

            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.AtLeast(3));
        }

        [TestMethod]
        public void TestSetHydrantBackInServiceSendsNotifications()
        {
            var town = GetEntityFactory<Town>().Create();
            var hydrant = GetEntityFactory<Hydrant>().Create(new {Status = typeof(RetiredAssetStatusFactory), Town = town });
            var contact = GetEntityFactory<Contact>().CreateList(2, new { Email = "test@a.com" });
            var contactTypes = GetEntityFactory<ContactType>().CreateList(7);//new { Description = "Hydrant Out of Service"});
            town.TownContacts.Add(new TownContact { Contact = contact[0], Town = town, ContactType = contactTypes[6] });
            GetEntityFactory<HydrantOutOfService>().Create(new { OutOfServiceDate = DateTime.Now.AddDays(-1), Hydrant = hydrant});
            Session.Flush();
            Session.Clear();

            var model = _viewModelFactory.BuildWithOverrides<MarkBackInServiceHydrant, Hydrant>(hydrant,
                new {BackInServiceDate = DateTime.Now});

            _target.SetHydrantBackInService(model);

            _noteServ.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.AtLeast(2));
        }

        #endregion

        #region GetHydrantPrefix

        [TestMethod]
        public void TestGetHydrantPrefixReturnsEmptyPrefixIfAllParametersAreNull()
        {
            var result = (JsonResult)_target.GetHydrantPrefix(null, null, null);
            dynamic data = result.Data;
            Assert.AreEqual(string.Empty, data["prefix"]);
        }

        [TestMethod]
        public void TestGetHydrantPrefixReturnsEmptyPrefixIfTownIdIsNotAValidTownId()
        {
            var result = (JsonResult)_target.GetHydrantPrefix(null, 0, null);
            dynamic data = result.Data;
            Assert.AreEqual(string.Empty, data["prefix"]);
        }

        [TestMethod]
        public void TestGetHydrantPrefixReturnsHydrantPrefixFromRepository()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc1, Town = town, Abbreviation = "ZZ" });
            var ts = GetFactory<TownSectionFactory>().Create();
            var expected = Repository.GenerateHydrantPrefix(opc1, town, ts, null);
            var result = (JsonResult)_target.GetHydrantPrefix(opc1.Id, town.Id, ts.Id);
            dynamic data = result.Data;
            Assert.AreEqual(expected, data["prefix"]);
        }

        #endregion

        #region Routes

        [TestMethod]
        public void TestRouteByTownIdReturnsRoutesInTown()
        {
            var towns = GetEntityFactory<Town>().CreateList(2);
            var hydrantValid = GetEntityFactory<Hydrant>().Create(new { Town = towns[0], Route = 1 });
            var hydrantInvalid = GetEntityFactory<Hydrant>().Create(new { Town = towns[1], Route = 2 });

            var result = (CascadingActionResult)_target.RouteByTownId(towns[0].Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(2, actual.Count()); // because the first is --select here--
            Assert.AreEqual(hydrantValid.Route.ToString(), actual.Last().Text);
        }

        [TestMethod]
        public void TestRouteByOperatingCenterIdAndOrTownIdReturnsRoutesInTown()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var towns = GetEntityFactory<Town>().CreateList(2);
            var hydrantValid = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = operatingCenter, Town = towns[0], Route = 1 });
            var hydrantInvalid = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = operatingCenter, Town = towns[1], Route = 2 });

            var result = (CascadingActionResult)_target.RouteByOperatingCenterIdAndOrTownId(operatingCenter.Id, null);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(3, actual.Count()); // because the first is --select here--

            result = (CascadingActionResult)_target.RouteByOperatingCenterIdAndOrTownId(operatingCenter.Id, towns[0].Id);
            actual = result.GetSelectListItems();

            Assert.AreEqual(2, actual.Count()); // because the first is --select here--
            Assert.AreEqual(hydrantValid.Route.ToString(), actual.Last().Text);
        }

        #endregion

        #region By Street/Town Id

        [TestMethod]
        public void TestHydrantsByTownIdReturnsHydrantInTown()
        {
            var towns = GetEntityFactory<Town>().CreateList(2);
            var hydrantValid = GetEntityFactory<Hydrant>().Create(new {Town = towns[0]});
            var hydrantInvalid = GetEntityFactory<Hydrant>().Create(new { Town = towns[1] });

            var results = (CascadingActionResult)_target.ByTownId(towns[0].Id);
            var actual = results.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count()-1);
            Assert.AreEqual(hydrantValid.Id.ToString(), actual.Last().Value);
            Assert.AreEqual(hydrantValid.HydrantNumber, actual.Last().Text);
        }

        [TestMethod]
        public void TestHydrantsByStreetIdReturnsHydrantInTown()
        {
            var streets = GetEntityFactory<Street>().CreateList(2);
            var hydrantValid = GetEntityFactory<Hydrant>().Create(new { Street = streets[0] });
            var hydrantInvalid = GetEntityFactory<Hydrant>().Create(new { Street = streets[1] });

            var results = (CascadingActionResult)_target.ByStreetId(streets[0].Id);
            var actual = results.GetSelectListItems().ToArray();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(hydrantValid.Id.ToString(), actual.Last().Value);
            Assert.AreEqual(hydrantValid.HydrantNumber, actual.Last().Text);
        }

        #endregion

        #region ValidateUnusedHydrantSuffix

        [TestMethod]
        public void TestValidateUnusedHydrantSuffixReturnsFalseIfHydrantSuffixValidIsNull()
        {
            var result = (JsonResult)_target.ValidateUnusedFoundHydrantSuffix(null, 1, 1, 1);
            Assert.IsFalse((bool)result.Data);
        }

        [TestMethod]
        public void TestValidateUnusedHydrantSuffixReturnsFalseIfTownIsNull()
        {
            var result = (JsonResult)_target.ValidateUnusedFoundHydrantSuffix(1, 1, null, 1);
            Assert.IsFalse((bool)result.Data);
        }

        [TestMethod]
        public void TestValidateUnusedHydrantSuffixReturnsFalseIfOperatingCenterIsNull()
        {
            var result = (JsonResult)_target.ValidateUnusedFoundHydrantSuffix(1, null, 1, 1);
            Assert.IsFalse((bool)result.Data);
        }

        [TestMethod]
        public void TestValidateUnusedHydrantSuffixReturnsTrueIfTheHydrantSuffixIsUnused()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc, Town = town, Abbreviation = "ZZ" });
            var hydrantOne = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc, Town = town, HydrantSuffix = 1, HydrantNumber = "HZZ-1" });
            var hydrantThree = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc, Town = town, HydrantSuffix = 3, HydrantNumber = "HZZ-3" });

            var result = (JsonResult)_target.ValidateUnusedFoundHydrantSuffix(2, opc.Id, town.Id, null);
            Assert.IsTrue((bool)result.Data);
        }

        [TestMethod]
        public void TestValidateUnusedHydrantSuffixReturnsErrorIfTheHydrantSuffixIsAlreadyInUse()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc, Town = town, Abbreviation = "ZZ" });

            var hydrantOne = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc, Town = town, HydrantSuffix = 1, HydrantNumber = "HZZ-1", Status = typeof(ActiveAssetStatusFactory) });
            var hydrantThree = GetFactory<HydrantFactory>().Create(new { OperatingCenter = opc, Town = town, HydrantSuffix = 3, HydrantNumber = "HZZ-3", Status = typeof(ActiveAssetStatusFactory) });

            var result = (JsonResult)_target.ValidateUnusedFoundHydrantSuffix(3, opc.Id, town.Id, null);
            Assert.AreEqual("A hydrant already exists with this hydrant suffix.", result.Data);
        }

        #endregion

        #region By OperatingCenter

        [TestMethod]
        public void TestGetByOperatingCenter()
        {
            var operatingCenterA = GetFactory<UniqueOperatingCenterFactory>().Create();
            var operatingCenterB = GetFactory<UniqueOperatingCenterFactory>().Create();
            var hydrantAInOperatingCenterA = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = operatingCenterA, HydrantNumber = "A" });
            var hydrantBInOperatingCenterB = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = operatingCenterB, HydrantNumber = "B" });
            var hydrantCInOperatingCenterB = GetEntityFactory<Hydrant>().Create(new { OperatingCenter = operatingCenterB, HydrantNumber = "C" });

            var actionResult = (CascadingActionResult)_target.ByOperatingCenter(operatingCenterA.Id);
            var selectListItems = actionResult.GetSelectListItems().ToList();

            Assert.AreEqual(2, selectListItems.Count);
            Assert.AreEqual(hydrantAInOperatingCenterA.DescriptionWithStatus, selectListItems[1].Text);

            actionResult = (CascadingActionResult)_target.ByOperatingCenter(operatingCenterB.Id);
            selectListItems = actionResult.GetSelectListItems().ToList();

            Assert.AreEqual(3, selectListItems.Count);
            Assert.AreEqual(hydrantBInOperatingCenterB.DescriptionWithStatus, selectListItems[1].Text);
            Assert.AreEqual(hydrantCInOperatingCenterB.DescriptionWithStatus, selectListItems[2].Text);

            actionResult = (CascadingActionResult)_target.ByOperatingCenter(operatingCenterA.Id, operatingCenterB.Id);
            selectListItems = actionResult.GetSelectListItems().ToList();

            Assert.AreEqual(4, selectListItems.Count);
            Assert.AreEqual(hydrantAInOperatingCenterA.DescriptionWithStatus, selectListItems[1].Text);
            Assert.AreEqual(hydrantBInOperatingCenterB.DescriptionWithStatus, selectListItems[2].Text);
            Assert.AreEqual(hydrantCInOperatingCenterB.DescriptionWithStatus, selectListItems[3].Text);
        }

        #endregion

        #endregion

        #region Test classes

        // This has to be named exactly the same as the base controller or else some tests involving RouteContexts start failing.
        public class HydrantController : MapCallMVC.Areas.FieldOperations.Controllers.HydrantController
        {
            public ReplaceHydrant LastReplaceHydrantVM { get; set; }

            public HydrantController(ControllerBaseWithPersistenceArguments<IHydrantRepository, Hydrant, User> args) : base(args) { }

            protected override ReplaceHydrant GetReplaceHydrantModel()
            {
                LastReplaceHydrantVM = base.GetReplaceHydrantModel();
                return LastReplaceHydrantVM;
            }
        }

        #endregion
    }
}