using System;
using System.Linq;
using Contractors.Tests.Controllers;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MMSINC.Authentication;
using MMSINC.Testing.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate.Criterion;
using StructureMap;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;
using MapCall.Common.Model.Mappings;
using Contractors.Models.ViewModels;
using MMSINC.Utilities.Documents;

namespace Contractors.Tests.Models.Repositories
{
    [TestClass]
    public class WorkOrderRepositoryTest : ContractorsControllerTestBase<WorkOrder, WorkOrderRepository>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IIconSetRepository>().Use<IconSetRepository>();
            i.For<IDocumentService>().Singleton().Use(ctx => ctx.GetInstance<InMemoryDocumentService>());
        }

        [TestInitialize]
        public void WorkOrderRepositoryTestInitialize()
        {
            Repository = _container.GetInstance<WorkOrderRepository>();
        }

        #endregion

        #region Contractor Restrictions

        [TestMethod]
        public void TestWorkOrdersLinqValuesReturnedAreForTheCurrentContractor()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var invalid = GetFactory<WorkOrderFactory>().Create();

            var actual = Repository.GetAll();

            Assert.IsTrue(actual.Contains(order));
            Assert.IsFalse(actual.Contains(invalid));
        }

        [TestMethod]
        public void TestWorkOrdersCriteriaValuesReturnedAreForTheCurrentContractor()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var invalid = GetFactory<WorkOrderFactory>().Create();

            var actual = Repository.Search(Restrictions.IsNotNull("Id")).List<WorkOrder>();

            Assert.IsTrue(actual.Contains(order));
            Assert.IsFalse(actual.Contains(invalid));
        }

        #endregion

        #region Criterion

        [TestMethod]
        public void TestNotCompletedReturnsOnlyNotCompletedWorkOrders()
        {
            var openOrder = GetFactory<WorkOrderFactory>().Create( new { AssignedContractor = _currentUser.Contractor });
            var closedOrder = GetFactory<WorkOrderFactory>().Create(new { DateCompleted = DateTime.Now, AssignedContractor = _currentUser.Contractor });

            var actual = Repository.Search(WorkOrderRepository.NotCompleted).List<WorkOrder>();
            Assert.IsTrue(actual.Contains(openOrder));
            Assert.IsFalse(actual.Contains(closedOrder));
        }

        [TestMethod]
        public void TestMarkoutRequiredRestrictionReturnsOnlyWorkOrdersWithMarkoutRequirementNotNone()
        {
            var emergency = GetFactory<WorkOrderFactory>().Create(new
            {
                MarkoutRequirement = typeof(MarkoutRequirementEmergencyFactory),
                AssignedContractor = _currentUser.Contractor
            });
            var routine = GetFactory<WorkOrderFactory>().Create(new
            {
                MarkoutRequirement = typeof(MarkoutRequirementRoutineFactory),
                AssignedContractor = _currentUser.Contractor
            });
            var none = GetFactory<WorkOrderFactory>().Create(new
            {
                MarkoutRequirement = typeof(MarkoutRequirementNoneFactory),
                AssignedContractor = _currentUser.Contractor
            });

            var actual = Repository.Search(WorkOrderRepository.MarkoutRequired).List<WorkOrder>();

            Assert.IsTrue(actual.Contains(emergency));
            Assert.IsTrue(actual.Contains(routine));
            Assert.IsFalse(actual.Contains(none));
        }
 
        [TestMethod]
        public void TestStreetOpeningPermitRequiredReturnsOnlyWorkOrdersWhereStreetOpeningPermitIsRequired()
        {
            var required = GetFactory<WorkOrderFactory>().Create(new
            {
                StreetOpeningPermitRequired = true,
                AssignedContractor = _currentUser.Contractor
            });
            var notRequired = GetFactory<WorkOrderFactory>().Create(new
            {
                StreetOpeningPermitRequired = false,
                AssignedContractor = _currentUser.Contractor
            });

            var actual = Repository.Search(Repository.StreetOpeningPermitRequired).List<WorkOrder>();

            Assert.IsTrue(actual.Contains(required));
            Assert.IsFalse(actual.Contains(notRequired));
        }

        [TestMethod]
        public void TestNotApprovedReturnsOnlyNotApprovedWorkOrders()
        {
            var approvedOrder = GetFactory<WorkOrderFactory>().Create(new { ApprovedOn = DateTime.Now, AssignedContractor = _currentUser.Contractor });
            var unapprovedOrder = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });

            var actual = Repository.Search(WorkOrderRepository.NotApproved).List<WorkOrder>();

            Assert.IsFalse(actual.Contains(approvedOrder));
            Assert.IsTrue(actual.Contains(unapprovedOrder));
        }

        [TestMethod]
        public void TestEmergencyPriorityReturnsOnlyEmergencyPriorityWorkOrders()
        {
            var emergencyOrder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor, 
                Priority = typeof(EmergencyWorkOrderPriorityFactory)
            });
            var highPriorityOrder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                Priority = typeof(HighPriorityWorkOrderPriorityFactory)
            });
            var routineOrder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                Priority = typeof(RoutineWorkOrderPriorityFactory)
            });

            var actual = Repository.Search(WorkOrderRepository.EmergencyPriority).List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(emergencyOrder));
        }

        #endregion

        #region Detached Criteria
        
        [TestMethod]
        public void TestValidMarkoutIdsReturnsOnlyWorkOrdersWithValidMarkouts()
        {
            var orderMarkoutRequirementGoodMarkout = GetFactory<WorkOrderFactory>().Create(new
            {
                MarkoutRequirement = typeof(MarkoutRequirementRoutineFactory),
                AssignedContractor = _currentUser.Contractor
            });
            orderMarkoutRequirementGoodMarkout.Markouts.Add(
                GetFactory<MarkoutFactory>().Create(new { ExpirationDate = DateTime.Now.AddDays(1) }));

            Session.Flush();

            var actual = Repository.Search(Subqueries.Exists(Repository.ValidMarkoutIds)).List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void TestValidPermitIdsReturnsOnlyWorkOrdersWithValidStreetOpeningPermits()
        {
            var order = GetFactory<WorkOrderFactory>().Create( new {
                StreetOpeningPermitRequired = true,
                AssignedContractor = _currentUser.Contractor
            });
            var order2 = GetFactory<WorkOrderFactory>().Create(new {
                StreetOpeningPermitRequired = true,
                AssignedContractor = _currentUser.Contractor
            });
            order.StreetOpeningPermits.Add(
                GetFactory<StreetOpeningPermitFactory>().Create(new {
                    WorkOrder = order,
                    ExpirationDate = DateTime.Now.AddDays(1),
                    DateIssued = DateTime.Now
                }));
            order2.StreetOpeningPermits.Add(
                GetFactory<StreetOpeningPermitFactory>().Create(new {
                    WorkOrder = order2,
                    DateIssued = DateTime.Now
                }));
            Session.Flush();

            var actual = Repository.Search(Subqueries.Exists(Repository.ValidPermitIds)).List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
        }

        [TestMethod]
        public void TestValidCrewAssignmentsReturnsOnlyWorkOrdersWithValidCrewAssignments()
        {
            var order = GetFactory<WorkOrderFactory>().Create( new {
                AssignedContractor = _currentUser.Contractor
            });
            var crew = GetFactory<CrewFactory>().Create( new {
                _currentUser.Contractor
            });
            order.CrewAssignments.Add(
                GetFactory<CrewAssignmentFactory>().Create(new {
                    AssignedFor = DateTime.Now.AddDays(-1),
                    Crew = crew
                })
            );
            Session.SaveOrUpdate(crew);
            Session.SaveOrUpdate(order);
            Session.Flush();
            
            var actual =
                Repository.Search(Subqueries.Exists(Repository.ValidCrewAssignments)).List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(order));
        }

        [TestMethod]
        public void TestValidCrewAssignmentsDoesNotIncludeValidCrewAssignmentsForOtherContractors()
        {
            var otherUser = GetFactory<ContractorUserFactory>().Create(new { Email = "blergh@fargh.net"});
            var crew = GetFactory<CrewFactory>().Create(new {_currentUser.Contractor});
            var otherCrew = GetFactory<CrewFactory>().Create(new {otherUser.Contractor});
            
            // both orders assigned to current user's contractor
            var order = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor});
            var otherOrder = GetFactory<WorkOrderFactory>().Create(new{
                AssignedContractor = _currentUser.Contractor
            });

            order.CrewAssignments.Add(GetFactory<CrewAssignmentFactory>().Create(new {
                AssignedFor = DateTime.Now.AddDays(-1),
                DateStarted = DateTime.Now,
                Crew = crew
            }));
            otherOrder.CrewAssignments.Add(GetFactory<CrewAssignmentFactory>().Create( new {
                AssignedFor = DateTime.Now.AddDays(-1),
                DateStarted = DateTime.Now,
                Crew = otherCrew
            }));

            Session.SaveOrUpdate(crew);
            Session.SaveOrUpdate(otherCrew);
            Session.SaveOrUpdate(order);
            Session.SaveOrUpdate(otherOrder);
            Session.Flush();

            var actual = Repository.Search(Subqueries.Exists(
                Repository.ValidCrewAssignments))
                    .List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(order));
            Assert.IsFalse(actual.Contains(otherOrder));

        }

        #endregion

        #region Filtering

        #region General

        private WorkOrder CreateWorkOrderWithDocument(string filename, DocumentType dt = null, Contractor assignedContractor = null)
        {
            var dataType = GetFactory<DataTypeFactory>().Create(new { TableName = WorkOrderMap.TABLE_NAME });
            var documentType = dt ?? GetFactory<DocumentTypeFactory>().Create(new { DataType = dataType });
            var documentStatus = GetFactory<DocumentStatusFactory>().Create();
            var document = GetFactory<DocumentFactory>().Create(new {
                FileName = filename,
                DocumentType = documentType,
                CreatedAt = DateTime.MinValue,
            });
            var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = assignedContractor
            });
            GetFactory<DocumentLinkFactory>().Create(new {
                document.DocumentType,
                document.DocumentType.DataType,
                Document = document,
                LinkedId = workOrder.Id,
                DocumentStatus = documentStatus,
                ReviewFrequency = 1,
                ReviewFrequencyUnit = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create(),
                UpdatedAt = DateTime.MinValue,
                NextReviewDate = DateTime.MinValue,
            });
            return workOrder;
        }

        [TestMethod]
        public void TestSearchReturnsHelpTopicsWithDocumentsFilteredByDocumentType()
        {
            var dataType = GetFactory<DataTypeFactory>().Create(new { TableName = WorkOrderMap.TABLE_NAME });
            var documentType = GetFactory<DocumentTypeFactory>().Create(new { DataType = dataType });
            var workOrderWithDoc1 = CreateWorkOrderWithDocument("abc", documentType, _currentUser.Contractor);
            var workOrderWithWrongDoc = CreateWorkOrderWithDocument("123 abc xyz", null, _currentUser.Contractor);
            var workOrderWithDocButNotAssignedContractor = CreateWorkOrderWithDocument("123 abc xyz", null, null);
            var workOrderWithoutDocuments = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });

            var model = new WorkOrderGeneralSearch { DocumentType = new[] { documentType.Id } };
            var result = Repository.SearchGeneralOrders(model).ToList();

            Assert.IsTrue(result.Contains(workOrderWithDoc1));
            Assert.IsFalse(result.Contains(workOrderWithWrongDoc));
            Assert.IsFalse(result.Contains(workOrderWithDocButNotAssignedContractor));
            Assert.IsFalse(result.Contains(workOrderWithoutDocuments));
        }

        #endregion

        #region PlanningOrders

        [TestMethod]
        public void TestPlanningOrdersDoesNotReturnOrderWhenPermitAndMarkoutAreNotRequired()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = typeof(MarkoutRequirementNoneFactory),
                StreetOpeningPermitRequired = false,
                AssignedContractor = _currentUser.Contractor 
            });

            var actual = Repository.PlanningOrders.List<WorkOrder>();

            Assert.IsFalse(actual.Contains(order));
        }

        [TestMethod]
        public void TestPlanningOrdersDoesNotReturnACompletedOrder()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new
            {
                MarkoutRequirement = typeof(MarkoutRequirementRoutineFactory),
                DateCompleted = DateTime.Now,
                AssignedContractor = _currentUser.Contractor
            });

            var actual = Repository.PlanningOrders.List<WorkOrder>();

            Assert.IsFalse(actual.Contains(order));
        }

        [TestMethod]
        public void TestPlanningOrdersReturnsOnlyMarkoutRequirementOrders()
        {
            var orderMarkoutRequirementNoMarkout = GetFactory<WorkOrderFactory>().Create(new
            {
                MarkoutRequirement = typeof(MarkoutRequirementRoutineFactory),
                AssignedContractor = _currentUser.Contractor
            });
            var orderMarkoutRequirementExpiredMarkout = GetFactory<WorkOrderFactory>().Create(new
            {
                MarkoutRequirement = typeof(MarkoutRequirementRoutineFactory),
                AssignedContractor = _currentUser.Contractor
            });
            var orderMarkoutRequirementGoodMarkout = GetFactory<WorkOrderFactory>().Create(new
            {
                MarkoutRequirement = typeof(MarkoutRequirementRoutineFactory),
                AssignedContractor = _currentUser.Contractor
            });

            orderMarkoutRequirementExpiredMarkout.Markouts.Add(
                GetFactory<MarkoutFactory>().Create(new { ExpirationDate = DateTime.Now.AddDays(-1) }));
            orderMarkoutRequirementGoodMarkout.Markouts.Add(
                GetFactory<MarkoutFactory>().Create(new { ExpirationDate = DateTime.Now.AddDays(1) }));

            Session.Flush();

            var actual = Repository.PlanningOrders.List<WorkOrder>();

            Assert.AreEqual(2, actual.Count);
            Assert.IsTrue(actual.Contains(orderMarkoutRequirementNoMarkout));
            Assert.IsTrue(actual.Contains(orderMarkoutRequirementExpiredMarkout));
            Assert.IsFalse(actual.Contains(orderMarkoutRequirementGoodMarkout));
        }

        [TestMethod]
        public void TestPlanningOrdersReturnsOnlyStreetOpeningPermitRequirementOrders()
        {
            var orderPermitRequiredNoPermit = GetFactory<WorkOrderFactory>().Create(new
            {
                StreetOpeningPermitRequired = true,
                AssignedContractor = _currentUser.Contractor
            });
            var orderPermitRequiredExpiredPermit = GetFactory<WorkOrderFactory>().Create(new
            {
                StreetOpeningPermitRequired = true,
                AssignedContractor = _currentUser.Contractor
            });
            var orderPermitRequiredGoodPermit = GetFactory<WorkOrderFactory>().Create(new
            {
                StreetOpeningPermitRequired = true,
                AssignedContractor = _currentUser.Contractor
            });

            orderPermitRequiredExpiredPermit.StreetOpeningPermits.Add(
                GetFactory<StreetOpeningPermitFactory>().Create(new {
                    WorkOrder = orderPermitRequiredExpiredPermit,
                    ExpirationDate = DateTime.Now.AddDays(-1)
                }));
            orderPermitRequiredGoodPermit.StreetOpeningPermits.Add(
                GetFactory<StreetOpeningPermitFactory>().Create(new {
                    WorkOrder = orderPermitRequiredGoodPermit,
                    DateIssued = DateTime.Now.AddDays(-1),
                    ExpirationDate = DateTime.Now.AddDays(1)
                }));
            Session.Flush();

            var actual = Repository.PlanningOrders.List<WorkOrder>();

            Assert.IsTrue(actual.Contains(orderPermitRequiredNoPermit));
            Assert.IsTrue(actual.Contains(orderPermitRequiredExpiredPermit));
            Assert.IsFalse(actual.Contains(orderPermitRequiredGoodPermit));
        }

        [TestMethod]
        public void TestPlanningOrdersDoesNotReturnCancelledOrders()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new
            {
                MarkoutRequirement = typeof(MarkoutRequirementRoutineFactory),
                CancelledAt = DateTime.Now,
                AssignedContractor = _currentUser.Contractor
            });
            
            var actual = Repository.PlanningOrders.List<WorkOrder>();

            Assert.IsFalse(actual.Contains(order));
        }
        
        #endregion

        #region Scheduling

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithNoMarkoutRequirementOrStreetOpeningPermitRequirement()
        {
            var regularOrder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });

            Session.Flush();

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(regularOrder));
        }
        
        [TestMethod]
        public void TestSchedulingOrdersDoesNotReturnACompletedWorkOrder()
        {
            var regularOrder = GetFactory<CompletedWorkOrderFactory>().Create(new{
                AssignedContractor = _currentUser.Contractor
            });

            Session.Flush();

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(regularOrder));
        }

        [TestMethod]
        public void TestSchedulingOrdersDoesNotReturnACancelledWorkOrder()
        {
            var permitRequired = GetFactory<StreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                CancelledAt = DateTime.Now
            });
            Session.Flush();
            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(permitRequired));
        }
        
        #region Street Opening Permits

        [TestMethod]
        public void TestSchedulingOrdersDoesNotReturnAWorkOrderWithAStreetOpeningPermitRequiredWithNoValidStreetOpeningPermit()
        {
            var permitRequired = GetFactory<StreetOpeningPermitRequiredWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });

            Session.Flush();

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(permitRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersDoesNotReturnAWorkOrderWithAStreetOpeningPermitRequiredWithAnExpiredStreetOpeningPermit()
        {
            var permitRequired = GetFactory<StreetOpeningPermitRequiredWithExpiredPermitWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(permitRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithAStreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermit()
        {
            var permitRequired = GetFactory<StreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactory>().Create(new{
                AssignedContractor = _currentUser.Contractor
            });
            Session.Flush();
            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(permitRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithAStreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFuture()
        {
            var permitRequired = GetFactory<StreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFutureWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });
            Session.Flush();
            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(permitRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithEmergencyPriorityWithAStreetOpeningPermitRequiredWithoutAnStreetOpeningPermit()
        {
            var permitRequired = GetFactory<StreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
            });

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(permitRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithNoStreetOpeningPermitRequiredThatHasInvalidStreetOpeningPermits()
        {
            var wo = GetFactory<StreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                StreetOpeningPermitRequired = false
            });
            var permit = GetFactory<StreetOpeningPermitFactory>().Create(new {
                WorkOrder = wo,
                DateIssued = DateTime.Now
            });
            wo.StreetOpeningPermits.Add(permit);

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(wo));
            Assert.AreEqual(permit.Id, actual.First().StreetOpeningPermits.First().Id);
        }
        
        #endregion

        #region Markouts

        [TestMethod]
        public void TestSchedulingOrdersDoesNotReturnAWorkOrderWithARoutineMarkoutRequirementWithNoMarkout()
        {
            var markoutRequired = GetFactory<MarkoutRequirementRoutineWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersDoesNotReturnAWorkOrderWithARoutineMarkoutRequirementWithAnExpiredMarkout()
        {
            var markoutRequired = GetFactory<MarkoutRequirementRoutineWithAnExpiredMarkoutWorkOrderFactory>().Create(new{
                AssignedContractor = _currentUser.Contractor
            });

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithARoutineMarkoutAndAValidMarkoutExists()
        {
            var markoutRequired = GetFactory<MarkoutRequirementRoutineWithAValidMarkoutWorkOrderFactory>().Create(new{
                AssignedContractor = _currentUser.Contractor
            });
            Session.Flush();
            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithARoutineMarkoutAndAMarkoutExistsThatIsNotReadyYet()
        {
            var markoutRequired = GetFactory<MarkoutRequirementRoutineWithAValidFutureMarkoutWorkOrderFactory>().Create(new{
                AssignedContractor = _currentUser.Contractor
            });
            Session.Flush();
            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithAnEmergencyMarkoutAndNoMarkoutExists()
        {
            var markoutRequired = GetFactory<MarkoutRequirementEmergencyWorkOrderFactory>().Create(new{
                AssignedContractor = _currentUser.Contractor
            });
            
            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithAnEmergencyMarkoutAndAnExpiredMarkoutExists()
        {
            var markoutRequired = GetFactory<MarkoutRequirementEmergencyWithExpiredMarkoutWorkOrderFactory>().Create(new{
                AssignedContractor = _currentUser.Contractor
            });
            
            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithAnEmergencyMarkoutAndValidMarkoutExists()
        {
            var markoutRequired = GetFactory<MarkoutRequirementEmergencyWithValidMarkoutWorkOrderFactory>().Create(new{
                AssignedContractor = _currentUser.Contractor
            });

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithAnEmergencyMarkoutRequirementAndStreetOpeningPermitRequiredWithNoPermit()
        {
            var order = GetFactory<MarkoutRequirementEmergencyPermitRequiredWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(order));
        }

        #endregion
       
        #endregion

        #region Finalization

        [TestMethod]
        public void TestFinalizationOrdersReturnsWorkOrderWithEmergencyPriority()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                Priority = typeof(EmergencyWorkOrderPriorityFactory)
            });

            var actual = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(order));
        }

        [TestMethod]
        public void TestFinalizationOrdersDoesNotReturnAnApprovedWorkOrder()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor,
                ApprovedOn = DateTime.Now,
                Priority = typeof(EmergencyWorkOrderPriorityFactory)
            });

            var actual = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(order));
        }

        [TestMethod]
        public void TestFinalizationOrdersDoesNotReturnACancelledWorkOrder()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor,
                CancelledAt = DateTime.Now,
                Priority = typeof(EmergencyWorkOrderPriorityFactory)
            });

            var actual = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(order));
        }

        [TestMethod]
        public void TestFinalizationOrdersReturnsWorkOrdersWithValidCrewAssignments()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new { 
                AssignedContractor = _currentUser.Contractor});
            var extraOrder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor});
            var crew = GetFactory<CrewFactory>().Create(new {
                _currentUser.Contractor});
            order.CrewAssignments.Add(
                GetFactory<CrewAssignmentFactory>().Create(new {
                    AssignedFor = DateTime.Now.AddHours(-1),
                    Crew = crew}));
            Session.SaveOrUpdate(crew);
            Session.SaveOrUpdate(order);
            Session.Flush();

            var actual = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(order));
            Assert.IsFalse(actual.Contains(extraOrder));
        }

        [TestMethod]
        public void TestFinalizationOrdersReturnsWorkOrdersWithStartedFutureCrewAssignments()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor
            });
            var extraOrder = GetFactory<WorkOrderFactory>().Create(new
            {
                AssignedContractor = _currentUser.Contractor
            });
            var crew = GetFactory<CrewFactory>().Create(new
            {
                _currentUser.Contractor
            });
            order.CrewAssignments.Add(
                GetFactory<CrewAssignmentFactory>().Create(new
                {
                    AssignedFor = DateTime.Now.AddDays(1),
                    DateStarted = DateTime.Now,
                    Crew = crew
                }));
            Session.SaveOrUpdate(crew);
            Session.SaveOrUpdate(order);
            Session.Flush();

            var actual = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(order));
            Assert.IsFalse(actual.Contains(extraOrder));
        }
        #endregion

        #endregion

        #region Requisitions

        [TestMethod]
        public void TestThatIfSomethingSomehowRemovesTheRequisitionsFromAWorkOrderThatTheyAreNotActuallyDeletedBecauseItIsReadOnlyAndStuff()
        {
            var wo = GetFactory<WorkOrderFactory>().Create(new { AssignedContractor = _currentUser.Contractor });
            var req = GetFactory<RequisitionFactory>().Create(new{ WorkOrder = wo });

            Session.Evict(wo);
            Session.Evict(req);

            wo = Repository.Find(wo.Id);
            Assert.AreEqual(1, wo.Requisitions.Count);

            wo.Requisitions.Clear();
            Session.Save(wo);

            Session.Evict(wo);
            wo = Repository.Find(wo.Id);
            Assert.AreEqual(1, wo.Requisitions.Count);
        }
        
        #endregion
    }
}
