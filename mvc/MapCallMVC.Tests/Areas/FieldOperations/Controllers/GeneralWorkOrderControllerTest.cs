using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class GeneralWorkOrderControllerTest : MapCallMvcControllerTestBase<GeneralWorkOrderController, WorkOrder, GeneralWorkOrderRepository>
    {
        #region Fields

        private Mock<INotificationService> _notifier;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<INotificationService>().Use((_notifier = new Mock<INotificationService>()).Object);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var defaultIcon = GetFactory<MapIconFactory>().Create(new {FileName = "pin_black"});
            GetFactory<WorkDescriptionFactory>().CreateAll();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.ExpectedIndexViewName = "../WorkOrder/Index";  
            options.CreateValidEntity = () => {
                var wo = GetFactory<WorkOrderFactory>().Create();
                AddWorkManagementRoleToCurrentUserForOperatingCenter(wo.OperatingCenter);
                return wo;
            };
            options.InitializeUpdateViewModel = (vm) => {
                var model = (EditGeneralWorkOrderModel)vm;
                model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
                model.RequestedBy = WorkOrderRequester.Indices.CUSTOMER;
                model.CustomerName = "Test";
                model.SecondaryPhoneNumber = "9876543210";
                model.PremiseNumber = "9876543210";
            };
        }

        #endregion
        
        #region Private Methods

        private void AddWorkManagementRoleToCurrentUserForOperatingCenter(OperatingCenter opc)
        {
            var role = GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, opc, _currentUser, RoleActions.UserAdministrator);
        }        

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/FieldOperations/GeneralWorkOrder/Show/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/GeneralWorkOrder/Index/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/GeneralWorkOrder/Search/", module, RoleActions.Read);
                a.RequiresRole("~/FieldOperations/GeneralWorkOrder/Edit/", module, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/FieldOperations/GeneralWorkOrder/EditFromIndex/");
                a.RequiresRole("~/FieldOperations/GeneralWorkOrder/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/GeneralWorkOrder/RemovedAssignedContractor/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/GeneralWorkOrder/UpdateAdditional/", module, RoleActions.Edit);
                a.RequiresSiteAdminUser("~/FieldOperations/GeneralWorkOrder/UpdateFromIndex/");
                a.RequiresRole("~/FieldOperations/GeneralWorkOrder/UpdateTrafficControl/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/GeneralWorkOrder/UpdateComplianceData/", module, RoleActions.Edit);
                a.RequiresRole("~/FieldOperations/GeneralWorkOrder/UpdateServiceLineInfo/", module, RoleActions.Edit);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            // overridden because the search model has required properties that
            // the auto-tester can't set with expected values. This means no
            // results are returned.
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create();
            AddWorkManagementRoleToCurrentUserForOperatingCenter(operatingCenter);
            var eq1 = GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = operatingCenter });
            var eq2 = GetEntityFactory<WorkOrder>().Create(new { OperatingCenter = operatingCenter });
            var search = new SearchWorkOrder { Id = eq1.Id };

            var result = _target.Index(search) as ViewResult;
            var resultModel = ((SearchWorkOrder)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreSame(eq1, resultModel[0]);

            search.Id = eq2.Id;

            result = _target.Index(search) as ViewResult;
            resultModel = ((SearchWorkOrder)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(1, resultModel.Count);
            Assert.AreSame(eq2, resultModel[0]);

            search.Id = null;
            search.OperatingCenter = operatingCenter.Id;

            result = _target.Index(search) as ViewResult;
            resultModel = ((SearchWorkOrder)result.Model).Results.ToList();

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Count);
            Assert.AreSame(eq1, resultModel[0]);
            Assert.AreSame(eq2, resultModel[1]);
        }

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var premiseNumber = "1234567";
            long installation = 23456789;
            long deviceLocation = 34567890;
            var assetType = GetFactory<MainAssetTypeFactory>().Create();
            var planningPlant = GetFactory<PlanningPlantFactory>().Create();
            var operatingCenter = GetFactory<OperatingCenterFactory>().Create(new { DistributionPlanningPlant = planningPlant });
            var assignedContractor = GetFactory<ContractorFactory>().Create();
            var entity0 = GetEntityFactory<WorkOrder>().Create(new {StreetNumber = "123", AssetType = assetType, OperatingCenter = operatingCenter, AssignedContractor = assignedContractor });
            var entity1 = GetEntityFactory<WorkOrder>().Create(new {StreetNumber = "125", AssetType = assetType, OperatingCenter = operatingCenter });
            var entity2 = GetFactory<WorkOrderFactory>().Create(new {PremiseNumber = premiseNumber, Installation = installation, DeviceLocation = deviceLocation});
            var publicWaterSupply = GetEntityFactory<PublicWaterSupply>().Create();
            var premise = GetFactory<PremiseFactory>().Create(new {entity2.PremiseNumber, Installation = entity2.Installation.ToString(), DeviceLocation = entity2.DeviceLocation.ToString(), PublicWaterSupply = publicWaterSupply});
            var search = new SearchWorkOrder();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;
            
            entity2.Premise = premise;
            
            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Order #");
                helper.AreEqual(entity1.Id, "Order #", 1);
                helper.AreEqual(entity0.StreetNumber, "StreetNumber");
                helper.AreEqual(entity1.StreetNumber, "StreetNumber", 1);
                helper.AreEqual(entity0.AssignedContractor, "AssignedContractor");
                helper.AreEqual(entity2.PWSID, "PWSID");
                Assert.AreEqual(premise.PremiseNumber, entity2.PremiseNumber);
            }
        }

        [TestMethod]
        public void TestByOperatingCenterIdNewServicesReturnsMaterialsForOperatingCenter()
        {
            var serviceMaterial1 = GetEntityFactory<ServiceMaterial>().Create(new { Description = "valid" });
            var serviceMaterial2 = GetEntityFactory<ServiceMaterial>().Create(new { Description = "valid2" });
            var entity0 = GetEntityFactory<WorkOrder>().Create(new {
                PreviousServiceLineMaterial = serviceMaterial1,
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity0.OperatingCenter);
            
            var entity1 = GetEntityFactory<WorkOrder>().Create(new {
                PreviousServiceLineMaterial = serviceMaterial2
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity1.OperatingCenter);
            var search = new SearchWorkOrder {
                PreviousServiceLineMaterial = new[] { serviceMaterial1.Id }
            };

            var result = _target.Index(search);
            
            Assert.AreEqual(1, search.Results.Count());
            Assert.AreEqual(entity0.Id, search.Results.First().Id);

            search.PreviousServiceLineMaterial = new[] { serviceMaterial1.Id, serviceMaterial2.Id };
            result = _target.Index(search);

            Assert.AreEqual(2, search.Results.Count());
            Assert.AreEqual(entity0.Id, search.Results.First().Id);
            Assert.AreEqual(entity1.Id, search.Results.Last().Id);
        }
        
        [TestMethod]
        public void TestByHasPitcherFilterBeenProvidedToCustomerReturnsResults()
        {
            var entity0 = GetEntityFactory<WorkOrder>().Create(new {
                HasPitcherFilterBeenProvidedToCustomer = true
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity0.OperatingCenter);
            var search = new SearchWorkOrder {
                HasPitcherFilterBeenProvidedToCustomer = true
            };

            var result = _target.Index(search);
            
            Assert.AreEqual(1, search.Results.Count());
            Assert.AreEqual(entity0.Id, search.Results.First().Id);
        }
        
        [TestMethod]
        public void TestByDatePitcherFilterDeliveredToCustomerReturnsResults()
        {
            var entity0 = GetEntityFactory<WorkOrder>().Create(new {
                DatePitcherFilterDeliveredToCustomer = new DateTime(2022, 8, 10)
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity0.OperatingCenter);
            var entity1 = GetEntityFactory<WorkOrder>().Create(new {
                DatePitcherFilterDeliveredToCustomer = new DateTime(2022, 8, 12)
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity1.OperatingCenter);

            var search = new SearchWorkOrder {
                DatePitcherFilterDeliveredToCustomer = new DateRange {
                    Start = new DateTime(2022, 8, 10),
                    End = new DateTime(2022, 8, 12),
                    Operator = RangeOperator.Between
                }
            };

            var result = _target.Index(search);
            
            Assert.AreEqual(2, search.Results.Count());
            Assert.AreEqual(entity0.Id, search.Results.First().Id);
            Assert.AreEqual(entity1.Id, search.Results.Last().Id);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestUpdateTrafficControlSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<WorkOrder>().Create();
            AddWorkManagementRoleToCurrentUserForOperatingCenter(eq.OperatingCenter);
            var model = new EditTrafficControl(_container) {
                AppendNotes = "Testing Notes",
                Id = eq.Id,
                NumberOfOfficersRequired = 8
            };

            var result = _target.UpdateTrafficControl(model) as RedirectToRouteResult;

            var entity = Session.Get<WorkOrder>(eq.Id);

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(8, entity.NumberOfOfficersRequired);
        }

        [TestMethod]
        public void TestUpdateAdditionalSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<WorkOrder>().Create();
            AddWorkManagementRoleToCurrentUserForOperatingCenter(eq.OperatingCenter);
            var workDescription = GetEntityFactory<WorkDescription>().Create();
            var estimatedCustomerImpact = GetFactory<ZeroToFiftyCustomerImpactRangeFactory>().Create();
            var anticipatedRepairTime = GetFactory<FourToSixRepairTimeRangeFactory>().Create();

            var model = new EditWorkOrderAdditional(_container) {
                FinalWorkDescription = workDescription.Id,
                LostWater = 10,
                DistanceFromCrossStreet = 20.0,
                AppendNotes = "Testing Notes",
                AlertIssued = true,
                TrafficImpact = true,
                RepairTime = anticipatedRepairTime.Id,
                CustomerImpact = estimatedCustomerImpact.Id,
                Id = eq.Id
            };

            var result = _target.UpdateAdditional(model) as RedirectToRouteResult;

            var entity = Session.Get<WorkOrder>(eq.Id);

            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(workDescription.Id, entity.WorkDescription?.Id);
            Assert.AreEqual(estimatedCustomerImpact.Id, entity.EstimatedCustomerImpact?.Id);
            Assert.AreEqual(anticipatedRepairTime.Id, entity.AnticipatedRepairTime?.Id);
            Assert.IsTrue(entity.AlertIssued);
            Assert.IsTrue(entity.SignificantTrafficImpact);
            Assert.AreEqual(10, entity.LostWater);
            Assert.AreEqual(20.0, entity.DistanceFromCrossStreet);
        }

        #endregion

        #region Notifications

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

            var model = _viewModelFactory.Build<EditGeneralWorkOrderModel, WorkOrder>(entity);
            model.WorkDescription = GetFactory<MainBreakRepairWorkDescriptionFactory>().Create().Id;
            model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreEqual(GeneralWorkOrderController.MAIN_BREAK_NOTIFICATION, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.AreEqual(GeneralWorkOrderController.WORK_DESCRIPTION_CHANGED, resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Exactly(2));
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailWhenWorkDescriptionChangedToSewerMainOverflow()
        {
            var entity = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = GetFactory<BallCurbStopRepairWorkDescriptionFactory>().Create()
            });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);

            var model = _viewModelFactory.Build<EditGeneralWorkOrderModel, WorkOrder>(entity);
            model.WorkDescription = GetFactory<SewerMainOverflowWorkDescriptionFactory>().Create().Id;
            model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreEqual(GeneralWorkOrderController.SEWER_OVERFLOW_CHANGED_NOTIFICATION, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        [TestMethod]
        public void TestUpdateSendsNotificationEmailForWorkOrderWithSampleSites()
        {
            var entity = GetFactory<WorkOrderFactory>().Create(new { HasSampleSite = true });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(entity.OperatingCenter);
            var model = _viewModelFactory.Build<EditGeneralWorkOrderModel, WorkOrder>(entity);
            model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
            NotifierArgs resultArgs = null;

            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);
            var result = _target.Update(model);

            Assert.AreEqual(entity.OperatingCenter.Id, resultArgs.OperatingCenterId);
            Assert.AreEqual(RoleModules.FieldServicesWorkManagement, resultArgs.Module);
            Assert.AreEqual(GeneralWorkOrderController.SAMPLE_SITE_NOTIFICATION, resultArgs.Purpose);
            Assert.AreSame(entity, resultArgs.Data);
            Assert.IsNull(resultArgs.Subject);
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
        }

        #endregion

        #region SAP Syncronization

        [TestMethod]
        public void TestUpdateSendsEmailWhenSAPErrorOccurs()
        {
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, SAPWorkOrdersEnabled = true });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(opCntr);
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var entity = GetEntityFactory<WorkOrder>().Create(new {
                OperatingCenter = opCntr,
                Town = town
            });

            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            var sapWorkOrder = new SAPWorkOrder { OrderNumber = "123456789", SAPErrorCode = "Something strange is a foot at the Circle K.", WBSElement = "R18.asdfas-1" };
            sapRepository.Setup(x => x.Save(It.IsAny<SAPWorkOrder>())).Returns(sapWorkOrder);
            _container.Inject(sapRepository.Object);

            var model = _viewModelFactory.Build<EditGeneralWorkOrderModel, WorkOrder>(entity);
            model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            //ACT
            _target.Update(model);
            var workOrder = Repository.Find(model.Id);

            //ASSERT
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
            Assert.AreEqual(sapWorkOrder.SAPErrorCode, workOrder.SAPErrorCode);
            Assert.AreEqual(int.Parse(sapWorkOrder.OrderNumber), workOrder.SAPWorkOrderNumber);
            Assert.AreEqual(sapWorkOrder.WBSElement, workOrder.AccountCharged);
        }

        [TestMethod]
        public void TestUpdateCallsSAPRepositoryCreateAndRecordsErrorCodeUponFailure()
        {
            var sapWorkOrderStepCreate = GetFactory<CreateSAPWorkOrderStepFactory>().Create();
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, SAPWorkOrdersEnabled = true });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(opCntr);
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var entity = GetEntityFactory<WorkOrder>().Create(new {
                OperatingCenter = opCntr,
                Town = town
            });

            var model = _viewModelFactory.Build<EditGeneralWorkOrderModel, WorkOrder>(entity);
            model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;

            _target.Update(model);

            // FAILS BECAUSE WE DID NOT INJECT AN SAPWorkOrderRepository
            var workOrder = Repository.Find(model.Id);
            Assert.IsTrue(workOrder.SAPErrorCode.StartsWith(WorkOrderController.SAP_UPDATE_FAILURE));
        }

        [TestMethod]
        public void TestUpdateCallsSAPRepositoryCreateAndRecordsSAPEquipmentIdAndNoErrorMessage()
        {
            //ARRANGE
            GetFactory<UpdateSAPWorkOrderStepFactory>().Create();
            var opCntr = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, SAPWorkOrdersEnabled = true });
            AddWorkManagementRoleToCurrentUserForOperatingCenter(opCntr);
            var town = GetEntityFactory<Town>().Create();
            town.OperatingCentersTowns.Add(new OperatingCenterTown { OperatingCenter = opCntr, Town = town, Abbreviation = "XX" });
            var entity = GetEntityFactory<WorkOrder>().Create(new {
                OperatingCenter = opCntr,
                Town = town
            });
            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            var sapWorkOrder = new SAPWorkOrder { OrderNumber = "123456789", SAPErrorCode = "Successfully", WBSElement = "R18.asdfas-1", EquipmentNo = "00123", CostCenter = "123456" };
            sapRepository.Setup(x => x.Save(It.IsAny<SAPWorkOrder>())).Returns(sapWorkOrder);
            _container.Inject(sapRepository.Object);

            var model = _viewModelFactory.Build<EditGeneralWorkOrderModel, WorkOrder>(entity);
            model.CoordinateId = GetEntityFactory<Coordinate>().Create().Id;
            NotifierArgs resultArgs = null;
            _notifier.Setup(x => x.Notify(It.IsAny<NotifierArgs>())).Callback<NotifierArgs>(x => resultArgs = x);

            //ACT
            _target.Update(model);
            var workOrder = Repository.Find(model.Id);

            //ASSERT
            _notifier.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
            Assert.AreEqual(sapWorkOrder.SAPErrorCode, workOrder.SAPErrorCode);
            Assert.AreEqual(int.Parse(sapWorkOrder.OrderNumber), workOrder.SAPWorkOrderNumber);
            Assert.AreEqual(sapWorkOrder.WBSElement, workOrder.AccountCharged);
            Assert.AreEqual(123, workOrder.SAPEquipmentNumber);
            Assert.AreEqual("123456", workOrder.BusinessUnit);
            Assert.AreEqual(SAPWorkOrderStep.Indices.UPDATE, workOrder.SAPWorkOrderStep.Id);
        }

        #endregion
    }
}