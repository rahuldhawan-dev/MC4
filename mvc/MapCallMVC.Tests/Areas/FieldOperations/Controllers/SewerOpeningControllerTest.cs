using System;
using System.Collections.Generic;
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
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class SewerOpeningControllerTest : MapCallMvcControllerTestBase<SewerOpeningControllerTest.SewerOpeningController, SewerOpening>
    {
        #region Fields

        private AssetStatus _activeStatus, _retiredStatus, _pendingStatus, _removedStatus, _cancelledStatus;
        private User _user;
        private Mock<ISAPEquipmentRepository> _sapRepository;
        private Mock<INotificationService> _notifier;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _notifier = e.For<INotificationService>().Mock();
        }

        protected override User CreateUser()
        {
            return _user = GetFactory<AdminUserFactory>().Create();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _sapRepository = new Mock<ISAPEquipmentRepository>();
            _container.Inject(_sapRepository.Object);

            // These need to exist
            _activeStatus = GetFactory<ActiveAssetStatusFactory>().Create();
            _retiredStatus = GetFactory<RetiredAssetStatusFactory>().Create();
            _removedStatus = GetFactory<RemovedAssetStatusFactory>().Create();
            _cancelledStatus = GetFactory<CancelledAssetStatusFactory>().Create();
            _pendingStatus = GetFactory<PendingAssetStatusFactory>().Create();
            GetFactory<SewerOpeningAssetTypeFactory>().Create();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateSewerOpening)vm;
                var opc = GetEntityFactory<OperatingCenter>().Create();
                var town = GetEntityFactory<Town>().Create();
                town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc, Town = town, Abbreviation = "FO" });
                model.OperatingCenter = opc.Id;
                model.Town = town.Id;
            };
            options.InitializeSearchTester = (tester) => {
                // Ignore Route because the search tester thinks it's an entity due to it
                // being a dropdown, but it is a literal dropdown for integers instead of entity ids.
                tester.IgnoredPropertyNames.Add(nameof(SearchSewerOpening.Route));
            };
        }

        #endregion

        #region Authorization

        [TestMethod]        
        public override void TestControllerAuthorization()
        {
            var role = SewerOpeningController.ROLE;

            Authorization.Assert(a => {
                a.RequiresRole("~/FieldOperations/SewerOpening/Search/", role);
                a.RequiresRole("~/FieldOperations/SewerOpening/Show/", role);
                a.RequiresRole("~/FieldOperations/SewerOpening/Index/", role);
                a.RequiresRole("~/FieldOperations/SewerOpening/New/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/SewerOpening/Create/", role, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/SewerOpening/Edit/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/SewerOpening/Update/", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/SewerOpening/Replace/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SewerOpening/Copy/", role, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/SewerOpening/AddSewerOpeningSewerOpeningConnection", role, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/SewerOpening/RemoveSewerOpeningSewerOpeningConnection", role, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/FieldOperations/SewerOpening/Destroy/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/SewerOpening/RouteByTownId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/SewerOpening/ByTownId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/SewerOpening/ByStreetId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/SewerOpening/ByStreetIdForWorkOrders/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/SewerOpening/ActiveByTownId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/SewerOpening/ByPartialSewerOpeningMatchByTown/");
            });
        }

        #endregion

        #region Show

        [TestMethod]
        public void TestShowSetsArcCollectorUrlInViewData()
        {
            var entity = GetFactory<SewerOpeningFactory>().Create(new {OpeningNumber = "MAN-101"});

            _target.Show(entity.Id);

            MyAssert.StringsAreEqual(
                string.Format(ArcCollectorLinkGenerator.ARCGIS_COLLECTOR_HTML_FORMAT_STRING,
                    string.Format(
                        ArcCollectorLinkGenerator.ARCGIS,
                        ArcCollectorLinkGenerator.ARCGIS_COLLECTOR_URL,
                        entity.OperatingCenter.ArcMobileMapId,
                        string.Empty,
                        $"%7B%22MapCall_ID%22:%221%22,%22StructureID%22:%22MAN-101%22,%22Diameter%22:%22%22,%22LifeCycleStatus%22:%221%22,%22SAPID%22:%22000000000000123456%22%7D")),
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
                        $"%7B%22MapCall_ID%22:%221%22,%22StructureID%22:%22MAN-101%22,%22Diameter%22:%22%22,%22LifeCycleStatus%22:%221%22,%22SAPID%22:%22000000000000123456%22%7D")),
                _target.ViewData["ArcCollectorLink"].ToString());
        }

        [TestMethod]
        public void TestShowRespondsToMap()
        {
            var sm1 = GetEntityFactory<SewerOpening>().Create();
            InitializeControllerAndRequest("~/FieldOperations/SewerOpening/Show/" + sm1.Id + ".map");

            var result = (MapResult)_target.Show(sm1.Id);
            var resultModel = result.CoordinateSets.Single().Coordinates.ToArray();

            Assert.AreEqual(1, resultModel.Count());
            Assert.IsTrue(resultModel.Contains(sm1));
        }

        [TestMethod]
        public void TestShowRespondsToFrag()
        {
            var sm1 = GetEntityFactory<SewerOpening>().Create();
            InitializeControllerAndRequest("~/FieldOperations/SewerOpening/Show/" + sm1.Id + ".frag");

            var result = _target.Show(sm1.Id) as PartialViewResult;

            MvcAssert.IsViewNamed(result, "_ShowPopup");
            MvcAssert.IsViewWithModel(result, sm1);
        }
        
        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<SewerOpening>().Create(new {OpeningNumber = "description 0"});
            var entity1 = GetEntityFactory<SewerOpening>().Create(new {OpeningNumber = "description 1"});
            var search = new SearchSewerOpening();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.OpeningNumber, "OpeningNumber");
                helper.AreEqual(entity1.OpeningNumber, "OpeningNumber", 1);
            }
        }

        [TestMethod]
        public void TestIndexRespondsToMapWithExpectedModels()
        {
            InitializeControllerAndRequest("~/FieldOperations/SewerOpening/Index.map");
            var sm1 = GetEntityFactory<SewerOpening>().Create();

            var result = _target.Index(new SearchSewerOpening()) as AssetMapResult;
            var resultModel = result.CoordinateSets.Single().Coordinates.First();

            Assert.AreEqual(sm1.Id, resultModel.Id);
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

            var model = _viewModelFactory.Build<CreateSewerOpening, SewerOpening>( GetEntityFactory<SewerOpening>().BuildWithConcreteDependencies(new { OperatingCenter = opCntr, Town = town }));

            _target.Create(model);

            // FAILS BECAUSE WE DID NOT INJECT AN SAPRepository
            var sewerOpening = Repository.Find(model.Id);
            Assert.IsNotNull(sewerOpening);
            Assert.IsTrue(sewerOpening.SAPErrorCode.StartsWith(SewerOpeningController.SAP_UPDATE_FAILURE));
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
            var model = _viewModelFactory.Build<CreateSewerOpening, SewerOpening>( GetEntityFactory<SewerOpening>().BuildWithConcreteDependencies(new { OperatingCenter = opCntr, Town = town }));

            //ACT
            _target.Create(model);
            var sewerOpening = Repository.Find(model.Id);

            //ASSERT
            Assert.AreEqual(string.Empty, sewerOpening.SAPErrorCode);
            Assert.AreEqual(int.Parse(sapEquipment.SAPEquipmentNumber), sewerOpening.SAPEquipmentId);
        }

        #endregion

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<SewerOpening>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditSewerOpening, SewerOpening>(eq, new {
                OpeningNumber = expected
            }));

            Assert.AreEqual(expected, Session.Get<SewerOpening>(eq.Id).OpeningNumber);
        }

        [TestMethod]
        public void TestEditDoesNotThrowExceptionWhenOpeningStatusIsNull()
        {
            var eq = GetEntityFactory<SewerOpening>().Create();

            eq.Status = null;

            var result = _target.Edit(eq.Id) as ViewResult;
            var sewerOpening = (EditSewerOpening)result.Model;

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.AreEqual(eq.Id, sewerOpening.Id);
            Assert.AreEqual(eq.Status, sewerOpening.Status);
        }

        #region SAP Syncronization

        [TestMethod]
        public void TestUpdateCallsSAPRepositorySaveAndRecordsErrorCodeUponFailure()
        {
            _user.IsAdmin = true;
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });

            var model = _viewModelFactory.Build<EditSewerOpening, SewerOpening>( GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr, Town = town }));

            _target.Update(model);

            // FAILS BECAUSE WE DID NOT INJECT AN SAPRepository
            var sewerOpening = Repository.Find(model.Id);
            Assert.IsTrue(sewerOpening.SAPErrorCode.StartsWith(SewerOpeningController.SAP_UPDATE_FAILURE));
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
            var sapEquipment = new SAPEquipment { SAPErrorCode = string.Empty, SAPEquipmentNumber = sapEquipmentId };
            _sapRepository.Setup(x => x.Save(It.IsAny<SAPEquipment>())).Returns(sapEquipment);
            var entity = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr, Town = town, SAPEquipmentId = int.Parse(sapEquipmentId) });
            var model = _viewModelFactory.Build<EditSewerOpening, SewerOpening>( entity);

            //ACT
            _target.Update(model);
            var sewerOpening = Repository.Find(model.Id);

            //ASSERT
            Assert.IsTrue(string.IsNullOrWhiteSpace(sewerOpening.SAPErrorCode));
            Assert.AreEqual(int.Parse(sapEquipmentId), sewerOpening.SAPEquipmentId);
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
            var entity = GetEntityFactory<SewerOpening>().Create(new { OperatingCenter = opCntr, Town = town, SAPEquipmentId = 0 });
            var model = _viewModelFactory.Build<EditSewerOpening, SewerOpening>( entity);

            //ACT
            _target.Update(model);
            var sewerOpening = Repository.Find(model.Id);

            //ASSERT
            Assert.IsTrue(string.IsNullOrWhiteSpace(sewerOpening.SAPErrorCode));
            Assert.AreEqual(int.Parse(sapEquipmentId), sewerOpening.SAPEquipmentId);
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

            var existingOpening = GetFactory<SewerOpeningFactory>().Create(new { OperatingCenter = sapOPC, Status = typeof(ActiveAssetStatusFactory) });
            Assert.IsTrue(existingOpening.OperatingCenter.CanSyncWithSAP, "Sanity");
            Assert.AreNotSame(pending, existingOpening.Status, "Sanity");

            var existingSewerMainCleaning1 = GetFactory<SewerMainCleaningFactory>().Create(new
            {
                Opening1 = existingOpening
            });
            existingOpening.SewerMainCleanings1.Add(existingSewerMainCleaning1);
            var existingSewerMainCleaning2 = GetFactory<SewerMainCleaningFactory>().Create(new
            {
                Opening2 = existingOpening
            });
            existingOpening.SewerMainCleanings2.Add(existingSewerMainCleaning2);

            _target.Replace(existingOpening.Id);

            var lastReplacementModel = _target.LastReplaceSewerOpeningVM;

            Assert.AreSame(requestRetirement, existingOpening.Status, "The existing openings's status must be set to REQUEST RETIREMENT");
            Assert.AreNotEqual(existingOpening.Id, lastReplacementModel.Id, "The Id must be the REPLACEMENT hydrant's id.");

            var replacement = Repository.Find(lastReplacementModel.Id);

            // Fields that are copied from the existing hydrant.
            Assert.AreSame(existingOpening.OperatingCenter, replacement.OperatingCenter);
            Assert.AreSame(existingOpening.Town, replacement.Town);
            Assert.AreSame(existingOpening.TownSection, replacement.TownSection);
            Assert.AreEqual(existingOpening.StreetNumber, replacement.StreetNumber);
            Assert.AreSame(existingOpening.Street, replacement.Street);
            Assert.AreSame(existingOpening.IntersectingStreet, replacement.IntersectingStreet);
            Assert.AreEqual(existingOpening.DistanceFromCrossStreet, replacement.DistanceFromCrossStreet);
            Assert.AreEqual(existingOpening.Route, replacement.Route);
            Assert.AreEqual(existingOpening.Stop, replacement.Stop);
            Assert.AreEqual(existingOpening.MapPage, replacement.MapPage);
            Assert.AreEqual(existingOpening.OpeningNumber, replacement.OpeningNumber);
            Assert.AreEqual(existingOpening.OpeningSuffix, replacement.OpeningSuffix);
            Assert.AreEqual(existingOpening.Critical, replacement.Critical);
            Assert.AreEqual(existingOpening.CriticalNotes, replacement.CriticalNotes);
            Assert.AreSame(existingOpening.WasteWaterSystem, replacement.WasteWaterSystem);

            // Fields that are not copied from the existing hydrant.
            Assert.AreSame(pending, replacement.Status);
            Assert.AreSame(_user, replacement.CreatedBy);
            Assert.AreEqual(expectedDate, replacement.CreatedAt);

            Assert.AreNotEqual(existingOpening.Coordinate.Id, replacement.Coordinate.Id, "A new Coordinate record must be created.");
            Assert.AreEqual(existingOpening.Coordinate.Latitude, replacement.Coordinate.Latitude);
            Assert.AreEqual(existingOpening.Coordinate.Longitude, replacement.Coordinate.Longitude);
            Assert.AreSame(existingOpening.Coordinate.Icon, replacement.Coordinate.Icon);

            // Ensure the main cleanings have been copied to new records.
            var replacementCleaning1 = replacement.SewerMainCleanings1.Single();
            Assert.AreNotEqual(existingSewerMainCleaning1.Id, replacementCleaning1.Id, "A new inspection record must be created.");
            Assert.AreSame(replacement, replacementCleaning1.Opening1, "The new record must belong to the new replacement hydrant.");
            var replacementCleaning2 = replacement.SewerMainCleanings2.Single();
            Assert.AreNotEqual(existingSewerMainCleaning2.Id, replacementCleaning2.Id, "A new inspection record must be created.");
            Assert.AreSame(replacement, replacementCleaning2.Opening2, "The new record must belong to the new replacement hydrant.");
        }

        [TestMethod]
        public void TestReplaceReturns404IfHydrantDoesNotExist()
        {
            MvcAssert.IsNotFound(_target.Replace(0));
        }

        [TestMethod]
        public void TestReplaceReturns404IfHydrantIsNotActive()
        {
            var inactive = GetFactory<HydrantFactory>().Create(new
            {
               Status = typeof(PendingAssetStatusFactory)
            });

            MvcAssert.IsNotFound(_target.Replace(inactive.Id));
        }

        #endregion

        #region Notification

        [TestMethod]
        public void TestCreateSendsNotificationEmailForCorrectAssetStatuses()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc1, Town = town, Abbreviation = "XX" });
            var ent = GetEntityFactory<SewerOpening>().Create();
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var model = _viewModelFactory.BuildWithOverrides<CreateSewerOpening, SewerOpening>(ent, new {Town = town.Id, OperatingCenter = opc1.Id });

            foreach (var Status in new[] { _activeStatus.Id, _retiredStatus.Id })
            {
                model.Id = 0;
                model.Status = Status;
                NotifierArgs resultArgs = null;
                _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

                var result = _target.Create(model);
                var entity = Repository.Find(model.Id);

                Assert.AreSame(entity, ((SewerOpeningNotification)resultArgs.Data).SewerOpening);
                Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
                Assert.AreEqual(SewerOpeningController.ROLE, resultArgs.Module);
                Assert.AreEqual(SewerOpeningController.NOTIFICATION_PURPOSE, resultArgs.Purpose);
                Assert.IsNull(resultArgs.Subject);
            }
        }

        [TestMethod]
        public void TestCreateDoesNotSendNotificationForInvalidAssetStatus()
        {
            var ent = GetEntityFactory<SewerOpening>().Create();
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc1, Town = town, Abbreviation = "XX" });

            var model = _viewModelFactory.BuildWithOverrides<CreateSewerOpening, SewerOpening>(ent, new {Status = _pendingStatus.Id, Town = town.Id, OperatingCenter = opc1.Id });
            model.Id = 0;

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            var result = _target.Create(model);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
            Assert.IsNull(resultArgs);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenSwitchedToActive()
        {
            var entity = GetEntityFactory<SewerOpening>().Create(new { Status = _pendingStatus });
            var model = _viewModelFactory.BuildWithOverrides<EditSewerOpening, SewerOpening>(entity, new { Status = _activeStatus.Id });

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);
            entity = Repository.Find(model.Id);

            Assert.AreSame(entity, ((SewerOpeningNotification)resultArgs.Data).SewerOpening);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenSwitchedFromActive()
        {
            var entity = GetEntityFactory<SewerOpening>().Create(new { Status = _activeStatus });
            var model = _viewModelFactory.BuildWithOverrides<EditSewerOpening, SewerOpening>(entity, new { Status = _pendingStatus.Id });


            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);
            entity = Repository.Find(model.Id);

            Assert.AreSame(entity, ((SewerOpeningNotification)resultArgs.Data).SewerOpening);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenSwitchedToRetired()
        {
            var entity = GetEntityFactory<SewerOpening>().Create(new { Status = _pendingStatus  });
            var model = _viewModelFactory.BuildWithOverrides<EditSewerOpening, SewerOpening>(entity, new { Status = _retiredStatus.Id });

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);
            entity = Repository.Find(model.Id);

            Assert.AreSame(entity, ((SewerOpeningNotification)resultArgs.Data).SewerOpening);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenSwitchedFromRetired()
        {
            var entity = GetEntityFactory<SewerOpening>().Create(new { Status = _retiredStatus });
            var model = _viewModelFactory.BuildWithOverrides<EditSewerOpening, SewerOpening>(entity, new { Status = _pendingStatus.Id });

            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);
            entity = Repository.Find(model.Id);

            Assert.AreSame(entity, ((SewerOpeningNotification)resultArgs.Data).SewerOpening);
        }

        [TestMethod]
        public void TestCopySendsNotificationEmailWhenActive()
        {
            _user.IsAdmin = true;
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opc1, Town = town, Abbreviation = "XX" });
            var SewerOpening = GetEntityFactory<SewerOpening>().Create(new { Town = town, OperatingCenter = opc1, Status = _activeStatus});

            _target.Copy(SewerOpening.Id);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()));
        }

        #endregion

        #region Add/Remove Sewer Opening Connections

        [TestMethod]
        public void TestAddSewerOpeningConnectionAddsASewerOpeningConnectionToTheCorrectCollection()
        {
            var entity = GetEntityFactory<SewerOpening>().Create();
            var downstreamOpening = GetEntityFactory<SewerOpening>().Create();
            var downstreamModel = new AddSewerOpeningConnection(_container)
                { Id = entity.Id, ConnectedOpening = downstreamOpening.Id, ConnectedOpeningDownstream = true };
            var upstreamModel = new AddSewerOpeningConnection(_container)
                { Id = entity.Id, ConnectedOpening = downstreamOpening.Id, ConnectedOpeningDownstream = false };

            _target.RunModelValidation(downstreamModel);
            MyAssert.CausesIncrease(
                () => _target.AddSewerOpeningSewerOpeningConnection(downstreamModel), 
                () => Repository.Find(entity.Id).DownstreamSewerOpeningConnections.Count);

            _target.ModelState.Clear();
            _target.RunModelValidation(upstreamModel);
            MyAssert.DoesNotCauseIncrease(
                () => _target.AddSewerOpeningSewerOpeningConnection(upstreamModel),
                () => Repository.Find(entity.Id).UpstreamSewerOpeningConnections.Count);
        }

        [TestMethod]
        public void TestRemoveSewerOpeningConnectionRemovesTheSewerOpening()
        {
            var sm1 = GetEntityFactory<SewerOpening>().Create();
            var sm2 = GetEntityFactory<SewerOpening>().Create();
            _target.AddSewerOpeningSewerOpeningConnection(new AddSewerOpeningConnection(_container) { 
                Id = sm1.Id,
                ConnectedOpening = sm2.Id,
                ConnectedOpeningDownstream = true
            });
            Session.Clear();

            MyAssert.CausesDecrease(() =>
                _target.RemoveSewerOpeningSewerOpeningConnection(new RemoveSewerOpeningSewerOpeningConnection(_container) { 
                    Id = sm1.Id,
                    SewerOpeningSewerOpeningConnectionId = sm1.DownstreamSewerOpeningConnections.First().Id
                }),
                () => Repository.Find(sm1.Id).DownstreamSewerOpeningConnections.Count);
        }

        [TestMethod]
        public void TestRemoveSewerOpeningConnectionRemovesTheUpstreamSewerOpening()
        {
            var sm1 = GetEntityFactory<SewerOpening>().Create();
            var sm2 = GetEntityFactory<SewerOpening>().Create();

            _target.AddSewerOpeningSewerOpeningConnection(new AddSewerOpeningConnection
(_container) {
                Id = sm1.Id,
                ConnectedOpening = sm2.Id,
                ConnectedOpeningDownstream = false
            });
            Session.Clear();

            MyAssert.CausesDecrease(() =>
                _target.RemoveSewerOpeningSewerOpeningConnection(new RemoveSewerOpeningSewerOpeningConnection
(_container) {
                    Id = sm1.Id,
                    SewerOpeningSewerOpeningConnectionId = sm1.UpstreamSewerOpeningConnections.First().Id
                }),
                () => Repository.Find(sm1.Id).UpstreamSewerOpeningConnections.Count);
        }

        #endregion

        #region Cascades

        [TestMethod]
        public void TestRouteByTownIdReturnsRoutesInTown()
        {
            var towns = GetEntityFactory<Town>().CreateList(2);
            var openingValid = GetEntityFactory<SewerOpening>().Create(new {Town = towns[0], Route = 1});
            var openingBad = GetEntityFactory<SewerOpening>().Create(new {Town = towns[1], Route = 2});

            var result = (CascadingActionResult)_target.RouteByTownId(towns[0].Id);
            var actual = result.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(openingValid.Route.ToString(), actual.Last().Text);
        }

        [TestMethod]
        public void TestSewerOpeningsByTownIdReturnsOpeningsInTown()
        {
            var towns = GetEntityFactory<Town>().CreateList(2);
            var openingValid = GetEntityFactory<SewerOpening>().Create(new { Town = towns[0] });
            var openingBad = GetEntityFactory<SewerOpening>().Create(new { Town = towns[1] });

            var results = (CascadingActionResult)_target.ByTownId(towns[0].Id);
            var actual = results.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(openingValid.Id.ToString(), actual.Last().Value);
            Assert.AreEqual(openingValid.OpeningNumber, actual.Last().Text);
        }

        [TestMethod]
        public void TestSewerOpeningsActiveByTownIdReturnsOpeningsInTown()
        {
            var assetStatus1 = GetFactory<ActiveAssetStatusFactory>().Create();
            var assetStatus2 = GetFactory<PendingAssetStatusFactory>().Create();
            var towns = GetEntityFactory<Town>().CreateList(2);
            var openingValid = GetEntityFactory<SewerOpening>().Create(new { Town = towns[0], Status = assetStatus1 });
            var openingBad1 = GetEntityFactory<SewerOpening>().Create(new { Town = towns[1], Status = assetStatus1});
            var openingBad2 = GetEntityFactory<SewerOpening>().Create(new { Town = towns[0], Status = assetStatus2 });

            var results = (CascadingActionResult)_target.ActiveByTownId(towns[0].Id);
            var actual = results.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(openingValid.Id.ToString(), actual.Last().Value);
            Assert.AreEqual(openingValid.OpeningNumber, actual.Last().Text);
        }

        #endregion

        [TestMethod]
        public void TestSewerOpeningsByStreetIdReturnsOpeningsOnStreet()
        {
            var streets = GetEntityFactory<Street>().CreateList(2);
            var openingValid = GetEntityFactory<SewerOpening>().Create(new { Street = streets[0] });
            var openingBad = GetEntityFactory<SewerOpening>().Create(new { Street = streets[1] });

            var results = (CascadingActionResult)_target.ByStreetId(streets[0].Id);
            var actual = results.GetSelectListItems();

            Assert.AreEqual(1, actual.Count() - 1);
            Assert.AreEqual(openingValid.Id.ToString(), actual.Last().Value);
            Assert.AreEqual(openingValid.OpeningNumber, actual.Last().Text);
        }

        #region ByPartialSewerOpeningMatchByTown

        [TestMethod]
        public void TestByPartialSewerOpeningMatchByTownReturnsAutoCompletelist()
        {
            var town = GetEntityFactory<Town>().Create();
            var sewerList = GetEntityFactory<SewerOpening>().CreateList(20, new { Town = town });
            var sewer = GetEntityFactory<SewerOpening>().Create(new { Town = town, OpeningNumber = "ABC123" });
            var result = (AutoCompleteResult)_target.ByPartialSewerOpeningMatchByTown("ABC", sewer.Town.Id);
            var resultList = (IEnumerable<SewerOpening>)result.Data;

            Assert.AreEqual(1, resultList.Count());
            Assert.AreEqual(sewer.OpeningNumber, resultList.First().ToString());
        }

        [TestMethod]
        public void TestByPartialSewerOpeningMatchByTownReturnsNoResultsWhenTownDoesNotMatch()
        {
            var town = GetEntityFactory<Town>().Create();
            var town2 = GetEntityFactory<Town>().Create();
            var sewerList = GetEntityFactory<SewerOpening>().CreateList(20, new { Town = town2 });
            var sewer = GetEntityFactory<SewerOpening>().Create(new { Town = town, OpeningNumber = "ABC123" });
            var result = (AutoCompleteResult)_target.ByPartialSewerOpeningMatchByTown("ABC", town2.Id);
            var resultList = (IEnumerable<SewerOpening>)result.Data;

            Assert.AreEqual(0, resultList.Count());
        }

        [TestMethod]
        public void TestByPartialSewerOpeningMatchByTownReturnsALimitedSetOfResults()
        {
            var expectedMaxResults = MapCallMVC.Areas.FieldOperations.Controllers.SewerOpeningController.MAX_AUTOCOMPLETE_RESULTS;
            var town = GetEntityFactory<Town>().Create();
            var sewerOpenings = new List<SewerOpening>();
            for (var i = 0; i < expectedMaxResults + 1; i++)
            {
                var sewer = GetEntityFactory<SewerOpening>().Create(new { Town = town, OpeningNumber = $"ABC{i}" });
                sewerOpenings.Add(sewer);
            }

            var result = (AutoCompleteResult)_target.ByPartialSewerOpeningMatchByTown("ABC", town.Id);
            var resultList = (IEnumerable<SewerOpening>)result.Data;

            Assert.AreEqual(expectedMaxResults, resultList.Count());
        }

        #endregion

        #region Lookups

        [TestMethod]
        public void TestSewerDropDownLookupForNewSetsActiveOC()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {IsActive = true});
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new {IsActive = false});

            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            _target.SetLookupData(ControllerAction.New);

            var opcDropDownData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            // Need to filter out _user default OC since it created a active OC when creating the _user object

            var DropDownData = opcDropDownData.Where(x => x.Value != _user.DefaultOperatingCenter.Id.ToString());

            Assert.AreEqual(1, DropDownData.Count());
            Assert.AreEqual(opc1.Id.ToString(), DropDownData.First().Value);
            Assert.AreNotEqual(opc2.Id.ToString(), DropDownData.First().Value);

        }

        #endregion

        #region Test classes

        // This has to be named exactly the same as the base controller or else some tests involving RouteContexts start failing.
        public class SewerOpeningController : MapCallMVC.Areas.FieldOperations.Controllers.SewerOpeningController
        {
            public ReplaceSewerOpening LastReplaceSewerOpeningVM { get; set; }

            public SewerOpeningController(ControllerBaseWithPersistenceArguments<ISewerOpeningRepository, SewerOpening, User> args) : base(args) { }

            protected override ReplaceSewerOpening GetReplaceSewerOpeningModel()
            {
                LastReplaceSewerOpeningVM = base.GetReplaceSewerOpeningModel();
                return LastReplaceSewerOpeningVM;
            }
        }

        #endregion
    }
}
