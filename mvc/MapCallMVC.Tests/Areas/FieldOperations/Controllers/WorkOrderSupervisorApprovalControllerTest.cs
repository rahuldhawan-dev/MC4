using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
using Moq;
using StructureMap;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Utilities.Pdf;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkOrderSupervisorApprovalControllerTest : MapCallMvcControllerTestBase<WorkOrderSupervisorApprovalController, WorkOrder, WorkOrderRepository>
    {
        #region Fields

        private Mock<INotificationService> _notificationService;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IImageToPdfConverter>().Mock();
            i.For<INotificationService>().Use((_notificationService = new Mock<INotificationService>()).Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container.Inject<IWorkOrderRepository>(Repository);
            GetFactory<WorkDescriptionFactory>().CreateAll();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.CreateValidEntity = () => GetFactory<CompletedWorkOrderFactory>().Create();
        }

        protected override User CreateUser()
        {
            var user = base.CreateUser();
            user.IsAdmin = true;
            Session.Save(user);
            return user;
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/FieldOperations/WorkOrderSupervisorApproval/Index/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkOrderSupervisorApproval/Search/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkOrderSupervisorApproval/Show/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkOrderSupervisorApproval/Approve/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkOrderSupervisorApproval/Reject/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkOrderSupervisorApproval/Edit", module, RoleActions.UserAdministrator);
            });
        }

        #endregion

        #region Approve

        [TestMethod]
        public void TestApproveRedirectsToWorkOrderSupervisorApprovalShowPageOnError()
        {
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create();
            var model = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            model.Id = workOrder.Id;

            _target.ModelState.AddModelError("whoopsy", "doodle");

            var result = _target.Approve(model);

            MvcAssert.RedirectsToRoute(result, "WorkOrderSupervisorApproval", "Show", new { id = workOrder.Id });
        }

        [TestMethod]
        public void TestApproveRedirectsToWorkOrderSupervisorApprovalShowPageOnSuccess()
        {
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create();
            var model = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            model.Id = workOrder.Id;

            ValidationAssert.ModelStateIsValid(model); // Sanity, because OnSuccess and OnError will have the same result.

            var result = _target.Approve(model);

            MvcAssert.RedirectsToRoute(result, "WorkOrderSupervisorApproval", "Show", new { id = workOrder.Id });
        }

        [TestMethod]
        public void TestApproveCompletesTheSAPWorkOrderAndUpdatesTheSAPErrorCodeWhenTheMapCallWorkOrderIsUpdatableInSAP()
        {
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create();
            workOrder.OperatingCenter.SAPEnabled = true;
            workOrder.OperatingCenter.SAPWorkOrdersEnabled = true;
            workOrder.SAPWorkOrderNumber = 12345;
            Assert.IsTrue(workOrder.IsSAPUpdatableWorkOrder, "Sanity");

            var model = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            model.Map(workOrder);

            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            var sapWorkOrder = new SAPCompleteWorkOrder { OrderNumber = "12345", Status = "I'm actually an error code!", WBSElement = "R18.asdfas-1" };
            sapRepository.Setup(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>())).Returns(sapWorkOrder);
            _container.Inject(sapRepository.Object);

            _target.Approve(model);

            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
            Assert.AreEqual("I'm actually an error code!", workOrder.SAPErrorCode);
        }

        [TestMethod]
        public void TestApproveDoesNotCompleteTheSAPWorkOrderWhenTheMapCallWorkOrderIsNotUpdatableInSAP()
        {
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create();
            workOrder.OperatingCenter.SAPEnabled = false;
            Assert.IsFalse(workOrder.IsSAPUpdatableWorkOrder, "Sanity");

            var model = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            model.Map(workOrder);

            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            _container.Inject(sapRepository.Object);

            _target.Approve(model);

            sapRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        [TestMethod]
        public void TestApproveSendsNotificationForAssets()
        {
            // To test this properly, we need to test a work order for every possibly valid
            // WorkDescription. All of them are created at the start of the test in initialization.
            var expectedNotificationPurpose = WorkOrderSupervisorApprovalController.SupervisorApprovalNotifications.ASSET_ORDER_COMPLETED;
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create();
            workOrder.OperatingCenter.SAPEnabled = false;
            
            var notifierArgs = new List<NotifierArgs>();

            foreach (var assetId in WorkDescriptionRepository.ASSET_COMPLETION)
            {
                workOrder.WorkDescription = Session.Load<WorkDescription>(assetId);

                // Clear previous invocations, otherwise they'll just keep adding up.
                _notificationService.Invocations.Clear();
                notifierArgs.Clear();
                _notificationService.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback((NotifierArgs x) => {
                    notifierArgs.Add(x);
                });

                var model = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
                model.Map(workOrder);

                _target.Approve(model);

                Assert.IsTrue(
                    notifierArgs.Any(x => x.Purpose == expectedNotificationPurpose),
                    "A notification should have been sent");
            }

            // Finally, test that one of these work descriptions that's not in ASSET_COMPLETION results in
            // the notification *not* being sent.

            workOrder.WorkDescription = Session.Load<WorkDescription>((int)WorkDescription.Indices.SEWER_MAIN_OVERFLOW);
            _notificationService.Invocations.Clear();
            notifierArgs.Clear();
            var modelNoNotification = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            modelNoNotification.Map(workOrder);

            _target.Approve(modelNoNotification);

            Assert.IsFalse(
                notifierArgs.Any(x => x.Purpose == expectedNotificationPurpose),
                "No notifications should have been sent.");
        }

        [TestMethod]
        public void TestApproveSendsNotificationForCurbPitWorkDescriptionsWithCompliancePurposes()
        {
            // To test this properly, we need to test a work order for every possibly valid
            // WorkDescription. All of them are created at the start of the test in initialization.

            var expectedNotificationPurpose = WorkOrderSupervisorApprovalController.SupervisorApprovalNotifications.CURB_PIT_COMPLIANCE;
            var expectedPurpose = GetFactory<ComplianceWorkOrderPurposeFactory>().Create();
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create(new {
                Purpose = expectedPurpose
            });
            workOrder.OperatingCenter.SAPEnabled = false;
            
            var notifierArgs = new List<NotifierArgs>();

            foreach (var assetId in WorkDescriptionRepository.CURB_PIT)
            {
                workOrder.WorkDescription = Session.Load<WorkDescription>(assetId);

                // Clear previous invocations, otherwise they'll just keep adding up.
                _notificationService.Invocations.Clear();
                notifierArgs.Clear();
                _notificationService.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback((NotifierArgs x) => {
                    notifierArgs.Add(x);
                });

                var model = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
                model.Map(workOrder);

                _target.Approve(model);

                Assert.IsTrue(
                    notifierArgs.Any(x => x.Purpose == expectedNotificationPurpose),
                    "A notification should have been sent");
            }

            // Next test that a work order with a different *purpose* won't send the notification.
            workOrder.Purpose = GetFactory<AssetRecordControlWorkOrderPurposeFactory>().Create();
            _notificationService.Invocations.Clear();
            notifierArgs.Clear();
            var modelNoNotification = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            modelNoNotification.Map(workOrder);

            _target.Approve(modelNoNotification);

            Assert.IsFalse(
                notifierArgs.Any(x => x.Purpose == expectedNotificationPurpose),
                "No notifications should have been sent.");

            // Finally, test that one of these work descriptions that's not in ASSET_COMPLETION results in
            // the notification *not* being sent when there's a valid purpose.

            workOrder.WorkDescription = Session.Load<WorkDescription>((int)WorkDescription.Indices.SEWER_MAIN_OVERFLOW);
            workOrder.Purpose = expectedPurpose;
            _notificationService.Invocations.Clear();
            notifierArgs.Clear();
            modelNoNotification = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            modelNoNotification.Map(workOrder);

            _target.Approve(modelNoNotification);

            Assert.IsFalse(
                notifierArgs.Any(x => x.Purpose == expectedNotificationPurpose),
                "No notifications should have been sent.");
        }
        
        [TestMethod]
        public void TestApproveSendsNotificationForCurbPitWorkDescriptionsWithEstimatesPurposes()
        {
            // To test this properly, we need to test a work order for every possibly valid
            // WorkDescription. All of them are created at the start of the test in initialization.

            var expectedNotificationPurpose = WorkOrderSupervisorApprovalController.SupervisorApprovalNotifications.CURB_PIT_ESTIMATE;
            var expectedPurpose = GetFactory<EstimatesWorkOrderPurposeFactory>().Create();
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create(new {
                Purpose = expectedPurpose
            });
            workOrder.OperatingCenter.SAPEnabled = false;
            
            var notifierArgs = new List<NotifierArgs>();

            foreach (var assetId in WorkDescriptionRepository.CURB_PIT)
            {
                workOrder.WorkDescription = Session.Load<WorkDescription>(assetId);

                // Clear previous invocations, otherwise they'll just keep adding up.
                _notificationService.Invocations.Clear();
                notifierArgs.Clear();
                _notificationService.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback((NotifierArgs x) => {
                    notifierArgs.Add(x);
                });

                var model = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
                model.Map(workOrder);

                _target.Approve(model);

                Assert.IsTrue(
                    notifierArgs.Any(x => x.Purpose == expectedNotificationPurpose),
                    "A notification should have been sent");
            }

            // Next test that a work order with a different *purpose* won't send the notification.
            workOrder.Purpose = GetFactory<AssetRecordControlWorkOrderPurposeFactory>().Create();
            _notificationService.Invocations.Clear();
            notifierArgs.Clear();
            var modelNoNotification = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            modelNoNotification.Map(workOrder);

            _target.Approve(modelNoNotification);

            Assert.IsFalse(
                notifierArgs.Any(x => x.Purpose == expectedNotificationPurpose),
                "No notifications should have been sent.");

            // Finally, test that one of these work descriptions that's not in ASSET_COMPLETION results in
            // the notification *not* being sent when there's a valid purpose.

            workOrder.WorkDescription = Session.Load<WorkDescription>((int)WorkDescription.Indices.SEWER_MAIN_OVERFLOW);
            workOrder.Purpose = expectedPurpose;
            _notificationService.Invocations.Clear();
            notifierArgs.Clear();
            modelNoNotification = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            modelNoNotification.Map(workOrder);

            _target.Approve(modelNoNotification);

            Assert.IsFalse(
                notifierArgs.Any(x => x.Purpose == expectedNotificationPurpose),
                "No notifications should have been sent.");
        }

        [TestMethod]
        public void TestApproveSendsNotificationForCurbPitWorkDescriptionsWithRevenuePurposes()
        {
            // To test this properly, we need to test a work order for every possibly valid
            // WorkDescription. All of them are created at the start of the test in initialization.

            var expectedNotificationPurpose = WorkOrderSupervisorApprovalController.SupervisorApprovalNotifications.CURB_PIT_REVENUE;
            var expectedPurpose = GetFactory<Revenue150To500WorkOrderPurposeFactory>().Create();
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create(new {
                Purpose = expectedPurpose
            });
            workOrder.OperatingCenter.SAPEnabled = false;
            
            var notifierArgs = new List<NotifierArgs>();

            foreach (var assetId in WorkDescriptionRepository.CURB_PIT)
            {
                workOrder.WorkDescription = Session.Load<WorkDescription>(assetId);

                // Clear previous invocations, otherwise they'll just keep adding up.
                _notificationService.Invocations.Clear();
                notifierArgs.Clear();
                _notificationService.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback((NotifierArgs x) => {
                    notifierArgs.Add(x);
                });

                var model = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
                model.Map(workOrder);

                _target.Approve(model);

                Assert.IsTrue(
                    notifierArgs.Any(x => x.Purpose == expectedNotificationPurpose),
                    "A notification should have been sent");
            }

            // Next test that a work order with a different *purpose* won't send the notification.
            workOrder.Purpose = GetFactory<AssetRecordControlWorkOrderPurposeFactory>().Create();
            _notificationService.Invocations.Clear();
            notifierArgs.Clear();
            var modelNoNotification = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            modelNoNotification.Map(workOrder);

            _target.Approve(modelNoNotification);

            Assert.IsFalse(
                notifierArgs.Any(x => x.Purpose == expectedNotificationPurpose),
                "No notifications should have been sent.");

            // Finally, test that one of these work descriptions that's not in ASSET_COMPLETION results in
            // the notification *not* being sent when there's a valid purpose.

            workOrder.WorkDescription = Session.Load<WorkDescription>((int)WorkDescription.Indices.SEWER_MAIN_OVERFLOW);
            workOrder.Purpose = expectedPurpose;
            _notificationService.Invocations.Clear();
            notifierArgs.Clear();
            modelNoNotification = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            modelNoNotification.Map(workOrder);

            _target.Approve(modelNoNotification);

            Assert.IsFalse(
                notifierArgs.Any(x => x.Purpose == expectedNotificationPurpose),
                "No notifications should have been sent.");
        }

        [TestMethod]
        public void TestApproveForServiceLineInstallationPartialWorkDescriptionAutomaticallyCreatesWorkOrderWithServiceLineInstallationCompletePartialWorkDescription()
        {
            GetFactory<HighPriorityWorkOrderPriorityFactory>().Create();
            GetFactory<CustomerWorkOrderPurposeFactory>().Create();
            GetFactory<ServiceLineInstallationCompletePartialWorkDescriptionFactory>().Create();
            GetFactory<NSIWorkOrderRequesterFactory>().Create();
            GetFactory<CreateSAPWorkOrderStepFactory>().Create();
            var now = DateTime.Now;
            _dateTimeProvider.SetNow(now);
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            var town = GetFactory<TownFactory>().Create();
            var markoutRequirement = GetFactory<RoutineMarkoutRequirementFactory>().Create();
            var townSection = GetFactory<TownSectionFactory>().Create();
            var street = GetFactory<StreetFactory>().Create();
            var cross = GetFactory<StreetFactory>().Create();
            var pmat = GetFactory<PlantMaintenanceActivityTypeFactory>().Create();
            var service = GetFactory<ServiceFactory>().Create();

            var entity = GetFactory<CompletedWorkOrderFactory>().Create(new {
                AssetType = GetFactory<ServiceAssetTypeFactory>().Create(),
                WorkDescription = GetFactory<ServiceLineInstallationPartialWorkDescriptionFactory>().Create(), // should create revisit work order for this work description
                MarkoutRequirement = markoutRequirement,
                OperatingCenter = operatingCenter,
                Town = town,
                Notes = "abc",
                DeviceLocation = Convert.ToInt64(123),
                SAPEquipmentNumber = Convert.ToInt64(456),
                Installation = Convert.ToInt64(789),
                PremiseNumber = "321",
                ServiceNumber = "654",
                Latitude = 32.58074M,
                Longitude = -117.059681M,
                TownSection = townSection,
                StreetNumber = "765432",
                Street = street,
                ApartmentAddtl = "apart",
                NearestCrossStreet = cross,
                ZipCode = "zip",
                PlantMaintenanceActivityTypeOverride = pmat,
                AccountCharged = "acct charged",
                Service = service
            });

            var model = _viewModelFactory.Build<SupervisorApproveWorkOrder, WorkOrder>(entity);

            _target.Approve(model);

            // revisit work order is created
            var revisitWorkOrder = Session.Query<WorkOrder>().FirstOrDefault(x => x.OriginalOrderNumber.Id == entity.Id);
            Assert.IsNotNull(revisitWorkOrder);
            Assert.AreEqual(AssetType.Indices.SERVICE, revisitWorkOrder.AssetType.Id);
            Assert.AreEqual((int)WorkDescription.Indices.SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL, revisitWorkOrder.WorkDescription.Id);
            Assert.AreEqual(operatingCenter.Id, revisitWorkOrder.OperatingCenter.Id);
            Assert.AreEqual(town.Id, revisitWorkOrder.Town.Id);
            Assert.AreEqual(WorkOrderRequester.Indices.NSI, revisitWorkOrder.RequestedBy.Id);
            Assert.AreEqual((int)WorkOrderPurpose.Indices.CUSTOMER, revisitWorkOrder.Purpose.Id);
            Assert.AreEqual((int)WorkOrderPriority.Indices.HIGH_PRIORITY, revisitWorkOrder.Priority.Id);
            Assert.AreEqual(markoutRequirement.Id, revisitWorkOrder.MarkoutRequirement.Id);
            Assert.AreEqual("abc", revisitWorkOrder.Notes);
            Assert.AreEqual(123, revisitWorkOrder.DeviceLocation);
            Assert.AreEqual(456, revisitWorkOrder.SAPEquipmentNumber);
            Assert.AreEqual(789, revisitWorkOrder.Installation);
            Assert.AreEqual("321", revisitWorkOrder.PremiseNumber);
            Assert.AreEqual("654", revisitWorkOrder.ServiceNumber);
            Assert.AreEqual("RETRY::Created from Revisit", revisitWorkOrder.SAPErrorCode);
            Assert.AreEqual((int)SAPWorkOrderStep.Indices.CREATE, revisitWorkOrder.SAPWorkOrderStep.Id);
            Assert.AreEqual(32.58074M, revisitWorkOrder.Latitude);
            Assert.AreEqual(-117.059681M, revisitWorkOrder.Longitude);
            Assert.AreEqual(townSection.Id, revisitWorkOrder.TownSection.Id);
            Assert.AreEqual("765432", revisitWorkOrder.StreetNumber);
            Assert.AreEqual(street.Id, revisitWorkOrder.Street.Id);
            Assert.AreEqual("apart", revisitWorkOrder.ApartmentAddtl);
            Assert.AreEqual(cross.Id, revisitWorkOrder.NearestCrossStreet.Id);
            Assert.AreEqual("zip", revisitWorkOrder.ZipCode);
            Assert.AreEqual(pmat.Id, revisitWorkOrder.PlantMaintenanceActivityTypeOverride.Id);
            Assert.AreEqual("acct charged", revisitWorkOrder.AccountCharged);
            Assert.AreEqual(service.Id, revisitWorkOrder.Service.Id);
            Assert.AreEqual(now, revisitWorkOrder.DateReceived);

            // create revisit work order with some null fields
            entity = GetFactory<CompletedWorkOrderFactory>().Create(new {
                AssetType = GetFactory<ServiceAssetTypeFactory>().Create(),
                WorkDescription = GetFactory<ServiceLineInstallationPartialWorkDescriptionFactory>().Create(), // should create revisit work order for this work description
                MarkoutRequirement = markoutRequirement,
                OperatingCenter = operatingCenter,
                Town = town,
                Notes = "abc",
                DeviceLocation = Convert.ToInt64(123),
                SAPEquipmentNumber = Convert.ToInt64(456),
                Installation = Convert.ToInt64(789),
                PremiseNumber = "321",
                ServiceNumber = "654",
                Latitude = 32.58074M,
                Longitude = -117.059681M,
                StreetNumber = "765432",
                Street = street,
                ApartmentAddtl = "apart",
                NearestCrossStreet = cross,
                ZipCode = "zip"
            });
            model = _viewModelFactory.Build<SupervisorApproveWorkOrder, WorkOrder>(entity);
            _target.Approve(model);
            // revisit work order is created
            revisitWorkOrder = Session.Query<WorkOrder>().FirstOrDefault(x => x.OriginalOrderNumber.Id == entity.Id);
            Assert.IsNotNull(revisitWorkOrder);
            Assert.IsNull(revisitWorkOrder.TownSection);
            Assert.IsNull(revisitWorkOrder.Service);

            // only create revisit work order if a revisit work order doesn't already exist
            _target.Approve(model);
            var secondRevisitWorkOrder = Session.Query<WorkOrder>().FirstOrDefault(x => x.OriginalOrderNumber.Id == entity.Id && x.Id != revisitWorkOrder.Id);
            Assert.IsNull(secondRevisitWorkOrder);
        }

        [TestMethod]
        public void TestApproveDoesNotCreateRevisitWorkOrderForNonServiceLineInstallationPartialWorkDescription()
        {
            // only create revisit work order for SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL work description and Service asset type
            GetFactory<ServiceLineRenewalLeadWorkDescriptionFactory>().Create();
            GetFactory<NSIWorkOrderRequesterFactory>().Create();
            var entity = GetFactory<CompletedWorkOrderFactory>().Create(new {
                AssetType = GetFactory<ServiceAssetTypeFactory>().Create(),
                WorkDescription = GetFactory<ServiceLineRenewalLeadWorkDescriptionFactory>().Create(), // shouldn't create revisit work order for this work description
            });

            var model = _viewModelFactory.Build<SupervisorApproveWorkOrder, WorkOrder>(entity);

            _target.Approve(model);

            // doesn't create revisit work order for non SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL work description
            _target.Approve(model);
            var revisitWorkOrder = Session.Query<WorkOrder>().FirstOrDefault(x => x.OriginalOrderNumber.Id == entity.Id);
            Assert.IsNull(revisitWorkOrder);
        }

        #endregion

        #region Reject

        [TestMethod]
        public void TestRejectRedirectsToWorkOrderSupervisorApprovalShowPageOnError()
        {
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create();
            var model = _viewModelFactory.Build<SupervisorRejectWorkOrder>();
            model.Id = workOrder.Id;

            _target.ModelState.AddModelError("whoopsy", "doodle");

            var result = _target.Reject(model);

            MvcAssert.RedirectsToRoute(result, "WorkOrderSupervisorApproval", "Show", new { id = workOrder.Id });
        }

        [TestMethod]
        public void TestRejectRedirectsToGeneralWorkOrderShowPageOnSuccess()
        {
            var workOrder = GetFactory<CompletedWorkOrderFactory>().Create();
            var model = _viewModelFactory.Build<SupervisorRejectWorkOrder>();
            model.Id = workOrder.Id;
            model.RejectionReason = "neato";

            ValidationAssert.ModelStateIsValid(model); // Sanity, because OnSuccess and OnError will have the same result.

            var result = _target.Reject(model);

            MvcAssert.RedirectsToRoute(result, "GeneralWorkOrder", "Show", new { id = workOrder.Id });
        }

        #endregion

        #region Edit

        [TestMethod]
        public override void TestEditReturns404IfMatchingRecordCanNotBeFound()
        {
            // noop, Edit action only needed so users with edit permissions can add/edit documents
        }

        [TestMethod]
        public override void TestEditReturnsEditViewWithEditViewModel()
        {
            // noop, Edit action only needed so users with edit permissions can add/edit documents
        }

        #endregion
    }
}
