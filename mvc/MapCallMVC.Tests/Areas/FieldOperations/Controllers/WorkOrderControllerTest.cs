using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Data.NHibernate;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkOrderControllerTest : MapCallMvcControllerTestBase<WorkOrderController, WorkOrder, WorkOrderRepository>
    {
        #region Fields

        private RepositoryBase<Coordinate> _coordinateRepo;
        private IconSetRepository _iconSetRepo;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<INotificationService>().Use((_notificationService = new Mock<INotificationService>()).Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _coordinateRepo = _container.GetInstance<RepositoryBase<Coordinate>>();
            _iconSetRepo = _container.GetInstance<IconSetRepository>();

            _container.Inject<IWorkOrderRepository>(Repository);
            _container.Inject<IRepository<Coordinate>>(_coordinateRepo);
            _container.Inject<IIconSetRepository>(_iconSetRepo);

            var defaultIcon = GetFactory<MapIconFactory>().Create(new { FileName = "pin_black" });
            GetFactory<WorkDescriptionFactory>().CreateAll();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => {
                var opc = GetEntityFactory<OperatingCenter>().Create();
                AddWorkManagementRoleToCurrentUserForOperatingCenter(opc);
                return GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = opc });
            };
            options.InitializeCreateViewModel = (vm) => {
                var model = (CreateWorkOrder)vm;
                model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
                model.CustomerName = "Some Guy";
                model.IsRevisit = false;
                model.PremiseNumber = "1234567890";
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditWorkOrder)vm;
                model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
                model.IsRevisit = false;
                model.PremiseNumber = "1234567890";
            };
            options.UpdateRedirectsToRouteOnSuccessArgs = (vm) => {
                var model = (EditWorkOrder)vm;
                return new { action = "Edit", controller = "GeneralWorkOrder", id = model.Id };
            };
        }

        #endregion

        #region Private Methods

        private void AddWorkManagementRoleToCurrentUserForOperatingCenter(OperatingCenter opc)
        {
            var role = GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, opc, _currentUser, RoleActions.UserAdministrator);
        }        

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresLoggedInUserOnly("~/FieldOperations/WorkOrder/FindByPartialWorkOrderIDMatch/");
                a.RequiresRole("~/FieldOperations/WorkOrder/Show/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/WorkOrder/History/", module);
                a.RequiresRole("~/FieldOperations/WorkOrder/New/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/WorkOrder/Create/", module, RoleActions.Add);
                a.RequiresRole("~/FieldOperations/WorkOrder/Edit/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkOrder/Update/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkOrder/CompleteMaterialPlanning/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/WorkOrder/Cancel/", module, RoleActions.Edit);
                a.RequiresLoggedInUserOnly("~/FieldOperations/WorkOrder/ByTownIdForServices/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/WorkOrder/ByTownIdForMainBreaks/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/WorkOrder/ByTownId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/WorkOrder/FindBySAPWorkOrderNumber/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/WorkOrder/ByWorkOrderId/");
                a.RequiresLoggedInUserOnly("~/FieldOperations/WorkOrder/GetAccountingCode/");
            });
        }

        #endregion

        [TestMethod]
        public void TestFindByPartialWorkOrderIDReturnsJsonResultWithJsonResults()
        {
            var workorder = GetFactory<WorkOrderFactory>().Create();
            var invalid = GetFactory<WorkOrderFactory>().Create();

            var result = (AutoCompleteResult)_target.FindByPartialWorkOrderIDMatch(workorder.Id.ToString());
            var model = (IEnumerable<dynamic>)result.Data;
            Assert.AreSame(workorder, model.Single());
            Assert.AreEqual(1, model.Count());
        }

        [TestMethod]
        public void TestFindBySAPWorkOrderNumberReturnsSuccessFalseIfParameterIsNull()
        {
            var result = (JsonResult)_target.FindBySAPWorkOrderNumber(null);
            dynamic data = result.Data;

            Assert.IsFalse(data.success);
        }

        [TestMethod]
        public void TestFindBySAPWorkOrderNumberReturnsSuccessFalseIfNoMatch()
        {
            var result = (JsonResult)_target.FindBySAPWorkOrderNumber(2135);
            dynamic data = result.Data;

            Assert.IsFalse(data.success);
        }

        [TestMethod]
        public void TestFindBySAPWorkOrderNumberReturnsExpectedSerializedData()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create(new { SAPWorkOrderNumber = (long)1234567890 });
            var result = (JsonResult)_target.FindBySAPWorkOrderNumber(workOrder.SAPWorkOrderNumber);

            var data = (Dictionary<string, object>)result.Data;
            Assert.AreEqual(true, data["success"]);
            Assert.AreEqual(workOrder.OperatingCenter.Id, data["operatingCenterId"]);
            Assert.AreEqual(workOrder.Id, data["workOrderId"]);
            Assert.AreEqual(workOrder.StreetAddress + ", " + workOrder.TownAddress, data["address"]);

            var coordId = (int)data["coordinateId"];
            var coord = _coordinateRepo.Find(coordId);
            Assert.AreEqual(coord.Latitude, Convert.ToDecimal(workOrder.Latitude));
            Assert.AreEqual(coord.Longitude, Convert.ToDecimal(workOrder.Longitude));
        }

        [TestMethod]
        public void TestFindByMapCallWorkOrderNumberReturnsExpectedSerializedData()
        {
            var workDescription = GetFactory<ServiceLineRepairWorkDescriptionFactory>().Create();
            var workOrder = GetFactory<WorkOrderFactory>().Create(new { WorkDescription = workDescription });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(workOrder.OperatingCenter);
            var result = (JsonResult)_target.ByWorkOrderId(new SearchWorkOrderId { WorkOrderId = workOrder.Id });

            var data = (Dictionary<string, object>)result.Data;

            Assert.AreEqual(true, data["success"]);
            Assert.AreEqual(workOrder.Id, data["workOrderId"]);
            Assert.AreEqual(workOrder.OperatingCenter.Id, data["operatingCenterId"]);
            Assert.AreEqual(workOrder.StreetAddress + " " + workOrder.TownAddress, data["location"]);
            Assert.AreEqual(workOrder.Latitude, data["latitude"]);
            Assert.AreEqual(workOrder.Longitude, data["longitude"]);
            Assert.AreEqual(workDescription.Description, data["description"]);

            //var coordId = (int)data["coordinateId"];
            //var coord = _coordinateRepo.Find(coordId);
            //Assert.AreEqual(coord.Latitude, Convert.ToDecimal(workOrder.Latitude));
            //Assert.AreEqual(coord.Longitude, Convert.ToDecimal(workOrder.Longitude));
        }

        [TestMethod]
        public override void TestShowReturnsNotFoundIfRecordCanNotBeFound()
        {
            // noop: This doesn't happen due to the redirect. 271 handles not found.
        }

        [TestMethod]
        public override void TestShowReturnsShowViewWhenRecordIsFound()
        {
            // noop: This redirects instead.
        }

        [TestMethod]
        public void TestGetAccountingCodeReturns404IfNoWorkOrders()
        {
            var result = _target.GetAccountingCode(666) as HttpNotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestGetAccountingCodeReturnsAccountingCodeForWorkOrder()
        {
            var wo = GetEntityFactory<WorkOrder>().Create(new {AccountCharged = "1232221"});
            AddWorkManagementRoleToCurrentUserForOperatingCenter(wo.OperatingCenter);

            var result = _target.GetAccountingCode(wo.Id) as JsonResult;
            dynamic data = result.Data;

            Assert.AreEqual(wo.AccountCharged, data.accountingCode);
        }

        [TestMethod]
        public void TestGetByTownIdForMainBreaksReturnsByTownIdForMainBreaks()
        {
            //Arrange
            var town = GetEntityFactory<Town>().Create();
            var mainBreakRepairWorkDescription = GetFactory<WaterMainBreakRepairWorkDescriptionFactory>().Create();
            var mainBreakReplaceWorkDescription = GetFactory<WaterMainBreakReplaceWorkDescriptionFactory>().Create();
            var otherWorkDescription = GetFactory<CheckNoWaterWorkDescriptionFactory>().Create();
            var orderMainBreakRepair = GetEntityFactory<WorkOrder>().Create(new {WorkDescription = mainBreakRepairWorkDescription, Town = town});
            var orderMainBreakReplace = GetEntityFactory<WorkOrder>().Create(new {WorkDescription = mainBreakReplaceWorkDescription, Town = town });
            var orderOther = GetEntityFactory<WorkOrder>().Create(new {WorkDescription = otherWorkDescription, Town = town });

            //Act
            var result = (CascadingActionResult)_target.ByTownIdForMainBreaks(orderMainBreakRepair.Town.Id);
            var actual = result.GetSelectListItems();

            //Assert
            Assert.AreEqual(2, actual.Count() - 1); // --select here--
            foreach (var selectListItem in actual)
            {
                Assert.AreNotEqual(otherWorkDescription.Id.ToString(), selectListItem.Value);
            }
        }

        #region New/Create

        [TestMethod]
        public void TestNewSetsNotesIfSAPNotificatioNumberIsSet()
        {
            var expected = "these are some notes";
            _target.TempData[SapNotificationController.TEMP_DATA_CREATE_WORK_ORDER_NOTES] = expected;

            var result = (ViewResult)_target.New(new CreateWorkOrder(_container) { SAPNotificationNumber = 1232 });

            Assert.AreEqual(expected, ((CreateWorkOrder)result.Model).Notes);
        }

        [TestMethod]
        public void TestNewSetsNotesEvenIfModelCoordinateIsNullIfSAPNotificatioNumberIsSet()
        {
            var expected = "these are some notes";
            var mapIcon = GetFactory<MapIconFactory>().Create();
            var iconRepo = new Mock<IRepository<MapIcon>>();
            _container.Inject(iconRepo.Object);
            iconRepo.Setup(x => x.Find(MapIcon.Indices.WorkOrder)).Returns(mapIcon);

            _target.TempData[SapNotificationController.TEMP_DATA_CREATE_WORK_ORDER_NOTES] = expected;

            var result = (ViewResult)_target.New(new CreateWorkOrder(_container) { SAPNotificationNumber = 1232, Latitude = 23.1m, Longitude = -74m});

            Assert.AreEqual(expected, ((CreateWorkOrder)result.Model).Notes);
        }

        [TestMethod]
        public void TestNewDoesNotSetNotesIfSAPNotificationNumberIsNotSet()
        {
            _target.TempData[SapNotificationController.TEMP_DATA_CREATE_WORK_ORDER_NOTES] = "these are some notes";

            var result = (ViewResult)_target.New(new CreateWorkOrder(_container));

            Assert.IsNull(((CreateWorkOrder)result.Model).Notes);
        }
        
        #region Notifications

        private void TestCreateNotificationsByWorkDescriptions(int[] descriptionIds, string purpose)
        {
            foreach (var descId in descriptionIds)
            {
                var entity = GetEntityFactory<WorkOrder>().BuildWithConcreteDependencies(new {
                    WorkDescription = Session.Get<WorkDescription>(descId)
                });
                var coordinate = GetEntityFactory<Coordinate>().Create();
                var model = _viewModelFactory.BuildWithOverrides<CreateWorkOrder, WorkOrder>(entity, x => {
                    x.CoordinateId = coordinate.Id;
                });
                AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
                NotifierArgs resultArgs = null;
                _notificationService.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

                _target.Create(model);

                Assert.AreEqual(model.OperatingCenter.Value, resultArgs.OperatingCenterId);
                Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
                if (descId == (int)WorkDescription.Indices.SERVICE_LINE_RENEWAL_LEAD)
                {
                    purpose = WorkOrderController.NotificationPurposes.SERVICE_LINE_RENEWAL_LEAD_ENTERED;
                }
                Assert.AreEqual(purpose, resultArgs.Purpose);
            }

            _notificationService.VerifyAll();
        }

        [TestMethod]
        public void TestCreateSendsNotificationForMainBreakRepairReplace()
        {
            TestCreateNotificationsByWorkDescriptions(WorkDescriptionRepository.MAIN_BREAKS,
                WorkOrderController.NotificationPurposes.MAIN_BREAK);
        }

        [TestMethod]
        public void TestCreateSendsNotificationToTownContactsForMainBreakRepairReplace()
        {
            var town = GetEntityFactory<Town>().Create();
            var yesContact = GetEntityFactory<Contact>().Create();
            var noContact = GetEntityFactory<Contact>().Create();
            GetEntityFactory<ContactType>().CreateArray(ContactType.Indices.TRAFFIC_CONTROL + 1);
            town.TownContacts.Add(new TownContact {
                Town = town,
                Contact = yesContact,
                ContactType = Session.Get<ContactType>((int)ContactType.Indices.MAIN_BREAK_NOTIFICATION)
            });
            town.TownContacts.Add(new TownContact {
                Town = town,
                Contact = noContact,
                ContactType = Session.Get<ContactType>(ContactType.Indices.MAIN_BREAK_NOTIFICATION + 1)
            });
            Session.SaveOrUpdate(town);
            Session.Flush();
            Session.Clear();

            foreach (var descId in WorkDescriptionRepository.MAIN_BREAKS)
            {
                var entity = GetEntityFactory<WorkOrder>().BuildWithConcreteDependencies(new {
                    WorkDescription = Session.Get<WorkDescription>(descId),
                    Town = town
                });
                AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
                var model = _viewModelFactory.BuildWithOverrides<CreateWorkOrder, WorkOrder>(entity, new {
                    CoordinateId = GetEntityFactory<Coordinate>().Create().Id
                });

                _target.Create(model);
                entity = Repository.Find(model.Id);

                _notificationService.Verify(
                    x =>
                        x.Notify(model.OperatingCenter.Value, RoleModules.FieldServicesWorkManagement,
                            WorkOrderController.NotificationPurposes.MAIN_BREAK, entity, null, yesContact.Email, null));
            }

            _notificationService.VerifyAll();
        }

        [TestMethod]
        public void TestCreateSendsNotificationForServiceLineInstallation()
        {
            TestCreateNotificationsByWorkDescriptions(WorkDescriptionRepository.SERVICE_LINE_INSTALLATIONS,
                WorkOrderController.NotificationPurposes.SERVICE_LINE_INSTALLATION_ENTERED);
        }

        [TestMethod]
        public void TestCreateSendsNotificationForServiceLineRenewal()
        {
            TestCreateNotificationsByWorkDescriptions(WorkDescriptionRepository.SERVICE_LINE_RENEWALS,
                WorkOrderController.NotificationPurposes.SERVICE_LINE_RENEWAL_ENTERED);
        }

        [TestMethod]
        public void TestCreateSendsNotificationForSewerMainOrServiceOverflow()
        {
            TestCreateNotificationsByWorkDescriptions(WorkDescriptionRepository.SEWER_OVERFLOW,
                WorkOrderController.NotificationPurposes.SEWER_OVERFLOW_ENTERED);
        }

        [TestMethod]
        public void TestCreateSendsNotificationIfAssetTypeIsEquipment()
        {
            var entity = GetEntityFactory<WorkOrder>().BuildWithConcreteDependencies(new {
                Valve = default(Valve),
                Equipment = GetEntityFactory<Equipment>().Create(),
                AssetType = Session.Get<AssetType>((int)AssetType.Indices.EQUIPMENT),
                WorkDescription = Session.Get<WorkDescription>((int)WorkDescription.Indices.PUMP_REPAIR)
            });
            var model = _viewModelFactory.BuildWithOverrides<CreateWorkOrder, WorkOrder>(entity, new {
                CoordinateId = GetEntityFactory<Coordinate>().Create().Id
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);

            _target.Create(model);

            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == WorkOrderController.NotificationPurposes.EQUIPMENT_REPAIR)), Times.Once);
        }

        [TestMethod]
        public void TestCreateSendsNotificationIfAssociatedWithSampleSite()
        {
            var sampleSite = CreateEntity<SampleSite>(new {
                Premise = GetEntityFactory<Premise>().Create(new {
                    PremiseNumber = "9001"
                })
            });

            var entity = GetEntityFactory<WorkOrder>().BuildWithConcreteDependencies(new {
                PremiseNumber = sampleSite.Premise.PremiseNumber,
                Installation = (long)900001,
                Equipment = GetEntityFactory<Equipment>().Create(),
                AssetType = Session.Get<AssetType>((int)AssetType.Indices.EQUIPMENT),
                WorkDescription = Session.Get<WorkDescription>((int)WorkDescription.Indices.PUMP_REPAIR),
                HasSampleSite = true
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var model = _viewModelFactory.BuildWithOverrides<CreateWorkOrder, WorkOrder>(entity, new {
                CoordinateId = GetEntityFactory<Coordinate>().Create().Id
            });

            _target.Create(model);
            entity = Repository.Find(model.Id);

            _notificationService.Verify(
                x =>
                    x.Notify(model.OperatingCenter.Value, RoleModules.FieldServicesWorkManagement,
                        WorkOrderController.NotificationPurposes.SAMPLE_SITE_NOTIFICATION, entity, null, null, null));
        }

        [TestMethod]
        public void TestCreateSendsNotificationIfFRCCCreatedAndEmergency()
        {
            var emergencyPriority = GetFactory<EmergencyWorkOrderPriorityFactory>().Create();
            var frccRequestedBy = GetFactory<FRCCWorkOrderRequesterFactory>().Create();
            var entity = GetEntityFactory<WorkOrder>()
               .BuildWithConcreteDependencies(new {
                    Priority = emergencyPriority,
                    RequestedBy = frccRequestedBy
                });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var model = _viewModelFactory.BuildWithOverrides<CreateWorkOrder, WorkOrder>(entity, new {
                CoordinateId = GetEntityFactory<Coordinate>().Create().Id
            });

            _target.Create(model);
            entity = Repository.Find(model.Id);

            _notificationService.Verify(x => x.Notify(model.OperatingCenter.Value,
                RoleModules.FieldServicesWorkManagement,
                WorkOrderController.NotificationPurposes.FRCC_EMERGENCY_CREATED, entity, null, null, null));
        }

        [TestMethod]
        public void TestCreateSendsNotificationIfRequestedByIsAcousticMonitoring()
        {
            var acousticMonitoringRequestedBy = GetFactory<AcousticMonitoringWorkOrderRequesterFactory>().Create();
            var entity = GetEntityFactory<WorkOrder>()
               .BuildWithConcreteDependencies(new {
                    RequestedBy = acousticMonitoringRequestedBy
                });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var model = _viewModelFactory.BuildWithOverrides<CreateWorkOrder, WorkOrder>(entity, new {
                CoordinateId = GetEntityFactory<Coordinate>().Create().Id
            });

            _target.Create(model);
            entity = Repository.Find(model.Id);

            _notificationService.Verify(x => x.Notify(model.OperatingCenter.Value,
                RoleModules.FieldServicesWorkManagement,
                WorkOrderController.NotificationPurposes.ACOUSTIC_MONITORING_CREATED, entity, null, null, null));
        }

        #endregion

        #region SAP Syncronization

        [TestMethod]
        public void TestCreateSendsEmailWhenSAPErrorOccurs()
        {
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, SAPWorkOrdersEnabled = true });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(opCntr);
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });

            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            var sapWorkOrder = new SAPWorkOrder { OrderNumber = "123456789", SAPErrorCode = "Something strange is a foot at the Circle K.", WBSElement = "R18.asdfas-1" };
            sapRepository.Setup(x => x.Save(It.IsAny<SAPWorkOrder>())).Returns(sapWorkOrder);
            _container.Inject(sapRepository.Object);

            var model = _viewModelFactory.Build<CreateWorkOrder, WorkOrder>(GetEntityFactory<WorkOrder>().BuildWithConcreteDependencies(new { OperatingCenter = opCntr, Town = town }));
            model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;

            //ACT
            _target.Create(model);
            var workOrder = Repository.Find(model.Id);

            //ASSERT
            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
            Assert.AreEqual(sapWorkOrder.SAPErrorCode, workOrder.SAPErrorCode);
            Assert.AreEqual(int.Parse(sapWorkOrder.OrderNumber), workOrder.SAPWorkOrderNumber);
            Assert.AreEqual(sapWorkOrder.WBSElement, workOrder.AccountCharged);
        }

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsErrorCodeUponFailure()
        {
            var sapWorkOrderStepCreate = GetFactory<CreateSAPWorkOrderStepFactory>().Create();
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, SAPWorkOrdersEnabled = true });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(opCntr);
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });

            var model = _viewModelFactory.Build<CreateWorkOrder, WorkOrder>(GetEntityFactory<WorkOrder>().BuildWithConcreteDependencies(new { OperatingCenter = opCntr, Town = town }));
            model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;

            _target.Create(model);

            // FAILS BECAUSE WE DID NOT INJECT AN SAPWorkOrderRepository
            var workOrder = Repository.Find(model.Id);
            Assert.IsTrue(workOrder.SAPErrorCode.StartsWith(WorkOrderController.SAP_UPDATE_FAILURE));
            Assert.IsTrue(workOrder.SAPWorkOrderStep != null && workOrder.SAPWorkOrderStep.Id == SAPWorkOrderStep.Indices.CREATE);
        }

        [TestMethod]
        public void TestCreateCallsSAPRepositoryCreateAndRecordsSAPEquipmentIdAndNoErrorMessage()
        {
            //ARRANGE
            GetFactory<UpdateSAPWorkOrderStepFactory>().Create();
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, SAPWorkOrdersEnabled = true });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(opCntr);
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            var sapWorkOrder = new SAPWorkOrder { OrderNumber = "123456789", SAPErrorCode = "Successfully", WBSElement = "R18.asdfas-1", EquipmentNo = "00123", CostCenter = "123456"};
            sapRepository.Setup(x => x.Save(It.IsAny<SAPWorkOrder>())).Returns(sapWorkOrder);
            _container.Inject(sapRepository.Object);

            var model = _viewModelFactory.Build<CreateWorkOrder, WorkOrder>(GetEntityFactory<WorkOrder>().BuildWithConcreteDependencies(new { OperatingCenter = opCntr, Town = town }));
            model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;

            //ACT
            _target.Create(model);
            var workOrder = Repository.Find(model.Id);

            //ASSERT
            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
            Assert.AreEqual(sapWorkOrder.SAPErrorCode, workOrder.SAPErrorCode);
            Assert.AreEqual(int.Parse(sapWorkOrder.OrderNumber), workOrder.SAPWorkOrderNumber);
            Assert.AreEqual(sapWorkOrder.WBSElement, workOrder.AccountCharged);
            Assert.AreEqual(123, workOrder.SAPEquipmentNumber);
            Assert.AreEqual("123456", workOrder.BusinessUnit);
            Assert.AreEqual(SAPWorkOrderStep.Indices.UPDATE, workOrder.SAPWorkOrderStep.Id);
        }

        private void TestCreateWorkOrderByWorkDescriptions(int[] descriptionIds)
        {
            foreach (var descId in descriptionIds)
            {
                var premiseNumber = "1324354678";
                long deviceLocation = 34567890;
                var entity = GetEntityFactory<WorkOrder>().Create(new {
                    PremiseNumber = premiseNumber,
                    DeviceLocation = deviceLocation,
                    AssetType = GetFactory<ServiceAssetTypeFactory>().Create(),
                    Installation = Convert.ToInt64(9876),
                    WorkDescription = Session.Get<WorkDescription>(descId)
                });
                AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
                var model = _viewModelFactory.BuildWithOverrides<CreateWorkOrder, WorkOrder>(entity, x => {
                    x.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
                });
                model.Service = null;

                //ACT
                _target.Create(model);
                var workOrder = Repository.Find(model.Id);

                //ASSERT
                Assert.IsNotNull(workOrder.Service);

                var service = Session.Get<Service>(workOrder.Service.Id);

                Assert.AreEqual(workOrder.Installation?.ToString(), service.Installation);
                Assert.AreEqual(workOrder.DeviceLocation?.ToString(), service.DeviceLocation);
                Assert.AreEqual(workOrder.PremiseNumber, service.PremiseNumber);
            }
        }

        [TestMethod]
        public void TestCreateWorkOrderCreatesServiceRecord()
        {
            GetFactory<ServiceCategoryFactory>().CreateList(22);
            GetEntityFactory<ServicePriority>().CreateList(3);
            GetEntityFactory<ServiceInstallationPurpose>().CreateList(3);
            TestCreateWorkOrderByWorkDescriptions(WorkDescription.AUTO_CREATE_SERVICE_WORK_DESCRIPTIONS);
        }

        [TestMethod]
        public void TestCreateRedirectsToNewFromWorkOrderWhenServiceCreationFails()
        {
            var premiseNumber = "132435467"; // Should be 10 in length
            long deviceLocation = 34567890;
            var entity = GetEntityFactory<WorkOrder>().Create(new {
                PremiseNumber = premiseNumber,
                DeviceLocation = deviceLocation,
                AssetType = GetFactory<ServiceAssetTypeFactory>().Create(),
                Installation = Convert.ToInt64(9876),
                WorkDescription = Session.Get<WorkDescription>((int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION)
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var model = _viewModelFactory.BuildWithOverrides<CreateWorkOrder, WorkOrder>(entity, x => {
                x.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
            });
            model.Service = null;

            //ACT
            var result = _target.Create(model);

            //ASSERT
            Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            var redirectResult = (RedirectToRouteResult)result;

            MvcAssert.RedirectsToRoute(redirectResult, "Service", "NewFromWorkOrder", new { area = "FieldOperations", workOrderId = model.Id });
        }

        #endregion

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestEditReturns404IfWorkOrderNotFound()
        {
            var result = _target.Edit(666) as HttpNotFoundResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestEditRedirectsToShowIfSAPWorkOrderStepIsInvalid()
        {
            var workOrderStep = GetFactory<ApproveGoodsSAPWorkOrderStepFactory>().Create();
            var workOrder = GetEntityFactory<WorkOrder>().Create(new { SAPWorkOrderStep = workOrderStep });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(workOrder.OperatingCenter);

            var result = _target.Edit(workOrder.Id);

            MvcAssert.RedirectsToRoute(result, "WorkOrder", "Show", new {id = workOrder.Id });

            var otherInvalidStep = GetFactory<CompleteSAPWorkOrderStepFactory>().Create();
            workOrder = GetEntityFactory<WorkOrder>().Create(new { SAPWorkOrderStep = otherInvalidStep });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(workOrder.OperatingCenter);

            result = _target.Edit(workOrder.Id);

            MvcAssert.RedirectsToRoute(result, "WorkOrder", "Show", new { id = workOrder.Id });
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<WorkOrder>().Create();
            AddWorkManagementRoleToCurrentUserForOperatingCenter(eq.OperatingCenter);
            var coordinate = GetEntityFactory<Coordinate>().Create();
            var expected = "123321123";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditWorkOrder, WorkOrder>(eq, x => {
                x.PremiseNumber = expected;
                x.CoordinateId = coordinate.Id;
            }));

            Assert.AreEqual(expected, Session.Get<WorkOrder>(eq.Id).PremiseNumber);
        }

        [TestMethod]
        public void TestEditSendsNotificationIfAssociatedWithSampleSite()
        {
            var sampleSite = CreateEntity<SampleSite>(new {
                Premise = GetEntityFactory<Premise>().Create(new {
                    PremiseNumber = "9001"
                })
            });
            var coordinate = GetEntityFactory<Coordinate>().Create();

            var model = GetEntityFactory<WorkOrder>().Create(new {
                sampleSite.Premise.PremiseNumber,
                Installation = (long)900001,
                Equipment = GetEntityFactory<Equipment>().Create(),
                AssetType = Session.Get<AssetType>(AssetType.Indices.EQUIPMENT),
                WorkDescription = Session.Get<WorkDescription>((int)WorkDescription.Indices.PUMP_REPAIR),
                HasSampleSite = true
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(model.OperatingCenter);
            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditWorkOrder, WorkOrder>(model, x => {
                x.PremiseNumber = sampleSite.Premise.PremiseNumber;
                x.HasSampleSite = true;
                x.CoordinateId = coordinate.Id;
            }));

            var entity = Repository.Find(model.Id);

            _notificationService.Verify(
                x =>
                    x.Notify(model.OperatingCenter.Id, RoleModules.FieldServicesWorkManagement,
                        WorkOrderController.NotificationPurposes.SAMPLE_SITE_NOTIFICATION, entity, null, null, null));
        }

        [TestMethod]
        public void TestCompletesMaterialPlanningIfNoSAPErrorAndSetsMaterialPlanningCompletedOn()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(operatingCenter);
            var order = GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = operatingCenter });
            var model = _viewModelFactory.Build<CompleteMaterialPlanning, WorkOrder>(order);
            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            var sapWorkOrder = new SAPWorkOrder { OrderNumber = "123456789", SAPErrorCode = "SUCCESS", WBSElement = "R18.asdfas-1" };
            sapRepository.Setup(x => x.Save(It.IsAny<SAPWorkOrder>())).Returns(sapWorkOrder);
            _container.Inject(sapRepository.Object);

            var result = (RedirectToRouteResult)_target.CompleteMaterialPlanning(model);
            var entity = Repository.Find(model.Id);

            Assert.IsNotNull(result);
            Assert.IsNotNull(entity);
            Assert.IsNotNull(entity.MaterialPlanningCompletedOn);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCompletesMaterialPlanningIfSAPError()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(operatingCenter);
            var order = GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = operatingCenter });
            var model = _viewModelFactory.Build<CompleteMaterialPlanning, WorkOrder>(order);
            
            var result = (RedirectToRouteResult)_target.CompleteMaterialPlanning(model);
            var entity = Repository.Find(model.Id);

            Assert.IsNotNull(result);
            Assert.IsNotNull(entity);
            Assert.IsNull(entity.MaterialPlanningCompletedOn);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCancelOverridesCancellationValuesIfSAPReturnsAnError()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new
                { SAPEnabled = true, IsContractedOperations = false, SAPWorkOrdersEnabled = true });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(operatingCenter);
            var order = GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = operatingCenter, SAPWorkOrderNumber = 12356L });
            var model = _viewModelFactory.BuildWithOverrides<CancelWorkOrder, WorkOrder>(order, x => {
                x.WorkOrderCancellationReason = GetFactory<WorkOrderCancellationReasonFactory>().Create().Id;
            });
            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            var sapWorkOrder = new SAPWorkOrder { OrderNumber = "123456789", SAPErrorCode = "Enter a plant", WBSElement = "R18.asdfas-1" };
            sapRepository.Setup(x => x.Save(It.IsAny<SAPWorkOrder>())).Returns(sapWorkOrder);
            _container.Inject(sapRepository.Object);

            var result = (RedirectToRouteResult)_target.Cancel(model);
            var entity = Repository.Find(model.Id);

            Assert.IsNotNull(result);
            Assert.IsNull(entity.WorkOrderCancellationReason);
            Assert.IsNull(entity.CancelledAt);
            Assert.AreEqual(sapWorkOrder.SAPErrorCode, entity.SAPErrorCode);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCancelIsCancelledIfSAPIsSuccessful()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                SAPEnabled = true, IsContractedOperations = false, SAPWorkOrdersEnabled = true
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(operatingCenter);
            var order = GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = operatingCenter, SAPWorkOrderNumber = 12356L });
            var model = _viewModelFactory.BuildWithOverrides<CancelWorkOrder, WorkOrder>(order, x => {
                x.WorkOrderCancellationReason = GetFactory<WorkOrderCancellationReasonFactory>().Create().Id;
            });
            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            var sapWorkOrder = new SAPWorkOrder { OrderNumber = "123456789", SAPErrorCode = "SUCCESS", WBSElement = "R18.asdfas-1" };
            sapRepository.Setup(x => x.Save(It.IsAny<SAPWorkOrder>())).Returns(sapWorkOrder);
            _container.Inject(sapRepository.Object);

            var result = (RedirectToRouteResult)_target.Cancel(model);
            var entity = Repository.Find(model.Id);

            Assert.IsNotNull(result);
            Assert.IsNotNull(entity);
            Assert.IsNotNull(entity.CancelledAt);
            Assert.AreEqual(sapWorkOrder.SAPErrorCode, entity.SAPErrorCode);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCancelCancelsAndDoesNotCallSAPWhenNotSAPUpdatableWorkOrder()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new
                { SAPEnabled = true, IsContractedOperations = false, SAPWorkOrdersEnabled = true });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(operatingCenter);
            // create an order that should not be cancelled in SAP - cancelled with no sap work order or notification numbers
            var order = GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = operatingCenter, CancelledAt = DateTime.Now });
            var model = _viewModelFactory.BuildWithOverrides<CancelWorkOrder, WorkOrder>(order, x => {
                x.WorkOrderCancellationReason = GetFactory<WorkOrderCancellationReasonFactory>().Create().Id;
            });
            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            _container.Inject(sapRepository.Object);

            var result = (RedirectToRouteResult)_target.Cancel(model);
            // Verify SAP wasn't called
            sapRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);

            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestUpdateCallsSAPRepositoryAndSetsEntityFieldsFromResponse()
        {
            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            var sapWorkOrder = new SAPProgressWorkOrder {
                OrderNumber = "123456789", 
                SAPErrorCode = "Something strange is a foot at the Circle K.",
                WBSElement = "R18.asdfas-1",
                CostCenter = "123456", 
                MaterialDocument = "MD", 
                NotificationNumber = "42"
            };
            sapRepository.Setup(x => x.Update(It.IsAny<SAPProgressWorkOrder>())).Returns(sapWorkOrder);
            _container.Inject(sapRepository.Object);

            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new {
                SAPEnabled = true, 
                IsContractedOperations = false, 
                SAPWorkOrdersEnabled = true
            });
            var workOrderStep = GetFactory<CreateSAPWorkOrderStepFactory>().Create();
            var order = GetEntityFactory<WorkOrder>().Create(new {
                OperatingCenter = operatingCenter,
                SAPWorkOrderNumber = 123456789l,
                SAPWorkOrderStep = workOrderStep
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(order.OperatingCenter);
            var model = _viewModelFactory.BuildWithOverrides<EditWorkOrder, WorkOrder>(order, x => {
                x.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
            });
            
            var result = _target.Update(model) as RedirectToRouteResult;
            var entity = Repository.Find(model.Id);

            Assert.AreEqual("Edit", result.RouteValues["action"]);
            Assert.AreEqual("GeneralWorkOrder", result.RouteValues["controller"]);
            Assert.AreEqual(sapWorkOrder.CostCenter, entity.BusinessUnit);
            Assert.AreEqual(sapWorkOrder.OrderNumber, entity.SAPWorkOrderNumber.ToString());
            Assert.AreEqual(sapWorkOrder.WBSElement, entity.AccountCharged);
            Assert.AreEqual(sapWorkOrder.NotificationNumber, entity.SAPNotificationNumber.ToString());
            Assert.AreEqual(sapWorkOrder.MaterialDocument, entity.MaterialsDocID);
        }
        
        #endregion  
    }
}
