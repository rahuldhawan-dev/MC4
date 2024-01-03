using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderStockToIssue;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderSupervisorApproval;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.FieldOperations.Controllers
{
    [TestClass]
    public class WorkOrderStockToIssueControllerTest : MapCallMvcControllerTestBase<WorkOrderStockToIssueController, WorkOrder, WorkOrderRepository>
    {
        #region Fields

        private Mock<INotificationService> _notificationService;

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
        
        private WorkOrder CreateApprovedWorkOrderWithMaterials(object args = null)
        {
            var wo = CreateApprovedWorkOrder(args);
            var mu = new MaterialUsed {
                Material = GetEntityFactory<Material>().Create(),
                WorkOrder = wo,
                Quantity = 1,
            };
            wo.MaterialsUsed.Add(mu);
            Session.Save(mu);
            Session.Save(wo);
            Session.Flush();
            return wo;
        }

        private WorkOrder CreateApprovedWorkOrder(object args = null)
        {
            var wo = GetFactory<WorkOrderFactory>().Create(args);
            wo.ApprovedBy = GetEntityFactory<User>().Create();
            Session.Save(wo);
            Session.Flush();
            return wo;
        }

        protected override IEntity CreateEntityForAutomatedTests(bool saveEntity = true)
        {
            return CreateApprovedWorkOrderWithMaterials();
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesWorkManagement;
                a.RequiresRole("~/FieldOperations/WorkOrderStockToIssue/Index/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkOrderStockToIssue/Search/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkOrderStockToIssue/Show/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkOrderStockToIssue/Approve/", module, RoleActions.UserAdministrator);
                a.RequiresRole("~/FieldOperations/WorkOrderStockToIssue/Edit/", module, RoleActions.UserAdministrator);
            });
        }

        #endregion

        #region Approve

        [TestMethod]
        public void TestApproveRedirectsToShowPageOnError()
        {
            var workOrder = CreateApprovedWorkOrderWithMaterials();
            var model = _viewModelFactory.Build<ApproveWorkOrderStockToIssue>();
            model.Id = workOrder.Id;
            
            _target.ModelState.AddModelError("whoopsy", "doodle");

            var result = _target.Approve(model);

            MvcAssert.RedirectsToRoute(result, "WorkOrderStockToIssue", "Show", new { id = workOrder.Id });
        }

        [TestMethod]
        public void TestApproveRedirectsToWorkOrderShowPageOnSuccess()
        {
            var workOrder = CreateApprovedWorkOrderWithMaterials();
            var model = _viewModelFactory.Build<ApproveWorkOrderStockToIssue>();
            model.Id = workOrder.Id;
            model.MaterialPostingDate = DateTime.Now;

            ValidationAssert.ModelStateIsValid(model); // Sanity, because OnSuccess and OnError will have the same result.

            var result = _target.Approve(model);

            MvcAssert.RedirectsToRoute(result, "WorkOrderStockToIssue", "Show", new { id = workOrder.Id });
        }

        [TestMethod]
        public void TestApproveCallsSAPApproveGoodsAndUpdatesTheSAPErrorCodeWhenTheMapCallWorkOrderIsUpdatableInSAP()
        {
            var workOrder = CreateApprovedWorkOrderWithMaterials();
            workOrder.OperatingCenter.SAPEnabled = true;
            workOrder.OperatingCenter.SAPWorkOrdersEnabled = true;
            workOrder.SAPWorkOrderNumber = 12345;
            Assert.IsTrue(workOrder.IsSAPUpdatableWorkOrder, "Sanity");

            var model = _viewModelFactory.Build<ApproveWorkOrderStockToIssue>();
            model.Map(workOrder);
            model.MaterialPostingDate = DateTime.Now;
            ValidationAssert.ModelStateIsValid(model); // Sanity, because OnSuccess and OnError will have the same result.

            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            var expectedSAPResponse = new SAPGoodsIssueCollection();
            var sapWorkOrder = new SAPGoodsIssue { OrderNumber = "12345", Status = "I'm actually an error code!" };
            expectedSAPResponse.Items.Add(sapWorkOrder);
            sapRepository.Setup(x => x.Approve(It.IsAny<SAPGoodsIssue>())).Returns(expectedSAPResponse);
            _container.Inject(sapRepository.Object);

            _target.Approve(model);

            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Once);
            Assert.AreEqual("I'm actually an error code!", workOrder.SAPErrorCode);
        }
        
        [TestMethod]
        public void TestApproveDoesNotCallSAPWhenTheMapCallWorkOrderIsNotUpdatableInSAP()
        {
            var workOrder = CreateApprovedWorkOrderWithMaterials();
            workOrder.OperatingCenter.SAPEnabled = false;
            Assert.IsFalse(workOrder.IsSAPUpdatableWorkOrder, "Sanity");

            var model = _viewModelFactory.Build<SupervisorApproveWorkOrder>();
            model.Map(workOrder);

            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            _container.Inject(sapRepository.Object);

            //_target.Approve(model);
            
            sapRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        [TestMethod]
        public void TestApproveDoesNotCallSAPWhenTheWorkOrderDoesNotHaveMaterialsUsed()
        {
            var workOrder = CreateApprovedWorkOrder();
            workOrder.OperatingCenter.SAPEnabled = true;
            workOrder.OperatingCenter.SAPWorkOrdersEnabled = true;
            workOrder.SAPWorkOrderNumber = 12345;
            Assert.IsTrue(workOrder.IsSAPUpdatableWorkOrder, "Sanity");
            workOrder.MaterialsUsed.Clear();

            var model = _viewModelFactory.Build<ApproveWorkOrderStockToIssue>();
            model.Map(workOrder);

            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            _container.Inject(sapRepository.Object);

            _target.Approve(model);
          
            sapRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        [TestMethod]
        public void TestApproveDoesNotCallSAPWhenTheWorkOrderHasNonStockMaterialsUsed()
        {
            var workOrder = CreateApprovedWorkOrder();
            workOrder.OperatingCenter.SAPEnabled = true;
            workOrder.OperatingCenter.SAPWorkOrdersEnabled = true;
            workOrder.SAPWorkOrderNumber = 12345;
            Assert.IsTrue(workOrder.IsSAPUpdatableWorkOrder, "Sanity");
            workOrder.MaterialsUsed.Clear();
            workOrder.MaterialsUsed.Add(new MaterialUsed {
                Material = null,
                WorkOrder = workOrder,
                Quantity = 10
            });

            var model = _viewModelFactory.Build<ApproveWorkOrderStockToIssue>();
            model.Map(workOrder);

            var sapRepository = new Mock<ISAPWorkOrderRepository>();
            _container.Inject(sapRepository.Object);

            _target.Approve(model);
          
            sapRepository.Verify(x => x.Approve(It.IsAny<SAPGoodsIssue>()), Times.Never);
            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
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
