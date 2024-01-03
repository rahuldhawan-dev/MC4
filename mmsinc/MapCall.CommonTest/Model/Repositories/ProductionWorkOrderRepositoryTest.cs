using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Authentication;
using Moq;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class ProductionWorkOrderRepositoryTest : MapCallMvcSecuredRepositoryTestBase<ProductionWorkOrder,
        ProductionWorkOrderRepository, User>
    {
        private DateTime _now;

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use(new TestDateTimeProvider(_now = DateTime.Now));
            i.For<IProductionWorkOrderRepository>().Use<ProductionWorkOrderRepository>();
        }

        #endregion

        #region Tests

        #region GetBy

        [TestMethod]
        public void TestGetByFacilityIdForLockoutFormsReturnsByFacilityIdOnly()
        {
            var facilities = GetEntityFactory<Facility>().CreateList(2);
            var badOrder = GetEntityFactory<ProductionWorkOrder>().Create(new {Facility = facilities[0]});
            var goodOrder = GetEntityFactory<ProductionWorkOrder>().Create(new {Facility = facilities[1]});

            var results = Repository.GetByFacilityIdForLockoutForms(facilities[1].Id);

            Assert.AreEqual(1, results.Count());
            Assert.AreEqual(goodOrder.Id, results.First().Id);
        }

        #endregion

        #region GetPerformanceReport

        [TestMethod]
        public void TestGetPerformanceReportByState()
        {
            var state1 = GetEntityFactory<State>().Create();
            var state2 = GetEntityFactory<State>().Create();

            var state1opCenter1 = GetEntityFactory<OperatingCenter>().Create(new {State = state1});
            var state2opCenter1 = GetEntityFactory<OperatingCenter>().Create(new {State = state2});

            var orderTypes = GetFactory<OrderTypeFactory>().CreateAll();
            var productionWorkDescription1 = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypes[0] });
            var productionWorkDescription4 = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypes[4] });

            var withState1 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = state1opCenter1,
                ProductionWorkDescription = productionWorkDescription1,
                DateReceived = _now
            });
            var withState2 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = state2opCenter1,
                ProductionWorkDescription = productionWorkDescription4,
                DateReceived = _now
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now}
            };

            Repository.GetPerformanceReport(search);

            Assert.IsTrue(search.Results.Any(r => r.State == state1.Abbreviation));
            Assert.IsTrue(search.Results.Any(r => r.State == state2.Abbreviation));

            Assert.IsTrue(search.Results.All(r => string.IsNullOrWhiteSpace(r.OperatingCenter)));
            Assert.IsTrue(search.Results.All(r => string.IsNullOrWhiteSpace(r.PlanningPlant)));
            Assert.IsTrue(search.Results.All(r => string.IsNullOrWhiteSpace(r.Facility)));

            Assert.IsTrue(search.Results.All(r => r.NumberCreated == 1));
        }

        [TestMethod]
        public void TestGetPerformanceReportByOperatingCenter()
        {
            var state1 = GetEntityFactory<State>().Create();
            var state2 = GetEntityFactory<State>().Create();

            var state1opCenter1 = GetEntityFactory<OperatingCenter>().Create(new {State = state1});
            var state2opCenter1 = GetEntityFactory<OperatingCenter>().Create(new {State = state2});

            var orderTypes = GetFactory<OrderTypeFactory>().CreateAll();
            var productionWorkDescription1 = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypes[0] });
            var productionWorkDescription4 = GetEntityFactory<ProductionWorkDescription>().Create(new { OrderType = orderTypes[4] });

            var withState1 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = state1opCenter1,
                ProductionWorkDescription = productionWorkDescription1,
                DateReceived = _now
            });
            var withState2 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = state2opCenter1,
                ProductionWorkDescription = productionWorkDescription4,
                DateReceived = _now
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now},
                State = new[] {state1.Id, state2.Id}
            };

            Repository.GetPerformanceReport(search);

            Assert.IsTrue(search.Results.Any(r => r.State == state1.Abbreviation));
            Assert.IsTrue(search.Results.Any(r => r.State == state2.Abbreviation));

            Assert.IsTrue(search.Results.Any(r => r.OperatingCenter == state1opCenter1.Description));
            Assert.IsTrue(search.Results.Any(r => r.OperatingCenter == state2opCenter1.Description));

            Assert.IsTrue(search.Results.All(r => string.IsNullOrWhiteSpace(r.PlanningPlant)));
            Assert.IsTrue(search.Results.All(r => string.IsNullOrWhiteSpace(r.Facility)));

            Assert.IsTrue(search.Results.All(r => r.NumberCreated == 1));
        }

        [TestMethod]
        public void TestGetPerformanceReportByPlanningPlantFromOperatingCenter()
        {
            var state = GetEntityFactory<State>().Create();

            var opCenter = GetEntityFactory<OperatingCenter>().Create(new {State = state});

            var pp1 = GetEntityFactory<PlanningPlant>().Create(new {OperatingCenter = opCenter});
            var pp2 = GetEntityFactory<PlanningPlant>().Create(new {OperatingCenter = opCenter});

            var withPP1 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opCenter,
                PlanningPlant = pp1,
                DateReceived = _now
            });
            var withPP2 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opCenter,
                PlanningPlant = pp2,
                DateReceived = _now
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now},
                State = new[] {state.Id},
                OperatingCenter = new[] {opCenter.Id}
            };

            Repository.GetPerformanceReport(search);

            Assert.IsTrue(search.Results.All(r => r.State == state.Abbreviation));
            Assert.IsTrue(search.Results.All(r => r.OperatingCenter == opCenter.Description));

            Assert.IsTrue(search.Results.Any(r => r.PlanningPlant == $"{pp1.Code} - {pp1.Description}"));
            Assert.IsTrue(search.Results.Any(r => r.PlanningPlant == $"{pp2.Code} - {pp2.Description}"));

            Assert.IsTrue(search.Results.All(r => string.IsNullOrWhiteSpace(r.Facility)));

            Assert.IsTrue(search.Results.All(r => r.NumberCreated == 1));
        }

        [TestMethod]
        public void TestGetPerformanceReportByPlanningPlantFromPlanningPlant()
        {
            var state = GetEntityFactory<State>().Create();

            var opCenter = GetEntityFactory<OperatingCenter>().Create(new {State = state});

            var pp1 = GetEntityFactory<PlanningPlant>().Create(new {OperatingCenter = opCenter});
            var pp2 = GetEntityFactory<PlanningPlant>().Create(new {OperatingCenter = opCenter});

            var withPP1 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opCenter,
                PlanningPlant = pp1,
                DateReceived = _now
            });
            var withPP2 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opCenter,
                PlanningPlant = pp2,
                DateReceived = _now
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now},
                State = new[] {state.Id},
                OperatingCenter = new[] {opCenter.Id},
                PlanningPlant = new[] {pp1.Id}
            };

            Repository.GetPerformanceReport(search);

            Assert.IsTrue(search.Results.All(r => r.State == state.Abbreviation));
            Assert.IsTrue(search.Results.All(r => r.OperatingCenter == opCenter.Description));

            Assert.IsTrue(search.Results.Any(r => r.PlanningPlant == $"{pp1.Code} - {pp1.Description}"));

            Assert.IsFalse(search.Results.Any(r => r.PlanningPlant == $"{pp2.Code} - {pp2.Description}"));

            Assert.IsTrue(search.Results.All(r => string.IsNullOrWhiteSpace(r.Facility)));

            Assert.IsTrue(search.Results.All(r => r.NumberCreated == 1));
        }

        [TestMethod]
        public void TestGetPerformanceReportByFacility()
        {
            var state = GetEntityFactory<State>().Create();

            var opCenter = GetEntityFactory<OperatingCenter>().Create(new {State = state});

            var pp = GetEntityFactory<PlanningPlant>().Create(new {OperatingCenter = opCenter});

            var facility1 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opCenter, PlanningPlant = pp});
            var facility2 = GetEntityFactory<Facility>().Create(new {OperatingCenter = opCenter, PlanningPlant = pp});

            var withFacility1 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opCenter,
                PlanningPlant = pp,
                Facility = facility1,
                DateReceived = _now
            });
            var withFacility2 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opCenter,
                PlanningPlant = pp,
                Facility = facility2,
                DateReceived = _now
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now},
                State = new[] {state.Id},
                OperatingCenter = new[] {opCenter.Id},
                PlanningPlant = new[] {pp.Id},
                Facility = new[] {facility1.Id, facility2.Id}
            };

            Repository.GetPerformanceReport(search);

            Assert.IsTrue(search.Results.All(r => r.State == state.Abbreviation));
            Assert.IsTrue(search.Results.All(r => r.OperatingCenter == opCenter.Description));

            Assert.IsTrue(search.Results.All(r => r.PlanningPlant == $"{pp.Code} - {pp.Description}"));

            Assert.IsTrue(search.Results.Any(r => r.Facility == facility1.FacilityName));
            Assert.IsTrue(search.Results.Any(r => r.Facility == facility2.FacilityName));

            Assert.IsTrue(search.Results.All(r => r.NumberCreated == 1));
        }

        [TestMethod]
        public void TestGetPerformanceReportReturnsExpectedUnscheduledOrderCount()
        {
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now}
            };

            Repository.GetPerformanceReport(search);
            var result = search.Results.Single();

            Assert.AreEqual(1, result.NumberCreated);
            Assert.AreEqual(1, result.NumberUnscheduled);
            Assert.AreEqual(0, result.NumberScheduled);
            Assert.AreEqual(0, result.NumberIncomplete);
            Assert.AreEqual(0, result.NumberCanceled);
            Assert.AreEqual(0, result.NumberCompleted);
            Assert.AreEqual(0, result.NumberNotApproved);
        }

        [TestMethod]
        public void TestGetPerformanceReportReturnsExpectedScheduledOrderCount()
        {
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
            });

            var empAss = GetEntityFactory<EmployeeAssignment>().Create(new {
                ProductionWorkOrder = order
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now}
            };

            Repository.GetPerformanceReport(search);
            var result = search.Results.Single();

            Assert.AreEqual(1, result.NumberCreated);
            Assert.AreEqual(0, result.NumberUnscheduled);
            Assert.AreEqual(1, result.NumberScheduled);
            Assert.AreEqual(0, result.NumberIncomplete);
            Assert.AreEqual(0, result.NumberCanceled);
            Assert.AreEqual(0, result.NumberCompleted);
            Assert.AreEqual(0, result.NumberNotApproved);
        }

        [TestMethod]
        public void TestGetPerformanceReportReturnsExpectedIncompleteOrderCountWhenWorkOrderHasUnendedAssignments()
        {
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
            });

            var empAss = GetEntityFactory<EmployeeAssignment>().Create(new {
                ProductionWorkOrder = order,
                DateStarted = _now
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now}
            };

            Repository.GetPerformanceReport(search);
            var result = search.Results.Single();

            Assert.AreEqual(1, result.NumberCreated);
            Assert.AreEqual(0, result.NumberUnscheduled);
            Assert.AreEqual(0, result.NumberScheduled);
            Assert.AreEqual(1, result.NumberIncomplete);
            Assert.AreEqual(0, result.NumberCanceled);
            Assert.AreEqual(0, result.NumberCompleted);
            Assert.AreEqual(0, result.NumberNotApproved);
        }

        [TestMethod]
        public void
            TestGetPerformanceReportReturnsExpectedIncompleteOrderCountWhenWorkOrderHasCompletedAssignmentsButTheWorkOrderIsNotComplete()
        {
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
            });

            var empAss = GetEntityFactory<EmployeeAssignment>().Create(new {
                ProductionWorkOrder = order,
                DateStarted = _now,
                DateEnded = _now
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now}
            };

            Repository.GetPerformanceReport(search);
            var result = search.Results.Single();

            Assert.AreEqual(1, result.NumberCreated);
            Assert.AreEqual(0, result.NumberUnscheduled);
            Assert.AreEqual(0, result.NumberScheduled);
            Assert.AreEqual(1, result.NumberIncomplete);
            Assert.AreEqual(0, result.NumberCanceled);
            Assert.AreEqual(0, result.NumberCompleted);
            Assert.AreEqual(0, result.NumberNotApproved);
        }

        [TestMethod]
        public void TestGetPerformanceReportReturnsExpectedCancelledOrderCount()
        {
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
                DateCancelled = _now
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now}
            };

            Repository.GetPerformanceReport(search);
            var result = search.Results.Single();

            Assert.AreEqual(1, result.NumberCreated);
            Assert.AreEqual(0, result.NumberUnscheduled);
            Assert.AreEqual(0, result.NumberScheduled);
            Assert.AreEqual(0, result.NumberIncomplete);
            Assert.AreEqual(1, result.NumberCanceled);
            Assert.AreEqual(0, result.NumberCompleted);
            Assert.AreEqual(0, result.NumberNotApproved);
        }

        [TestMethod]
        public void TestGetPerformanceReportReturnsExpectedCompletedOrderCount()
        {
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
                DateCompleted = _now
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now}
            };

            Repository.GetPerformanceReport(search);
            var result = search.Results.Single();

            Assert.AreEqual(1, result.NumberCreated);
            Assert.AreEqual(0, result.NumberUnscheduled);
            Assert.AreEqual(0, result.NumberScheduled);
            Assert.AreEqual(0, result.NumberIncomplete);
            Assert.AreEqual(0, result.NumberCanceled);
            Assert.AreEqual(1, result.NumberCompleted);
            Assert.AreEqual(1, result.NumberNotApproved,
                "NotApproved count should be the same as Completed in this situation.");
        }

        [TestMethod]
        public void TestGetPerformanceReportReturnsExpectedNotApprovedOrderCount()
        {
            var order = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
                DateCompleted = _now
            });

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now}
            };

            Repository.GetPerformanceReport(search);
            var result = search.Results.Single();

            Assert.AreEqual(1, result.NumberCreated);
            Assert.AreEqual(0, result.NumberUnscheduled);
            Assert.AreEqual(0, result.NumberScheduled);
            Assert.AreEqual(0, result.NumberIncomplete);
            Assert.AreEqual(0, result.NumberCanceled);
            Assert.AreEqual(1, result.NumberCompleted);
            Assert.AreEqual(1, result.NumberNotApproved);

            order.ApprovedOn = _now;
            Session.Save(order);
            Session.Flush();
            Repository.GetPerformanceReport(search);
            result = search.Results.Single();
            Assert.AreEqual(1, result.NumberCreated);
            Assert.AreEqual(0, result.NumberUnscheduled);
            Assert.AreEqual(0, result.NumberScheduled);
            Assert.AreEqual(0, result.NumberIncomplete);
            Assert.AreEqual(0, result.NumberCanceled);
            Assert.AreEqual(1, result.NumberCompleted);
            Assert.AreEqual(0, result.NumberNotApproved);
        }

        [TestMethod]
        public void
            TestGetPerformanceReportReturnsExpectedResultsWhenSearchingByOperatingCenterAndAWorkOrderDoesNotHaveAPlanningPlant()
        {
            // There was a bug with this query originally. When searching via operating center, the count queries
            // were not including the operating center, resulting in all the wrong counts when the order's
            // planning plant was null.

            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var differentOpc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var pwDesc = GetEntityFactory<ProductionWorkDescription>().Create();
            var orderWithPlanningPlant = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
                OperatingCenter = opc,
                ProductionWorkDescription = pwDesc,
                PlanningPlant = typeof(PlanningPlantFactory),
            });

            var orderWithoutPlanningPlant = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
                OperatingCenter = opc,
                ProductionWorkDescription = pwDesc,
                PlanningPlant = (PlanningPlant)null
            });
            var anotherOrderWithoutPlanningPlant = GetEntityFactory<ProductionWorkOrder>().Create(new {
                DateReceived = _now,
                OperatingCenter = differentOpc,
                ProductionWorkDescription = pwDesc,
                PlanningPlant = (PlanningPlant)null
            });

            Assert.IsNull(orderWithoutPlanningPlant.PlanningPlant, "Sanity");

            var search = new SearchProductionWorkOrderPerformance {
                DateReceived = new RequiredDateRange {Operator = RangeOperator.Equal, End = _now},
                OperatingCenter = new[] {opc.Id}
            };

            Repository.GetPerformanceReport(search);
            var result = search.Results.Single(x =>
                x.PlanningPlantId == null && x.OperatingCenterId.GetValueOrDefault() == opc.Id);

            Assert.AreEqual(1,
                search.Results
                      .Single(x => x.PlanningPlantId == null && x.OperatingCenterId.GetValueOrDefault() == opc.Id)
                      .NumberCreated);
        }

        #endregion

        #region SearchForExcel

        [TestMethod]
        public void TestSearchForExcelReturnsPreReqsAndExpectedRowCount()
        {
            var AnswerForPreReq = "False";
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new {OperatingCenter = opc});
            var pwo1 = GetEntityFactory<ProductionWorkOrder>().Create(new {OperatingCenter = opc1});
            var searchModel = new SearchProductionWorkOrder {
                OperatingCenter = opc.Id
            };

            var results = Repository.SearchForExcel(searchModel);

            Assert.AreEqual(1, results.Count());

            var result = results.SingleOrDefault(x => x.Id == pwo.Id.ToString());

            Assert.IsInstanceOfType(result, typeof(ProductionWorkOrderExcelItem));
            Assert.AreEqual(result.Id, pwo.Id.ToString());
            Assert.AreEqual(result.AirPermit, AnswerForPreReq);
            Assert.AreEqual(result.HasLockoutRequirement, AnswerForPreReq);
            Assert.AreEqual(result.JobSafetyChecklist, AnswerForPreReq);
            Assert.AreEqual(result.OperatingCenter, pwo.OperatingCenter.ToString());

            var badResult = results.SingleOrDefault(x => x.Id == pwo1.Id.ToString());
            Assert.IsNull(badResult);
        }

        [TestMethod]
        public void TestSearchForExcelReturnsLockoutFormsandDevicesAsExpected()
        {
            var AnswerForPreReq = "False";
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var locD = GetFactory<LockoutDeviceFactory>().Create();
            var locD1 = GetFactory<LockoutDeviceFactory>().Create();
            var locF = GetFactory<LockoutFormFactory>().Create();
            var locF1 = GetFactory<LockoutFormFactory>().Create();
            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new {OperatingCenter = opc});
            pwo.LockoutDevices.Add(locD);
            pwo.LockoutDevices.Add(locD1);
            pwo.LockoutForms.Add(locF);
            pwo.LockoutForms.Add(locF1);
            var locDDisplayString = pwo.LockoutDevices[0].LockoutDeviceColor.ToString() + " - " +
                                    pwo.LockoutDevices[0].SerialNumber.ToString() + " - " +
                                    pwo.LockoutDevices[0].Description.ToString();
            var locD1DisplayString = pwo.LockoutDevices[1].LockoutDeviceColor.ToString() + " - " +
                                     pwo.LockoutDevices[1].SerialNumber.ToString() + " - " +
                                     pwo.LockoutDevices[1].Description.ToString();
            var searchModel = new SearchProductionWorkOrder {
                OperatingCenter = opc.Id
            };

            var results = Repository.SearchForExcel(searchModel);

            Assert.AreEqual(1, results.Count());

            var result = results.SingleOrDefault(x => x.Id == pwo.Id.ToString());

            Assert.AreEqual(result.LockoutForms, locF.Id.ToString() + ", " + locF1.Id.ToString());
            Assert.AreEqual(result.LockoutDevices, locDDisplayString + ", " + locD1DisplayString);
        }

        [TestMethod]
        public void TestSearchForExcelReturnsLockoutFormCreated()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var lockoutForm = GetFactory<LockoutFormFactory>().Create();
            IList<LockoutForm> lockoutForms = new List<LockoutForm>();
            lockoutForms.Add(lockoutForm);

            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opc,
                LockoutForms = lockoutForms
            });

            var pwo2 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = opc,
            });

            var searchModel = new SearchProductionWorkOrder {
                OperatingCenter = opc.Id
            };

            var results = Repository.SearchForExcel(searchModel);

            Assert.AreEqual(2, results.Count());

            var result = results.SingleOrDefault(x => x.Id == pwo.Id.ToString());

            Assert.IsInstanceOfType(result, typeof(ProductionWorkOrderExcelItem));
            Assert.AreEqual(result.Id, pwo.Id.ToString());
            Assert.AreEqual(result.LockoutFormCreated, "True");

            result = results.Last();
            Assert.AreEqual(result.LockoutFormCreated, "False");
        }

        [TestMethod]
        public void TestSearchForExcelReturnsRedTagPermitCreated()
        {
            var oc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var pwo1 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = oc,
                RedTagPermit = GetFactory<RedTagPermitFactory>().Create()
            });

            var pwo2 = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = oc
            });

            var searchModel = new SearchProductionWorkOrder {
                OperatingCenter = oc.Id
            };

            var results = Repository.SearchForExcel(searchModel).ToList();

            Assert.AreEqual(2, results.Count);

            var resultItem1 = results.Single(x => x.Id == pwo1.Id.ToString());
            var resultItem2 = results.Single(x => x.Id == pwo2.Id.ToString());

            Assert.IsInstanceOfType(resultItem1, typeof(ProductionWorkOrderExcelItem));
            Assert.IsInstanceOfType(resultItem2, typeof(ProductionWorkOrderExcelItem));
            Assert.AreEqual(resultItem1.RedTagPermitCreated, "True");
            Assert.AreEqual(resultItem2.RedTagPermitCreated, "False");
        }

        [TestMethod]
        public void TestSearchForExcelReturnsRedTagPermitsAsExpected()
        {
            var oc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var rtp = GetFactory<RedTagPermitFactory>().Create();

            var pwo = GetEntityFactory<ProductionWorkOrder>().Create(new {
                OperatingCenter = oc,
                RedTagPermit = rtp
            });

            var searchModel = new SearchProductionWorkOrder {
                OperatingCenter = oc.Id
            };

            var results = Repository.SearchForExcel(searchModel).ToList();

            Assert.AreEqual(1, results.Count);

            var result = results.Single(x => x.Id == pwo.Id.ToString());

            Assert.AreEqual(rtp.Id.ToString(), result.RedTagPermit);
        }

        #endregion

        [TestMethod]
        public void TestSearchForProductionWorkOrderHistory()
        {
            var opc = GetFactory<OperatingCenterFactory>().Create();
            var eq = GetFactory<EquipmentFactory>().Create(new { OperatingCenter = opc });
            var pwo1 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            var pwo2 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            var pwoe1 = new ProductionWorkOrderEquipment {
                Equipment = eq, 
                ProductionWorkOrder = pwo1, 
                IsParent = true
            };
            var pwoe2 = new ProductionWorkOrderEquipment {
                Equipment = eq,
                ProductionWorkOrder = pwo2,
                IsParent = true
            }; 
            pwo1.Equipments.Add(pwoe1);
            pwo2.Equipments.Add(pwoe2);
            var pwo4 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc, Equipments = new HashSet<ProductionWorkOrderEquipment> { pwoe1, pwoe2 } });

            var search = new SearchProductionWorkOrderHistory {
                OperatingCenterId = opc.Id,
                Equipment = eq.Id,
                Results = new List<ProductionWorkOrder>{pwo4}
            };

            Repository.SearchForProductionWorkOrderHistory(search);

            Assert.AreEqual(2,
                search.Results.Count());
        }
        
        /// <summary>
        /// Test for bug MC-4400 - Users noticed that the ProductionWorkOrder search results did not match its
        /// corresponding Excel export. This test ensures that SearchForDistinct and
        /// SearchForExcel both return the same results
        /// </summary>
        [TestMethod]
        public void TestSearchForDistinctAndSearchForExcelReturnTheSameResults()
        {
            var opc = GetFactory<UniqueOperatingCenterFactory>().Create();
            var equipmentWithAllTheBools = GetFactory<EquipmentFactory>()
               .Create(new {
                    OperatingCenter = opc,
                    HasProcessSafetyManagement = true,
                    HasCompanyRequirement = true,
                    HasRegulatoryRequirement = true,
                    HasOshaRequirement = true,
                    OtherCompliance = true
                });
            var equipment = GetFactory<EquipmentFactory>().Create(new { OperatingCenter = opc, SAPEquipmentId = 1 });
            var pwo1 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            var pwo2 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            var pwo3 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            var pwo4 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            var pwo5 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            var pwo6 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            var pwo7 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            var pwo8 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            var pwo9 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            var pwo10 = GetFactory<ProductionWorkOrderFactory>().Create(new { OperatingCenter = opc });
            
            var pwoe1 = new ProductionWorkOrderEquipment {
                Equipment = equipmentWithAllTheBools, 
                ProductionWorkOrder = pwo1
            };
            
            var pwoe2 = new ProductionWorkOrderEquipment {
                Equipment = equipmentWithAllTheBools, 
                ProductionWorkOrder = pwo2
            };
            
            var pwoe3 = new ProductionWorkOrderEquipment {
                Equipment = equipmentWithAllTheBools, 
                ProductionWorkOrder = pwo3
            };
            
            var pwoe4 = new ProductionWorkOrderEquipment {
                Equipment = equipmentWithAllTheBools, 
                ProductionWorkOrder = pwo4
            };
            
            var pwoe5 = new ProductionWorkOrderEquipment {
                Equipment = equipmentWithAllTheBools, 
                ProductionWorkOrder = pwo5
            };
            
            var pwoe6 = new ProductionWorkOrderEquipment {
                Equipment = equipment,
                SAPEquipmentId = 1,
                ProductionWorkOrder = pwo6
            };
            
            var pwoe7 = new ProductionWorkOrderEquipment {
                Equipment = equipment,
                SAPEquipmentId = 1,
                ProductionWorkOrder = pwo7
            };
            
            var pwoe8 = new ProductionWorkOrderEquipment {
                Equipment = equipment,
                SAPEquipmentId = 1,
                ProductionWorkOrder = pwo8
            };
            
            var pwoe9 = new ProductionWorkOrderEquipment {
                Equipment = equipment,
                SAPEquipmentId = 1,
                ProductionWorkOrder = pwo9
            };
            
            var pwoe10 = new ProductionWorkOrderEquipment {
                Equipment = equipment,
                SAPEquipmentId = 1,
                ProductionWorkOrder = pwo10
            };
            
            pwo1.Equipments.Add(pwoe1);
            pwo2.Equipments.Add(pwoe2);
            pwo3.Equipments.Add(pwoe3);
            pwo4.Equipments.Add(pwoe4);
            pwo5.Equipments.Add(pwoe5);
            pwo6.Equipments.Add(pwoe6);
            pwo7.Equipments.Add(pwoe7);
            pwo8.Equipments.Add(pwoe8);
            pwo9.Equipments.Add(pwoe9);
            pwo10.Equipments.Add(pwoe10);

            Session.Flush();

            var searchAllTheBools = new SearchProductionWorkOrder {
                OperatingCenter = opc.Id,
                HasProcessSafetyManagement = true,
                HasCompanyRequirement = true,
                HasRegulatoryRequirement = true,
                HasOshaRequirement = true,
                OtherCompliance = true
            };

            var searchEquipment = new SearchProductionWorkOrder {
                OperatingCenter = opc.Id,
                Equipment = equipment.Id,
                SAPEquipmentId = 1
            };

            var searchForDistinctResults = Repository.SearchForDistinct(searchAllTheBools);
            var searchForExcelResults = Repository.SearchForExcel(searchAllTheBools);

            Assert.AreEqual(5, searchForDistinctResults.Count());
            Assert.AreEqual(5, searchForExcelResults.Count());
            
            var searchForDistinctResults2 = Repository.SearchForDistinct(searchEquipment);
            var searchForExcelResults2 = Repository.SearchForExcel(searchEquipment);
            
            Assert.AreEqual(5, searchForDistinctResults2.Count());
            Assert.AreEqual(5, searchForExcelResults2.Count());
        }

        [TestMethod]
        public void TestGetAutoCancelRoutineProductionWorkOrdersReturnsOnlyWorkOrdersAllowedToBeCancelled()
        {
            var pwoFactory = GetFactory<ProductionWorkOrderFactory>();
            var planFactory = GetFactory<MaintenancePlanFactory>();
            var assignmentFac = GetFactory<EmployeeAssignmentFactory>();
            var autoCancelPlans = planFactory.CreateList(3, new { HasACompletionRequirement = true });

            var pwos = autoCancelPlans
                      // These should be returned from GetAutoCancelRoutineProductionWorkOrders
                      .Select(plan => pwoFactory.Create(new { MaintenancePlan = plan }))

                      // These should not be returned from GetAutoCancelRoutineProductionWorkOrders since they were already cancelled
                      .Concat(autoCancelPlans.Select(plan => pwoFactory.Create(new { MaintenancePlan = plan, DateCancelled = DateTime.Today })))

                      // These should not be returned from GetAutoCancelRoutineProductionWorkOrders since they were already completed
                     .Concat(autoCancelPlans.Select(plan => pwoFactory.Create(new { MaintenancePlan = plan, DateCompleted = DateTime.Today })))

                     // These should not be returned from GetAutoCancelRoutineProductionWorkOrders since they have EmployeeAssignments that were already started
                     .Concat(autoCancelPlans.Select(plan => pwoFactory.Create(new { MaintenancePlan = plan, DateCompleted = DateTime.Today, EmployeeAssignments = assignmentFac.CreateSet(1, new { DateStarted = DateTime.Today }) })))

                      // These should not be returned from GetAutoCancelRoutineProductionWorkOrders since MaterialsApprovedOn isn't null
                     .Concat(autoCancelPlans.Select(plan => pwoFactory.Create(new { MaintenancePlan = plan, MaterialsApprovedOn = DateTime.Today })))
                     .ToList();

            var scheduledPlans = pwos.Select(x => new ScheduledMaintenancePlan(x.MaintenancePlan));

            Session.Flush();
            Session.Clear();

            var orderIdsToCancel = Repository.GetAutoCancelRoutineProductionWorkOrders(scheduledPlans).ToList();
            var ordersToCancel = Repository.FindManyByIds(orderIdsToCancel).Values.ToList();

            Assert.IsTrue(ordersToCancel.All(x => x.MaintenancePlan.HasACompletionRequirement
                                                  && x.MaterialsApprovedOn == null
                                                  && x.DateCancelled == null
                                                  && x.DateCompleted == null
                                                  && (x.EmployeeAssignments.Any() == false ||
                                                      x.EmployeeAssignments.All(y =>
                                                          y.DateStarted == null && 
                                                          y.DateEnded == null))));
        }

        [TestMethod]
        public void TestGetAutoCancelRoutineProductionWorkOrdersReturnsOnlyWorkOrdersFromAutoCancelScheduledPlans()
        {
            var pwoFactory = GetFactory<ProductionWorkOrderFactory>();
            var planFactory = GetFactory<MaintenancePlanFactory>();
            var userFac = GetFactory<AdminUserFactory>();
            var plans = planFactory.CreateList(2);
            var autoCancelPlans = planFactory.CreateList(3, new { HasACompletionRequirement = true });
            var badAutoCancelPlans = planFactory.CreateList(2, new { HasACompletionRequirement = true });
            var unscheduledPlans = planFactory.CreateList(5);
            var user = userFac.Create();
            
            var pwos = plans
                      .Select(plan => pwoFactory.Create(new { MaintenancePlan = plan }))
                      .Concat(autoCancelPlans.Select(plan => pwoFactory.Create(new { MaintenancePlan = plan })))
                      .Concat(badAutoCancelPlans.Select(plan => pwoFactory.Create(new { MaintenancePlan = plan, DateCancelled = DateTime.Today, CancelledBy = user })))
                      .ToList();

            pwos.AddRange(badAutoCancelPlans.Select(plan => pwoFactory.Create(new {
                MaintenancePlan = plan,
                DateCancelled = DateTime.Today,
                CancelledBy = user
            })));

            var scheduledPlans = pwos.Select(x => new ScheduledMaintenancePlan(x.MaintenancePlan));
            
            Session.Flush();
            Session.Clear();

            var orderIdsToCancel = Repository.GetAutoCancelRoutineProductionWorkOrders(scheduledPlans).ToList();

            var ordersToCancel = Repository.FindManyByIds(orderIdsToCancel).Values.ToList();

            Assert.AreEqual(ordersToCancel.Count, 3);

            // For every (good) AutoCancel plan, there must be a corresponding work order
            Assert.IsTrue(autoCancelPlans.All(plan => ordersToCancel.Any(pwo => pwo.MaintenancePlan.Id == plan.Id)));

            // Make sure of all the orders returned, none are already cancelled.
            Assert.IsTrue(ordersToCancel.All(x =>
                x.DateCancelled == null
                && x.CancelledBy == null
                && badAutoCancelPlans.All(plan => x.MaintenancePlan.Id != plan.Id)));
        }

        [TestMethod]
        public void TestCancelOrderSetsAppropriateFieldsOnPwoAndPersistsCancellation()
        {
            GetFactory<ProductionWorkOrderCancellationReasonFactory>().CreateAll();

            var pwo = GetFactory<ProductionWorkOrderFactory>().CreateList(1);
            
            Repository.CancelOrders(pwo.Select(x => x.Id));

            Session.Flush();
            Session.Clear();

            var actual = Repository.Find(pwo[0].Id);

            Assert.IsNotNull(actual.CancelledBy);
            Assert.IsNotNull(actual.CancellationReason);
            Assert.AreEqual(ProductionWorkOrderCancellationReason.Indices.ORDER_PAST_EXPIRATION_DATE, actual.CancellationReason.Id);
            Assert.IsNotNull(actual.DateCancelled);
            Assert.AreEqual(_now.Date, actual.DateCancelled);
        }

        [TestMethod]
        public void TestBuildRoutineProductionWorkOrdersGeneratesValidProductionWorkOrderObjects()
        {
            GetFactory<ProductionWorkOrderPriorityFactory>().CreateAll();
            var plans = GetFactory<MaintenancePlanFactory>().CreateList(1);
            var equipment = GetFactory<EquipmentFactory>().Create();
            var equipmentType = GetFactory<EquipmentTypeFactory>().Create();

            plans[0].ProductionWorkOrderFrequency = new ProductionWorkOrderFrequency { Id = 1 };
            equipment.MaintenancePlans.Add(new EquipmentMaintenancePlan { Equipment = equipment, MaintenancePlan = plans[0] });
            plans[0].Equipment.Add(equipment);
            plans[0].EquipmentTypes.Add(equipmentType);

            Session.Flush();

            var scheduledPlans = new List<ScheduledMaintenancePlan> {
                new ScheduledMaintenancePlan(plans[0])
            };

            var pwo = Repository.BuildRoutineProductionWorkOrdersFromScheduledPlans(scheduledPlans).ToList()[0];

            Assert.IsNotNull(pwo);
            Assert.IsNotNull(pwo.DateReceived);
            Assert.IsNotNull(pwo.BasicStart);
            Assert.IsNotNull(pwo.DueDate);
            Assert.IsNotNull(pwo.StartDate);
            Assert.AreEqual(pwo.StartDate, DateTime.Today);
            Assert.AreEqual(plans[0].EquipmentTypes.FirstOrDefault().Id, pwo.EquipmentType.Id);
            Assert.AreEqual(plans[0].Id, pwo.MaintenancePlan.Id);
            Assert.AreEqual(plans[0].OperatingCenter.Id, pwo.OperatingCenter.Id);
            Assert.AreEqual(plans[0].PlanningPlant.Id, pwo.PlanningPlant.Id);
            Assert.AreEqual(plans[0].WorkDescription.Id, pwo.ProductionWorkDescription.Id);
            Assert.AreEqual(plans[0].Facility.Id, pwo.Facility.Id);
            Assert.AreEqual(plans[0].LocalTaskDescription, pwo.LocalTaskDescription);
            Assert.AreEqual(plans[0].ProductionWorkOrderFrequency.GetFrequencyNextEndDate(pwo.DateReceived.Value), pwo.DateReceived.Value);
            Assert.IsNotNull(pwo.Equipments);
            Assert.IsTrue(pwo.Equipments.Count == 1);
            Assert.IsTrue(pwo.Equipments.All(x => plans[0].Equipment.Contains(x.Equipment)));
            Assert.AreEqual((int)ProductionWorkOrderPriority.Indices.ROUTINE, pwo.Priority.Id);
        }

        [TestMethod]
        public void TestSaveAllAndGetAssignmentsForNotificationsSavesAllAndReturnsTheEmployeeAssignments()
        {
            var planFactory = GetFactory<MaintenancePlanFactory>();
            var assignmentFac = GetFactory<EmployeeAssignmentFactory>();
            var employee = GetEntityFactory<Employee>().Create();
            var routinePriority = GetFactory<RoutineProductionWorkOrderPriorityFactory>().Create();

            var plans = planFactory.CreateList(3, new { HasACompletionRequirement = true });

            var pwos = plans.Select(plan => new ProductionWorkOrder {
                MaintenancePlan = plan,
                OperatingCenter = plan.OperatingCenter,
                PlanningPlant = plan.PlanningPlant,
                ProductionWorkDescription = plan.WorkDescription,
                LocalTaskDescription = plan.LocalTaskDescription,
                Facility = plan.Facility,
                Priority = routinePriority,
                BreakdownIndicator = true,
                StartDate = plan.Start,
                DateReceived = DateTime.Today,
                BasicStart = DateTime.Today,
                DueDate = DateTime.Today.AddDays(1)
            }).ToList();

            foreach (var pwo in pwos)
            {
                var assignment = assignmentFac.Build(new { ProductionWorkOrder = pwo, AssignedTo = employee });
                pwo.EmployeeAssignments.Add(assignment);
            }

            var assignments = Repository.SaveAllAndGetAssignmentsForNotifications(pwos).ToList();
            var expectedPwos = Repository.GetAll().ToList();
            
            Assert.AreEqual(expectedPwos.Count, 3); // Make sure the PWOs were saved
            Assert.AreEqual(assignments.Count, 3); // Make sure the EmployeeAssignments were saved and returned
            Assert.IsTrue(assignments.All(x => x.AssignedTo == employee));
        }

        #endregion

        #region Helper classes

        private class SearchProductionWorkOrderPerformance : SearchSet<ProductionWorkOrderPerformanceResultViewModel>,
            ISearchProductionWorkOrderPerformance
        {
            public int[] State { get; set; } = new int[0];
            public int[] OperatingCenter { get; set; } = new int[0];
            public int[] PlanningPlant { get; set; } = new int[0];
            public int[] Facility { get; set; } = new int[0];
            public RequiredDateRange DateReceived { get; set; }
            public int[] OrderType { get; set; } = new int[0];
            public string[] SelectedOrderTypes { get; set; }
            public string[] SelectedFacilities { get; set; }
            public string[] SelectedOperatingCenters { get; set; }
            public string[] SelectedPlanningPlants { get; set; }
            public string[] SelectedStates { get; set; }
        }

        private class SearchProductionWorkOrder : SearchSet<ProductionWorkOrder>, ISearchProductionWorkOrder
        {
            public int OperatingCenter { get; set; }
            public int? Equipment { get; set; }
            public int? SAPEquipmentId { get; set; }
            public bool? HasProcessSafetyManagement { get; set; }
            public bool? HasCompanyRequirement { get; set; }
            public bool? HasRegulatoryRequirement { get; set; }
            public bool? HasOshaRequirement { get; set; }
            public bool? OtherCompliance { get; set; }
        }

        private class SearchProductionWorkOrderHistory : SearchSet<ProductionWorkOrder>, ISearchProductionWorkOrderHistory
        {
            [Search(CanMap = false)]
            public int? OperatingCenterId { get; set; }
            [Search(CanMap = false)]
            public int? Equipment { get; set; }
        }

        #endregion
    }
}
