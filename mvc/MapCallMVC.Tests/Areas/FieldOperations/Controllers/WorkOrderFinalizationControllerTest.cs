using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderFinalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkOrderFinalizationControllerTest : MapCallMvcControllerTestBase<WorkOrderFinalizationController, WorkOrder>
    {
        #region Fields

        private Mock<INotificationService> _notifier;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<INotificationService>().Use((_notifier = new Mock<INotificationService>()).Object);
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);

            options.CreateValidEntity = () => {
                var wo = GetFactory<WorkOrderFactory>().Create(new {
                    SAPWorkOrderNumber = (long?)11251,
                    OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
                    Priority = typeof(EmergencyWorkOrderPriorityFactory)
                });
                AddWorkManagementRoleToCurrentUserForOperatingCenter(wo.OperatingCenter);
                return wo;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditWorkOrderFinalization)vm;
                model.CompletedDate = DateTime.Now;
                model.DistanceFromCrossStreet = 1;
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
                a.RequiresRole("~/WorkOrderFinalization/Search", module, RoleActions.Read);
                a.RequiresRole("~/WorkOrderFinalization/Index", module, RoleActions.Read);
                a.RequiresRole("~/WorkOrderFinalization/Edit", module, RoleActions.Edit);
                a.RequiresRole("~/WorkOrderFinalization/Update", module, RoleActions.Edit);
                a.RequiresRole("~/WorkOrderFinalization/Show", module, RoleActions.Read);
            });
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // The finalization work orders criteria needs one  ofthe following for SAP filtering:
            //   - The operating center has SAPWorkOrdersEnabled = false
            //   - OR the operating center's IsContractedOperations = true
            //   - OR the work order has an SAPWorkOrderNumber value

            // All of these need to be valid work orders
            var validWorkOrderBecauseItHasSAPWorkOrderNumber = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)11251,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory)
            });
            var validWorkOrderBecauseIsNotSAPEnabled = GetFactory<WorkOrderFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
            });
            var validWorkOrderBecauseIsContractedOperations = GetFactory<WorkOrderFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = true }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
            });

            // All work orders below should be invalid for filtering

            var invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
            });
            var invalidBecauseItIsSAPEnabledButNotContractedOperations = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory)
            });
            var invalidBecauseItDoesNotHaveSAPWorkOrderNumber = GetFactory<CompletedWorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory)
            });
            
            GetFactory<WildcardOpCenterRoleFactory>().Create(RoleModules.FieldServicesWorkManagement, null, _currentUser, RoleActions.Read);

            var result = (ViewResult)_target.Index(new SearchWorkOrderFinalization());
            var results = ((SearchWorkOrderFinalization)result.Model).Results;

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.Contains(validWorkOrderBecauseItHasSAPWorkOrderNumber));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsNotSAPEnabled));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsContractedOperations));

            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber));
            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledButNotContractedOperations));
            Assert.IsFalse(results.Contains(invalidBecauseItDoesNotHaveSAPWorkOrderNumber));
        }

        [TestMethod]
        public void TestEditDefaultsMeterLocationToPremiseMeterLocationForServiceAssetType()
        {
            var eq = GetEntityFactory<WorkOrder>().Create(new {
                PremiseNumber = "A13243546", AssetType = GetFactory<ServiceAssetTypeFactory>().Create(),
                Installation = Convert.ToInt64(9876)
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(eq.OperatingCenter);
            GetFactory<OpenCrewAssignmentFactory>().Create(new { WorkOrder = eq });
            var meterLocation = GetEntityFactory<MeterLocation>().Create(new { SAPCode = "c1"});
            GetFactory<PremiseFactory>().Create(new { PremiseNumber = "A13243546", MeterLocation = meterLocation, Installation = "9876" });
            GetFactory<PremiseFactory>().Create(new { PremiseNumber = "A13243546" });

            var result = _target.Edit(eq.Id);

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.AreEqual(meterLocation.Id, ((result as ViewResult).Model as EditWorkOrderFinalization).WorkOrder.MeterLocation.Id);

            // Test for WorkOrder.Installation = null
            eq = GetEntityFactory<WorkOrder>().Create(new {
                PremiseNumber = "B13243546",
                AssetType = GetFactory<ServiceAssetTypeFactory>().Create(),
                OperatingCenter = eq.OperatingCenter
            });
            GetFactory<OpenCrewAssignmentFactory>().Create(new { WorkOrder = eq }); 
            meterLocation = GetEntityFactory<MeterLocation>().Create(new { SAPCode = "c1" });
            GetFactory<PremiseFactory>().Create(new { PremiseNumber = "B13243546", MeterLocation = meterLocation, Installation = "9876" });
            GetFactory<PremiseFactory>().Create(new { PremiseNumber = "B13243546" });

            result = _target.Edit(eq.Id);

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.AreEqual(meterLocation.Id, ((result as ViewResult).Model as EditWorkOrderFinalization).WorkOrder.MeterLocation.Id);
        }

        [TestMethod]
        public void TestEditDoesNotDefaultMeterLocationToPremiseMeterLocationForNonServiceAssetTypes()
        {
            var eq = GetEntityFactory<WorkOrder>().Create();
            AddWorkManagementRoleToCurrentUserForOperatingCenter(eq.OperatingCenter);
            GetFactory<OpenCrewAssignmentFactory>().Create(new { WorkOrder = eq });
            var meterLocation = GetEntityFactory<MeterLocation>().Create(new { SAPCode = "c1" });
            GetFactory<PremiseFactory>().Create(new { PremiseNumber = "13243546", MeterLocation = meterLocation });

            var result = _target.Edit(eq.Id);

            MvcAssert.IsViewNamed(result, "Edit");
            Assert.IsNull(((result as ViewResult).Model as EditWorkOrderFinalization).WorkOrder.MeterLocation);
        }

        [TestMethod]
        public void TestUpdateUpdatesMeterLocationInServiceInstallationForServiceAssetType()
        {
            var eq = GetEntityFactory<WorkOrder>().Create(new {
                DigitalAsBuiltRequired = true,
                AssetType = GetFactory<ServiceAssetTypeFactory>().Create()
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(eq.OperatingCenter);
            var i = GetEntityFactory<ServiceInstallation>().Create(new {
                WorkOrder = eq
            });
            Session.Refresh(eq);
            GetEntityFactory<MeterLocation>().Create(new { SAPCode = "c0"});
            var meterLocation = GetEntityFactory<MeterLocation>().Create(new { SAPCode = "c1" });
            GetEntityFactory<MeterSupplementalLocation>().Create();
            var now = DateTime.Now;

            var model = new EditWorkOrderFinalization(_container) {
                Id = eq.Id,
                CompletedDate = now,
                MeterLocation = meterLocation.Id
            };

            _target.Update(model);

            var installation = Session.Query<ServiceInstallation>().FirstOrDefault(x => x.WorkOrder.Id == eq.Id);
            Assert.AreEqual(meterLocation.Id, installation?.MeterLocation.Id);
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<WorkOrder>().Create(new {
                DigitalAsBuiltRequired = true
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(eq.OperatingCenter);
            var flushingNoticeType = GetEntityFactory<WorkOrderFlushingNoticeType>().Create();
            var now = DateTime.Now;

            var model = new EditWorkOrderFinalization(_container) {
                Id = eq.Id,
                CompletedDate = now,
                FlushingNoticeType = flushingNoticeType.Id,
                DigitalAsBuiltCompleted = true
            };

            var result = _target.Update(model);

            var entity = Session.Get<WorkOrder>(eq.Id);
            Assert.AreEqual(now, entity.DateCompleted);
            Assert.AreEqual(flushingNoticeType.Id, entity.FlushingNoticeType.Id);
            Assert.IsTrue(entity.DigitalAsBuiltCompleted);
        }

        [TestMethod]
        public void TestSetLookUpDataForCompanyServiceLineMaterialSetsCorrectlyOnEdit()
        {
            var serviceMaterial1 = GetEntityFactory<ServiceMaterial>().Create(new { Id = 1, IsEditEnabled = true });
            var serviceMaterial2 = GetEntityFactory<ServiceMaterial>().Create(new { Id = 2, IsEditEnabled = true });
            var serviceMaterial3 = GetEntityFactory<ServiceMaterial>().Create(new { Id = 3, IsEditEnabled = false });

            _target.SetLookupData(ControllerAction.Edit);

            var companyServiceLineMaterials = (IEnumerable<SelectListItem>)_target.ViewData["CompanyServiceLineMaterial"];

            Assert.AreEqual(2, companyServiceLineMaterials.Count());
            Assert.IsTrue(companyServiceLineMaterials.All(c => c.Value.Equals("1") || c.Value.Equals("2")));
        }

        [TestMethod]
        public void TestEditRedirectsToShowPageWhenWorkOrderIsSupervisorApproved()
        {
            var wo = GetFactory<WorkOrderFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
                ApprovedOn = DateTime.Now
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(wo.OperatingCenter);
            var result = _target.Edit(wo.Id);
            MvcAssert.RedirectsToRoute(result, "WorkOrderFinalization", "Show", new { id = wo.Id, bypassCheck = true });
        }

        [TestMethod]
        public void TestEditRedirectsToShowPageWhenWorkOrderIsFinalized()
        {
            var wo = GetFactory<WorkOrderFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
                DateCompleted = DateTime.Now
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(wo.OperatingCenter);
            var result = _target.Edit(wo.Id);
            MvcAssert.RedirectsToRoute(result, "WorkOrderFinalization", "Show", new { id = wo.Id });
        }

        [TestMethod]
        public void TestEditReturns404IfWorkOrderIsNotReadyForFinalization()
        {
            var wo = GetFactory<WorkOrderFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false, IsContractedOperations = false })
            });
            var result = (HttpNotFoundResult)_target.Edit(wo.Id);

            Assert.AreEqual(string.Format(WorkOrderFinalizationController.WORK_ORDER_NOT_FOUND, wo.Id), result.StatusDescription);
        }

        [TestMethod]
        public void TestEditOnModelFoundDisplaysNotificationForContractorAssignedOrder()
        {
            var wo = GetFactory<WorkOrderFactory>().Create(new {
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false, IsContractedOperations = false }),
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
                AssignedContractor = GetFactory<ContractorFactory>().Create()
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(wo.OperatingCenter);
            var result = (ViewResult)_target.Edit(wo.Id);

            Assert.IsNotNull(result.TempData[MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY]);
        }

        #endregion

        #region Notifications

        [TestMethod]
        public void TestUpdateSendsNotificationEmailForMainBreakWorkOrder()
        {
            var entity = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = GetFactory<MainBreakRepairWorkDescriptionFactory>().Create()
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var mainBreak = GetEntityFactory<MainBreak>().Create(new { WorkOrder = entity });
            entity.MainBreaks.Add(mainBreak);

            var model = _viewModelFactory.Build<EditWorkOrderFinalization, WorkOrder>(entity);
            model.CompletedDate = DateTime.Now;

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreEqual(WorkOrderFinalizationController.MAIN_BREAK_COMPLETED_NOTIFICATION, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);

            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailForServiceLineRenewalWorkOrder()
        {
            var entity = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create()
            });
            
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var model = _viewModelFactory.Build<EditWorkOrderFinalization, WorkOrder>(entity);
            model.CompletedDate = DateTime.Now;

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreEqual(WorkOrderFinalizationController.SERVICE_LINE_RENEWAL_COMPLETED, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailForEmergencyWorkOrderRequestedByFRCC()
        {
            var entity = GetFactory<WorkOrderFactory>().Create(new {
                Priority = GetFactory<EmergencyWorkOrderPriorityFactory>().Create(),
                RequestedBy = GetFactory<FRCCWorkOrderRequesterFactory>().Create()
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var model = _viewModelFactory.Build<EditWorkOrderFinalization, WorkOrder>(entity);
            model.CompletedDate = DateTime.Now;

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreEqual(WorkOrderFinalizationController.FRCC_EMERGENCY_COMPLETED_NOTIFICATION, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenWorkDescriptionChangedToMainBreakRepair()
        {
            var town = GetFactory<TownFactory>().Create();
            var townContacts = GetFactory<TownContactFactory>().CreateList(8, new {
                Town = town
            });
            town.TownContacts = new List<TownContact>(townContacts);
            var entity = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = GetFactory<BallCurbStopRepairWorkDescriptionFactory>().Create(),
                Town = town
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var mainBreak = GetEntityFactory<MainBreak>().Create(new { WorkOrder = entity });
            entity.MainBreaks.Add(mainBreak);

            var model = _viewModelFactory.Build<EditWorkOrderFinalization, WorkOrder>(entity);
            model.FinalWorkDescription = GetFactory<MainBreakRepairWorkDescriptionFactory>().Create().Id;

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreEqual(WorkOrderFinalizationController.MAIN_BREAK_NOTIFICATION, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(WorkOrderFinalizationController.WORK_DESCRIPTION_CHANGED, resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenWorkDescriptionChangedToSewerMainOverflow()
        {
            var entity = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = GetFactory<BallCurbStopRepairWorkDescriptionFactory>().Create()
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);

            var model = _viewModelFactory.Build<EditWorkOrderFinalization, WorkOrder>(entity);
            model.FinalWorkDescription = GetFactory<SewerMainOverflowWorkDescriptionFactory>().Create().Id;

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreEqual(WorkOrderFinalizationController.SEWER_OVERFLOW_CHANGED_NOTIFICATION, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailForWorkOrderWithSampleSites()
        {
            var entity = GetFactory<WorkOrderFactory>().Create(new { HasSampleSite = true });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var model = _viewModelFactory.Build<EditWorkOrderFinalization, WorkOrder>(entity);
            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreEqual(WorkOrderFinalizationController.SAMPLE_SITE_NOTIFICATION, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }
        
        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenPitcherFilterNotDeliveredAndServiceMaterialGalvanizedAndStateNJ()
        {
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create(new {Description = ServiceMaterial.Descriptions.GALVANIZED});
            var state = GetFactory<StateFactory>().Create();
            var town = GetFactory<TownFactory>().Create(new {State = state});
            var entity = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create(),
                HasPitcherFilterBeenProvidedToCustomer = false,
                PreviousServiceLineMaterial = serviceMaterial,
                Town = town
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            
            var model = _viewModelFactory.Build<EditWorkOrderFinalization, WorkOrder>(entity);
            model.CompletedDate = DateTime.Now;

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreEqual(WorkOrderFinalizationController.PITCHER_FILTER_NOT_DELIVERED, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNotNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Exactly(2));
        }
        
        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenPitcherFilterNotDeliveredAndServiceMaterialIsLead()
        {
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create(new {Description = ServiceMaterial.Descriptions.LEAD});
            var state = GetFactory<StateFactory>().Create(new {Id = 5, Name = "California", Abbreviation = "CA" });
            var town = GetFactory<TownFactory>().Create(new {State = state});
            var entity = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create(),
                HasPitcherFilterBeenProvidedToCustomer = false,
                PreviousServiceLineMaterial = serviceMaterial,
                Town = town
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            
            var model = _viewModelFactory.Build<EditWorkOrderFinalization, WorkOrder>(entity);
            model.CompletedDate = DateTime.Now;

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreEqual(WorkOrderFinalizationController.PITCHER_FILTER_NOT_DELIVERED, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNotNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Exactly(2));
        }
        
        [TestMethod]
        public void TestUpdateDoesNotSendsNotificationEmailWhenPitcherFilterDelivered()
        {
            var serviceMaterial = GetEntityFactory<ServiceMaterial>().Create(new {Description = ServiceMaterial.Descriptions.LEAD});
            var state = GetFactory<StateFactory>().Create(new {Id = 5, Name = "California", Abbreviation = "CA" });
            var town = GetFactory<TownFactory>().Create(new {State = state});
            var entity = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = GetFactory<ServiceLineRenewalWorkDescriptionFactory>().Create(),
                HasPitcherFilterBeenProvidedToCustomer = true,
                PreviousServiceLineMaterial = serviceMaterial,
                Town = town
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var model = _viewModelFactory.Build<EditWorkOrderFinalization, WorkOrder>(entity);
            model.CompletedDate = DateTime.Now;

            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreNotEqual(WorkOrderFinalizationController.PITCHER_FILTER_NOT_DELIVERED, resultArgs.Purpose);
            Assert.IsNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        #endregion
    }
}
