using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using System;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Utilities;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class WorkOrderRepositoryTest : MapCallMvcSecuredRepositoryTestBase<WorkOrder, WorkOrderRepository, User>
    {
        #region Fields

        private TestDateTimeProvider _dateTimeProvider;
        private DateTime _now;

        private OperatingCenter _op1;
        private State _state1;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            _state1 = GetFactory<StateFactory>().Create();
            _op1 = GetFactory<UniqueOperatingCenterFactory>().Create(new {State = _state1});
            return GetFactory<UserFactory>().Create(new {
                IsAdmin = true,
                DefaultOperatingCenter = _op1
            });
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));
            i.For<IIconSetRepository>().Use<IconSetRepository>();
            i.For<IServiceRepository>().Use<ServiceRepository>();
            i.For<ITapImageRepository>().Use<TapImageRepository>();
            i.For<IImageToPdfConverter>().Use(() => new ImageToPdfConverter());
        }

        #endregion

        #region Filtering

        #region Criteria

        [TestMethod]
        public void TestCriteriaDoesNotReturnFacilitiesFromOtherOperatingCentersForUser()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.FieldServicesWorkManagement });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false, DefaultOperatingCenter = opCntr1});
            var role = GetFactory<RoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                OperatingCenter = opCntr1,
                User = user
            });

            Session.Save(user);

            var woMatch = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = opCntr1});
            var woNotAMatch = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<WorkOrderRepository>();
            var model = new EmptySearchSet<WorkOrder>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(woMatch));
            Assert.IsFalse(result.Contains(woNotAMatch));
        }

        [TestMethod]
        public void TestCriteriaReturnsAllFacilitiesIfUserHasMatchingRoleWithWildcardOperatingCenter()
        {
            var opCntr1 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ7", OperatingCenterName = "Shrewsbury"});
            var opCntr2 = GetFactory<OperatingCenterFactory>()
               .Create(new { OperatingCenterCode = "NJ4", OperatingCenterName = "Lakewood"});
            var application = GetFactory<ApplicationFactory>().Create(new { Id = RoleApplications.FieldServices });
            var module = GetFactory<ModuleFactory>().Create(new { Id = RoleModules.FieldServicesWorkManagement });
            var action = GetFactory<ActionFactory>().Create(new { Id = RoleActions.Read });
            var user = GetFactory<UserFactory>().Create(new { IsAdmin = false});
            var role = GetFactory<WildcardOpCenterRoleFactory>().Create(new {
                Application = application,
                Module = module,
                Action = action,
                User = user
            });

            Assert.IsNull(role.OperatingCenter);
            Session.Save(user);

            var woMatch = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = opCntr1});
            var woAlsoAMatch = GetFactory<WorkOrderFactory>().Create(new { OperatingCenter = opCntr2});

            Repository = _container.With(new MockAuthenticationService(user).Object).GetInstance<WorkOrderRepository>();
            var model = new EmptySearchSet<WorkOrder>();
            var result = Repository.Search(model);

            Assert.IsTrue(result.Contains(woMatch));
            Assert.IsTrue(result.Contains(woAlsoAMatch));
        }

        #endregion 

        #region PrePlanningOrders

        [TestMethod]
        public void TestPrePlanningOrdersFiltersOutCompletedOrders()
        {
            var woFactory = GetEntityFactory<WorkOrder>();
            var completedWithMarkoutRequirement = woFactory.Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                DateCompleted = _now
            });
            var completedWithSOPRequirement = woFactory.Create(new {
                StreetOpeningPermitRequired = true,
                DateCompleted = _now
            });

            var results = Repository
               .GetPrePlanningWorkOrders(new TestSearchWorkOrderPrePlanning());

            MyAssert.DoesNotContain(results, completedWithMarkoutRequirement);
            MyAssert.DoesNotContain(results, completedWithSOPRequirement);

            Assert.IsNull(Repository.FindPrePlanningOrder(completedWithMarkoutRequirement.Id));
            Assert.IsNull(Repository.FindPrePlanningOrder(completedWithSOPRequirement.Id));
        }

        [TestMethod]
        public void TestPrePlanningOrdersFiltersOutCancelledOrders()
        {
            var woFactory = GetEntityFactory<WorkOrder>();
            var cancelledWithMarkoutRequirement = woFactory.Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                CancelledAt = _now
            });
            var cancelledWithSOPRequirement = woFactory.Create(new {
                StreetOpeningPermitRequired = true,
                CancelledAt = _now
            });

            var results = Repository
               .GetPrePlanningWorkOrders(new TestSearchWorkOrderPrePlanning());

            MyAssert.DoesNotContain(results, cancelledWithMarkoutRequirement);
            MyAssert.DoesNotContain(results, cancelledWithSOPRequirement);

            Assert.IsNull(Repository.FindPrePlanningOrder(cancelledWithMarkoutRequirement.Id));
            Assert.IsNull(Repository.FindPrePlanningOrder(cancelledWithSOPRequirement.Id));
        }

        [TestMethod]
        public void TestPrePlanningOrdersContainsOrdersWithNoRequirements()
        {
            var noRequirements = GetEntityFactory<WorkOrder>().Create();

            var results = Repository
               .GetPrePlanningWorkOrders(new TestSearchWorkOrderPrePlanning());

            MyAssert.Contains(results, noRequirements);

            Assert.AreEqual(noRequirements, Repository.FindPrePlanningOrder(noRequirements.Id));
        }

        [TestMethod]
        public void TestPrePlanningOrdersReturnsOnlyPrePlanningWorkOrders()
        {
            var factory = GetEntityFactory<WorkOrder>();
            var completed = factory.Create(new {
                DateCompleted = DateTime.Now,
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                Notes = "completed"
            });
            var noRequirements = factory.Create(new {Notes = "no requirements"});
            var markoutRequired = factory.Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                Notes = "markout required"
            });
            var sopRequired = factory.Create(new {
                StreetOpeningPermitRequired = true,
                Notes = "sop required"
            });

            var results = Repository
               .GetPrePlanningWorkOrders(new TestSearchWorkOrderPrePlanning());

            MyAssert.Contains(results, markoutRequired);
            MyAssert.Contains(results, sopRequired);

            MyAssert.DoesNotContain(results, completed);
            MyAssert.Contains(results, noRequirements);

            Assert.AreEqual(markoutRequired, Repository.FindPrePlanningOrder(markoutRequired.Id));
            Assert.AreEqual(sopRequired, Repository.FindPrePlanningOrder(sopRequired.Id));

            Assert.IsNull(Repository.FindPrePlanningOrder(completed.Id));
            Assert.AreEqual(noRequirements, Repository.FindPrePlanningOrder(noRequirements.Id));
        }

        #endregion

        #region PlanningOrders

        [TestMethod]
        public void TestPlanningOrdersDoesNotReturnOrderWhenPermitAndMarkoutAreNotRequired()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = typeof(NoneMarkoutRequirementFactory),
                StreetOpeningPermitRequired = false,
            });

            var actual = Repository.GetPlanningWorkOrders(new TestSearchWorkOrderPlanning());

            Assert.IsFalse(actual.Contains(order));

            Assert.IsNull(Repository.FindPlanningOrder(order.Id));
        }

        [TestMethod]
        public void TestPlanningOrdersDoesNotReturnACompletedOrder()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                DateCompleted = _now,
            });

            var actual = Repository.GetPlanningWorkOrders(new TestSearchWorkOrderPlanning());

            Assert.IsFalse(actual.Contains(order));

            Assert.IsNull(Repository.FindPlanningOrder(order.Id));
        }

        [TestMethod]
        public void TestPlanningOrdersReturnsOnlyMarkoutRequirementOrders()
        {
            var orderMarkoutRequirementNoMarkout = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                StreetOpeningPermitRequired = false,
            });
            var orderMarkoutRequirementExpiredMarkout = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                StreetOpeningPermitRequired = false,
            });
            var orderMarkoutRequirementGoodMarkout = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                StreetOpeningPermitRequired = false,
            });

            GetFactory<MarkoutFactory>().Create(new
                {ExpirationDate = _now.AddDays(-1), WorkOrder = orderMarkoutRequirementExpiredMarkout});
            GetFactory<MarkoutFactory>().Create(new
                {ExpirationDate = _now.AddDays(1), WorkOrder = orderMarkoutRequirementGoodMarkout});

            var actual = Repository.GetPlanningWorkOrders(new TestSearchWorkOrderPlanning());

            Assert.AreEqual(2, actual.Count());
            Assert.IsTrue(actual.Contains(orderMarkoutRequirementNoMarkout));
            Assert.IsTrue(actual.Contains(orderMarkoutRequirementExpiredMarkout));
            Assert.IsFalse(actual.Contains(orderMarkoutRequirementGoodMarkout));

            Assert.AreEqual(
                orderMarkoutRequirementNoMarkout,
                Repository.FindPlanningOrder(orderMarkoutRequirementNoMarkout.Id));
            Assert.AreEqual(
                orderMarkoutRequirementExpiredMarkout,
                Repository.FindPlanningOrder(orderMarkoutRequirementExpiredMarkout.Id));
            Assert.IsNull(Repository.FindPlanningOrder(orderMarkoutRequirementGoodMarkout.Id));
        }

        [TestMethod]
        public void TestPlanningOrdersReturnsOnlyStreetOpeningPermitRequirementOrders()
        {
            var orderPermitRequiredNoPermit = GetFactory<WorkOrderFactory>().Create(new {
                StreetOpeningPermitRequired = true,
            });
            var orderPermitRequiredExpiredPermit = GetFactory<WorkOrderFactory>().Create(new {
                StreetOpeningPermitRequired = true,
            });
            var orderPermitRequiredGoodPermit = GetFactory<WorkOrderFactory>().Create(new {
                StreetOpeningPermitRequired = true,
            });

            orderPermitRequiredExpiredPermit.StreetOpeningPermits.Add(
                GetFactory<StreetOpeningPermitFactory>().Create(new {
                    WorkOrder = orderPermitRequiredExpiredPermit,
                    ExpirationDate = _now.AddDays(-1)
                }));
            orderPermitRequiredGoodPermit.StreetOpeningPermits.Add(
                GetFactory<StreetOpeningPermitFactory>().Create(new {
                    WorkOrder = orderPermitRequiredGoodPermit,
                    DateIssued = _now.AddDays(-1),
                    ExpirationDate = _now.AddDays(1)
                }));
            Session.Flush();

            var actual = Repository.GetPlanningWorkOrders(new TestSearchWorkOrderPlanning());

            Assert.IsTrue(actual.Contains(orderPermitRequiredNoPermit));
            Assert.IsTrue(actual.Contains(orderPermitRequiredExpiredPermit));
            Assert.IsFalse(actual.Contains(orderPermitRequiredGoodPermit));

            Assert.AreEqual(
                orderPermitRequiredNoPermit,
                Repository.FindPlanningOrder(orderPermitRequiredNoPermit.Id));
            Assert.AreEqual(
                orderPermitRequiredExpiredPermit,
                Repository.FindPlanningOrder(orderPermitRequiredExpiredPermit.Id));
            Assert.IsNull(Repository.FindPlanningOrder(orderPermitRequiredGoodPermit.Id));
        }

        [TestMethod]
        public void TestPlanningOrdersDoesNotReturnCancelledOrders()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory),
                CancelledAt = _now,
            });

            var actual = Repository.GetPlanningWorkOrders(new TestSearchWorkOrderPlanning());

            Assert.IsFalse(actual.Contains(order));

            Assert.IsNull(Repository.FindPlanningOrder(order.Id));
        }

        #endregion

        #region Scheduling

        [TestMethod]
        public void TestSchedulingOrdersCriteriaProperlyFiltersForSAP()
        {
            // The scheduling criteria needs one  ofthe following for SAP filtering:
            //   - The operating center has SAPWorkOrdersEnabled = false
            //   - OR the operating center's IsContractedOperations = true
            //   - OR the work order has an SAPWorkOrderNumber value

            // All of these need to be valid work orders
            var validWorkOrderBecauseItHasSAPWorkOrderNumber = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)11251, 
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false })
            });
            var validWorkOrderBecauseIsNotSAPEnabled = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)11251,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false, IsContractedOperations = false })
            });
            var validWorkOrderBecauseIsContractedOperations = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = true })
            });
            
            // All work orders below should be invalid for filtering
            var validOperatingCenterForInvalids = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false, IsContractedOperations = true });
   
            var invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false })
            });
            var invalidBecauseItIsSAPEnabledButNotContractedOperations = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false })
            });
            var invalidBecauseItDoesNotHaveSAPWorkOrderNumber = GetFactory<WorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }), 
            });
            // Only doing a check for cancelled as the rest of the criteria is tested in other tests.
            // This is just to ensure that we're past the SAP filtering.
            var invalidBecauseCancelled = GetEntityFactory<WorkOrder>().Create(new { CancelledAt = DateTime.Now, OperatingCenter = validOperatingCenterForInvalids });

            var results = Repository.SchedulingOrders.List<WorkOrder>();
            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.Contains(validWorkOrderBecauseItHasSAPWorkOrderNumber));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsNotSAPEnabled));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsContractedOperations));
            Assert.IsFalse(results.Contains(invalidBecauseCancelled));
            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber));
            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledButNotContractedOperations));
            Assert.IsFalse(results.Contains(invalidBecauseItDoesNotHaveSAPWorkOrderNumber));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithNoMarkoutRequirementOrStreetOpeningPermitRequirement()
        {
            var regularOrder = GetFactory<WorkOrderFactory>().Create();

            Session.Flush();

            var actual = Repository.GetSchedulingWorkOrders(new TestSearchWorkOrderScheduling());

            Assert.AreEqual(1, actual.Count());
            Assert.IsTrue(actual.Contains(regularOrder));

            var single = Repository.FindSchedulingOrder(regularOrder.Id);

            Assert.IsNotNull(single);
        }

        [TestMethod]
        public void TestSchedulingOrdersDoesNotReturnACompletedWorkOrder()
        {
            var completed = GetFactory<CompletedWorkOrderFactory>().Create();

            Session.Flush();

            var actual = Repository.GetSchedulingWorkOrders(new TestSearchWorkOrderScheduling());

            Assert.AreEqual(0, actual.Count());
            Assert.IsFalse(actual.Contains(completed));

            var single = Repository.FindSchedulingOrder(completed.Id);

            Assert.IsNull(single);
        }

        [TestMethod]
        public void TestSchedulingOrdersDoesNotReturnACancelledWorkOrder()
        {
            var permitRequired =
                GetFactory<StreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactory>().Create(new {
                    CancelledAt = _now
                });
            Session.Flush();

            var actual = Repository.GetSchedulingWorkOrders(new TestSearchWorkOrderScheduling());

            Assert.AreEqual(0, actual.Count());
            Assert.IsFalse(actual.Contains(permitRequired));

            var single = Repository.FindSchedulingOrder(permitRequired.Id);

            Assert.IsNull(single);
        }

        #region Street Opening Permits

        [TestMethod]
        public void
            TestSchedulingOrdersDoesNotReturnAWorkOrderWithAStreetOpeningPermitRequiredWithNoValidStreetOpeningPermit()
        {
            var permitRequired = GetFactory<StreetOpeningPermitRequiredWorkOrderFactory>().Create();

            Session.Flush();

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(permitRequired));
        }

        [TestMethod]
        public void
            TestSchedulingOrdersDoesNotReturnAWorkOrderWithAStreetOpeningPermitRequiredWithAnExpiredStreetOpeningPermit()
        {
            var permitRequired = GetFactory<StreetOpeningPermitRequiredWithExpiredPermitWorkOrderFactory>().Create();

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(permitRequired));
        }

        [TestMethod]
        public void
            TestSchedulingOrdersReturnsAWorkOrderWithAStreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermit()
        {
            var permitRequired =
                GetFactory<StreetOpeningPermitRequiredWithAnIssuedStreetOpeningPermitWorkOrderFactory>().Create();
            Session.Flush();
            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(permitRequired));
        }

        [TestMethod]
        public void
            TestSchedulingOrdersReturnsAWorkOrderWithAStreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFuture()
        {
            var permitRequired =
                GetFactory<StreetOpeningPermitRequiredWithAStreetOpeningPermitInTheFutureWorkOrderFactory>().Create();
            Session.Flush();
            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(permitRequired));
        }

        [TestMethod]
        public void
            TestSchedulingOrdersReturnsAWorkOrderWithEmergencyPriorityWithAStreetOpeningPermitRequiredWithoutAnStreetOpeningPermit()
        {
            var permitRequired =
                GetFactory<StreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactory>()
                   .Create();

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(permitRequired));
        }

        [TestMethod]
        public void
            TestSchedulingOrdersReturnsAWorkOrderWithNoStreetOpeningPermitRequiredThatHasInvalidStreetOpeningPermits()
        {
            var wo =
                GetFactory<StreetOpeningPermitRequiredEmergencyPriorityWithoutAnStreetOpeningPermitWorkOrderFactory>()
                   .Create(new {
                        StreetOpeningPermitRequired = false
                    });
            var permit = GetFactory<StreetOpeningPermitFactory>().Create(new {
                WorkOrder = wo,
                DateIssued = _now
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
            var markoutRequired = GetFactory<MarkoutRequirementRoutineWorkOrderFactory>().Create();

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersDoesNotReturnAWorkOrderWithARoutineMarkoutRequirementWithAnExpiredMarkout()
        {
            var markoutRequired = GetEntityFactory<WorkOrder>().Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory)
            });
            GetEntityFactory<Markout>().Create(new {
                WorkOrder = markoutRequired,
                ReadyDate = _now.AddDays(-10),
                ExpirationDate = _now.AddDays(-1)
            });

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithARoutineMarkoutAndAValidMarkoutExists()
        {
            var markoutRequired = GetEntityFactory<WorkOrder>().Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory)
            });
            GetEntityFactory<Markout>().Create(new {
                WorkOrder = markoutRequired,
                ReadyDate = _now,
                ExpirationDate = _now.AddDays(10)
            });

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithARoutineMarkoutAndAMarkoutExistsThatIsNotReadyYet()
        {
            var markoutRequired = GetEntityFactory<WorkOrder>().Create(new {
                MarkoutRequirement = typeof(RoutineMarkoutRequirementFactory)
            });
            GetEntityFactory<Markout>().Create(new {
                WorkOrder = markoutRequired,
                ReadyDate = _now.AddDays(1),
                ExpirationDate = _now.AddDays(10)
            });

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithAnEmergencyMarkoutAndNoMarkoutExists()
        {
            var markoutRequired = GetFactory<MarkoutRequirementEmergencyWorkOrderFactory>().Create();

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithAnEmergencyMarkoutAndAnExpiredMarkoutExists()
        {
            var markoutRequired = GetEntityFactory<WorkOrder>().Create(new {
                MarkoutRequirement = typeof(EmergencyMarkoutRequirementFactory)
            });
            GetEntityFactory<Markout>().Create(new {
                WorkOrder = markoutRequired,
                ReadyDate = _now.AddDays(-14),
                ExpirationDate = _now.AddDays(-10)
            });

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void TestSchedulingOrdersReturnsAWorkOrderWithAnEmergencyMarkoutAndValidMarkoutExists()
        {
            var markoutRequired = GetEntityFactory<WorkOrder>().Create(new {
                MarkoutRequirement = typeof(EmergencyMarkoutRequirementFactory)
            });
            GetEntityFactory<Markout>().Create(new {
                WorkOrder = markoutRequired,
                ReadyDate = _now,
                ExpirationDate = _now.AddDays(10)
            });

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(markoutRequired));
        }

        [TestMethod]
        public void
            TestSchedulingOrdersReturnsAWorkOrderWithAnEmergencyMarkoutRequirementAndStreetOpeningPermitRequiredWithNoPermit()
        {
            var order = GetFactory<MarkoutRequirementEmergencyPermitRequiredWorkOrderFactory>().Create();

            var actual = Repository.SchedulingOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(order));
        }

        #endregion

        #endregion

        #region StockToIssue
        
        [TestMethod]
        public void TestGetStockToIssueWorkOrdersReturnsResultsWithExpectedFilters()
        {
            // The supervisor approval criteria needs the following:
            // - Work order can't be cancelled
            // - Work order must have been approved 
            // - Work order must have materials used
            // - One of the three following things:
            //    - The operating center has SAPWorkOrdersEnabled = false
            //    - OR the operating center's IsContractedOperations = true
            //    - OR the work order has an SAPWorkOrderNumber value

            WorkOrder createApprovedWorkOrderWithMaterials(object args)
            {
                var wo = GetFactory<WorkOrderFactory>().Create(args);
                wo.ApprovedBy = GetEntityFactory<User>().Create();
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
            
            // All of these need to be valid work orders
            var validWorkOrderBecauseItHasSAPWorkOrderNumber = createApprovedWorkOrderWithMaterials(new {
                SAPWorkOrderNumber = (long?)11251, 
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
            });
            var validWorkOrderBecauseIsNotSAPEnabled = createApprovedWorkOrderWithMaterials(new {
                SAPWorkOrderNumber = (long?)11251,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false, IsContractedOperations = false }),
            });
            var validWorkOrderBecauseIsContractedOperations = createApprovedWorkOrderWithMaterials(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = true }),
            });
            
            // All work orders below should be invalid for filtering
            var validOperatingCenterForInvalids = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false });
            var invalidBecauseCancelled = GetEntityFactory<WorkOrder>().Create(new { CancelledAt = DateTime.Now, OperatingCenter = validOperatingCenterForInvalids });
            var invalidBecauseNotCompleted = GetEntityFactory<WorkOrder>().Create(new { DateCompleted = (DateTime?)null, OperatingCenter = validOperatingCenterForInvalids });
            var invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber = createApprovedWorkOrderWithMaterials(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false })
            });
            var invalidBecauseItIsSAPEnabledButNotContractedOperations = createApprovedWorkOrderWithMaterials(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false })
            });
            var invalidBecauseItDoesNotHaveSAPWorkOrderNumber = createApprovedWorkOrderWithMaterials(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }), 
            });
            var invalidBecauseThereAreNotMaterialsUsed = GetFactory<CompletedWorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)11251, 
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
                ApprovedBy = typeof(UserFactory)
            });
            var invalidBecauseNotApproved = createApprovedWorkOrderWithMaterials(new {
                SAPWorkOrderNumber = (long?)11251, 
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }),
            });
            invalidBecauseNotApproved.ApprovedBy = null;
            Session.Save(invalidBecauseNotApproved);
            Session.Flush();

            var results = Repository.GetStockToIssueWorkOrders(new EmptySearchSet<WorkOrder>());
            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.Contains(validWorkOrderBecauseItHasSAPWorkOrderNumber));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsNotSAPEnabled));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsContractedOperations));
            Assert.IsFalse(results.Contains(invalidBecauseCancelled));
            Assert.IsFalse(results.Contains(invalidBecauseNotCompleted));
            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber));
            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledButNotContractedOperations));
            Assert.IsFalse(results.Contains(invalidBecauseItDoesNotHaveSAPWorkOrderNumber));
            Assert.IsFalse(results.Contains(invalidBecauseThereAreNotMaterialsUsed));
            Assert.IsFalse(results.Contains(invalidBecauseNotApproved));
        }

        [TestMethod]
        public void TestGetStockToIssueWorkOrdersReturnsResultsFilteredByRole()
        {
            WorkOrder createApprovedWorkOrderWithMaterials(object args)
            {
                var wo = GetFactory<WorkOrderFactory>().Create(args);
                wo.ApprovedBy = GetEntityFactory<User>().Create();
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

            var validOpcForRole = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false });
            var invalidOpcForRole = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false });
            var expectedWorkOrder = createApprovedWorkOrderWithMaterials(new {
                SAPWorkOrderNumber = (long?)11251,
                OperatingCenter = validOpcForRole
            });
            var unexpectedWorkOrder = createApprovedWorkOrderWithMaterials(new {
                SAPWorkOrderNumber = (long?)11251,
                OperatingCenter = invalidOpcForRole
            });

            // setup User
            User.IsAdmin = false;
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, validOpcForRole, User);

            var results = Repository.GetStockToIssueWorkOrders(new EmptySearchSet<WorkOrder>());
            Assert.AreSame(expectedWorkOrder, results.Single());
        }

        #endregion

        #region Supervisor Approval

        [TestMethod]
        public void TestGetSupervisorApprovalWorkOrdersReturnsResultsWithExpectedFilters()
        {
            // The supervisor approval criteria needs the following:
            // - Work order can't be cancelled
            // - Work order must have a completion date 
            // - One of the three following things:
            //    - The operating center has SAPWorkOrdersEnabled = false
            //    - OR the operating center's IsContractedOperations = true
            //    - OR the work order has an SAPWorkOrderNumber value

            // All of these need to be valid work orders
            var validWorkOrderBecauseItHasSAPWorkOrderNumber = GetFactory<CompletedWorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)11251, 
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false })
            });
            var validWorkOrderBecauseIsNotSAPEnabled = GetFactory<CompletedWorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)11251,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false, IsContractedOperations = false })
            });
            var validWorkOrderBecauseIsContractedOperations = GetFactory<CompletedWorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = true })
            });
            
            // All work orders below should be invalid for filtering
            var validOperatingCenterForInvalids = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = false });
            var invalidBecauseCancelled = GetEntityFactory<WorkOrder>().Create(new { CancelledAt = DateTime.Now, OperatingCenter = validOperatingCenterForInvalids });
            var invalidBecauseNotCompleted = GetEntityFactory<WorkOrder>().Create(new { DateCompleted = (DateTime?)null, OperatingCenter = validOperatingCenterForInvalids });
            var invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber = GetFactory<CompletedWorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false })
            });
            var invalidBecauseItIsSAPEnabledButNotContractedOperations = GetFactory<CompletedWorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false })
            });
            var invalidBecauseItDoesNotHaveSAPWorkOrderNumber = GetFactory<CompletedWorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)null,
                OperatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false }), 
            });

            var results = Repository.GetSupervisorApprovalWorkOrders(new EmptySearchSet<WorkOrder>());
            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.Contains(validWorkOrderBecauseItHasSAPWorkOrderNumber));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsNotSAPEnabled));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsContractedOperations));
            Assert.IsFalse(results.Contains(invalidBecauseCancelled));
            Assert.IsFalse(results.Contains(invalidBecauseNotCompleted));
            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber));
            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledButNotContractedOperations));
            Assert.IsFalse(results.Contains(invalidBecauseItDoesNotHaveSAPWorkOrderNumber));
        }

        [TestMethod]
        public void TestGetSupervisorApprovalWorkOrdersReturnsResultsFilteredByRole()
        {
            var validOpcForRole = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false });
            var invalidOpcForRole = GetFactory<UniqueOperatingCenterFactory>().Create(new { SAPWorkOrdersEnabled = true, IsContractedOperations = false });
            var expectedWorkOrder = GetFactory<CompletedWorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)11251,
                OperatingCenter = validOpcForRole
            });
            var unexpectedWorkOrder = GetFactory<CompletedWorkOrderFactory>().Create(new {
                SAPWorkOrderNumber = (long?)11251,
                OperatingCenter = invalidOpcForRole
            });

            // setup User
            User.IsAdmin = false;
            GetFactory<RoleFactory>().Create(RoleModules.FieldServicesWorkManagement, validOpcForRole, User);

            var results = Repository.GetSupervisorApprovalWorkOrders(new EmptySearchSet<WorkOrder>());
            Assert.AreSame(expectedWorkOrder, results.Single());
        }

        #endregion

        #region Finalization

        [TestMethod]
        public void TestFinalizationOrdersReturnsWorkOrderWithEmergencyPriority()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new {
                Priority = typeof(EmergencyWorkOrderPriorityFactory)
            });

            var actual = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(order));
            Assert.IsNotNull(Repository.FindFinalizationOrder(order.Id));
        }

        [TestMethod]
        public void TestFinalizationOrdersDoesNotReturnAnApprovedWorkOrder()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new {
                ApprovedOn = _now,
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
                AssignedContractor = GetFactory<ContractorFactory>().Create()
            });

            var actual = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(order));
            Assert.IsNull(Repository.FindFinalizationOrder(order.Id));
        }

        [TestMethod]
        public void TestFinalizationOrdersDoesNotReturnACancelledWorkOrder()
        {
            var order = GetFactory<WorkOrderFactory>().Create(new {
                CancelledAt = _now,
                Priority = typeof(EmergencyWorkOrderPriorityFactory),
                AssignedContractor = GetFactory<ContractorFactory>().Create()
            });

            var actual = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(0, actual.Count);
            Assert.IsFalse(actual.Contains(order));
            Assert.IsNull(Repository.FindFinalizationOrder(order.Id));
        }

        [TestMethod]
        public void TestFinalizationOrdersReturnsWorkOrdersWithValidCrewAssignments()
        {
            var order = GetFactory<WorkOrderFactory>().Create();
            var extraOrder = GetFactory<WorkOrderFactory>().Create();
            var crew = GetFactory<CrewFactory>().Create();
            order.CrewAssignments.Add(
                GetFactory<CrewAssignmentFactory>().Create(new {
                    AssignedFor = _now.AddHours(-1),
                    Crew = crew
                }));
            Session.SaveOrUpdate(crew);
            Session.SaveOrUpdate(order);
            Session.Flush();

            var actual = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(order));
            Assert.IsFalse(actual.Contains(extraOrder));
            Assert.IsNotNull(Repository.FindFinalizationOrder(order.Id));
        }

        [TestMethod]
        public void TestFinalizationOrdersReturnsWorkOrdersWithStartedFutureCrewAssignments()
        {
            var order = GetFactory<WorkOrderFactory>().Create();
            var extraOrder = GetFactory<WorkOrderFactory>().Create();
            var crew = GetFactory<CrewFactory>().Create();
            order.CrewAssignments.Add(
                GetFactory<CrewAssignmentFactory>().Create(new {
                    AssignedFor = _now.AddDays(1),
                    DateStarted = _now,
                    Crew = crew,
                    WorkOrder = order
                }));
            Session.SaveOrUpdate(crew);
            Session.SaveOrUpdate(order);
            Session.Flush();

            var actual = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(1, actual.Count);
            Assert.IsTrue(actual.Contains(order));
            Assert.IsFalse(actual.Contains(extraOrder));
            Assert.IsNotNull(Repository.FindFinalizationOrder(order.Id));
        }

        [TestMethod]
        public void TestFinalizationOrdersReturnsWorkOrdersWithValidSAPCriteria()
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
            
            var results = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.Contains(validWorkOrderBecauseItHasSAPWorkOrderNumber));
            Assert.IsNotNull(Repository.FindFinalizationOrder(validWorkOrderBecauseItHasSAPWorkOrderNumber.Id));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsNotSAPEnabled));
            Assert.IsNotNull(Repository.FindFinalizationOrder(validWorkOrderBecauseIsNotSAPEnabled.Id));
            Assert.IsTrue(results.Contains(validWorkOrderBecauseIsContractedOperations));
            Assert.IsNotNull(Repository.FindFinalizationOrder(validWorkOrderBecauseIsContractedOperations.Id));

            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber));
            Assert.IsNull(Repository.FindFinalizationOrder(invalidBecauseItIsSAPEnabledWithoutAnSAPWorkOrderNumber.Id));
            Assert.IsFalse(results.Contains(invalidBecauseItIsSAPEnabledButNotContractedOperations));
            Assert.IsNull(Repository.FindFinalizationOrder(invalidBecauseItIsSAPEnabledButNotContractedOperations.Id));
            Assert.IsFalse(results.Contains(invalidBecauseItDoesNotHaveSAPWorkOrderNumber));
            Assert.IsNull(Repository.FindFinalizationOrder(invalidBecauseItDoesNotHaveSAPWorkOrderNumber.Id));
        }

        [TestMethod]
        public void TestFinalizationOrdersReturnWorkOrdersAssignedToContractorsWithOrWithoutCrewAssignments()
        {
            var crew = GetFactory<CrewFactory>().Create();
            var order = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = GetFactory<ContractorFactory>().Create()
            });
            var orderWithAssignments = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = GetFactory<ContractorFactory>().Create()
            });
            orderWithAssignments.CrewAssignments.Add(
                GetFactory<CrewAssignmentFactory>().Create(new {
                    AssignedFor = _now.AddHours(-1),
                    Crew = crew
                }));
            Session.SaveOrUpdate(crew);
            Session.SaveOrUpdate(orderWithAssignments);
            Session.Flush();

            var actual = Repository.FinalizationOrders.List<WorkOrder>();

            Assert.AreEqual(2, actual.Count);
        }

        #endregion

        #endregion

        #region FindByPartialWorkOrderID

        [TestMethod]
        public void TestFindByPartialWorkOrderIDMatchReturnsEmptyCollectionIfNoResults()
        {
            var result = Repository.FindByPartialWorkOrderIDMatch("1232111111");
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TestFindByPartialWorkOrderIDMatchReturnsEmptyCollectionifNullOrEmptyParameter()
        {
            var result = Repository.FindByPartialWorkOrderIDMatch(null);
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public void TestFindByPartialWorkOrderIDMatchReturnsExpectedMatches()
        {
            var workOrders = GetFactory<WorkOrderFactory>().CreateList(12);

            var result = Repository.FindByPartialWorkOrderIDMatch("1");
            Assert.AreEqual(4, result.Count());

            result = Repository.FindByPartialWorkOrderIDMatch("12");
            Assert.AreEqual(1, result.Count());
        }

        #endregion

        #region GetSAPNotificationsCriterion

        [TestMethod]
        public void TestGetSAPNotificationsCriterionReturnsOrdersWithSAPNotifications()
        {
            var valid = GetFactory<WorkOrderFactory>().Create(new {SAPNotificationNumber = 9999999999});
            var valid2 = GetFactory<WorkOrderFactory>().Create(new
                {SAPNotificationNumber = 9999999999, DateCompleted = Lambdas.GetNow()});
            var invalid = GetFactory<WorkOrderFactory>().Create();

            var target = Repository.Search(Repository.GetSAPNotificationsCriterion()).List<WorkOrder>();

            Assert.AreEqual(2, target.Count);
            Assert.AreSame(valid, target[0]);
            Assert.AreSame(valid2, target[1]);
        }

        #endregion

        #region GetDistinctYearsCompleted

        [TestMethod]
        public void TestGetDistinctYearsCompletedReturnsDistinctYearsCompleted()
        {
            var wo1 = GetFactory<WorkOrderFactory>().Create(new {DateCompleted = Lambdas.GetYesterday});
            var wo2 = GetFactory<WorkOrderFactory>().Create(new {DateCompleted = Lambdas.GetYesterday});
            var wo3 = GetFactory<WorkOrderFactory>().Create(new {DateCompleted = Lambdas.GetLastYear});

            var target = Repository.GetDistinctYearsCompleted();

            Assert.AreEqual(2, target.Count());
            Assert.AreEqual(wo1.DateCompleted.Value.Year, target.First());
            Assert.AreEqual(wo3.DateCompleted.Value.Year, target.Last());
        }

        #endregion

        #region FindBySAPWorkOrderNumber

        [TestMethod]
        public void TestFindBySAPWorkOrderNumberReturnsMatch()
        {
            var valid = GetFactory<WorkOrderFactory>().Create(new {SAPWorkOrderNumber = 9999999999});
            var invalid = GetFactory<WorkOrderFactory>().Create();

            var result = Repository.FindBySAPWorkOrderNumber(9999999999);
            Assert.AreSame(valid, result);
        }

        [TestMethod]
        public void TestFindBySAPWorkOrderNumberReturnsNullIfNoMatch()
        {
            var valid = GetFactory<WorkOrderFactory>().Create(new {SAPWorkOrderNumber = 9999999999});
            var result = Repository.FindBySAPWorkOrderNumber(123456789);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void
            TestFindBySAPWorkOrderDoesNotHaveAMassiveBreakdownAndCryBecauseTheyAllowMultipleWorkOrdersToHaveTheSameSAPWorkOrderNumber()
        {
            const long sap = 9999999999;
            var valid = GetFactory<WorkOrderFactory>().Create(new {SAPWorkOrderNumber = sap});
            var alsoConsiderablyValid = GetFactory<WorkOrderFactory>().Create(new {SAPWorkOrderNumber = sap});

            var result = Repository.FindBySAPWorkOrderNumber(sap);
            Assert.AreSame(valid, result);
        }

        #endregion

        #region GetMainBreakReport

        [TestMethod]
        public void TestGetMainBreaksAndServiceLineRepairsReturnsWorkOrdersForMainBreaksAndServiceLineRepairs()
        {
            var waterReplace = GetFactory<WaterMainBreakReplaceWorkDescriptionFactory>().Create();
            var waterRepair = GetFactory<WaterMainBreakRepairWorkDescriptionFactory>().Create();
            var sewerReplace = GetFactory<SewerMainBreakReplaceWorkDescriptionFactory>().Create();
            var sewerRepair = GetFactory<SewerMainBreakRepairWorkDescriptionFactory>().Create();
            var serviceRepair = GetFactory<ServiceLineRepairWorkDescriptionFactory>().Create();
            var other = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            //valid
            var wo1 = GetEntityFactory<WorkOrder>()
               .Create(new {WorkDescription = waterReplace, DateCompleted = Lambdas.GetNow});
            var wo2 = GetEntityFactory<WorkOrder>()
               .Create(new {WorkDescription = waterRepair, DateCompleted = Lambdas.GetNow});
            var wo3 = GetEntityFactory<WorkOrder>()
               .Create(new {WorkDescription = sewerReplace, DateCompleted = Lambdas.GetNow});
            var wo4 = GetEntityFactory<WorkOrder>()
               .Create(new {WorkDescription = sewerRepair, DateCompleted = Lambdas.GetNow});
            var wo5 = GetEntityFactory<WorkOrder>()
               .Create(new {WorkDescription = serviceRepair, DateCompleted = Lambdas.GetNow});
            //invalid
            var wo6 = GetEntityFactory<WorkOrder>()
               .Create(new {WorkDescription = other, DateCompleted = Lambdas.GetNow});
            var wo7 = GetEntityFactory<WorkOrder>().Create(new {WorkDescription = waterReplace});

            var search = new EmptySearchSet<WorkOrder>();
            var results = Repository.SearchMainBreaksAndServiceLineRepairsReport(search).ToList();
            var workDescriptions = results.Select(x => x.WorkDescription).Distinct();

            Assert.IsNotNull(results);
            Assert.AreEqual(5, results.Count());
            Assert.IsFalse(workDescriptions.Contains(other));
            Assert.AreEqual(1, results.Count(x => x.WorkDescription.Id == waterReplace.Id));
        }

        [TestMethod]
        public void
            TestSearchMainBreaksAndServiceLineRepairsReportReturnsValidWorkOrdersForMainbreaksAndServiceLineRepairs()
        {
            var waterReplace = GetFactory<WaterMainBreakReplaceWorkDescriptionFactory>().Create();
            var waterRepair = GetFactory<WaterMainBreakRepairWorkDescriptionFactory>().Create();
            var sewerReplace = GetFactory<SewerMainBreakReplaceWorkDescriptionFactory>().Create();
            var sewerRepair = GetFactory<SewerMainBreakRepairWorkDescriptionFactory>().Create();
            var serviceRepair = GetFactory<ServiceLineRepairWorkDescriptionFactory>().Create();
            var other = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();

            //valid
            var wo1 = GetEntityFactory<WorkOrder>().Create(new {
                WorkDescription = waterReplace, DateCompleted = new DateTime(2014, 1, 1),
                OperatingCenter = operatingCenter
            });
            var wo11 = GetEntityFactory<WorkOrder>().Create(new {
                WorkDescription = waterReplace, DateCompleted = new DateTime(2014, 2, 1),
                OperatingCenter = operatingCenter
            });
            var wo2 = GetEntityFactory<WorkOrder>().Create(new {
                WorkDescription = waterRepair, DateCompleted = new DateTime(2014, 1, 1),
                OperatingCenter = operatingCenter
            });
            var wo21 = GetEntityFactory<WorkOrder>().Create(new {
                WorkDescription = waterRepair, DateCompleted = new DateTime(2014, 2, 1),
                OperatingCenter = operatingCenter
            });
            var wo3 = GetEntityFactory<WorkOrder>().Create(new {
                WorkDescription = sewerReplace, DateCompleted = new DateTime(2014, 1, 1),
                OperatingCenter = operatingCenter
            });
            var wo31 = GetEntityFactory<WorkOrder>().Create(new {
                WorkDescription = sewerReplace, DateCompleted = new DateTime(2014, 2, 1),
                OperatingCenter = operatingCenter
            });
            var wo4 = GetEntityFactory<WorkOrder>().Create(new {
                WorkDescription = sewerRepair, DateCompleted = new DateTime(2014, 1, 1),
                OperatingCenter = operatingCenter
            });
            var wo41 = GetEntityFactory<WorkOrder>().Create(new {
                WorkDescription = sewerRepair, DateCompleted = new DateTime(2014, 2, 1),
                OperatingCenter = operatingCenter
            });
            var wo5 = GetEntityFactory<WorkOrder>().Create(new {
                WorkDescription = serviceRepair, DateCompleted = new DateTime(2014, 1, 1),
                OperatingCenter = operatingCenter
            });
            var wo51 = GetEntityFactory<WorkOrder>().Create(new {
                WorkDescription = serviceRepair, DateCompleted = new DateTime(2014, 2, 1),
                OperatingCenter = operatingCenter
            });
            var wo52 = GetEntityFactory<WorkOrder>().Create(new {
                WorkDescription = serviceRepair, DateCompleted = new DateTime(2014, 2, 1),
                OperatingCenter = operatingCenter
            });
            //invalid
            var wo6 = GetEntityFactory<WorkOrder>()
               .Create(new {WorkDescription = other, DateCompleted = Lambdas.GetNow});
            var wo7 = GetEntityFactory<WorkOrder>().Create(new {WorkDescription = waterReplace});

            var search = new EmptySearchSet<WorkOrder>();
            var results = Repository.SearchMainBreaksAndServiceLineRepairsReport(search).ToList();

            Assert.IsNotNull(results);

            Assert.AreEqual(serviceRepair, results[0].WorkDescription);
            Assert.AreEqual(1, results[0].Jan);
            Assert.AreEqual(2, results[0].Feb);
            Assert.AreEqual(3, results[0].Total);
            Assert.AreEqual(sewerRepair, results[1].WorkDescription);
            Assert.AreEqual(sewerReplace, results[2].WorkDescription);
            Assert.AreEqual(waterRepair, results[3].WorkDescription);
            Assert.AreEqual(1, results[3].Jan);
            Assert.AreEqual(1, results[3].Feb);
            Assert.AreEqual(2, results[3].Total);
            Assert.AreEqual(waterReplace, results[4].WorkDescription);
        }

        #endregion

        #region GetLostWaterInPastDay

        [TestMethod]
        public void TestGetLostWaterInPastDayGetsLostWaterInPastDay()
        {
            var now = _now;
            var yesterday = now.AddDays(-1);
            _dateTimeProvider.SetNow(now);
            var lostWater = GetEntityFactory<WorkOrder>().Create(new {DateCompleted = yesterday, LostWater = 123});
            var noLostWater = GetEntityFactory<WorkOrder>().Create(new {DateCompleted = yesterday, LostWater = 0});
            var previousDay = GetEntityFactory<WorkOrder>()
               .Create(new {DateCompleted = yesterday.Date.AddSeconds(-1), LostWater = 123});
            var today = GetEntityFactory<WorkOrder>()
               .Create(new {DateCompleted = yesterday.Date.AddDays(1), LostWater = 123});

            var results = Repository.GetLostWaterInPastDay();

            MyAssert.Contains(results, lostWater, "Expected order was not in result");
            MyAssert.DoesNotContain(results, noLostWater, "Order with no lost water was in result");
            MyAssert.DoesNotContain(results, previousDay, "Order from previous day was in result");
            MyAssert.DoesNotContain(results, today, "Order from today was in result");
        }

        #endregion

        #region GetWorkOrdersWithSapIssues

        [TestMethod]
        public void TestGetWorkOrdersWithSapIssuesReturnsWorkOrdersWithSapIssues()
        {
            var sapWorkOrderStep = GetFactory<CreateSAPWorkOrderStepFactory>().Create();
            var wo1 = GetFactory<WorkOrderFactory>().Create(new {SAPErrorCode = "Successfully"});
            var wo2 = GetFactory<WorkOrderFactory>().Create(new {SAPErrorCode = "Something went wrong"});
            var wo3 = GetFactory<WorkOrderFactory>().Create(new {SAPErrorCode = "RETRY::Connection Error"});
            var wo4 = GetFactory<WorkOrderFactory>().Create(new
                {SAPErrorCode = "RETRY::Connection Error", SAPWorkOrderStep = sapWorkOrderStep});

            var result = Repository.GetWorkOrdersWithSapRetryIssues();

            Assert.AreEqual(1, result.Count());
        }

        #endregion

        #region GetMainBreakServiceLineRegulatedReport

        [TestMethod]
        public void TestGetMainBreakServiceLineRegulated()
        {
            var op2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var waterReplace = GetFactory<WaterMainBreakReplaceWorkDescriptionFactory>().Create();
            var waterRepair = GetFactory<WaterMainBreakRepairWorkDescriptionFactory>().Create();
            var sewerReplace = GetFactory<SewerMainBreakReplaceWorkDescriptionFactory>().Create();
            var sewerRepair = GetFactory<SewerMainBreakRepairWorkDescriptionFactory>().Create();
            var serviceRepair = GetFactory<ServiceLineRepairWorkDescriptionFactory>().Create();
            var other = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            //valid
            var wo1 = GetEntityFactory<WorkOrder>().Create(new
                {OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace, DateCompleted = Lambdas.GetNow});
            var wo1a = GetEntityFactory<WorkOrder>().Create(new
                {OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace, DateCompleted = Lambdas.GetNow});
            var wo2 = GetEntityFactory<WorkOrder>().Create(new
                {OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterRepair, DateCompleted = Lambdas.GetNow});
            var wo3 = GetEntityFactory<WorkOrder>().Create(new
                {OperatingCenter = User.DefaultOperatingCenter, WorkDescription = sewerReplace, DateCompleted = Lambdas.GetNow});
            var wo4 = GetEntityFactory<WorkOrder>().Create(new
                {OperatingCenter = User.DefaultOperatingCenter, WorkDescription = sewerRepair, DateCompleted = Lambdas.GetNow});
            var wo5 = GetEntityFactory<WorkOrder>().Create(new
                {OperatingCenter = op2, WorkDescription = serviceRepair, DateCompleted = Lambdas.GetNow});
            var wo6 = GetEntityFactory<WorkOrder>().Create(new
                {OperatingCenter = op2, WorkDescription = other, DateCompleted = Lambdas.GetNow});
            var wo6a = GetEntityFactory<WorkOrder>().Create(new
                {OperatingCenter = op2, WorkDescription = other, DateCompleted = Lambdas.GetNow});
            var wo7 = GetEntityFactory<WorkOrder>().Create(new
                {OperatingCenter = op2, WorkDescription = waterReplace, DateCompleted = Lambdas.GetNow});

            var search = new TestSearchMainBreakServiceLine {Year = Lambdas.GetNow().Year};

            var result = Repository.GetCompletedWorkOrdersForMainBreaksOnServices(search).ToArray();

            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void TestGetMainBreakServiceLineRegulatedReport()
        {
            var valve = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = User.DefaultOperatingCenter
            });
            // want two states
            // several months
            var state2 = GetFactory<StateFactory>().Create(new {Abbreviation = "NY", Name = "New York"});
            var op2 = GetFactory<UniqueOperatingCenterFactory>().Create(new {State = state2});
            var op3 = GetFactory<UniqueOperatingCenterFactory>().Create(new {State = state2});
            var waterReplace = GetFactory<WaterMainBreakReplaceWorkDescriptionFactory>().Create();
            var waterRepair = GetFactory<WaterMainBreakRepairWorkDescriptionFactory>().Create();
            var sewerReplace = GetFactory<SewerMainBreakReplaceWorkDescriptionFactory>().Create();
            var sewerRepair = GetFactory<SewerMainBreakRepairWorkDescriptionFactory>().Create();

            var woValid1 = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2018, 2, 1), Valve = valve
            });
            var woValid2a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2018, 3, 1), Valve = valve
            });
            var woValid2b = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2018, 3, 4), Valve = valve
            });
            var woValid3 = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2018, 12, 1), Valve = valve
            });
            var woValid4 = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2018, 11, 1), Valve = valve
            });
            var woWrongDescription1 = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = sewerReplace,
                DateCompleted = new DateTime(2018, 2, 1), Valve = valve
            });
            var woWrongDescription2 = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = sewerRepair,
                DateCompleted = new DateTime(2018, 7, 1), Valve = valve
            });

            var woValid5a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op2, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2018, 2, 1), Valve = valve
            });
            var woValid6a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op2, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2018, 8, 1), Valve = valve
            });

            var woValid6b = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2018, 9, 1), Valve = valve
            });
            var woValid6c = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2018, 9, 1), Valve = valve
            });
            var woValid7a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2018, 12, 1), Valve = valve
            });

            var woInThePast1a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2015, 2, 1), Valve = valve
            });
            var woInThePast1b = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2016, 3, 1), Valve = valve
            });
            var woInThePast1c = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2017, 3, 4), Valve = valve
            });
            var woInThePast2a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2015, 12, 1), Valve = valve
            });
            var woInThePast2b = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2016, 11, 1), Valve = valve
            });
            var woInThePast3a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = sewerReplace,
                DateCompleted = new DateTime(2017, 2, 1), Valve = valve
            });
            var woInThePast4a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = sewerRepair,
                DateCompleted = new DateTime(2016, 7, 1), Valve = valve
            });
            var woInThePast5a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op2, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2014, 2, 1), Valve = valve
            });
            var woInThePast6a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op2, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2011, 8, 1), Valve = valve
            });
            var woInThePast6b = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterRepair,
                DateCompleted = new DateTime(1995, 9, 1), Valve = valve
            });
            var woInThePast6c = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2017, 9, 1), Valve = valve
            });
            var woInThePast7a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2017, 12, 1), Valve = valve
            });

            var search = new TestSearchMainBreakServiceLine {
                Year = 2018,
                OperatingCenter = new[] {
                    User.DefaultOperatingCenter.Id, op2.Id, op3.Id
                }
            };

            var result = Repository.GetMainBreakServiceLineReport(search).ToArray();

            //1st Row of results - state 1, op1, waterRepair
            Assert.AreEqual(_state1.Abbreviation, result[0].State);
            Assert.AreEqual(User.DefaultOperatingCenter.ToString(), result[0].OperatingCenter);
            Assert.AreEqual(waterRepair.Description, result[0].WorkDescription);
            Assert.AreEqual(0, result[0].Jan);
            Assert.AreEqual(1, result[0].Nov);
            Assert.AreEqual(1, result[0].Dec);

            //2nd Row of results - state 1, op1, waterReplace
            Assert.AreEqual(_state1.Abbreviation, result[1].State);
            Assert.AreEqual(User.DefaultOperatingCenter.ToString(), result[1].OperatingCenter);
            Assert.AreEqual(waterReplace.Description, result[1].WorkDescription);
            //3rd Row of results - op1 summary
            Assert.AreEqual(_state1.Abbreviation, result[2].State);
            Assert.AreEqual(User.DefaultOperatingCenter.ToString(), result[2].OperatingCenter);
            Assert.IsNull(result[2].WorkDescription);
            Assert.AreEqual(5, result[2].Total);

            //4th row - state 1 summary
            Assert.AreEqual(_state1.Abbreviation + " Total", result[3].State);
            Assert.IsNull(result[3].OperatingCenter);
            Assert.IsNull(result[3].WorkDescription);

            //5th Row of results - state 2, op2, waterRepair
            Assert.AreEqual(state2.Abbreviation, result[4].State);
            Assert.AreEqual(op2.ToString(), result[4].OperatingCenter);
            Assert.AreEqual(waterRepair.Description, result[4].WorkDescription);

            //6th Row of results - state 2, op2, waterReplace
            Assert.AreEqual(state2.Abbreviation, result[5].State);
            Assert.AreEqual(op2.ToString(), result[5].OperatingCenter);
            Assert.AreEqual(waterReplace.Description, result[5].WorkDescription);

            //7th Row - state 2, op2 summary
            Assert.AreEqual(state2.Abbreviation, result[6].State);
            Assert.AreEqual(op2.ToString(), result[6].OperatingCenter);
            Assert.IsNull(result[6].WorkDescription);

            //8th Row - state 2, op3, waterRepair
            Assert.AreEqual(state2.Abbreviation, result[7].State);
            Assert.AreEqual(op3.ToString(), result[7].OperatingCenter);
            Assert.AreEqual(waterRepair.Description, result[7].WorkDescription);

            //9th Row state 2, op3, waterReplace
            Assert.AreEqual(state2.Abbreviation, result[8].State);
            Assert.AreEqual(op3.ToString(), result[8].OperatingCenter);
            Assert.AreEqual(waterReplace.Description, result[8].WorkDescription);

            //10th Row state 2, op3, summary
            Assert.AreEqual(state2.Abbreviation, result[9].State);
            Assert.AreEqual(op3.ToString(), result[9].OperatingCenter);
            Assert.IsNull(result[9].WorkDescription);

            //11th Row state2 summary
            Assert.AreEqual(state2.Abbreviation + " Total", result[10].State);
            Assert.IsNull(result[10].OperatingCenter);
            Assert.IsNull(result[10].WorkDescription);

            //12th row summary
            Assert.AreEqual("Total", result[11].State);
            Assert.IsNull(result[11].OperatingCenter);
            Assert.IsNull(result[11].WorkDescription);
            Assert.AreEqual(10, result[11].Total);
        }

        [TestMethod]
        public void TestGetMainBreakServiceLineReportDoesNotBreakWhenOnlyStateSelected()
        {
            var valve = GetEntityFactory<Valve>().Create(new {
                OperatingCenter = User.DefaultOperatingCenter
            });
            // want two states
            // several months
            var state2 = GetFactory<StateFactory>().Create(new {Abbreviation = "NY", Name = "New York"});
            var op2 = GetFactory<UniqueOperatingCenterFactory>().Create(new {State = state2});
            var op3 = GetFactory<UniqueOperatingCenterFactory>().Create(new {State = state2});
            var waterReplace = GetFactory<WaterMainBreakReplaceWorkDescriptionFactory>().Create();
            var waterRepair = GetFactory<WaterMainBreakRepairWorkDescriptionFactory>().Create();
            var sewerReplace = GetFactory<SewerMainBreakReplaceWorkDescriptionFactory>().Create();
            var sewerRepair = GetFactory<SewerMainBreakRepairWorkDescriptionFactory>().Create();

            var woValid1 = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2018, 2, 1), Valve = valve
            });
            var woValid2a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2018, 3, 1), Valve = valve
            });
            var woValid2b = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2018, 3, 4), Valve = valve
            });
            var woValid3 = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2018, 12, 1), Valve = valve
            });
            var woValid4 = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2018, 11, 1), Valve = valve
            });
            var woWrongDescription1 = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = sewerReplace,
                DateCompleted = new DateTime(2018, 2, 1), Valve = valve
            });
            var woWrongDescription2 = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = sewerRepair,
                DateCompleted = new DateTime(2018, 7, 1), Valve = valve
            });

            var woValid5a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op2, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2018, 2, 1), Valve = valve
            });
            var woValid6a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op2, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2018, 8, 1), Valve = valve
            });

            var woValid6b = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2018, 9, 1), Valve = valve
            });
            var woValid6c = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2018, 9, 1), Valve = valve
            });
            var woValid7a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2018, 12, 1), Valve = valve
            });

            var woInThePast1a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2015, 2, 1), Valve = valve
            });
            var woInThePast1b = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2016, 3, 1), Valve = valve
            });
            var woInThePast1c = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2017, 3, 4), Valve = valve
            });
            var woInThePast2a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2015, 12, 1), Valve = valve
            });
            var woInThePast2b = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2016, 11, 1), Valve = valve
            });
            var woInThePast3a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = sewerReplace,
                DateCompleted = new DateTime(2017, 2, 1), Valve = valve
            });
            var woInThePast4a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = User.DefaultOperatingCenter, WorkDescription = sewerRepair,
                DateCompleted = new DateTime(2016, 7, 1), Valve = valve
            });
            var woInThePast5a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op2, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2014, 2, 1), Valve = valve
            });
            var woInThePast6a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op2, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2011, 8, 1), Valve = valve
            });
            var woInThePast6b = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterRepair,
                DateCompleted = new DateTime(1995, 9, 1), Valve = valve
            });
            var woInThePast6c = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterRepair,
                DateCompleted = new DateTime(2017, 9, 1), Valve = valve
            });
            var woInThePast7a = GetEntityFactory<WorkOrder>().Create(new {
                CreatedBy = User, OperatingCenter = op3, WorkDescription = waterReplace,
                DateCompleted = new DateTime(2017, 12, 1), Valve = valve
            });

            var search = new TestSearchMainBreakServiceLine {
                Year = 2018,
                State = state2.Id
            };

            MyAssert.DoesNotThrow(() => Repository.GetMainBreakServiceLineReport(search).ToArray());
        }

        #endregion

        #region GetCompletedWorkOrderPreJobSafetyBriefCounts

        [TestMethod]
        public void TestGetCompletedWorkOrderPreJobSafetyBriefCountsWorksAsExpected()
        {
            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {State = state});

            // Create first result we want to see. 
            // One work description, one work order, one safetyBrief
            var waterMainBleeders = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = waterMainBleeders, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });
            var safetyBrief1 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder1});

            // Create second result we want to see
            // One work desc, two work orders, two safetyBrief
            var changeBurstMeter = GetFactory<ChangeBurstMeterWorkDescriptionFactory>().Create();
            var workorder2 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = changeBurstMeter, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });
            var workorder3 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = changeBurstMeter, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });
            var safetyBrief2 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder2});
            var safetyBrief3 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder3});

            // Create third result we want to see
            // One work desc, two work orders, one safetyBrief
            var checkNoWater = GetFactory<CheckNoWaterWorkDescriptionFactory>().Create();
            var workorder4 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = checkNoWater, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var workorder5 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = checkNoWater, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var safetyBrief4 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder4});

            // Create fourth result we want to see
            // One work desc, one work order, two safetyBrief. This should only return 1 for the WorkOrdersWithPreJobSafetyBriefCount instead of 2.
            var curbBoxRepair = GetFactory<CurbBoxRepairWorkDescriptionFactory>().Create();
            var workorder6 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = curbBoxRepair, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var safetyBrief5 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder6});
            var safetyBrief6 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder6});

            // Create fifth result we want to see
            // One work desc, one work order, no safetyBrief.
            var ballCurbStopRepair = GetFactory<BallCurbStopRepairWorkDescriptionFactory>().Create();
            var workorder7 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = ballCurbStopRepair, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });

            var search = new TestSearchCompletedWorkOrdersWithPreJobSafetyBriefs();
            // DateCompleted does not matter for this test, but it has to be set because it's required.
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.MinValue,
                End = DateTime.MaxValue
            };
            var results = Repository.GetCompletedWorkOrderPreJobSafetyBriefCounts(search).ToList();
            Assert.AreEqual(5, search.Count, "Sanity");

            var result = results.Single(x => x.WorkDescription == waterMainBleeders.Description);
            Assert.AreEqual(waterMainBleeders.Description, result.WorkDescription);
            Assert.AreEqual(waterMainBleeders.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder1.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder1.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder1.Town.State.Abbreviation, result.State);
            Assert.AreEqual(1, result.WorkOrderCount);
            Assert.AreEqual(1, result.WorkOrdersWithPreJobSafetyBriefCount);

            result = results.Single(x => x.WorkDescription == changeBurstMeter.Description);
            Assert.AreEqual(changeBurstMeter.Description, result.WorkDescription);
            Assert.AreEqual(changeBurstMeter.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder2.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder2.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder2.Town.State.Abbreviation, result.State);
            Assert.AreEqual(2, result.WorkOrderCount);
            Assert.AreEqual(2, result.WorkOrdersWithPreJobSafetyBriefCount);

            result = results.Single(x => x.WorkDescription == checkNoWater.Description);
            Assert.AreEqual(checkNoWater.Description, result.WorkDescription);
            Assert.AreEqual(checkNoWater.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder4.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder4.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder4.Town.State.Abbreviation, result.State);
            Assert.AreEqual(2, result.WorkOrderCount);
            Assert.AreEqual(1, result.WorkOrdersWithPreJobSafetyBriefCount);

            result = results.Single(x => x.WorkDescription == curbBoxRepair.Description);
            Assert.AreEqual(curbBoxRepair.Description, result.WorkDescription);
            Assert.AreEqual(curbBoxRepair.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder6.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder6.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder6.Town.State.Abbreviation, result.State);
            Assert.AreEqual(1, result.WorkOrderCount);
            Assert.AreEqual(1, result.WorkOrdersWithPreJobSafetyBriefCount);

            result = results.Single(x => x.WorkDescription == ballCurbStopRepair.Description);
            Assert.AreEqual(ballCurbStopRepair.Description, result.WorkDescription);
            Assert.AreEqual(ballCurbStopRepair.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder7.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder7.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder7.Town.State.Abbreviation, result.State);
            Assert.AreEqual(1, result.WorkOrderCount);
            Assert.AreEqual(0, result.WorkOrdersWithPreJobSafetyBriefCount);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrdersPreJobSafetyBriefCountOnlyPullsWhenRequiredFieldIsNotNull()
        {
            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {State = state});
            var waterMainBleeders = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = waterMainBleeders, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });
            var safetyBrief1 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder1});
            safetyBrief1.AnyPotentialWeatherHazards = null;
            Session.Flush();
            Session.Clear();
            Session.Save(safetyBrief1);
            var workorder3 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = waterMainBleeders, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });
            var safetyBrief2 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder3});

            var search = new TestSearchCompletedWorkOrdersWithPreJobSafetyBriefs();

            search.OperatingCenter = new[] {operatingCenter.Id};
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.MinValue,
                End = DateTime.MaxValue
            };

            var results = Repository.GetCompletedWorkOrderPreJobSafetyBriefCounts(search).ToList();
            Assert.AreEqual(1, search.Count, "Sanity");
            var result = results.Single(x => x.OperatingCenterId == operatingCenter.Id);
            Assert.AreEqual(1, result.WorkOrdersWithoutPreJobSafetyBriefCount);
            Assert.AreEqual(1, result.WorkOrdersWithPreJobSafetyBriefCount);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderPreJobSafetyBriefCountsCorrectlyFiltersByDateCompleted()
        {
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>()
               .Create(new {WorkDescription = workDesc1, DateCompleted = DateTime.Today});
            var workorder2 = GetFactory<WorkOrderFactory>().Create(new {WorkDescription = workDesc1});
            var safetyBrief1 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder1});

            var search = new TestSearchCompletedWorkOrdersWithPreJobSafetyBriefs();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };
            var result = Repository.GetCompletedWorkOrderPreJobSafetyBriefCounts(search).Single();
            Assert.AreEqual(1, result.WorkOrderCount);
            Assert.AreEqual(1, result.WorkOrdersWithPreJobSafetyBriefCount);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderPreJobSafetyBriefCountsCorrectlyFiltersByOperatingCenters()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc3 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, OperatingCenter = opc1});
            var safetyBrief1 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder1});
            var workorder2 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, OperatingCenter = opc2});
            var safetyBrief2 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder2});
            var workorder3 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, OperatingCenter = opc3});
            var safetyBrief3 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder3});

            var search = new TestSearchCompletedWorkOrdersWithPreJobSafetyBriefs();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };
            search.OperatingCenter = new[] {opc1.Id, opc2.Id};
            var results = Repository.GetCompletedWorkOrderPreJobSafetyBriefCounts(search).ToList();
            Assert.AreEqual(2, results.Count(), "Sanity");

            var opc1Result = results.Single(x => x.OperatingCenter == opc1.OperatingCenterCode);
            Assert.AreEqual(1, opc1Result.WorkOrderCount);
            Assert.AreEqual(1, opc1Result.WorkOrdersWithPreJobSafetyBriefCount);
            var opc2Result = results.Single(x => x.OperatingCenter == opc2.OperatingCenterCode);
            Assert.AreEqual(1, opc2Result.WorkOrderCount);
            Assert.AreEqual(1, opc2Result.WorkOrdersWithPreJobSafetyBriefCount);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderPreJobSafetyBriefCountsCorrectlyFiltersByState()
        {
            var state1 = GetFactory<StateFactory>().Create(new {Abbreviation = "XX"});
            var county1 = GetFactory<CountyFactory>().Create(new {State = state1});
            var town1 = GetFactory<TownFactory>().Create(new {County = county1});
            var state2 = GetFactory<StateFactory>().Create(new {Abbreviation = "ZZ"});
            var county2 = GetFactory<CountyFactory>().Create(new {State = state2});
            var town2 = GetFactory<TownFactory>().Create(new {County = county2});
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, Town = town1});
            var safetyBrief1 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder1});
            var workorder2 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, Town = town2});
            var safetyBrief2 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder2});

            var search = new TestSearchCompletedWorkOrdersWithPreJobSafetyBriefs();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };
            search.State = state1.Id;
            var results = Repository.GetCompletedWorkOrderPreJobSafetyBriefCounts(search).ToList();
            Assert.AreEqual(1, results.Count(), "Sanity");

            var opc1Result = results.Single(x => x.State == state1.Abbreviation);
            Assert.AreEqual(1, opc1Result.WorkOrderCount);
            Assert.AreEqual(1, opc1Result.WorkOrdersWithPreJobSafetyBriefCount);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderPreJobSafetyBriefCountsCorrectlyFiltersByWorkDescription()
        {
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>()
               .Create(new {WorkDescription = workDesc1, DateCompleted = DateTime.Today});
            var safetyBrief1 = GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workorder1});

            // Test that we can find the workorder when we're specifically filtering for it.
            var search = new TestSearchCompletedWorkOrdersWithPreJobSafetyBriefs();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };
            search.WorkDescription = new[] {workDesc1.Id};

            var result = Repository.GetCompletedWorkOrderPreJobSafetyBriefCounts(search).Single();
            Assert.AreEqual(workDesc1.Description, result.WorkDescription);

            // Test that we do not find that same workorder when it's not included in the filter.
            search.WorkDescription = new[] {0};
            Assert.IsFalse(Repository.GetCompletedWorkOrderPreJobSafetyBriefCounts(search).Any());
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderPreJobSafetyBriefCountsCorrectlyFiltersByIsAssignedContractor()
        {
            // WorkDescriptions are only needed for this test so we can actually check the result since the work orders
            // themselves are not actually in the results.
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workDesc2 = GetFactory<ChangeBurstMeterWorkDescriptionFactory>().Create();
            var workOrderNotAssignedContractor = GetFactory<WorkOrderFactory>()
               .Create(new {WorkDescription = workDesc1, DateCompleted = DateTime.Today});
            Assert.IsNull(workOrderNotAssignedContractor.AssignedContractor, "Sanity");
            GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workOrderNotAssignedContractor});
            var workOrderAssignedContractor = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = workDesc2, DateCompleted = DateTime.Today,
                AssignedContractor = typeof(ContractorFactory)
            });
            GetFactory<JobSiteCheckListFactory>().Create(new {MapCallWorkOrder = workOrderAssignedContractor});

            // Test that we can find the workorder when we're specifically filtering for it.
            var search = new TestSearchCompletedWorkOrdersWithPreJobSafetyBriefs();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };

            // Test if IsAssignedContractor filter is ignored when the value is null
            search.IsAssignedContractor = null;
            var result = Repository.GetCompletedWorkOrderPreJobSafetyBriefCounts(search).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(x => x.WorkDescriptionId == workDesc1.Id));
            Assert.IsTrue(result.Any(x => x.WorkDescriptionId == workDesc2.Id));

            // Test that we only return results for work orders without assigned contractors
            search.IsAssignedContractor = false;
            Assert.AreEqual(workDesc1.Id,
                Repository.GetCompletedWorkOrderPreJobSafetyBriefCounts(search).Single().WorkDescriptionId);

            // Test that we only return results for work orders that have assigned contractors
            search.IsAssignedContractor = true;
            Assert.AreEqual(workDesc2.Id,
                Repository.GetCompletedWorkOrderPreJobSafetyBriefCounts(search).Single().WorkDescriptionId);
        }

        #endregion

        #region GetCompletedWorkOrdersWithJobSiteCheckListCounts

        [TestMethod]
        public void TestGetCompletedWorkOrderJobSiteCheckListCountsWorksAsExpected()
        {
            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {State = state});

            // Create first result we want to see. 
            // One work description, one work order, one jscl
            var waterMainBleeders = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = waterMainBleeders, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });
            var jscl1 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder1, HasExcavation = true});

            // Create second result we want to see
            // One work desc, two work orders, two jscl
            var changeBurstMeter = GetFactory<ChangeBurstMeterWorkDescriptionFactory>().Create();
            var workorder2 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = changeBurstMeter, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });
            var workorder3 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = changeBurstMeter, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });
            var jscl2 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder2, HasExcavation = true});
            var jscl3 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder3, HasExcavation = true});

            // Create third result we want to see
            // One work desc, two work orders, one jscl
            var checkNoWater = GetFactory<CheckNoWaterWorkDescriptionFactory>().Create();
            var workorder4 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = checkNoWater, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var workorder5 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = checkNoWater, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var jscl4 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder4, HasExcavation = true});

            // Create fourth result we want to see
            // One work desc, one work order, two jscl. This should only return 1 for the WorkOrdersWithPreJobSafetyBriefCount instead of 2.
            var curbBoxRepair = GetFactory<CurbBoxRepairWorkDescriptionFactory>().Create();
            var workorder6 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = curbBoxRepair, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var jscl5 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder6, HasExcavation = true});
            var jscl6 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder6, HasExcavation = true});

            // Create fifth result we want to see
            // One work desc, one work order, no jscl.
            var ballCurbStopRepair = GetFactory<BallCurbStopRepairWorkDescriptionFactory>().Create();
            var workorder7 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = ballCurbStopRepair, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });

            var search = new TestSearchCompletedWorkOrdersWithJobSiteCheckLists();
            // DateCompleted does not matter for this test, but it has to be set because it's required.
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.MinValue,
                End = DateTime.MaxValue
            };
            var results = Repository.GetCompletedWorkOrderJobSiteCheckListCounts(search).ToList();
            Assert.AreEqual(5, search.Count, "Sanity");

            var result = results.Single(x => x.WorkDescription == waterMainBleeders.Description);
            Assert.AreEqual(waterMainBleeders.Description, result.WorkDescription);
            Assert.AreEqual(waterMainBleeders.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder1.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder1.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder1.Town.State.Abbreviation, result.State);
            Assert.AreEqual(1, result.WorkOrderCount);
            Assert.AreEqual(1, result.WorkOrdersWithJobSiteCheckListCount);

            result = results.Single(x => x.WorkDescription == changeBurstMeter.Description);
            Assert.AreEqual(changeBurstMeter.Description, result.WorkDescription);
            Assert.AreEqual(changeBurstMeter.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder2.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder2.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder2.Town.State.Abbreviation, result.State);
            Assert.AreEqual(2, result.WorkOrderCount);
            Assert.AreEqual(2, result.WorkOrdersWithJobSiteCheckListCount);

            result = results.Single(x => x.WorkDescription == checkNoWater.Description);
            Assert.AreEqual(checkNoWater.Description, result.WorkDescription);
            Assert.AreEqual(checkNoWater.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder4.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder4.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder4.Town.State.Abbreviation, result.State);
            Assert.AreEqual(2, result.WorkOrderCount);
            Assert.AreEqual(1, result.WorkOrdersWithJobSiteCheckListCount);

            result = results.Single(x => x.WorkDescription == curbBoxRepair.Description);
            Assert.AreEqual(curbBoxRepair.Description, result.WorkDescription);
            Assert.AreEqual(curbBoxRepair.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder6.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder6.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder6.Town.State.Abbreviation, result.State);
            Assert.AreEqual(1, result.WorkOrderCount);
            Assert.AreEqual(1, result.WorkOrdersWithJobSiteCheckListCount);

            result = results.Single(x => x.WorkDescription == ballCurbStopRepair.Description);
            Assert.AreEqual(ballCurbStopRepair.Description, result.WorkDescription);
            Assert.AreEqual(ballCurbStopRepair.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder7.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder7.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder7.Town.State.Abbreviation, result.State);
            Assert.AreEqual(1, result.WorkOrderCount);
            Assert.AreEqual(0, result.WorkOrdersWithJobSiteCheckListCount);
        }

        [TestMethod]
        public void TestGetCompletedWOrkOrderJobSiteCheckListCountsCorrectlyFilterByHasExcavation()
        {
            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {State = state});
            var waterMainBleeders = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = waterMainBleeders, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });
            var jscl1 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder1, HasExcavation = true});
            var workorder3 = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = waterMainBleeders, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter
            });
            var jscl2 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder3, HasExcavation = true});

            jscl1.HasExcavation = null;

            Session.Flush();
            Session.Clear();
            Session.Save(jscl1);

            var search = new TestSearchCompletedWorkOrdersWithJobSiteCheckLists();
            // DateCompleted does not matter for this test, but it has to be set because it's required.
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.MinValue,
                End = DateTime.MaxValue
            };
            var results = Repository.GetCompletedWorkOrderJobSiteCheckListCounts(search).ToList();
            Assert.AreEqual(1, search.Count);
            var result = results.Single(x => x.OperatingCenterId == operatingCenter.Id);
            Assert.AreEqual(1, result.WorkOrdersWithJobSiteCheckListCount);
            Assert.AreEqual(1, result.WorkOrdersWithoutJobSiteCheckListCount);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderJobSiteCheckListCountsCorrectlyFiltersByDateCompleted()
        {
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>()
               .Create(new {WorkDescription = workDesc1, DateCompleted = DateTime.Today});
            var workorder2 = GetFactory<WorkOrderFactory>().Create(new {WorkDescription = workDesc1});
            var jscl1 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder1, HasExcavation = true});

            var search = new TestSearchCompletedWorkOrdersWithJobSiteCheckLists();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };
            var result = Repository.GetCompletedWorkOrderJobSiteCheckListCounts(search).Single();
            Assert.AreEqual(1, result.WorkOrderCount);
            Assert.AreEqual(1, result.WorkOrdersWithJobSiteCheckListCount);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderJobSiteCheckListCountsCorrectlyFiltersByOperatingCenters()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc3 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, OperatingCenter = opc1});
            var jscl1 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder1, HasExcavation = false});
            var workorder2 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, OperatingCenter = opc2});
            var jscl2 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder2, HasExcavation = false});
            var workorder3 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, OperatingCenter = opc3});
            var jscl3 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder3, HasExcavation = false});

            var search = new TestSearchCompletedWorkOrdersWithJobSiteCheckLists();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };
            search.OperatingCenter = new[] {opc1.Id, opc2.Id};
            var results = Repository.GetCompletedWorkOrderJobSiteCheckListCounts(search).ToList();
            Assert.AreEqual(2, results.Count(), "Sanity");

            var opc1Result = results.Single(x => x.OperatingCenter == opc1.OperatingCenterCode);
            Assert.AreEqual(1, opc1Result.WorkOrderCount);
            Assert.AreEqual(1, opc1Result.WorkOrdersWithJobSiteCheckListCount);
            var opc2Result = results.Single(x => x.OperatingCenter == opc2.OperatingCenterCode);
            Assert.AreEqual(1, opc2Result.WorkOrderCount);
            Assert.AreEqual(1, opc2Result.WorkOrdersWithJobSiteCheckListCount);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderJobSiteCheckListCountsCorrectlyFiltersByState()
        {
            var state1 = GetFactory<StateFactory>().Create(new {Abbreviation = "XX"});
            var county1 = GetFactory<CountyFactory>().Create(new {State = state1});
            var town1 = GetFactory<TownFactory>().Create(new {County = county1});
            var state2 = GetFactory<StateFactory>().Create(new {Abbreviation = "ZZ"});
            var county2 = GetFactory<CountyFactory>().Create(new {State = state2});
            var town2 = GetFactory<TownFactory>().Create(new {County = county2});
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, Town = town1});
            var jscl1 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder1, HasExcavation = false});
            var workorder2 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, Town = town2});
            var jscl2 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder2, HasExcavation = false});

            var search = new TestSearchCompletedWorkOrdersWithJobSiteCheckLists();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };
            search.State = state1.Id;
            var results = Repository.GetCompletedWorkOrderJobSiteCheckListCounts(search).ToList();
            Assert.AreEqual(1, results.Count(), "Sanity");

            var opc1Result = results.Single(x => x.State == state1.Abbreviation);
            Assert.AreEqual(1, opc1Result.WorkOrderCount);
            Assert.AreEqual(1, opc1Result.WorkOrdersWithJobSiteCheckListCount);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderJobSiteCheckListCountsCorrectlyFiltersByWorkDescription()
        {
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>()
               .Create(new {WorkDescription = workDesc1, DateCompleted = DateTime.Today});
            var jscl1 = GetFactory<JobSiteCheckListFactory>()
               .Create(new {MapCallWorkOrder = workorder1, HasExcavation = false});

            // Test that we can find the workorder when we're specifically filtering for it.
            var search = new TestSearchCompletedWorkOrdersWithJobSiteCheckLists();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };
            search.WorkDescription = new[] {workDesc1.Id};

            var result = Repository.GetCompletedWorkOrderJobSiteCheckListCounts(search).Single();
            Assert.AreEqual(workDesc1.Description, result.WorkDescription);

            // Test that we do not find that same workorder when it's not included in the filter.
            search.WorkDescription = new[] {0};
            Assert.IsFalse(Repository.GetCompletedWorkOrderJobSiteCheckListCounts(search).Any());
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderJobSiteCheckListCountsCorrectlyFiltersByIsAssignedContractor()
        {
            // WorkDescriptions are only needed for this test so we can actually check the result since the work orders
            // themselves are not actually in the results.
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workDesc2 = GetFactory<ChangeBurstMeterWorkDescriptionFactory>().Create();
            var workOrderNotAssignedContractor = GetFactory<WorkOrderFactory>()
               .Create(new {WorkDescription = workDesc1, DateCompleted = DateTime.Today});
            Assert.IsNull(workOrderNotAssignedContractor.AssignedContractor, "Sanity");
            GetFactory<JobSiteCheckListFactory>().Create(new
                {MapCallWorkOrder = workOrderNotAssignedContractor, HasExcavation = false});
            var workOrderAssignedContractor = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = workDesc2, DateCompleted = DateTime.Today,
                AssignedContractor = typeof(ContractorFactory)
            });
            GetFactory<JobSiteCheckListFactory>().Create(new
                {MapCallWorkOrder = workOrderAssignedContractor, HasExcavation = false});

            // Test that we can find the workorder when we're specifically filtering for it.
            var search = new TestSearchCompletedWorkOrdersWithJobSiteCheckLists();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };

            // Test if IsAssignedContractor filter is ignored when the value is null
            search.IsAssignedContractor = null;
            var result = Repository.GetCompletedWorkOrderJobSiteCheckListCounts(search).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(x => x.WorkDescriptionId == workDesc1.Id));
            Assert.IsTrue(result.Any(x => x.WorkDescriptionId == workDesc2.Id));

            // Test that we only return results for work orders without assigned contractors
            search.IsAssignedContractor = false;
            Assert.AreEqual(workDesc1.Id,
                Repository.GetCompletedWorkOrderJobSiteCheckListCounts(search).Single().WorkDescriptionId);

            // Test that we only return results for work orders that have assigned contractors
            search.IsAssignedContractor = true;
            Assert.AreEqual(workDesc2.Id,
                Repository.GetCompletedWorkOrderJobSiteCheckListCounts(search).Single().WorkDescriptionId);
        }

        #endregion

        [TestMethod]
        public void TestGetCompletedWorkOrderMarkoutCountsCorrectlyFiltersByDateCompleted()
        {
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>()
               .Create(new {WorkDescription = workDesc1, DateCompleted = DateTime.Today});
            var workorder2 = GetFactory<WorkOrderFactory>().Create(new {WorkDescription = workDesc1});
            var mrk1 = GetFactory<MarkoutFactory>().Create(new {WorkOrder = workorder1});

            var search = new TestSearchCompletedWorkOrdersWithPreJobSafetyBriefs();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };
            var result = Repository.GetCompletedWorkOrderPreJobSafetyBriefCounts(search).Single();
            Assert.AreEqual(1, result.WorkOrderCount);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderMarkoutCountsCorrectlyFiltersByIsAssignedContractor()
        {
            // WorkDescriptions are only needed for this test so we can actually check the result since the work orders
            // themselves are not actually in the results.
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workDesc2 = GetFactory<ChangeBurstMeterWorkDescriptionFactory>().Create();
            var workOrderNotAssignedContractor = GetFactory<WorkOrderFactory>()
               .Create(new {WorkDescription = workDesc1, DateCompleted = DateTime.Today});
            Assert.IsNull(workOrderNotAssignedContractor.AssignedContractor, "Sanity");
            GetFactory<MarkoutFactory>().Create(new {WorkOrder = workOrderNotAssignedContractor});
            var workOrderAssignedContractor = GetFactory<WorkOrderFactory>().Create(new {
                WorkDescription = workDesc2, DateCompleted = DateTime.Today,
                AssignedContractor = typeof(ContractorFactory)
            });
            GetFactory<MarkoutFactory>().Create(new {WorkOrder = workOrderAssignedContractor});

            // Test that we can find the workorder when we're specifically filtering for it.
            var search = new TestSearchCompletedWorkOrdersWithMarkout();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };

            // Test if IsAssignedContractor filter is ignored when the value is null
            search.IsAssignedContractor = null;
            var result = Repository.GetCompletedWorkOrderMarkoutCounts(search).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(x => x.WorkDescriptionId == workDesc1.Id));
            Assert.IsTrue(result.Any(x => x.WorkDescriptionId == workDesc2.Id));

            // Test that we only return results for work orders without assigned contractors
            search.IsAssignedContractor = false;
            Assert.AreEqual(workDesc1.Id,
                Repository.GetCompletedWorkOrderMarkoutCounts(search).Single().WorkDescriptionId);

            // Test that we only return results for work orders that have assigned contractors
            search.IsAssignedContractor = true;
            Assert.AreEqual(workDesc2.Id,
                Repository.GetCompletedWorkOrderMarkoutCounts(search).Single().WorkDescriptionId);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderMarkoutCountsCorrectlyFiltersByOperatingCenters()
        {
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc3 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, OperatingCenter = opc1});
            var mrk1 = GetFactory<MarkoutFactory>().Create(new {WorkOrder = workorder1});
            var workorder2 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, OperatingCenter = opc2});
            var mrk2 = GetFactory<MarkoutFactory>().Create(new {WorkOrder = workorder2});
            var workorder3 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, OperatingCenter = opc3});
            var mrk3 = GetFactory<MarkoutFactory>().Create(new {WorkOrder = workorder3});

            var search = new TestSearchCompletedWorkOrdersWithMarkout();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };
            search.OperatingCenter = new[] {opc1.Id, opc2.Id};
            var results = Repository.GetCompletedWorkOrderMarkoutCounts(search).ToList();
            Assert.AreEqual(2, results.Count(), "Sanity");

            var opc1Result = results.Single(x => x.OperatingCenter == opc1.OperatingCenterCode);
            Assert.AreEqual(1, opc1Result.WorkOrderCount);
            Assert.AreEqual(1, opc1Result.MarkoutNoneCount);
            var opc2Result = results.Single(x => x.OperatingCenter == opc2.OperatingCenterCode);
            Assert.AreEqual(1, opc2Result.WorkOrderCount);
            Assert.AreEqual(1, opc2Result.MarkoutNoneCount);
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderMarkoutCountsCorrectlyFiltersByWorkDescription()
        {
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>()
               .Create(new {WorkDescription = workDesc1, DateCompleted = DateTime.Today});
            var mrk1 = GetFactory<MarkoutFactory>().Create(new {WorkOrder = workorder1});

            // Test that we can find the workorder when we're specifically filtering for it.
            var search = new TestSearchCompletedWorkOrdersWithMarkout();
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.Today,
                End = DateTime.Today
            };
            search.WorkDescription = new[] {workDesc1.Id};

            var result = Repository.GetCompletedWorkOrderMarkoutCounts(search).Single();
            Assert.AreEqual(workDesc1.Description, result.WorkDescription);

            // Test that we do not find that same workorder when it's not included in the filter.
            search.WorkDescription = new[] {0};
            Assert.IsFalse(Repository.GetCompletedWorkOrderMarkoutCounts(search).Any());
        }

        [TestMethod]
        public void TestGetCompletedWorkOrderMarkoutCountsWorksAsExpected()
        {
            var state = GetEntityFactory<State>().Create();
            var operatingCenter = GetEntityFactory<OperatingCenter>().Create(new {State = state});

            // Create first result we want to see. 
            // One work description, one work order, one markout
            var workDesc1 = GetFactory<WaterMainBleedersWorkDescriptionFactory>().Create();
            var workorder1 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc1, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var mkr1 = GetFactory<MarkoutFactory>().Create(new {WorkOrder = workorder1});

            // Create second result we want to see
            // One work desc, two work orders with markouts
            var workDesc2 = GetFactory<ChangeBurstMeterWorkDescriptionFactory>().Create();
            var workorder2 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc2, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var workorder3 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc2, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var mrk2 = GetFactory<MarkoutFactory>().Create(new {WorkOrder = workorder2});
            var mrk3 = GetFactory<MarkoutFactory>().Create(new {WorkOrder = workorder3});

            // Create third result we want to see
            // One work desc, two work orders, one markout
            var workDesc3 = GetFactory<CheckNoWaterWorkDescriptionFactory>().Create();
            var workorder4 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc3, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var workorder5 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc3, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var mrk4 = GetFactory<MarkoutFactory>().Create(new {WorkOrder = workorder4});

            // Create fourth result we want to see
            // One work desc, one work order, two markouts. This should only return 1 for the WorkOrdersWithMarkoutCount instead of 2.
            var workDesc4 = GetFactory<CurbBoxRepairWorkDescriptionFactory>().Create();
            var workorder6 = GetFactory<WorkOrderFactory>().Create(new
                {WorkDescription = workDesc4, DateCompleted = DateTime.Today, OperatingCenter = operatingCenter});
            var mrk5 = GetFactory<MarkoutFactory>().Create(new {WorkOrder = workorder6});
            var mrk6 = GetFactory<MarkoutFactory>().Create(new {WorkOrder = workorder6});

            var search = new TestSearchCompletedWorkOrdersWithMarkout();
            // DateCompleted does not matter for this test, but it has to be set because it's required.
            search.DateCompleted = new RequiredDateRange {
                Operator = RangeOperator.Between,
                Start = DateTime.MinValue,
                End = DateTime.MaxValue
            };
            var results = Repository.GetCompletedWorkOrderMarkoutCounts(search).ToList();
            Assert.AreEqual(4, search.Count, "Sanity");

            //1
            var result = results.Single(x => x.WorkDescription == workDesc1.Description);
            Assert.AreEqual(workDesc1.Description, result.WorkDescription);
            Assert.AreEqual(workDesc1.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder1.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder1.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder1.Town.State.Abbreviation, result.State);
            Assert.AreEqual(1, result.WorkOrderCount);
            Assert.AreEqual(1, result.MarkoutNoneCount);
            //2
            result = results.Single(x => x.WorkDescription == workDesc2.Description);
            Assert.AreEqual(workDesc2.Description, result.WorkDescription);
            Assert.AreEqual(workDesc2.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder2.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder2.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder2.Town.State.Abbreviation, result.State);
            Assert.AreEqual(2, result.WorkOrderCount);
            Assert.AreEqual(2, result.MarkoutNoneCount);
            //3 - 1 wo with markout
            result = results.Single(x => x.WorkDescription == workDesc3.Description);
            Assert.AreEqual(workDesc3.Description, result.WorkDescription);
            Assert.AreEqual(workDesc3.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder4.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder4.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder4.Town.State.Abbreviation, result.State);
            Assert.AreEqual(2, result.WorkOrderCount);
            Assert.AreEqual(2, result.MarkoutNoneCount);
            Assert.AreEqual(0, result.MarkoutRoutineCount);
            Assert.AreEqual(0, result.MarkoutEmergencyCount);
            //4 - 1 wo with markout
            result = results.Single(x => x.WorkDescription == workDesc4.Description);
            Assert.AreEqual(workDesc4.Description, result.WorkDescription);
            Assert.AreEqual(workDesc4.Id, result.WorkDescriptionId);
            Assert.AreEqual(workorder6.OperatingCenter.OperatingCenterCode, result.OperatingCenter);
            Assert.AreEqual(workorder6.OperatingCenter.Id, result.OperatingCenterId);
            Assert.AreEqual(workorder6.Town.State.Abbreviation, result.State);
            Assert.AreEqual(1, result.WorkOrderCount);
            Assert.AreEqual(1, result.MarkoutNoneCount);
        }

        [TestMethod]
        public void TestGetMainBreakRepairsForGIS()
        {
            var now = DateTime.Now;
            var coordinate = GetEntityFactory<Coordinate>().Create(new { Latitude = -10m, Longitude = 10m });
            var coordinate1 = GetEntityFactory<Coordinate>().Create(new { Latitude = -15m, Longitude = 15m });
            var wo0 = GetEntityFactory<WorkOrder>()
               .Create(new {
                    WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory),
                    DateCompleted = now.AddDays(-1),
                    Latitude = coordinate.Latitude,
                    Longitude = coordinate.Longitude,
                    CreatedAt = now.AddDays(-5)
                });
            var wo1 = GetEntityFactory<WorkOrder>()
               .Create(new {
                    WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory),
                    DateCompleted = now.AddDays(-1),
                    Latitude = coordinate1.Latitude,
                    Longitude = coordinate1.Longitude,
                    CreatedAt = now.AddDays(-7)
                });
            var wo2 = GetEntityFactory<WorkOrder>()
               .Create(new {
                    WorkDescription = typeof(WaterMainBreakRepairWorkDescriptionFactory),
                    DateCompleted = now.AddDays(-1),
                    Latitude = coordinate1.Latitude,
                    Longitude = coordinate1.Longitude,
                    CreatedAt = now.AddDays(-7),
                    DateStarted = now.AddHours(-12)
               });
            wo2.DateCompleted = null;
            var search = new TestSearchMainBreakRepairsForGIS {
                DateCompleted = new DateRange {
                    Start = now.AddDays(-2),
                    End = now,
                    Operator = RangeOperator.Between
                },
                RecentOrders = true
            };
            var results = Repository.GetMainBreakRepairsForGIS(search);

            Assert.AreEqual(3, results.Count());
            Assert.IsTrue(results.Contains(wo0));
            Assert.IsTrue(results.Contains(wo1));
            Assert.IsTrue(results.Contains(wo2));
        }

        [TestMethod]
        public void TestGetByServiceIdReturnsByServiceId()
        {
            var service1 = GetFactory<ServiceFactory>().Create();
            var wo1 = GetFactory<WorkOrderFactory>().CreateList(3, new { Service = service1 });
            var wo2 = GetFactory<WorkOrderFactory>().CreateList(4);

            var result = Repository.GetByServiceId(service1.Id);

            Assert.AreEqual(3, result.Count());
        }

        [TestMethod]
        public void TestGetWaterLossReportReturnsBySearch()
        {
            var wo1 = GetFactory<WorkOrderFactory>().Create(new
            {
                DateCompleted = DateTime.Today
            });

            var search = new TestSearchWaterLoss
            {
                Date = new RequiredDateRange
                {
                    Operator = RangeOperator.Between,
                    Start = DateTime.Today,
                    End = DateTime.Today
                }
            };

            Repository.GetWaterLossReport(search);

            Assert.AreEqual(1, search.Results.Count());
        }

        [TestMethod]
        public void TestGetWaterLossReportReturnsNothingWhenOutsideSearchRange()
        {
            var wo1 = GetFactory<WorkOrderFactory>().Create(new
            {
                DateCompleted = DateTime.Today.AddDays(-1)
            });

            var search = new TestSearchWaterLoss
            {
                Date = new RequiredDateRange
                {
                    Operator = RangeOperator.Between,
                    Start = DateTime.Today,
                    End = DateTime.Today
                }
            };

            Repository.GetWaterLossReport(search);

            Assert.AreEqual(0, search.Results.Count());
        }

        #region Test Classes

        private class TestSearchWorkOrderPrePlanning : SearchSet<WorkOrder> {}

        private class TestSearchWorkOrderPlanning : SearchSet<WorkOrder> {}

        private class TestSearchWorkOrderScheduling : SearchSet<WorkOrder> {}

        private class TestSearchMainBreakServiceLine : SearchSet<MainBreakReportItem>, ISearchMainBreakReport
        {
            public int? State { get; set; }
            public int[] OperatingCenter { get; set; }
            public int? Year { get; set; }
            public bool? IsContractedOperations { get; set; }
        }

        private class TestSearchCompletedWorkOrdersWithPreJobSafetyBriefs :
            SearchSet<CompletedWorkOrderWithPreJobSafetyBriefReportItem>,
            ISearchCompletedWorkOrdersWithPreJobSafetyBriefs
        {
            public int? State { get; set; }
            public int[] OperatingCenter { get; set; }
            public int[] WorkDescription { get; set; }
            public RequiredDateRange DateCompleted { get; set; }
            public bool? IsAssignedContractor { get; set; }
        }

        private class TestSearchCompletedWorkOrdersWithJobSiteCheckLists :
            SearchSet<CompletedWorkOrderWithJobSiteCheckListReportItem>, ISearchCompletedWorkOrdersWithJobSiteCheckLists
        {
            public int? State { get; set; }
            public int[] OperatingCenter { get; set; }
            public int[] WorkDescription { get; set; }
            public RequiredDateRange DateCompleted { get; set; }
            public bool? IsAssignedContractor { get; set; }
        }

        private class TestSearchCompletedWorkOrdersWithMarkout : SearchSet<CompletedWorkOrderWithMarkoutReportItem>,
            ISearchCompletedWorkOrdersWithMarkout
        {
            public int? State { get; set; }
            public int[] OperatingCenter { get; set; }
            public int[] WorkDescription { get; set; }
            public RequiredDateRange DateCompleted { get; set; }
            public bool? IsAssignedContractor { get; set; }
        }

        private class TestSearchMainBreakRepairsForGIS : SearchSet<WorkOrder>, ISearchMainBreakRepairsForGIS
        {
            [Search(CanMap = false)]
            public DateRange DateCompleted { get; set; }
            [Search(CanMap = false)]
            public bool? RecentOrders { get; set; }
            [Search(CanMap = false)]
            public DateRange DateStarted { get; set; }
        }

        private class TestSearchWaterLoss : SearchSet<WaterLossSearchResultViewModel>, ISearchWaterLoss
        {
            public RequiredDateRange Date { get; set; }
            public int[] OperatingCenter { get; set; }
        }

        #endregion
    }
}
