using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MapCall.Common.Utility.Notifications;
using MapCall.SAP.Model.Entities;
using MapCall.SAP.Model.Repositories;
using MapCallMVC.Areas.Production.Controllers;
using MapCallMVC.Areas.Production.Models.ViewModels;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.Documents;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.Production.Controller
{
    [TestClass]
    public class ProductionWorkOrderControllerTest : MapCallMvcControllerTestBase<ProductionWorkOrderController, ProductionWorkOrder, ProductionWorkOrderRepository>
    {
        #region Fields

        private Mock<ISAPProgressUnscheduledWorkOrderRepository> _sapProgressRepo; // = new Mock<ISAPProgressUnscheduledWorkOrderRepository>;
        private Mock<ISAPCompleteUnscheduledWorkOrderRepository> _sapCompleteRepo;
        private ProductionWorkOrderCancellationReason _companyError;
        private OperatingCenter _operatingCenter;
        private Employee _employee;
        private IEnumerable<OrderType> _orderTypes;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            _employee = GetEntityFactory<Employee>().Create(new {
                OperatingCenter = _operatingCenter,
                EmailAddress = "employee@work.com"
            });

            return GetFactory<AdminUserFactory>().Create(new {
                Employee = _employee,
            });
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _sapProgressRepo = new Mock<ISAPProgressUnscheduledWorkOrderRepository>();
            _container.Inject(_sapProgressRepo.Object);

            _sapCompleteRepo = new Mock<ISAPCompleteUnscheduledWorkOrderRepository>();
            _container.Inject(_sapCompleteRepo.Object);

            _companyError = GetFactory<CompanyErrorProductionWorkOrderCancellationReasonFactory>().Create();
            
            _orderTypes = GetFactory<OrderTypeFactory>().CreateAll();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<INotificationService>().Use((_notificationService = new Mock<INotificationService>()).Object);
            e.For<IDocumentService>().Singleton().Use<InMemoryDocumentService>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            options.InitializeCreateViewModel = (vm) => {
                // Needs to exist for the validation to pass.
                GetFactory<ProductionWorkDescriptionFactory>().Create(new {
                    OrderType = GetFactory<CorrectiveActionOrderTypeFactory>().Create()
                });
                GetFactory<ProductionWorkDescriptionFactory>().Create(new {
                    OrderType = GetFactory<PlantMaintenanceOrderTypeFactory>().Create()
                });
                var model = (CreateProductionWorkOrder)vm;
                model.Equipment = GetEntityFactory<Equipment>().Create().Id;
            };
            options.InitializeUpdateViewModel = (vm) => {
                // Needs to exist for the validation to pass.
                GetFactory<ProductionWorkDescriptionFactory>().Create(new {
                    OrderType = GetFactory<CorrectiveActionOrderTypeFactory>().Create()
                });
                GetFactory<ProductionWorkDescriptionFactory>().Create(new {
                    OrderType = GetFactory<PlantMaintenanceOrderTypeFactory>().Create()
                });
                var model = (EditProductionWorkOrder)vm;
                model.Equipment = GetEntityFactory<Equipment>().Create().Id;
            };
        }

        #endregion

        #region Tests

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            const RoleModules role = RoleModules.ProductionWorkManagement;
            Authorization.Assert(auth => {
                auth.RequiresRole("~/Production/ProductionWorkOrder/Show", role, RoleActions.Read);
                auth.RequiresRole("~/Production/ProductionWorkOrder/Search", role, RoleActions.Read);
                auth.RequiresRole("~/Production/ProductionWorkOrder/Index", role, RoleActions.Read);
                auth.RequiresLoggedInUserOnly("~/Production/ProductionWorkOrder/ByFacilityIdForLockoutForms");
                auth.RequiresLoggedInUserOnly("~/Production/ProductionWorkOrder/CorrectiveWorkOrdersForReplacedEquipment");
                //Create/Edit/Update
                auth.RequiresRole("~/Production/ProductionWorkOrder/New", role, RoleActions.Add);
                auth.RequiresRole("~/Production/ProductionWorkOrder/Create", role, RoleActions.Add);
                auth.RequiresRole("~/Production/ProductionWorkOrder/Edit", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/ProductionWorkOrder/Update", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/ProductionWorkOrder/CreateProductionWorkOrderFromPlan", role, RoleActions.Add);

                auth.RequiresRole("~/Production/ProductionWorkOrder/AddEmployeeAssignment", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/ProductionWorkOrder/RemoveEmployeeAssignment", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/ProductionWorkOrder/RemoveEmployeeAssignments", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/ProductionWorkOrder/AddProductionWorkOrderMaterialUsed", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/ProductionWorkOrder/RemoveProductionWorkOrderMaterialUsed", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/ProductionWorkOrder/CompleteProductionWorkOrder", role);
                auth.RequiresRole("~/Production/ProductionWorkOrder/SupervisorApproveProductionWorkOrder", role);
                auth.RequiresRole("~/Production/ProductionWorkOrder/RedTagPermitAuthorizationForProductionWorkOrder", role);
                auth.RequiresRole("~/Production/ProductionWorkOrder/RejectProductionWorkOrder", role);
                auth.RequiresRole("~/Production/ProductionWorkOrder/ApproveMaterialWorkOrder", role);
                auth.RequiresRole("~/Production/ProductionWorkOrder/CancelProductionWorkOrder", role);
                auth.RequiresRole("~/Production/ProductionWorkOrder/CapitalizeProductionWorkOrder", role);
                auth.RequiresRole("~/Production/ProductionWorkOrder/CompleteMaterialPlanning", role);
                auth.RequiresRole("~/Production/ProductionWorkOrder/AddProductionWorkOrderProductionPrerequisite", role, RoleActions.Edit);
                auth.RequiresRole("~/Production/ProductionWorkOrder/History", role);
                auth.RequiresRole("~/Production/ProductionWorkOrder/CreateProductionWorkOrderForEquipment", role, RoleActions.Add);
                auth.RequiresRole("~/Production/ProductionWorkOrder/IndexForMaintenancePlanOrders", role);
            });
        }

        #region Show

        #region Notifications

        [TestMethod]
        public void TestShowSetsDisplaysLockOutFormNotificationWhenLockoutFormPrerequisiteIsTrue()
        {
            //var dataType = GetEntityFactory<DataType>().Create(new { TableName = "ProductionWorkOrders" });
            //var documentType = GetEntityFactory<DocumentType>().Create(new { Name = "ProdFoo", DataType = dataType });
            //var docLink = GetFactory<DocumentLinkFactory>().Create(new { DataType = dataType, DocumentType = documentType});
            //var prodDoc = Session.Load<ProductionWorkOrderDocument>(docLink.Document.Id);
            // SQL logic error
            // foreign key mismatch - "ProductionWorkOrdersProductionPrerequisites" referencing "DocumentLinkView"
            var entity = GetEntityFactory<ProductionWorkOrder>().Create();
            var prerequisite = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var pwoPrerequisite = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new { ProductionPrerequisite = prerequisite, ProductionWorkOrder = entity });
            Session.Flush();
            Session.Evict(entity);

            _target.Show(entity.Id);

            _target.AssertTempDataContainsMessage(
                ProductionWorkOrderController.LOCKOUT_FORM_PREREQUISITE, 
                MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestShowDoesNotDisplaysLockOutFormNotificationWhenLockoutFormPrerequisiteIsTrueAndSkipRequirementIsTrue()
        {
            var entity = GetEntityFactory<ProductionWorkOrder>().Create();
            var prerequisite = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var pwoPrerequisite = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new { ProductionPrerequisite = prerequisite, ProductionWorkOrder = entity, SkipRequirement = true });
            Session.Flush();
            Session.Evict(entity);

            var result = _target.Show(entity.Id) as ViewResult;

            Assert.IsNull(result.TempData[MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY]);
        }

        [TestMethod]
        public void TestShowDisplaysSuccessMessageIfLockoutFormHasBeenCreatedButNotCompletedForOrderWhenLockoutFormPrerequisiteIsTrue()
        {
            var entity = GetEntityFactory<ProductionWorkOrder>().Create();
            var prerequisite = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var pwoPrerequisite = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new { ProductionPrerequisite = prerequisite, ProductionWorkOrder = entity });
            var lockout = GetEntityFactory<LockoutForm>().Create(new { ProductionWorkOrder = entity});
            Session.Flush();
            Session.Evict(entity);

            _target.Show(entity.Id);

            _target.AssertTempDataContainsMessage(
                ProductionWorkOrderController.LOCKOUT_FORM_CREATED, 
                MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestShowDisplaysSuccessMessageIfLockoutFormHasBeenCreatedAndCompletedForOrderWhenLockoutFormPrerequisiteIsTrue()
        {
            var entity = GetEntityFactory<ProductionWorkOrder>().Create();
            var prerequisite = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var pwoPrerequisite = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new { ProductionPrerequisite = prerequisite, ProductionWorkOrder = entity });
            var lockout = GetEntityFactory<LockoutForm>().Create(new { ProductionWorkOrder = entity, ReturnedToServiceDateTime = DateTime.Now});
            Session.Flush();
            Session.Evict(entity);

            _target.Show(entity.Id);

            _target.AssertTempDataContainsMessage(
                ProductionWorkOrderController.LOCKOUT_FORM_COMPLETED, 
                MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestShowDisplaysSuccessMessageIfRedTagPermitHasBeenCreatedButNotCompletedForOrderWhenRedTagPermitAuthIsTrue()
        {
            var entity = GetEntityFactory<ProductionWorkOrder>().Create(new {
                NeedsRedTagPermitAuthorization = true
            });
            var prerequisite = GetFactory<RedTagPermitProductionPrerequisiteFactory>().Create();
            var pwoPrerequisite = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new { ProductionPrerequisite = prerequisite, ProductionWorkOrder = entity });
            var redTagPermit = GetEntityFactory<RedTagPermit>().Create(new { ProductionWorkOrder = entity });
            Session.Flush();
            Session.Evict(entity);

            _target.Show(entity.Id);

            _target.AssertTempDataContainsMessage(
                ProductionWorkOrderController.RED_TAG_PERMIT_CREATED,
                MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestShowDisplaysSuccessMessageIfRedTagPermitFormHasBeenCreatedAndCompletedForOrderWhenRedTagPrerequisiteIsTrueAndAuthorized()
        {
            var entity = GetEntityFactory<ProductionWorkOrder>().Create(new {
                NeedsRedTagPermitAuthorization = true
            });
            var prerequisite = GetFactory<RedTagPermitProductionPrerequisiteFactory>().Create();
            var pwoPrerequisite = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new { ProductionPrerequisite = prerequisite, ProductionWorkOrder = entity });
            var redTagPermit = GetEntityFactory<RedTagPermit>().Create(new { ProductionWorkOrder = entity, EquipmentRestoredOn = DateTime.Now });
            Session.Flush();
            Session.Evict(entity);

            _target.Show(entity.Id);

            _target.AssertTempDataContainsMessage(
                ProductionWorkOrderController.RED_TAG_PERMIT_COMPLETED,
                MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY);
        }
        [TestMethod]
        public void TestShowDisplaysNotificationWhenConfinedSpaceFormIsRequired()
        {
            var entity = GetEntityFactory<ProductionWorkOrder>().Create();
            var prerequisite = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();
            var pwoPrereq = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new {ProductionPrerequisite = prerequisite, ProductionWorkOrder = entity});
            Session.Flush();
            Session.Evict(entity);

            _target.Show(entity.Id);

            _target.AssertTempDataContainsMessage(ProductionWorkOrderController.CONFINED_SPACE_PREREQUISITE, MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);
        }
        
        [TestMethod]
        public void TestShowDisplaysCSFCreationNotificationWhenConfinedSpaceFormsExistForPWO()
        {
            var entity = GetEntityFactory<ProductionWorkOrder>().Create();
            var prerequisite = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();
            var pwoPrereq = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new {ProductionPrerequisite = prerequisite, ProductionWorkOrder = entity});
            var csf = GetEntityFactory<ConfinedSpaceForm>().Create(new { ProductionWorkOrder = entity });

            Session.Flush();
            Session.Evict(entity);

            _target.Show(entity.Id);

            _target.AssertTempDataContainsMessage(ProductionWorkOrderController.CONFINED_SPACE_FORM_CREATED, MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestShowDisplaysCSFCompletedWithPermit()
        {
            var entity = GetEntityFactory<ProductionWorkOrder>().Create();
            var prerequisite = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();
            var pwoPrereq = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new{ProductionPrerequisite = prerequisite, ProductionWorkOrder = entity});
            var completedcsf = GetFactory<CompletedConfinedSpaceFormFactory>().Create(new {ProductionWorkOrder = entity});
            // Section 2 for CSF
            completedcsf.AtmosphericTests.Add(GetEntityFactory<ConfinedSpaceFormAtmosphericTest>().Create());
            // Section 5 for CSF
            completedcsf.PermitBeginsAt = DateTime.Now;
            completedcsf.PermitEndsAt = DateTime.Now;
            completedcsf.HasRetrievalSystem = true;
            completedcsf.HasContractRescueService = true;
            completedcsf.EmergencyResponseAgency = "Emergency Response Agency";
            completedcsf.EmergencyResponseContact = "Jeff Winger";

            Session.Flush();
            Session.Evict(entity);

            _target.Show(entity.Id);

            _target.AssertTempDataContainsMessage(ProductionWorkOrderController.CONFINED_SPACE_FORM_COMPLETED_WITH_PERMIT, MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY);
        }

        [TestMethod]
        public void TestShowDisplaysCSFCompletedWithoutPermit()
        {
            var entity = GetEntityFactory<ProductionWorkOrder>().Create();
            var prerequisite = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();
            var pwoPrereq = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new{ProductionPrerequisite = prerequisite, ProductionWorkOrder = entity});
            var completedcsf = GetFactory<CompletedConfinedSpaceFormFactory>().Create(new {ProductionWorkOrder = entity});
            completedcsf.AtmosphericTests.Add(GetEntityFactory<ConfinedSpaceFormAtmosphericTest>().Create());

            Session.Flush();
            Session.Evict(entity);

            _target.Show(entity.Id);

            _target.AssertTempDataContainsMessage(ProductionWorkOrderController.CONFINED_SPACE_FORM_COMPLETED_WITHOUT_PERMIT, MMSINC.Controllers.ControllerBase.SUCCESS_MESSAGE_KEY);
        }

        #endregion

        #region PDF

        [TestMethod]
        public void TestShowRespondsToPdf()
        {
            var entity = GetFactory<ProductionWorkOrderFactory>().Create();
            InitializeControllerAndRequest("~/Production/ProductionWorkOrder/Show/" + entity.Id + ".pdf");
            var result = _target.Show(entity.Id);
            Assert.IsInstanceOfType(result, typeof(PdfResult));
        }

        [TestMethod]
        public void TestShowReturnsPdfViewWithExpectedModel()
        {
            var entity = GetFactory<ProductionWorkOrderFactory>().Create();
            InitializeControllerAndRequest("~/Production/ProductionWorkOrder/Show/" + entity.Id + ".pdf");
            var result = _target.Show(entity.Id);

            MvcAssert.IsViewNamed(result, "Pdf");
            var model = (ProductionWorkOrder)((PdfResult)result).Model;
            Assert.AreSame(entity, model);
        }

        #endregion

        #endregion

        #region GetBy

        [TestMethod]
        public void TestByFacilityIdForLockoutForms()
        {
            var facilities = GetEntityFactory<Facility>().CreateList(2);
            var goodOrder = GetEntityFactory<ProductionWorkOrder>().Create(new {Facility = facilities[0]});
            var badOrder = GetEntityFactory<ProductionWorkOrder>().Create(new {Facility = facilities[1]});
            
            var result = (CascadingActionResult)_target.ByFacilityIdForLockoutForms(facilities[0].Id);
            var data = (IEnumerable<ProductionWorkOrder>)result.Data;

            Assert.AreEqual(goodOrder.Id, data.Single().Id);
        }

        #endregion

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<ProductionWorkOrder>().Create();
            var entity1 = GetEntityFactory<ProductionWorkOrder>().Create();
            var entity2 = GetEntityFactory<ProductionWorkOrder>().Create(new { Equipment = (Equipment)null});
            var lockoutPreReq = GetFactory<HasLockoutRequirementProductionPrerequisiteFactory>().Create();
            var lockoutPreReqOrder = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new {ProductionWorkOrder = entity0, ProductionPrerequisite = lockoutPreReq});
            var lockoutForm = GetEntityFactory<LockoutForm>().Create(new { ProductionWorkOrder = entity1 });
            entity1.LockoutForms.Add(lockoutForm);

            entity1.FacilityFacilityArea.FacilityArea = GetEntityFactory<FacilityArea>().Create();
            entity0.ProductionWorkOrderRequiresSupervisorApproval = new ProductionWorkOrderRequiresSupervisorApproval {
                RequiresSupervisorApproval = false
            };
            entity1.ProductionWorkOrderRequiresSupervisorApproval = new ProductionWorkOrderRequiresSupervisorApproval {
                RequiresSupervisorApproval = true
            };

            entity0.EstimatedCompletionHours = 5.55M;
            entity0.EmployeeAssignments.Add(GetEntityFactory<EmployeeAssignment>().Create(new { HoursWorked = 1.23M }));
            entity0.EmployeeAssignments.Add(GetEntityFactory<EmployeeAssignment>().Create(new { HoursWorked = 4.56M }));

            var search = new SearchProductionWorkOrder();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id.ToString(), "Id");
                helper.AreEqual(entity1.Id.ToString(), "Id", 1);
                helper.AreEqual("False", "AirPermit");
                helper.AreEqual("False", "AirPermit", 1);
                helper.AreEqual("False", "HasLockoutRequirement");
                helper.AreEqual("False", "HasLockoutRequirement", 1);
                helper.AreEqual("False", "LockoutFormCreated");
                helper.AreEqual("True", "LockoutFormCreated", 1);
                helper.AreEqual(entity1.FacilityFacilityArea.FacilityArea.Description, "FacilityArea", 1);
                helper.AreEqual("False", "ProductionWorkOrderRequiresSupervisorApproval");
                helper.AreEqual("True", "ProductionWorkOrderRequiresSupervisorApproval", 1);
                helper.AreEqual("5.55", "EstimatedCompletionHours");
                helper.AreEqual("0", "EstimatedCompletionHours", 1);
                helper.AreEqual("5.79", "ActualCompletionHours");
                helper.AreEqual("0", "ActualCompletionHours", 1);
            }
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsForUnscheduledPerformanceSearchType()
        {
            // This should return only orders that do not have any EmployeeAssignments and
            // have not been cancelled or completed.

            var orderWithoutAssignments = GetEntityFactory<ProductionWorkOrder>().Create();
            var cancelledOrderWithoutAssignments = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCancelled = DateTime.Now });
            var completedOrderWithoutAssignments = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCompleted = DateTime.Now });
            var orderWithAssignment = GetEntityFactory<ProductionWorkOrder>().Create();
            GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Now,
                ProductionWorkOrder = orderWithAssignment
            });

            var search = new SearchProductionWorkOrder {
                PerformanceSearchType = PerformanceSearchType.Unscheduled
            };
            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(orderWithoutAssignments));
            Assert.IsFalse(search.Results.Contains(cancelledOrderWithoutAssignments));
            Assert.IsFalse(search.Results.Contains(completedOrderWithoutAssignments));
            Assert.IsFalse(search.Results.Contains(orderWithAssignment));
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsForScheduledPerformanceSearchType()
        {
            // This should return only orders that are not cancelled, not completed, and
            // have at least one employee assignment that has not been started.

            var orderWithUnstartedAssignment = GetEntityFactory<ProductionWorkOrder>().Create();
            GetEntityFactory<EmployeeAssignment>().Create(new {
                ProductionWorkOrder = orderWithUnstartedAssignment
            });
            var cancelledOrderWithUnstartedAssignment = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCancelled = DateTime.Now });
            GetEntityFactory<EmployeeAssignment>().Create(new {
                ProductionWorkOrder = cancelledOrderWithUnstartedAssignment
            });
            var completedOrderWithUnstartedAssignment = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCompleted = DateTime.Now });
            GetEntityFactory<EmployeeAssignment>().Create(new {
                ProductionWorkOrder = completedOrderWithUnstartedAssignment
            });
            var orderWithStartedAssignment = GetEntityFactory<ProductionWorkOrder>().Create();
            GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Now,
                ProductionWorkOrder = orderWithStartedAssignment
            });
            var orderWithoutAnyAssignments = GetEntityFactory<ProductionWorkOrder>().Create();

            var search = new SearchProductionWorkOrder {
                PerformanceSearchType = PerformanceSearchType.Scheduled
            };
            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(orderWithUnstartedAssignment));
            Assert.IsFalse(search.Results.Contains(cancelledOrderWithUnstartedAssignment));
            Assert.IsFalse(search.Results.Contains(completedOrderWithUnstartedAssignment));
            Assert.IsFalse(search.Results.Contains(orderWithStartedAssignment));
            Assert.IsFalse(search.Results.Contains(orderWithoutAnyAssignments));
        }

        [TestMethod]
        public void TestIndexReturnsConfinedFormCreatedSearch()
        {
            var entityWithNoConfinedSpaceForm = GetEntityFactory<ProductionWorkOrder>().Create();

            var entityWithConfinedSpaceFormCreated = GetEntityFactory<ProductionWorkOrder>().Create();
            var prerequisite = GetFactory<IsConfinedSpaceProductionPrerequisiteFactory>().Create();
            var pwoPrereq = GetEntityFactory<ProductionWorkOrderProductionPrerequisite>().Create(new
                {ProductionPrerequisite = prerequisite, ProductionWorkOrder = entityWithConfinedSpaceFormCreated });
            var confinedSpaceForm = GetEntityFactory<ConfinedSpaceForm>().Create(new {ProductionWorkOrder = entityWithConfinedSpaceFormCreated });

            var search = new SearchProductionWorkOrder {
                ConfinedSpaceForms = true
            };
            _target.Index(search);
            Assert.IsTrue(search.Results.Contains(entityWithConfinedSpaceFormCreated));
            Assert.IsFalse(search.Results.Contains(entityWithNoConfinedSpaceForm));
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsForHasAssignmentsOnNonCancelledWorkOrderSearchType()
        {
            // This should return only orders that have EmployeeAssignments and have not been cancelled 

            var orderWithoutAssignments = GetEntityFactory<ProductionWorkOrder>().Create();
            var cancelledOrderWithoutAssignments = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCancelled = DateTime.Now });
            var cancelledOrderWithAssignments = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCancelled = DateTime.Now });
            var orderWithAssignments = GetEntityFactory<ProductionWorkOrder>().Create();
            GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Now,
                ProductionWorkOrder = orderWithAssignments
            });
            GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Now,
                ProductionWorkOrder = cancelledOrderWithAssignments
            });

            var search = new SearchProductionWorkOrder {
                HasAssignmentsOnNonCancelledWorkOrder = true
            };
            _target.Index(search);

            Assert.IsFalse(search.Results.Contains(orderWithoutAssignments));
            Assert.IsFalse(search.Results.Contains(cancelledOrderWithoutAssignments));
            Assert.IsFalse(search.Results.Contains(cancelledOrderWithAssignments));
            Assert.IsTrue(search.Results.Contains(orderWithAssignments));
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsForHasAssignmentsOnNonCancelledWorkOrderSearchTypeWhenFalse()
        {
            // This should return only orders that do Not have EmployeeAssignments and have not been cancelled 

            var orderWithoutAssignments = GetEntityFactory<ProductionWorkOrder>().Create();
            var cancelledOrderWithoutAssignments = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCancelled = DateTime.Now });
            var cancelledOrderWithAssignments = GetEntityFactory<ProductionWorkOrder>().Create();
            var orderWithAssignment = GetEntityFactory<ProductionWorkOrder>().Create();
            GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Now,
                ProductionWorkOrder = orderWithAssignment
            });
            GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Now,
                ProductionWorkOrder = cancelledOrderWithAssignments
            });

            var search = new SearchProductionWorkOrder {
                HasAssignmentsOnNonCancelledWorkOrder = false
            };
            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(orderWithoutAssignments));
            Assert.IsFalse(search.Results.Contains(cancelledOrderWithoutAssignments));
            Assert.IsFalse(search.Results.Contains(cancelledOrderWithAssignments));
            Assert.IsFalse(search.Results.Contains(orderWithAssignment));
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsForIncompletePerformanceSearchType()
        {
            // This should return only orders that are not cancelled, not completed, and
            // has employee assignments that have been started. It does not matter if the
            // assignment has been finished or not.

            var orderWithStartedAssignmentThatHasNotEnded = GetEntityFactory<ProductionWorkOrder>().Create();
            GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Now,
                ProductionWorkOrder = orderWithStartedAssignmentThatHasNotEnded
            });
            var orderWithStartedAssignmentThatHasEnded = GetEntityFactory<ProductionWorkOrder>().Create();
            GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Now,
                DateEnded = DateTime.Now,
                ProductionWorkOrder = orderWithStartedAssignmentThatHasEnded
            });
            var cancelledOrderWithStartedAssignment = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCancelled = DateTime.Now });
            GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Now,
                ProductionWorkOrder = cancelledOrderWithStartedAssignment
            });
            var completedOrderWithStartedAssignment = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCompleted = DateTime.Now });
            GetEntityFactory<EmployeeAssignment>().Create(new {
                DateStarted = DateTime.Now,
                ProductionWorkOrder = completedOrderWithStartedAssignment
            });

            var search = new SearchProductionWorkOrder {
                PerformanceSearchType = PerformanceSearchType.Incomplete
            };
            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(orderWithStartedAssignmentThatHasNotEnded));
            Assert.IsTrue(search.Results.Contains(orderWithStartedAssignmentThatHasEnded));
            Assert.IsFalse(search.Results.Contains(cancelledOrderWithStartedAssignment));
            Assert.IsFalse(search.Results.Contains(completedOrderWithStartedAssignment));
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsForCanceledPerformanceSearchType()
        {
            // This should return only orders that are canceled.

            var orderThatIsCancelled = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCancelled = DateTime.Now });
            var orderThatIsNotCancelled = GetEntityFactory<ProductionWorkOrder>().Create();

            var search = new SearchProductionWorkOrder {
                PerformanceSearchType = PerformanceSearchType.Canceled
            };
            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(orderThatIsCancelled));
            Assert.IsFalse(search.Results.Contains(orderThatIsNotCancelled));
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsForCompletedPerformanceSearchType()
        {
            // This should return only orders that are completed.

            var orderThatIsCompleted = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCompleted = DateTime.Now });
            var orderThatIsNotCompleted = GetEntityFactory<ProductionWorkOrder>().Create();

            var search = new SearchProductionWorkOrder {
                PerformanceSearchType = PerformanceSearchType.Completed
            };
            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(orderThatIsCompleted));
            Assert.IsFalse(search.Results.Contains(orderThatIsNotCompleted));
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsForNotApprovedPerformanceSearchType()
        {
            // This should return only orders that are completed.

            var orderThatIsCompletedAndNotApproved = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCompleted = DateTime.Now });
            var orderThatIsCompletedAndApproved = GetEntityFactory<ProductionWorkOrder>().Create(new { DateCompleted = DateTime.Now, ApprovedOn = DateTime.Now });

            var search = new SearchProductionWorkOrder {
                PerformanceSearchType = PerformanceSearchType.NotApproved
            };
            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(orderThatIsCompletedAndNotApproved));
            Assert.IsFalse(search.Results.Contains(orderThatIsCompletedAndApproved));
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsForState()
        {
            var state = GetEntityFactory<State>().Create(new { Abbreviation = "QQ" });
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create(new { State = state });
            var orderWithState = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = opc });
            var orderWithDifferentState = GetEntityFactory<ProductionWorkOrder>().Create();
            
            var search = new SearchProductionWorkOrder {
                State = state.Id
            };
            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(orderWithState));
            Assert.IsFalse(search.Results.Contains(orderWithDifferentState));
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsWhenThePlanningPlantIsNullPropertyIsTrue()
        {
            // This should return only orders that do not have any EmployeeAssignments and
            // have not been cancelled or completed.

            var orderWithPlanningPlant = GetEntityFactory<ProductionWorkOrder>().Create(new {
                PlanningPlant = typeof(PlanningPlantFactory)
            });
            var orderWithoutPlanningPlant = GetEntityFactory<ProductionWorkOrder>().Create(new {
                PlanningPlant = (PlanningPlant)null
            });

            var search = new SearchProductionWorkOrder {
                PerformanceSearchType = PerformanceSearchType.Created,
                PlanningPlantIsNull = true
            };
            _target.Index(search);

            Assert.IsTrue(search.Results.Contains(orderWithoutPlanningPlant));
            Assert.IsFalse(search.Results.Contains(orderWithPlanningPlant));
        }

        [TestMethod]
        public void TestIndexReturnsExpectedResultsForOperatingCenters()
        {
            var nj7 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var ny1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { OperatingCenterCode = "NY1", Id = 20 });
            var workDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = _orderTypes.Single(x => x.Id == OrderType.Indices.RP_CAPITAL_40) });
            var workDescription1 = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = _orderTypes.Single(x => x.Id == OrderType.Indices.CORRECTIVE_ACTION_20) });
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new
                { OperatingCenter = nj7, ProductionWorkDescription = workDescription, DateCompleted = DateTime.Now });
            var pwo1 = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = ny1, ProductionWorkDescription = workDescription1, DateCompleted = DateTime.Now });
            Session.Evict(pwo);
            Session.Evict(pwo1);
            pwo = Session.Load<ProductionWorkOrder>(pwo.Id);
            pwo1 = Session.Load<ProductionWorkOrder>(pwo1.Id);
            Assert.IsTrue(pwo.ProductionWorkOrderRequiresSupervisorApproval.RequiresSupervisorApproval);
            Assert.IsTrue(pwo1.ProductionWorkOrderRequiresSupervisorApproval.RequiresSupervisorApproval);
            var role = GetFactory<RoleFactory>().Create(new {
                Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Production }),
                Module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.ProductionWorkManagement, Application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.Production })}),
                Action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read }),
                User = _currentUser,
                OperatingCenter = nj7
            });
            Assert.IsTrue(_currentUser.Roles.Contains(role));
            _currentUser.IsAdmin = false;
            var search = new SearchProductionWorkOrder {
                OperatingCenter = null,
                RequiresSupervisorApproval = true
            };
            
            _target.Index(search);
            Assert.IsTrue(search.Results.Contains(pwo));
            Assert.IsFalse(search.Results.Contains(pwo1));
        }

        [TestMethod]
        public void TestIndexReturnsResultsForFrag()
        {
            var eq1 = GetEntityFactory<Equipment>().Create();
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(new {
                Facility = eq1.Facility
            });
            GetFactory<ProductionWorkOrderEquipmentFactory>().Create(new {
                Equipment = eq1,
                ProductionWorkOrder = productionWorkOrder
            });
            Session.Flush();
            var search = new SearchProductionWorkOrder { Equipments = eq1.Facility.Id };
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.FRAGMENT;

            var result = _target.Index(search);

            MvcAssert.IsPartialView(result);
            MvcAssert.IsViewNamed(result, "_Index");
            Assert.IsTrue(search.Results.Contains(productionWorkOrder));
            Assert.AreEqual(1, search.Results.Count());
        }

        [TestMethod]
        public void TestIndexReturnsNoResultsFrag()
        {
            var search = new SearchProductionWorkOrder();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.FRAGMENT;

            var result = _target.Index(search);

            MvcAssert.IsPartialView(result);
            MvcAssert.IsViewNamed(result, "_NoResults");
        }

        #endregion

        #region New/Create

        [TestMethod]
        public void TestCreateSendsNotificationIfEquipmentIsLinkedToAnEnvironmentalPermit()
        {
            var notificationPurpose = ProductionWorkOrderController.PERMIT_NOTIFICATION;
            // ARRANGE
            var existing = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter
            });
            var equipment = GetEntityFactory<Equipment>().Create();
            var permit = GetEntityFactory<EnvironmentalPermit>().Create(new { Equipment = new[] { equipment }});
            equipment.EnvironmentalPermits.Add(permit);
            var entity = _viewModelFactory.Build<CreateProductionWorkOrder, ProductionWorkOrder>(existing);
            entity.Equipment = equipment.Id;
            ActionResult result = null;

            // ACT
            _target.Create(entity);
            
            // ASSERT
            _notificationService.Verify(
                x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose)), Times.Once);
        }

        [TestMethod]
        public void TestCreateDoesNotSendNotificationIfEquipmentIsNotLinkedToAnEnvironmentalPermit()
        {
            var notificationPurpose = ProductionWorkOrderController.PERMIT_NOTIFICATION;
            // ARRANGE
            var existing = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter
            });
            var entity = _viewModelFactory.Build<CreateProductionWorkOrder, ProductionWorkOrder>(existing);
            ActionResult result = null;

            // ACT
            _target.Create(entity);

            // ASSERT
            _notificationService.Verify(
                x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose)), Times.Never);
        }

        [TestMethod]
        public void TestCreateDoesSendWorkOrderCreatedNotification()
        {
            var notificationPurpose = ProductionWorkOrderController.CREATE_NOTIFICATION;
            var existing = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter
            });
            var entity = _viewModelFactory.Build<CreateProductionWorkOrder, ProductionWorkOrder>(existing);

            _target.Create(entity);

            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose)), Times.Once);
        }

        [TestMethod]
        public void TestCreateSendsNotificationToTheCurrentUserEmployeeEmailAddressIfAssignToSelfIsTrue()
        {
            var notificationPurpose = ProductionWorkOrderController.ASSIGNED_NOTIFICATION;
            var existing = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter
            });
            var viewModel = _viewModelFactory.Build<CreateProductionWorkOrder, ProductionWorkOrder>(existing);

            // Test that it sends when false
            viewModel.AssignToSelf = false;
            _target.Create(viewModel);
            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose && a.Address == _employee.EmailAddress)), Times.Never);

            // Test that it sends when true
            viewModel.AssignToSelf = true;
            _target.Create(viewModel);
            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose && a.Address == _employee.EmailAddress)), Times.Once);
        }

        [TestMethod]
        public void TestCreateDisplaysNotificationErrorIfTheAssignedEmployeeDoesNotHaveAnEmailAddress()
        {
            var notificationPurpose = ProductionWorkOrderController.ASSIGNED_NOTIFICATION;
            _currentUser.Employee.EmailAddress = null;
            var existing = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter
            });
            var viewModel = _viewModelFactory.Build<CreateProductionWorkOrder, ProductionWorkOrder>(existing);
            viewModel.AssignToSelf = true;
            _target.Create(viewModel);

            _target.AssertTempDataContainsMessage($"Unable to send assignment notification to {_employee.FullName} because their employee record is missing an email address.", MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);
            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose)), Times.Never);
        }

        [TestMethod]
        public void TestCreateFromPlanRedirectsToTheProductionWorkOrderShowPageAfterSuccessfullySaving()
        {
            var priority = GetFactory<RoutineOffScheduledProductionWorkOrderPriorityFactory>().Create();
            var pwo = GetFactory<ProductionWorkOrderFactory>().Create(new { Priority = priority });
            var mp = GetFactory<MaintenancePlanFactory>().Create();
            var model = _viewModelFactory.BuildWithOverrides<CreateProductionWorkOrderFromPlan, ProductionWorkOrder>(pwo, new {
                MaintenancePlan = mp.Id
            });

            var result = _target.CreateProductionWorkOrderFromPlan(model);

            MvcAssert.RedirectsToRoute(result, "ProductionWorkOrder", "Show", new { id = model.Id });
        }

        [TestMethod]
        public void TestCreateProductionWorkOrderFromPlanSavesNewRecordWhenModelStateIsValid()
        {
            var priority = GetFactory<RoutineOffScheduledProductionWorkOrderPriorityFactory>().Create();
            var pwo = GetFactory<ProductionWorkOrderFactory>().Create(new { Priority = priority });
            var mp = GetFactory<MaintenancePlanFactory>().Create();
            var model = _viewModelFactory.BuildWithOverrides<CreateProductionWorkOrderFromPlan, ProductionWorkOrder>(pwo, new {
                MaintenancePlan = mp.Id
            });

            Assert.IsTrue(_target.ModelState.IsValid);
            ActionResult result;

            MyAssert.CausesIncrease(
                () => result = _target.CreateProductionWorkOrderFromPlan(model),
                () => Repository.GetAll().Count());
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<ProductionWorkOrder>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditProductionWorkOrder, ProductionWorkOrder>(eq, new {
                OrderNotes = expected
            }));

            Assert.AreEqual(expected, Session.Get<ProductionWorkOrder>(eq.Id).OrderNotes);
        }

        #endregion

        #region Account Completion Actions

        #region Complete

        [TestMethod]
        public void TestCompleteProductionWorkOrderCallsProgressSAPWhenType40()
        {
            _sapProgressRepo.Setup(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>())).Returns(new SAPProgressUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var orderTypes = GetEntityFactory<OrderType>().CreateList(4);
            var productionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypes[OrderType.Indices.RP_CAPITAL_40 - 1] });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = operatingCenter, ProductionWorkDescription = productionWorkDescription });
            var viewModel = _viewModelFactory.Build<CompleteProductionWorkOrder, ProductionWorkOrder>(order);

            var result = _target.CompleteProductionWorkOrder(viewModel) as RedirectToRouteResult;
            Session.Evict(order);
            order = Session.Load<ProductionWorkOrder>(order.Id);

            _sapProgressRepo.Verify(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>()));
            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCompleteProductionWorkOrderCallsProgressSAPWhenType20or40AndHasMaterials()
        {
            _sapProgressRepo.Setup(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>())).Returns(new SAPProgressUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var orderTypes = GetEntityFactory<OrderType>().CreateList(4);
            var productionWorkDescription20 = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypes[OrderType.Indices.CORRECTIVE_ACTION_20 - 1] });
            var productionWorkDescription40 = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypes[OrderType.Indices.RP_CAPITAL_40 - 1] });
            var productionWorkDescriptionList = new List<ProductionWorkDescription> { productionWorkDescription20, productionWorkDescription40 };
            foreach (var productionWorkDescription in productionWorkDescriptionList)
            {
                var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(
                    new {
                        OperatingCenter = operatingCenter,
                        ProductionWorkDescription = productionWorkDescription
                    });
                var productionWorkOrderMaterialUsedList = GetEntityFactory<ProductionWorkOrderMaterialUsed>()
                   .CreateSet(4, new { ProductionWorkOrder = productionWorkOrder });
                productionWorkOrder.ProductionWorkOrderMaterialUsed = productionWorkOrderMaterialUsedList;

                var viewModel = _viewModelFactory.Build<CompleteProductionWorkOrder, ProductionWorkOrder>(productionWorkOrder);
                var result = _target.CompleteProductionWorkOrder(viewModel) as RedirectToRouteResult;
                Session.Evict(productionWorkOrder);
                productionWorkOrder = Session.Load<ProductionWorkOrder>(productionWorkOrder.Id);

                _sapProgressRepo.Verify(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>()));
                Assert.IsNotNull(result);
                Assert.AreEqual("Show", result.RouteValues["action"]);
            }
        }

        [TestMethod]
        public void TestCompleteProductionWorkOrderCallsProgressAndFinalizeSAPWhenType20Or40AndHasNOMaterials()
        {
            _sapProgressRepo.Setup(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>())).Returns(new SAPProgressUnscheduledWorkOrder { Status = "Success" });
            _sapCompleteRepo.Setup(x => x.Save(It.IsAny<SAPCompleteUnscheduledWorkOrder>())).Returns(new SAPCompleteUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var orderTypes = GetEntityFactory<OrderType>().CreateList(4);
            var productionWorkDescription20 = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypes[OrderType.Indices.CORRECTIVE_ACTION_20 - 1] });
            var productionWorkDescription40 = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypes[OrderType.Indices.RP_CAPITAL_40 - 1] });
            var productionWorkDescriptionList = new List<ProductionWorkDescription> { productionWorkDescription20, productionWorkDescription40 };
            foreach (var productionWorkDescription in productionWorkDescriptionList)
            {
                var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(
                    new {
                        OperatingCenter = operatingCenter,
                        ProductionWorkDescription = productionWorkDescription
                    });
                var viewModel = _viewModelFactory.Build<CompleteProductionWorkOrder, ProductionWorkOrder>(productionWorkOrder);
                var result = _target.CompleteProductionWorkOrder(viewModel) as RedirectToRouteResult;
                Session.Evict(productionWorkOrder);
                productionWorkOrder = Session.Load<ProductionWorkOrder>(productionWorkOrder.Id);
                _sapProgressRepo.Verify(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>()));
                _sapCompleteRepo.Verify(x => x.Save(It.IsAny<SAPCompleteUnscheduledWorkOrder>()));

                Assert.IsNotNull(result);
                Assert.AreEqual("Show", result.RouteValues["action"]);
            }
        }

        [TestMethod]
        public void TestCompleteDoesSendCompleteNotification()
        {
            var notificationPurpose = ProductionWorkOrderController.COMPLETED_NOTIFICATION;
            var existing = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter
            });
            var entity = _viewModelFactory.Build<CompleteProductionWorkOrder, ProductionWorkOrder>(existing);

            _target.CompleteProductionWorkOrder(entity);

            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose)), Times.Once);
        }

        #endregion

        #region RedTagAuthorization

        [TestMethod]
        public void TestRedTagAuthorizationRedirectsToShow()
        {
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(new {
                NeedsRedTagPermitAuthorization = true
            });

            var viewModel = _viewModelFactory.Build<RedTagPermitAuthorizationViewModel, ProductionWorkOrder>(productionWorkOrder);
            var result = (RedirectToRouteResult)_target.RedTagPermitAuthorizationForProductionWorkOrder(viewModel);

            MvcAssert.RedirectsToRoute(result, "Show", new { id = productionWorkOrder.Id });
        }

        #endregion

        #region SupervisorApproval

        [TestMethod]
        public void TestSupervisorApproveProductionWorkOrderCallsFinalizesWhenTypeRPCapital40()
        {
            _sapCompleteRepo.Setup(x => x.Save(It.IsAny<SAPCompleteUnscheduledWorkOrder>())).Returns(new SAPCompleteUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var productionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = _orderTypes.Single(x => x.Id == OrderType.Indices.RP_CAPITAL_40) });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = operatingCenter,
                ProductionWorkDescription = productionWorkDescription,
                DateCompleted = DateTime.Now
            });
            Assert.IsTrue(order.CanBeSupervisorApproved, "Sanity");
            var viewModel = _viewModelFactory.Build<SupervisorApproveProductionWorkOrder, ProductionWorkOrder>(order);

            var result = (RedirectToRouteResult)_target.SupervisorApproveProductionWorkOrder(viewModel);

            _sapCompleteRepo.Verify(x => x.Save(It.IsAny<SAPCompleteUnscheduledWorkOrder>()));
            MvcAssert.RedirectsToRoute(result, "Show", new { id = order.Id });
        }

        [TestMethod]
        public void TestSupervisorApproveProductionWorkOrderDoesNotCallAnySAPMethodsWhenTypeOperationalActivity10()
        {
            _sapCompleteRepo.VerifyNoOtherCalls();

            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var productionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = _orderTypes.Single(x => x.Id == OrderType.Indices.OPERATIONAL_ACTIVITY_10) });
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(
                new {
                    OperatingCenter = operatingCenter,
                    ProductionWorkDescription = productionWorkDescription,
                    DateCompleted = DateTime.Now,
                });
            Assert.IsFalse(productionWorkOrder.CanBeSupervisorApproved, "Sanity check");

            var viewModel =
                _viewModelFactory.Build<SupervisorApproveProductionWorkOrder, ProductionWorkOrder>(
                    productionWorkOrder);

            var result = (RedirectToRouteResult)_target.SupervisorApproveProductionWorkOrder(viewModel);
            MvcAssert.RedirectsToRoute(result, "Show", new { id = productionWorkOrder.Id });
        }

        [TestMethod]
        public void TestSupervisorApproveProductionWorkOrderCallsFinalizeWhenTypeCorrectiveAction20AndNoMaterials()
        {
            _sapCompleteRepo.Setup(x => x.Save(It.IsAny<SAPCompleteUnscheduledWorkOrder>())).Returns(new SAPCompleteUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var productionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = _orderTypes.Single(x => x.Id == OrderType.Indices.CORRECTIVE_ACTION_20) });
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(
                new {
                    OperatingCenter = operatingCenter,
                    ProductionWorkDescription = productionWorkDescription,
                    DateCompleted = DateTime.Now,
                });

            Assert.IsTrue(productionWorkOrder.CanBeSupervisorApproved, "Sanity check");
            var viewModel =
                _viewModelFactory.Build<SupervisorApproveProductionWorkOrder, ProductionWorkOrder>(
                    productionWorkOrder);
            var result = _target.SupervisorApproveProductionWorkOrder(viewModel) as RedirectToRouteResult;
            Session.Evict(productionWorkOrder);
            productionWorkOrder = Session.Load<ProductionWorkOrder>(productionWorkOrder.Id);

            _sapCompleteRepo.Verify(x => x.Save(It.IsAny<SAPCompleteUnscheduledWorkOrder>()));
            MvcAssert.RedirectsToRoute(result, "Show", new { id = productionWorkOrder.Id });
        }

        [TestMethod]
        public void TestSupervisorApproveProductionWorkOrderCallsProgressWhenTypeCorrective20AndHasMaterials()
        {
            _sapProgressRepo.Setup(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>())).Returns(new SAPProgressUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var productionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = _orderTypes.Single(x => x.Id == OrderType.Indices.CORRECTIVE_ACTION_20) });
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create(
                new {
                    OperatingCenter = operatingCenter,
                    ProductionWorkDescription = productionWorkDescription,
                    DateCompleted = DateTime.Now
                });
            Assert.IsTrue(productionWorkOrder.CanBeSupervisorApproved, "Sanity check");
            var productionWorkOrderMaterialUsedList = GetEntityFactory<ProductionWorkOrderMaterialUsed>()
               .CreateSet(4, new { ProductionWorkOrder = productionWorkOrder });
            productionWorkOrder.ProductionWorkOrderMaterialUsed = productionWorkOrderMaterialUsedList;

            var viewModel = _viewModelFactory.Build<SupervisorApproveProductionWorkOrder, ProductionWorkOrder>(productionWorkOrder);
            var result = _target.SupervisorApproveProductionWorkOrder(viewModel) as RedirectToRouteResult;
            Session.Evict(productionWorkOrder);
            productionWorkOrder = Session.Load<ProductionWorkOrder>(productionWorkOrder.Id);

            _sapProgressRepo.Verify(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>()));
            MvcAssert.RedirectsToRoute(result, "Show", new { id = productionWorkOrder.Id });
        }

        #endregion

        #region SAP

        [TestMethod]
        public void TestRejectProductionWorkOrderCallsProgressSAP()
        {
            _sapProgressRepo.Setup(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>())).Returns(new SAPProgressUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = operatingCenter });
            Session.Save(order);
            var model = _viewModelFactory.Build<RejectProductionWorkOrder, ProductionWorkOrder>(order);

            var result = _target.RejectProductionWorkOrder(model) as RedirectToRouteResult;
            order = Session.Load<ProductionWorkOrder>(order.Id);

            _sapProgressRepo.Verify(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>()));
            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestProductionWorkOrderDoesntCallsProgressSAPWhenSAPEnabledIsFalse()
        {
            _sapProgressRepo.Setup(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>())).Returns(new SAPProgressUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = false });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = operatingCenter });
            Session.Save(order);
            var model = _viewModelFactory.Build<RejectProductionWorkOrder, ProductionWorkOrder>(order);

            var result = _target.RejectProductionWorkOrder(model) as RedirectToRouteResult;
            order = Session.Load<ProductionWorkOrder>(order.Id);

            _sapProgressRepo.Verify(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>()), Times.Never);
            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestApproveMaterialWorkOrderCallsFinalizeSAP()
        {
            _sapCompleteRepo.Setup(x => x.Save(It.IsAny<SAPCompleteUnscheduledWorkOrder>())).Returns(new SAPCompleteUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = operatingCenter });
            Session.Save(order);
            var model = _viewModelFactory.Build<ApproveMaterialWorkOrder, ProductionWorkOrder>(order);

            var result = _target.ApproveMaterialWorkOrder(model) as RedirectToRouteResult;
            order = Session.Load<ProductionWorkOrder>(order.Id);

            _sapCompleteRepo.Verify(x => x.Save(It.IsAny<SAPCompleteUnscheduledWorkOrder>()));
            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCancelProductionWorkOrderCallsProgressSAP()
        {
            _sapProgressRepo.Setup(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>())).Returns(new SAPProgressUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = operatingCenter });
            Session.Save(order);
            var model = _viewModelFactory.BuildWithOverrides<CancelProductionWorkOrder, ProductionWorkOrder>(order, new {
                CancellationReason = _companyError.Id,
            });

            var result = _target.CancelProductionWorkOrder(model) as RedirectToRouteResult;
            order = Session.Load<ProductionWorkOrder>(order.Id);

            _sapProgressRepo.Verify(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>()));
            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCapitalizeProductionWorkOrderCallsProgressSAPWhenCapitalizationCancelsOrder()
        {
            _sapProgressRepo.Setup(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>())).Returns(new SAPProgressUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = operatingCenter });
            Session.Save(order);
            var model = _viewModelFactory.BuildWithOverrides<CapitalizeProductionWorkOrder, ProductionWorkOrder>(order, new {
                CancellationReason = _companyError.Id,
                CapitalizationCancelsOrder = true,
                CapitalizationReason = "Test"
            });

            var result = _target.CapitalizeProductionWorkOrder(model) as RedirectToRouteResult;
            order = Session.Load<ProductionWorkOrder>(order.Id);

            _sapProgressRepo.Verify(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>()));
            Assert.IsNotNull(result);
            Assert.AreEqual(model.CapitalizationCancelsOrder, true);
            Assert.AreEqual("New", result.RouteValues["action"]);
            Assert.AreEqual(model.CapitalizationReason, order.CapitalizationReason);
        }

        [TestMethod]
        public void TestCapitalizeProductionWorkOrderCallsFinalizeSAPWhenCapitalizationDoesNotCancelOrder()
        {
            _sapCompleteRepo.Setup(x => x.Save(It.IsAny<SAPCompleteUnscheduledWorkOrder>())).Returns(new SAPCompleteUnscheduledWorkOrder { Status = "Success" });
            var employee = GetEntityFactory<Employee>().Create();
            var productionWorkOrder = GetEntityFactory<ProductionWorkOrder>().Create();
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            var employeeAssignments = GetEntityFactory<EmployeeAssignment>().CreateSet(2, new {
                AssignedTo = employee,
                ProductionWorkOrder = productionWorkOrder,
                DateEnded = DateTime.Today
            });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = operatingCenter, EmployeeAssignments = employeeAssignments });
            Session.Save(order);
            var model = _viewModelFactory.BuildWithOverrides<CapitalizeProductionWorkOrder, ProductionWorkOrder>(order, new {
                CapitalizationCancelsOrder = false,
            });

            var result = _target.CapitalizeProductionWorkOrder(model) as RedirectToRouteResult;
            order = Session.Load<ProductionWorkOrder>(order.Id);

            _sapCompleteRepo.Verify(x => x.Save(It.IsAny<SAPCompleteUnscheduledWorkOrder>()));
            Assert.IsNotNull(result);
            Assert.AreEqual(model.CapitalizationCancelsOrder, false);
            Assert.AreEqual("New", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TestCapitalizeProductionWorkOrderRedirectToNewWithCreateProductionWorkOrder()
        {
            _sapProgressRepo.Setup(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>())).Returns(new SAPProgressUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = operatingCenter });
            Session.Save(order);
            var model = _viewModelFactory.BuildWithOverrides<CapitalizeProductionWorkOrder, ProductionWorkOrder>(order, new {
                CancellationReason = _companyError.Id,
                CapitalizationCancelsOrder = true,
                CapitalizationReason = "Test"
            });

            var result = _target.CapitalizeProductionWorkOrder(model) as RedirectToRouteResult;
            order = Session.Load<ProductionWorkOrder>(order.Id);

            _sapProgressRepo.Verify(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>()));
            Assert.IsNotNull(result);
            Assert.AreEqual("New", result.RouteValues["action"]);
            Assert.AreEqual(model.CapitalizationReason, order.CapitalizationReason);
        }

        [TestMethod]
        public void TestCompletesMaterialPlanningIfNoSAPErrorAndSetsMaterialsPlannedOn()
        {
            _sapProgressRepo.Setup(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>())).Returns(new SAPProgressUnscheduledWorkOrder { Status = "Success" });
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPEnabled = true, IsContractedOperations = false });
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new { OperatingCenter = operatingCenter });
            Session.Save(order);
            var model = _viewModelFactory.Build<CompleteProductionMaterialPlanning, ProductionWorkOrder>(order);

            var result = _target.CompleteMaterialPlanning(model) as RedirectToRouteResult;
            order = Session.Load<ProductionWorkOrder>(order.Id);

            _sapProgressRepo.Verify(x => x.Save(It.IsAny<SAPProgressUnscheduledWorkOrder>()));
            Assert.IsNotNull(result);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        #endregion

        #endregion

        #region Lookups

        [TestMethod]
        public void TestProductionWorkOrderDropDownLookupForNewSetsActiveOC()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = true });
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsActive = false });

            _authenticationService.Setup(x => x.CurrentUserIsAdmin).Returns(true);

            _target.SetLookupData(ControllerAction.New);

            var opcDropDownData = (IEnumerable<SelectListItem>)_target.ViewData["OperatingCenter"];

            var authenticationServiceOc = _authenticationService.Object.CurrentUser.DefaultOperatingCenter.Id.ToString();

            // Need to filter out for our specific test OC's since two OC's get created on the _authenticationservice mock and GetUser()

            var DropDownData = opcDropDownData.Where(x => x.Value != _operatingCenter.Id.ToString() && x.Value != authenticationServiceOc);

            Assert.AreEqual(1, DropDownData.Count());
            Assert.AreEqual(opc1.Id.ToString(), DropDownData.First().Value);
            Assert.AreNotEqual(opc2.Id.ToString(), DropDownData.First().Value);
        }

        [TestMethod]
        public void TestProductionWorkOrderDropDownLookupForSearchSetsActiveTaskType()
        {
            var tt1 = GetFactory<MaintenancePlanTaskTypeFactory>().Create(new { IsActive = true });
            var tt2 = GetFactory<MaintenancePlanTaskTypeFactory>().Create(new { IsActive = false });

            _target.SetLookupData(ControllerAction.Search);

            var ttDropDownData = (IEnumerable<SelectListItem>)_target.ViewData["MaintenancePlanTaskType"];
            
            Assert.AreEqual(1, ttDropDownData.Count());
            Assert.AreEqual(tt1.Id.ToString(), ttDropDownData.First().Value);
            Assert.AreNotEqual(tt2.Id.ToString(), ttDropDownData.First().Value);
        }

        #endregion

        #region EmployeeAssignment

        [TestMethod]
        public void TestAddEmployeeAssignmentSendsNotificationToTheAssignedEmployeesEmailAddress()
        {
            var notificationPurpose = ProductionWorkOrderController.ASSIGNED_NOTIFICATION;
            var employee = GetEntityFactory<Employee>().Create(new { EmailAddress = "employee@work.com" });
            var employeeAssignment = GetEntityFactory<EmployeeAssignment>().Create(new { AssignedTo = employee });
            var existing = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter,
            });

            var entity = _viewModelFactory.BuildWithOverrides<AddEmployeeAssignmentProductionWorkOrder, ProductionWorkOrder>(existing, new {
                AssignedTo = employeeAssignment.AssignedTo.Id,
                AssignedFor = DateTime.Now
            });

            _target.AddEmployeeAssignment(entity);

            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose && a.Address == employee.EmailAddress)), Times.Once);
        }

        [TestMethod]
        public void TestAddEmployeeAssignmentDisplaysNotificationErrorIfTheAssignedEmployeeDoesNotHaveAnEmailAddress()
        {
            var employee = GetEntityFactory<Employee>().Create();
            employee.EmailAddress = null;
            var employeeAssignment = GetEntityFactory<EmployeeAssignment>().Create(new { AssignedTo = employee });
            var existing = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter,
            });
            var entity = _viewModelFactory.BuildWithOverrides<AddEmployeeAssignmentProductionWorkOrder, ProductionWorkOrder>(existing, new {
                AssignedTo = employeeAssignment.AssignedTo.Id,
                AssignedFor = DateTime.Now
            });

            _target.AddEmployeeAssignment(entity);
            _target.AssertTempDataContainsMessage($"Unable to send assignment notification to {employee.FullName} because their employee record is missing an email address.", MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);

            _notificationService.Verify(x => x.Notify(It.IsAny<NotifierArgs>()), Times.Never);
        }

        #endregion

        #region Supervisor Approval Required Notification

        [TestMethod]
        public void TestCompleteDoesNotSendSupervisorApprovalNotificationIfWorkOrderCanNotBeSupervisorApproved()
        {
            var notificationPurpose = ProductionWorkOrderController.SUPERVISOR_APPROVAL_REQUIRED;
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter
            });

            Assert.IsFalse(pwo.CanBeSupervisorApproved, "Sanity.");
            var supervisor = GetEntityFactory<Employee>().Create();
            var employee = GetEntityFactory<Employee>().Create(new { EmailAddress = "employee1@work.com", ReportsTo = supervisor });
            var employeeAssignment1 = GetEntityFactory<EmployeeAssignment>().Create(new { AssignedTo = employee, ProductionWorkOrder = pwo });
            pwo.EmployeeAssignments.Add(employeeAssignment1);

            var viewModel = _viewModelFactory.Build<CompleteProductionWorkOrder, ProductionWorkOrder>(pwo);

            _target.CompleteProductionWorkOrder(viewModel);

            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose)), Times.Never);
        }

        [TestMethod]
        public void TestCompleteSendsSupervisorApprovalRequiredNotificationToAllSupervisorsOfAssignedEmployees()
        {
            var notificationPurpose = ProductionWorkOrderController.SUPERVISOR_APPROVAL_REQUIRED;
            var productionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = _orderTypes.Single(x => x.Id == OrderType.Indices.CORRECTIVE_ACTION_20) });
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter,
                ProductionWorkDescription = productionWorkDescription
            });

            var supervisor1 = GetEntityFactory<Employee>().Create(new { EmailAddress = "supervisor1@work.com" });
            var supervisor2 = GetEntityFactory<Employee>().Create(new { EmailAddress = "supervisor2@work.com" });

            var employee1 = GetEntityFactory<Employee>().Create(new { EmailAddress = "employee1@work.com", ReportsTo = supervisor1 });
            var employee2 = GetEntityFactory<Employee>().Create(new { EmailAddress = "employee2@work.com", ReportsTo = supervisor2 });
            var employeeAssignment1 = GetEntityFactory<EmployeeAssignment>().Create(new { AssignedTo = employee1, ProductionWorkOrder = pwo });
            var employeeAssignment2 = GetEntityFactory<EmployeeAssignment>().Create(new { AssignedTo = employee2, ProductionWorkOrder = pwo });
            pwo.EmployeeAssignments.Add(employeeAssignment1);
            pwo.EmployeeAssignments.Add(employeeAssignment2);

            var viewModel = _viewModelFactory.Build<CompleteProductionWorkOrder, ProductionWorkOrder>(pwo);

            _target.CompleteProductionWorkOrder(viewModel);

            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose && a.Address == supervisor1.EmailAddress)), Times.Once);
            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose && a.Address == supervisor2.EmailAddress)), Times.Once);
        }

        [TestMethod]
        public void TestCompleteDisplaysNotificationErrorForSupervisorApprovalIfAssignedEmployeeDoesNotHaveSupervisorSet()
        {
            var notificationPurpose = ProductionWorkOrderController.SUPERVISOR_APPROVAL_REQUIRED;
            var productionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = _orderTypes.Single(x => x.Id == OrderType.Indices.CORRECTIVE_ACTION_20) });
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter,
                ProductionWorkDescription = productionWorkDescription
            });

            var employee = GetEntityFactory<Employee>().Create(new { EmailAddress = "employee1@work.com" });
            employee.ReportsTo = null;
            var employeeAssignment1 = GetEntityFactory<EmployeeAssignment>().Create(new { AssignedTo = employee, ProductionWorkOrder = pwo });
            pwo.EmployeeAssignments.Add(employeeAssignment1);

            var viewModel = _viewModelFactory.Build<CompleteProductionWorkOrder, ProductionWorkOrder>(pwo);

            _target.CompleteProductionWorkOrder(viewModel);

            _target.AssertTempDataContainsMessage($"Unable to supervisor approval notification to {employee.FullName}'s supervisor because their employee record does not have a supervisor set.", MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);

            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose)), Times.Never);
        }

        [TestMethod]
        public void TestCompleteDisplaysNotificationErrorForSupervisorApprovalifSupervisorIsMissingEmailAddress()
        {
            var notificationPurpose = ProductionWorkOrderController.SUPERVISOR_APPROVAL_REQUIRED;
            var productionWorkDescription = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = _orderTypes.Single(x => x.Id == OrderType.Indices.CORRECTIVE_ACTION_20) });
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter,
                ProductionWorkDescription = productionWorkDescription
            });

            var supervisor = GetEntityFactory<Employee>().Create();
            supervisor.EmailAddress = null;
            var employee = GetEntityFactory<Employee>().Create(new { EmailAddress = "employee1@work.com", ReportsTo = supervisor });
            var employeeAssignment1 = GetEntityFactory<EmployeeAssignment>().Create(new { AssignedTo = employee, ProductionWorkOrder = pwo });
            pwo.EmployeeAssignments.Add(employeeAssignment1);

            var viewModel = _viewModelFactory.Build<CompleteProductionWorkOrder, ProductionWorkOrder>(pwo);

            _target.CompleteProductionWorkOrder(viewModel);

            _target.AssertTempDataContainsMessage($"Unable to supervisor approval notification to {employee.FullName}'s supervisor({supervisor.FullName}) because the supervisor's employee record is missing an email address.", MMSINC.Controllers.ControllerBase.NOTIFICATION_MESSAGE_KEY);

            _notificationService.Verify(x => x.Notify(It.Is<NotifierArgs>(a => a.Purpose == notificationPurpose)), Times.Never);
        }

        #endregion

        #region Create Production Work Order For Equipment

        [TestMethod]
        public void TestCreateProductionWorkOrderForEquipment()
        {
            var equipment = GetEntityFactory<Equipment>().Create();
            var facility = GetEntityFactory<Facility>().Create();
            var planningPlant = GetEntityFactory<PlanningPlant>().Create();
            var functionalLocation = GetEntityFactory<FunctionalLocation>().Create();
            var priority = GetEntityFactory<ProductionWorkOrderPriority>().Create();
            var asLeftAllConditions = GetEntityFactory<AsLeftCondition>().CreateList(6);
            var asLeftCondition =
                asLeftAllConditions.Single(x => x.Id == AsLeftCondition.Indices.NEEDS_EMERGENCY_REPAIR);

            var equipmentTypeGenerator = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var orderTypes = GetFactory<OrderTypeFactory>().CreateAll();
            var pwdRoutine = GetEntityFactory<ProductionWorkDescription>().Create(new
                { EquipmentType = equipmentTypeGenerator, Description = "Routine", OrderType = orderTypes[4] });
            GetEntityFactory<ProductionWorkDescription>().Create(new
                { EquipmentType = equipmentTypeGenerator, Description = "GENERAL REPAIR", OrderType = orderTypes[2] });

            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter,
                Equipment = equipment,
                Facility = facility,
                PlanningPlant = planningPlant,
                FunctionalLocation = functionalLocation.Description,
                Priority = priority,
                ProductionWorkDescription = pwdRoutine
            });

            var productionWorkOrderEquipment = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = pwo,
                Equipment = equipment,
                AsLeftCondition = asLeftCondition
            });

            var model = _viewModelFactory
               .BuildWithOverrides<ProductionWorkOrderEquipmentViewModel, ProductionWorkOrderEquipment>(
                    productionWorkOrderEquipment, new {
                        Priority = (int)ProductionWorkOrderPriority.Indices.EMERGENCY,
                        ProductionWorkOrderId = pwo.Id
                    });

            var newPwo = _target.CreateProductionWorkOrderForEquipment(model);

            var result = newPwo as RedirectToRouteResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.RouteValues["Id"], Session.Get<ProductionWorkOrder>(model.Id).Id);
            Assert.AreEqual(true, Session.Get<ProductionWorkOrder>((int)result
               .RouteValues["newProductionWorkOrderId"]).AutoCreatedCorrectiveWorkOrder);
            Assert.AreEqual("Show", result.RouteValues["action"]);
        }

        #endregion

        #region Supervisor Needs Emergency Repair Notification

        [TestMethod]
        public void TestSendSupervisorNeedsEmergencyRepairNotification()
        {
            var notificationPurpose =
                ProductionWorkOrderController.AS_LEFT_CONDITION_NEEDS_EMERGENCY_REPAIR;

            var equipment = GetEntityFactory<Equipment>().Create();
            var facility = GetEntityFactory<Facility>().Create();
            var priority = GetEntityFactory<ProductionWorkOrderPriority>().Create();
            var equipmentTypeGenerator = GetFactory<EquipmentTypeGeneratorFactory>().Create();
            var orderTypes = GetFactory<OrderTypeFactory>().CreateAll();
            var pwdRoutine = GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = equipmentTypeGenerator, Description = "Routine", OrderType = orderTypes[4] });
            GetEntityFactory<ProductionWorkDescription>().Create(new { EquipmentType = equipmentTypeGenerator, Description = "GENERAL REPAIR", OrderType = orderTypes[2] });

            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = _operatingCenter,
                Facility = facility,
                Priority = priority,
                ProductionWorkDescription = pwdRoutine
            });

            var productionWorkOrderEquipment = GetEntityFactory<ProductionWorkOrderEquipment>().Create(new {
                ProductionWorkOrder = pwo,
                Equipment = equipment
            });

            pwo.Equipments.Add(productionWorkOrderEquipment);

            var supervisor1 = GetEntityFactory<Employee>().Create(new { EmailAddress = "supervisor1@work.com" });

            var employee1 = GetEntityFactory<Employee>()
               .Create(new { EmailAddress = "employee1@work.com", ReportsTo = supervisor1 });
            var employeeAssignment1 = GetEntityFactory<EmployeeAssignment>()
               .Create(new { AssignedTo = employee1, ProductionWorkOrder = pwo });
            pwo.EmployeeAssignments.Add(employeeAssignment1);

            var pwoeq = GetEntityFactory<ProductionWorkOrderEquipment>().Create();
            var asLeftConditions = GetEntityFactory<AsLeftCondition>().CreateList(6);
            var asFoundConditions = GetEntityFactory<AsFoundCondition>().CreateList(5);
            var asFoundConditionExpected =
                asFoundConditions.Single(x => x.Id == AsFoundCondition.Indices.ACCEPTABLE_GOOD);
            var asLeftConditionExpected = asLeftConditions.Single(x =>
                x.Id == AsLeftCondition.Indices.NEEDS_EMERGENCY_REPAIR);

            var model = _viewModelFactory
               .BuildWithOverrides<ProductionWorkOrderEquipmentViewModel, ProductionWorkOrderEquipment>(
                    productionWorkOrderEquipment, new {
                        Priority = (int)ProductionWorkOrderPriority.Indices.EMERGENCY,
                        ProductionWorkOrderId = pwo.Id,
                        AsLeftCondition = asLeftConditionExpected.Id,
                        AsFoundCondition = asFoundConditionExpected.Id,
                        AsLeftConditionComment = "This is testing",
                        Equipment = pwoeq.Id
                    });

            var newPwo = _target.CreateProductionWorkOrderForEquipment(model);

            _notificationService.Verify(
                x => x.Notify(It.Is<NotifierArgs>(a =>
                    a.Purpose == notificationPurpose && a.Address == supervisor1.EmailAddress)), Times.Once);
        }

        #endregion

        #endregion
    }
}