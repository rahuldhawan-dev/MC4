using System;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions;
using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.StructureMap;
using Moq;
using StructureMap;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using Material = MapCall.Common.Model.Entities.Material;
using OperatingCenter = WorkOrders.Model.OperatingCenter;
using WorkDescription = WorkOrders.Model.WorkDescription;
using WorkDescriptionRepository = WorkOrders.Model.WorkDescriptionRepository;
using WorkOrder = WorkOrders.Model.WorkOrder;
using WorkOrderRepository = WorkOrders.Model.WorkOrderRepository;

namespace _271ObjectTests.Tests.Unit.Model
{
    [TestClass]
    public class SapWorkOrdersBaseRepositoryTest : EventFiringTestClass
    {
        private WorkOrderRepository target;
        private Mock<IGeneralWorkOrderRepository> _generalWorkOrderRepository;
        private Mock<ISAPWorkOrderRepository> _sapWorkOrderRepository;
        private Mock<ISAPNewServiceInstallationRepository> _sapNewServiceInstallationRepository;
        private Mock<IRepository<WorkOrder>> _workOrdersWorkOrderRepository;
        private Mock<IUser> _user;
        private IContainer _container;
        private Mock<ISecurityService> _securityService;
        private Mock<IAuditLogEntryRepository> _auditLogEntryRepository;

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            _container = new Container();

            _generalWorkOrderRepository = new Mock<IGeneralWorkOrderRepository>();
            _sapWorkOrderRepository = new Mock<ISAPWorkOrderRepository>();
            _sapNewServiceInstallationRepository = new Mock<ISAPNewServiceInstallationRepository>();
            _workOrdersWorkOrderRepository = new Mock<IRepository<WorkOrder>>();
            _securityService = new Mock<ISecurityService>();
            _auditLogEntryRepository = new Mock<IAuditLogEntryRepository>();

            _user = new Mock<IUser>();
            target = new WorkOrderRepository();

            _container.Inject(_generalWorkOrderRepository.Object);
            _container.Inject(_sapWorkOrderRepository.Object);
            _container.Inject(_sapNewServiceInstallationRepository.Object);
            _container.Inject(_workOrdersWorkOrderRepository.Object);
            _container.Inject(_auditLogEntryRepository.Object);

            _user.Setup(x => x.Name).Returns("Foo");
            target.CurrentUser = _user.Object;
            typeof(WorkOrderRepository).SetHiddenStaticFieldValueByName(
                "_securityService", _securityService.Object);

            _securityService.Setup(x => x.GetEmployeeID()).Returns(8);

            base.EventFiringTestClassInitialize();
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        #region Private Methods
        
        private MapCall.Common.Model.Entities.WorkOrder GetGeneralWorkOrder(WorkOrder workOrder)
        {
            var generalWorkOrder = new MapCall.Common.Model.Entities.WorkOrder {
                Id = workOrder.WorkOrderID,
                CancelledAt = workOrder.CancelledAt,
                SAPWorkOrderNumber = workOrder.SAPWorkOrderNumber,
                MaterialsApprovedOn = workOrder.MaterialsApprovedOn,
                ApprovedOn = workOrder.ApprovedOn,
                SAPErrorCode = workOrder.SAPErrorCode,
                HasSAPErrorCode = (!string.IsNullOrEmpty(workOrder.SAPErrorCode))
            };
            if (workOrder.SAPWorkOrderStepID != null)
            {
                generalWorkOrder.SAPWorkOrderStep = new MapCall.Common.Model.Entities.SAPWorkOrderStep { Id = workOrder.SAPWorkOrderStepID.Value };
            }
            if (workOrder.DateCompleted.HasValue)
                generalWorkOrder.DateCompleted = workOrder.DateCompleted;
            if (workOrder.OperatingCenter != null)
            {
                generalWorkOrder.OperatingCenter = new MapCall.Common.Model.Entities.OperatingCenter {
                    SAPEnabled = workOrder.OperatingCenter.SAPEnabled,
                    SAPWorkOrdersEnabled =
                        workOrder.OperatingCenter.SAPWorkOrdersEnabled,
                    IsContractedOperations =
                        workOrder.OperatingCenter.IsContractedOperations
                };
            }
            return generalWorkOrder;
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestUpdateSAPWorkOrderDoesNotAttemptToFindAGeneralWorkOrderIfProvided()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                OperatingCenter = new OperatingCenter { SAPEnabled = false }
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);

            target.UpdateSAPWorkOrder(workOrder, generalWorkOrder);
           
            _generalWorkOrderRepository.Verify(x => x.Find(It.IsAny<int>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderDoesNotUpdateSAPWhenOperatingCentersNotSAPEnabled()
        {
            var workOrder = new WorkOrder {
                WorkOrderID = 1, OperatingCenter = new OperatingCenter { SAPEnabled = false }
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);

            target.UpdateSAPWorkOrder(workOrder);

            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderDoesNotUpdateSAPWhenOperatingCentersNotSAPWorkOrdersEnabled()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = false }
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);

            target.UpdateSAPWorkOrder(workOrder);

            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderDoesNotUpdateSAPWhenOperatingCenterIsContractedOperations()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true, IsContractedOperations = true }
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);

            target.UpdateSAPWorkOrder(workOrder);

            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderDoesNotUpdateSAPWhenWorkOrderHasAlreadyBeenCancelled()
        {
            var workOrder = new WorkOrder
            {
                CancelledAt = DateTime.Now,
                WorkOrderID = 1,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = false }
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);

            target.UpdateSAPWorkOrder(workOrder);

            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderDoesNotUpdateSAPWithAnExistingOrderWithoutAnSAPWorkOrderIfItWasAlsoCancelled()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true }
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);
            workOrder.CancelledAt = DateTime.Now;

            target.UpdateSAPWorkOrder(workOrder);

            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderDoesNotUpdateSAPWithAnExistingOrderThatWasAlreadyMaterialsApproved()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true },
                MaterialsApprovedOn = DateTime.Now
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);
            
            target.UpdateSAPWorkOrder(workOrder);

            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderDoesNotUpdateSAPWithAnExistingOrderThatWasAlreadyApprovedWithNoMaterials()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true },
                ApprovedOn = DateTime.Now
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);

            target.UpdateSAPWorkOrder(workOrder);

            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Never);
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderCallsCreateWhenNoSAPWorkOrderNumber()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true }
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            var sapWorkOrder = new SAPWorkOrder { SAPErrorCode = "Successfully", OrderNumber = "123123123", WBSElement = "R18-123", NotificationNumber = "123321"};
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);
            _sapWorkOrderRepository.Setup(x => x.Save(It.IsAny<SAPWorkOrder>())).Returns(sapWorkOrder);
            target.UpdateSAPWorkOrder(workOrder);

            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Exactly(2));
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.Is<WorkOrder>(args => 
                args.SAPErrorCode == sapWorkOrder.SAPErrorCode && 
                args.SAPWorkOrderNumber.ToString() == sapWorkOrder.OrderNumber &&
                args.AccountCharged == sapWorkOrder.WBSElement && 
                args.SAPNotificationNumber.ToString() == sapWorkOrder.NotificationNumber)), Times.Once);
            _auditLogEntryRepository.Verify(x => x.Save(It.IsAny<AuditLogEntry>()), Times.Exactly(4));
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderCallsSAPMaterialApprovalWhenApprovingMaterialWithApprovableMaterials()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                SAPWorkOrderNumber = 123456,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true }
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            generalWorkOrder.MaterialsUsed.Add(new MaterialUsed { Material = new Material { PartNumber = "123333", Description = "CLAMP"} });
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);
            var sapGoodsIssued = new SAPGoodsIssue { MaterialDocument = "1234", Status = "Successfully"};
            var sapGoodsCollection = new SAPGoodsIssueCollection();
            sapGoodsCollection.Items.Add(sapGoodsIssued);

            _sapWorkOrderRepository.Setup(x => x.Approve(It.IsAny<SAPGoodsIssue>())).Returns(sapGoodsCollection);
            // change submitted work order
            workOrder.MaterialsApprovedOn = DateTime.Now;
            workOrder.MaterialsApprovedByID = 24;

            target.UpdateSAPWorkOrder(workOrder);

            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Exactly(2));
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.Is<WorkOrder>(
                            args =>
                                args.MaterialsDocID ==
                                sapGoodsIssued.MaterialDocument &&
                                args.SAPErrorCode == sapGoodsIssued.Status)),
                Times.Once);
            _auditLogEntryRepository.Verify(x => x.Save(It.IsAny<AuditLogEntry>()), Times.Exactly(4));
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderDoesNotCallSAPMaterialApprovalWhenApprovingMaterialWithNoApprovableMaterials()
        {
            var workOrder = new WorkOrder
            {
                SAPErrorCode = "Existing SAP Error Code",
                WorkOrderID = 1,
                SAPWorkOrderNumber = 123456,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true }
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);
            var sapGoodsIssued = new SAPGoodsIssue { MaterialDocument = "1234", Status = "Successfully" };
            var sapGoodsCollection = new SAPGoodsIssueCollection();
            sapGoodsCollection.Items.Add(sapGoodsIssued);

            _sapWorkOrderRepository.Setup(x => x.Approve(It.IsAny<SAPGoodsIssue>())).Returns(sapGoodsCollection);
            // change submitted work order
            workOrder.MaterialsApprovedOn = DateTime.Now;
            workOrder.MaterialsApprovedByID = 24;

            target.UpdateSAPWorkOrder(workOrder);

            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Exactly(2));
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Never);
            _auditLogEntryRepository.Verify(x => x.Save(It.IsAny<AuditLogEntry>()), Times.Exactly(1));
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderCallsCompleteSAPWorkOrderWhenWorkIsFinallyApproved()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                SAPWorkOrderNumber = 123456,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true }
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);

            // change submitted work order
            workOrder.ApprovedOn = DateTime.Now;
            workOrder.ApprovedByID = 24;

            target.UpdateSAPWorkOrder(workOrder);

            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Exactly(2));
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Once);
            _auditLogEntryRepository.Verify(x => x.Save(It.IsAny<AuditLogEntry>()), Times.Exactly(4));
        }

        [TestMethod]
        public void TestCompleteIsCalledIfAnSapErrorExistsAndTheLastStageWasComplete()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                SAPWorkOrderNumber = 123456,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true },
                SAPWorkOrderStepID = WorkOrders.Model.SAPWorkOrderStep.Indices.COMPLETE,
                SAPErrorCode = "Mistakes were made."
            };
            workOrder.ApprovedOn = DateTime.Now;
            workOrder.ApprovedByID = 420;
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);

            target.UpdateSAPWorkOrder(workOrder);
            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Exactly(2));
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Once);
            _auditLogEntryRepository.Verify(x => x.Save(It.IsAny<AuditLogEntry>()), Times.Exactly(4));
        }

        [TestMethod]
        public void TestCompleteIsNotCalledIfAnSapErrorExistsAndTheLastStageWasCompleteAndWeAreNotActualApproving()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                SAPWorkOrderNumber = 123456,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true },
                SAPWorkOrderStepID = WorkOrders.Model.SAPWorkOrderStep.Indices.COMPLETE,
                SAPErrorCode = "Mistakes were made."
            };
            workOrder.ApprovedOn = DateTime.Now;
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);

            target.UpdateSAPWorkOrder(workOrder);
            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Never);
        }

        [TestMethod]
        public void TestApproveGoodsIsCalledIfAnSapErrorExistsAndTheLastStageWasApproveGoods()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                SAPWorkOrderNumber = 123456,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true },
                SAPWorkOrderStepID = WorkOrders.Model.SAPWorkOrderStep.Indices.APPROVE_GOODS,
                SAPErrorCode = "Mistakes were made."
            };
            workOrder.ApprovedOn = DateTime.Now;
            workOrder.MaterialsApprovedOn = DateTime.Now;
            workOrder.MaterialsApprovedByID = 666;
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            generalWorkOrder.MaterialsUsed.Add(new MaterialUsed { Material = new Material { PartNumber = "123333", Description = "CLAMP" } });
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);
            target.UpdateSAPWorkOrder(workOrder);
            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Exactly(2));
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Once);
            _auditLogEntryRepository.Verify(x => x.Save(It.IsAny<AuditLogEntry>()), Times.Exactly(3));
        }

        [TestMethod]
        public void TestApproveGoodsIsNotCalledIfAnSapErrorExistsAndTheLastStageWasApproveGoodsAndWeAreNotActuallyApprovingGoods()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                SAPWorkOrderNumber = 123456,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true },
                SAPWorkOrderStepID = WorkOrders.Model.SAPWorkOrderStep.Indices.APPROVE_GOODS,
                SAPErrorCode = "Mistakes were made."
            };
            workOrder.ApprovedOn = DateTime.Now;
            workOrder.MaterialsApprovedOn = DateTime.Now;
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            generalWorkOrder.MaterialsUsed.Add(new MaterialUsed { Material = new Material { PartNumber = "123333", Description = "CLAMP" } });
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);
            target.UpdateSAPWorkOrder(workOrder);
            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Once);
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Never);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.IsAny<WorkOrder>()), Times.Never);
        }


        [TestMethod]
        public void TestUpdateSAPWorkOrderCallsUpdateIfANormalUpdateIsRequired()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                SAPWorkOrderNumber = 123456,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true }
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);
            var sapProgressWorkOrder = new SAPProgressWorkOrder
            {
                OrderNumber = "1234",
                WBSElement = "R19-123",
                NotificationNumber = "12344321",
                MaterialDocument = "123222222",
                Status = "Successfully"
            };
            _sapWorkOrderRepository.Setup(
                    x => x.Update(It.IsAny<SAPProgressWorkOrder>()))
                .Returns(sapProgressWorkOrder);
            // change submitted work order
            target.UpdateSAPWorkOrder(workOrder);
            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Exactly(2));
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Once);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.Is<WorkOrder>(args =>
                args.SAPWorkOrderNumber.ToString() == sapProgressWorkOrder.OrderNumber &&
                args.AccountCharged == sapProgressWorkOrder.WBSElement &&
                args.SAPNotificationNumber.ToString() == sapProgressWorkOrder.NotificationNumber &&
                args.MaterialsDocID == sapProgressWorkOrder.MaterialDocument &&
                args.SAPErrorCode == sapProgressWorkOrder.Status)), Times.Once);
            _auditLogEntryRepository.Verify(x => x.Save(It.IsAny<AuditLogEntry>()), Times.Exactly(4));
        }
        [TestMethod]
        public void TestUpdateSAPWorkOrderCallsUpdateIfANormalUpdateIsRequiredDoesNotIncludeCrewAssignmentsIfNotNecessary()
        {
            var workOrder = new WorkOrder
            {
                WorkOrderID = 1,
                SAPWorkOrderNumber = 123456,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true },
                DateCompleted = DateTime.Now
            };
            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            generalWorkOrder.CrewAssignments.Add(new MapCall.Common.Model.Entities.CrewAssignment());
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);
            var sapProgressWorkOrder = new SAPProgressWorkOrder
            {
                OrderNumber = "1234",
                WBSElement = "R19-123",
                NotificationNumber = "12344321",
                MaterialDocument = "123222222",
                Status = "Successfully"
            };
            _sapWorkOrderRepository.Setup(
                    x => x.Update(It.IsAny<SAPProgressWorkOrder>()))
                .Returns(sapProgressWorkOrder);
            // change submitted work order
            target.UpdateSAPWorkOrder(workOrder);
            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Exactly(2));
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.Is<SAPProgressWorkOrder>(z => z.sapCrewAssignments == null)), Times.Once);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.Is<WorkOrder>(args =>
                args.SAPWorkOrderNumber.ToString() == sapProgressWorkOrder.OrderNumber &&
                args.AccountCharged == sapProgressWorkOrder.WBSElement &&
                args.SAPNotificationNumber.ToString() == sapProgressWorkOrder.NotificationNumber &&
                args.MaterialsDocID == sapProgressWorkOrder.MaterialDocument &&
                args.SAPErrorCode == sapProgressWorkOrder.Status)), Times.Once);
            _auditLogEntryRepository.Verify(x => x.Save(It.IsAny<AuditLogEntry>()), Times.Exactly(4));
        }

        [TestMethod]
        public void TestUpdateSAPWorkOrderCallsExtraUpdateForNewServiceInstallationMeterSet()
        {
            var wd = new WorkDescription { WorkDescriptionID = WorkDescriptionRepository.INSTALL_METER };
            var workOrder = new WorkOrder
            {
                WorkDescriptionID = wd.WorkDescriptionID,
                WorkDescription = wd,
                WorkOrderID = 1,
                SAPWorkOrderNumber = 123456,
                OperatingCenter = new OperatingCenter { SAPEnabled = true, SAPWorkOrdersEnabled = true }
            };

            var generalWorkOrder = GetGeneralWorkOrder(workOrder);
            _generalWorkOrderRepository.Setup(x => x.Find(workOrder.WorkOrderID)).Returns(generalWorkOrder);
            generalWorkOrder.ServiceInstallations.Add(new ServiceInstallation());
            workOrder.DateCompleted = DateTime.Now;
            var sapProgressWorkOrder = new SAPProgressWorkOrder
            {
                OrderNumber = "1234",
                WBSElement = "R19-123",
                NotificationNumber = "12344321",
                MaterialDocument = "123222222",
                Status = "Successfully"
            };
            _sapWorkOrderRepository.Setup(
                    x => x.Update(It.IsAny<SAPProgressWorkOrder>()))
                .Returns(sapProgressWorkOrder);
            var sapNewServiceInstallation = new SAPNewServiceInstallation { SAPStatus = "Successfully did a thing" };
            _sapNewServiceInstallationRepository.Setup(
                    x => x.Save(It.IsAny<SAPNewServiceInstallation>()))
                .Returns(sapNewServiceInstallation);

            // change submitted work order
            target.UpdateSAPWorkOrder(workOrder);
            _generalWorkOrderRepository.Verify(x => x.Find(workOrder.WorkOrderID), Times.Exactly(2));
            _sapWorkOrderRepository.Verify(x => x.Save(It.IsAny<SAPWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Complete(It.IsAny<SAPCompleteWorkOrder>()), Times.Never);
            _sapWorkOrderRepository.Verify(x => x.Update(It.IsAny<SAPProgressWorkOrder>()), Times.Once);
            _workOrdersWorkOrderRepository.Verify(x => x.UpdateCurrentEntity(It.Is<WorkOrder>(args =>
                args.SAPWorkOrderNumber.ToString() == sapProgressWorkOrder.OrderNumber &&
                args.AccountCharged == sapProgressWorkOrder.WBSElement &&
                args.SAPNotificationNumber.ToString() == sapProgressWorkOrder.NotificationNumber &&
                args.MaterialsDocID == sapProgressWorkOrder.MaterialDocument &&
                args.SAPErrorCode == sapNewServiceInstallation.SAPStatus)), Times.Once);
            _auditLogEntryRepository.Verify(x => x.Save(It.IsAny<AuditLogEntry>()), Times.Exactly(6));
        }

        #endregion
    }
}